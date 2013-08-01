/**
\file	VsPlaneTest.cpp

\brief	Implements the vortex plane class.
**/

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPlaneTest.h"
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
\date	4/17/2011
**/
VsPlaneTest::VsPlaneTest()
{
	//SetThisPointers();
	//m_bCullBackfaces = true; //we want back face culling on by default for planes.
}

/**
\brief	Destructor.

\author	dcofer
\date	4/17/2011
**/
VsPlaneTest::~VsPlaneTest()
{
	//try
	//{
	//	DeleteGraphics();
	//	DeletePhysics();
	//}
	//catch(...)
	//{Std_TraceMsg(0, "Caught Error in desctructor of VsPlaneTest/\r\n", "", -1, false, true);}
}

void VsPlaneTest::CreateGraphicsGeometry()
{
	//m_osgGeometry = CreatePlaneGeometry(CornerX(), CornerY(), m_ptSize.x, m_ptSize.y, GridX(), GridY(), false);
}

void VsPlaneTest::CreatePhysicsGeometry()
{
	//if(IsCollisionObject())
	//	m_vxGeometry = new VxPlane();
}

void VsPlaneTest::CreateParts()
{ 
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	osg::Group *m_World = lpVsSim->OSGRoot();	
    VxMaterialTable *m_MT = lpVsSim->Frame()->getMaterialTable();

    //****** Terrain ******//
	osg::ref_ptr<osg::Node> terrain;
    VxGeometry* terrainGeom = 0;

    // Create a plane geometry using an OSG helper function. Note that 
    // graphically, a plane cannot be infinite, so we create it to be
    // 2x2, centered around the world origin.
    osg::ref_ptr<osg::Geometry> terrainDrawable = osg::createTexturedQuadGeometry(
        osg::Vec3(-50, 0, -50), osg::Vec3(100, 0, 0), osg::Vec3(0, 0, 100), 0, 0, 1, 1);

    // Create the node necessary to hold the geometry.
    osg::ref_ptr<osg::Geode> terrainGeode = new osg::Geode;
    terrainGeode->addDrawable(terrainDrawable.get());
    terrain = terrainGeode.release();

    // Load the geometry from the same node as the graphical object.
    terrainGeom = new VxPlane;

    // Create the graphical object.
    // Its root is a transform node (so we could move it around).
    this->node = new osg::MatrixTransform;
    this->node->setName("TerrainNode");

    // Read the file and add it as a child of the transform.
    this->node->addChild(terrain.get());

    // Add it to the scene graph.
    m_World->addChild(this->node.get());

    // Create the physics object.
    this->part = new VxPart;                         // Create the part.
    this->part->setName("TerrainPart");              // Give it a name.
    this->part->setControl(VxEntity::kControlNode);  // Set it to kinetic.
    this->part->freeze(true);                            // Freeze it.

    // Add the geometry to the part.
    this->part->addGeometry(terrainGeom, m_MT->getMaterial("DEFAULTMATERIAL"));

    // Add it to the universe.
    lpVsSim->Universe()->addEntity(this->part);
}

void VsPlaneTest::ResizePhysicsGeometry()
{
	//if(m_vxGeometry)
	//	m_vxGeometry = new VxPlane();
}
//Planes can never have fluid interactions/dynamics.
void VsPlaneTest::Physics_FluidDataChanged()
{}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
