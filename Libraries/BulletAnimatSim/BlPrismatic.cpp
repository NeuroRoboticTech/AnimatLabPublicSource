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
        GetLimitsFromRelaxations(m_vLowerLinear, m_vUpperLinear, m_vLowerAngular, m_vUpperAngular);

        float fltErp = 0, fltCfm=0;

        if(m_bEnableLimits)
        {
            m_bJointLocked = false;

            m_vLowerLinear[0] = m_lpLowerLimit->LimitPos();
            m_vUpperLinear[0] = m_lpUpperLimit->LimitPos();

            float fltKp = m_lpUpperLimit->Stiffness();
            float fltKd = m_lpUpperLimit->Damping();
            float fltH = m_lpSim->PhysicsTimeStep()*1000;
            
            fltErp = (fltH*fltKp)/((fltH*fltKp) + fltKd);
            fltCfm = 1/((fltH*fltKp) + fltKd);
        }
        else
        {
            //To disable limits in bullet we need the lower limit to be bigger than the upper limit
            m_bJointLocked = false;

            m_vLowerLinear[0] = 1;
            m_vUpperLinear[0] = -1;
        }

        m_btPrismatic->setParam(BT_CONSTRAINT_STOP_CFM, fltCfm, 0);
        m_btPrismatic->setParam(BT_CONSTRAINT_STOP_ERP, fltErp, 0);

        float fltBounce = m_lpUpperLimit->Restitution();
        float fltHigh = m_btPrismatic->getTranslationalLimitMotor()->m_restitution = fltBounce;

        m_btPrismatic->setLinearLowerLimit(m_vLowerLinear);
        m_btPrismatic->setLinearUpperLimit(m_vUpperLinear);

		m_btPrismatic->setAngularLowerLimit(m_vLowerAngular);
		m_btPrismatic->setAngularUpperLimit(m_vUpperAngular);
    }
}

void BlPrismatic::TimeStepModified()
{
	Prismatic::TimeStepModified();
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

    InitBaseJointPointers(m_lpParent, m_lpChild, m_aryRelaxations, 0);

    btTransform mtJointRelParent, mtJointRelChild;
	CStdFPoint vRot(0, 0, osg::PI);
    CalculateRelativeJointMatrices(vRot, mtJointRelParent, mtJointRelChild);

	m_btPrismatic = new btAnimatGeneric6DofConstraint(*m_lpBlParent->Part(), *m_lpBlChild->Part(), mtJointRelParent, mtJointRelChild, false); 

    GetBlSimulator()->DynamicsWorld()->addConstraint(m_btPrismatic, true);
    m_btPrismatic->setDbgDrawSize(btScalar(5.f));

	BlPrismaticLimit *lpUpperLimit = dynamic_cast<BlPrismaticLimit *>(m_lpUpperLimit);
	BlPrismaticLimit *lpLowerLimit = dynamic_cast<BlPrismaticLimit *>(m_lpLowerLimit);

	m_btJoint = m_btPrismatic;
    m_bt6DofJoint = m_btPrismatic;

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	//If the motor is enabled then it will start out with a velocity of	zero.
	EnableMotor(m_bEnableMotorInit);

    //Turn off sleeping thresholds for parent and child of prismatic joints to prevent the parts from
    //falling asleep. If it does the joint motor has a tendency to not work.
    if(m_lpBlParent && m_lpBlParent->Part())
        m_lpBlParent->Part()->setSleepingThresholds(0, 0);

    if(m_lpBlChild && m_lpBlChild->Part())
        m_lpBlChild->Part()->setSleepingThresholds(0, 0);

    m_btPrismatic->setJointFeedback(&m_btJointFeedback);

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
	else if(strType == "JOINTDESIREDPOSITION")
		return &m_fltReportSetPosition;
	else if(strType == "JOINTSETPOSITION")
		return &m_fltReportSetPosition;
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

		lpData = Prismatic::GetDataPointer(strType);
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

void BlPrismatic::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	BlJoint::Physics_QueryProperties(aryProperties);
	Prismatic::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("JointRotation", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointPosition", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointActualVelocity", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointForce", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointRotationDeg", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointDesiredPosition", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointSetPosition", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointDesiredVelocity", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("JointSetVelocity", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Enable", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));
}

