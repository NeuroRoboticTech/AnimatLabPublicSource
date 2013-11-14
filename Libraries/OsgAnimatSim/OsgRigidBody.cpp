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
    OsgMovableItem::Physics_UpdateMatrix();
    StartGripDrag();
    EndGripDrag();
}
 
void OsgRigidBody::UpdatePositionAndRotationFromMatrix()
{
	if(m_lpThisRB)
		m_lpThisRB->UpdatePhysicsPosFromGraphics();
    else
    	OsgBody::UpdatePositionAndRotationFromMatrix();
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



//The way bullet is operating is making do things a bit wonky for moving them in real time in the editor.
//Bullet insists that all OSG objects be relative to the world and attached to the root. If I try and use
//the dragger to move of those then it will only move that part, and not all the child parts, and this looks
//bad and is definetly different than how it previously worked. To get around this what I am doing is first 
//deleting the physics fo this part and all child joints. I am deleting all child graphics (but not this ones 
//graphics or the dragger would not work anymore). I am then recreating the graphics. However, when StartGripDrag
//is called in the drag handler it sets a sim property InDrag to true. If InDrag is true then the method AddOsgNodeToParent
//returns true. If this method is false then parts are added directly to root, if not then they are added to the 
//parent part. So essentially, I am deleting the osg tree node that has everything connected to root, and then recreating
//it with everything connected to the parent object being dragged. When the grip drag is over I have to reset things
//back going against root. Annoying.
void OsgRigidBody::StartGripDrag()
{
    //Delete the physics and graphics
    DeletePhysics(true);
    DeleteChildGraphics(true);

    //The recreate the graphics with AddOsgNodeToParent returning true for all parts.
    //This will recreate the graphics with each node attached to its parent instead of
    //everything connected to the root node like what is required for bullet to work.
    SetupChildGraphics(true);
}


void OsgRigidBody::EndGripDrag()
{
    //If this is a static part then we need to call EndGripDrag on its parent part so it can recreate
    //the entire static part instead of calling it on this one.
    if(m_lpThisRB && m_lpThisRB->HasStaticJoint())
    {
        OsgBody::UpdatePositionAndRotationFromMatrix();

        OsgRigidBody *lpOsgParent = dynamic_cast<OsgRigidBody *>(m_lpThisRB->Parent());
        if(lpOsgParent)
        {
            //If we called StartGripDrag on the parent when the child was started dragging then we would
            //be deleting the child graphics object during the drag, and that would be bad. So lets call
            //start on the parent here to delete all the required things, and then end to recreate them.
            lpOsgParent->DeletePhysics(true);
            lpOsgParent->DeleteChildGraphics(true);

            lpOsgParent->EndGripDrag();
        }
    }
    else
    {
        //We must recreate the physics geometry here in order for the code that calculates positions to work correctly
        //because it performs check to make sure it has valid physics geometry.
        CreatePhysicsGeometry();

        OsgBody::UpdatePositionAndRotationFromMatrix();

        DeleteChildGraphics(true);

        SetupPhysics();

        //Completely recreate the child parts and joints
        if(m_lpThisRB)
        {
            //Recreate the child parts.
            m_lpThisRB->CreateChildParts();

            //Recreate the joint from this part to its parent if one exists
	        if(m_lpThisRB->JointToParent())
		        m_lpThisRB->JointToParent()->CreateJoint();

            //Recreate the joint between this part and its parent
            m_lpThisRB->CreateChildJoints();
        }
    }

    if(m_lpThisRB && m_lpThisRB->IsRoot())
    {
        OsgMovableItem *lpOsgStruct = dynamic_cast<OsgMovableItem *>(m_lpThisRB->GetStructure());
        if(lpOsgStruct)
            lpOsgStruct->FinalMatrix(m_osgFinalMatrix);
        Physics_UpdateAbsolutePosition();
    }
}


void OsgRigidBody::SetupChildGraphics(bool bRoot)
{
    if(!bRoot)
    {
        InitializeGraphicsGeometry();
        SetupGraphics();
    }

	OsgJoint *lpVsJoint = dynamic_cast<OsgJoint *>(m_lpThisRB->JointToParent());

    if(lpVsJoint)
        lpVsJoint->SetupGraphics();

    int iCount = m_lpThisRB->ChildParts()->GetSize();
    for(int iIdx=0; iIdx<iCount; iIdx++)
    {
        RigidBody *lpChild = m_lpThisRB->ChildParts()->at(iIdx);
        if(lpChild)
        {
        	OsgRigidBody *lpVsChild = dynamic_cast<OsgRigidBody *>(lpChild);
            if(lpVsChild)
                lpVsChild->SetupChildGraphics(false);
        }
    }
}

void OsgRigidBody::DeleteChildGraphics(bool bRoot)
{
    if(!bRoot)
        DeleteGraphics();

    OsgJoint *lpVsJoint = dynamic_cast<OsgJoint *>(m_lpThisRB->JointToParent());

    if(lpVsJoint)
        lpVsJoint->DeleteGraphics();

    int iCount = m_lpThisRB->ChildParts()->GetSize();
    for(int iIdx=0; iIdx<iCount; iIdx++)
    {
        RigidBody *lpChild = m_lpThisRB->ChildParts()->at(iIdx);
        if(lpChild)
        {
        	OsgRigidBody *lpVsChild = dynamic_cast<OsgRigidBody *>(lpChild);
            if(lpVsChild)
                lpVsChild->DeleteChildGraphics(false);
        }
    }
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
			OsgBody::BuildLocalMatrix(m_lpThisAB->GetStructure()->AbsolutePosition(), m_lpThisRB->CenterOfMassWithStaticChildren(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
		else
			OsgBody::BuildLocalMatrix(m_lpThisMI->Position(), m_lpThisRB->CenterOfMassWithStaticChildren(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
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

void OsgRigidBody::CreateGeometry() 
{
    InitializeGraphicsGeometry();

    //Do not try to create the physics geometry here if we have a static joint because it will have already been 
    //created in the parent object.
    if(m_lpThisRB && !m_lpThisRB->HasStaticJoint())
    	CreatePhysicsGeometry();
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
        {
            //Do nothing here because static parts are created in the parent parts CreateDynamicPart method.
            //The reason is that all the static parts and geometries for a compound shape must already exist
            // in order to create them in bullet.
        }
		else
			CreateDynamicPart();
	}
}

bool OsgRigidBody::AddOsgNodeToParent()
{
	if(!Physics_IsGeometryDefined() || !m_lpThisRB || m_lpThisRB->IsContactSensor() || GetOsgSimulator()->InDrag() || m_lpThisRB->HasStaticJoint())
        return true;
    else
        return false;
}

float *OsgRigidBody::Physics_GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);
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
