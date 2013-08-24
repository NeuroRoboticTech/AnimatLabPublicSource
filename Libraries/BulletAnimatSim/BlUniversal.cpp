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
    //FIX PHYSICS
	//m_vxSocket = NULL;
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
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlUniversal/\r\n", "", -1, false, true);}
}

void BlUniversal::DeletePhysics()
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

void BlUniversal::SetupPhysics()
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
	//VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(true), vMT);
	//Vx::VxTransform vTrans(vMT);
	//Vx::VxReal3 vxRot;
	//vTrans.getRotationEulerAngles(vxRot);

	//CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]); //= m_lpThisMI->Rotation();

 //   VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	//osg::Vec3d vNormAxis = NormalizeAxis(vLocalRot);
	//VxVector3 axis((double) vNormAxis[0], (double) vNormAxis[1], (double) vNormAxis[2]);

	////m_vxSocket = new VxUniversal(lpVsParent->Part(), lpVsChild->Part(), pos.v, VxVector3(1, 0, 0), VxVector3(0, 1, 0)); 
	//m_vxSocket = new VxHomokinetic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 
	//m_vxSocket->setName(m_strID.c_str());

	//GetBlSimulator()->Universe()->addConstraint(m_vxSocket);

	////Disable collisions between this object and its parent
	//m_lpChild->DisableCollision(m_lpParent);

	//m_vxJoint = m_vxSocket;
	//m_iCoordID = -1; //Not used fo
}

void BlUniversal::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods


bool BlUniversal::SetData(const string &strDataType, const string &strValue, bool bThrowError)
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

void BlUniversal::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	BallSocket::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
