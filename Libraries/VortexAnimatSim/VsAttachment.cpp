// VsAttachment.cpp: implementation of the VsAttachment class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
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
	SetThisPointers();
}

VsAttachment::~VsAttachment()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsAttachment\r\n", "", -1, FALSE, TRUE);}
}

void VsAttachment::CreateGraphicsGeometry() 
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void VsAttachment::CreatePhysicsGeometry() {}

void VsAttachment::ResizePhysicsGeometry() {}

void VsAttachment::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

