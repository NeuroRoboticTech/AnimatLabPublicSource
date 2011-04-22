/**
\file	VsBallSocket.cpp

\brief	Implements the vortex ball socket class.
**/

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsBallSocket.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
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
VsBallSocket::VsBallSocket()
{
	m_lpThis = this;
	m_lpThisJoint = this;
	PhysicsBody(this);
	m_vxSocket = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
VsBallSocket::~VsBallSocket()
{
}

void VsBallSocket::ResetGraphicsAndPhysics()
{
	VsBody::BuildLocalMatrix();

	SetupPhysics();	
}

void VsBallSocket::DeletePhysics()
{
	if(!m_vxSocket)
		return;

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	lpVsSim->Universe()->removeConstraint(m_vxSocket);
	delete m_vxSocket;
	m_vxSocket = NULL;
	m_vxJoint = NULL;

	m_lpChild->EnableCollision(m_lpParent);
}

void VsBallSocket::SetupPhysics()
{
	if(m_vxSocket)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VxAssembly *lpAssem = (VxAssembly *) m_lpStructure->Assembly();

	CStdFPoint vGlobal = this->GetOSGWorldCoords();
	
	Vx::VxReal44 vMT;
	VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(), vMT);
	Vx::VxTransform vTrans(vMT);
	Vx::VxReal3 vxRot;
	vTrans.getRotationEulerAngles(vxRot);

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]); //= m_lpThis->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	VxVector3 axis = NormalizeAxis(vLocalRot);

	m_vxSocket = new VxBallAndSocket(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 
	m_vxSocket->setName(m_strID.c_str());

	//lpAssem->addConstraint(m_vxHinge);
	lpVsSim->Universe()->addConstraint(m_vxSocket);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	m_vxJoint = m_vxSocket;
	m_iCoordID = -1; //Not used fo
}

void VsBallSocket::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

BOOL VsBallSocket::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(VsJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(BallSocket::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
