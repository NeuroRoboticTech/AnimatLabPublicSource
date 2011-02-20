// VsHudItem.cpp: implementation of the VsHudItem class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsHudItem::VsHudItem()
{
}

VsHudItem::~VsHudItem()
{
}

void VsHudItem::Load(Simulator *lpSim, CStdXml &oXml)
{
	AnimatBase::Load(oXml);
}

	}			// Visualization
}				//VortexAnimatSim
