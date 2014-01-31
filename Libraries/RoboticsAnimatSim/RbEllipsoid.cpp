/**
\file	RbEllipsoid.cpp

\brief	Implements the vortex ellipsoid class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbEllipsoid.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
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
RbEllipsoid::RbEllipsoid()
{
	SetThisPointers();
}

RbEllipsoid::~RbEllipsoid()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbEllipsoid/\r\n", "", -1, false, true);}
}

void RbEllipsoid::CreateParts()
{
	RbRigidBody::CreateItem();
	Ellipsoid::CreateParts();
}

void RbEllipsoid::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Ellipsoid::CreateJoints();
	RbRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
