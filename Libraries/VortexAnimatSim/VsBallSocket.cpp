// VsBallSocket.cpp: implementation of the VsBallSocket class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
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


void VsBallSocket::CreateJoint(Simulator *lpSim, Structure *lpStructure)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);
	VxAssembly *lpAssem = (VxAssembly *) lpStructure->Assembly();

	CStdFPoint vChildPos = lpVsChild->GetOSGWorldCoords();
	CStdFPoint vGlobal = vChildPos + m_lpThis->LocalPosition();
	CStdFPoint vLocalRot = m_lpThis->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
    VxVector3 axis((double) vLocalRot.x, (double) vLocalRot.y, (double) vLocalRot.z); 
	m_vxSocket = new VxBallAndSocket(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 

	//lpAssem->addConstraint(m_vxHinge);
	lpVsSim->Universe()->addConstraint(m_vxSocket);
	lpStructure->AddCollisionPair(m_lpParent->ID(), m_lpChild->ID());

	//m_vxSocket->setLowerLimit(m_vxHinge->kAngularCoordinate,m_fltConstraintLow, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxHinge->setUpperLimit(m_vxHinge->kAngularCoordinate, m_fltConstraintHigh, 0, m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxHinge->setLimitsActive(m_vxHinge->kAngularCoordinate, m_bEnableLimits);	

	m_vxJoint = m_vxSocket;
	m_iCoordID = m_vxSocket->kCoordinateAngular;
}

/*
float *VsBallSocket::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
}

void VsBallSocket::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	VsJoint::ResetSimulation(lpSim, lpStructure);
	BallSocket::ResetSimulation(lpSim, lpStructure);
}
*/
//void VsBallSocket::StepSimulation(Simulator *lpSim, Structure *lpStructure)
//{
//	SetVelocityToDesired();
//}

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
