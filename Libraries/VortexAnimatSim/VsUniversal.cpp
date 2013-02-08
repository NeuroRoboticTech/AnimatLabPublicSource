/**
\file	VsUniversal.cpp

\brief	Implements the vortex universal class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"
#include "VsUniversal.h"


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
VsUniversal::VsUniversal()
{
	SetThisPointers();
	m_vxSocket = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
VsUniversal::~VsUniversal()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsUniversal/\r\n", "", -1, FALSE, TRUE);}
}

void VsUniversal::DeletePhysics()
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

void VsUniversal::SetupPhysics()
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
	VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(TRUE), vMT);
	Vx::VxTransform vTrans(vMT);
	Vx::VxReal3 vxRot;
	vTrans.getRotationEulerAngles(vxRot);

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]); //= m_lpThisMI->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	VxVector3 axis = NormalizeAxis(vLocalRot);

	//m_vxSocket = new VxUniversal(lpVsParent->Part(), lpVsChild->Part(), pos.v, VxVector3(1, 0, 0), VxVector3(0, 1, 0)); 
	m_vxSocket = new VxHomokinetic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 
	m_vxSocket->setName(m_strID.c_str());

	//lpAssem->addConstraint(m_vxHinge);
	GetVsSimulator()->Universe()->addConstraint(m_vxSocket);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	m_vxJoint = m_vxSocket;
	m_iCoordID = -1; //Not used fo
}

void VsUniversal::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods


BOOL VsUniversal::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
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

void VsUniversal::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	VsJoint::Physics_QueryProperties(aryNames, aryTypes);
	BallSocket::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
