/**
\file	OsgMouth.cpp

\brief	Implements the vortex mouth class.
**/

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

#include "OsgMouth.h"

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor.

\author	dcofer
\date	6/12/2011
**/
OsgMouth::OsgMouth()
{
    m_bPhsyicsDefined = false;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	6/12/2011
**/
OsgMouth::~OsgMouth()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of OsgMouth/\r\n", "", -1, false, true);}
}

void OsgMouth::CreateGraphicsGeometry() 
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void OsgMouth::CreatePhysicsGeometry() 
{
    m_bPhsyicsDefined = true;
}

void OsgMouth::ResizePhysicsGeometry() {}

void OsgMouth::CreateParts()
{
	CreateGeometry();

	OsgRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

