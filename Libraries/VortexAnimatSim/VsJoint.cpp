// VsJoint.cpp: implementation of the VsJoint class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
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
}

VsJoint::~VsJoint()
{
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

	m_lpThis->AbsolutePosition(vPos[0], vPos[0], vPos[0]);
	m_lpThis->ReportWorldPosition(m_lpThis->AbsolutePosition() * m_lpThis->GetSimulator()->DistanceUnits());
}

void VsJoint::Physics_CollectBodyData()
{
	if(m_lpThis && m_vxJoint)
	{
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThis->Parent());
		if(!lpVsParent)
			THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

		UpdatePosition();

		if(m_vxJoint->isAngular(m_iCoordID) == true)
		{
			m_lpThisJoint->JointPosition(m_vxJoint->getCoordinateCurrentPosition (m_iCoordID)); 
			m_lpThisJoint->JointVelocity(m_vxJoint->getCoordinateVelocity (m_iCoordID));
			m_lpThisJoint->JointForce(m_vxJoint->getCoordinateForce (m_iCoordID));
		}
		else
		{
			float fltDistanceUnits = m_lpThis->GetSimulator()->DistanceUnits();
			float fltMassUnits = m_lpThis->GetSimulator()->MassUnits();

			m_lpThisJoint->JointPosition(m_vxJoint->getCoordinateCurrentPosition (m_iCoordID) * fltDistanceUnits); 
			m_lpThisJoint->JointVelocity(m_vxJoint->getCoordinateVelocity(m_iCoordID) * fltDistanceUnits);
			m_lpThisJoint->JointForce(m_vxJoint->getCoordinateForce(m_iCoordID) * fltMassUnits * fltDistanceUnits);
		}
	}
}

float *VsJoint::Physics_GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	Joint *lpBody = dynamic_cast<Joint *>(this);

	return NULL;
}

osg::Group *VsJoint::ParentOSG()
{
	RigidBody *lpParent = m_lpThis->Parent();
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

void VsJoint::SetupGraphics()
{
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
	LocalMatrix(SetupMatrix(m_lpThis->LocalPosition(), m_lpThis->Rotation()));
	m_osgMT->setMatrix(m_osgLocalMatrix);
	m_osgDragger->SetupMatrix();

	//Now lets get the localmatrix from the child object and use that for our offsetmatrix
	VsBody *lpVsChild = dynamic_cast<VsBody *>(m_lpThisJoint->Child());
	if(lpVsChild)
		ChildOffsetMatrix(lpVsChild->LocalMatrix());

	//If we are here then we did not have a physics component, just and OSG one.
	CStdFPoint vPos = VsBody::GetOSGWorldCoords();
	vPos.ClearNearZero();
	m_lpThis->AbsolutePosition(vPos.x, vPos.y, vPos.z);
	m_lpThis->ReportWorldPosition(m_lpThis->AbsolutePosition() * m_lpThis->GetSimulator()->DistanceUnits());
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

	}			// Environment
}				//VortexAnimatSim