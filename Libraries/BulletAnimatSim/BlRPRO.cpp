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
    //FIX PHYSICS
	//m_vxSocket = NULL;
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
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlRPRO\r\n", "", -1, false, true);}
}

void BlRPRO::DeletePhysics()
{
    //FIX PHYSICS
	//if(!m_vxSocket)
	//	return;

	//if(GetBlSimulator() && GetBlSimulator()->Universe())
	//{
	//	GetBlSimulator()->Universe()->removeConstraint(m_vxSocket);
	//	delete m_vxSocket;

	//	if(m_lpChild && m_lpParent)
	//		m_lpChild->EnableCollision(m_lpParent);
	//}

	//m_vxSocket = NULL;
	//m_vxJoint = NULL;
}

void BlRPRO::SetupPhysics()
{
    //FIX PHYSICS
	//if(m_vxSocket)
	//	DeletePhysics();

	//if(!m_lpParent)
	//	THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	//if(!m_lpChild)
	//	THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	//BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpParent);
	//if(!lpVsParent)
	//	THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	//BlRigidBody *lpVsChild = dynamic_cast<BlRigidBody *>(m_lpChild);
	//if(!lpVsChild)
	//	THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	//CStdFPoint vGlobal = this->GetOSGWorldCoords();
	//
	//Vx::VxReal44 vMT;
	//VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(), vMT);
	//Vx::VxTransform vTrans(vMT);

 //   VxVector3 vPos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 

	//m_vxSocket = new VxRPRO();
	//m_vxSocket->setName(m_strID.c_str());
	//m_vxSocket->setParts(lpVsParent->Part(), lpVsChild->Part());
	//m_vxSocket->setPosition(vPos);

	//VxReal3 aryStrength;
	//aryStrength[0] = aryStrength[1] = aryStrength[2] = VX_INFINITY;

	//Vx::VxQuaternion q;
	//m_vxSocket->setRelativeQuaternionFromPart();

	//m_vxSocket->setLinearStrength(aryStrength);
	//m_vxSocket->setAngularStrength(aryStrength);

	//GetBlSimulator()->Universe()->addConstraint(m_vxSocket);

	////Disable collisions between this object and its parent
	//m_lpChild->DisableCollision(m_lpParent);

	//m_vxJoint = m_vxSocket;
	//m_iCoordID = -1; //Not used fo
}

void BlRPRO::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

bool BlRPRO::SetData(const string &strDataType, const string &strValue, bool bThrowError)
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

void BlRPRO::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	RPRO::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
