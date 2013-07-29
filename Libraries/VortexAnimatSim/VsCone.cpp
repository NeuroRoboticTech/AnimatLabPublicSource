// VsCone.cpp: implementation of the VsCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsCone.h"
#include "VsSimulator.h"

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
	SetThisPointers();
}

VsCone::~VsCone()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsCone\r\n", "", -1, FALSE, TRUE);}
}

void VsCone::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, m_iSides, true, true, true);
}

void VsCone::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
		//For the moment build a test node to generate the mesh from.
		osg::ref_ptr<osg::Geometry> osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, m_iSides, true, true, true);
		osg::ref_ptr<osg::Geode> osgNode = new osg::Geode;
		osgNode->addDrawable(m_osgGeometry.get());

		m_vxGeometry = VxConvexMesh::createFromNode(osgNode.get());
	}
}

void VsCone::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
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

void VsCone::ResizePhysicsGeometry()
{
	if(m_vxGeometry && m_vxCollisionGeometry && m_vxSensor)
	{
		if(!m_vxSensor->removeCollisionGeometry(m_vxCollisionGeometry))
			THROW_PARAM_ERROR(Vs_Err_lRemovingCollisionGeometry, Vs_Err_strRemovingCollisionGeometry, "ID: ", m_strID);

		delete m_vxCollisionGeometry;
		m_vxCollisionGeometry = NULL;

		CreatePhysicsGeometry();
		int iMaterialID = m_lpSim->GetMaterialID(MaterialID());
		CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisRB->Density()));
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
