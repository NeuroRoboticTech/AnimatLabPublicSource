/**
\file	VsTorus.cpp

\brief	Implements the vortex Torus class.
**/

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsTorus.h"
#include "VsSimulator.h"

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
VsTorus::VsTorus()
{
	SetThisPointers();
}

VsTorus::~VsTorus()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsTorus/\r\n", "", -1, FALSE, TRUE);}
}

void VsTorus::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateTorusGeometry(m_fltInsideRadius, m_fltOutsideRadius, m_iSides, m_iRings);
}

void VsTorus::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
		//For the moment build a test node to generate the mesh from.
		osg::ref_ptr<osg::Geometry> osgGeometry = CreateTorusGeometry(m_fltInsideRadius, m_fltOutsideRadius, m_iSides, m_iRings);
		osg::ref_ptr<osg::Geode> osgNode = new osg::Geode;
		osgNode->addDrawable(m_osgGeometry.get());

		m_vxGeometry = GetVsSimulator()->CreatTriangleMeshFromOsg(osgNode.get());
	}
}

void VsTorus::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
	Torus::CreateParts();
	VsRigidBody::SetBody();
}

void VsTorus::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Torus::CreateJoints();
	VsRigidBody::Initialize();
}

void VsTorus::ResizePhysicsGeometry()
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
