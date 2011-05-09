/**
\file	VsHinge.cpp

\brief	Implements the vortex hinge class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsJoint.h"
#include "VsHingeLimit.h"
#include "VsHinge.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

/**
\brief	Default constructor.

\author	dcofer
\date	4/15/2011
**/
VsHinge::VsHinge()
{
	SetThisPointers();
	m_vxHinge = NULL;
	m_fltRotationDeg = 0;

	m_lpUpperLimit = new VsHingeLimit();
	m_lpLowerLimit = new VsHingeLimit();
	m_lpPosFlap = new VsHingeLimit();

	m_lpUpperLimit->LimitPos(0.25*VX_PI, FALSE);
	m_lpLowerLimit->LimitPos(-0.25*VX_PI, FALSE);
	m_lpPosFlap->LimitPos(Hinge::JointPosition(), FALSE);
	m_lpPosFlap->IsShowPosition(TRUE);

	m_lpUpperLimit->Color(1, 1, 1, 1);
	m_lpLowerLimit->Color(1, 0, 0, 1);
	m_lpPosFlap->Color(0, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(TRUE);
	m_lpUpperLimit->IsLowerLimit(FALSE);
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
VsHinge::~VsHinge()
{
	//ConstraintLimits are deleted in the base objects.
}

void VsHinge::EnableLimits(BOOL bVal)
{
	Hinge::EnableLimits(bVal);

	if(m_vxHinge)
		m_vxHinge->setLimitsActive(m_vxHinge->kAngularCoordinate, m_bEnableLimits);	

	if(m_bEnableLimits)
	{
		if(m_lpLowerLimit) m_lpLowerLimit->SetLimitPos();
		if(m_lpUpperLimit) m_lpUpperLimit->SetLimitPos();
	}
}

void VsHinge::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}

void VsHinge::SetAlpha()
{
	VsJoint::SetAlpha();

	m_lpUpperLimit->Alpha(m_fltAlpha);
	m_lpLowerLimit->Alpha(m_fltAlpha);
	m_lpPosFlap->Alpha(m_fltAlpha);

	if(m_osgCylinderMat.valid() && m_osgCylinderSS.valid())
		SetMaterialAlpha(m_osgCylinderMat.get(), m_osgCylinderSS.get(), m_fltAlpha);

}

/**
\brief	Creates the cylinder graphics.

\author	dcofer
\date	4/15/2011
**/
void VsHinge::CreateCylinderGraphics()
{
	//Create the cylinder for the hinge
	m_osgCylinder = CreateConeGeometry(CylinderHeight(), CylinderRadius(), CylinderRadius(), 30, true, true, true);
	osg::ref_ptr<osg::Geode> osgCylinder = new osg::Geode;
	osgCylinder->addDrawable(m_osgCylinder.get());

	CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
	m_osgCylinderMT = new osg::MatrixTransform();
	m_osgCylinderMT->setMatrix(SetupMatrix(vPos, vRot));
	m_osgCylinderMT->addChild(osgCylinder.get());

	//create a material to use with the pos flap
	if(!m_osgCylinderMat.valid())
		m_osgCylinderMat = new osg::Material();		

	//create a stateset for this node
	m_osgCylinderSS = m_osgCylinderMT->getOrCreateStateSet();

	//set the diffuse property of this node to the color of this body	
	m_osgCylinderMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
	m_osgCylinderMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(1, 0.25, 1, 1));
	m_osgCylinderMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
	m_osgCylinderMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
	m_osgCylinderSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

	//apply the material
	m_osgCylinderSS->setAttribute(m_osgCylinderMat.get(), osg::StateAttribute::ON);
}

