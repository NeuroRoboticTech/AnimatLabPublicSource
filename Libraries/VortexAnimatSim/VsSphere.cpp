// VsSphere.cpp: implementation of the VsSphere class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsSphere::VsSphere()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
	m_cgSphere = NULL;
}

VsSphere::~VsSphere()
{

}
//
//void VsSphere::Selected(BOOL bValue, BOOL bSelectMultiple)  
//{
//	Sphere::Selected(bValue, bSelectMultiple);
//	VsRigidBody::Selected(bValue, bSelectMultiple);
//}

void VsSphere::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	m_osgGeometry = CreateSphereGeometry(50, 50, m_fltRadius);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = new VxSphere(m_fltRadius);

	//Lets get the volume and areas
	m_fltVolume = 1.33333*VX_PI*m_fltRadius*m_fltRadius*m_fltRadius;
	m_fltXArea = 2*VX_PI*m_fltRadius*m_fltRadius;
	m_fltYArea = m_fltXArea;
	m_fltZArea = m_fltXArea;

	VsRigidBody::CreateBody(lpSim, lpStructure);
	Sphere::CreateParts(lpSim, lpStructure);
	VsRigidBody::SetBody(lpSim, lpStructure);
}

void VsSphere::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint(lpSim, lpStructure);

	Sphere::CreateJoints(lpSim, lpStructure);
	VsRigidBody::Initialize(lpSim, lpStructure);
}
//
//void VsSphere::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
//{
//	VsRigidBody::ResetSimulation(lpSim, lpStructure);
//	Sphere::ResetSimulation(lpSim, lpStructure);
//}
//
//void VsSphere::StepSimulation(Simulator *lpSim, Structure *lpStructure)
//{
//	Sphere::StepSimulation(lpSim, lpStructure);
//	VsRigidBody::CollectBodyData(lpSim);
//}
//
//void VsSphere::EnableCollision(Simulator *lpSim, RigidBody *lpBody)
//{VsRigidBody::EnableCollision(lpSim, lpBody);}
//
//void VsSphere::DisableCollision(Simulator *lpSim, RigidBody *lpBody)
//{VsRigidBody::DisableCollision(lpSim, lpBody);}
//
//float *VsSphere::GetDataPointer(string strDataType)
//{
//	string strType = Std_CheckString(strDataType);
//	float *lpData = NULL;
//
//	lpData = Sphere::GetDataPointer(strDataType);
//	if(lpData) return lpData;
//
//	lpData = VsRigidBody::GetPhysicsDataPointer(strDataType);
//	if(lpData) return lpData;
//
//	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);
//
//	return NULL;
//}
//
//void VsSphere::AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
//{VsRigidBody::AddBodyForce(lpSim, fltPx, fltPy, fltPz, fltFx, fltFy, fltFz, bScaleUnits);}
//
//void VsSphere::AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz, BOOL bScaleUnits)
//{VsRigidBody::AddBodyTorque(lpSim, fltTx, fltTy, fltTz, bScaleUnits);}
//
//CStdFPoint VsSphere::GetVelocityAtPoint(float x, float y, float z)
//{return VsRigidBody::GetVelocityAtPoint(x, y, z);}
//
//float VsSphere::GetMass()
//{return VsRigidBody::GetMass();}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
