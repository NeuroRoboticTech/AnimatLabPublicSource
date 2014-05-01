/**
\file	Adapter.cpp

\brief	Implements the adapter class.
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
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"


namespace AnimatSim
{
	namespace Adapters
	{
/**
\brief	Default constructor.

\author	dcofer
\date	3/18/2011
**/
Adapter::Adapter()
{
	m_lpGain = NULL;
	m_lpSourceNode = NULL;
	m_lpSourceData = NULL;
	m_lpTargetNode = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
Adapter::~Adapter()
{

try
{
	m_lpSourceNode = NULL;
	m_lpSourceData = NULL;
	m_lpTargetNode = NULL;
	if(m_lpGain) delete m_lpGain;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Adapter\r\n", "", -1, false, true);}
}

/**
\brief	Gets the name of the source NeuralModule.

\author	dcofer
\date	3/18/2011

\return	Source NeuralModule name.
**/
std::string Adapter::SourceModule() {return m_strSourceModule;}

/**
\brief	Sets the Source NeuralModule name.

\author	dcofer
\date	3/18/2011

\param	strName	Name of the source NeuralModule. 
**/
void Adapter::SourceModule(std::string strName)
{
	if(Std_IsBlank(strName)) 
		THROW_TEXT_ERROR(Al_Err_lModuleNameBlank, Al_Err_strModuleNameBlank, ". Source Module. ID: " + strName);
	m_strSourceModule = strName;
}

/**
\brief	Gets the GUID ID of the source node where we will get the source data variable.

\author	dcofer
\date	3/18/2011

\return	GUID ID of the source node.
**/
std::string Adapter::SourceID() {return m_strSourceID;}

/**
\brief	Sets the GUID ID of the Source node.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID for the source node. 
**/
void Adapter::SourceID(std::string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Source ID");
	m_strSourceID = strID;
}

/**
\brief	Gets the source data type.

\author	dcofer
\date	3/18/2011

\return	Source data type.
**/
std::string Adapter::SourceDataType() {return m_strSourceDataType;}

/**
\brief	Sets the source data type.

\author	dcofer
\date	3/18/2011

\param	strType	Source DataType. 
**/
void Adapter::SourceDataType(std::string strType)
{
	if(Std_IsBlank(strType)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Source DataType");
	m_strSourceDataType = strType;
}

/**
\brief	Gets the source node.

\author	dcofer
\date	3/18/2011

\return	Pointer to the source node.
**/
Node *Adapter::SourceNode() {return m_lpSourceNode;}

/**
\brief	Gets the name of the target NeuralModule.

\author	dcofer
\date	3/18/2011

\return	Target NeuralModule name.
**/
std::string Adapter::TargetModule() {return m_strTargetModule;}

/**
\brief	Sets the target NeuralModule name.

\author	dcofer
\date	3/18/2011

\param	strName	Name of the target NeuralModule. 
**/
void Adapter::TargetModule(std::string strName)
{
	if(Std_IsBlank(strName)) 
		THROW_TEXT_ERROR(Al_Err_lModuleNameBlank, Al_Err_strModuleNameBlank, ". Target Module. ID: " + strName);
	m_strTargetModule = strName;
}

/**
\brief	Gets the GUID ID of the target node where we will add the transformed data variable.

\author	dcofer
\date	3/18/2011

\return	GUID ID of the target node.
**/
std::string Adapter::TargetID() {return m_strTargetID;}


/**
\brief	Sets the GUID ID of the target node.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID for the target node. 
**/
void Adapter::TargetID(std::string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Target ID");
	m_strTargetID = strID;
}

/**
\brief	Gets the target data type.

\author	dcofer
\date	3/18/2011

\return	Target data type.
**/
std::string Adapter::TargetDataType() {return m_strTargetDataType;}

/**
\brief	Sets the target data type.

\author	dcofer
\date	3/18/2011

\param	strType	Target DataType. 
**/
void Adapter::TargetDataType(std::string strType)
{
	if(Std_IsBlank(strType)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Target DataType");
	m_strTargetDataType = strType;
}

/**
\brief	Gets the target node.

\author	dcofer
\date	3/18/2011

\return	Pointer to the target node.
**/
Node *Adapter::TargetNode() {return m_lpTargetNode;}

/**
\brief	Gets the poitner to the gain function.

\author	dcofer
\date	3/18/2011

\return	Pointer to the gain.
**/
Gain *Adapter::GetGain() {return m_lpGain;}

void Adapter::SetGain(Gain *lpGain)
{
	if(m_lpGain)
	{
		delete m_lpGain;
		m_lpGain = NULL;
	}

	m_lpGain = lpGain;
	m_lpGain->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, true);
}

void Adapter::DetachAdaptersFromSimulation()
{
	if(m_lpSim)
	{
		m_lpSim->RemoveSourceAdapter(m_lpStructure, this);
		m_lpSim->RemoveTargetAdapter(m_lpStructure, this);
	}
}

void Adapter::AddExternalNodeInput(float fltInput)
{
	THROW_TEXT_ERROR(Al_Err_lOpNotDefinedForAdapter, Al_Err_strOpNotDefinedForAdapter, "AddExternalNodeInput");
}

float *Adapter::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "ENABLE")
		return &m_fltEnabled;

	return AnimatBase::GetDataPointer(strDataType);
}

/**
\brief	Creates and adds a gain object. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the gain. 
**/
void Adapter::AddGain(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");

	SetGain(LoadGain(m_lpSim, "Gain", oXml));
}

void Adapter::SetOriginID(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Adapter");

	oXml.IntoElem();  //Into Adapter Element

	//Load Source Data
	SourceModule(oXml.GetChildString("SourceModule"));
	SourceID(oXml.GetChildString("SourceID"));
	SourceDataType(oXml.GetChildString("SourceDataType"));

	oXml.OutOfElem(); //OutOf Adapter Element

	//Remove the adatper settings.
	m_lpSim->RemoveSourceAdapter(m_lpStructure, this);
	m_lpSim->RemoveTargetAdapter(m_lpStructure, this);

	Initialize();
}

void Adapter::SetDestinationID(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Adapter");

	oXml.IntoElem();  //Into Adapter Element

	//Load Source Data
	TargetModule(oXml.GetChildString("TargetModule"));
	TargetID(oXml.GetChildString("TargetID"));

	oXml.OutOfElem(); //OutOf Adapter Element

	//Remove the adatper settings.
	m_lpSim->RemoveSourceAdapter(m_lpStructure, this);
	m_lpSim->RemoveTargetAdapter(m_lpStructure, this);

	Initialize();
}

bool Adapter::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Node::SetData(strDataType, strValue, false))
		return true;

	if(strType == "GAIN")
	{
		AddGain(strValue);
		return true;
	}

	if(strType == "ORIGINID")
	{
		SetOriginID(strValue);
		return true;
	}

	if(strType == "DESTINATIONID")
	{
		SetDestinationID(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Data Type", strDataType);

	return false;
}

void Adapter::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Node::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Enable", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
}

