/**
\file	BlEllipsoid.cpp

\brief	Implements the vortex ellipsoid class.
**/

#include "StdAfx.h"
#include "BlOsgGeometry.h"
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
		DeletePhysics(false);
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
        DeleteCollisionGeometry();
        
        m_fltVolume = (4/3.0)*osg::PI*m_fltMajorRadius*m_fltMajorRadius*m_fltMinorRadius;
        m_btCollisionShape = OsgMeshToConvexHull(m_osgNode.get(), true, -1);
        //m_bDisplayDebugCollisionGraphic = true;
	}
}

void BlEllipsoid::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Ellipsoid::CreateParts();
}

void BlEllipsoid::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Ellipsoid::CreateJoints();
	BlRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
