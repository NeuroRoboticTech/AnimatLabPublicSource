/**
\file	BlPrismatic.cpp

\brief	Implements the vs prismatic class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlPrismaticLimit.h"
#include "BlRigidBody.h"
#include "BlPrismatic.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
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
BlPrismatic::BlPrismatic()
{
	SetThisPointers();
	m_btPrismatic = NULL;

	m_lpUpperLimit = new BlPrismaticLimit();
	m_lpLowerLimit = new BlPrismaticLimit();
	m_lpPosFlap = new BlPrismaticLimit();

	m_lpUpperLimit->LimitPos(1, false);
	m_lpLowerLimit->LimitPos(-1, false);
	m_lpPosFlap->LimitPos(Prismatic::JointPosition(), false);
	m_lpPosFlap->IsShowPosition(true);

	m_lpUpperLimit->Color(0, 0, 1, 1);
	m_lpLowerLimit->Color(1, 1, 0.333, 1);
	m_lpPosFlap->Color(1, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(true);
	m_lpUpperLimit->IsLowerLimit(false);
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
BlPrismatic::~BlPrismatic()
{
	//ConstraintLimits are deleted in the base objects.
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlPrismatic/\r\n", "", -1, false, true);}
}


void BlPrismatic::EnableLimits(bool bVal)
{
	Prismatic::EnableLimits(bVal);

    SetLimitValues();
}

void BlPrismatic::SetLimitValues()
{
    if(m_lpSim && m_btJoint && m_btPrismatic)
    {
        if(m_bEnableLimits)
        {
            m_bJointLocked = false;
            m_btPrismatic->setLinearLowerLimit(btVector3(m_lpLowerLimit->LimitPos(), 0, 0));
            m_btPrismatic->setLinearUpperLimit(btVector3(m_lpUpperLimit->LimitPos(), 0, 0));

            //Disable rotation about the axis for the prismatic joint.
		    m_btPrismatic->setAngularLowerLimit(btVector3(0,0,0));
		    m_btPrismatic->setAngularUpperLimit(btVector3(0,0,0));

            float fltKp = m_lpUpperLimit->Stiffness();
            float fltKd = m_lpUpperLimit->Damping();
            float fltH = m_lpSim->PhysicsTimeStep()*1000;
            
            float fltErp = (fltH*fltKp)/((fltH*fltKp) + fltKd);
            float fltCfm = 1/((fltH*fltKp) + fltKd);
        }
        else
        {
            //To disable limits in bullet we need the lower limit to be bigger than the upper limit
            m_bJointLocked = false;

            m_btPrismatic->setLinearLowerLimit(btVector3(1, 0, 0));
            m_btPrismatic->setLinearUpperLimit(btVector3(-1, 0, 0));

		    m_btPrismatic->setAngularLowerLimit(btVector3(0,0,0));
		    m_btPrismatic->setAngularUpperLimit(btVector3(0,0,0));
        }
    }
}

void BlPrismatic::TimeStepModified()
{
    SetLimitValues();
}

void BlPrismatic::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}


void BlPrismatic::SetAlpha()
{
	BlJoint::SetAlpha();

	m_lpUpperLimit->Alpha(m_fltAlpha);
	m_lpLowerLimit->Alpha(m_fltAlpha);
	m_lpPosFlap->Alpha(m_fltAlpha);
}

void BlPrismatic::DeleteJointGraphics()
{
	OsgPrismaticLimit *lpUpperLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpUpperLimit);
	OsgPrismaticLimit *lpLowerLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpLowerLimit);
	OsgPrismaticLimit *lpPosFlap = dynamic_cast<OsgPrismaticLimit *>(m_lpPosFlap);

    OsgPrismatic::DeletePrismaticGraphics(m_osgJointMT, lpUpperLimit, lpLowerLimit, lpPosFlap);

    if(m_lpUpperLimit) m_lpUpperLimit->DeleteGraphics();
    if(m_lpLowerLimit) m_lpLowerLimit->DeleteGraphics();
    if(m_lpPosFlap) m_lpPosFlap->DeleteGraphics();
}

void BlPrismatic::CreateJointGraphics()
{
	OsgPrismaticLimit *lpUpperLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpUpperLimit);
	OsgPrismaticLimit *lpLowerLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpLowerLimit);
	OsgPrismaticLimit *lpPosFlap = dynamic_cast<OsgPrismaticLimit *>(m_lpPosFlap);

	m_lpPosFlap->LimitPos(Prismatic::JointPosition());
	OsgPrismatic::CreatePrismaticGraphics(BoxSize(), CylinderRadius(), 
                                          m_osgJointMT, lpUpperLimit, 
                                          lpLowerLimit, lpPosFlap);
}

void BlPrismatic::SetupPhysics()
{
    if(m_btJoint)
		DeletePhysics(false);

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	BlRigidBody *lpVsChild = dynamic_cast<BlRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

    btTransform mtJointRelParent, mtJointRelChild;
	CStdFPoint vRot(0, 0, osg::PI);
    CalculateRelativeJointMatrices(vRot, mtJointRelParent, mtJointRelChild);

	m_btPrismatic = new btGeneric6DofConstraint(*lpVsParent->Part(), *lpVsChild->Part(), mtJointRelParent, mtJointRelChild, false); 

    GetBlSimulator()->DynamicsWorld()->addConstraint(m_btPrismatic, true);
    m_btPrismatic->setDbgDrawSize(btScalar(5.f));

	BlPrismaticLimit *lpUpperLimit = dynamic_cast<BlPrismaticLimit *>(m_lpUpperLimit);
	BlPrismaticLimit *lpLowerLimit = dynamic_cast<BlPrismaticLimit *>(m_lpLowerLimit);

	m_btJoint = m_btPrismatic;

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	//If the motor is enabled then it will start out with a velocity of	zero.
	EnableMotor(m_bEnableMotorInit);

    Prismatic::Initialize();
    BlJoint::Initialize();
}

void BlPrismatic::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

float *BlPrismatic::GetDataPointer(const std::string &strDataType)
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
		lpData = Prismatic::GetDataPointer(strDataType);
		if(lpData) return lpData;

		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);
	}

	return lpData;
}

bool BlPrismatic::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Prismatic::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlPrismatic::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	Prismatic::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

void BlPrismatic::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

bool BlPrismatic::JointIsLocked()
{
	btVector3 vLower, vUpper;
	m_btPrismatic->getLinearLowerLimit(vLower);
	m_btPrismatic->getLinearUpperLimit(vUpper);
	if( (vLower[0] == vUpper[0]) || (vLower[0] > vUpper[0]) )
		return true;
	else
		return false;
}


void BlPrismatic::Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce)
{
    if (m_btJoint && m_btPrismatic)
	{ 		
		if(bOn)
		{
            m_bJointLocked = true;

		    m_btPrismatic->setLinearLowerLimit(btVector3(fltPosition, 0, 0));
		    m_btPrismatic->setLinearUpperLimit(btVector3(fltPosition, 0, 0));
		}
		else if (m_bMotorOn)
			Physics_EnableMotor(true, 0, fltMaxLockForce, false);
		else
            SetLimitValues();
	}
}

void BlPrismatic::Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup)
{
    if (m_btJoint && m_btPrismatic)
	{   
		if(bOn)
        {
			//I had to cut this if statement out. I kept running into one instance after another where I ran inot a problem if I did not do this every single time.
			// It is really annoying and inefficient, but I cannot find another way to reiably guarantee that the motor will behave coorectly under all conditions without
			// doing this every single time I set the motor velocity.
            //if(!m_bMotorOn || bForceWakeup || m_bJointLocked || JointIsLocked() || fabs(m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0]) < 1e-4)
            //{    
                SetLimitValues();
                m_lpThisJoint->WakeDynamics();
            //}

		    m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = true;
		    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0] = -fltDesiredVelocity;
		    m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = fltMaxForce;
        }
		else
        {
            TurnMotorOff();

            if(m_bMotorOn || bForceWakeup || m_bJointLocked || JointIsLocked())
            {
                m_lpThisJoint->WakeDynamics();
                SetLimitValues();
            }
        }

		m_bMotorOn = bOn;
	}
}

void BlPrismatic::Physics_MaxForce(float fltVal)
{
    if(m_btJoint && m_btPrismatic)
        m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = fltVal;
}

float BlPrismatic::GetCurrentBtPosition()
{
    if(m_btJoint && m_btPrismatic)
    {
        btVector3 vDiff = m_btPrismatic->getTranslationalLimitMotor()->m_currentLinearDiff;
        return vDiff[0];
    }
    else
        return 0;
}

void BlPrismatic::TurnMotorOff()
{
    if(m_btPrismatic)
    {
        if(m_lpFriction && m_lpFriction->Enabled())
        {
            //0.032 is a coefficient that produces friction behavior in bullet using the same coefficient values
            //that were specified in vortex engine. This way I get similar behavior between the two.
            float	maxMotorImpulse = m_lpFriction->Coefficient()*0.032f;  
		    m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = true;
		    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0] = 0;
		    m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = maxMotorImpulse;
        }
        else
		    m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = false;
    }
}

void BlPrismatic::SetConstraintFriction()
{
    if(m_btPrismatic && !m_bJointLocked && !m_bMotorOn && m_bEnabled)
        TurnMotorOff();
}

void BlPrismatic::ResetSimulation()
{
	Prismatic::ResetSimulation();

    m_btPrismatic->getTranslationalLimitMotor()->m_currentLinearDiff = btVector3(0, 0, 0);
    m_btPrismatic->getTranslationalLimitMotor()->m_accumulatedImpulse = btVector3(0, 0, 0);
    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity = btVector3(0, 0, 0);
}

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
