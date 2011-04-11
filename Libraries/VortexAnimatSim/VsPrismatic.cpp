/**
\file	VsPrismatic.cpp

\brief	Implements the vs prismatic class.
**/

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsPrismaticLimit.h"
#include "VsRigidBody.h"
#include "VsPrismatic.h"
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
\date	3/31/2011
**/
VsPrismatic::VsPrismatic()
{
	m_lpThis = this;
	m_lpThisJoint = this;
	m_lpPhysicsBody = this;
	m_lpPhysicsMotorJoint = this;
	m_lpThisMotorJoint = this;
	m_vxPrismatic = NULL;

	m_lpUpperLimit = new VsPrismaticLimit();
	m_lpLowerLimit = new VsPrismaticLimit();
	m_lpPosFlap = new VsPrismaticLimit();

	m_lpUpperLimit->LimitPos(1, FALSE);
	m_lpLowerLimit->LimitPos(-1, FALSE);
	m_lpPosFlap->LimitPos(Prismatic::JointPosition(), FALSE);
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
\date	3/31/2011
**/
VsPrismatic::~VsPrismatic()
{
	//ConstraintLimits are deleted in the base objects.
}


void VsPrismatic::EnableLimits(BOOL bVal)
{
	Prismatic::EnableLimits(bVal);

	if(m_vxPrismatic)
		m_vxPrismatic->setLimitsActive(m_vxPrismatic->kLinearCoordinate, m_bEnableLimits);	

	if(m_bEnableLimits)
	{
		if(m_lpLowerLimit) m_lpLowerLimit->SetLimitPos();
		if(m_lpUpperLimit) m_lpUpperLimit->SetLimitPos();
	}
}


void VsPrismatic::ResetGraphicsAndPhysics()
{
	VsBody::BuildLocalMatrix();

	SetupPhysics();	
}

void VsPrismatic::Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	m_oRotation = oPoint;
	m_oReportRotation = m_oRotation;

	ResetGraphicsAndPhysics();

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->RotationChanged();
}

void VsPrismatic::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}


void VsPrismatic::SetAlpha()
{
	VsJoint::SetAlpha();

	m_lpUpperLimit->Alpha(m_fltAlpha);
	m_lpLowerLimit->Alpha(m_fltAlpha);
	m_lpPosFlap->Alpha(m_fltAlpha);

	if(m_osgCylinderMat.valid() && m_osgCylinderSS.valid())
		SetMaterialAlpha(m_osgCylinderMat.get(), m_osgCylinderSS.get(), m_fltAlpha);

}

void VsPrismatic::CreateCylinderGraphics()
{
	//Create the cylinder for the Prismatic
	m_osgCylinder = CreateConeGeometry(CylinderHeight(), CylinderRadius(), CylinderRadius(), 10, true, true, true);
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

void VsPrismatic::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG();
	osg::ref_ptr<osg::Group> osgChild = ChildOSG();

	if(m_osgParent.valid())
	{
		CreateCylinderGraphics();

		VsPrismaticLimit *lpUpperLimit = dynamic_cast<VsPrismaticLimit *>(m_lpUpperLimit);
		VsPrismaticLimit *lpLowerLimit = dynamic_cast<VsPrismaticLimit *>(m_lpLowerLimit);
		VsPrismaticLimit *lpPosFlap = dynamic_cast<VsPrismaticLimit *>(m_lpPosFlap);

		lpPosFlap->LimitPos(Prismatic::JointPosition());

		lpUpperLimit->SetupGraphics();
		lpLowerLimit->SetupGraphics();
		lpPosFlap->SetupGraphics();

		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
		vPos.Set(0, 0, 0); vRot.Set(0, VX_PI/2, 0); 
		osg::ref_ptr<osg::MatrixTransform> m_osgPrismaticMT = new osg::MatrixTransform();
		m_osgPrismaticMT->setMatrix(SetupMatrix(vPos, vRot));

		m_osgPrismaticMT->addChild(m_osgCylinderMT.get());
		m_osgPrismaticMT->addChild(lpUpperLimit->BoxTranslateMT());
		m_osgPrismaticMT->addChild(lpLowerLimit->BoxTranslateMT());
		m_osgPrismaticMT->addChild(lpPosFlap->BoxTranslateMT());

		m_osgNode = m_osgPrismaticMT.get();

		VsBody::BuildLocalMatrix();

		SetAlpha();
		SetCulling();
		SetVisible(m_lpThis->IsVisible());

		//Add it to the scene graph.
		m_osgParent->addChild(m_osgRoot.get());

		//Set the position with the world coordinates.
		m_lpThis->AbsolutePosition(VsBody::GetOSGWorldCoords());

		//We need to set the UserData on the OSG side so we can do picking.
		//We need to use a node visitor to set the user data for all drawable nodes in all geodes for the group.
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void VsPrismatic::DeletePhysics()
{
	if(!m_vxPrismatic)
		return;

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	lpVsSim->Universe()->removeConstraint(m_vxPrismatic);
	delete m_vxPrismatic;
	m_vxPrismatic = NULL;
	m_vxJoint = NULL;

	m_lpChild->EnableCollision(m_lpParent);
}

void VsPrismatic::SetupPhysics()
{
	if(m_vxPrismatic)
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

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]);

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	VxVector3 axis = NormalizeAxis(vLocalRot);

	m_vxPrismatic = new VxPrismatic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 

	//lpAssem->addConstraint(m_vxPrismatic);
	lpVsSim->Universe()->addConstraint(m_vxPrismatic);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	VsPrismaticLimit *lpUpperLimit = dynamic_cast<VsPrismaticLimit *>(m_lpUpperLimit);
	VsPrismaticLimit *lpLowerLimit = dynamic_cast<VsPrismaticLimit *>(m_lpLowerLimit);

	lpUpperLimit->PrismaticRef(m_vxPrismatic);
	lpLowerLimit->PrismaticRef(m_vxPrismatic);

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	m_vxJoint = m_vxPrismatic;
	m_iCoordID = m_vxPrismatic->kLinearCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	if(m_bEnableMotor)
		EnableLock(TRUE, m_fltPosition, m_fltMaxForce);
}

void VsPrismatic::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}


#pragma region DataAccesMethods

float *VsPrismatic::GetDataPointer(string strDataType)
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
		lpData = Prismatic::GetDataPointer(strDataType);
		if(lpData) return lpData;

		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);
	}

	return lpData;
}

BOOL VsPrismatic::SetData(string strDataType, string strValue, BOOL bThrowError)
{

	if(strDataType == "ATTACHEDPARTMOVEDORROTATED")
	{
		AttachedPartMovedOrRotated(strValue);
		return true;
	}

	if(Prismatic::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void VsPrismatic::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
