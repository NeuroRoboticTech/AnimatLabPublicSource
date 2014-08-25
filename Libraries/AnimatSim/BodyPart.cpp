/**
\file	BodyPart.cpp

\brief	Implements the body part class. 
**/

#include "StdAfx.h"
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
#include "Light.h"
#include "LightManager.h"
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
	m_bSynchWithRobot = false;
	m_fltSynchUpdateInterval = 0;
	m_iSynchUpdateInterval = 0;
	m_iSynchCount = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
BodyPart::~BodyPart(void)
{

try
{
	m_aryRobotParts.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BodyPart\r\n", "", -1, false, true);}
}

#pragma region AccessorMutators

/**
\brief	Called to collect any body data for this part. 

\author	dcofer
\date	3/2/2011
**/
void BodyPart::UpdateData()
{
	if(m_lpPhysicsMovableItem && NeedsRobotSynch())
		m_lpPhysicsMovableItem->Physics_CollectData();
}

/**
 \brief UpdateData is called during this body parts sim update call, and before any of its child updates because those children may need import
 information like this parts position. However, there are a number of pieces of information that are not critical to the part itself, but that a user
 may have asked for. I do not want to collect that info for every part if it is not needed. So instead, if a user asks for it then this part is added
 to a list on the simulation. After all parts have been updated for a simulation step then we loop through the list of just the necessary parts
 and call UpdateExtraData to collect this additional data. It is important that this be done after all parts have stepped becasue some of this data
 will only be correct at the end. An example of this is force applied to a part. Forces can be added by any child parts, so it is only at the end
 that these values are valid.

 \author    David Cofer
 \date  12/29/2013
 */
void BodyPart::UpdateExtraData()
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_CollectExtraData();
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
	m_lpPhysicsBody = lpBody;
}

/**
\brief	Called when this object has been resized.

\details This method is called when an item is resized. It is overloaded in the derived class and
allows that child class to perform any necessary graphics/physics calls for the resize event. 

\author	dcofer
\date	3/2/2011
**/
void BodyPart::Resize() 
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_Resize();

	if(m_lpCallback)
		m_lpCallback->SizeChanged();
}

/**
\brief	Gets whether the m_bRobotAdpaterSynch flag applies to this adapter.

\discussion Adpaters between neural elements should not need to be synched because 
			they are not dependent on IO timing. This flag allows you to control this by setting it to false
			for adapters that do not need it.

\author	dcofer
\date	6/30/2014

\return	Synch status of this adapter. 
**/
bool BodyPart::SynchWithRobot() {return m_bSynchWithRobot;}

/**
\brief	Determines whether the m_bRobotAdpaterSynch flag applies to this adapter. 

\author	dcofer
\date	6/30/2014

\param	bVal Synch status of this adapter. 
**/
void BodyPart::SynchWithRobot(bool bVal) {m_bSynchWithRobot = bVal;}

/**
\brief	This is how often we need to update this particular adapter.

\discussion For example, if you are using a round robin scheme with a robot IO update time of 5 ms with 4 motors, then you would set this to be 20 ms.

\author	dcofer
\date	6/30/2014

\return	interval. 
**/
float BodyPart::SynchUpdateInterval() {return m_fltSynchUpdateInterval;}

/**
\brief	Determines how often we need to update this particular adapter.

\author	dcofer
\date	6/30/2014

\param	fltVal interval. 
**/
void BodyPart::SynchUpdateInterval(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "SynchUpdateInterval", true);
	m_fltSynchUpdateInterval = fltVal;

	if(m_lpSim)
	{
		float fltTimeStep = m_lpSim->PhysicsTimeStep();

		if(fltTimeStep > 0)
			m_iSynchUpdateInterval = (int) ((m_fltSynchUpdateInterval/fltTimeStep) + 0.5);
		else
			m_iSynchUpdateInterval = 0;
	}
}

#pragma endregion

void BodyPart::Selected(bool bValue, bool bSelectMultiple)
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

void BodyPart::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)
{
	if(m_lpCallback)
		m_lpCallback->AddBodyClicked(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ);
}

void BodyPart::WakeDynamics()
{
    if(m_lpPhysicsBody)
        m_lpPhysicsBody->Physics_WakeDynamics();
}

#pragma region DataAccesMethods

