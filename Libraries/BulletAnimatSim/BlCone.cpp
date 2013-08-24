// BlCone.cpp: implementation of the BlCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlRigidBody.h"
#include "BlCone.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlCone::BlCone()
{
	SetThisPointers();
}

BlCone::~BlCone()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlCone\r\n", "", -1, false, true);}
}

void BlCone::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, m_iSides, true, true, true);
}

void BlCone::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
		//For the moment build a test node to generate the mesh from.
		osg::ref_ptr<osg::Geometry> osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, m_iSides, true, true, true);
		osg::ref_ptr<osg::Geode> osgNode = new osg::Geode;
		osgNode->addDrawable(m_osgGeometry.get());

         //FIX PHYSICS
		//m_vxGeometry = VxConvexMesh::createFromNode(osgNode.get());
	}
}

void BlCone::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Cone::CreateParts();
	BlRigidBody::SetBody();
}

void BlCone::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cone::CreateJoints();
	BlRigidBody::Initialize();
}

void BlCone::ResizePhysicsGeometry()
{
     //FIX PHYSICS
	//if(m_vxGeometry && m_vxCollisionGeometry && m_vxSensor)
	//{
	//	if(!m_vxSensor->removeCollisionGeometry(m_vxCollisionGeometry))
	//		THROW_PARAM_ERROR(Bl_Err_lRemovingCollisionGeometry, Bl_Err_strRemovingCollisionGeometry, "ID: ", m_strID);

	//	delete m_vxCollisionGeometry;
	//	m_vxCollisionGeometry = NULL;

	//	CreatePhysicsGeometry();
	//	int iMaterialID = m_lpSim->GetMaterialID(MaterialID());
	//	CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisRB->Density()));
	//}
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
