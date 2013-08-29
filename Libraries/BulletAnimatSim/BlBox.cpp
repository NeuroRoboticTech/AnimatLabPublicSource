// BlBox.cpp: implementation of the BlBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlBox.h"
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

BlBox::BlBox()
{
	SetThisPointers();
}

BlBox::~BlBox()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlBox\r\n", "", -1, false, true);}
}

void BlBox::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
}

void BlBox::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
    {
        m_fltMass = m_fltDensity * m_fltLength * m_fltWidth * m_fltHeight;
        m_btCollisionShape= osgbCollision::btBoxCollisionShapeFromOSG( m_osgNode.get() );
    }
}

void BlBox::CreateDynamicPart()
{
    BlSimulator *lpSim = GetBlSimulator();

	if(lpSim && m_lpThisRB && m_lpThisAB)
	{
        osg::ref_ptr< osgbDynamics::CreationRecord > cr = new osgbDynamics::CreationRecord;
        cr->_sceneGraph = m_osgMT.get();
        cr->_shapeType = BOX_SHAPE_PROXYTYPE;
        cr->_mass = 1.f;
        cr->_restitution = 1.f;
        //cr->_parentTransform = m_osgMT->getMatrix();
        m_btPart = osgbDynamics::createRigidBody( cr.get(), m_btCollisionShape );
        lpSim->DynamicsWorld()->addRigidBody( m_btPart );

        m_osgbMotion = dynamic_cast<osgbDynamics::MotionState *>(m_btPart->getMotionState());
        m_osgbMotion->setCenterOfMass(osg::Vec3(0, 0, 0));
        m_btCollisionShape = m_btPart->getCollisionShape();

  //      m_osgDebugNode = osgbCollision::osgNodeFromBtCollisionShape( m_btCollisionShape );
  //      m_osgDebugNode->setName(m_lpThisRB->Name() + "_Debug");
		//m_osgNodeGroup->addChild(m_osgDebugNode.get());	
	}
}

void BlBox::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Box::CreateParts();
	BlRigidBody::SetBody();
}

void BlBox::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Box::CreateJoints();
	BlRigidBody::Initialize();
}

void BlBox::ResizePhysicsGeometry()
{
 //FIX PHYSICS
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
