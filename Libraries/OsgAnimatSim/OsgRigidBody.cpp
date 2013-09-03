// OsgRigidBody.cpp: implementation of the OsgRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgSimulator.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

OsgRigidBody::OsgRigidBody()
{
	m_bCollectExtraData = false;

	for(int i=0; i<3; i++)
	{
		m_vTorque[i] = 0;
		m_vForce[i] = 0;
		m_vLinearVelocity[i] = 0;
		m_vAngularVelocity[i] = 0;
		m_vLinearAcceleration[i] = 0;
		m_vAngularAcceleration[i] = 0;
	}
}

OsgRigidBody::~OsgRigidBody()
{

try
{
	//int i= 5;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of OsgRigidBody\r\n", "", -1, false, true);}
}

void OsgRigidBody::SetThisPointers()
{
	OsgBody::SetThisPointers();
	m_lpThisRB = dynamic_cast<RigidBody *>(this);
	if(!m_lpThisRB)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisRB, " + m_lpThisAB->Name());

	m_lpThisRB->PhysicsBody(this);
}

void OsgRigidBody::Physics_UpdateMatrix()
{
	OsgBody::Physics_UpdateMatrix();

	if(m_lpThisRB)
		m_lpThisRB->UpdatePhysicsPosFromGraphics();
}
 
void OsgRigidBody::UpdatePositionAndRotationFromMatrix()
{
	OsgBody::UpdatePositionAndRotationFromMatrix();

	if(m_lpThisRB)
		m_lpThisRB->UpdatePhysicsPosFromGraphics();
}

void OsgRigidBody::Physics_SetColor()
{
	if(m_lpThisRB)
		SetColor(*m_lpThisRB->Ambient(), *m_lpThisRB->Diffuse(), *m_lpThisRB->Specular(), m_lpThisRB->Shininess()); 
}

void OsgRigidBody::Physics_TextureChanged()
{
	if(m_lpThisRB)
		SetTexture(m_lpThisRB->Texture());
}

void OsgRigidBody::Physics_Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = NULL;
		if(m_osgGeometryRotationMT.valid())
			osgGroup = dynamic_cast<osg::Geode *>(m_osgGeometryRotationMT->getChild(0));
		else
			osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());

		if(!osgGroup)
			THROW_TEXT_ERROR(Osg_Err_lNodeNotGeode, Osg_Err_strNodeNotGeode, m_lpThisAB->Name());

		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
			osgGroup->removeDrawable(m_osgGeometry.get());

		m_osgGeometry.release();

		//Create a new box geometry with the new sizes.
		CreateGraphicsGeometry();
		if(m_lpThisAB)
			m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

		//Add it to the geode.
		osgGroup->addDrawable(m_osgGeometry.get());

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		if(m_osgNodeGroup.valid())
		{
			osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
			osgVisitor->traverse(*m_osgNodeGroup);
		}
	}

	if(Physics_IsDefined())
	{
		ResizePhysicsGeometry();

		//We need to reset the density in order for it to recompute the mass and volume.
		if(m_lpThisRB)
			Physics_SetDensity(m_lpThisRB->Density());

		//Now get base values, including mass and volume
		GetBaseValues();
	}
}

void OsgRigidBody::Physics_SelectedVertex(float fltXPos, float fltYPos, float fltZPos)
{
	if(m_lpThisRB->IsCollisionObject() && m_osgSelVertexMT.valid())
	{
		osg::Matrix osgMT;
		osgMT.makeIdentity();
		osgMT = osgMT.translate(fltXPos, fltYPos, fltZPos);

		m_osgSelVertexMT->setMatrix(osgMT);
	}
}


void OsgRigidBody::ShowSelectedVertex()
{
	if(m_lpThisRB && m_lpThisAB && m_lpThisRB->IsCollisionObject() && m_lpThisAB->Selected() && m_osgMT.valid() && m_osgSelVertexMT.valid())
	{
		if(!m_osgMT->containsNode(m_osgSelVertexMT.get()))
			m_osgMT->addChild(m_osgSelVertexMT.get());
	}
}

void OsgRigidBody::HideSelectedVertex()
{
	if(m_lpThisRB->IsCollisionObject() && m_osgMT.valid() && m_osgSelVertexMT.valid())
	{
		if(m_osgMT->containsNode(m_osgSelVertexMT.get()))
			m_osgMT->removeChild(m_osgSelVertexMT.get());
	}
}

void OsgRigidBody::Physics_ResizeSelectedReceptiveFieldVertex()
{
	DeleteSelectedVertex();
	CreateSelectedVertex(m_lpThisAB->Name());
}

void OsgRigidBody::Initialize()
{
	//GetBaseValues();
}

