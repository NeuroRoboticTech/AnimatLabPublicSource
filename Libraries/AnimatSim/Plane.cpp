// Plane.cpp: implementation of the Plane class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Plane.h"
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a plane object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a plane. 
*/

Plane::Plane()
{
	m_fltDensity = 0;
	m_bFreeze = TRUE;

	m_ptSize.Set(200, 200, 0);
}

/*! \brief 
   Destroys the plane object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the plane object..	 
*/

Plane::~Plane()
{

}

/**
\brief	Gets the corner x coordinate.

\details This is the upper-left corner of the visible plane.

\author	dcofer
\date	4/18/2011

\return	x coordinate of the upper-left corner.
**/
float Plane::CornerX() {return m_oAbsPosition.x-(m_ptSize.x/2.0f);}

/**
\brief	Gets the corner y coordinate.

\details This is the upper-left corner of the visible plane.

\author	dcofer
\date	4/18/2011

\return	y coordinate of the upper-left corner.
**/
float Plane::CornerY() {return m_oAbsPosition.y-(m_ptSize.y/2.0f);}

/**
\brief	Gets the width of a segment for the x dimension of the plane.

\details We can split the plane into widht/length sections. This primarily used to tile
the plane for texturing so that a texture is repeated multiple times instead of being stretched
over the whole plane only once. This property controls the tiling in the x direction.

\author	dcofer
\date	4/18/2011

\return	Grid x size.
**/
float Plane::GridX() {return (float) m_iWidthSegments;}

/**
\brief	Gets the length of a segment for the y dimension of the plane.

\details We can split the plane into widht/length sections. This primarily used to tile
the plane for texturing so that a texture is repeated multiple times instead of being stretched
over the whole plane only once. This property controls the tiling in the y direction.

\author	dcofer
\date	4/18/2011

\return	Grid y size.
**/
float Plane::GridY() {return (float) m_iLengthSegments;}

/**
\brief	Gets the size of the visible plane.

\author	dcofer
\date	4/18/2011

\return	size.
**/
CStdFPoint Plane::Size() {return m_ptSize;}

/**
\brief	Sets the size (x,y) of the visible plane.

\author	dcofer
\date	4/18/2011

\param	ptPoint	   	The point. 
\param	bUseScaling	true to use unit scaling.

\except The x and y values must be greater than zero.
**/
void Plane::Size(CStdFPoint ptPoint, BOOL bUseScaling)
{
	if(ptPoint.x <= 0)
		THROW_PARAM_ERROR(Al_Err_lInavlidPlaneSize, Al_Err_strInavlidPlaneSize, "X Size", ptPoint.x);
	if(ptPoint.y <= 0)
		THROW_PARAM_ERROR(Al_Err_lInavlidPlaneSize, Al_Err_strInavlidPlaneSize, "Y Size", ptPoint.y);

	if(bUseScaling)
		m_ptSize = ptPoint * m_lpSim->InverseDistanceUnits();
	else
		m_ptSize = ptPoint;

	Resize();
}

void Plane::Size(string strXml, BOOL bUseScaling)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Size");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Size", vPos);
	Size(vPos, bUseScaling);
}

/**
\brief	Gets the width segments.

\details This is the number of segments to break the x dimension up into.

\author	dcofer
\date	4/18/2011

\return	segments.
**/
int Plane::WidthSegments() {return m_iWidthSegments;}

/**
\brief	Sets the width segments.

\details This is the number of segments to break the x dimension up into.

\author	dcofer
\date	4/18/2011

\param	iVal	The new value.
\except Must be greater than zero.
**/
void Plane::WidthSegments(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, TRUE, "Plane.WidthSegments");
	m_iWidthSegments = iVal;

	Resize();
}

/**
\brief	Gets the length segments.

\details This is the number of segments to break the y dimension up into.

\author	dcofer
\date	4/18/2011

\return	segments.
**/
int Plane::LengthSegments() {return m_iLengthSegments;}

/**
\brief	Sets the length segments.

\details This is the number of segments to break the y dimension up into.

\author	dcofer
\date	4/18/2011

\param	iVal	The new value.
\except Must be greater than zero.
**/
void Plane::LengthSegments(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, TRUE, "Plane.LengthSegments");
	m_iLengthSegments = iVal;

	Resize();
}

BOOL Plane::AllowMouseManipulation() {return FALSE;}

BOOL Plane::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "SIZE")
	{
		Size(strValue);
		return TRUE;
	}

	if(strType == "LENGTHSEGMENTS")
	{
		LengthSegments(atoi(strValue.c_str()));
		return TRUE;
	}

	if(strType == "WIDTHSEGMENTS")
	{
		WidthSegments(atoi(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Plane::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Size", vPos);
	Size(vPos);
	
	WidthSegments(oXml.GetChildInt("WidthSegments", m_iWidthSegments));
	LengthSegments(oXml.GetChildInt("LengthSegments", m_iLengthSegments));

	oXml.OutOfElem(); //OutOf RigidBody Element

	//Rotation is always (1.57, 0, 0). This rotates it so that the Y axis is up.
	m_oRotation.Set(-1.57f, 0, 0);
	//Density is always zero, because it is always frozen.
	m_fltDensity = 0;
	m_bFreeze = TRUE;
}

		}		//Bodies
	}			//Environment
}				//AnimatSim