/**
\file	Box.cpp

\brief	Implements the box class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Box.h"
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
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/4/2011
**/
Box::Box()
{
	m_fltLength = 1;
	m_fltWidth = 1;
	m_fltHeight = 1;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/4/2011
**/
Box::~Box()
{
}

float Box::Length() {return m_fltLength;}

void Box::Length(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BoxSize.Length");
	if(bUseScaling)
		m_fltLength = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLength = fltVal;

	Resize();
}

float Box::Width() {return m_fltWidth;}

void Box::Width(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BoxSize.Width");
	if(bUseScaling)
		m_fltWidth = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltWidth = fltVal;

	Resize();
}

float Box::Height() {return m_fltHeight;}

void Box::Height(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BoxSize.Height");
	if(bUseScaling)
		m_fltHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltHeight = fltVal;

	Resize();
}

BOOL Box::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "LENGTH")
	{
		Length(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "WIDTH")
	{
		Width(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "HEIGHT")
	{
		Height(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Box::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	Length(oXml.GetChildFloat("Length", m_fltLength));
	Width(oXml.GetChildFloat("Width", m_fltWidth));
	Height(oXml.GetChildFloat("Height", m_fltHeight));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