/**
\brief	Builds the local matrix.

\details If this is the root object then use the world coordinates of the structure instead of the
local coordinates of the rigid body.

\author	dcofer
\date	5/11/2011
**/
void  OsgRigidBody::BuildLocalMatrix()
{
	//build the local matrix
	if(m_lpThisRB && m_lpThisMI && m_lpThisAB)
	{
		if(m_lpThisRB->IsRoot())
			OsgBody::BuildLocalMatrix(m_lpThisAB->GetStructure()->AbsolutePosition(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
		else
			OsgBody::BuildLocalMatrix(m_lpThisMI->Position(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
	}
}

/**
\brief	Gets the parent osg node.

\details If this is the root object then attach it directly to the OSG root node because we are building its local matrix
using the structures absolute position. This is so the vortex stuff works correctly, and so that you can move the structure
by moving the root body. 

\author	dcofer
\date	5/11/2011

\return	Pointer to the OSG group node of the parent.
**/
osg::MatrixTransform *OsgRigidBody::ParentOSG()
{
	OsgMovableItem *lpItem = NULL;

	if(!m_lpThisRB->IsRoot() && m_lpParentVsMI)
		return m_lpParentVsMI->GetMatrixTransform();
	else
		return GetOsgSimulator()->OSGRoot();
}

void OsgRigidBody::ResetSensorCollisionGeom()
{
	OsgRigidBody *lpVsParent = dynamic_cast<OsgRigidBody *>(m_lpThisRB->Parent());

	if(lpVsParent)
		SetFollowEntity(lpVsParent);
}

void OsgRigidBody::SetupPhysics()
{
	//If no geometry is defined then this part does not have a physics representation.
	//it is purely an osg node attached to other parts. An example of this is an attachment point or a sensor.
	if(Physics_IsGeometryDefined() && m_lpThisRB)
	{
		//If the parent is not null and the joint is null then that means we need to statically link this part to 
		//its parent. So we do not create a physics part, we just get a link to its parents part.
		if(m_lpThisRB->IsContactSensor())
			CreateSensorPart();
		else if(m_lpThisRB->HasStaticJoint())
			CreateStaticPart();
		else
			CreateDynamicPart();
	}
}

bool OsgRigidBody::AddOsgNodeToParent()
{
	if(!Physics_IsGeometryDefined() || !m_lpThisRB || m_lpThisRB->IsContactSensor())
        return true;
    else
        return false;
}

float *OsgRigidBody::Physics_GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);
	RigidBody *lpBody = dynamic_cast<RigidBody *>(this);

	if(strType == "BODYTORQUEX")
		{m_bCollectExtraData = true; return (&m_vTorque[0]);}

	if(strType == "BODYTORQUEY")
		{m_bCollectExtraData = true; return (&m_vTorque[1]);}

	if(strType == "BODYTORQUEZ")
		{m_bCollectExtraData = true; return (&m_vTorque[2]);}

	if(strType == "BODYFORCEX")
		{m_bCollectExtraData = true; return (&m_vForce[0]);}

	if(strType == "BODYFORCEY")
		{m_bCollectExtraData = true; return (&m_vForce[1]);}

	if(strType == "BODYFORCEZ")
		{m_bCollectExtraData = true; return (&m_vForce[2]);}

	if(strType == "BODYLINEARVELOCITYX")
		{m_bCollectExtraData = true; return (&m_vLinearVelocity[0]);}

	if(strType == "BODYLINEARVELOCITYY")
		{m_bCollectExtraData = true; return (&m_vLinearVelocity[1]);}

	if(strType == "BODYLINEARVELOCITYZ")
		{m_bCollectExtraData = true; return (&m_vLinearVelocity[2]);}

	if(strType == "BODYANGULARVELOCITYX")
		{m_bCollectExtraData = true; return (&m_vAngularVelocity[0]);}

	if(strType == "BODYANGULARVELOCITYY")
		{m_bCollectExtraData = true; return (&m_vAngularVelocity[1]);}

	if(strType == "BODYANGULARVELOCITYZ")
		{m_bCollectExtraData = true; return (&m_vAngularVelocity[2]);}

	//if(strType == "BODYBUOYANCY")
	//	{m_bCollectExtraData = true; return (&m_fltReportBuoyancy);}

	if(strType == "BODYLINEARACCELERATIONX")
		{m_bCollectExtraData = true; return (&m_vLinearAcceleration[0]);}

	if(strType == "BODYLINEARACCELERATIONY")
		{m_bCollectExtraData = true; return (&m_vLinearAcceleration[1]);}

	if(strType == "BODYLINEARACCELERATIONZ")
		return (&m_vLinearAcceleration[2]);

	if(strType == "BODYANGULARACCELERATIONX")
		{m_bCollectExtraData = true; return (&m_vAngularAcceleration[0]);}

	if(strType == "BODYANGULARACCELERATIONY")
		{m_bCollectExtraData = true; return (&m_vAngularAcceleration[1]);}

	if(strType == "BODYANGULARACCELERATIONZ")
		{m_bCollectExtraData = true; return (&m_vAngularAcceleration[2]);}

	return NULL;
}


	}			// Environment
}				//OsgAnimatSim
