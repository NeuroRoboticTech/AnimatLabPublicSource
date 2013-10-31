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
            m_btPrismatic->setLowerLinLimit((btScalar) m_lpLowerLimit->LimitPos());
            m_btPrismatic->setUpperLinLimit((btScalar) m_lpUpperLimit->LimitPos());

            //Disable rotation about the axis for the prismatic joint.
            m_btPrismatic->setLowerAngLimit(0);
            m_btPrismatic->setUpperAngLimit(0);

            float fltKp = m_lpUpperLimit->Stiffness();
            float fltKd = m_lpUpperLimit->Damping();
            float fltH = m_lpSim->PhysicsTimeStep()*1000;
            
            float fltErp = (fltH*fltKp)/((fltH*fltKp) + fltKd);
            float fltCfm = 1/((fltH*fltKp) + fltKd);

            m_btPrismatic->setParam(BT_CONSTRAINT_STOP_CFM, fltCfm, -1);
            m_btPrismatic->setParam(BT_CONSTRAINT_STOP_ERP, fltErp, -1);
        }
        else
        {
            //To disable limits in bullet we need the lower limit to be bigger than the upper limit
            m_bJointLocked = false;
            m_btPrismatic->setLowerLinLimit(1);
            m_btPrismatic->setUpperLinLimit(-1);
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

    //Get the matrices for the joint relative to the child and parent.
    osg::Matrix osgJointRelParent = m_osgMT->getMatrix();
    CStdFPoint vPos = m_lpThisMI->Position();
    CStdFPoint vRot = m_lpThisMI->Rotation();
    osg::Matrix osgJointRelChild = SetupMatrix(vPos, vRot);

    btTransform tmJointRelParent = osgbCollision::asBtTransform(osgJointRelParent);
    btTransform tmJointRelChild = osgbCollision::asBtTransform(osgJointRelChild);

	m_btPrismatic = new btSliderConstraint(*lpVsParent->Part(), *lpVsChild->Part(), tmJointRelParent, tmJointRelChild, false); 

    GetBlSimulator()->DynamicsWorld()->addConstraint(m_btPrismatic, true);
    m_btPrismatic->setDbgDrawSize(btScalar(5.f));

    //There is a bug in the slider code where the LinPos is not initialized correctly until after the first time the 
    //limits are tested. I am calling this here manually to clear that variable.
    m_btPrismatic->testLinLimits();

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



void BlPrismatic::Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce)
{
    if (m_btJoint && m_btPrismatic)
	{ 		
		if(bOn)
		{
            m_bJointLocked = true;
            m_btPrismatic->setLowerLinLimit(fltPosition);
            m_btPrismatic->setUpperLinLimit(fltPosition);
		}
		else if (m_bMotorOn)
			Physics_EnableMotor(true, 0, fltMaxLockForce);
		else
            SetLimitValues();
	}
}

void BlPrismatic::Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce)
{
    if (m_btJoint && m_btPrismatic)
	{   
		if(bOn)
        {
            if(!m_bMotorOn)
            {
                SetLimitValues();
                m_lpThisJoint->WakeDynamics();
            }

			m_btPrismatic->setPoweredLinMotor(true);
            m_btPrismatic->setMaxLinMotorForce(fltMaxForce);
            m_btPrismatic->setTargetLinMotorVelocity(fltDesiredVelocity);
        }
		else
        {
            TurnMotorOff();

            if(m_bMotorOn)
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
        m_btPrismatic->setMaxLinMotorForce(fltVal);
}

float BlPrismatic::GetCurrentBtPosition()
{
    if(m_btJoint && m_btPrismatic)
        return m_btPrismatic->getLinearPos();
    else
        return 0;
}

void BlPrismatic::TurnMotorOff()
{
    if(m_btPrismatic)
    {
        //FIX PHYSICS (Need to fix bullet so this will work.)
        //if(m_lpFriction && m_lpFriction->Enabled())
        //{
        //    //0.032 is a coefficient that produces friction behavior in bullet using the same coefficient values
        //    //that were specified in vortex engine. This way I get similar behavior between the two.
        //    float	maxMotorImpulse = m_lpFriction->Coefficient()*0.032f;  
        //    m_btPrismatic->setMaxLinMotorForce(maxMotorImpulse);
        //    m_btPrismatic->setTargetLinMotorVelocity(0);
    	   // m_btPrismatic->setPoweredLinMotor(true);
        //}
        //else
    	    m_btPrismatic->setPoweredLinMotor(false);
    }
}

void BlPrismatic::SetConstraintFriction()
{
    if(m_btPrismatic && !m_bJointLocked && !m_bMotorOn && m_bEnabled)
        TurnMotorOff();
}


		}		//Joints
	}			// Environment
}				//BulletAnimatSim
