
#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsHingeLimit.h"
#include "VsHinge.h"
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

VsHinge::VsHinge()
{
	m_lpThis = this;
	m_lpThisJoint = this;
	m_lpPhysicsBody = this;
	m_vxHinge = NULL;
	m_fltRotationDeg = 0;

	m_lpUpperLimit = new VsHingeLimit();
	m_lpLowerLimit = new VsHingeLimit();
	m_lpPosFlap = new VsHingeLimit();

	m_lpUpperLimit->LimitPos(0.25*VX_PI, FALSE);
	m_lpLowerLimit->LimitPos(-0.25*VX_PI, FALSE);
	m_lpPosFlap->LimitPos(Hinge::JointPosition(), FALSE);

	m_lpUpperLimit->Color(1, 1, 1, 1);
	m_lpLowerLimit->Color(1, 0, 0, 1);
	m_lpPosFlap->Color(0, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(TRUE);
	m_lpUpperLimit->IsLowerLimit(FALSE);
}

VsHinge::~VsHinge()
{
	//ConstraintLimits are deleted in the base objects.
}

//If this is a servo motor then the "velocity" signal is not really a velocity signal in this case. 
//It is the desired position and we must convert it to the velocity needed to reach and maintian that position.
void VsHinge::CalculateServoVelocity()
{
	if(!m_vxHinge)
		return;

	float fltError = m_fltDesiredVelocity - m_fltPosition;
/*
	if(m_bEnableLimits)
	{
		if(m_fltDesiredVelocity>m_lpUpperLimit->LimitPos())
			m_fltDesiredVelocity = m_lpUpperLimit->LimitPos();
		if(m_fltDesiredVelocity<m_lpLowerLimit->LimitPos())
			m_fltDesiredVelocity = m_lpLowerLimit->LimitPos();

		float fltProp = fltError / (m_lpUpperLimit->LimitPos()-m_lpLowerLimit->LimitPos());

		m_fltDesiredVelocity = fltProp * m_ftlServoGain; 
	}
	else
		m_fltDesiredVelocity = fltError * m_fltMaxVelocity; 
	*/
}

void VsHinge::SetVelocityToDesired()
{/*
	if(m_bEnableMotor)
	{			
		if(m_bServoMotor)
			CalculateServoVelocity();

		if(m_fltDesiredVelocity>m_fltMaxVelocity)
			m_fltDesiredVelocity = m_fltMaxVelocity;

		if(m_fltDesiredVelocity < -m_fltMaxVelocity)
			m_fltDesiredVelocity = -m_fltMaxVelocity;

		m_fltSetVelocity = m_fltDesiredVelocity;
		m_fltDesiredVelocity = 0;

		//Only do anything if the velocity value has changed
		if( fabs(m_fltVelocity - m_fltSetVelocity) > 1e-4)
		{
			if(fabs(m_fltSetVelocity) > 1e-4)
				VsJoint::SetVelocity(m_fltSetVelocity, m_fltMaxTorque);
			else
				VsJoint::EnableLock(TRUE, m_fltPosition, m_fltMaxTorque);
		}
		
		m_fltPrevVelocity = m_fltSetVelocity;
	}*/
}

void VsHinge::EnableMotor(BOOL bVal)
{
	VsJoint::EnableMotor(bVal, m_fltSetVelocity, m_fltMaxTorque);
	m_bEnableMotor = bVal;
	m_fltPrevVelocity = -1000000;  //reset the prev velocity for the next usage
}


void VsHinge::ResetGraphicsAndPhysics()
{

	VsBody::BuildLocalMatrix();

	SetupPhysics(m_lpSim, m_lpStructure);	
}

void VsHinge::Rotation(CStdFPoint &oPoint) 
{
	m_oRotation = oPoint;
	ResetGraphicsAndPhysics();
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

void VsHinge::CreateCylinderGraphics(Simulator *lpSim, Structure *lpStructure)
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

void VsHinge::SetupGraphics(Simulator *lpSim, Structure *lpStructure)
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG(lpSim, lpStructure);
	osg::ref_ptr<osg::Group> osgChild = ChildOSG(lpSim, lpStructure);

	if(m_osgParent.valid())
	{
		CreateCylinderGraphics(lpSim, lpStructure);

		VsHingeLimit *lpUpperLimit = dynamic_cast<VsHingeLimit *>(m_lpUpperLimit);
		VsHingeLimit *lpLowerLimit = dynamic_cast<VsHingeLimit *>(m_lpLowerLimit);
		VsHingeLimit *lpPosFlap = dynamic_cast<VsHingeLimit *>(m_lpPosFlap);

		lpPosFlap->LimitPos(Hinge::JointPosition());

		lpUpperLimit->SetupGraphics(lpSim, lpStructure);
		lpLowerLimit->SetupGraphics(lpSim, lpStructure);
		lpPosFlap->SetupGraphics(lpSim, lpStructure);

		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
		vPos.Set(0, 0, 0); vRot.Set(0, VX_PI/2, 0); 
		osg::ref_ptr<osg::MatrixTransform> m_osgHingeMT = new osg::MatrixTransform();
		m_osgHingeMT->setMatrix(SetupMatrix(vPos, vRot));

		m_osgHingeMT->addChild(m_osgCylinderMT.get());
		m_osgHingeMT->addChild(lpUpperLimit->FlapTranslateMT());
		m_osgHingeMT->addChild(lpLowerLimit->FlapTranslateMT());
		m_osgHingeMT->addChild(lpPosFlap->FlapTranslateMT());

		m_osgNode = m_osgHingeMT.get();

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

	m_lpChild->EnableCollision(m_lpSim, m_lpParent);
}

void VsHinge::SetupPhysics(Simulator *lpSim, Structure *lpStructure)
{
	if(m_vxHinge)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VxAssembly *lpAssem = (VxAssembly *) lpStructure->Assembly();

	CStdFPoint vGlobal = this->GetOSGWorldCoords();
	
	Vx::VxReal44 vMT;
	VxOSG::copyOsgMatrix_to_VxReal44(this->FinalMatrix(), vMT);
	Vx::VxTransform vTrans(vMT);
	Vx::VxReal3 vxRot;
	vTrans.getRotationEulerAngles(vxRot);

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]); //= m_lpThis->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	VxVector3 axis = NormalizeAxis(vLocalRot);

	m_vxHinge = new VxHinge(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 

	//lpAssem->addConstraint(m_vxHinge);
	lpVsSim->Universe()->addConstraint(m_vxHinge);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(lpSim, m_lpParent);

	VsHingeLimit *lpUpperLimit = dynamic_cast<VsHingeLimit *>(m_lpUpperLimit);
	VsHingeLimit *lpLowerLimit = dynamic_cast<VsHingeLimit *>(m_lpLowerLimit);

	lpUpperLimit->HingeRef(m_vxHinge);
	lpLowerLimit->HingeRef(m_vxHinge);

	//m_vxHinge->setLowerLimit(m_vxHinge->kAngularCoordinate,m_lpLowerLimit->LimitPos(), 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxHinge->setUpperLimit(m_vxHinge->kAngularCoordinate, m_lpUpperLimit->LimitPos(), 0, m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxHinge->setLimitsActive(m_vxHinge->kAngularCoordinate, m_bEnableLimits);	

	m_vxJoint = m_vxHinge;
	m_iCoordID = m_vxHinge->kAngularCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	if(m_bEnableMotor)
		VsJoint::EnableLock(TRUE, m_fltPosition, m_fltMaxTorque);
}

void VsHinge::CreateJoint(Simulator *lpSim, Structure *lpStructure)
{
	SetupGraphics(lpSim, lpStructure);
	SetupPhysics(lpSim, lpStructure);
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

	if(strDataType == "ATTACHEDPARTMOVEDORROTATED")
	{
		AttachedPartMovedOrRotated(strValue);
		return true;
	}
	//else if(strDataType == "ROTATION")
	//{
	//	Rotation(strValue);
	//	return true;
	//}

	if(Hinge::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

//
//void VsHinge::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
//{
//	VsJoint::ResetSimulation(lpSim, lpStructure);
//	Hinge::ResetSimulation(lpSim, lpStructure);
//}

void VsHinge::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	UpdateData(lpSim, lpStructure);
	SetVelocityToDesired();

	//iVal++;
	//if(iVal == 10000)
	//	Hinge::Rotation(0, VX_PI/2, 0);
	//if(iVal == 10000)
	//	ConstraintHigh(0.75*VX_PI);
}

void VsHinge::UpdateData(Simulator *lpSim, Structure *lpStructure)
{
	Hinge::UpdateData(lpSim, lpStructure);
	m_fltRotationDeg = ((m_fltPosition/VX_PI)*180);
}

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
