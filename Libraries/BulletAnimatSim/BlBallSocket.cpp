/**
\file	BlBallSocket.cpp

\brief	Implements the vortex ball socket class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlBallSocket.h"
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
BlBallSocket::BlBallSocket()
{
	SetThisPointers();
	m_btSocket = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
BlBallSocket::~BlBallSocket()
{

	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlBallSocket\r\n", "", -1, false, true);}
}

void BlBallSocket::SetupPhysics()
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

	m_btSocket = new btConeTwistConstraint(*m_lpBlParent->Part(), *m_lpBlChild->Part(), mtJointRelParent, mtJointRelChild); 

    GetBlSimulator()->DynamicsWorld()->addConstraint(m_btSocket, true);
    m_btSocket->setDbgDrawSize(btScalar(5.f));

	m_btJoint = m_btSocket;
}

void BlBallSocket::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

bool BlBallSocket::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
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

void BlBallSocket::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	BallSocket::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
