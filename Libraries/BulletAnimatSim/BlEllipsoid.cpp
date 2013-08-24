/**
\file	BlEllipsoid.cpp

\brief	Implements the vortex ellipsoid class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlRigidBody.h"
#include "BlEllipsoid.h"
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
BlEllipsoid::BlEllipsoid()
{
	SetThisPointers();
}

BlEllipsoid::~BlEllipsoid()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlEllipsoid/\r\n", "", -1, false, true);}
}

void BlEllipsoid::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateEllipsoidGeometry(m_iLatSegments, m_iLongSegments, m_fltMajorRadius, m_fltMinorRadius);
}

void BlEllipsoid::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
		//For the moment build a test node to generate the mesh from.
		osg::ref_ptr<osg::Geometry> osgGeometry = CreateEllipsoidGeometry(m_iLatSegments, m_iLongSegments, m_fltMajorRadius, m_fltMinorRadius);
		osg::ref_ptr<osg::Geode> osgNode = new osg::Geode;
		osgNode->addDrawable(m_osgGeometry.get());

        //FIX PHYSICS
		//m_vxGeometry = GetBlSimulator()->CreateConvexMeshFromOsg(osgNode.get());
	}
}

void BlEllipsoid::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Ellipsoid::CreateParts();
	BlRigidBody::SetBody();
}

void BlEllipsoid::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Ellipsoid::CreateJoints();
	BlRigidBody::Initialize();
}

void BlEllipsoid::ResizePhysicsGeometry()
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
