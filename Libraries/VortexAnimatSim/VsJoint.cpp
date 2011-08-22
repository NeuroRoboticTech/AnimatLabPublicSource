// VsJoint.cpp: implementation of the VsJoint class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsJoint::VsJoint()
{
	m_vxJoint = NULL;
	m_iCoordID = -1; //-1 if not used.
}

VsJoint::~VsJoint()
{
}

void VsJoint::SetThisPointers()
{
	VsBody::SetThisPointers();

	m_lpThisJoint = dynamic_cast<Joint *>(this);
	if(!m_lpThisJoint)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisJoint, " + m_lpThisAB->Name());

	m_lpThisJoint->PhysicsBody(this);
}

VxVector3 VsJoint::NormalizeAxis(CStdFPoint vLocalRot)
{
	osg::Vec3 vPosN(1, 0, 0);
	CStdFPoint vMatrixPos(0, 0, 0);

	osg::Matrix osgMT = SetupMatrix(vMatrixPos, vLocalRot);

	osg::Vec3 vNorm = vPosN * osgMT;

	VxVector3 axis((double) vNorm[0], (double) vNorm[1], (double) vNorm[2]);

	return axis;
}

void VsJoint::UpdatePosition()
{	
	Vx::VxReal3 vPos;
	m_vxJoint->getPartAttachmentPosition(0, vPos);

	UpdateWorldMatrix();
	m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
}

void VsJoint::Physics_CollectData()
{
	if(m_lpThisJoint && m_vxJoint)
	{
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThisJoint->Parent());
		if(!lpVsParent)
			THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

		UpdatePosition();

		//Only attempt to make these calls if the coordinate ID is a valid number.
		if(m_iCoordID >= 0)
		{
			if(m_vxJoint->isAngular(m_iCoordID) == true)
			{
				m_lpThisJoint->JointPosition(m_vxJoint->getCoordinateCurrentPosition (m_iCoordID)); 
				m_lpThisJoint->JointVelocity(m_vxJoint->getCoordinateVelocity (m_iCoordID));
				m_lpThisJoint->JointForce(m_vxJoint->getCoordinateForce (m_iCoordID));
			}
			else
			{
				float fltDistanceUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
				float fltMassUnits = m_lpThisAB->GetSimulator()->MassUnits();

				m_lpThisJoint->JointPosition(m_vxJoint->getCoordinateCurrentPosition (m_iCoordID) * fltDistanceUnits); 
				m_lpThisJoint->JointVelocity(m_vxJoint->getCoordinateVelocity(m_iCoordID) * fltDistanceUnits);
				m_lpThisJoint->JointForce(m_vxJoint->getCoordinateForce(m_iCoordID) * fltMassUnits * fltDistanceUnits);
			}
		}
	}
}

osg::Group *VsJoint::ParentOSG()
{
	RigidBody *lpParent = m_lpThisJoint->Parent();
	if(lpParent)
	{
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(lpParent);
		if(lpVsParent) return lpVsParent->GetMatrixTransform();
	}

	return NULL;
}

osg::Group *VsJoint::ChildOSG()
{
	RigidBody *lpChild = m_lpThisJoint->Child();
	if(lpChild)
	{
		VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(lpChild);
		if(lpVsChild) return lpVsChild->GetMatrixTransform();
	}

	return NULL;
}

void VsJoint::SetAlpha()
{
	VsBody::SetAlpha();

	if(m_osgDefaultBallMat.valid() && m_osgDefaultBallSS.valid())
		SetMaterialAlpha(m_osgDefaultBallMat.get(), m_osgDefaultBallSS.get(), m_lpThisMI->Alpha());
}

void VsJoint::Physics_PositionChanged()
{
	VsBody::Physics_PositionChanged();
	Physics_ResetGraphicsAndPhysics();
}

void VsJoint::Physics_RotationChanged()
{
	Physics_ResetGraphicsAndPhysics();
}

// TODO !!!! This method needs to be implemented.
void VsJoint::Physics_Resize()
{
}

