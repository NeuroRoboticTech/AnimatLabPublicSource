// VsHinge.cpp: implementation of the VsHinge class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsHinge::VsHinge()
{
	m_lpThis = this;
	m_lpThisJoint = this;
	m_lpPhysicsBody = this;
	m_vxHinge = NULL;
	m_fltRotationDeg = 0;
	m_fltConstraintLow = -0.25*VX_PI;
	m_fltConstraintHigh = 0.25*VX_PI;
	iVal = 0;
}

VsHinge::~VsHinge()
{

}
//
//void VsHinge::Selected(BOOL bValue, BOOL bSelectMultiple)  
//{
//	Hinge::Selected(bValue, bSelectMultiple);
//	VsJoint::Selected(bValue, bSelectMultiple);
//}

//If this is a servo motor then the "velocity" signal is not really a velocity signal in this case. 
//It is the desired position and we must convert it to the velocity needed to reach and maintian that position.
void VsHinge::CalculateServoVelocity()
{
	if(!m_vxHinge)
		return;

	float fltError = m_fltDesiredVelocity - m_fltPosition;

	if(m_bEnableLimits)
	{
		if(m_fltDesiredVelocity>m_fltConstraintHigh)
			m_fltDesiredVelocity = m_fltConstraintHigh;
		if(m_fltDesiredVelocity<m_fltConstraintLow)
			m_fltDesiredVelocity = m_fltConstraintLow;

		float fltProp = fltError / (m_fltConstraintHigh-m_fltConstraintLow);

		m_fltDesiredVelocity = fltProp * m_ftlServoGain; 
	}
	else
		m_fltDesiredVelocity = fltError * m_fltMaxVelocity; 
}

void VsHinge::SetVelocityToDesired()
{
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
	}
}

void VsHinge::EnableMotor(BOOL bVal)
{
	VsJoint::EnableMotor(bVal, m_fltSetVelocity, m_fltMaxTorque);
	m_bEnableMotor = bVal;
	m_fltPrevVelocity = -1000000;  //reset the prev velocity for the next usage
}


