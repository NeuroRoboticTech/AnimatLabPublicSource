// VsSphere.cpp: implementation of the VsSphere class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
#include "VsStructure.h"
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

VsSphere::VsSphere()
{
	SetThisPointers();
	m_cgSphere = NULL;
}

VsSphere::~VsSphere()
{

}

void VsSphere::CreateParts()
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = new VxSphere(m_fltRadius);

	//Lets get the volume and areas
	m_fltVolume = 1.33333*VX_PI*m_fltRadius*m_fltRadius*m_fltRadius;
	m_fltXArea = 2*VX_PI*m_fltRadius*m_fltRadius;
	m_fltYArea = m_fltXArea;
	m_fltZArea = m_fltXArea;

	VsRigidBody::CreateItem();
	Sphere::CreateParts();
	VsRigidBody::SetBody();
}

void VsSphere::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Sphere::CreateJoints();
	VsRigidBody::Initialize();
}

void VsSphere::Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
		if(!osgGroup)
			THROW_TEXT_ERROR(Vs_Err_lNodeNotGeode, Vs_Err_strNodeNotGeode, m_lpThisAB->Name());

		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
			osgGroup->removeDrawable(m_osgGeometry.get());

		m_osgGeometry.release();

		//Create a new box geometry with the new sizes.
		m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
		m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

		//Add it to the geode.
		osgGroup->addDrawable(m_osgGeometry.get());

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgNodeGroup);
	}

	if(m_vxGeometry)
	{
		VxSphere *vxSphere = dynamic_cast<VxSphere *>(m_vxGeometry);

		if(!vxSphere)
			THROW_TEXT_ERROR(Vs_Err_lGeometryMismatch, Vs_Err_strGeometryMismatch, m_lpThisAB->Name());
		
		vxSphere->setRadius(m_fltRadius);
		GetBaseValues();
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
