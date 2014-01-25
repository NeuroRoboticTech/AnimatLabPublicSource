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

	m_lpUpperLimit->LimitPos(0.25*VX_PI, false);
	m_lpLowerLimit->LimitPos(-0.25*VX_PI, false);
	m_lpPosFlap->LimitPos(Hinge::JointPosition(), false);
	m_lpPosFlap->IsShowPosition(true);

	m_lpUpperLimit->Color(1, 0, 0, 1);
	m_lpLowerLimit->Color(1, 1, 1, 1);
	m_lpPosFlap->Color(0, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(true);
	m_lpUpperLimit->IsLowerLimit(false);
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
VsHinge::~VsHinge()
{
	//ConstraintLimits are deleted in the base objects.
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsHinge\r\n", "", -1, false, true);}
}

void VsHinge::EnableLimits(bool bVal)
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

void VsHinge::DeleteJointGraphics()
{
	VsHingeLimit *lpUpperLimit = dynamic_cast<VsHingeLimit *>(m_lpUpperLimit);
	VsHingeLimit *lpLowerLimit = dynamic_cast<VsHingeLimit *>(m_lpLowerLimit);
	VsHingeLimit *lpPosFlap = dynamic_cast<VsHingeLimit *>(m_lpPosFlap);

    if(m_osgJointMT.valid())
    {
        if(m_osgCylinderMT.valid()) m_osgJointMT->removeChild(m_osgCylinderMT.get());
		if(lpUpperLimit && lpUpperLimit->FlapTranslateMT()) m_osgJointMT->removeChild(lpUpperLimit->FlapTranslateMT());
		if(lpLowerLimit && lpLowerLimit->FlapTranslateMT()) m_osgJointMT->removeChild(lpLowerLimit->FlapTranslateMT());
		if(lpPosFlap && lpPosFlap->FlapTranslateMT()) m_osgJointMT->removeChild(lpPosFlap->FlapTranslateMT());
    }

    m_osgCylinder.release();
    m_osgCylinderGeode.release();
    m_osgCylinderMT.release();
    m_osgCylinderMat.release();
    m_osgCylinderSS.release();

    if(m_lpUpperLimit) m_lpUpperLimit->DeleteGraphics();
    if(m_lpLowerLimit) m_lpLowerLimit->DeleteGraphics();
    if(m_lpPosFlap) m_lpPosFlap->DeleteGraphics();
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
	m_osgCylinderGeode = new osg::Geode;
	m_osgCylinderGeode->addDrawable(m_osgCylinder.get());

	CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
	m_osgCylinderMT = new osg::MatrixTransform();
	m_osgCylinderMT->setMatrix(SetupMatrix(vPos, vRot));
	m_osgCylinderMT->addChild(m_osgCylinderGeode.get());

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

void VsHinge::CreateJointGraphics()
{
	CreateCylinderGraphics();

	VsHingeLimit *lpUpperLimit = dynamic_cast<VsHingeLimit *>(m_lpUpperLimit);
	VsHingeLimit *lpLowerLimit = dynamic_cast<VsHingeLimit *>(m_lpLowerLimit);
	VsHingeLimit *lpPosFlap = dynamic_cast<VsHingeLimit *>(m_lpPosFlap);

	lpPosFlap->LimitPos(Hinge::JointPosition());

	lpUpperLimit->SetupGraphics();
	lpLowerLimit->SetupGraphics();
	lpPosFlap->SetupGraphics();

	m_osgJointMT->addChild(m_osgCylinderMT.get());
	m_osgJointMT->addChild(lpUpperLimit->FlapTranslateMT());
	m_osgJointMT->addChild(lpLowerLimit->FlapTranslateMT());
	m_osgJointMT->addChild(lpPosFlap->FlapTranslateMT());
}

void VsHinge::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG();

	if(m_osgParent.valid())
	{
		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
		vPos.Set(0, 0, 0); vRot.Set(0, VX_PI/2, 0); 
		
		m_osgJointMT = new osg::MatrixTransform();
		m_osgJointMT->setMatrix(SetupMatrix(vPos, vRot));

        CreateJointGraphics();

		m_osgNode = m_osgJointMT.get();

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

void VsHinge::DeletePhysics()
{
	if(!m_vxHinge)
		return;

	if(GetVsSimulator() && GetVsSimulator()->Universe())
	{
		GetVsSimulator()->Universe()->removeConstraint(m_vxHinge);
		delete m_vxHinge;
		
		if(m_lpChild && m_lpParent)
			m_lpChild->EnableCollision(m_lpParent);
	}

	m_vxHinge = NULL;
	m_vxJoint = NULL;
}

void VsHinge::SetupPhysics()
{
	if(m_vxHinge)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

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

    GetVsSimulator()->Universe()->addConstraint(m_vxHinge);

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
	EnableMotor(m_bEnableMotorInit);

    Hinge::Initialize();
    VsJoint::Initialize();
}

void VsHinge::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

float *VsHinge::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

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
		return &m_fltReportSetVelocity;
	else if(strType == "JOINTSETVELOCITY")
		return &m_fltReportSetVelocity;
	else if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "MOTORFORCETOAX")
		return &m_fltNullReport;
	else if(strType == "MOTORFORCETOAY")
		return &m_fltNullReport;
	else if(strType == "MOTORFORCETOAZ")
		return &m_fltNullReport;
	else if(strType == "MOTORFORCETOBX")
		return &m_fltNullReport;
	else if(strType == "MOTORFORCETOBY")
		return &m_fltNullReport;
	else if(strType == "MOTORFORCETOBZ")
		return &m_fltNullReport;
	else if(strType == "MOTORTORQUETOAX")
		return &m_fltNullReport;
	else if(strType == "MOTORTORQUETOAY")
		return &m_fltNullReport;
	else if(strType == "MOTORTORQUETOAZ")
		return &m_fltNullReport;
	else if(strType == "MOTORTORQUETOBX")
		return &m_fltNullReport;
	else if(strType == "MOTORTORQUETOBY")
		return &m_fltNullReport;
	else if(strType == "MOTORTORQUETOBZ")
		return &m_fltNullReport;
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

bool VsHinge::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(VsJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Hinge::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void VsHinge::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	VsJoint::Physics_QueryProperties(aryNames, aryTypes);
	Hinge::QueryProperties(aryNames, aryTypes);
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
