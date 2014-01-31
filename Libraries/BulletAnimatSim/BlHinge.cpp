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
    m_fltChildMassWithChildren = 0;

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

	m_lpBlParent = dynamic_cast<BlRigidBody *>(m_lpParent);
	if(!m_lpBlParent)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	m_lpBlChild = dynamic_cast<BlRigidBody *>(m_lpChild);
	if(!m_lpBlChild)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

    m_btParent = m_lpBlParent->Part();
    m_btChild = m_lpBlChild->Part();

    btTransform mtJointRelParent, mtJointRelChild;
    CalculateRelativeJointMatrices(mtJointRelParent, mtJointRelChild);

    m_btHinge = new btAnimatGeneric6DofConstraint(*m_lpBlParent->Part(), *m_lpBlChild->Part(), mtJointRelParent, mtJointRelChild, false); 

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

    //Turn off sleeping thresholds for parent and child of hinge joints to prevent the parts from
    //falling asleep. If it does the joint motor has a tendency to not work.
    if(m_lpBlParent && m_lpBlParent->Part())
        m_lpBlParent->Part()->setSleepingThresholds(0, 0);

    if(m_lpBlChild && m_lpBlChild->Part())
        m_lpBlChild->Part()->setSleepingThresholds(0, 0);

    m_btHinge->setJointFeedback(&m_btJointFeedback);

    Hinge::Initialize();
    BlJoint::Initialize();

    m_fltChildMassWithChildren = m_lpChild->MassWithChildren();
    m_fltChildMassWithChildren *= (m_lpThisAB->GetSimulator()->MassUnits());
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
        lpData = BlMotorizedJoint::Physics_GetDataPointer(strType);
		if(lpData) return lpData;

		lpData = Hinge::GetDataPointer(strType);
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
    ApplyMotorAssist();
}

void BlHinge::UpdateData()
{
	Hinge::UpdateData();
	m_fltRotationDeg = ((m_fltPosition/osg::PI)*180);
}

