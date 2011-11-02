#pragma once

#include <vcclr.h>

#using <mscorlib.dll>
using namespace System;

namespace AnimatGUI
{
	namespace Interfaces
	{

class MovableItemCallback : public AnimatSim::IMovableItemCallback
{
protected:
	gcroot<ManagedAnimatInterfaces::IDataObjectInterface ^>  m_doObj;

public:
	MovableItemCallback(ManagedAnimatInterfaces::IDataObjectInterface ^doObj);
	virtual ~MovableItemCallback(void);

	virtual void PositionChanged();
	virtual void RotationChanged();
	virtual void SelectionChanged(BOOL bSelected, BOOL bSelectMultiple);
	virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
	virtual void SelectedVertexChanged(float fltPosX, float fltPosY, float fltPosZ);
};

	}
}
