/**
\file	VsRPRO.cpp

\brief	Implements the vortex ball socket class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsRPRO.h"
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
VsRPRO::VsRPRO()
{
	SetThisPointers();
	m_vxSocket = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
VsRPRO::~VsRPRO()
{

	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsRPRO\r\n", "", -1, FALSE, TRUE);}
}

void VsRPRO::DeletePhysics()
{
	if(!m_vxSocket)
		return;

	if(GetVsSimulator() && GetVsSimulator()->Universe())
	{
		GetVsSimulator()->Universe()->removeConstraint(m_vxSocket);
		delete m_vxSocket;

		if(m_lpChild && m_lpParent)
			m_lpChild->EnableCollision(m_lpParent);
	}

	m_vxSocket = NULL;
	m_vxJoint = NULL;
}

void VsRPRO::SetupPhysics()
{
	if(m_vxSocket)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

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

    VxVector3 vPos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 

	m_vxSocket = new VxRPRO();
	m_vxSocket->setName(m_strID.c_str());
	m_vxSocket->setParts(lpVsParent->Part(), lpVsChild->Part());
	m_vxSocket->setPosition(vPos);

	VxReal3 aryStrength;
	aryStrength[0] = aryStrength[1] = aryStrength[2] = VX_INFINITY;

	Vx::VxQuaternion q;
	m_vxSocket->setRelativeQuaternionFromPart();

	m_vxSocket->setLinearStrength(aryStrength);
	m_vxSocket->setAngularStrength(aryStrength);

	GetVsSimulator()->Universe()->addConstraint(m_vxSocket);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	m_vxJoint = m_vxSocket;
	m_iCoordID = -1; //Not used fo
}

void VsRPRO::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

BOOL VsRPRO::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(VsJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(RPRO::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void VsRPRO::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	VsJoint::Physics_QueryProperties(aryNames, aryTypes);
	RPRO::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
