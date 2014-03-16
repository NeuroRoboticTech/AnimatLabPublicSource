// OsgJoint.cpp: implementation of the OsgJoint class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgRigidBody.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

OsgJoint::OsgJoint()
{
	m_lpVsParent = NULL;
	m_lpVsChild = NULL;
    m_vJointGraphicsRotOffset.Set(0, osg::PI/2, 0) ;
}

OsgJoint::~OsgJoint()
{
}

void OsgJoint::Physics_SetParent(MovableItem *lpParent)
{
	m_lpParentVsMI = dynamic_cast<OsgMovableItem *>(lpParent);
	m_lpVsParent = dynamic_cast<OsgBody *>(lpParent);
}

void OsgJoint::Physics_SetChild(MovableItem *lpChild) 
{
	m_lpVsChild = dynamic_cast<OsgRigidBody *>(lpChild);
}

void OsgJoint::SetThisPointers()
{
	OsgBody::SetThisPointers();

	m_lpThisJoint = dynamic_cast<Joint *>(this);
	if(!m_lpThisJoint)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisJoint, " + m_lpThisAB->Name());

	m_lpThisJoint->PhysicsBody(this);
}

osg::Vec3d OsgJoint::NormalizeAxis(CStdFPoint vLocalRot)
{
	osg::Vec3 vPosN(1, 0, 0);
	CStdFPoint vMatrixPos(0, 0, 0);

	osg::Matrix osgMT = SetupMatrix(vMatrixPos, vLocalRot);

	osg::Vec3 vNorm = vPosN * osgMT;

	return vNorm;
}

osg::MatrixTransform *OsgJoint::ParentOSG()
{
	if(m_lpVsParent)
		return m_lpVsParent->GetMatrixTransform();

	return NULL;
}

osg::MatrixTransform *OsgJoint::ChildOSG()
{
	if(m_lpVsChild)
		return m_lpVsChild->GetMatrixTransform();

	return NULL;
}

void OsgJoint::SetAlpha()
{
	OsgBody::SetAlpha();

	if(m_osgDefaultBallMat.valid() && m_osgDefaultBallSS.valid())
		SetMaterialAlpha(m_osgDefaultBallMat.get(), m_osgDefaultBallSS.get(), m_lpThisMI->Alpha());
}

void OsgJoint::Physics_PositionChanged()
{
	OsgBody::Physics_PositionChanged();
	Physics_ResetGraphicsAndPhysics();
}

void OsgJoint::Physics_RotationChanged()
{
	Physics_ResetGraphicsAndPhysics();
}

void OsgJoint::Physics_ResetGraphicsAndPhysics()
{
    OsgMovableItem::Physics_ResetGraphicsAndPhysics();

    if(m_osgDragger.valid())
        m_osgDragger->SetupMatrix();
}

void OsgJoint::DeleteGraphics()
{
    DeleteJointGraphics();
    OsgBody::DeleteGraphics();
}

void OsgJoint::DeleteJointGraphics()
{
	if(m_osgJointMT.valid() && m_osgDefaultBallMT.valid()) m_osgJointMT->removeChild(m_osgDefaultBallMT.get());
    if(m_osgDefaultBall.valid()) m_osgDefaultBall.release();
    if(m_osgDefaultBallMT.valid()) m_osgDefaultBallMT.release();
    if(m_osgDefaultBallSS.valid()) m_osgDefaultBallSS.release();
}

void OsgJoint::ResetDraggerOnResize()
{
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

/**
\brief	Creates the default ball graphics.

\author	dcofer
\date	4/15/2011
**/
void OsgJoint::CreateJointGraphics()
{
	//Create the cylinder for the hinge
	m_osgDefaultBall = CreateSphereGeometry(15, 15, m_lpThisJoint->Size());
	osg::ref_ptr<osg::Geode> osgBall = new osg::Geode;
	osgBall->addDrawable(m_osgDefaultBall.get());

	CStdFPoint vPos(0, 0, 0), vRot(osg::PI/2, 0, 0); 
	m_osgDefaultBallMT = new osg::MatrixTransform();
	m_osgDefaultBallMT->setMatrix(SetupMatrix(vPos, vRot));
	m_osgDefaultBallMT->addChild(osgBall.get());

	//create a material to use with the pos flap
	if(!m_osgDefaultBallMat.valid())
		m_osgDefaultBallMat = new osg::Material();		

	//create a stateset for this node
	m_osgDefaultBallSS = m_osgDefaultBallMT->getOrCreateStateSet();

	//set the diffuse property of this node to the color of this body	
	m_osgDefaultBallMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
	m_osgDefaultBallMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(1, 0.25, 1, 1));
	m_osgDefaultBallMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
	m_osgDefaultBallMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
	m_osgDefaultBallSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

	//apply the material
	m_osgDefaultBallSS->setAttribute(m_osgDefaultBallMat.get(), osg::StateAttribute::ON);

	m_osgJointMT->addChild(m_osgDefaultBallMT.get());
}