bool BlHinge::JointIsLocked()
{
	btVector3 vLower, vUpper;
	m_btHinge->getAngularLowerLimit(vLower);
	m_btHinge->getAngularUpperLimit(vUpper);
	if( (vLower[0] == vUpper[0]) || (vLower[0] > vUpper[0]) )
		return true;
	else
		return false;
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
            if(!m_bMotorOn || bForceWakeup || m_bJointLocked || JointIsLocked())
            {
                m_fltNextPredictedPos = m_lpThisJoint->JointPosition();
                m_fltPredictedPos = m_fltNextPredictedPos;
            }

			//I had to move these statements out of the if above. I kept running into one instance after another where I ran inot a problem if I did not do this every single time.
			// It is really annoying and inefficient, but I cannot find another way to reiably guarantee that the motor will behave coorectly under all conditions without
			// doing this every single time I set the motor velocity.
            SetLimitValues();
            m_lpThisJoint->WakeDynamics();

		    m_btHinge->getRotationalLimitMotor(0)->m_enableMotor = true;
		    m_btHinge->getRotationalLimitMotor(0)->m_targetVelocity = fltDesiredVelocity;
		    m_btHinge->getRotationalLimitMotor(0)->m_maxMotorForce = fltMaxForce;
        }
		else
        {
            TurnMotorOff();

            if(m_bMotorOn || bForceWakeup || m_bJointLocked || JointIsLocked())
            {
                m_iAssistCountdown = 3;
                ClearAssistForces();
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
            float	maxMotorImpulse = m_lpFriction->Coefficient()*0.032f*(m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());  
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
	Hinge::ResetSimulation();

    m_btHinge->getRotationalLimitMotor(0)->m_currentPosition = 0;
    m_btHinge->getRotationalLimitMotor(0)->m_accumulatedImpulse = 0;
}

void BlHinge::EnableFeedback()
{
    if(m_btHinge) m_btHinge->enableFeedback(true);
    GetSimulator()->AddToExtractExtraData(m_lpThisJoint);
}

bool BlHinge::NeedApplyAssist()
{
    //int i = 4;
    //if(GetSimulator()->Time() >= 1.1)
    //    i=5;

    if(m_btHinge && m_bMotorOn && m_lpBlParent && m_lpBlChild && m_btParent && m_btChild && m_lpAssistPid && m_lpAssistPid->Enabled())
    {
        float fltSetVel = SetVelocity();
        float fltPos = m_btHinge->getRotationalLimitMotor(0)->m_currentPosition;
        float fltLow = m_btHinge->getRotationalLimitMotor(0)->m_loLimit;
        float fltHigh = m_btHinge->getRotationalLimitMotor(0)->m_hiLimit;

        float fltLow1Perc = fabs(fltLow)*0.001;
        float fltHigh1Perc = fabs(fltHigh)*0.001;

        //If we are moving upwards and are not at upper limit then apply force.
        if(fltSetVel > 0 && fabs(fltHigh-fltPos)>=fltHigh1Perc)
            return true;

        //If we are moving downwards and are not at lower limit then apply force.
        if(fltSetVel < 0 && fabs(fltPos-fltLow)>=fltLow1Perc)
            return true;

        //If we get here then we should be applying assist forces, but we are at the limit, so clear the
        //vectors so we are not actually applying anything.
        ClearAssistForces();
    }

    return false;
}

void BlHinge::ApplyMotorAssist()
{
    ////If the motor is on and moving then give it an assisst if it is not making its velocity goal.
    //if(NeedApplyAssist())
    //{
    //    if(m_iAssistCountdown<=0)
    //    {
	   //     float fDisUnits = m_lpThisAB->GetSimulator()->InverseDistanceUnits();
	   //     float fMassUnits = m_lpThisAB->GetSimulator()->InverseMassUnits();
    //        float fltRatio = fMassUnits * fDisUnits;

    //        float fltDt = GetSimulator()->PhysicsTimeStep();
    //        float fltSetPoint = m_fltPredictedPos;
    //        float fltInput = m_lpThisJoint->JointPosition();
    //        float fltSetVel = SetVelocity();
    //        float fltVelDiffSign = (fltSetVel-m_lpThisJoint->JointVelocity());

    //        m_lpAssistPid->Setpoint(fltSetPoint);
    //        m_fltMotorAssistMagnitudeReport = fabs(m_lpAssistPid->Calculate(fltDt, fltInput)) * Std_Sign(fltVelDiffSign);
    //        float fltForceMag = m_fltMotorAssistMagnitudeReport * m_fltChildMassWithChildren;
    //        if(fltForceMag > m_fltMaxForceNotScaled)
    //            fltForceMag = m_fltMaxForceNotScaled;
    //        if(fltForceMag < -m_fltMaxForceNotScaled)
    //            fltForceMag = -m_fltMaxForceNotScaled;

    //        btVector3 vbtMotorAxis = m_btHinge->GetAngularForceAxis(0);
    //        
    //        CStdFPoint vMotorAxis(vbtMotorAxis[0], vbtMotorAxis[1], vbtMotorAxis[2]);
    //        CStdFPoint vBodyA = m_lpThisJoint->Parent()->AbsolutePosition();
    //        CStdFPoint vBodyB = m_lpThisJoint->Child()->AbsolutePosition();
    //        CStdFPoint vHinge = m_lpThisJoint->AbsolutePosition();

    //        CStdFPoint vBodyAxisA = (vHinge - vBodyA);
    //        vBodyAxisA.Normalize();
    //        vBodyAxisA = vMotorAxis ^ vBodyAxisA;

    //        CStdFPoint vBodyAxisB = (vHinge - vBodyB);
    //        vBodyAxisB.Normalize();
    //        vBodyAxisB = vMotorAxis ^ vBodyAxisB;

    //        CStdFPoint vBodyAForceReport = vBodyAxisA * -fltForceMag;
    //        CStdFPoint vBodyBForceReport = vBodyAxisB * fltForceMag;
    //        CStdFPoint vBodyAForce = vBodyAForceReport * fltRatio;
    //        CStdFPoint vBodyBForce = vBodyBForceReport * fltRatio;
    //        m_fltMotorAssistMagnitude = m_fltMotorAssistMagnitudeReport * fltRatio;

    //        m_vMotorAssistForceToA = vBodyAForce;
    //        m_vMotorAssistForceToB = vBodyBForce;
    //        m_vMotorAssistForceToAReport = vBodyAForceReport;
    //        m_vMotorAssistForceToBReport = vBodyBForceReport;

    //        btVector3 vbtMotorAForce(vBodyAForce.x, vBodyAForce.y, vBodyAForce.z);
    //        btVector3 vbtMotorBForce(vBodyBForce.x, vBodyBForce.y, vBodyBForce.z);

    //        m_btParent->applyCentralForce(vbtMotorAForce);
    //        m_btChild->applyCentralForce(vbtMotorBForce);
    //        m_iAssistCountdown = 0;
    //    }
    //    else
    //        m_iAssistCountdown--;
    //}
}

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