void VsHinge::ConstraintLow(float fltVal)
{
	Hinge::ConstraintLow(fltVal);

	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	//Reset the position and rotation of the flap.
	if(m_osgMinFlapRotateMT.valid() && m_osgMinFlapTranslateMT.valid())
	{
		float fltHeight = m_fltScale * 0.2f;

		vPos.Set(0, 0, 0); vRot.Set(0, 0, -m_fltConstraintLow); 
		m_osgMinFlapRotateMT->setMatrix(SetupMatrix(vPos, vRot));

		vPos.Set((fltHeight/2)*sin(-m_fltConstraintLow), -(fltHeight/2)*cos(-m_fltConstraintLow), 0); vRot.Set(0, 0, 0); 
		m_osgMinFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
	}

	//Set the lower limit on the physics hinge object.
	if(m_vxHinge)
		m_vxHinge->setLowerLimit(m_vxHinge->kAngularCoordinate, m_fltConstraintLow, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
}

void VsHinge::ConstraintHigh(float fltVal)
{
	Hinge::ConstraintHigh(fltVal);

	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	//Reset the position and rotation of the flap.
	if(m_osgMinFlapRotateMT.valid() && m_osgMinFlapTranslateMT.valid())
	{
		float fltHeight = m_fltScale * 0.2f;

		vPos.Set(0, 0, 0); vRot.Set(0, 0, -m_fltConstraintHigh); 
		m_osgMaxFlapRotateMT->setMatrix(SetupMatrix(vPos, vRot));

		vPos.Set((fltHeight/2)*sin(-m_fltConstraintHigh), -(fltHeight/2)*cos(-m_fltConstraintHigh), 0); vRot.Set(0, 0, 0); 
		m_osgMaxFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
	}

	//Set the lower limit on the physics hinge object.
	if(m_vxHinge)
		m_vxHinge->setUpperLimit(m_vxHinge->kAngularCoordinate, m_fltConstraintHigh, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
}

void VsHinge::ResetGraphicsAndPhysics()
{

	VsBody::BuildLocalMatrix();

	//if(m_osgPosFlapRotateMT.valid() && m_osgPosFlapReferenceMT.valid() && m_osgPosFlapTranslateMT.valid())
	//{
	//	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 
	//	m_osgPosFlapRotateMT->setMatrix(SetupMatrix(vPos, m_oRotation));

	//	CStdFPoint vRefPos = GetOSGWorldCoords(m_osgPosFlapReferenceMT.get());
	//	vPos = vRefPos - m_lpChild->AbsolutePosition(); 
	//	//We want to reverse any rotations performed on the child object.
	//	vRot = m_lpChild->Rotation(); vRot.x = -vRot.x; vRot.y = -vRot.y; vRot.z = -vRot.z;  
	//	m_osgPosFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
	//}

	SetupPhysics(m_lpSim, m_lpStructure);	
}

void VsHinge::Rotation(CStdFPoint &oPoint) 
{
	m_oRotation = oPoint;
	ResetGraphicsAndPhysics();
}

void VsHinge::SetAlpha()
{
	VsJoint::SetAlpha();

	//Now set the alpha for all of the sub parts we created for the joint
	//if(m_osgPosFlapMat.valid() && m_osgPosFlapSS.valid())
	//	SetMaterialAlpha(m_osgPosFlapMat.get(), m_osgPosFlapSS.get(), m_fltAlpha);

	if(m_osgMinFlapMat.valid() && m_osgMinFlapSS.valid())
		SetMaterialAlpha(m_osgMinFlapMat.get(), m_osgMinFlapSS.get(), m_fltAlpha);

	if(m_osgMaxFlapMat.valid() && m_osgMaxFlapSS.valid())
		SetMaterialAlpha(m_osgMaxFlapMat.get(), m_osgMaxFlapSS.get(), m_fltAlpha);

	if(m_osgCylinderMat.valid() && m_osgCylinderSS.valid())
		SetMaterialAlpha(m_osgCylinderMat.get(), m_osgCylinderSS.get(), m_fltAlpha);

}

//void VsHinge::SetVisible(BOOL bVisible)
//{
//	//Now set the visible for all of the sub parts we created for the joint
//	VsJoint::SetVisible(m_osgCylinderMT.get(), bVisible);
//	VsJoint::SetVisible(m_osgCylinderMT.get(), bVisible);
//	VsJoint::SetVisible(m_osgCylinderMT.get(), bVisible);
//}

void VsHinge::SetupGraphics(Simulator *lpSim, Structure *lpStructure)
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG(lpSim, lpStructure);
	osg::ref_ptr<osg::Group> osgChild = ChildOSG(lpSim, lpStructure);

	if(m_osgParent.valid())
	{
		float fltRadius = m_fltScale * 0.05f;
		float fltHeight = m_fltScale * 0.2f;
		float fltFlapWidth = m_fltScale * 0.01f;

		//Create the cylinder for the hinge
		m_osgCylinder = CreateConeGeometry(fltHeight, fltRadius, fltRadius, 30, true, true, true);
		osg::ref_ptr<osg::Geode> osgCylinder = new osg::Geode;
		osgCylinder->addDrawable(m_osgCylinder.get());

		CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
		m_osgCylinderMT = new osg::MatrixTransform();
		m_osgCylinderMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgCylinderMT->addChild(osgCylinder.get());

		//Create the min flap
		m_osgMinFlap = CreateBoxGeometry(fltFlapWidth, (fltHeight/2), fltHeight, fltFlapWidth, (fltHeight/2), fltHeight);
		osg::ref_ptr<osg::Geode> osgMinFlap = new osg::Geode;
		osgMinFlap->addDrawable(m_osgMinFlap.get());

		//Rotate flap first then translate.
		//m_fltConstraintLow = -(2*VX_PI)/4;
		vPos.Set(0, 0, 0); vRot.Set(0, 0, -m_fltConstraintLow); 
		m_osgMinFlapRotateMT = new osg::MatrixTransform();
		m_osgMinFlapRotateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgMinFlapRotateMT->addChild(osgMinFlap.get());

		vPos.Set((fltHeight/2)*sin(-m_fltConstraintLow), -(fltHeight/2)*cos(-m_fltConstraintLow), 0); vRot.Set(0, 0, 0); 
		m_osgMinFlapTranslateMT = new osg::MatrixTransform();
		m_osgMinFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgMinFlapTranslateMT->addChild(m_osgMinFlapRotateMT.get());

		//Create the max flap
		m_osgMaxFlap = CreateBoxGeometry(fltFlapWidth, (fltHeight/2), fltHeight, fltFlapWidth, (fltHeight/2), fltHeight);
		osg::ref_ptr<osg::Geode> osgMaxFlap = new osg::Geode;
		osgMaxFlap->addDrawable(m_osgMaxFlap.get());

		//Rotate flap first then translate.
		//m_fltConstraintHigh = (1*VX_PI)/4;
		vPos.Set(0, 0, 0); vRot.Set(0, 0, -m_fltConstraintHigh); 
		m_osgMaxFlapRotateMT = new osg::MatrixTransform();
		m_osgMaxFlapRotateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgMaxFlapRotateMT->addChild(osgMaxFlap.get());

		vPos.Set((fltHeight/2)*sin(-m_fltConstraintHigh), -(fltHeight/2)*cos(-m_fltConstraintHigh), 0); vRot.Set(0, 0, 0); 
		m_osgMaxFlapTranslateMT = new osg::MatrixTransform();
		m_osgMaxFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgMaxFlapTranslateMT->addChild(m_osgMaxFlapRotateMT.get());

		//Now add a reference point. We will find the coordinates of this point and use that to setup the position
		//of our movable flap. It is empty.
		vPos.Set(0, -fltRadius, 0); vRot.Set(0, 0, 0); 
		m_osgPosFlapReferenceMT = new osg::MatrixTransform();
		m_osgPosFlapReferenceMT->setMatrix(SetupMatrix(vPos, vRot));

		//Add the parts to the group node.
		vPos.Set(0, 0, 0); vRot.Set(0, VX_PI/2, 0); 
		osg::ref_ptr<osg::MatrixTransform> m_osgHingeMT = new osg::MatrixTransform();
		m_osgHingeMT->setMatrix(SetupMatrix(vPos, vRot));

		m_osgHingeMT->addChild(m_osgCylinderMT.get());
		m_osgHingeMT->addChild(m_osgMinFlapTranslateMT.get());
		m_osgHingeMT->addChild(m_osgMaxFlapTranslateMT.get());
		m_osgHingeMT->addChild(m_osgPosFlapReferenceMT.get());

		m_osgNode = m_osgHingeMT.get();

		VsBody::BuildLocalMatrix();
/*
		//Create the max flap
		float fltPosFlapHeight = fltHeight - (fltHeight*0.05); //make this flap just slightly smaller than the others
		m_osgPosFlap = CreateBoxGeometry(fltPosFlapHeight, (fltHeight/2), fltFlapWidth, fltPosFlapHeight, (fltHeight/2), fltFlapWidth);
		osg::ref_ptr<osg::Geode> osgPosFlap = new osg::Geode;
		osgPosFlap->addDrawable(m_osgPosFlap.get());

		//Rotate flap first then translate.
		vPos.Set(0, 0, 0); vRot = m_lpThis->Rotation();
		m_osgPosFlapRotateMT = new osg::MatrixTransform();
		m_osgPosFlapRotateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgPosFlapRotateMT->addChild(osgPosFlap.get());

		CStdFPoint vRefPos = GetOSGWorldCoords(m_osgPosFlapReferenceMT.get());
		vPos = vRefPos - m_lpChild->AbsolutePosition(); //vRot.Set(0, 0, 0); 
		//We want to reverse any rotations performed on the child object.
		vRot = m_lpChild->Rotation(); vRot.x = -vRot.x; vRot.y = -vRot.y; vRot.z = -vRot.z;  
		m_osgPosFlapTranslateMT = new osg::MatrixTransform();
		m_osgPosFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgPosFlapTranslateMT->addChild(m_osgPosFlapRotateMT.get());
		osgChild->addChild(m_osgPosFlapTranslateMT.get());
*/

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

		//create a material to use with the pos flap
		if(!m_osgMinFlapMat.valid())
			m_osgMinFlapMat = new osg::Material();		

		//create a stateset for this node
		m_osgMinFlapSS = m_osgMinFlapTranslateMT->getOrCreateStateSet();

		//set the diffuse property of this node to the color of this body	
		m_osgMinFlapMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
		m_osgMinFlapMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(1, 0, 0, 1));
		m_osgMinFlapMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
		m_osgMinFlapMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
		m_osgMinFlapSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

		//apply the material
		m_osgMinFlapSS->setAttribute(m_osgMinFlapMat.get(), osg::StateAttribute::ON);

		//create a material to use with the pos flap
		if(!m_osgMaxFlapMat.valid())
			m_osgMaxFlapMat = new osg::Material();		

		//create a stateset for this node
		m_osgMaxFlapSS = m_osgMaxFlapTranslateMT->getOrCreateStateSet();

		//set the diffuse property of this node to the color of this body	
		m_osgMaxFlapMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
		m_osgMaxFlapMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(1, 1, 1, 1));
		m_osgMaxFlapMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
		m_osgMaxFlapMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
		m_osgMaxFlapSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

		//apply the material
		m_osgMaxFlapSS->setAttribute(m_osgMaxFlapMat.get(), osg::StateAttribute::ON);
/*
		//create a material to use with the pos flap
		if(!m_osgPosFlapMat.valid())
			m_osgPosFlapMat = new osg::Material();		

		//create a stateset for this node
		m_osgPosFlapSS = m_osgPosFlapTranslateMT->getOrCreateStateSet();

		//set the diffuse property of this node to the color of this body	
		m_osgPosFlapMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
		m_osgPosFlapMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(0, 0, 1, 1));
		m_osgPosFlapMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
		m_osgPosFlapMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
		m_osgPosFlapSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

		//apply the material
		m_osgPosFlapSS->setAttribute(m_osgPosFlapMat.get(), osg::StateAttribute::ON);
*/
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

	m_vxHinge->setLowerLimit(m_vxHinge->kAngularCoordinate,m_fltConstraintLow, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	m_vxHinge->setUpperLimit(m_vxHinge->kAngularCoordinate, m_fltConstraintHigh, 0, m_fltRestitution, m_fltStiffness, m_fltDamping);
	m_vxHinge->setLimitsActive(m_vxHinge->kAngularCoordinate, m_bEnableLimits);	

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
