// VsMuscle.cpp: implementation of the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsOrganism.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsDragger.h"


namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsOrganism::VsOrganism()
{
	//SetThisPointers();
}

VsOrganism::~VsOrganism()
{

}

void VsOrganism::ResetSimulation()
{
	//VsMovableItem::Physics_ResetSimulation();

	Organism::ResetSimulation();
}

	}			// Environment
}				//VortexAnimatSim
