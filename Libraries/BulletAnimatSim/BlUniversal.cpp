/**
\file	BlUniversal.cpp

\brief	Implements the vortex universal class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlSphere.h"
#include "BlSimulator.h"
#include "BlUniversal.h"


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
BlUniversal::BlUniversal()
{
	SetThisPointers();
 	m_btSocket = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
BlUniversal::~BlUniversal()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlUniversal/\r\n", "", -1, false, true);}
}

void BlUniversal::SetupPhysics()
{
    if(m_btJoint)
		DeletePhysics(false);

    InitBaseJointPointers(m_lpParent, m_lpChild, m_aryRelaxations, -1);

    btTransform mtJointRelParent, mtJointRelChild;
    CalculateRelativeJointMatrices(mtJointRelParent, mtJointRelChild);

	m_btSocket = new btConeTwistConstraint(*m_lpBlParent->Part(), *m_lpBlChild->Part(), mtJointRelParent, mtJointRelChild); 

    GetBlSimulator()->DynamicsWorld()->addConstraint(m_btSocket, true);
    m_btSocket->setDbgDrawSize(btScalar(5.f));

    if(m_lpBlParent && m_lpBlParent->Part())
        m_lpBlParent->Part()->setSleepingThresholds(0, 0);

    if(m_lpBlChild && m_lpBlChild->Part())
        m_lpBlChild->Part()->setSleepingThresholds(0, 0);

	m_btJoint = m_btSocket;

    BallSocket::Initialize();
    BlJoint::Initialize();
}

void BlUniversal::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

void BlUniversal::Physics_ResetSimulation()
{
    BlJoint::Physics_ResetSimulation();

    if(m_btSocket)
        m_btSocket->internalSetAppliedImpulse(0);
}

#pragma region DataAccesMethods


bool BlUniversal::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(BallSocket::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlUniversal::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	BallSocket::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
