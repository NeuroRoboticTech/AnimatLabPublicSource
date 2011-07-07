/**
\file	Light.cpp

\brief	Implements a light object. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "Light.h"
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
Light::Light(void)
{
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
Light::~Light(void)
{
}

#pragma region AccessorMutators

/**
\brief	Called to collect any body data for this part. 

\author	dcofer
\date	3/2/2011
**/
void Light::UpdateData()
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_CollectData();
}

/**
\brief	Called when this object has been resized.

\details This method is called when an item is resized. It is overloaded in the derived class and
allows that child class to perform any necessary graphics/physics calls for the resize event. 

\author	dcofer
\date	3/2/2011
**/
void Light::Resize() 
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_Resize();
}

#pragma endregion

void Light::Selected(BOOL bValue, BOOL bSelectMultiple)
{
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
void Light::VisualSelectionModeChanged(int iNewMode)
{
	MovableItem::VisualSelectionModeChanged(iNewMode);
}


#pragma region DataAccesMethods

void Light::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	m_lpMovableSim = lpSim;
}

float *Light::GetDataPointer(string strDataType)
{
	return MovableItem::GetDataPointer(strDataType);
}

BOOL Light::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(AnimatBase::SetData(strDataType, strValue, FALSE))
		return true;

	if(MovableItem::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void Light::Create()
{
}

void Light::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);
	MovableItem::Load(oXml);
}

	}			//Environment
}				//AnimatSim