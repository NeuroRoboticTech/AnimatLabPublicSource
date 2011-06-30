// FluidPlane.cpp: implementation of the FluidPlane class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Plane.h"
#include "FluidPlane.h"
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

/**
\brief	Default constructor.

\author	dcofer
\date	6/30/2011
**/
FluidPlane::FluidPlane()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	6/30/2011
**/
FluidPlane::~FluidPlane()
{

}


/**
\brief	Gets the velocity of the fluid.

\author	dcofer
\date	3/2/2011

\return	returns velocity. 
**/
CStdFPoint FluidPlane::Velocity() {return m_vVelocity;}

/**
\brief	Sets the velocity of the fluid.

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new point to use to set the velocity.
\param	bUseScaling   	If true then the position values that are passed in will be scaled by the
						unit scaling values.
**/
void FluidPlane::Velocity(CStdFPoint &oPoint, BOOL bUseScaling) 
{
	if(bUseScaling)
		m_vVelocity = oPoint * m_lpMovableSim->InverseDistanceUnits();
	else
		m_vVelocity = oPoint;
}

/**
\brief	Sets the velocity of the fluid.

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void FluidPlane::Velocity(float fltX, float fltY, float fltZ, BOOL bUseScaling) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	Velocity(vPos, bUseScaling);
}

/**
\brief	Sets the velocity of the fluid. This method is primarily used by the GUI to
reset the velocity using an xml data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the position. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void FluidPlane::Velocity(string strXml, BOOL bUseScaling)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Velocity");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Velocity", vPos);
	Velocity(vPos, bUseScaling);
}

/**
\brief	Sets the gravity of the fluid.

\details For some reason we must set the gravity again within the fluid in the vortex simulator.
This method allows me to do that. It will be overridden in the VsFluidPlane object to do this.

\author	dcofer
\date	6/30/2011
**/
void FluidPlane::SetGravity()
{}

BOOL FluidPlane::AllowRotateDragX() {return FALSE;}

BOOL FluidPlane::AllowRotateDragY() {return FALSE;}

BOOL FluidPlane::AllowRotateDragZ() {return FALSE;}

/**
\brief	Sets the rotation of the rigid body.

\details We do not allow a fluid plane to be rotated.

\author	dcofer
\date	6/30/2011

\param [in,out]	oPoint  	The point.
\param	bFireChangeEvent	true to fire change event.
\param	bUpdateMatrix   	true to update matrix.
**/
void FluidPlane::Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
}

BOOL FluidPlane::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Plane::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "VELOCITY")
	{
		Velocity(strValue);
		return TRUE;
	}

	if(strType == "GRAVITY")
	{
		SetGravity();
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void FluidPlane::Load(CStdXml &oXml)
{
	Plane::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	CStdFPoint vPoint;
	Std_LoadPoint(oXml, "Velocity", vPoint, FALSE);
	Velocity(vPoint);

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim