#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "MotorizedJoint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
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
#include "Simulator.h"


namespace AnimatSim
{
	namespace Environment
	{

MotorizedJoint::MotorizedJoint(void)
{
	m_lpPhysicsMotorJoint = NULL;
	m_fltSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltMaxVelocity = 100;
	m_fltPrevVelocity = -1000000;
	m_bEnableMotor = FALSE;
	m_bEnableMotorInit = FALSE;
	m_fltMaxForce = 1000;
	m_bServoMotor = FALSE;
	m_ftlServoGain = 100;
	m_lpPhysicsMotorJoint = NULL;
}

MotorizedJoint::~MotorizedJoint(void)
{
}

/**
\brief	Gets the physics body interface pointer. This is an interface reference to the Vs version
of this object. It will allow us to call methods directly in the Vs (OSG) version of the object
directly without having to overload a bunch of methods in each joint.. 

\author	dcofer
\date	3/2/2011

\return	Pointer to Vs interface, NULL else. 
**/
IMotorizedJoint *MotorizedJoint::PhysicsMotorJoint() {return m_lpPhysicsMotorJoint;}

/**
\brief	Sets the physics motorized joint interface pointer. This is an interface reference to the Vs version
of this object. It will allow us to call methods directly in the Vs (OSG) version of the object
directly without having to overload a bunch of methods in each joint.. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpBody	The pointer to the phsyics body interface. 
**/
void MotorizedJoint::PhysicsMotorJoint(IMotorizedJoint *lpJoint) 
{
	m_lpPhysicsMotorJoint = lpJoint;
}

/**
\brief	Tells if the motor is enabled.

\author	dcofer
\date	3/22/2011

\return	true if it is enabled, false otherwise.
**/
BOOL MotorizedJoint::EnableMotor() {return m_bEnableMotor;}

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
void MotorizedJoint::EnableMotor(BOOL bVal)
{
	if(m_lpPhysicsMotorJoint)
		m_lpPhysicsMotorJoint->Physics_EnableMotor(bVal, m_fltDesiredVelocity, m_fltMaxForce);
	m_bEnableMotor = bVal;
}


/**
\brief	Sets whether this is a servo motor or not.

\details A servo motor is one where the position of the joint is specified instead of the velocity that is normally
used to control the motor.

\author	dcofer
\date	3/24/2011

\param	bServo	true to set to be a servo motor. 
**/
void MotorizedJoint::ServoMotor(BOOL bServo) {m_bServoMotor = bServo;}

/**
\brief	Gets whether this is set to be a servo motor.

\author	dcofer
\date	3/24/2011

\return	true if it is a servo motor, false otherwise.
**/
BOOL MotorizedJoint::ServoMotor() {return m_bServoMotor;}

/**
\brief	Sets the servo gain used to calculate the new velocity for maintaining a position with a servo motor.

\author	dcofer
\date	3/24/2011

\param	fltVal	The new gain value. 
**/
void MotorizedJoint::ServoGain(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "ServoGain", TRUE);
	m_ftlServoGain= fltVal;
}

/**
\brief	Gets the servo gain.

\author	dcofer
\date	3/24/2011

\return	Servo gain.
**/
float MotorizedJoint::ServoGain() {return m_ftlServoGain;}

/**
\brief	Sets the Maximum torque.

\author	dcofer
\date	3/24/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MotorizedJoint::MaxForce(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "MaxTorque");

	if(bUseScaling)
	{
		//If it uses radians then this is really a torque and not a force, so we have to scale appropriately.
		if(this->UsesRadians())
			fltVal *= m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits() * m_lpSim->InverseDistanceUnits();
		else
			fltVal *=  m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits();
	}

	m_fltMaxForce = fltVal;

	//If max torque is over 1000 N then assume we mean infinity.
	if(m_fltMaxForce >= 1000)
		m_fltMaxForce = 1e35f;

	if(m_lpPhysicsMotorJoint)
		m_lpPhysicsMotorJoint->Physics_MaxForce(m_fltMaxForce);
}

/**
\brief	Gets the maximum torque.

\author	dcofer
\date	3/24/2011

\return	Maximum torque the motor can apply.
**/
float MotorizedJoint::MaxForce() {return m_fltMaxForce;}

/**
\brief	Gets the maximum velocity.

\author	dcofer
\date	3/22/2011

\return	Maximum motor velocity that is allowed.
**/
float MotorizedJoint::MaxVelocity() {return m_fltMaxVelocity;};

/**
\brief	Sets the maximum velocity allowed by the motorized joint.

\author	dcofer
\date	3/22/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MotorizedJoint::MaxVelocity(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Joint.MaxVelocity");

	if(bUseScaling && !UsesRadians())
		m_fltMaxVelocity = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMaxVelocity = fltVal;
}

/**
\brief	Gets the velocity that is actually set using the physics method.

\author	dcofer
\date	3/22/2011

\return	The velocity that was set.
**/
float MotorizedJoint::SetVelocity() {return m_fltSetVelocity;}

/**
\brief	Sets the velocity that is actually set using the physics method.

\author	dcofer
\date	4/1/2011

\param	fltVal	The new value. 
**/
void MotorizedJoint::SetVelocity(float fltVal) {m_fltSetVelocity = fltVal;}

/**
\brief	Gets the desired velocity.

\author	dcofer
\date	3/22/2011

\return	Desired velocity.
**/
float MotorizedJoint::DesiredVelocity() {return m_fltDesiredVelocity;}

/**
\brief	Sets the desired velocity.

\author	dcofer
\date	3/22/2011

\param	fltVelocity	The new velocity. 
**/
void MotorizedJoint::DesiredVelocity(float fltVelocity) {m_fltDesiredVelocity = fltVelocity;}

/**
\brief	Sets the desired velocity.

\author	dcofer
\date	3/22/2011

\param	fltInput	The new velocity. 
**/
void MotorizedJoint::MotorInput(float fltInput) {m_fltDesiredVelocity = fltInput;}

/**
\brief	Sets the previous velocity that is actually used by the physics method.

\author	dcofer
\date	3/22/2011

\return	The previous velocity that was set.
**/
float MotorizedJoint::PrevVelocity() {return m_fltPrevVelocity;}

/**
\brief	Sets the previous velocity that is actually used by the physics method.

\author	dcofer
\date	4/1/2011

\param	fltVal	The previous value. 
**/
void MotorizedJoint::PrevVelocity(float fltVal) {m_fltPrevVelocity = fltVal;}

/**
\brief	Sets the desired velocity to use for the motor.

\author	dcofer
\date	4/3/2011
**/
void MotorizedJoint::SetVelocityToDesired()
{
	if(m_lpPhysicsMotorJoint)
		m_lpPhysicsMotorJoint->Physics_SetVelocityToDesired();
}

/**
\brief	Enables/disables the motor lock.

\details If you enable the motor lock then this locks the joint at the specified location.
Only a force greater than fltMaxLockForce will be able to move it from this position.

\author	dcofer
\date	4/3/2011

\param	bOn			   	true to on. 
\param	fltPosition	   	The flt position. 
\param	fltMaxLockForce	The flt maximum lock force. 
**/
void MotorizedJoint::EnableLock(BOOL bOn, float fltPosition, float fltMaxLockForce)
{
	if(m_lpPhysicsMotorJoint)
		m_lpPhysicsMotorJoint->Physics_EnableLock(bOn, fltPosition, fltMaxLockForce);
}

void MotorizedJoint::Load(CStdXml &oXml)
{
	Joint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	EnableMotor(oXml.GetChildBool("EnableMotor", m_bEnableMotor));
	MaxVelocity(oXml.GetChildFloat("MaxVelocity", m_fltMaxVelocity));

	MaxForce(oXml.GetChildFloat("MaxForce", m_fltMaxForce));
	ServoMotor(oXml.GetChildBool("ServoMotor", m_bServoMotor));
	ServoGain(oXml.GetChildFloat("ServoGain", m_ftlServoGain));

	oXml.OutOfElem(); //OutOf Joint Element
}

	}
}