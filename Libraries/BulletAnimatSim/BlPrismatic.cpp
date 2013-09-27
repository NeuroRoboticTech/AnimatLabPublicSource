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
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlPrismatic/\r\n", "", -1, false, true);}
}


void BlPrismatic::EnableLimits(bool bVal)
{
	Prismatic::EnableLimits(bVal);

	if(m_bEnableLimits)
	{
		if(m_lpLowerLimit) m_lpLowerLimit->SetLimitPos();
		if(m_lpUpperLimit) m_lpUpperLimit->SetLimitPos();
	}
}

void BlPrismatic::SetLimitValues()
{
    if(m_btJoint && m_btPrismatic)
    {
        m_bJointLocked = false;
        m_btPrismatic->setLowerLinLimit((btScalar) m_lpLowerLimit->LimitPos());
        m_btPrismatic->setUpperLinLimit((btScalar) m_lpUpperLimit->LimitPos());

        //Disable rotation about the axis for the prismatic joint.
        m_btPrismatic->setLowerAngLimit(0);
        m_btPrismatic->setUpperAngLimit(0);
    }
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
		DeletePhysics();

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

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	m_btJoint = m_btPrismatic;

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

float *BlPrismatic::GetDataPointer(const string &strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

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

bool BlPrismatic::SetData(const string &strDataType, const string &strValue, bool bThrowError)
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

void BlPrismatic::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
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
                SetLimitValues();

			m_btPrismatic->setPoweredLinMotor(true);
            m_btPrismatic->setMaxLinMotorForce(fltMaxForce);
            m_btPrismatic->setTargetLinMotorVelocity(fltDesiredVelocity);
        }
		else
			m_btPrismatic->setPoweredLinMotor(false);

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


		}		//Joints
	}			// Environment
}				//BulletAnimatSim
