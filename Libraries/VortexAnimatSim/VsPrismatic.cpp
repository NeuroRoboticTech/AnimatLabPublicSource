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

	m_lpUpperLimit->LimitPos(0.25*VX_PI, FALSE);
	m_lpLowerLimit->LimitPos(-0.25*VX_PI, FALSE);
	m_lpPosFlap->LimitPos(Prismatic::JointPosition(), FALSE);

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
{/*
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

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]); //= m_lpThis->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	VxVector3 axis = NormalizeAxis(vLocalRot);

	m_vxPrismatic = new VxPrismatic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 

	//lpAssem->addConstraint(m_vxPrismatic);
	lpVsSim->Universe()->addConstraint(m_vxPrismatic);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	VsPrismaticLimit *lpUpperLimit = dynamic_cast<VsPrismaticLimit *>(m_lpUpperLimit);
	VsPrismaticLimit *lpLowerLimit = dynamic_cast<VsPrismaticLimit *>(m_lpLowerLimit);

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	lpUpperLimit->PrismaticRef(m_vxPrismatic);
	lpLowerLimit->PrismaticRef(m_vxPrismatic);

	m_vxJoint = m_vxPrismatic;
	m_iCoordID = m_vxPrismatic->kAngularCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	if(m_bEnableMotor)
		EnableLock(TRUE, m_fltPosition, m_fltMaxForce);
		*/
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



/*
void VsPrismatic::CreateJoint()
{
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

	CStdFPoint vChildPos = lpVsChild->GetOSGWorldCoords();
	CStdFPoint vGlobal = vChildPos + m_lpThis->LocalPosition();
	CStdFPoint vLocalRot = m_lpThis->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
    VxVector3 axis((double) vLocalRot.x, (double) vLocalRot.y, (double) vLocalRot.z); 
	m_vxPrismatic = new VxPrismatic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 

	//lpAssem->addConstraint(m_vxPrismatic);
	lpVsSim->Universe()->addConstraint(m_vxPrismatic);
	m_lpStructure->AddCollisionPair(m_lpParent->ID(), m_lpChild->ID());

	m_vxPrismatic->setLowerLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintLow);
	m_vxPrismatic->setUpperLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintHigh);
	//m_vxPrismatic->setLimitsActive(m_vxPrismatic->kLinearCoordinate, m_bEnableLimits);

	//m_vxPrismatic->setLowerLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintLow, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxPrismatic->setUpperLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintHigh, 0, m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxPrismatic->setLimitsActive(m_vxPrismatic->kLinearCoordinate, m_bEnableLimits);	

    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintP1, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintP2, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintA0, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintA1, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintA2, m_fltStiffness, m_fltDamping, 0, true );

	m_vxJoint = m_vxPrismatic;
	m_iCoordID = m_vxPrismatic->kLinearCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	if(m_bEnableMotor)
		EnableLock(TRUE, m_fltPosition, m_fltMaxForce);

	m_fltDistanceUnits = m_lpSim->DistanceUnits();
}
*/


		}		//Joints
	}			// Environment
}				//VortexAnimatSim