#pragma endregion

void BlPrismatic::StepSimulation()
{ 
    ////Test code
    //int iTest = 0;
    //if(m_lpSim->Time() >= 1.02)
    //    iTest = 1;

    UpdateData();
	SetVelocityToDesired();
    ApplyMotorAssist();
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

            m_vLowerLinear[0] = m_vUpperLinear[0] = fltPosition;

		    m_btPrismatic->setLinearLowerLimit(m_vLowerLinear);
		    m_btPrismatic->setLinearUpperLimit(m_vUpperLinear);

            m_btPrismatic->enableSpring(0, false);
            m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = true;
		    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0] = 0;
		    m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = fltMaxLockForce;
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
           //if(Std_ToLower(m_lpThisJoint->ID()) == "61cbf08d-4625-4b9f-87cd-d08b778cf04e" && GetSimulator()->Time() >= 1.01)
           //     bOn = bOn;  //Testing

			//I had to cut this if statement out. I kept running into one instance after another where I ran inot a problem if I did not do this every single time.
			// It is really annoying and inefficient, but I cannot find another way to reiably guarantee that the motor will behave coorectly under all conditions without
			// doing this every single time I set the motor velocity.
            //if(!m_bMotorOn || bForceWakeup || m_bJointLocked || JointIsLocked() || fabs(m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0]) < 1e-4)
            //{    
                SetLimitValues();
                m_lpThisJoint->WakeDynamics();
            //}

            m_btPrismatic->enableSpring(0, false);
		    m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = true;
		    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0] = -fltDesiredVelocity;
		    m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = fltMaxForce;
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
        if(m_aryBlRelaxations[0] && m_aryBlRelaxations[0]->Enabled())
        {
            //If we have the spring enabled then use that.
            m_btPrismatic->enableSpring(0, true);
        }
        else if(m_lpFriction && m_lpFriction->Enabled())
        {
            //Otherwise if we have friction enable use it.

            //0.032 is a coefficient that produces friction behavior in bullet using the same coefficient values
            //that were specified in vortex engine. This way I get similar behavior between the two.
            float	maxMotorImpulse = m_lpFriction->Coefficient()*0.032f*(m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());  
            m_btPrismatic->enableSpring(0, false);
		    m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = true;
		    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity[0] = 0;
		    m_btPrismatic->getTranslationalLimitMotor()->m_maxMotorForce[0] = maxMotorImpulse;
        }
        else //Otherwise just turn the motor off
        {
            m_btPrismatic->enableSpring(0, false);
		    m_btPrismatic->getTranslationalLimitMotor()->m_enableMotor[0] = false;
        }

        m_btPrismatic->getTranslationalLimitMotor()->m_accumulatedImpulse = btVector3(0, 0, 0);
        m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity = btVector3(0, 0, 0);
        m_btPrismatic->getTranslationalLimitMotor()->m_currentLimit[0] = 0;
        m_btPrismatic->getTranslationalLimitMotor()->m_currentLimit[1] = 0;
        m_btPrismatic->getTranslationalLimitMotor()->m_currentLimit[2] = 0;
        m_btPrismatic->getTranslationalLimitMotor()->m_currentLimitError = btVector3(0, 0, 0);
    }
}

void BlPrismatic::AxisConstraintSpringEnableChanged(bool bEnabled)
{
    if(!m_bMotorOn)
        m_btPrismatic->enableSpring(0, bEnabled);
}

void BlPrismatic::SetConstraintFriction()
{
    if(m_btPrismatic && !m_bJointLocked && !m_bMotorOn && m_bEnabled)
        TurnMotorOff();
}

