// BlCone.cpp: implementation of the BlCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlOsgGeometry.h"
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
		DeletePhysics(false);
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
        DeleteCollisionGeometry();

        CalculateVolumeAndAreas();

        btConvexHullShape *btHull = OsgMeshToConvexHull(m_osgNode.get(), true, -1);
        m_btCollisionShape = btHull;
        //m_bDisplayDebugCollisionGraphic = false;
	}
}

void BlCone::CalculateVolumeAndAreas()
{
        m_fltVolume = (1/3.0)*osg::PI*((m_fltLowerRadius*m_fltLowerRadius + m_fltLowerRadius*m_fltUpperRadius + m_fltUpperRadius*m_fltUpperRadius)/m_fltHeight);

        //First get the large and small radius
        float fltLargeDiam = 2*STD_MAX(m_fltLowerRadius, m_fltUpperRadius);
        float fltSmallDiam = 2*STD_MIN(m_fltLowerRadius, m_fltUpperRadius);

        //Now get the side profile area by geting the area of the rectangle of the two radii, and then subtracting 2 traingles from the large radius to small radius.
        float fltProfileRect = fltLargeDiam * m_fltHeight;
        float fltTriHeight = (fltLargeDiam - fltSmallDiam) / 2.0;
        float fltTriBase = m_fltHeight;
        float fltProfileTriangle = 0.5f * fltTriBase * fltTriHeight;

        float fltProfileArea = fltProfileRect - (2*fltProfileRect);

        m_vArea.x = fltProfileArea;
        m_vArea.y = fltProfileArea;
        m_vArea.z = 2*osg::PI*fltLargeDiam*fltSmallDiam;
}

void BlCone::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Cone::CreateParts();
}

void BlCone::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cone::CreateJoints();
	BlRigidBody::Initialize();
}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
