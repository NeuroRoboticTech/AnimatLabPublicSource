#include "stdafx.h"
//#include "ILogger.h"
#include "Util.h"
#include "PropertyUpdateException.h"
//#include "ISimulatorInterface.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
//#include "IDataObjectInterface.h"
#include "DataObjectInterface.h"
#include "MovableItemCallback.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

MovableItemCallback::MovableItemCallback(ManagedAnimatInterfaces::IDataObjectInterface ^doObj)
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

void MovableItemCallback::SizeChanged()
{
	m_doObj->FireSizeChangedEvent();
}

void MovableItemCallback::SelectionChanged(bool bSelected, bool bSelectMultiple)
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
