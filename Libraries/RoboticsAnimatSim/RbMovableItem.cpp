#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbMovableItem::RbMovableItem()
{
    m_lpRbSim = NULL;
	m_lpThisAB = NULL;
	m_lpThisMI = NULL;
	m_lpThisRbMI = NULL;
	m_lpParentRbMI = NULL;
    m_fltReportNull = 0;
}


RbMovableItem::~RbMovableItem()
{
}

void RbMovableItem::SetThisPointers()
{
	m_lpThisAB = dynamic_cast<AnimatBase *>(this);
	if(!m_lpThisAB)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisAB");

	m_lpThisMI = dynamic_cast<MovableItem *>(this);
	if(!m_lpThisMI)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisMI, " + m_lpThisAB->Name());

	m_lpThisRbMI = dynamic_cast<RbMovableItem *>(this);
	if(!m_lpThisRbMI)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisRbMI, " + m_lpThisAB->Name());

	m_lpThisMI->PhysicsMovableItem(this);
}

RbSimulator *RbMovableItem::GetRbSimulator()
{
    if(!m_lpRbSim)
    {
        m_lpRbSim = dynamic_cast<RbSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisRbMI)
		    THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpRbSim, " + m_lpThisAB->Name());
    }
    
    return m_lpRbSim;
}

std::string RbMovableItem::Physics_ID()
{
	if(m_lpThisAB)
		return m_lpThisAB->ID();
	else
		return "";
}

#pragma region Selection-Code

void RbMovableItem::Physics_Selected(bool bValue, bool bSelectMultiple)  
{
}

#pragma endregion

float *RbMovableItem::Physics_GetDataPointer(const std::string &strDataType) {return NULL;}

RbMovableItem *RbMovableItem::RbParent()
{
	return m_lpParentRbMI;
}

bool RbMovableItem::Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos)
{
	return false;
}

void RbMovableItem::Physics_UpdateMatrix()
{
}

void RbMovableItem::Physics_UpdateAbsolutePosition()
{
}

void RbMovableItem::Physics_LoadLocalTransformMatrix(CStdXml &oXml) 
{
}

void RbMovableItem::Physics_SaveLocalTransformMatrix(CStdXml &oXml) 
{
}

std::string RbMovableItem::Physics_GetLocalTransformMatrixString() 
{
    return "";
}

void RbMovableItem::Physics_ResizeDragHandler(float fltRadius)
{
}

void RbMovableItem::Physics_ResetGraphicsAndPhysics()
{
}

void RbMovableItem::Physics_PositionChanged()
{
}

void RbMovableItem::Physics_RotationChanged()
{
}

BoundingBox RbMovableItem::Physics_GetBoundingBox()
{
 	BoundingBox abb;
	return abb;
}

float RbMovableItem::Physics_GetBoundingRadius()
{
	BoundingBox bb = Physics_GetBoundingBox();
	return bb.MaxDimension();
}

void RbMovableItem::Physics_CollectData()
{
}

void RbMovableItem::Physics_ResetSimulation()
{
}

void RbMovableItem::Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm)
{
}

void RbMovableItem::CreateItem()
{
	m_lpThisAB->Initialize();
}

	}			// Environment
//}				//RoboticsAnimatSim

}