void BlPrismatic::ResetSimulation()
{
	Prismatic::ResetSimulation();

    for(int iIdx=0; iIdx<3; iIdx++)
    {
        m_btPrismatic->getRotationalLimitMotor(iIdx)->m_currentPosition = 0;
        m_btPrismatic->getRotationalLimitMotor(iIdx)->m_accumulatedImpulse = 0;
        m_btPrismatic->getRotationalLimitMotor(iIdx)->m_currentLimit = 0;
        m_btPrismatic->getRotationalLimitMotor(iIdx)->m_currentLimitError = 0;
        m_btPrismatic->getRotationalLimitMotor(iIdx)->m_targetVelocity = 0;
    }

    m_btPrismatic->getTranslationalLimitMotor()->m_currentLinearDiff = btVector3(0, 0, 0);
    m_btPrismatic->getTranslationalLimitMotor()->m_accumulatedImpulse = btVector3(0, 0, 0);
    m_btPrismatic->getTranslationalLimitMotor()->m_targetVelocity = btVector3(0, 0, 0);
    m_btPrismatic->getTranslationalLimitMotor()->m_currentLimit[0] = 0;
    m_btPrismatic->getTranslationalLimitMotor()->m_currentLimit[1] = 0;
    m_btPrismatic->getTranslationalLimitMotor()->m_currentLimit[2] = 0;
    m_btPrismatic->getTranslationalLimitMotor()->m_currentLimitError = btVector3(0, 0, 0);
}

void BlPrismatic::EnableFeedback()
{
    if(m_btPrismatic) m_btPrismatic->enableFeedback(true);
    GetSimulator()->AddToExtractExtraData(m_lpThisJoint);
}

bool BlPrismatic::NeedApplyAssist()
{
    //int i = 4;
    //if(GetSimulator()->Time() >= 1.1)
    //    i=5;

    if(m_btPrismatic && m_bMotorOn && m_lpBlParent && m_lpBlChild && m_btParent && m_btChild && m_lpAssistPid && m_lpAssistPid->Enabled())
    {
        float fltSetVel = SetVelocity();
        float fltPos = m_btPrismatic->getTranslationalLimitMotor()->m_currentLinearDiff[0];
        float fltLow = m_btPrismatic->getTranslationalLimitMotor()->m_lowerLimit[0];
        float fltHigh = m_btPrismatic->getTranslationalLimitMotor()->m_upperLimit[0];

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

void BlPrismatic::ApplyMotorAssist()
{
    //If the motor is on and moving then give it an assisst if it is not making its velocity goal.
    if(NeedApplyAssist())
    {
        if(m_iAssistCountdown<=0)
        {
	        float fDisUnits = m_lpThisAB->GetSimulator()->InverseDistanceUnits();
	        float fMassUnits = m_lpThisAB->GetSimulator()->InverseMassUnits();
            float fltRatio = fMassUnits * fDisUnits;

            float fltDt = GetSimulator()->PhysicsTimeStep();
            float fltSetPoint = SetVelocity();
            float fltInput = m_lpThisJoint->JointVelocity() * fDisUnits;

            m_lpAssistPid->Setpoint(fltSetPoint);
            m_fltMotorAssistMagnitude = m_lpAssistPid->Calculate(fltDt, fltInput);
            float fltForceMag = m_fltMotorAssistMagnitude;
            if(fltForceMag > m_fltMaxForceNotScaled)
                fltForceMag = m_fltMaxForceNotScaled;
            if(fltForceMag < -m_fltMaxForceNotScaled)
                fltForceMag = -m_fltMaxForceNotScaled;
       
            btVector3 vMotorAxis = m_btPrismatic->GetLinearForceAxis(0);

            btVector3 vBodyAForceReport = -fltForceMag * vMotorAxis;
            btVector3 vBodyBForceReport = fltForceMag * vMotorAxis;
            btVector3 vBodyAForce = fltRatio * vBodyAForceReport;
            btVector3 vBodyBForce = fltRatio * vBodyBForceReport;
            m_fltMotorAssistMagnitudeReport = m_fltMotorAssistMagnitude * fltRatio;

            for(int i=0; i<3; i++)
            {
                m_vMotorAssistForceToA[i] = vBodyAForce[i];
                m_vMotorAssistForceToB[i] = vBodyBForce[i];
                m_vMotorAssistForceToAReport[i] = vBodyAForceReport[i];
                m_vMotorAssistForceToBReport[i] = vBodyBForceReport[i];
            }

            m_btParent->applyCentralForce(vBodyAForce);
            m_btChild->applyCentralForce(vBodyBForce);
            m_iAssistCountdown = 0;
        }
        else
            m_iAssistCountdown--;
    }
}

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
