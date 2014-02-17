/**
\file	BlRPRO.cpp

\brief	Implements the vortex ball socket class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlRPRO.h"
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
BlRPRO::BlRPRO()
{
	SetThisPointers();
	//m_btSocket = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
BlRPRO::~BlRPRO()
{

	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlRPRO\r\n", "", -1, false, true);}
}

void BlRPRO::SetupPhysics()
{
	if(m_btJoint)
		DeletePhysics(false);

    InitBaseJointPointers(m_lpParent, m_lpChild, m_aryRelaxations, -1);

    btTransform mtJointRelParent, mtJointRelChild;
    CalculateRelativeJointMatrices(mtJointRelParent, mtJointRelChild);

    //All limits are turned off by default.
	m_btSocket = new btAnimatGeneric6DofConstraint(*m_lpBlParent->Part(), *m_lpBlChild->Part(), mtJointRelParent, mtJointRelChild, true); 
    m_btSocket->setLinearLowerLimit(btVector3(0, 0, 0));
    m_btSocket->setLinearUpperLimit(btVector3(0, 0, 0));
    m_btSocket->setAngularLowerLimit(btVector3(0, 0, 0));
    m_btSocket->setAngularUpperLimit(btVector3(0, 0, 0));

    GetBlSimulator()->DynamicsWorld()->addConstraint(m_btSocket, true);
    m_btSocket->setDbgDrawSize(btScalar(5.f));

	m_btJoint = m_btSocket;
}

void BlRPRO::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

bool BlRPRO::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(RPRO::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlRPRO::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	RPRO::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