void Adapter::Initialize()
{
	Node::Initialize();

	m_lpSourceNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strSourceID));
	if(!m_lpSourceNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strSourceID);

	m_lpSourceData = m_lpSourceNode->GetDataPointer(m_strSourceDataType);

	if(!m_lpSourceData)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Adapter: " + m_strID + " StructureID: " + m_lpStructure->ID() + "SourceID: " + m_strSourceID + " DataType: " + m_strSourceDataType));

	m_lpTargetNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetID));
	if(!m_lpTargetNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetID);

	m_lpSim->AttachSourceAdapter(m_lpStructure, this);
	m_lpSim->AttachTargetAdapter(m_lpStructure, this);
}

void Adapter::StepSimulation()
{
	if(m_bEnabled)
	{
		float fltInput = m_lpGain->CalculateGain(*m_lpSourceData);
		m_lpTargetNode->AddExternalNodeInput(fltInput);
	}
}

void Adapter::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	//Load Source Data
	SourceModule(oXml.GetChildString("SourceModule"));
	SourceID(oXml.GetChildString("SourceID"));
	SourceDataType(oXml.GetChildString("SourceDataType"));

	//Load Target Data
	TargetModule(oXml.GetChildString("TargetModule"));
	TargetID(oXml.GetChildString("TargetID"));

	SetGain(LoadGain(m_lpSim, "Gain", oXml));
	
	Enabled(oXml.GetChildBool("Enabled"));

	oXml.OutOfElem(); //OutOf Adapter Element
}


	}			//Adapters
}			//AnimatSim
