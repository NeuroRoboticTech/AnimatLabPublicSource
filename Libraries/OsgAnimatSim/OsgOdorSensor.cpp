// OsgOdorSensor.cpp: implementation of the OsgOdorSensor class.
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

#include "OsgOdorSensor.h"

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

OsgOdorSensor::OsgOdorSensor()
{
    m_bPhsyicsDefined = false;
	SetThisPointers();
}

OsgOdorSensor::~OsgOdorSensor()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of OsgOdorSensor/\r\n", "", -1, false, true);}
}

void OsgOdorSensor::CreateGraphicsGeometry() 
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void OsgOdorSensor::CreatePhysicsGeometry() 
{
    m_bPhsyicsDefined = true;
}

void OsgOdorSensor::ResizePhysicsGeometry() {}

void OsgOdorSensor::CreateParts()
{
	CreateGeometry();

	OsgRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

