// VsCylinder.cpp: implementation of the VsCylinder class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsCylinder.h"
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

VsCylinder::VsCylinder()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
}

VsCylinder::~VsCylinder()
{

}

//void VsCylinder::Selected(BOOL bValue, BOOL bSelectMultiple)  
//{
//	Cylinder::Selected(bValue, bSelectMultiple);
//	VsRigidBody::Selected(bValue, bSelectMultiple);
//}

void VsCylinder::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltRadius, m_fltRadius, 50, true, true, true);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = new VxCylinder(m_fltRadius, m_fltHeight);

	//Lets get the volume and areas
	m_fltVolume = 2*VX_PI*m_fltRadius*m_fltRadius*m_fltHeight;
	m_fltXArea = 2*m_fltRadius*m_fltHeight;
	m_fltYArea = 2*m_fltRadius*m_fltHeight;
	m_fltZArea = 2*VX_PI*m_fltRadius*m_fltRadius;

	VsRigidBody::CreateBody(lpSim, lpStructure);
	Cylinder::CreateParts(lpSim, lpStructure);
	VsRigidBody::SetBody(lpSim, lpStructure);
}

void VsCylinder::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint(lpSim, lpStructure);

	Cylinder::CreateJoints(lpSim, lpStructure);
	VsRigidBody::Initialize(lpSim, lpStructure);
}
/*
void VsCylinder::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	VsRigidBody::ResetSimulation(lpSim, lpStructure);
	Cylinder::ResetSimulation(lpSim, lpStructure);
}

void VsCylinder::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	Cylinder::StepSimulation(lpSim, lpStructure);
	VsRigidBody::CollectBodyData(lpSim);
}

float *VsCylinder::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	lpData = Cylinder::GetDataPointer(strDataType);
	if(lpData) return lpData;

	lpData = VsRigidBody::GetPhysicsDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

void VsCylinder::EnableCollision(Simulator *lpSim, RigidBody *lpBody)
{VsRigidBody::EnableCollision(lpSim, lpBody);}

void VsCylinder::DisableCollision(Simulator *lpSim, RigidBody *lpBody)
{VsRigidBody::DisableCollision(lpSim, lpBody);}

void VsCylinder::AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
{VsRigidBody::AddBodyForce(lpSim, fltPx, fltPy, fltPz, fltFx, fltFy, fltFz, bScaleUnits);}

void VsCylinder::AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz, BOOL bScaleUnits)
{VsRigidBody::AddBodyTorque(lpSim, fltTx, fltTy, fltTz, bScaleUnits);}

CStdFPoint VsCylinder::GetVelocityAtPoint(float x, float y, float z)
{return VsRigidBody::GetVelocityAtPoint(x, y, z);}

float VsCylinder::GetMass()
{return VsRigidBody::GetMass();}
*/

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