void BodyPart::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Node::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	m_lpMovableSim = lpSim;
}

float *BodyPart::GetDataPointer(const std::string &strDataType)
{
	return MovableItem::GetDataPointer(strDataType);
}

bool BodyPart::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(Node::SetData(strDataType, strValue, false))
		return true;

	if(MovableItem::SetData(strDataType, strValue, false))
		return true;
	
	std::string strType = Std_CheckString(strDataType);

	if(strType == "SYNCHWITHROBOT")
	{
		SynchWithRobot(Std_ToBool(strValue));
		return true;
	}

	if(strType == "SYNCHUPDATEINTERVAL")
	{
		SynchUpdateInterval(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BodyPart::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Node::QueryProperties(aryProperties);
	MovableItem::QueryProperties(aryProperties);
	aryProperties.Add(new TypeProperty("SynchWithRobot", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SynchUpdateInterval", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

/**
\brief	Gets a pointer to the roboto part interface associated with this body part.

\author	dcofer
\date	4/25/2014
**/
CStdArray<RobotPartInterface *> *BodyPart::GetRobotPartInterfaces() {return &m_aryRobotParts;};

/**
\brief	Sets a pointer to the roboto part interface associated with this body part.

\author	dcofer
\date	4/25/2014

\param	lpPart	The new robot part. 
**/
void BodyPart::AddRobotPartInterface(RobotPartInterface *lpPart) 
{
	if(FindRobotPartListIndex(lpPart->ID(), false) == -1)
		m_aryRobotParts.Add(lpPart);
};

/**
\brief	Sets a pointer to the roboto part interface associated with this body part.

\author	dcofer
\date	4/25/2014

\param	lpPart	The new robot part. 
**/
void BodyPart::RemoveRobotPartInterface(RobotPartInterface *lpPart) 
{
	int iIdx = FindRobotPartListIndex(lpPart->ID(), false);
	if(iIdx >= 0)
		m_aryRobotParts.RemoveAt(iIdx);
};

/**
\brief	Finds the index of a robot part attached to this body part with the matching ID.

\author	dcofer
\date	4/25/2014

\param	strID	Part ID to find. 
\param	bThrowError	If true and the part is not found it throws an exception. If false and not found it returns -1. 
**/
int BodyPart::FindRobotPartListIndex(std::string strID, bool bThrowError)
{
	int iCount = m_aryRobotParts.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(m_aryRobotParts[iIdx]->ID() == strID)
			return iIdx;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lPartInterfaceIDNotFound, Al_Err_strPartInterfaceIDNotFound, "ID", strID);

	return -1;
}

#pragma endregion

/**
\brief	If the time step is modified then we need to recalculate the length of the delay buffer.

\discussion If a neural module has been assigned to this adapter then that is its target module and
we need to use the time step associated with it to determine how big the delay buffer should be in length.
If the module is NULL then the target for this adapter is the physics engine and we should use the physics time step instead.

\author	dcofer
\date	5/15/2014
**/
void BodyPart::TimeStepModified()
{
	Node::TimeStepModified();
	SynchUpdateInterval(m_fltSynchUpdateInterval);
}

bool BodyPart::NeedsRobotSynch()
{
	if(!m_lpSim->InSimulation() || !m_lpSim->RobotAdpaterSynch() || !m_bSynchWithRobot || m_iSynchUpdateInterval <= 0)
		return true;
	
	bool bRet = false;

	if(m_iSynchCount == m_iSynchUpdateInterval)
	{
		m_iSynchCount = 0;
		bRet = true;
	}
	else
		m_iSynchCount++;

	return bRet;
}

void BodyPart::ResetSimulation()
{
	Node::ResetSimulation();

	m_iSynchCount = 0;
}

void BodyPart::Initialize()
{
	Node::Initialize();
	SynchUpdateInterval(m_fltSynchUpdateInterval);
}

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

	oXml.IntoElem();  //Into BodyPart Element

	SynchWithRobot(oXml.GetChildBool("SynchWithRobot", m_bSynchWithRobot));
	SynchUpdateInterval(oXml.GetChildFloat("SynchUpdateInterval", m_fltSynchUpdateInterval));

	oXml.OutOfElem(); //OutOf BodyPart Element
}

	}			//Environment
}				//AnimatSim