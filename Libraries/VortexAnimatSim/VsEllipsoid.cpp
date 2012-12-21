/**
\file	VsEllipsoid.cpp

\brief	Implements the vortex ellipsoid class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsEllipsoid.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
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
VsEllipsoid::VsEllipsoid()
{
	SetThisPointers();
}

VsEllipsoid::~VsEllipsoid()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsEllipsoid/\r\n", "", -1, FALSE, TRUE);}
}

void VsEllipsoid::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateEllipsoidGeometry(m_iLatSegments, m_iLongSegments, m_fltMajorRadius, m_fltMinorRadius);
}

void VsEllipsoid::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
		//For the moment build a test node to generate the mesh from.
		osg::ref_ptr<osg::Geometry> osgGeometry = CreateEllipsoidGeometry(m_iLatSegments, m_iLongSegments, m_fltMajorRadius, m_fltMinorRadius);
		osg::ref_ptr<osg::Geode> osgNode = new osg::Geode;
		osgNode->addDrawable(m_osgGeometry.get());

		m_vxGeometry = GetVsSimulator()->CreateConvexMeshFromOsg(osgNode.get());
	}
}

void VsEllipsoid::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
	Ellipsoid::CreateParts();
	VsRigidBody::SetBody();
}

void VsEllipsoid::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Ellipsoid::CreateJoints();
	VsRigidBody::Initialize();
}

void VsEllipsoid::ResizePhysicsGeometry()
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
