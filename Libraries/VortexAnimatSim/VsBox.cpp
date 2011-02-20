// VsBox.cpp: implementation of the VsBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsBox.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsBox::VsBox()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
}

VsBox::~VsBox()
{
}

void VsBox::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	m_osgGeometry = CreateBoxGeometry(m_fltLength, m_fltHeight, m_fltWidth, m_fltLength, m_fltHeight, m_fltWidth);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;

	if(IsCollisionObject())
		m_vxGeometry = new VxBox(m_fltLength, m_fltHeight, m_fltWidth);

	//Lets get the volume and areas
	m_fltVolume = m_fltLength * m_fltHeight * m_fltWidth;
	m_fltXArea = m_fltHeight * m_fltWidth;
	m_fltYArea = m_fltLength * m_fltWidth;
	m_fltZArea = m_fltHeight * m_fltLength;

	VsRigidBody::CreateBody(lpSim, lpStructure);
	Box::CreateParts(lpSim, lpStructure);
	VsRigidBody::SetBody(lpSim, lpStructure);
}

void VsBox::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint(lpSim, lpStructure);

	Box::CreateJoints(lpSim, lpStructure);
	VsRigidBody::Initialize(lpSim, lpStructure);
}

void VsBox::Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
		if(!osgGroup)
			THROW_TEXT_ERROR(Vs_Err_lNodeNotGeode, Vs_Err_strNodeNotGeode, m_lpThis->Name());

		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
			osgGroup->removeDrawable(m_osgGeometry.get());

		m_osgGeometry.release();

		//Create a new box geometry with the new sizes.
		m_osgGeometry = CreateBoxGeometry(m_fltLength, m_fltHeight, m_fltWidth, m_fltLength, m_fltHeight, m_fltWidth);
		
		//Add it to the geode.
		osgGroup->addDrawable(m_osgGeometry.get());

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();
	}

	if(m_vxGeometry)
	{
		VxBox *vxBox = dynamic_cast<VxBox *>(m_vxGeometry);

		if(!vxBox)
			THROW_TEXT_ERROR(Vs_Err_lGeometryMismatch, Vs_Err_strGeometryMismatch, m_lpThis->Name());
		
		vxBox->setDimensions(m_fltLength, m_fltHeight, m_fltWidth);
		GetBaseValues();

		//Reset the user data for the new parts.
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThis->Parent());
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(lpVsParent);
		osgVisitor->traverse(*m_osgMT);

		//Temp code. Lets save it out and make sure the collision stuff is actually correct.
	    VxPersistence::saveFrame("C:\\AnimatLabSDK\\VS9\\Experiments\\Test.vxf", VxPersistence::kAutoGenerateGraphics);
	}
}


/*
void VsBox::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	VsRigidBody::ResetSimulation(lpSim, lpStructure);
	Box::ResetSimulation(lpSim, lpStructure);
}

void VsBox::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	Box::StepSimulation(lpSim, lpStructure);
	VsRigidBody::CollectBodyData(lpSim);
}

float *VsBox::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	lpData = Box::GetDataPointer(strDataType);
	if(lpData) return lpData;

	lpData = VsRigidBody::GetPhysicsDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

void VsBox::Selected(BOOL bValue, BOOL bSelectMultiple)  
{
	Box::Selected(bValue, bSelectMultiple);
	VsRigidBody::Selected(bValue, bSelectMultiple);
}

void VsBox::EnableCollision(Simulator *lpSim, RigidBody *lpBody)
{VsRigidBody::EnableCollision(lpSim, lpBody);}

void VsBox::DisableCollision(Simulator *lpSim, RigidBody *lpBody)
{VsRigidBody::DisableCollision(lpSim, lpBody);}

void VsBox::AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
{VsRigidBody::AddBodyForce(lpSim, fltPx, fltPy, fltPz, fltFx, fltFy, fltFz, bScaleUnits);}

void VsBox::AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz, BOOL bScaleUnits)
{VsRigidBody::AddBodyTorque(lpSim, fltTx, fltTy, fltTz, bScaleUnits);}

CStdFPoint VsBox::GetVelocityAtPoint(float x, float y, float z)
{return VsRigidBody::GetVelocityAtPoint(x, y, z);}

float VsBox::GetMass()
{return VsRigidBody::GetMass();}
*/

/*
void VsBox::operator=(VsBox &oOrig)
{
	VsRigidBody::Copy(&oOrig);
	m_oCollisionBoxSize = oOrig.m_oCollisionBoxSize;
	m_oGraphicsBoxSize = oOrig.m_oGraphicsBoxSize;
}

CStdSerialize *VsBox::Clone()
{
	VsBox *lpPart = NULL;

try
{
	lpPart = new VsBox(m_lpParent);
	lpPart->operator=(*this);

	return lpPart;
}
catch(CStdErrorInfo oError)
{
	if(lpPart) delete lpPart;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpPart) delete lpPart;
	THROW_ERROR(Err_lUnspecifiedError, Err_strUnspecifiedError);
	return NULL;
}
}


void VsBox::Trace(ostream &oOs)
{
//	oOs << "{ Name: " << m_strName  << ", Size: " << m_oSize ;
//	oOs <<", RelPos: " << m_oRelPosition << ", AbsPos: " << m_oAbsPosition;
//	oOs << ", " << m_fltDensity << "}";
}
*/

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
