/**
\file	BodyPart.cpp

\brief	Implements the body part class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBase.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
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
/**
\brief	Default constructor. 

\author	dcofer
\date	3/2/2011
**/
BodyPart::BodyPart(void)
{
	m_lpPhysicsBody = NULL;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
BodyPart::~BodyPart(void)
{
}

#pragma region AccessorMutators

/**
\brief	Called to collect any body data for this part. 

\author	dcofer
\date	3/2/2011
**/
void BodyPart::UpdateData()
{
	if(m_lpPhysicsBase)
		m_lpPhysicsBase->Physics_CollectData();
}

/**
\brief	Gets the physics body interface pointer. This is an interface reference to the Vs version
of this object. It will allow us to call methods directly in the Vs (OSG) version of the object
directly without having to overload a bunch of methods in each box, sphere, etc.. 

\author	dcofer
\date	3/2/2011

\return	Pointer to Vs interface, NULL else. 
**/
IPhysicsBody *BodyPart::PhysicsBody() {return m_lpPhysicsBody;}

/**
\brief	Sets the physics body interface pointer. This is an interface reference to the Vs version
of this object. It will allow us to call methods directly in the Vs (OSG) version of the object
directly without having to overload a bunch of methods in each box, sphere, etc.. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpBody	The pointer to the phsyics body interface. 
**/
void BodyPart::PhysicsBody(IPhysicsBody *lpBody) 
{
	m_lpPhysicsBase = lpBody;
	m_lpPhysicsBody = lpBody;
}

/**
\brief	Called when this object has been resized.

\details This method is called when an item is resized. It is overloaded in the derived class and
allows that child class to perform any necessary graphics/physics calls for the resize event. 

\author	dcofer
\date	3/2/2011
**/
void BodyPart::Resize() {};

#pragma endregion

void BodyPart::Selected(BOOL bValue, BOOL bSelectMultiple)
{
	Node::Selected(bValue, bSelectMultiple);
	MovableItem::Selected(bValue, bSelectMultiple);
}

/**
\brief	Called when the visual selection mode changed in GUI.

\details In the GUI the user can select several different types of visual selection modes This
method is called any time that the user switches the selection mode in the GUI. This allows us to
change the current Alpha value of the objects so the display is correct. 

\author	dcofer
\date	3/2/2011

\param	iNewMode	The new VisualSelectionMode. 
**/
void BodyPart::VisualSelectionModeChanged(int iNewMode)
{
	Node::VisualSelectionModeChanged(iNewMode);
	MovableItem::VisualSelectionModeChanged(iNewMode);
}

/**
\brief	Called when the user clicks on this object while the AddBody mode is active.

\details When the user selects the AddBody mode in the GUI then the simulation detects when a
part is clicked in the simulation window. (This occurs in the VsCameraManipulator class). It gets
the position of the click in global coordinates, and the normal vector for the surface that was
clicked. We then need to pass this info back up the GUI and let it know the click occurred. This
method uses the IMovableItemCallback object to send this info back up to the GUI. 

\author	dcofer
\date	3/2/2011

\param	fltPosX		The position x coordinate. 
\param	fltPosY		The position y coordinate. 
\param	fltPosZ		The position z coordinate. 
\param	fltNormX	The normal x coordinate. 
\param	fltNormY	The normal y coordinate. 
\param	fltNormZ	The normal z coordinate. 
**/
void BodyPart::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)
{
	if(m_lpCallback)
		m_lpCallback->AddBodyClicked(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ);
}

#pragma region DataAccesMethods

void BodyPart::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	Node::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	m_lpMovableSim = lpSim;
}

float *BodyPart::GetDataPointer(string strDataType)
{
	return MovableItem::GetDataPointer(strDataType);
}

BOOL BodyPart::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(Node::SetData(strDataType, strValue, FALSE))
		return true;

	if(MovableItem::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

/**
\brief	Updates the physics position from graphics.

\details This updates the position of the physcis node directly
from the scenegraph position of the graphics node for this item, and then
does the same for all child items.

\author	dcofer
\date	3/26/2011
**/
void BodyPart::UpdatePhysicsPosFromGraphics()
{
}

void BodyPart::Load(CStdXml &oXml)
{
	Node::Load(oXml);
	MovableItem::Load(oXml);
}

	}			//Environment
}				//AnimatSim