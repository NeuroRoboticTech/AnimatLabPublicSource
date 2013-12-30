/**
\file	BlHinge.cpp

\brief	Implements the vortex hinge class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlJoint.h"
#include "BlHingeLimit.h"
#include "BlHinge.h"
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
\date	4/15/2011
**/
BlHinge::BlHinge()
{
	SetThisPointers();
	m_btHinge = NULL;
	m_fltRotationDeg = 0;

	m_lpUpperLimit = new BlHingeLimit();
	m_lpLowerLimit = new BlHingeLimit();
	m_lpPosFlap = new BlHingeLimit();

	m_lpUpperLimit->LimitPos(0.25*osg::PI, false);
	m_lpLowerLimit->LimitPos(-0.25*osg::PI, false);
	m_lpPosFlap->LimitPos(Hinge::JointPosition(), false);
	m_lpPosFlap->IsShowPosition(true);

	m_lpUpperLimit->Color(1, 0, 0, 1);
	m_lpLowerLimit->Color(1, 1, 1, 1);
	m_lpPosFlap->Color(0, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(true);
	m_lpUpperLimit->IsLowerLimit(false);
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
BlHinge::~BlHinge()
{
	//ConstraintLimits are deleted in the base objects.
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlHinge\r\n", "", -1, false, true);}
}

void BlHinge::EnableLimits(bool bVal)
{
	Hinge::EnableLimits(bVal);

    SetLimitValues();
}

void BlHinge::SetLimitValues()
{
    if(m_btHinge)
    {
        if(m_bEnableLimits)
        {
            m_bJointLocked = false;

            m_btHinge->setLinearLowerLimit(btVector3(0, 0, 0));
            m_btHinge->setLinearUpperLimit(btVector3(0, 0, 0));

            //Disable rotation about the axis for the prismatic joint.
		    m_btHinge->setAngularLowerLimit(btVector3(m_lpLowerLimit->LimitPos(),0,0));
		    m_btHinge->setAngularUpperLimit(btVector3(m_lpUpperLimit->LimitPos(),0,0));

            //float fltKp = m_lpUpperLimit->Stiffness();
            //float fltKd = m_lpUpperLimit->Damping();
            //float fltH = m_lpSim->PhysicsTimeStep()*1000;
            
            //float fltErp = 0.9; //(fltH*fltKp)/((fltH*fltKp) + fltKd);
            //float fltCfm = 0.1; //1/((fltH*fltKp) + fltKd);

            //m_btHinge->setParam(BT_CONSTRAINT_STOP_CFM, fltCfm, -1);
            //m_btHinge->setParam(BT_CONSTRAINT_STOP_ERP, fltErp, -1);
        }
        else
        {
            //To disable limits in bullet we need the lower limit to be bigger than the upper limit
            m_bJointLocked = false;

            m_btHinge->setLinearLowerLimit(btVector3(0, 0, 0));
            m_btHinge->setLinearUpperLimit(btVector3(0, 0, 0));

            //Disable rotation about the axis for the prismatic joint.
		    m_btHinge->setAngularLowerLimit(btVector3(1,0,0));
		    m_btHinge->setAngularUpperLimit(btVector3(-1,0,0));
        }
    }
}

void BlHinge::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}

void BlHinge::SetAlpha()
{
	BlJoint::SetAlpha();

	m_lpUpperLimit->Alpha(m_fltAlpha);
	m_lpLowerLimit->Alpha(m_fltAlpha);
	m_lpPosFlap->Alpha(m_fltAlpha);

	if(m_osgCylinderMat.valid() && m_osgCylinderSS.valid())
		SetMaterialAlpha(m_osgCylinderMat.get(), m_osgCylinderSS.get(), m_fltAlpha);

}

void BlHinge::DeleteJointGraphics()
{
	OsgHingeLimit *lpUpperLimit = dynamic_cast<OsgHingeLimit *>(m_lpUpperLimit);
	OsgHingeLimit *lpLowerLimit = dynamic_cast<OsgHingeLimit *>(m_lpLowerLimit);
	OsgHingeLimit *lpPosFlap = dynamic_cast<OsgHingeLimit *>(m_lpPosFlap);

    OsgHinge::DeleteHingeGraphics(m_osgJointMT, lpUpperLimit, lpLowerLimit, lpPosFlap);

    if(m_lpUpperLimit) m_lpUpperLimit->DeleteGraphics();
    if(m_lpLowerLimit) m_lpLowerLimit->DeleteGraphics();
    if(m_lpPosFlap) m_lpPosFlap->DeleteGraphics();
}

void BlHinge::CreateJointGraphics()
{
	OsgHingeLimit *lpUpperLimit = dynamic_cast<OsgHingeLimit *>(m_lpUpperLimit);
	OsgHingeLimit *lpLowerLimit = dynamic_cast<OsgHingeLimit *>(m_lpLowerLimit);
	OsgHingeLimit *lpPosFlap = dynamic_cast<OsgHingeLimit *>(m_lpPosFlap);

    float fltLimitPos = Hinge::JointPosition();
	m_lpPosFlap->LimitPos(fltLimitPos);

    OsgHinge::CreateHingeGraphics(CylinderHeight(), CylinderRadius(), FlapWidth(), 
                                  m_osgJointMT, lpUpperLimit, lpLowerLimit, lpPosFlap);
}


void BlHinge::Physics_UpdateAbsolutePosition()
{
	//If we are here then we did not have a physics component, just and OSG one.
	CStdFPoint vPos = OsgMovableItem::GetOSGWorldCoords();
	vPos.ClearNearZero();
	m_lpThisMI->AbsolutePosition(vPos.x, vPos.y, vPos.z);
}

void BlHinge::SetupPhysics()
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
    CalculateRelativeJointMatrices(mtJointRelParent, mtJointRelChild);

    m_btHinge = new btGeneric6DofConstraint(*lpVsParent->Part(), *lpVsChild->Part(), mtJointRelParent, mtJointRelChild, false); 

    m_btHinge->setDbgDrawSize(btScalar(5.f));

	GetBlSimulator()->DynamicsWorld()->addConstraint(m_btHinge, true);

	BlHingeLimit *lpUpperLimit = dynamic_cast<BlHingeLimit *>(m_lpUpperLimit);
	BlHingeLimit *lpLowerLimit = dynamic_cast<BlHingeLimit *>(m_lpLowerLimit);

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	m_btJoint = m_btHinge;

    //Init the current position.
    m_btHinge->getRotationalLimitMotor(0)->m_currentPosition = 0;

	//If the motor is enabled then it will start out with a velocity of	zero.
    EnableMotor(m_bEnableMotorInit);

    Hinge::Initialize();
    BlJoint::Initialize();
}

