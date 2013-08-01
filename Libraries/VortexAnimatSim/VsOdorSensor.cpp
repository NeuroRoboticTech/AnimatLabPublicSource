// VsOdorSensor.cpp: implementation of the VsOdorSensor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsOdorSensor.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsOdorSensor::VsOdorSensor()
{
	SetThisPointers();
}

VsOdorSensor::~VsOdorSensor()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsOdorSensor/\r\n", "", -1, false, true);}
}

void VsOdorSensor::CreateGraphicsGeometry() 
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void VsOdorSensor::CreatePhysicsGeometry() {}

void VsOdorSensor::ResizePhysicsGeometry() {}

void VsOdorSensor::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

