// VsCone.cpp: implementation of the VsCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsRigidBody.h"
#include "VsCone.h"
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

VsCone::VsCone()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
}

VsCone::~VsCone()
{

}

//void VsCone::Selected(BOOL bValue, BOOL bSelectMultiple)  
//{
//	Cone::Selected(bValue, bSelectMultiple);
//	VsRigidBody::Selected(bValue, bSelectMultiple);
//}

void VsCone::CreateParts()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, 50, true, true, true);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());

	m_osgNode = osgGroup;
	m_vxGeometry = VxConvexMesh::createFromNode(m_osgNode.get());

	//Lets get the volume and areas
	m_fltVolume = 2*VX_PI*m_fltLowerRadius*m_fltLowerRadius*m_fltHeight;
	m_fltXArea = 2*m_fltLowerRadius*m_fltHeight;
	m_fltYArea = 2*m_fltLowerRadius*m_fltHeight;
	m_fltZArea = 2*VX_PI*m_fltLowerRadius*m_fltLowerRadius;

	VsRigidBody::CreateBody();
	Cone::CreateParts();
	VsRigidBody::SetBody();
}

void VsCone::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cone::CreateJoints();
	VsRigidBody::Initialize();
}
/*
void VsCone::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	VsRigidBody::ResetSimulation(lpSim, lpStructure);
	Cone::ResetSimulation(lpSim, lpStructure);
}

void VsCone::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	Cone::StepSimulation(lpSim, lpStructure);
	VsRigidBody::CollectBodyData(lpSim);
}

float *VsCone::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	lpData = Cone::GetDataPointer(strDataType);
	if(lpData) return lpData;

	lpData = VsRigidBody::GetPhysicsDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

void VsCone::EnableCollision(Simulator *lpSim, RigidBody *lpBody)
{VsRigidBody::EnableCollision(lpSim, lpBody);}

void VsCone::DisableCollision(Simulator *lpSim, RigidBody *lpBody)
{VsRigidBody::DisableCollision(lpSim, lpBody);}

void VsCone::AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
{VsRigidBody::AddBodyForce(lpSim, fltPx, fltPy, fltPz, fltFx, fltFy, fltFz, bScaleUnits);}

void VsCone::AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz, BOOL bScaleUnits)
{VsRigidBody::AddBodyTorque(lpSim, fltTx, fltTy, fltTz, bScaleUnits);}

CStdFPoint VsCone::GetVelocityAtPoint(float x, float y, float z)
{return VsRigidBody::GetVelocityAtPoint(x, y, z);}

float VsCone::GetMass()
{return VsRigidBody::GetMass();}
*/

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
