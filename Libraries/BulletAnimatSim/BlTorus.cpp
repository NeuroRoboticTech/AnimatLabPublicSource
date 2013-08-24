/**
\file	BlTorus.cpp

\brief	Implements the vortex Torus class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlRigidBody.h"
#include "BlTorus.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor.

\author	dcofer
\date	6/12/2011
**/
BlTorus::BlTorus()
{
	SetThisPointers();
}

BlTorus::~BlTorus()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlTorus/\r\n", "", -1, false, true);}
}

void BlTorus::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateTorusGeometry(m_fltInsideRadius, m_fltOutsideRadius, m_iSides, m_iRings);
}

void BlTorus::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
		//For the moment build a test node to generate the mesh from.
		osg::ref_ptr<osg::Geometry> osgGeometry = CreateTorusGeometry(m_fltInsideRadius, m_fltOutsideRadius, m_iSides, m_iRings);
		osg::ref_ptr<osg::Geode> osgNode = new osg::Geode;
		osgNode->addDrawable(m_osgGeometry.get());

        //FIX PHYSICS
		//m_vxGeometry = GetBlSimulator()->CreatTriangleMeshFromOsg(osgNode.get());
	}
}

void BlTorus::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Torus::CreateParts();
	BlRigidBody::SetBody();
}

void BlTorus::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Torus::CreateJoints();
	BlRigidBody::Initialize();
}

void BlTorus::ResizePhysicsGeometry()
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
