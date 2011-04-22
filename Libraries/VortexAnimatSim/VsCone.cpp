// VsCone.cpp: implementation of the VsCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsRigidBody.h"
#include "VsCone.h"
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

VsCone::VsCone()
{
	m_lpThis = this;
	m_lpThisBody = this;
	PhysicsBody(this);
}

VsCone::~VsCone()
{

}

void VsCone::CreateParts()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, m_iSides, true, true, true);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());

	m_osgNode = osgGroup;

	if(IsCollisionObject())
		m_vxGeometry = VxConvexMesh::createFromNode(m_osgNode.get());

	//Lets get the volume and areas
	m_fltVolume = 2*VX_PI*m_fltLowerRadius*m_fltLowerRadius*m_fltHeight;
	m_fltXArea = 2*m_fltLowerRadius*m_fltHeight;
	m_fltYArea = 2*m_fltLowerRadius*m_fltHeight;
	m_fltZArea = 2*VX_PI*m_fltLowerRadius*m_fltLowerRadius;

	VsRigidBody::CreateBody();
	Cone::CreateParts();
	VsRigidBody::SetBody();
}

void VsCone::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cone::CreateJoints();
	VsRigidBody::Initialize();
}

void VsCone::Resize()
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
		m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, m_iSides, true, true, true);
		m_osgGeometry->setName(m_lpThis->Name() + "_Geometry");

		//Add it to the geode.
		osgGroup->addDrawable(m_osgGeometry.get());

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgNodeGroup);
	}

	if(m_vxGeometry && m_vxCollisionGeometry && m_vxSensor)
	{
		if(!m_vxSensor->removeCollisionGeometry(m_vxCollisionGeometry))
			THROW_PARAM_ERROR(Vs_Err_lRemovingCollisionGeometry, Vs_Err_strRemovingCollisionGeometry, "ID: ", m_strID);

		delete m_vxCollisionGeometry;
		m_vxCollisionGeometry = NULL;

		int iMaterialID = m_lpSim->GetMaterialID(MaterialID());
		m_vxGeometry = VxConvexMesh::createFromNode(m_osgNode.get());
		Vx::VxCollisionGeometry *vxCollisionGeometry = m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisBody->Density());

		GetBaseValues();
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
