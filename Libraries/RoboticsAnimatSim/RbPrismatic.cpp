/**
\file	RbPrismatic.cpp

\brief	Implements the vs prismatic class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbPrismaticLimit.h"
#include "RbRigidBody.h"
#include "RbPrismatic.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
RbPrismatic::RbPrismatic()
{
	SetThisPointers();

	m_lpUpperLimit = new RbPrismaticLimit();
	m_lpLowerLimit = new RbPrismaticLimit();
	m_lpPosFlap = new RbPrismaticLimit();

	m_lpUpperLimit->LimitPos(1, false);
	m_lpLowerLimit->LimitPos(-1, false);
	m_lpPosFlap->LimitPos(Prismatic::JointPosition(), false);
	m_lpPosFlap->IsShowPosition(true);

	m_lpUpperLimit->Color(0, 0, 1, 1);
	m_lpLowerLimit->Color(1, 1, 0.333f, 1);
	m_lpPosFlap->Color(1, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(true);
	m_lpUpperLimit->IsLowerLimit(false);
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
RbPrismatic::~RbPrismatic()
{
	//ConstraintLimits are deleted in the base objects.
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbPrismatic/\r\n", "", -1, false, true);}
}


void RbPrismatic::EnableLimits(bool bVal)
{
	Prismatic::EnableLimits(bVal);

    SetLimitValues();
}

void RbPrismatic::SetLimitValues()
{
    //if(m_lpSim && m_btJoint && m_btPrismatic)
    //{
    //    if(m_bEnableLimits)
    //    {
    //        m_bJointLocked = false;
    //        m_btPrismatic->setLinearLowerLimit(btVector3(m_lpLowerLimit->LimitPos(), 0, 0));
    //        m_btPrismatic->setLinearUpperLimit(btVector3(m_lpUpperLimit->LimitPos(), 0, 0));

    //        //Disable rotation about the axis for the prismatic joint.
		  //  m_btPrismatic->setAngularLowerLimit(btVector3(0,0,0));
		  //  m_btPrismatic->setAngularUpperLimit(btVector3(0,0,0));

    //        float fltKp = m_lpUpperLimit->Stiffness();
    //        float fltKd = m_lpUpperLimit->Damping();
    //        float fltH = m_lpSim->PhysicsTimeStep()*1000;
    //        
    //        float fltErp = (fltH*fltKp)/((fltH*fltKp) + fltKd);
    //        float fltCfm = 1/((fltH*fltKp) + fltKd);
    //    }
    //    else
    //    {
    //        //To disable limits in bullet we need the lower limit to be bigger than the upper limit
    //        m_bJointLocked = false;

    //        m_btPrismatic->setLinearLowerLimit(btVector3(1, 0, 0));
    //        m_btPrismatic->setLinearUpperLimit(btVector3(-1, 0, 0));

		  //  m_btPrismatic->setAngularLowerLimit(btVector3(0,0,0));
		  //  m_btPrismatic->setAngularUpperLimit(btVector3(0,0,0));
    //    }
    //}
}

void RbPrismatic::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}

void RbPrismatic::CreateJoint()
{
}


#pragma region DataAccesMethods

float *RbPrismatic::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	if(strType == "JOINTROTATION")
		return &m_fltPosition;
	else if(strType == "JOINTPOSITION")
		return &m_fltPosition;
	else if(strType == "JOINTACTUALVELOCITY")
		return &m_fltVelocity;
	else if(strType == "JOINTFORCE")
		return &m_fltForce;
	else if(strType == "JOINTDESIREDVELOCITY")
		return &m_fltReportSetVelocity;
	else if(strType == "JOINTSETVELOCITY")
		return &m_fltReportSetVelocity;
	else if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "CONTACTCOUNT")
		THROW_PARAM_ERROR(Al_Err_lMustBeContactBodyToGetCount, Al_Err_strMustBeContactBodyToGetCount, "JointID", m_strName);
	else
	{
        lpData = RbMotorizedJoint::Physics_GetDataPointer(strType);
		if(lpData) return lpData;

		lpData = Prismatic::GetDataPointer(strType);
		if(lpData) return lpData;

		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);
	}

	return lpData;
}

bool RbPrismatic::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	//if(RbJoint::Physics_SetData(strDataType, strValue))
	//	return true;

	if(Prismatic::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbPrismatic::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	//RbJoint::Physics_QueryProperties(aryProperties);
	Prismatic::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("JointRotation", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointPosition", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointActualVelocity", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointForce", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointRotationDeg", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointDesiredVelocity", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointSetVelocity", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Enable", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));
}

#pragma endregion

void RbPrismatic::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

void RbPrismatic::Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce)
{
 //   if (m_btJoint && m_btPrismatic)
	//{ 		
	//	if(bOn)
	//	{
 //           m_bJointLocked = true;

	//	    m_btPrismatic->setLinearLowerLimit(btVector3(fltPosition, 0, 0));
	//	    m_btPrismatic->setLinearUpperLimit(btVector3(fltPosition, 0, 0));
	//	}
	//	else if (m_bMotorOn)
	//		Physics_EnableMotor(true, 0, fltMaxLockForce, false);
	//	else
 //           SetLimitValues();
	//}
}

void RbPrismatic::Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup)
{
 //   if (m_btJoint && m_btPrismatic)
	//{   
	//	if(bOn)
 //       {
 //          //if(Std_ToLower(m_lpThisJoint->ID()) == "61cbf08d-4625-4b9f-87cd-d08b778cf04e" && GetSimulator()->Time() >= 1.01)
 //          //     bOn = bOn;  //Testing

	//		//I had to cut this if statement out. I kept running into one instance after another where I ran inot a problem if I did not do this every single time.
	//		// It is really annoying and inefficient, but I cannot find another way to reiably guarantee that the motor will behave coorectly under all conditions without
	//		// doing this every single time I set the motor velocity.
 //           //if(!m_bMotorOn || bForceWakeup || m_bJointLocked || JointIsLocked() || fabs(m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0]) < 1e-4)
 //           //{    
 //               SetLimitValues();
 //               m_lpThisJoint->WakeDynamics();
 //           //}

	//	    m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = true;
	//	    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0] = -fltDesiredVelocity;
	//	    m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = fltMaxForce;
 //       }
	//	else
 //       {
 //           TurnMotorOff();

 //           if(m_bMotorOn || bForceWakeup || m_bJointLocked || JointIsLocked())
 //           {
 //               m_iAssistCountdown = 3;
 //               ClearAssistForces();
 //               m_lpThisJoint->WakeDynamics();
 //               SetLimitValues();
 //           }
 //       }

	//	m_bMotorOn = bOn;
	//}
}

void RbPrismatic::Physics_MaxForce(float fltVal)
{
    //if(m_btJoint && m_btPrismatic)
    //    m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = fltVal;
}

void RbPrismatic::TurnMotorOff()
{
    //if(m_btPrismatic)
    //{
    //    if(m_lpFriction && m_lpFriction->Enabled())
    //    {
    //        //0.032 is a coefficient that produces friction behavior in bullet using the same coefficient values
    //        //that were specified in vortex engine. This way I get similar behavior between the two.
    //        float	maxMotorImpulse = m_lpFriction->Coefficient()*0.032f*(m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());  
		  //  m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = true;
		  //  m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0] = 0;
		  //  m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = maxMotorImpulse;
    //    }
    //    else
		  //  m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = false;
    //}
}

void RbPrismatic::SetConstraintFriction()
{
    //if(m_btPrismatic && !m_bJointLocked && !m_bMotorOn && m_bEnabled)
    //    TurnMotorOff();
}

void RbPrismatic::ResetSimulation()
{
	Prismatic::ResetSimulation();

    //m_btPrismatic->getTranslationalLimitMotor()->m_currentLinearDiff = btVector3(0, 0, 0);
    //m_btPrismatic->getTranslationalLimitMotor()->m_accumulatedImpulse = btVector3(0, 0, 0);
    //m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity = btVector3(0, 0, 0);
}

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
