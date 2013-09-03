// VsMeshBase.cpp: implementation of the VsMeshBase class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsMeshBase.h"
#include "VsSimulator.h"
#include "VsDragger.h"
#include "VsOsgUserDataVisitor.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsMeshBase::VsMeshBase()
{
}

VsMeshBase::~VsMeshBase()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsMeshBase\r\n", "", -1, false, true);}
}

void VsMeshBase::SetThisPointers()
{
	VsRigidBody::SetThisPointers();
	m_lpThisMesh = dynamic_cast<Mesh *>(this);
	if(!m_lpThisMesh)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisMesh, " + m_lpThisAB->Name());
}

void VsMeshBase::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateBoxGeometry(1, 1, 1, 1, 1, 1);
}

void VsMeshBase::LoadMeshNode()
{
	string strPath = m_lpThisAB->GetSimulator()->ProjectPath();
	string strMeshFile;
	
	if(m_lpThisRB->IsCollisionObject() && m_lpThisMesh->CollisionMeshType() == "CONVEX")
		strMeshFile	= m_lpThisMesh->ConvexMeshFile();
	else
		strMeshFile	= m_lpThisMesh->MeshFile();

	string strFile = AnimatSim::GetFilePath(strPath, strMeshFile);
	m_osgBaseMeshNode = GetVsSimulator()->MeshMgr()->LoadMesh(strFile);

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

void VsMeshBase::CreateDefaultMesh()
{
	CreateGraphicsGeometry();
	m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());

	m_osgBaseMeshNode = osgGroup;
}

void VsMeshBase::CreateGeometry()
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
		THROW_TEXT_ERROR(Vs_Err_lMeshOsgNodeGroupNotDefined, Vs_Err_strMeshOsgNodeGroupNotDefined, "Body: " + m_lpThisAB->Name() + " Mesh: " + AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), m_lpThisMesh->MeshFile()));

	osgNodeGroup->addChild(m_osgMeshNode.get());
}

void VsMeshBase::CreatePhysicsGeometry()
{
	if(m_lpThisRB->IsCollisionObject())
	{
		if(m_lpThisMesh->CollisionMeshType() == "CONVEX")
			m_vxGeometry = GetVsSimulator()->CreateConvexMeshFromOsg(m_osgMeshNode.get()); 
		else
			m_vxGeometry = GetVsSimulator()->CreatTriangleMeshFromOsg(m_osgMeshNode.get()); 

		if(!m_vxGeometry)
			THROW_TEXT_ERROR(Vs_Err_lCreatingGeometry, Vs_Err_strCreatingGeometry, "Body: " + m_lpThisAB->Name() + " Mesh: " + AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), m_lpThisMesh->MeshFile()));
	}
}


void VsMeshBase::ResizePhysicsGeometry()
{
	if(m_vxGeometry && m_vxCollisionGeometry && m_vxSensor)
	{
		if(!m_vxSensor->removeCollisionGeometry(m_vxCollisionGeometry))
			THROW_PARAM_ERROR(Vs_Err_lRemovingCollisionGeometry, Vs_Err_strRemovingCollisionGeometry, "ID: ", m_lpThisAB->ID());

		delete m_vxCollisionGeometry;
		m_vxCollisionGeometry = NULL;

		int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(m_lpThisRB->MaterialID());
		CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisRB->Density()));
	}
}

void VsMeshBase::Physics_Resize()
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

		//Create a new box geometry with the new sizes.
		CreateGeometry();

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		if(m_osgNodeGroup.valid())
		{
			osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
			osgVisitor->traverse(*m_osgNodeGroup);
		}
	}

	if(m_vxGeometry)
	{
		ResizePhysicsGeometry();

		//We need to reset the density in order for it to recompute the mass and volume.
		Physics_SetDensity(m_lpThisRB->Density());

		//Now get base values, including mass and volume
		GetBaseValues();
	}
}


		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
