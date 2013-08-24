/**
\file	BlHinge.cpp

\brief	Implements the vortex hinge class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlJoint.h"
#include "BlHingeLimit.h"
#include "BlHinge.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
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
BlHinge::BlHinge()
{
	SetThisPointers();
    //FIX PHYSICS
	//m_vxHinge = NULL;
	m_fltRotationDeg = 0;

	m_lpUpperLimit = new BlHingeLimit();
	m_lpLowerLimit = new BlHingeLimit();
	m_lpPosFlap = new BlHingeLimit();

	m_lpUpperLimit->LimitPos(0.25*osg::PI, false);
	m_lpLowerLimit->LimitPos(-0.25*osg::PI, false);
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
BlHinge::~BlHinge()
{
	//ConstraintLimits are deleted in the base objects.
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlHinge\r\n", "", -1, false, true);}
}

void BlHinge::EnableLimits(bool bVal)
{
	Hinge::EnableLimits(bVal);

    //FIX PHYSICS
	//if(m_vxHinge)
	//	m_vxHinge->setLimitsActive(m_vxHinge->kAngularCoordinate, m_bEnableLimits);	

	if(m_bEnableLimits)
	{
		if(m_lpLowerLimit) m_lpLowerLimit->SetLimitPos();
		if(m_lpUpperLimit) m_lpUpperLimit->SetLimitPos();
	}
}

void BlHinge::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}

void BlHinge::SetAlpha()
{
	BlJoint::SetAlpha();

	m_lpUpperLimit->Alpha(m_fltAlpha);
	m_lpLowerLimit->Alpha(m_fltAlpha);
	m_lpPosFlap->Alpha(m_fltAlpha);

	if(m_osgCylinderMat.valid() && m_osgCylinderSS.valid())
		SetMaterialAlpha(m_osgCylinderMat.get(), m_osgCylinderSS.get(), m_fltAlpha);

}

void BlHinge::DeleteJointGraphics()
{
	OsgHingeLimit *lpUpperLimit = dynamic_cast<OsgHingeLimit *>(m_lpUpperLimit);
	OsgHingeLimit *lpLowerLimit = dynamic_cast<OsgHingeLimit *>(m_lpLowerLimit);
	OsgHingeLimit *lpPosFlap = dynamic_cast<OsgHingeLimit *>(m_lpPosFlap);

    OsgHinge::DeleteHingeGraphics(m_osgJointMT, lpUpperLimit, lpLowerLimit, lpPosFlap);

    if(m_lpUpperLimit) m_lpUpperLimit->DeleteGraphics();
    if(m_lpLowerLimit) m_lpLowerLimit->DeleteGraphics();
    if(m_lpPosFlap) m_lpPosFlap->DeleteGraphics();
}

void BlHinge::CreateJointGraphics()
{
	OsgHingeLimit *lpUpperLimit = dynamic_cast<OsgHingeLimit *>(m_lpUpperLimit);
	OsgHingeLimit *lpLowerLimit = dynamic_cast<OsgHingeLimit *>(m_lpLowerLimit);
	OsgHingeLimit *lpPosFlap = dynamic_cast<OsgHingeLimit *>(m_lpPosFlap);

    float fltLimitPos = Hinge::JointPosition();
	m_lpPosFlap->LimitPos(fltLimitPos);

    OsgHinge::CreateHingeGraphics(CylinderHeight(), CylinderRadius(), FlapWidth(), 
                                  m_osgJointMT, lpUpperLimit, lpLowerLimit, lpPosFlap);
}

void BlHinge::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG();

	if(m_osgParent.valid())
	{
		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot(osg::PI/2, 0, 0); 
		vPos.Set(0, 0, 0); vRot.Set(0, osg::PI/2, 0); 
		
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
		osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void BlHinge::DeletePhysics()
{
    //FIX PHYSICS
	//if(!m_vxHinge)
	//	return;

	//if(GetBlSimulator() && GetBlSimulator()->Universe())
	//{
	//	GetBlSimulator()->Universe()->removeConstraint(m_vxHinge);
	//	delete m_vxHinge;
	//	
	//	if(m_lpChild && m_lpParent)
	//		m_lpChild->EnableCollision(m_lpParent);
	//}

	//m_vxHinge = NULL;
	//m_vxJoint = NULL;
}

void BlHinge::SetupPhysics()
{
    //FIX PHYSICS
	//if(m_vxHinge)
	//	DeletePhysics();

	//if(!m_lpParent)
	//	THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	//if(!m_lpChild)
	//	THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	//BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpParent);
	//if(!lpVsParent)
	//	THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	//BlRigidBody *lpVsChild = dynamic_cast<BlRigidBody *>(m_lpChild);
	//if(!lpVsChild)
	//	THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	//CStdFPoint vGlobal = this->GetOSGWorldCoords();
	//
	//Vx::VxReal44 vMT;
	//VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(), vMT);
	//Vx::VxTransform vTrans(vMT);
	//Vx::VxReal3 vxRot;
	//vTrans.getRotationEulerAngles(vxRot);

	//CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]); //= m_lpThisMI->Rotation();

 //   VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	//osg::Vec3d vNormAxis = NormalizeAxis(vLocalRot);
	//VxVector3 axis((double) vNormAxis[0], (double) vNormAxis[1], (double) vNormAxis[2]);

	//m_vxHinge = new VxHinge(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 
	//m_vxHinge->setName(m_strID.c_str());

	//GetBlSimulator()->Universe()->addConstraint(m_vxHinge);

	////Disable collisions between this object and its parent
	//m_lpChild->DisableCollision(m_lpParent);

	//BlHingeLimit *lpUpperLimit = dynamic_cast<BlHingeLimit *>(m_lpUpperLimit);
	//BlHingeLimit *lpLowerLimit = dynamic_cast<BlHingeLimit *>(m_lpLowerLimit);

	//lpUpperLimit->HingeRef(m_vxHinge);
	//lpLowerLimit->HingeRef(m_vxHinge);

	////Re-enable the limits once we have initialized the joint
	//EnableLimits(m_bEnableLimits);

	//m_vxJoint = m_vxHinge;
	//m_iCoordID = m_vxHinge->kAngularCoordinate;

	////If the motor is enabled then it will start out with a velocity of	zero.
	//EnableMotor(m_bEnableMotorInit);

 //   Hinge::Initialize();
 //   BlJoint::Initialize();
}

void BlHinge::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

float *BlHinge::GetDataPointer(const string &strDataType)
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
		return &m_fltReportSetVelocity;
	else if(strType == "JOINTSETVELOCITY")
		return &m_fltReportSetVelocity;
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

bool BlHinge::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Hinge::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlHinge::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	Hinge::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

void BlHinge::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

void BlHinge::UpdateData()
{
	Hinge::UpdateData();
	m_fltRotationDeg = ((m_fltPosition/osg::PI)*180);
}

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