/**
\brief	Sets up the graphics for the joint.

\detail This base joint class sets up a default ball graphics object. A few of the joint
classes do not have any special graphics associated with them, so simply knowing where the joint
is located is enough, and a ball to define that position is sufficient. This base code creates
that graphics so that each derived class does not have to. If a joint does need special 
graphics then just override this function and define it yourself, but do not call this base method.

\author	dcofer
\date	4/15/2011
**/
void OsgJoint::SetupGraphics()
{
	//Setup Vs pointers to child and parent.
	m_lpVsParent = dynamic_cast<OsgRigidBody *>(m_lpThisJoint->Parent());
	if(!m_lpVsParent)
		THROW_ERROR(Osg_Err_lUnableToConvertToVsRigidBody, Osg_Err_strUnableToConvertToVsRigidBody);

	m_lpVsChild = dynamic_cast<OsgRigidBody *>(m_lpThisJoint->Child());
	if(!m_lpVsChild)
		THROW_ERROR(Osg_Err_lUnableToConvertToVsRigidBody, Osg_Err_strUnableToConvertToVsRigidBody);

	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG();

	if(m_osgParent.valid())
	{
		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot = m_vJointGraphicsRotOffset;

		m_osgJointMT = new osg::MatrixTransform();
		m_osgJointMT->setMatrix(SetupMatrix(vPos, vRot));

		m_osgNode = m_osgJointMT.get();

		//Create the sphere.
		CreateJointGraphics();

		BuildLocalMatrix();

		SetAlpha();
		SetCulling();
		SetVisible(m_lpThisMI->IsVisible());

		//Add it to the scene graph.
		m_osgParent->addChild(m_osgRoot.get());

		//Set the position with the world coordinates.
		Physics_UpdateAbsolutePosition();

		//We need to set the UserData on the OSG side so we can do picking.
		//We need to use a node visitor to set the user data for all drawable nodes in all geodes for the group.
		osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void OsgJoint::SetupPhysics()
{
}

void OsgJoint::Initialize()
{
}

osg::Matrix OsgJoint::GetChildWorldMatrix()
{
	if(m_lpVsChild)
		return m_lpVsChild->GetWorldMatrix();
	
	osg::Matrix osgMatrix;
	osgMatrix.makeIdentity();
	return osgMatrix;
}

osg::Matrix OsgJoint::GetChildPhysicsWorldMatrix()
{
	if(m_lpVsChild)
		return m_lpVsChild->GetPhysicsWorldMatrix();
	
	osg::Matrix osgMatrix;
	osgMatrix.makeIdentity();
	return osgMatrix;
}

osg::Matrix OsgJoint::GetChildComMatrix(bool bInvert)
{
	if(m_lpVsChild)
		return m_lpVsChild->GetComMatrix(bInvert);
	
	osg::Matrix osgMatrix;
	osgMatrix.makeIdentity();
	return osgMatrix;
}

//When moving the joint using the drag handler we need to delete the physics for this joint, and then
//recreate it using the new position/orientation.
void OsgJoint::StartGripDrag()
{
    //Delete the physics for the joint
    DeletePhysics(false);
}

//Now recreate the joint.
void OsgJoint::EndGripDrag()
{
    UpdatePositionAndRotationFromMatrix();

    //It does not seem like we should have to reset the graphics here, just the physics.
    //However, we must do this in order to properly reconfigure the OSG matrix transforms that
    //are then used to setup the physics.
    DeleteGraphics();
    SetupGraphics();

    //Now we can setup the physics
    SetupPhysics();
}

void OsgJoint::UpdatePositionAndRotationFromMatrix()
{
    osg::Matrix mtParent = GetParentWorldMatrix();
    osg::Matrix mtChild = GetChildWorldMatrix();
    osg::Matrix mtLocal = m_osgMT->getMatrix();

	//Lets get the world location of the new transform matrix. This is relative to the parent body.
	osg::Matrix mtWorld = mtLocal * mtParent;
 
    //Now calculate the local transform relative to the child for the update
    osg::Matrix mtJointRelToChild = mtWorld * osg::Matrix::inverse(mtChild);

	OsgBody::UpdatePositionAndRotationFromMatrix(mtJointRelToChild);
}

void OsgJoint::Physics_UpdateMatrix()
{
	CStdFPoint vPos = m_lpThisMI->Position();
	CStdFPoint vRot = m_lpThisMI->Rotation();
	LocalMatrix(SetupMatrix(vPos, vRot));
	m_osgMT->setMatrix(m_osgLocalMatrix);

    //If we are here then we did not have a physics component, just and OSG one.
	Physics_UpdateAbsolutePosition();
}

/**
 \brief Builds the local matrix.

 \description This method appears to not be needed here. It is just a pass through. However, 
 if it is not here then the code will not compile because it tries to match to the other overload
 for some reason. Not sure why, but it does.

 \author    David Cofer
 \date  11/18/2013
 */
void  OsgJoint::BuildLocalMatrix()
{
	OsgBody::BuildLocalMatrix();
}

void OsgJoint::BuildLocalMatrix(CStdFPoint localPos, CStdFPoint vLocalOffset, CStdFPoint localRot, std::string strName)
{
	if(!m_osgMT.valid())
	{
		m_osgMT = new osgManipulator::Selection;
		m_osgMT->setName(strName + "_MT");
	}

	if(!m_osgRoot.valid())
	{
		m_osgRoot = new osg::Group;
		m_osgRoot->setName(strName + "_Root");
	}

	if(!m_osgRoot->containsNode(m_osgMT.get()))
		m_osgRoot->addChild(m_osgMT.get());

    //We must add the osg graphics to the parent osg node /because if we do not then the joint will move 
    //when the child moves, and that is not correct. However, the joint is really attached relative to the 
    //child. So we need to calculate the local transform needed relative to the parent.
    osg::Matrix mtParent = GetParentWorldMatrix();
    osg::Matrix mtChild = GetChildWorldMatrix();
    osg::Matrix mtLocal = SetupMatrix(localPos, localRot);

    osg::Matrix mtJointMTFromChild = mtLocal * mtChild;
    osg::Matrix mtLocalRelToParent = mtJointMTFromChild * osg::Matrix::inverse(mtParent);

	LocalMatrix(mtLocalRelToParent);

	//set the matrix to the matrix transform node
	m_osgMT->setMatrix(m_osgLocalMatrix);	
	m_osgMT->setName(strName.c_str());

	//First create the node group. The reason for this is so that we can add other decorated groups on to this node.
	//This is used to add the selected overlays.
	if(!m_osgNodeGroup.valid())
	{
		m_osgNodeGroup = new osg::Group();
		m_osgNodeGroup->addChild(m_osgNode.get());		
		m_osgNodeGroup->setName(strName + "_NodeGroup");

		m_osgMT->addChild(m_osgNodeGroup.get());
	
		CreateSelectedGraphics(strName);
	}
}

bool OsgJoint::Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos)
{
	if(m_lpVsParent && m_lpVsChild)
	{
		fltWorldX *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();
		fltWorldY *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();
		fltWorldZ *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();

		CStdFPoint vPos(fltWorldX, fltWorldY, fltWorldZ), vRot(0, 0, 0);
		osg::Matrix osgWorldPos = SetupMatrix(vPos, vRot);

		//Get the parent object.
		osg::Matrix osgInverse = osg::Matrix::inverse(m_lpVsChild->GetWorldMatrix());

		osg::Matrix osgCalc = osgWorldPos * osgInverse;

		osg::Vec3 vCoord = osgCalc.getTrans();
		vLocalPos.Set(vCoord[0] * m_lpThisAB->GetSimulator()->DistanceUnits(), 
				      vCoord[1] * m_lpThisAB->GetSimulator()->DistanceUnits(), 
				      vCoord[2] * m_lpThisAB->GetSimulator()->DistanceUnits());
		
		return true;
	}

	return false;
}

bool OsgJoint::Physics_SetData(const std::string &strDataType, const std::string &strValue)
{

	if(strDataType == "ATTACHEDPARTMOVEDORROTATED")
	{
		AttachedPartMovedOrRotated(strValue);
		return true;
	}

	return false;
}

void OsgJoint::Physics_QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
}

void OsgJoint::Physics_Resize()
{
    if(Physics_IsDefined())
    {
        DeleteJointGraphics();
        CreateJointGraphics();
        ResetDraggerOnResize();
    }
}

void OsgJoint::Physics_ResetSimulation()
{
	if(Physics_IsDefined())
	{
		m_lpThisJoint->JointPosition(0); 
		m_lpThisJoint->JointVelocity(0);
		m_lpThisJoint->JointForce(0);
	}
}


	}			// Environment
}				//OsgAnimatSim