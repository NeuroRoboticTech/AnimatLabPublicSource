#pragma once

#include <vcclr.h>

#using <mscorlib.dll>
using namespace System;

namespace AnimatGUI
{
	namespace Interfaces
	{

class BodyPartCallback : public AnimatSim::IBodyPartCallback
{
protected:
	gcroot<DataObjectInterface ^>  m_doObj;

public:
	BodyPartCallback(DataObjectInterface ^doObj);
	virtual ~BodyPartCallback(void);

	virtual void PositionChanged();
	virtual void RotationChanged();
	virtual void SelectionChanged(BOOL bSelected, BOOL bSelectMultiple);
	virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
};

	}
}
