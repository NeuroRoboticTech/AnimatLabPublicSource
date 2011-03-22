/**
\file	Joint.cpp

\brief	Implements the joint class.
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
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

/**
\brief	Default constructor.

\author	dcofer
\date	3/22/2011
**/
Joint::Joint()
{
	m_lpChild = NULL;
	m_fltSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltMaxVelocity = 100;
	m_fltPrevVelocity = -1000000;
	m_bEnableMotor = FALSE;
	m_bEnableMotorInit = FALSE;
	m_fltPosition = 0;
	m_fltVelocity = 0;
	m_fltForce = 0;
	m_fltSize = 0.02f;
	m_bEnableLimits = TRUE;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
Joint::~Joint()
{
	//We also do not delete our references to these objects.
	m_lpParent = NULL;
	m_lpChild = NULL;
}
			
/**
\brief	Tells whether this joint uses radians or meters for its measurements.

\details This is defaulted to TRUE. You must override this and set it to the appropriate
value for your derived classes.

\author	dcofer
\date	3/22/2011

\return	true if it uses radians, false if it uses meters.
**/
BOOL Joint::UsesRadians() {return TRUE;}

/**
\brief	Gets the size of the graphical representation of this joint.

\author	dcofer
\date	3/22/2011

\return	Size for the graphics object.
**/
float Joint::Size() {return m_fltSize;};

/**
\brief	Sets the size of the graphical representation of this joint.

\author	dcofer
\date	3/22/2011

\param	fltVal	   	The new size value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Joint::Size(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Joint.Size");
	if(bUseScaling)
		m_fltSize = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSize = fltVal;

	Resize();
}

/**
\brief	Tells if the motor is enabled.

\author	dcofer
\date	3/22/2011

\return	true if it is enabled, false otherwise.
**/
BOOL Joint::EnableMotor() {return m_bEnableMotor;};

/**
\brief	Enables\disables the motor.

\details If this is a motorized joint then when you turn it on the
physics engine will calculate the torque that needs to be
applied to this joint in order for it to have the desired
Velocity for its current load. 

\author	dcofer
\date	3/22/2011

\param	bVal	true to enable. 
**/
void Joint::EnableMotor(BOOL bVal)
{
	m_bEnableMotor = bVal;
	//TODO Add sim code here.
}

/**
\brief	Gets the maximum velocity.

\author	dcofer
\date	3/22/2011

\return	Maximum motor velocity that is allowed.
**/
float Joint::MaxVelocity() {return m_fltMaxVelocity;};

/**
\brief	Sets the maximum velocity allowed by the motorized joint.

\author	dcofer
\date	3/22/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Joint::MaxVelocity(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Joint.MaxVelocity");

	if(bUseScaling && !UsesRadians())
		m_fltMaxVelocity = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMaxVelocity = fltVal;

	//TODO Add sim code here.
}

/**
\brief	Tells if ConstraintLimits are enabled.

\author	dcofer
\date	3/22/2011

\return	true if it limits are enabled, false otherwise.
**/
BOOL Joint::EnableLimits() {return m_bEnableLimits;};

/**
\brief	Sets whether ContrainLimits are enabled or not.

\author	dcofer
\date	3/22/2011

\param	bVal	true to enable. 
**/
void Joint::EnableLimits(BOOL bVal) {m_bEnableLimits = bVal;}

int Joint::VisualSelectionType() {return JOINT_SELECTION_MODE;}

/**
\brief	Gets the child RigidBody part for this joint.

\author	dcofer
\date	3/22/2011

\return	Pointer to the child RigidBody for this joint.
**/
RigidBody *Joint::Child() {return m_lpChild;}

/**
\brief	Sets the Child RigidBody part for this joint.

\author	dcofer
\date	3/22/2011

\param [in,out]	lpValue	IPointer to the child part. 
**/
void Joint::Child(RigidBody *lpValue) {m_lpChild = lpValue;}

/**
\brief	Gets the joint position.

\author	dcofer
\date	3/22/2011

\return	Joint position.
**/
float Joint::JointPosition() {return m_fltPosition;}

/**
\brief	Sets the joint position.

\author	dcofer
\date	3/22/2011

\param	fltPos	The new position. 
**/
void Joint::JointPosition(float fltPos) {m_fltPosition = fltPos;}

/**
\brief	Gets the joint velocity.

\author	dcofer
\date	3/22/2011

\return	Joint Velocity.
**/
float Joint::JointVelocity() {return m_fltVelocity;}

/**
\brief	Sets the joint velocity.

\author	dcofer
\date	3/22/2011

\param	fltVel	The new velocity. 
**/
void Joint::JointVelocity(float fltVel) {m_fltVelocity = fltVel;}

/**
\brief	Gets the joint force.

\author	dcofer
\date	3/22/2011

\return	Joint force.
**/
float Joint::JointForce() {return m_fltForce;}

/**
\brief	Sets the joint force.

\author	dcofer
\date	3/22/2011

\param	fltForce	The new force. 
**/
void Joint::JointForce(float fltForce) {m_fltForce = fltForce;}

/**
\brief	The velocity that is actually set using the physics method.

\author	dcofer
\date	3/22/2011

\return	The velocity that was set.
**/
float Joint::SetVelocity() {return m_fltSetVelocity;}

/**
\brief	Gets the desired velocity.

\author	dcofer
\date	3/22/2011

\return	Desired velocity.
**/
float Joint::DesiredVelocity() {return m_fltDesiredVelocity;}

/**
\brief	Sets the desired velocity.

\author	dcofer
\date	3/22/2011

\param	fltVelocity	The new velocity. 
**/
void Joint::DesiredVelocity(float fltVelocity) {m_fltDesiredVelocity = fltVelocity;}

/**
\brief	Sets the desired velocity.

\author	dcofer
\date	3/22/2011

\param	fltInput	The new velocity. 
**/
void Joint::MotorInput(float fltInput) {m_fltDesiredVelocity = fltInput;}

void Joint::AddExternalNodeInput(float fltInput)
{
	m_fltDesiredVelocity += fltInput;
}

/**
\brief	Sets the Set Velocity to the desired velocity.

\author	dcofer
\date	3/22/2011
**/
void Joint::SetVelocityToDesired()
{
	m_fltSetVelocity = m_fltDesiredVelocity;
	m_fltDesiredVelocity = 0;
}

/**
\brief	Creates the joint.

\details This method is called by the derived class in the physics library. It makes the calls necessary to create the actual joint
using the chosen phsyics API.

\author	dcofer
\date	3/22/2011
**/
void Joint::CreateJoint()
{}

void Joint::StepSimulation()
{
	UpdateData();
}


float *Joint::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	float *lpData = BodyPart::GetDataPointer(strDataType);
	if(lpData)
		return lpData;

	if(strType == "ENABLE")
		return &m_fltEnabled;

	if(m_lpPhysicsBody)
	{
		float *lpData = NULL;
		lpData = m_lpPhysicsBody->Physics_GetDataPointer(strDataType);
		if(lpData) return lpData;
	}

	return NULL;
}

