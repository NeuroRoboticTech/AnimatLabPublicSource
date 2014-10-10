// OsgAttachment.cpp: implementation of the OsgAttachment class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgRigidBody.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"

#include "OsgAttachment.h"


namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

OsgAttachment::OsgAttachment()
{
    m_bPhsyicsDefined = false;
	SetThisPointers();
}

OsgAttachment::~OsgAttachment()
{

	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of OsgAttachment\r\n", "", -1, false, true);}
}

void OsgAttachment::CreateGraphicsGeometry() 
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void OsgAttachment::CreatePhysicsGeometry() 
{    
    m_bPhsyicsDefined = true;
}

void OsgAttachment::ResizePhysicsGeometry() {}

void OsgAttachment::CreateParts()
{
	CreateGeometry();

	OsgRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

