// BlMeshBase.cpp: implementation of the BlMeshBase class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlOsgGeometry.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlMeshBase.h"
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

BlMeshBase::BlMeshBase()
{
}

BlMeshBase::~BlMeshBase()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BlMeshBase\r\n", "", -1, false, true);}
}

void BlMeshBase::SetThisPointers()
{
	BlRigidBody::SetThisPointers();
	m_lpThisMesh = dynamic_cast<Mesh *>(this);
	if(!m_lpThisMesh)
		THROW_TEXT_ERROR(Bl_Err_lThisPointerNotDefined, Bl_Err_strThisPointerNotDefined, "m_lpThisMesh, " + m_lpThisAB->Name());
}

void BlMeshBase::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateBoxGeometry(1, 1, 1, 1, 1, 1);
}

void BlMeshBase::LoadMeshNode()
{
	std::string strPath = m_lpThisAB->GetSimulator()->ProjectPath();
	std::string strMeshFile;
	
	if(m_lpThisRB->IsCollisionObject() && m_lpThisMesh->CollisionMeshType() == "CONVEX")
		strMeshFile	= m_lpThisMesh->ConvexMeshFile();
	else
		strMeshFile	= m_lpThisMesh->MeshFile();

	std::string strFile = AnimatSim::GetFilePath(strPath, strMeshFile);
	m_osgBaseMeshNode = GetBlSimulator()->MeshMgr()->LoadMesh(strFile);

	if(!m_osgBaseMeshNode.valid())
		CreateDefaultMesh();

	m_osgBaseMeshNode->setName(m_lpThisAB->Name() + "_MeshNodeBase");

	//Enforce turning on of lighting for this object. Some meshes have this turned off by default (3DS in particular)
	//This did not fix the problem.
	//m_osgBaseMeshNode->getOrCreateStateSet()->setMode( GL_LIGHTING, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON );

	CStdFPoint vScale(1, 1, 1);

	//If this is a convex collision object then it does not get scaled here. That is done
	//when it is converted into a collision mesh by the editor. Otherwise use the scaling.
	if( !(m_lpThisMesh->IsCollisionObject() && m_lpThisMesh->CollisionMeshType() == "CONVEX") )
		vScale= m_lpThisMesh->Scale();

	osg::Matrix osgScaleMatrix = osg::Matrix::identity();
	osgScaleMatrix.makeScale(vScale.x, vScale.y, vScale.z);

	m_osgMeshNode = new osg::MatrixTransform(osgScaleMatrix);

	m_osgMeshNode->addChild(m_osgBaseMeshNode.get());
	m_osgMeshNode->setName(m_lpThisAB->Name() + "_MeshNode");
}

void BlMeshBase::CreateDefaultMesh()
{
	CreateGraphicsGeometry();
	m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());

	m_osgBaseMeshNode = osgGroup;
}

void BlMeshBase::CreateGeometry()
{
	LoadMeshNode();

	//For the mesh stuff we need to create it immediately after the mesh is loaded, and before we add it to
	//the matrix transforms. This is because when vortex creates the collision geometry it was using the attached
	//matrix transform when setting up the vertex positions and this was causing the mesh to appear in the wrong 
	//position relative to the collision mesh
	CreatePhysicsGeometry();

	//Now add it to the nodes.
	osg::Group *osgNodeGroup = NULL;
	if(m_osgNode.valid())
		osgNodeGroup = dynamic_cast<osg::Group *>(m_osgNode.get());		
	else
	{
		osgNodeGroup = new osg::Group;
		m_osgNode = osgNodeGroup;
	}

	if(!osgNodeGroup)
		THROW_TEXT_ERROR(Bl_Err_lMeshOsgNodeGroupNotDefined, Bl_Err_strMeshOsgNodeGroupNotDefined, "Body: " + m_lpThisAB->Name() + " Mesh: " + AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), m_lpThisMesh->MeshFile()));

	osgNodeGroup->addChild(m_osgMeshNode.get());
}

void BlMeshBase::CreatePhysicsGeometry()
{
	if(m_lpThisRB->IsCollisionObject())
	{
        DeleteCollisionGeometry();

		if(m_lpThisMesh->CollisionMeshType() == "CONVEX")
        {
            m_eBodyType = CONVEX_HULL_SHAPE_PROXYTYPE;
            m_btCollisionShape = OsgMeshToConvexHull(m_osgMeshNode.get(), true, 0);
        }
		else
        {
            m_eBodyType = TRIANGLE_MESH_SHAPE_PROXYTYPE;
            m_btCollisionShape = osgbCollision::btTriMeshCollisionShapeFromOSG(m_osgMeshNode.get());
        }

		if(!m_btCollisionShape)
			THROW_TEXT_ERROR(Bl_Err_lCreatingGeometry, Bl_Err_strCreatingGeometry, "Body: " + m_lpThisAB->Name() + " Mesh: " + AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), m_lpThisMesh->MeshFile()));
	}
}

void BlMeshBase::Physics_Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid() && m_osgMeshNode.valid())
	{
		osg::Group *osgGroup = dynamic_cast<osg::Group *>(m_osgNode.get());

		if(osgGroup && osgGroup->containsNode(m_osgMeshNode.get()))
			osgGroup->removeChild(m_osgMeshNode.get());

		m_osgGeometry.release();
		m_osgMeshNode.release();
		m_osgBaseMeshNode.release();

		CreateGeometry();

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		if(m_osgNodeGroup.valid())
		{
			osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
			osgVisitor->traverse(*m_osgNodeGroup);
		}

		if(Physics_IsDefined())
		{
			ResizePhysicsGeometry();
			//Now get base values, including mass and volume
			GetBaseValues();
		}
	}
}

//void OsgRigidBody::Physics_Resize()
//{
//	//First lets get rid of the current current geometry and then put new geometry in place.
//	if(m_osgNode.valid())
//	{
//		osg::Geode *osgGroup = NULL;
//		if(m_osgGeometryRotationMT.valid())
//			osgGroup = dynamic_cast<osg::Geode *>(m_osgGeometryRotationMT->getChild(0));
//		else
//			osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
//
//		if(!osgGroup)
//			THROW_TEXT_ERROR(Osg_Err_lNodeNotGeode, Osg_Err_strNodeNotGeode, m_lpThisAB->Name());
//
//		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
//			osgGroup->removeDrawable(m_osgGeometry.get());
//
//		m_osgGeometry.release();
//
//		//Create a new box geometry with the new sizes.
//		CreateGraphicsGeometry();
//		if(m_lpThisAB)
//			m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");
//
//		//Add it to the geode.
//		osgGroup->addDrawable(m_osgGeometry.get());
//
//		//Now lets re-adjust the gripper size.
//		if(m_osgDragger.valid())
//			m_osgDragger->SetupMatrix();
//
//		//Reset the user data for the new parts.
//		if(m_osgNodeGroup.valid())
//		{
//			osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
//			osgVisitor->traverse(*m_osgNodeGroup);
//		}
//	}
//
//	if(Physics_IsDefined())
//	{
//		ResizePhysicsGeometry();
//
//		//We need to reset the density in order for it to recompute the mass and volume.
//		if(m_lpThisRB)
//			Physics_SetDensity(m_lpThisRB->Density());
//
//		//Now get base values, including mass and volume
//		GetBaseValues();
//	}
//}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
