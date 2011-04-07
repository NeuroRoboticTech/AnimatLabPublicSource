/**
\file	Prismatic.cpp

\brief	Implements the prismatic class.
**/

#include "stdafx.h"
#include "IMotorizedJoint.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "MotorizedJoint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "ConstraintLimit.h"
#include "Prismatic.h"
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
		namespace Joints
		{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
Prismatic::Prismatic()
{
	m_lpUpperLimit = NULL;
	m_lpLowerLimit = NULL;
	m_lpPosFlap = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
Prismatic::~Prismatic()
{
try
{
	if(m_lpUpperLimit)
	{
		delete m_lpUpperLimit;
		m_lpUpperLimit = NULL;
	}

	if(m_lpLowerLimit)
	{
		delete m_lpLowerLimit;
		m_lpLowerLimit = NULL;
	}

	if(m_lpPosFlap)
	{
		delete m_lpPosFlap;
		m_lpPosFlap = NULL;
	}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Prismatic\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the radius cylinder of the cylinder used to display the
axis of the prismatic joint in the environment.

\author	dcofer
\date	3/24/2011

\return	Radius of axis cylinder.
**/
float Prismatic::CylinderRadius() 
{
	return m_fltSize * 0.25f;
};

/**
\brief	Gets the height of the cylinder used to display the 
axis of the prismatic joint in the environment. If limits are 
enabled then this is the range between the two limits. If not then
it is defaulted to proportion of the size variable.

\author	dcofer
\date	3/24/2011

\return	Height of hinge cylidner.
**/
float Prismatic::CylinderHeight() 
{
	if(m_bEnableLimits)
		return GetLimitRange();
	else
		return m_fltSize*10;
};

/**
\brief	Gets the width of the flaps used to display the hinge in the environment.

\author	dcofer
\date	3/24/2011

\return	Flap width.
**/
float Prismatic::BoxSize() 
{
	return m_fltSize * 0.5f;
};

void Prismatic::Enabled(BOOL bValue) 
{
	EnableMotor(bValue);
	m_bEnabled = bValue;
}

/**
\brief	Gets a pointer to the upper limit ConstraintLimit.

\author	dcofer
\date	3/24/2011

\return	Pointer to ConstraintLimit.
**/
ConstraintLimit *Prismatic::UpperLimit() {return m_lpUpperLimit;}

/**
\brief	Gets a pointer to the lower limit ConstraintLimit.

\author	dcofer
\date	3/24/2011

\return	Pointer to ConstraintLimit.
**/
ConstraintLimit *Prismatic::LowerLimit() {return m_lpLowerLimit;}

float Prismatic::GetPositionWithinLimits(float fltPos)
{
	if(m_bEnableLimits)
	{
		if(fltPos>m_lpUpperLimit->LimitPos())
			fltPos = m_lpUpperLimit->LimitPos();
		if(fltPos<m_lpLowerLimit->LimitPos())
			fltPos = m_lpLowerLimit->LimitPos();
	}

	return fltPos;
}

float Prismatic::GetLimitRange()
{
	if(m_bEnableLimits)
		return (m_lpUpperLimit->LimitPos()-m_lpLowerLimit->LimitPos());
	else
		return -1;
}

void Prismatic::ResetSimulation()
{
	Joint::ResetSimulation();

	m_fltSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltPrevVelocity = 0;

	EnableMotor(m_bEnableMotorInit);
}

BOOL Prismatic::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Joint::SetData(strType, strValue, FALSE))
		return TRUE;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Prismatic::AddExternalNodeInput(float fltInput)
{
	m_fltDesiredVelocity += fltInput;
}

void Prismatic::Load(CStdXml &oXml)
{
	MotorizedJoint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	m_lpUpperLimit->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
	m_lpLowerLimit->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
	m_lpPosFlap->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, JointPosition());

	m_lpUpperLimit->Load(oXml, "UpperLimit");
	m_lpLowerLimit->Load(oXml, "LowerLimit");

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
