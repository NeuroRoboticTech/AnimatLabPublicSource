/**
\file	VoltageClamp.cpp

\brief	Implements the voltage clamp class. 
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
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "ExternalStimulus.h"
#include "ExternalInputStimulus.h"
#include "VoltageClamp.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/17/2011
**/
VoltageClamp::VoltageClamp()
{
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	m_lpTotalCurrent = NULL;
	m_lpVrest = NULL;
	m_lpGm = NULL;
	m_fltVtarget = -0.70f;
	m_fltActiveCurrent = 0;
	m_fltTargetCurrent = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/17/2011
**/
VoltageClamp::~VoltageClamp()
{

try
{
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	m_lpTotalCurrent = NULL;
	m_lpVrest = NULL;
	m_lpGm = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VoltageClamp\r\n", "", -1, false, true);}
}

std::string VoltageClamp::Type() {return "VoltageClamp";}

/**
\brief	Gets the GUID ID of the target node that will be enabled. 

\author	dcofer
\date	3/17/2011

\return	GUID ID of the node. 
**/
std::string VoltageClamp::TargetNodeID() {return m_strTargetNodeID;}

/**
\brief	Sets the GUID ID of the target node to enable. 

\author	dcofer
\date	3/17/2011

\param	strID	GUID ID. 
**/
void VoltageClamp::TargetNodeID(std::string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);
	m_strTargetNodeID = strID;
}

/**
\brief	Gets the target voltage for the clamp. 

\author	dcofer
\date	3/17/2011

\return	Target voltage. 
**/
float VoltageClamp::Vtarget() {return m_fltVtarget;}

/**
\brief	Sets the target voltage for the clamp. 

\author	dcofer
\date	3/17/2011

\param	fltVal	The new value. 
**/
void VoltageClamp::Vtarget(float fltVal)
{
	m_fltVtarget = fltVal;
	if(m_lpGm)
		m_fltTargetCurrent = (m_fltVtarget - *m_lpVrest)*(*m_lpGm);
}

void VoltageClamp::Initialize()
{
	ExternalStimulus::Initialize();

	//Lets try and get the node we will dealing with.
	m_lpNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpExternalCurrent = m_lpNode->GetDataPointer("ExternalCurrent");
	m_lpTotalCurrent = m_lpNode->GetDataPointer("TotalCurrent");
	m_lpVrest = m_lpNode->GetDataPointer("Vrest");
	m_lpGm = m_lpNode->GetDataPointer("Gm");

	//Calculate the target current to keep the voltage at the target level.
	m_fltTargetCurrent = (m_fltVtarget - *m_lpVrest)*(*m_lpGm);

	if(!m_lpExternalCurrent)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Stimulus: " + m_strID + "Node: " + m_strTargetNodeID + " DataType: ExternalCurrent"));
}

void VoltageClamp::Activate()
{
	ExternalStimulus::Activate();

	m_fltActiveCurrent = m_fltTargetCurrent - (*m_lpTotalCurrent);
	*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
}

void VoltageClamp::StepSimulation()
{
	m_fltActiveCurrent = m_fltTargetCurrent - (*m_lpTotalCurrent);
	*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
}

void VoltageClamp::Deactivate()
{		
	ExternalStimulus::Deactivate();
	//*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
}

void VoltageClamp::ResetSimulation()
{
	ExternalStimulus::ResetSimulation();
	
	m_fltActiveCurrent = 0;
}

float *VoltageClamp::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	if(strType == "CLAMPCURRENT")
		lpData = &m_fltActiveCurrent;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 

bool VoltageClamp::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(ExternalStimulus::SetData(strDataType, strValue, false))
		return true;

	if(strType == "VTARGET")
	{
		Vtarget((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void VoltageClamp::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	ExternalStimulus::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Vtarget", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

void VoltageClamp::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	TargetNodeID(oXml.GetChildString("TargetNodeID"));
	Vtarget(oXml.GetChildFloat("Vtarget", m_fltVtarget));

	oXml.OutOfElem(); //OutOf Simulus Element
}

	}			//ExternalStimuli
}				//VortexAnimatSim




