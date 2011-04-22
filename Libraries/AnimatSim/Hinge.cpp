/**
\file	Hinge.cpp

\brief	Implements the hinge class.
**/

#include "stdafx.h"
#include "IMotorizedJoint.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBase.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "MotorizedJoint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "ConstraintLimit.h"
#include "Hinge.h"
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
Hinge::Hinge()
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
Hinge::~Hinge()
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
{Std_TraceMsg(0, "Caught Error in desctructor of Hinge\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the radius cylinder of the cylinder used to display the hinge in the environment.

\author	dcofer
\date	3/24/2011

\return	Radius of hinge cylinder.
**/
float Hinge::CylinderRadius() 
{
	return m_fltSize * 0.25f;
};

/**
\brief	Gets the height of the cylinder used to display the hinge in the environment.

\author	dcofer
\date	3/24/2011

\return	Height of hinge cylidner.
**/
float Hinge::CylinderHeight() 
{
	return m_fltSize;
};

/**
\brief	Gets the width of the flaps used to display the hinge in the environment.

\author	dcofer
\date	3/24/2011

\return	Flap width.
**/
float Hinge::FlapWidth() 
{
	return m_fltSize * 0.05f;
};

void Hinge::Enabled(BOOL bValue) 
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
ConstraintLimit *Hinge::UpperLimit() {return m_lpUpperLimit;}

/**
\brief	Gets a pointer to the lower limit ConstraintLimit.

\author	dcofer
\date	3/24/2011

\return	Pointer to ConstraintLimit.
**/
ConstraintLimit *Hinge::LowerLimit() {return m_lpLowerLimit;}

float Hinge::GetPositionWithinLimits(float fltPos)
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

float Hinge::GetLimitRange()
{
	if(m_bEnableLimits)
		return (m_lpUpperLimit->LimitPos()-m_lpLowerLimit->LimitPos());
	else
		return -1;
}

void Hinge::ResetSimulation()
{
	Joint::ResetSimulation();

	m_fltSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltPrevVelocity = 0;

	EnableMotor(m_bEnableMotorInit);
}

BOOL Hinge::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Joint::SetData(strType, strValue, FALSE))
		return TRUE;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Hinge::AddExternalNodeInput(float fltInput)
{
	m_fltDesiredVelocity += fltInput;
}

void Hinge::Load(CStdXml &oXml)
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
