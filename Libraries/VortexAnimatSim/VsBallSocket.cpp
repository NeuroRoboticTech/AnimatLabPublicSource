// VsBallSocket.cpp: implementation of the VsBallSocket class.
//
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsBallSocket::VsBallSocket()
{
	m_lpThis = this;
	m_lpThisJoint = this;
	m_lpPhysicsBody = this;
	m_vxSocket = NULL;
}

VsBallSocket::~VsBallSocket()
{
}

//void VsBallSocket::Selected(BOOL bValue, BOOL bSelectMultiple) 
//{
//	BallSocket::Selected(bValue, bSelectMultiple);
//	VsJoint::Selected(bValue, bSelectMultiple);
//}

void VsBallSocket::SetVelocityToDesired()
{
}

void VsBallSocket::EnableMotor(BOOL bVal)
{
}


void VsBallSocket::CreateJoint()
{
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

	CStdFPoint vChildPos = lpVsChild->GetOSGWorldCoords();
	CStdFPoint vGlobal = vChildPos + m_lpThis->LocalPosition();
	CStdFPoint vLocalRot = m_lpThis->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
    VxVector3 axis((double) vLocalRot.x, (double) vLocalRot.y, (double) vLocalRot.z); 
	m_vxSocket = new VxBallAndSocket(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 

	lpVsSim->Universe()->addConstraint(m_vxSocket);
	m_lpStructure->AddCollisionPair(m_lpParent->ID(), m_lpChild->ID());

	m_vxJoint = m_vxSocket;
	m_iCoordID = m_vxSocket->kCoordinateAngular;
}

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
