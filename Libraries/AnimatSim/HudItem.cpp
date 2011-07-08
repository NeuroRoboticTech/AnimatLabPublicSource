/**
\file	HudItem.cpp

\brief	Implements the heads-up display item class.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"
#include "HudItem.h"

namespace AnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	7/7/2011
**/
HudItem::HudItem()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	7/7/2011
**/
HudItem::~HudItem()
{
}

void HudItem::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);
}

}				//AnimatSim