void BlHinge::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

float *BlHinge::GetDataPointer(const std::string &strDataType)
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
	else if(strType == "JOINTROTATIONDEG")
		return &m_fltRotationDeg;
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
		lpData = Hinge::GetDataPointer(strDataType);
		if(lpData) return lpData;

		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);
	}

	return lpData;
}

bool BlHinge::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Hinge::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlHinge::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	Hinge::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

void BlHinge::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

void BlHinge::UpdateData()
{
	Hinge::UpdateData();
	m_fltRotationDeg = ((m_fltPosition/osg::PI)*180);
}


void BlHinge::Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce)
{
    if (m_btJoint && m_btHinge)
	{ 		
		if(bOn)
		{
            m_bJointLocked = true;

		    m_btHinge->setAngularLowerLimit(btVector3(fltPosition,0,0));
		    m_btHinge->setAngularUpperLimit(btVector3(fltPosition,0,0));
		}
		else if (m_bMotorOn)
			Physics_EnableMotor(true, 0, fltMaxLockForce, false);
		else
            SetLimitValues();
	}
}

void BlHinge::Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup)
{
    if (m_btJoint && m_btHinge)
	{   
		if(bOn)
        {
            if(!m_bMotorOn || bForceWakeup)
            {
                SetLimitValues();
                m_lpThisJoint->WakeDynamics();
            }

		    m_btHinge->getRotationalLimitMotor(0)->m_enableMotor = true;
		    m_btHinge->getRotationalLimitMotor(0)->m_targetVelocity = fltDesiredVelocity;
		    m_btHinge->getRotationalLimitMotor(0)->m_maxMotorForce = fltMaxForce;
        }
		else
        {
            TurnMotorOff();

            if(m_bMotorOn || bForceWakeup)
            {
                m_lpThisJoint->WakeDynamics();
                SetLimitValues();
            }
        }

		m_bMotorOn = bOn;
	}
}

void BlHinge::Physics_MaxForce(float fltVal)
{
    if(m_btJoint && m_btHinge)
        m_btHinge->getRotationalLimitMotor(0)->m_maxMotorForce = fltVal;
}

float BlHinge::GetCurrentBtPosition()
{
    if(m_btJoint && m_btHinge)
        return m_btHinge->getRotationalLimitMotor(0)->m_currentPosition;
    else
        return 0;
}

void BlHinge::TurnMotorOff()
{
    if(m_btHinge)
    {
        if(m_lpFriction && m_lpFriction->Enabled())
        {
            //0.032 is a coefficient that produces friction behavior in bullet using the same coefficient values
            //that were specified in vortex engine. This way I get similar behavior between the two.
            float	maxMotorImpulse = m_lpFriction->Coefficient()*0.032f;  
		    m_btHinge->getRotationalLimitMotor(0)->m_enableMotor = true;
		    m_btHinge->getRotationalLimitMotor(0)->m_targetVelocity = 0;
		    m_btHinge->getRotationalLimitMotor(0)->m_maxMotorForce = maxMotorImpulse;
        }
        else
		    m_btHinge->getRotationalLimitMotor(0)->m_enableMotor = false;
    }
}

void BlHinge::SetConstraintFriction()
{
    if(m_btHinge && !m_bJointLocked && !m_bMotorOn && m_bEnabled)
        TurnMotorOff();
}

void BlHinge::ResetSimulation()
{
    m_btHinge->getRotationalLimitMotor(0)->m_currentPosition = 0;
    m_btHinge->getRotationalLimitMotor(0)->m_accumulatedImpulse = 0;
}


		}		//Joints
	}			// Environment
}				//BulletAnimatSim