BOOL Joint::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(BodyPart::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "ENABLELIMITS")
	{
		EnableLimits(Std_ToBool(strValue));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Joint::ResetSimulation()
{
	m_fltSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltPrevVelocity = 0;

	EnableMotor(m_bEnableMotorInit);

	m_fltPosition = 0;
	m_fltVelocity = 0;
	m_fltForce = 0;
}

void Joint::Load(CStdXml &oXml)
{
	BodyPart::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	if(!m_lpParent)
		THROW_PARAM_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined, "JointID", m_strName);

	if(!m_lpChild)
		THROW_PARAM_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined, "JointID", m_strName);

	//Reset the absolute position differently for a joint. It is derived from the child object, not the parent.
	m_oAbsPosition = m_lpChild->AbsolutePosition() + m_oLocalPosition;
	m_oReportLocalPosition = m_oLocalPosition * m_lpSim->DistanceUnits();
	m_oReportWorldPosition = m_oAbsPosition * m_lpSim->DistanceUnits();

	EnableMotor(oXml.GetChildBool("EnableMotor", m_bEnableMotor));
	MaxVelocity(oXml.GetChildFloat("MaxVelocity", m_fltMaxVelocity));
	EnableLimits(oXml.GetChildBool("EnableLimits", m_bEnableLimits));

	Size(oXml.GetChildFloat("Size", m_fltSize));

	oXml.OutOfElem(); //OutOf Joint Element
}

	}			//Environment
}				//AnimatSim
