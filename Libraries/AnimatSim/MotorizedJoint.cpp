#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
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
#include "Light.h"
#include "LightManager.h"
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
	m_fltReportSetVelocity = 0;
	m_fltMaxVelocity = 100;
	m_fltPrevVelocity = -1000000;
	m_bEnableMotor = false;
	m_bEnableMotorInit = false;
	m_fltMaxForce = 1000;
    m_fltMaxForceNotScaled = 10;
	m_bServoMotor = false;
	m_ftlServoGain = 100;
	m_lpPhysicsMotorJoint = NULL;
    //m_lpAssistPid = NULL;
    m_iAssistCountdown = 3;
    m_lpAssistPid = new CStdPID(0, 10, 0.2f, 10, true, false, false, 0, 0, 0, 70);
    ClearAssistForces();
}

MotorizedJoint::~MotorizedJoint(void)
{
	//ConstraintLimits are deleted in the base objects.
	try
	{

        if(m_lpAssistPid)
        {
            delete m_lpAssistPid;
            m_lpAssistPid = NULL;
        }
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlMotorizMotorizedJointedJoint\r\n", "", -1, false, true);}
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
bool MotorizedJoint::EnableMotor() {return m_bEnableMotor;}

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
void MotorizedJoint::EnableMotor(bool bVal)
{
	if(m_lpPhysicsMotorJoint)
		m_lpPhysicsMotorJoint->Physics_EnableMotor(bVal, m_fltDesiredVelocity, m_fltMaxForce, true);
	m_bEnableMotor = bVal;

	//If the sim is running then we do not set the history flag. Only set it if changed while the sim is not running.
	if(!m_lpSim->SimRunning())
		m_bEnableMotorInit = m_bEnableMotor;
}


/**
\brief	Sets whether this is a servo motor or not.

\details A servo motor is one where the position of the joint is specified instead of the velocity that is normally
used to control the motor.

\author	dcofer
\date	3/24/2011

\param	bServo	true to set to be a servo motor. 
**/
void MotorizedJoint::ServoMotor(bool bServo) {m_bServoMotor = bServo;}

/**
\brief	Gets whether this is set to be a servo motor.

\author	dcofer
\date	3/24/2011

\return	true if it is a servo motor, false otherwise.
**/
bool MotorizedJoint::ServoMotor() {return m_bServoMotor;}

/**
\brief	Sets the servo gain used to calculate the new velocity for maintaining a position with a servo motor.

\author	dcofer
\date	3/24/2011

\param	fltVal	The new gain value. 
**/
void MotorizedJoint::ServoGain(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "ServoGain", true);
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
void MotorizedJoint::MaxForce(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "MaxTorque");

	//If max torque is over 1000 N then assume we mean infinity.
	if(fltVal >= 5000)
		fltVal = 1e35f;
	else
	{
        m_fltMaxForceNotScaled = fltVal;

		if(bUseScaling)
		{
			//If it uses radians then this is really a torque and not a force, so we have to scale appropriately.
			if(this->UsesRadians())
				fltVal *= m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits() * m_lpSim->InverseDistanceUnits();
			else
				fltVal *=  m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits();
		}
	}

	m_fltMaxForce = fltVal;
	
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
\brief	Gets the maximum force/torque a motor can apply. This is the unscaled value. 

\author	dcofer
\date	3/24/2011

\return	unscaled Maximum torque the motor can apply.
**/
float MotorizedJoint::MaxForceNotScaled() {return m_fltMaxForceNotScaled;}

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
void MotorizedJoint::MaxVelocity(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Joint.MaxVelocity");

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
void MotorizedJoint::SetVelocity(float fltVal) 
{
	m_fltSetVelocity = fltVal;

	if(!UsesRadians())
		m_fltReportSetVelocity = m_fltSetVelocity * m_lpSim->DistanceUnits();
	else
		m_fltReportSetVelocity = m_fltSetVelocity;
}

/**
\brief	Gets the desired velocity.

\author	dcofer
\date	3/22/2011

\return	Desired velocity.
**/
float MotorizedJoint::DesiredVelocity() 
{
    float fltDesiredVel = m_fltDesiredVelocity;

	if(fltDesiredVel>m_fltMaxVelocity)
		fltDesiredVel = m_fltMaxVelocity;

	if(fltDesiredVel < -m_fltMaxVelocity)
		fltDesiredVel = -m_fltMaxVelocity;

    return fltDesiredVel;
}

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
 \brief Gets the assist countdown.

 \description Countdown timer till we can begin applying motor assist. Once a motor is turned on we wait for this
            many time steps before checking if its velocity matches the desired velocity. This is so the part
            has a chanct to start moving and so we do not apply additional forces if it is not required.

 \author    David Cofer
 \date  1/25/2014

 \return coundown timer value.
 */
int MotorizedJoint::AssistCountdown()
{return m_iAssistCountdown;}

/**
 \brief Sets the assist countdown.

 \author    David Cofer
 \date  1/25/2014

 \param iVal    The coundown value.
 */
void MotorizedJoint::AssistCountdown(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "Joint.AssistCountdown", true);
    m_iAssistCountdown = iVal;
}

/**
 \brief Gets the force vector that the motor is applying to body A. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \return un-scaled force vector.
 */
CStdFPoint MotorizedJoint::MotorForceToA()
{return m_vMotorForceToA;}

/**
 \brief Sets the force vector that the motor is applying to body A. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorForceToA(CStdFPoint &vVal)
{m_vMotorForceToA = vVal;}

/**
 \brief Gets the force vector that the motor assist is applying to body A. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistForceToA()
{return m_vMotorAssistForceToA;}

/**
 \brief Sets the force vector that the motor assist is applying to body A. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistForceToA(CStdFPoint &vVal)
{m_vMotorAssistForceToA = vVal;}

/**
 \brief Gets the force vector that the motor assist is applying to body A. (un-scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistForceToAReport()
{return m_vMotorAssistForceToAReport;}

/**
 \brief Sets the force vector that the motor assist is applying to body A. (un-scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistForceToAReport(CStdFPoint &vVal)
{m_vMotorAssistForceToAReport = vVal;}

/**
 \brief Gets the force vector that the motor is applying to body B. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \return un-scaled force vector.
 */
CStdFPoint MotorizedJoint::MotorForceToB()
{return m_vMotorForceToB;}

/**
 \brief Sets the force vector that the motor is applying to body B. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorForceToB(CStdFPoint &vVal)
{m_vMotorForceToB = vVal;}

/**
 \brief Gets the force vector that the motor assist is applying to body B. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistForceToB()
{return m_vMotorAssistForceToB;}

/**
 \brief Sets the force vector that the motor assist is applying to body B. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistForceToB(CStdFPoint &vVal)
{m_vMotorAssistForceToB = vVal;}

/**
 \brief Gets the force vector that the motor assist is applying to body B. (scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistForceToBReport()
{return m_vMotorAssistForceToBReport;}

/**
 \brief Sets the force vector that the motor assist is applying to body B. (un-scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistForceToBReport(CStdFPoint &vVal)
{m_vMotorAssistForceToBReport = vVal;}

/**
 \brief Gets the torque vector that the motor is applying to body A. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \return un-scaled torque vector.
 */
CStdFPoint MotorizedJoint::MotorTorqueToA()
{return m_vMotorTorqueToA;}

/**
 \brief Sets the torque vector that the motor is applying to body A. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorTorqueToA(CStdFPoint &vVal)
{m_vMotorTorqueToA = vVal;}

/**
 \brief Gets the torque vector that the motor assist is applying to body A. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistTorqueToA()
{return m_vMotorAssistTorqueToA;}

/**
 \brief Sets the torque vector that the motor assist is applying to body A. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistTorqueToA(CStdFPoint &vVal)
{m_vMotorAssistTorqueToA = vVal;}

/**
 \brief Gets the torque vector that the motor assist is applying to body A. (un-scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistTorqueToAReport()
{return m_vMotorAssistTorqueToAReport;}

/**
 \brief Sets the torque vector that the motor assist is applying to body A. (un-scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistTorqueToAReport(CStdFPoint &vVal)
{m_vMotorAssistTorqueToAReport = vVal;}

/**
 \brief Gets the torque vector that the motor is applying to body B. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \return un-scaled torque vector.
 */
CStdFPoint MotorizedJoint::MotorTorqueToB()
{return m_vMotorTorqueToB;}

/**
 \brief Sets the torque vector that the motor is applying to body B. (un-scaled units). 
        This includes any motor assist within it. 

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorTorqueToB(CStdFPoint &vVal)
{m_vMotorTorqueToB = vVal;}

/**
 \brief Gets the torque vector that the motor assist is applying to body B. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistTorqueToB()
{return m_vMotorAssistTorqueToB;}

/**
 \brief Sets the torque vector that the motor assist is applying to body B. (scaled units).

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistTorqueToB(CStdFPoint &vVal)
{m_vMotorAssistTorqueToB = vVal;}

/**
 \brief Gets the torque vector that the motor assist is applying to body B. (un-scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \return  Assist output.
 */
CStdFPoint MotorizedJoint::MotorAssistTorqueToBReport()
{return m_vMotorAssistTorqueToBReport;}

/**
 \brief Sets the torque vector that the motor assist is applying to body B. (un-scaled units).
        This is used for reporting purposes.

 \author    David Cofer
 \date  1/25/2014

 \param [in,out]    vVal    The input value.
 */
void MotorizedJoint::MotorAssistTorqueToBReport(CStdFPoint &vVal)
{m_vMotorAssistTorqueToBReport = vVal;}

/**
 \brief Gets a pointer to the motor assist pid controller.

 \author    David Cofer
 \date  1/25/2014

 \return    Pointer to PID object.
 */
CStdPID *MotorizedJoint::AssistPid()
{return m_lpAssistPid;}


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
void MotorizedJoint::EnableLock(bool bOn, float fltPosition, float fltMaxLockForce)
{
	if(m_lpPhysicsMotorJoint)
		m_lpPhysicsMotorJoint->Physics_EnableLock(bOn, fltPosition, fltMaxLockForce);
}
void MotorizedJoint::ResetSimulation()
{
	Joint::ResetSimulation();

	m_fltSetVelocity = 0;
	m_fltReportSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltPrevVelocity = 0;

	EnableMotor(m_bEnableMotorInit);

    ClearAssistForces();

    if(m_lpAssistPid)
        m_lpAssistPid->Reset();

    m_iAssistCountdown = 3;
}

float *MotorizedJoint::GetDataPointer(const std::string &strDataType)
{
	if(strDataType == "MOTORFORCETOAX")
	{
        EnableFeedback();
        return (&m_vMotorForceToA[0]);
    }
	else if(strDataType == "MOTORFORCETOAY")
	{
        EnableFeedback();
        return (&m_vMotorForceToA[1]);
    }
	else if(strDataType == "MOTORFORCETOAZ")
	{
        EnableFeedback();
        return (&m_vMotorForceToA[2]);
    }
	else if(strDataType == "MOTORASSISTFORCETOAX")
        return (&m_vMotorAssistForceToAReport[0]);
	else if(strDataType == "MOTORASSISTFORCETOAY")
        return (&m_vMotorAssistForceToAReport[1]);
	else if(strDataType == "MOTORASSISTFORCETOAZ")
        return (&m_vMotorAssistForceToAReport[2]);
	else if(strDataType == "MOTORFORCETOBX")
	{
        EnableFeedback();
        return (&m_vMotorForceToB[0]);
    }
	else if(strDataType == "MOTORFORCETOBY")
	{
        EnableFeedback();
        return (&m_vMotorForceToB[1]);
    }
	else if(strDataType == "MOTORFORCETOBZ")
	{
        EnableFeedback();
        return (&m_vMotorForceToB[2]);
    }
	else if(strDataType == "MOTORASSISTFORCETOBX")
        return (&m_vMotorAssistForceToBReport[0]);
	else if(strDataType == "MOTORASSISTFORCETOBY")
        return (&m_vMotorAssistForceToBReport[1]);
	else if(strDataType == "MOTORASSISTFORCETOBZ")
        return (&m_vMotorAssistForceToBReport[2]);
	else if(strDataType == "MOTORTORQUETOAX")
	{
        EnableFeedback();
        return (&m_vMotorTorqueToA[0]);
    }
	else if(strDataType == "MOTORTORQUETOAY")
	{
        EnableFeedback();
        return (&m_vMotorTorqueToA[1]);
    }
	else if(strDataType == "MOTORTORQUETOAZ")
	{
        EnableFeedback();
        return (&m_vMotorTorqueToA[2]);
    }
	else if(strDataType == "MOTORASSISTTORQUETOAX")
        return (&m_vMotorAssistTorqueToAReport[0]);
	else if(strDataType == "MOTORASSISTTORQUETOAY")
        return (&m_vMotorAssistTorqueToAReport[1]);
	else if(strDataType == "MOTORASSISTTORQUETOAZ")
        return (&m_vMotorAssistTorqueToAReport[2]);
	else if(strDataType == "MOTORTORQUETOBX")
	{
        EnableFeedback();
        return (&m_vMotorTorqueToB[0]);
    }
	else if(strDataType == "MOTORTORQUETOBY")
	{
        EnableFeedback();
        return (&m_vMotorTorqueToB[1]);
    }
	else if(strDataType == "MOTORTORQUETOBZ")
	{
        EnableFeedback();
        return (&m_vMotorTorqueToB[2]);
    }
	else if(strDataType == "MOTORASSISTTORQUETOBX")
        return (&m_vMotorAssistTorqueToBReport[0]);
	else if(strDataType == "MOTORASSISTTORQUETOBY")
        return (&m_vMotorAssistTorqueToBReport[1]);
	else if(strDataType == "MOTORASSISTTORQUETOBZ")
        return (&m_vMotorAssistTorqueToBReport[2]);
    else
        return Joint::GetDataPointer(strDataType);

    return NULL;
}

bool MotorizedJoint::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Joint::SetData(strType, strValue, false))
		return true;

	if(strType == "ENABLEMOTOR")
	{
		EnableMotor(Std_ToBool(strValue));
		return true;
	}

	if(strType == "SERVOMOTOR")
	{
		ServoMotor(Std_ToBool(strValue));
		return true;
	}
	
	if(strType == "SERVOGAIN")
	{
		ServoGain((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXFORCE")
	{
		MaxForce((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXVELOCITY")
	{
		MaxVelocity((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void MotorizedJoint::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	Joint::QueryProperties(aryNames, aryTypes);

	aryNames.Add("EnableMotor");
	aryTypes.Add("Boolean");

	aryNames.Add("ServoMotor");
	aryTypes.Add("Boolean");

	aryNames.Add("ServoGain");
	aryTypes.Add("Float");

	aryNames.Add("MaxForce");
	aryTypes.Add("Float");

	aryNames.Add("MaxVelocity");
	aryTypes.Add("Float");
}

/**
 \brief Clears the assist forces.

 \author    David Cofer
 \date  1/25/2014
 */
void MotorizedJoint::ClearAssistForces()
{
    m_vMotorForceToA.Set(0,0,0);
    m_vMotorAssistForceToA.Set(0,0,0);
    m_vMotorAssistForceToAReport.Set(0,0,0);
    m_vMotorForceToB.Set(0,0,0);
    m_vMotorAssistForceToB.Set(0,0,0);
    m_vMotorAssistForceToBReport.Set(0,0,0);
    m_vMotorTorqueToA.Set(0,0,0);
    m_vMotorAssistTorqueToA.Set(0,0,0);
    m_vMotorAssistTorqueToAReport.Set(0,0,0);
    m_vMotorTorqueToB.Set(0,0,0);
    m_vMotorAssistTorqueToB.Set(0,0,0);
    m_vMotorAssistTorqueToBReport.Set(0,0,0);
}

/**
 \brief Applies the motor assist.

 \author    David Cofer
 \date  1/25/2014
 */
void MotorizedJoint::ApplyMotorAssist()
{}

/**
 \brief Enables joint feedback.

 \author    David Cofer
 \date  1/25/2014
 */
void MotorizedJoint::EnableFeedback()
{}

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