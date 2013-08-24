// BlBoxTest.cpp: implementation of the BlBoxTest class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlBoxTest.h"
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

BlBoxTest::BlBoxTest()
{
	//SetThisPointers();
}

BlBoxTest::~BlBoxTest()
{
	//try
	//{
	//	DeleteGraphics();
	//	DeletePhysics();
	//}
	//catch(...)
	//{Std_TraceMsg(0, "Caught Error in desctructor of BlBoxTest\r\n", "", -1, false, true);}
}

void BlBoxTest::CreateGraphicsGeometry()
{
	//m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
}

void BlBoxTest::CreatePhysicsGeometry()
{
	//if(IsCollisionObject())
	//	m_vxGeometry = new VxBox(m_fltLength, m_fltHeight, m_fltWidth);
}

void BlBoxTest::CreateParts()
{
	BlSimulator *lpVsSim = dynamic_cast<BlSimulator *>(m_lpSim);
	osg::Group *m_World = lpVsSim->OSGRoot();	
    VxMaterialTable *m_MT = lpVsSim->Frame()->getMaterialTable();

	osg::ref_ptr<osg::Node> object;
    VxGeometry* objectGeom = 0;

	CStdFPoint vPos = this->Position();

    osg::ref_ptr<osg::ShapeDrawable> boxDrawable = new osg::ShapeDrawable(new osg::Box(osg::Vec3(0, 0, 0), m_fltLength, m_fltHeight, m_fltWidth));
	boxDrawable->setColor(osg::Vec4(1, 0, 0, 0)); 
    osg::ref_ptr<osg::Geode> box = new osg::Geode;
    box->addDrawable(boxDrawable.get());
    object = box.release();
    objectGeom = new VxBox(m_fltLength, m_fltHeight, m_fltWidth);

    // Create the graphical object.
    // Its root is a transform node (so we could move it around).
    this->node = new osg::MatrixTransform(osg::Matrix::translate(vPos.x, vPos.y, vPos.z));
    this->node->setName("Object");
    this->node->addChild(object.get());

    // Add it to the scene graph.
    m_World->addChild(this->node.get());

    // Create the physics object.
    // Note that we use a sphere geom to simplify things, but we could have
    // created a triangle mesh from the same file as the graphical object.
    this->part = new VxPart;                            // Create the part.
    this->part->setName("object");                      // Give it a name.
    this->part->setControl(VxEntity::kControlDynamic);  // Set it to dynamic.
    this->part->setPosition(vPos.x, vPos.y, vPos.z);         // Set its initial position.

    // Add the sphere geometry to the part.
    this->part->addGeometry(objectGeom, m_MT->getMaterial("DEFAULTMATERIAL"));
    this->part->setFastMoving(true);
    this->part->freeze(false);

    // Add the part to the universe.
    lpVsSim->Universe()->addEntity(this->part);

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->CreateParts();

}

void BlBoxTest::CreateJoints()
{
	//if(m_lpJointToParent)
	//	m_lpJointToParent->CreateJoint();

	//Box::CreateJoints();
	//BlRigidBody::Initialize();
}

void BlBoxTest::ResizePhysicsGeometry()
{
	//if(m_vxGeometry)
	//{
	//	VxBox *vxBox = dynamic_cast<VxBox *>(m_vxGeometry);

	//	if(!vxBox)
	//		THROW_TEXT_ERROR(Bl_Err_lGeometryMismatch, Bl_Err_strGeometryMismatch, m_lpThisAB->Name());
	//	
	//	vxBox->setDimensions(m_fltLength, m_fltHeight, m_fltWidth);
	//}
}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
