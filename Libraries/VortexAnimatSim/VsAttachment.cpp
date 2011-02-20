// VsAttachment.cpp: implementation of the VsAttachment class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsAttachment.h"
#include "VsSimulator.h"
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

VsAttachment::VsAttachment()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
}

VsAttachment::~VsAttachment()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsAttachment\r\n", "", -1, FALSE, TRUE);}
}

void VsAttachment::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	osg::ref_ptr<osg::ShapeDrawable> osgDrawable = new osg::ShapeDrawable(new osg::Sphere(osg::Vec3(0,0,0), m_fltRadius));
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(osgDrawable.get());
	m_osgNode = osgGroup;
	m_vxGeometry = NULL;  //No physics part.

	VsRigidBody::CreateBody(lpSim, lpStructure);
}

/*
void VsAttachment::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	Attachment::StepSimulation(lpSim, lpStructure);
	VsRigidBody::CollectBodyData(lpSim);
}

float *VsAttachment::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	lpData = Attachment::GetDataPointer(strDataType);
	if(lpData) return lpData;

	lpData = VsRigidBody::GetPhysicsDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

void VsAttachment::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	VsRigidBody::ResetSimulation(lpSim, lpStructure);
	Attachment::ResetSimulation(lpSim, lpStructure);
}
*/

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