/**
\brief	Creates the default ball graphics.

\author	dcofer
\date	4/15/2011
**/
void VsJoint::CreateJointGraphics()
{
	//Create the cylinder for the hinge
	m_osgDefaultBall = CreateSphereGeometry(15, 15, m_lpThisJoint->Size());
	osg::ref_ptr<osg::Geode> osgBall = new osg::Geode;
	osgBall->addDrawable(m_osgDefaultBall.get());

	CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
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
void VsJoint::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG();
	osg::ref_ptr<osg::Group> osgChild = ChildOSG();

	if(m_osgParent.valid())
	{
		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
		vPos.Set(0, 0, 0); vRot.Set(0, VX_PI/2, 0); 
		
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
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void VsJoint::SetupPhysics()
{
}

void VsJoint::Initialize()
{
}

void VsJoint::SetBody()
{
}

void VsJoint::LocalMatrix(osg::Matrix osgLocalMT)
{
	m_osgLocalMatrix = osgLocalMT;
	m_osgFinalMatrix = m_osgLocalMatrix * m_osgChildOffsetMatrix;
	UpdateWorldMatrix();
}

void VsJoint::ChildOffsetMatrix(osg::Matrix osgMT)
{
	m_osgChildOffsetMatrix = osgMT;
	m_osgFinalMatrix = m_osgLocalMatrix * m_osgChildOffsetMatrix;
	m_osgChildOffsetMT->setMatrix(m_osgChildOffsetMatrix);
}

void VsJoint::UpdatePositionAndRotationFromMatrix()
{
	VsBody::UpdatePositionAndRotationFromMatrix();
	SetupPhysics();
}

void VsJoint::Physics_UpdateMatrix()
{
	LocalMatrix(SetupMatrix(m_lpThisMI->Position(), m_lpThisMI->Rotation()));
	m_osgMT->setMatrix(m_osgLocalMatrix);
	m_osgDragger->SetupMatrix();

	//Now lets get the localmatrix from the child object and use that for our offsetmatrix
	VsBody *lpVsChild = dynamic_cast<VsBody *>(m_lpThisJoint->Child());
	if(lpVsChild)
		ChildOffsetMatrix(lpVsChild->LocalMatrix());

	//If we are here then we did not have a physics component, just and OSG one.
	Physics_UpdateAbsolutePosition();
}

void  VsJoint::BuildLocalMatrix()
{
	VsBody::BuildLocalMatrix();
}

void VsJoint::BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName)
{
	if(!m_osgMT.valid())
	{
		m_osgMT = new osgManipulator::Selection;
		m_osgMT->setName(strName + "_MT");
	}

	if(!m_osgChildOffsetMT.valid())
	{
		m_osgChildOffsetMT = new osg::MatrixTransform;
		m_osgChildOffsetMT->setName(strName + "ChildOffsetMT");
		m_osgChildOffsetMT->addChild(m_osgMT.get());
	}

	if(!m_osgRoot.valid())
	{
		m_osgRoot = new osg::Group;
		m_osgRoot->setName(strName + "_Root");
		m_osgRoot->addChild(m_osgChildOffsetMT.get());
	}

	LocalMatrix(SetupMatrix(localPos, localRot));

	//set the matrix to the matrix transform node
	m_osgMT->setMatrix(m_osgLocalMatrix);	
	m_osgMT->setName(strName.c_str());

	//Now lets get the localmatrix from the child object and use that for our offsetmatrix
	VsBody *lpVsChild = dynamic_cast<VsBody *>(m_lpThisJoint->Child());
	if(lpVsChild)
		ChildOffsetMatrix(lpVsChild->LocalMatrix());

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

BOOL VsJoint::Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos)
{
	VsMovableItem *lpParent = m_lpThisVsMI->VsParent();

	if(lpParent)
	{
		fltWorldX *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();
		fltWorldY *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();
		fltWorldZ *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();

		CStdFPoint vPos(fltWorldX, fltWorldY, fltWorldZ), vRot(0, 0, 0);
		osg::Matrix osgWorldPos = SetupMatrix(vPos, vRot);

		osg::Matrix osgChildOffsetInverse = osg::Matrix::inverse(m_osgChildOffsetMatrix);

		osgWorldPos = osgWorldPos * osgChildOffsetInverse;

		//Get the parent object.
		osg::Matrix osgInverse = osg::Matrix::inverse(lpParent->GetWorldMatrix());

		osg::Matrix osgCalc = osgWorldPos * osgInverse;

		osg::Vec3 vCoord = osgCalc.getTrans();
		vLocalPos.Set(vCoord[0] * m_lpThisAB->GetSimulator()->DistanceUnits(), 
				      vCoord[1] * m_lpThisAB->GetSimulator()->DistanceUnits(), 
				      vCoord[2] * m_lpThisAB->GetSimulator()->DistanceUnits());
		
		return TRUE;
	}

	return FALSE;
}

void VsJoint::Physics_ResetSimulation()
{
	if(m_vxJoint)
	{
		m_vxJoint->resetDynamics();

		UpdatePosition();
		m_lpThisJoint->JointPosition(0); 
		m_lpThisJoint->JointVelocity(0);
		m_lpThisJoint->JointForce(0);
	}
}


BOOL VsJoint::Physics_SetData(string strDataType, string strValue)
{

	if(strDataType == "ATTACHEDPARTMOVEDORROTATED")
	{
		AttachedPartMovedOrRotated(strValue);
		return true;
	}

	return FALSE;
}

	}			// Environment
}				//VortexAnimatSim