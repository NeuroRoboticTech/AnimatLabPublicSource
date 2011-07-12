/**
\file	LinearHillStretchReceptor.cpp

\brief	Implements the linear hill stretch receptor class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sensor.h"
#include "Attachment.h"
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

#include "ExternalStimulus.h"

#include "LineBase.h"
#include "Gain.h"
#include "SigmoidGain.h"
#include "LengthTensionGain.h"
#include "MuscleBase.h" 
#include "LinearHillMuscle.h"
#include "LinearHillStretchReceptor.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor.

\author	dcofer
\date	5/23/2011
**/
LinearHillStretchReceptor::LinearHillStretchReceptor()
{
	m_bApplyTension = FALSE;
	m_fltIaDischargeConstant = 100;
	m_fltIIDischargeConstant = 100;
	m_fltIaRate = 0;
	m_fltIIRate = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	5/23/2011
**/
LinearHillStretchReceptor::~LinearHillStretchReceptor()
{
}

/**
\brief	Gets wheter tension is applied by the receptor or not.

\author	dcofer
\date	5/23/2011

\return	true to apply tension, false to not.
**/
BOOL LinearHillStretchReceptor::ApplyTension() {return m_bApplyTension;}

/**
\brief	Sets wheter tension is applied by the receptor or not.

\author	dcofer
\date	5/23/2011

\param	bVal	true to apply tension.
**/
void LinearHillStretchReceptor::ApplyTension(BOOL bVal) {m_bApplyTension = bVal;}

/**
\brief	Gets the ia discharge constant.

\author	dcofer
\date	5/23/2011

\return	discharge constant.
**/
float LinearHillStretchReceptor::IaDischargeConstant() {return m_fltIaDischargeConstant;}

/**
\brief	Sets the ia discharge constant.

\author	dcofer
\date	5/23/2011

\param	fltVal	The new value.
**/
void LinearHillStretchReceptor::IaDischargeConstant(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1e11, fltVal, TRUE, "IaDischargeConstant");
	m_fltIaDischargeConstant = fltVal;
}

/**
\brief	Gets the ii discharge constant.

\author	dcofer
\date	5/23/2011

\return	discharge constant.
**/
float LinearHillStretchReceptor::IIDischargeConstant() {return m_fltIIDischargeConstant;}

/**
\brief	Sets the ii discharge constant.

\author	dcofer
\date	5/23/2011

\param	fltVal	The new value.
**/
void LinearHillStretchReceptor::IIDischargeConstant(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1e11, fltVal, TRUE, "IIDischargeConstant");
	m_fltIIDischargeConstant = fltVal;
}

/**
\brief	Gets the ia rate.

\author	dcofer
\date	5/23/2011

\return	rate.
**/
float LinearHillStretchReceptor::IaRate() {return m_fltIaRate;}

/**
\brief	Gets the ii rate.

\author	dcofer
\date	5/23/2011

\return	rate.
**/
float LinearHillStretchReceptor::IIRate() {return m_fltIIRate;}

void LinearHillStretchReceptor::CalculateTension()
{
	LinearHillMuscle::CalculateTension();

	m_fltIaRate = m_fltIaDischargeConstant*m_fltSeLength;
	m_fltIIRate = m_fltIIDischargeConstant*m_fltPeLength;
}

float *LinearHillStretchReceptor::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	float *lpData = LinearHillMuscle::GetDataPointer(strDataType);
	if(lpData) return lpData;

	if(strType == "IA")
		lpData = &m_fltIaRate;
	else if(strType == "IB")
		lpData = &m_fltIbRate;
	else if(strType == "II")
		lpData = &m_fltIIRate;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
}

BOOL LinearHillStretchReceptor::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(LinearHillMuscle::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "APPLYTENSION")
	{
		ApplyTension(Std_ToBool(strValue));
		return true;
	}

	if(strDataType == "IADISCHARGE")
	{
		IaDischargeConstant(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "IIDISCHARGE")
	{
		IIDischargeConstant(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void LinearHillStretchReceptor::Load(CStdXml &oXml)
{
	LinearHillMuscle::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	ApplyTension(oXml.GetChildBool("ApplyTension", m_bApplyTension));
	IaDischargeConstant(oXml.GetChildFloat("IaDischarge", m_fltIaDischargeConstant));
	IIDischargeConstant(oXml.GetChildFloat("IIDischarge", m_fltIIDischargeConstant));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
