/**
\file	PidControl.cpp

\brief	Implements the adapter class.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "NervousSystem.h"
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
#include "PidControl.h"

namespace AnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/18/2011
**/
PidControl::PidControl()
{
	m_bEnabled = true;
	m_bInitEnabled = m_bEnabled;
	m_fltEnabled = 0;
}

PidControl::PidControl(float fltSetpoint, float fltGain, float fltIntegralAct, float fltDerivativeAct, 
    bool bComplexError, bool bAntiResetWindup, bool bRampLimit, 
    float fltRangeMax, float fltRangeMin, float fltARWBound, float fltRampGradient) : 
CStdPID(fltSetpoint, fltGain, fltIntegralAct, fltDerivativeAct, bComplexError, bAntiResetWindup, 
        bRampLimit, fltRangeMax, fltRangeMin, fltARWBound, fltRampGradient)
{
}


/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
PidControl::~PidControl()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PidControl\r\n", "", -1, false, true);}
}

/**
\brief	Tells whether this node is enabled.

\details Some types of nodes can be enabled/disabled. For example, joints or muscles. This tells
what enabled state the node is in. This will not apply to every node object type. 

\author	dcofer
\date	2/24/2011

\return	true if it enabled, false if not. 
**/
bool PidControl::Enabled() {return m_bEnabled;}

/**
\brief	Enables the node.

\details Some types of nodes can be enabled/disabled. This sets the enabled state of the object. 

\author	dcofer
\date	2/24/2011

\param	bValue	true to enable. 
**/
void PidControl::Enabled(bool bValue) 
{
	m_bEnabled = bValue;
	m_fltEnabled = (float) m_bEnabled;

	//If the sim is running then we do not set the history flag. Only set it if changed while the sim is not running.
	if(!m_lpSim->SimRunning())
		m_bInitEnabled = m_bEnabled;
}

float *PidControl::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "ENABLE")
		return &m_fltEnabled;

	return AnimatBase::GetDataPointer(strDataType);
}

bool PidControl::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}

	if(strType == "KP")
	{
		Gain((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "KI")
	{
		IntegralAct((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "KD")
	{
		DerivativeAct((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "COMPLEXERROR")
	{
		ComplexError(Std_ToBool(strValue));
		return true;
	}

	if(strType == "ANTIRESETWINDUP")
	{
		AntiResetWindup(Std_ToBool(strValue));
		return true;
	}

	if(strType == "RAMPLIMIT")
	{
		RampLimit(Std_ToBool(strValue));
		return true;
	}

	if(strType == "RANGEMAX")
	{
		RangeMax((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "RANGEMIN")
	{
		RangeMin((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "ARWBOUND")
	{
		ARWBound((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "RAMPGRADIENT")
	{
		RampGradient((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Data Type", strDataType);

	return false;
}

void PidControl::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Enable", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Enabled", AnimatPropertyType::Boolean, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("Kp", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Ki", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Kd", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("ComplexError", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AntiResetWindup", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RampLimit", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RangeMax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RangeMin", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("ARWBound", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RampGradient", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

void PidControl::ResetSimulation()
{
    ResetVars();
}

void PidControl::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into PidControl Element

	//Load Source Data
	Setpoint(oXml.GetChildFloat("Setpoint", 0));
	Gain(oXml.GetChildFloat("Kp", m_fltGain)); 
	IntegralAct(oXml.GetChildFloat("Ki", m_fltIntegralAct)); 
	DerivativeAct(oXml.GetChildFloat("Kd", m_fltDerivativeAct));
	ComplexError(oXml.GetChildBool("ComplexError", m_bComplexError));
	AntiResetWindup(oXml.GetChildBool("AntiResetWindup", m_bAntiResetWindup));
	RampLimit(oXml.GetChildBool("RampLimit", m_bRampLimit));
	RangeMax(oXml.GetChildFloat("RangeMax", m_fltRangeMax));
	RangeMin(oXml.GetChildFloat("RangeMin", m_fltRangeMin));
	ARWBound(oXml.GetChildFloat("ARWBound", m_fltARWBound));
	RampGradient(oXml.GetChildFloat("RampGradient", m_fltRampGradient));

	Enabled(oXml.GetChildBool("Enabled", m_bEnabled));

	oXml.OutOfElem(); //OutOf PidControl Element
}

}			//AnimatSim
