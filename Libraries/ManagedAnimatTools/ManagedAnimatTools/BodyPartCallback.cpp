#include "stdafx.h"
#include "Util.h"
#include "Logger.h"
#include "PropertyUpdateException.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
#include "DataObjectInterface.h"
#include "BodyPartCallback.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

BodyPartCallback::BodyPartCallback(DataObjectInterface ^doObj)
{
	m_doObj = doObj;
}

BodyPartCallback::~BodyPartCallback(void)
{
}

void BodyPartCallback::PositionChanged()
{
	m_doObj->FirePositionChangedEvent();
}

void BodyPartCallback::RotationChanged()
{
	m_doObj->FireRotationChangedEvent();
}

void BodyPartCallback::SelectionChanged(BOOL bSelected, BOOL bSelectMultiple)
{
	m_doObj->FireSelectionChangedEvent(bSelected, bSelectMultiple);
}

void BodyPartCallback::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)
{
	m_doObj->FireAddBodyClickedEvent(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ);
}

	}
}
