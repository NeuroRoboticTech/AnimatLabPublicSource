/**
\file	MuscleBase.cpp

\brief	Implements the muscle base class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <math.h>
#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
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
#include "Simulator.h"

#include "ExternalStimulus.h"

#include "LineBase.h"
#include "MuscleBase.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor. 

\author	dcofer
\date	3/10/2011
**/
MuscleBase::MuscleBase()
{
	m_fltMaxTension = 0;
	m_fltVm = (float) -0.15;
	m_fltTdot = 0;
	m_fltTension = 0;
	m_fltPrevTension = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
MuscleBase::~MuscleBase()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of MuscleBase\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the tension of the muscle. 

\author	dcofer
\date	3/10/2011

\return	Tension. 
**/
float MuscleBase::Tension() {return m_fltTension;}

/**
\brief	Sets the tension of the muscle. 

\author	dcofer
\date	3/10/2011

\param	fltVal	The new value. 
\exception Tension cannot be less than zero.
**/
void MuscleBase::Tension(float fltVal)
{
	if(fltVal < 0)
		THROW_PARAM_ERROR(Al_Err_lForceLessThanZero, Al_Err_strForceLessThanZero, "MuscleID", m_strName);

	m_fltTension = fltVal;
}

/**
\brief	Gets the maximum tension. 

\author	dcofer
\date	3/10/2011

\return	Maximum tension allowed. 
**/
float MuscleBase::MaxTension() {return m_fltMaxTension;}

/**
\brief	Sets the Maximum tension. 

\author	dcofer
\date	3/10/2011

\param	fltVal	The new value. 
\exception Max tension must be greater than zero.
**/
void MuscleBase::MaxTension(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Max Tension");
	m_fltMaxTension = fltVal;
}

/**
\brief	Gets the total stimulation applied to the muscle. 

\author	dcofer
\date	3/10/2011

\return	Total Stim. 
**/
float MuscleBase::Vm() {return m_fltVm;}

/**
\brief	Gets the derivative of the tension. 

\author	dcofer
\date	3/10/2011

\return	Derivated of tension. 
**/
float MuscleBase::Tdot() {return m_fltTdot;}

/**
\brief	Gets the previous tension. 

\author	dcofer
\date	3/10/2011

\return	Previous tension. 
**/
float MuscleBase::PrevTension() {return m_fltPrevTension;}

/**
\brief	Gets whether the muscle is enabled. 

\author	dcofer
\date	3/10/2011

\return	true if it enabled, false otherwise. 
**/
BOOL MuscleBase::Enabled() {return m_bEnabled;};

/**
\brief	Sets whether this muscle is Enabled. 

\author	dcofer
\date	3/10/2011

\param	bVal	true to enable. 
**/
void MuscleBase::Enabled(BOOL bVal)
{
	m_bEnabled = bVal;
	m_fltEnabled = (float) bVal;

	if(!bVal)
	{
		m_fltTdot = 0;
		m_fltTension = 0;
	}
}

void MuscleBase::AddExternalNodeInput(float fltInput)
{
	//We are changing this. It is now really driven by the membrane voltage of the non-spiking neuron. Integration from 
	//different motor neurons takes place in the non-spiking neuron and we get that here instead of frequency and use that
	//to calculate the max isometric tension from the stim-tension curve.
	m_fltVm=fltInput;
}

#pragma region DataAccesMethods

float *MuscleBase::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	float *lpData = LineBase::GetDataPointer(strDataType);
	if(lpData) return lpData;

	if(strType == "TENSION")
		lpData = &m_fltTension;
	else if(strType == "TDOT")
		lpData = &m_fltTdot;
	else if(strType == "MEMBRANEVOLTAGE")
		lpData = &m_fltVm;

	return lpData;
}

BOOL MuscleBase::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(strDataType == "POSITION")
	{
		//Position(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void MuscleBase::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	LineBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_fltMaxTension = oXml.GetChildFloat("MaximumTension", m_fltMaxTension);


	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
