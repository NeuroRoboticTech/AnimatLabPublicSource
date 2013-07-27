/**
\file	OsgPrismatic.cpp

\brief	Implements the vs prismatic class.
**/

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgPrismaticLimit.h"
#include "OsgPrismatic.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
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
OsgPrismatic::OsgPrismatic()
{
	SetThisPointers();
	m_vxPrismatic = NULL;

	m_lpUpperLimit = new OsgPrismaticLimit();
	m_lpLowerLimit = new OsgPrismaticLimit();
	m_lpPosFlap = new OsgPrismaticLimit();

	m_lpUpperLimit->LimitPos(1, FALSE);
	m_lpLowerLimit->LimitPos(-1, FALSE);
	m_lpPosFlap->LimitPos(Prismatic::JointPosition(), FALSE);
	m_lpPosFlap->IsShowPosition(TRUE);

	m_lpUpperLimit->Color(0, 0, 1, 1);
	m_lpLowerLimit->Color(1, 1, 0.333, 1);
	m_lpPosFlap->Color(1, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(TRUE);
	m_lpUpperLimit->IsLowerLimit(FALSE);
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
OsgPrismatic::~OsgPrismatic()
{
	//ConstraintLimits are deleted in the base objects.
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of OsgPrismatic/\r\n", "", -1, FALSE, TRUE);}
}


void OsgPrismatic::EnableLimits(BOOL bVal)
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

void OsgPrismatic::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}


void OsgPrismatic::SetAlpha()
{
	OsgJoint::SetAlpha();

	m_lpUpperLimit->Alpha(m_fltAlpha);
	m_lpLowerLimit->Alpha(m_fltAlpha);
	m_lpPosFlap->Alpha(m_fltAlpha);
}

void OsgPrismatic::DeleteJointGraphics()
{
	OsgPrismaticLimit *lpUpperLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpUpperLimit);
	OsgPrismaticLimit *lpLowerLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpLowerLimit);
	OsgPrismaticLimit *lpPosFlap = dynamic_cast<OsgPrismaticLimit *>(m_lpPosFlap);

    if(m_osgJointMT.valid())
    {
		if(lpUpperLimit && lpUpperLimit->BoxMT()) m_osgJointMT->removeChild(lpUpperLimit->BoxMT());
		if(lpUpperLimit && lpUpperLimit->CylinderMT()) m_osgJointMT->removeChild(lpUpperLimit->CylinderMT());

        if(lpLowerLimit && lpLowerLimit->BoxMT()) m_osgJointMT->removeChild(lpLowerLimit->BoxMT());
		if(lpLowerLimit && lpLowerLimit->CylinderMT()) m_osgJointMT->removeChild(lpLowerLimit->CylinderMT());

		if(lpPosFlap && lpPosFlap->BoxMT()) m_osgJointMT->removeChild(lpPosFlap->BoxMT());
    }

    if(m_lpUpperLimit) m_lpUpperLimit->DeleteGraphics();
    if(m_lpLowerLimit) m_lpLowerLimit->DeleteGraphics();
    if(m_lpPosFlap) m_lpPosFlap->DeleteGraphics();
}

void OsgPrismatic::CreateJointGraphics()
{
	OsgPrismaticLimit *lpUpperLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpUpperLimit);
	OsgPrismaticLimit *lpLowerLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpLowerLimit);
	OsgPrismaticLimit *lpPosFlap = dynamic_cast<OsgPrismaticLimit *>(m_lpPosFlap);

	lpPosFlap->LimitPos(Prismatic::JointPosition());

	lpUpperLimit->SetupGraphics();
	lpLowerLimit->SetupGraphics();
	lpPosFlap->SetupGraphics();

	m_osgJointMT->addChild(lpUpperLimit->BoxMT());
	m_osgJointMT->addChild(lpUpperLimit->CylinderMT());

	m_osgJointMT->addChild(lpLowerLimit->BoxMT());
	m_osgJointMT->addChild(lpLowerLimit->CylinderMT());

	m_osgJointMT->addChild(lpPosFlap->BoxMT());
}

void OsgPrismatic::SetupGraphics()
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
		osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void OsgPrismatic::DeletePhysics()
{
	if(!m_vxPrismatic)
		return;

	if(GetVsSimulator() && GetVsSimulator()->Universe())
	{
		GetVsSimulator()->Universe()->removeConstraint(m_vxPrismatic);
		delete m_vxPrismatic;

		if(m_lpChild && m_lpParent)
			m_lpChild->EnableCollision(m_lpParent);
	}

	m_vxPrismatic = NULL;
	m_vxJoint = NULL;
}

void OsgPrismatic::SetupPhysics()
{
	if(m_vxPrismatic)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Osg_Err_lUnableToConvertToVsRigidBody, Osg_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Osg_Err_lUnableToConvertToVsRigidBody, Osg_Err_strUnableToConvertToVsRigidBody);

	VxAssembly *lpAssem = (VxAssembly *) m_lpStructure->Assembly();

	CStdFPoint vGlobal = this->GetOSGWorldCoords();
	
	Vx::VxReal44 vMT;
	VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(TRUE), vMT);
	Vx::VxTransform vTrans(vMT);
	Vx::VxReal3 vxRot;
	vTrans.getRotationEulerAngles(vxRot);

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]);

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	VxVector3 axis = NormalizeAxis(vLocalRot);

	m_vxPrismatic = new VxPrismatic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 
	m_vxPrismatic->setName(m_strID.c_str());

	//lpAssem->addConstraint(m_vxPrismatic);
	GetVsSimulator()->Universe()->addConstraint(m_vxPrismatic);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	OsgPrismaticLimit *lpUpperLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpUpperLimit);
	OsgPrismaticLimit *lpLowerLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpLowerLimit);

	lpUpperLimit->PrismaticRef(m_vxPrismatic);
	lpLowerLimit->PrismaticRef(m_vxPrismatic);

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	m_vxJoint = m_vxPrismatic;
	m_iCoordID = m_vxPrismatic->kLinearCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	EnableMotor(m_bEnableMotorInit);

    Prismatic::Initialize();
    OsgJoint::Initialize();
}

void OsgPrismatic::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

float *OsgPrismatic::GetDataPointer(const string &strDataType)
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
		return &m_fltReportSetVelocity;
	else if(strType == "JOINTSETVELOCITY")
		return &m_fltReportSetVelocity;
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

BOOL OsgPrismatic::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	if(OsgJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Prismatic::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void OsgPrismatic::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	OsgJoint::Physics_QueryProperties(aryNames, aryTypes);
	Prismatic::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

void OsgPrismatic::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

		}		//Joints
	}			// Environment
}				//OsgAnimatSim
