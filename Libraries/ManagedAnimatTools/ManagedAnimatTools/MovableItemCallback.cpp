#include "stdafx.h"
#include "Util.h"
#include "Logger.h"
#include "PropertyUpdateException.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
#include "DataObjectInterface.h"
#include "MovableItemCallback.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

MovableItemCallback::MovableItemCallback(DataObjectInterface ^doObj)
{
	m_doObj = doObj;
}

MovableItemCallback::~MovableItemCallback(void)
{
}

void MovableItemCallback::PositionChanged()
{
	m_doObj->FirePositionChangedEvent();
}

void MovableItemCallback::RotationChanged()
{
	m_doObj->FireRotationChangedEvent();
}

void MovableItemCallback::SelectionChanged(BOOL bSelected, BOOL bSelectMultiple)
{
	m_doObj->FireSelectionChangedEvent(bSelected, bSelectMultiple);
}

void MovableItemCallback::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)
{
	m_doObj->FireAddBodyClickedEvent(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ);
}

void MovableItemCallback::SelectedVertexChanged(float fltPosX, float fltPosY, float fltPosZ)
{
	m_doObj->FireSelectedVertexChangedEvent(fltPosX, fltPosY, fltPosZ);

}

	}
}