void VsHinge::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG();
	osg::ref_ptr<osg::Group> osgChild = ChildOSG();

	if(m_osgParent.valid())
	{
		CreateCylinderGraphics();

		VsHingeLimit *lpUpperLimit = dynamic_cast<VsHingeLimit *>(m_lpUpperLimit);
		VsHingeLimit *lpLowerLimit = dynamic_cast<VsHingeLimit *>(m_lpLowerLimit);
		VsHingeLimit *lpPosFlap = dynamic_cast<VsHingeLimit *>(m_lpPosFlap);

		lpPosFlap->LimitPos(Hinge::JointPosition());

		lpUpperLimit->SetupGraphics();
		lpLowerLimit->SetupGraphics();
		lpPosFlap->SetupGraphics();

		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
		vPos.Set(0, 0, 0); vRot.Set(0, VX_PI/2, 0); 
		
		m_osgJointMT = new osg::MatrixTransform();
		m_osgJointMT->setMatrix(SetupMatrix(vPos, vRot));

		m_osgJointMT->addChild(m_osgCylinderMT.get());
		m_osgJointMT->addChild(lpUpperLimit->FlapTranslateMT());
		m_osgJointMT->addChild(lpLowerLimit->FlapTranslateMT());
		m_osgJointMT->addChild(lpPosFlap->FlapTranslateMT());

		m_osgNode = m_osgJointMT.get();

		VsBody::BuildLocalMatrix();

		SetAlpha();
		SetCulling();
		SetVisible(m_lpThisMI->IsVisible());

		//Add it to the scene graph.
		m_osgParent->addChild(m_osgRoot.get());

		//Set the position with the world coordinates.
		UpdateAbsolutePosition();

		//We need to set the UserData on the OSG side so we can do picking.
		//We need to use a node visitor to set the user data for all drawable nodes in all geodes for the group.
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void VsHinge::DeletePhysics()
{
	if(!m_vxHinge)
		return;

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	lpVsSim->Universe()->removeConstraint(m_vxHinge);
	delete m_vxHinge;
	m_vxHinge = NULL;
	m_vxJoint = NULL;

	m_lpChild->EnableCollision(m_lpParent);
}

void VsHinge::SetupPhysics()
{
	if(m_vxHinge)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VxAssembly *lpAssem = (VxAssembly *) m_lpStructure->Assembly();

	CStdFPoint vGlobal = this->GetOSGWorldCoords();
	
	Vx::VxReal44 vMT;
	VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(), vMT);
	Vx::VxTransform vTrans(vMT);
	Vx::VxReal3 vxRot;
	vTrans.getRotationEulerAngles(vxRot);

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]); //= m_lpThisMI->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	VxVector3 axis = NormalizeAxis(vLocalRot);

	m_vxHinge = new VxHinge(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 
	m_vxHinge->setName(m_strID.c_str());

	//lpAssem->addConstraint(m_vxHinge);
	lpVsSim->Universe()->addConstraint(m_vxHinge);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	VsHingeLimit *lpUpperLimit = dynamic_cast<VsHingeLimit *>(m_lpUpperLimit);
	VsHingeLimit *lpLowerLimit = dynamic_cast<VsHingeLimit *>(m_lpLowerLimit);

	lpUpperLimit->HingeRef(m_vxHinge);
	lpLowerLimit->HingeRef(m_vxHinge);

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	m_vxJoint = m_vxHinge;
	m_iCoordID = m_vxHinge->kAngularCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	if(m_bEnableMotor)
		EnableLock(TRUE, m_fltPosition, m_fltMaxForce);
}

void VsHinge::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}


#pragma region DataAccesMethods

float *VsHinge::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "JOINTROTATION")
		return &m_fltPosition;
	else if(strType == "JOINTPOSITION")
		return &m_fltPosition;
	else if(strType == "JOINTACTUALVELOCITY")
		return &m_fltVelocity;
	else if(strType == "JOINTFORCE")
		return &m_fltForce;
	else if(strType == "JOINTROTATIONDEG")
		return &m_fltRotationDeg;
	else if(strType == "JOINTDESIREDVELOCITY")
		return &m_fltSetVelocity;
	else if(strType == "JOINTSETVELOCITY")
		return &m_fltSetVelocity;
	else if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "CONTACTCOUNT")
		THROW_PARAM_ERROR(Al_Err_lMustBeContactBodyToGetCount, Al_Err_strMustBeContactBodyToGetCount, "JointID", m_strName);
	else
	{
		lpData = Hinge::GetDataPointer(strDataType);
		if(lpData) return lpData;

		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);
	}

	return lpData;
}

BOOL VsHinge::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(VsJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Hinge::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void VsHinge::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

void VsHinge::UpdateData()
{
	Hinge::UpdateData();
	m_fltRotationDeg = ((m_fltPosition/VX_PI)*180);
}

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
