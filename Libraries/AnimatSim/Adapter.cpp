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
	m_bConnectedToPhysics = false;
	m_fltNextVal = 0;
	m_fltCalculatedVal = 0;
	m_fltDelayBufferInterval = 0;
	m_eDelayBufferMode = NoDelayBuffer;
	m_fltRobotIOScale = 1;
	m_fltInitIODisableDuration = 0;
	m_iTargetDataType = 0;
	m_bSynchWithRobot = false;
	m_fltSynchUpdateInterval = 0;
	m_iSynchUpdateInterval = 0;
	m_fltSynchUpdateStartInterval = 0;
	m_iSynchUpdateStartInterval = 0;
	m_iSynchCount = 0;
	m_iSynchTarget = -1;
	m_fltUpdatedValue = 0;
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

/**
\brief	Returns whether or not this adpater is connected to a physics object or not.

\author	dcofer
\date	5/13/2014

\return	Pointer to the gain.
**/
bool Adapter::ConnectedToPhysics() {return m_bConnectedToPhysics;}

/**
\brief	Returns the mode for the delay buffer.

\discussion This is what determines whether a delay buffer is used at all or only during the simulation. See eDelayBufferMode.

\author	dcofer
\date	5/15/2014

\return	mode.
**/
eDelayBufferMode Adapter::DelayBufferMode()
{
	return m_eDelayBufferMode;
}

/**
\brief	Sets the target data type.

\discussion This is what determines whether a delay buffer is used at all or only during the simulation. See eDelayBufferMode.

\author	dcofer
\date	3/18/2011

\param	eMode	new mode. 
**/
void Adapter::DelayBufferMode(eDelayBufferMode eMode)
{
	m_eDelayBufferMode = eMode;
	SetDelayBufferSize();
}

/**
\brief	Returns the time interval used for the delay buffer if this adapter is set to use one.

\author	dcofer
\date	5/15/2014

\return	interval.
**/
float Adapter::DelayBufferInterval() {return m_fltDelayBufferInterval;}

/**
\brief	Sets the time interval used for the delay buffer if this adapter is set to use one.

\author	dcofer
\date	3/18/2011

\param	fltVal	new time step. 
**/
void Adapter::DelayBufferInterval(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "DelayBufferInterval", true);
	m_fltDelayBufferInterval = fltVal;
	SetDelayBufferSize();
}

/**
\brief	Gets the scale value used for calculated values for this adapter during simulation mode only. 

\discussion If you are modeling a robot then you can use this to scale the IO of this adapter to match the real response of the robot.
			An example where this might be used is while simulating a motor. Real motors usually end up having a slightly slower response
			time than you get in the simulation. The value specified here is a percentage with 1 at 100%. To slow the simulated response time
			down slightly you could set it to 0.95 instead. This will only be applied during the simulation, NOT during the running of the real
			robot. This is only so you can try and tune your simulation repsonse to more closely match the real robot response.

\author	dcofer
\date	5/26/2014

\return	IO percentage. 
**/
float Adapter::RobotIOScale() {return m_fltRobotIOScale;}

/**
\brief	Sets the scale value used for calculated values for this adapter during simulation mode only. 

\discussion If you are modeling a robot then you can use this to scale the IO of this adapter to match the real response of the robot.
			An example where this might be used is while simulating a motor. Real motors usually end up having a slightly slower response
			time than you get in the simulation. The value specified here is a percentage with 1 at 100%. To slow the simulated response time
			down slightly you could set it to 0.95 instead. This will only be applied during the simulation, NOT during the running of the real
			robot. This is only so you can try and tune your simulation repsonse to more closely match the real robot response.

\author	dcofer
\date	5/26/2014

\param	fltVal	new IO percentage. 
**/
void Adapter::RobotIOScale(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "RobotIOScale");
	m_fltRobotIOScale = fltVal;
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
bool Adapter::SynchWithRobot() {return m_bSynchWithRobot;}

/**
\brief	Determines whether the m_bRobotAdpaterSynch flag applies to this adapter. 

\author	dcofer
\date	6/30/2014

\param	bVal Synch status of this adapter. 
**/
void Adapter::SynchWithRobot(bool bVal) {m_bSynchWithRobot = bVal;}

/**
\brief	This is how often we need to update this particular adapter.

\discussion For example, if you are using a round robin scheme with a robot IO update time of 5 ms with 4 motors, then you would set this to be 20 ms.

\author	dcofer
\date	6/30/2014

\return	interval. 
**/
float Adapter::SynchUpdateInterval() {return m_fltSynchUpdateInterval;}

/**
\brief	Determines how often we need to update this particular adapter.

\author	dcofer
\date	6/30/2014

\param	fltVal interval. 
**/
void Adapter::SynchUpdateInterval(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "SynchUpdateInterval", true);
	m_fltSynchUpdateInterval = fltVal;

	if(m_lpSim)
	{
		float fltTimeStep = m_lpSim->PhysicsTimeStep();
		if(m_lpModule)
			fltTimeStep = m_lpModule->TimeStep();

		if(fltTimeStep > 0)
			m_iSynchUpdateInterval = (int) ((m_fltSynchUpdateInterval/fltTimeStep) + 0.5);
		else
			m_iSynchUpdateInterval = 0;
	}
}

/**
\brief	This is the interval that this adapter waits the first time before doing its update.

\discussion For example, if you are using a round robin scheme with a robot IO update time of 5 ms with 4 motors, 
then the update interval would be set to 20 ms. You would set the start interval of motor 1 to 0, motor 2 to 5 ms, 
motor 3 to 10 ms, and motor 4 to 15 ms. This would stagger them in a round robin fashion the same way it was being
processed on the real robot.

\author	dcofer
\date	6/30/2014

\return	interval. 
**/
float Adapter::SynchUpdateStartInterval() {return m_fltSynchUpdateStartInterval;}

/**
\brief	This is the interval that this adapter waits the first time before doing its update.

\discussion For example, if you are using a round robin scheme with a robot IO update time of 5 ms with 4 motors, 
then the update interval would be set to 20 ms. You would set the start interval of motor 1 to 0, motor 2 to 5 ms, 
motor 3 to 10 ms, and motor 4 to 15 ms. This would stagger them in a round robin fashion the same way it was being
processed on the real robot.

\author	dcofer
\date	6/30/2014

\param	fltVal interval. 
**/
void Adapter::SynchUpdateStartInterval(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "SynchUpdateStartInterval", true);
	m_fltSynchUpdateStartInterval = fltVal;

	if(m_lpSim)
	{
		float fltTimeStep = m_lpSim->PhysicsTimeStep();
		if(m_lpModule)
			fltTimeStep = m_lpModule->TimeStep();

		if(fltTimeStep > 0)
			m_iSynchUpdateStartInterval = (int) ((m_fltSynchUpdateStartInterval/fltTimeStep) + 0.5);
		else
			m_iSynchUpdateStartInterval = 0;
	}
}

/**
\brief	Gets the duration for how long this adapter is disabled at the start of the simulation. 

\discussion It is sometimes useful to disable IO operations briefly at the start of the simulation to give neural systems a chance to stabilize.
			This param defines how long it should disable IO for this adapter at the start of the sim..

\author	dcofer
\date	6/11/2014

\return	duration for which IO is disabled at the start of the simulation. 
**/
float Adapter::InitIODisableDuration()
{
	return m_fltInitIODisableDuration;
}

/**
\brief	Sets the duration for how long this adapter is disabled at the start of the simulation. 

\discussion It is sometimes useful to disable IO operations briefly at the start of the simulation to give neural systems a chance to stabilize.
			This param defines how long it should disable IO for this adapter at the start of the sim..

\author	dcofer
\date	6/11/2014

\param	fltVal duration for which IO is disabled at the start of the simulation. 
**/
void Adapter::InitIODisableDuration(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "InitIODisableDuration", true);
	m_fltInitIODisableDuration = fltVal;
}

void Adapter::DetachAdaptersFromSimulation()
{
	if(m_lpSim)
	{
		m_lpSim->RemoveSourceAdapter(m_lpStructure, this);
		m_lpSim->RemoveTargetAdapter(m_lpStructure, this);
	}
}

void Adapter::AddExternalNodeInput(int iTargetDataType, float fltInput)
{
	THROW_TEXT_ERROR(Al_Err_lOpNotDefinedForAdapter, Al_Err_strOpNotDefinedForAdapter, "AddExternalNodeInput");
}

float *Adapter::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "CALCULATEDVAL")
		return &m_fltCalculatedVal;
	else if(strType == "NEXTVAL")
		return &m_fltNextVal;
	else if(strType == "UPDATEDVALUE")
		return &m_fltUpdatedValue;

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
	TargetDataType(oXml.GetChildString("TargetDataType"));

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

	if(strType == "DELAYBUFFERMODE")
	{
		DelayBufferMode((eDelayBufferMode) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "DELAYBUFFERINTERVAL")
	{
		DelayBufferInterval(atof(strValue.c_str()));
		return true;
	}

	if(strType == "ROBOTIOSCALE")
	{
		RobotIOScale(atof(strValue.c_str()));
		return true;
	}

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

	if(strType == "SYNCHUPDATESTARTINTERVAL")
	{
		SynchUpdateStartInterval(atof(strValue.c_str()));
		return true;
	}

	if(strType == "INITIODISABLEDURATION")
	{
		InitIODisableDuration(atof(strValue.c_str()));
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
	aryProperties.Add(new TypeProperty("CalculatedVal", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("NextVal", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("UpdatedValue", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("DelayBufferMode", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("DelayBufferInterval", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RobotIOScale", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SynchWithRobot", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SynchUpdateInterval", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SynchUpdateStartInterval", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("InitIODisableDuration", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

/**
\brief	If the time step is modified then we need to recalculate the length of the delay buffer.

\discussion If a neural module has been assigned to this adapter then that is its target module and
we need to use the time step associated with it to determine how big the delay buffer should be in length.
If the module is NULL then the target for this adapter is the physics engine and we should use the physics time step instead.

\author	dcofer
\date	5/15/2014
**/
void Adapter::TimeStepModified()
{
	Node::TimeStepModified();

	SetDelayBufferSize();
	SynchUpdateInterval(m_fltSynchUpdateInterval);
	SynchUpdateStartInterval(m_fltSynchUpdateStartInterval);
}

bool Adapter::NeedsRobotSynch()
{
	if(!m_bSynchWithRobot || m_iSynchUpdateInterval <= 0)
		return true;

	if(m_iSynchCount == m_iSynchTarget)
	{
		m_iSynchTarget = m_iSynchUpdateInterval;
		m_iSynchCount = 0;
		return true;
	}
	else
		return false;
}

void Adapter::ResetSimulation()
{
	Node::ResetSimulation();

	m_fltNextVal = 0;
	m_fltCalculatedVal = 0;
	m_iSynchCount = 0;
	m_iSynchTarget = -1;
	m_fltUpdatedValue = 0;
	int iSize = m_aryDelayBuffer.GetSize();
	for(int iIdx=0; iIdx<iSize; iIdx++)
		m_aryDelayBuffer[iIdx] = 0;
}

void Adapter::SetDelayBufferSize()
{
	if(m_eDelayBufferMode == eDelayBufferMode::DelayBufferAlwaysOn || 
	  (m_eDelayBufferMode == eDelayBufferMode::DelayBufferInSimOnly && m_lpSim->InSimulation()))
	{
		float fltTimeStep = m_lpSim->PhysicsTimeStep();
		if(m_lpModule)
			fltTimeStep = m_lpModule->TimeStep();

		if(fltTimeStep > 0)
		{
			int iLength = (int) (m_fltDelayBufferInterval/fltTimeStep);
			m_aryDelayBuffer.SetSize(iLength);
		}
	}
	else
		m_aryDelayBuffer.RemoveAll();
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

	//Get the integer of the target data type we should use when calling AddExternalNodeInput. Zero is the default and only one most
	//systems use.
	m_iTargetDataType = m_lpTargetNode->GetTargetDataTypeIndex(m_strTargetDataType);

	m_lpSim->AttachSourceAdapter(m_lpStructure, this);
	m_lpSim->AttachTargetAdapter(m_lpStructure, this);

	m_bConnectedToPhysics = m_lpSim->IsPhysicsAdapter(this);

	//Reset times now that we are initialized
	SetDelayBufferSize();
	SynchUpdateInterval(m_fltSynchUpdateInterval);
	SynchUpdateStartInterval(m_fltSynchUpdateStartInterval);
}

void Adapter::StepSimulation()
{
	m_fltUpdatedValue = 0;

	////Test code
	//int i=5;
	//if(Std_ToLower(m_strID) == "8a07c86e-3972-43f8-b91d-9b4b58605a82" && m_lpSim->Time() > 0.2) //
	//	i=6;

	if(m_bEnabled && m_lpSim->Time() >= m_fltInitIODisableDuration)
	{
		//If this is the first time we are coming in here then setup the first update correctly 
		if(m_bSynchWithRobot && m_iSynchTarget < 0)
			m_iSynchTarget = m_iSynchUpdateStartInterval;

		//If we are trying to synch the adapters to match the IO charachteristics of a robot then we should only
		//calcualte the value from the source data based on the robot synch interval. Otherwise, use the value we calculated last time.
		if(!m_lpSim->RobotAdpaterSynch() || !m_bSynchWithRobot || (m_lpSim->RobotAdpaterSynch() && m_bSynchWithRobot && NeedsRobotSynch()))
		{
			m_fltCalculatedVal = m_lpGain->CalculateGain(*m_lpSourceData);
			m_fltUpdatedValue = 1;

			////Test code
			//int i=5;
			//if(Std_ToLower(m_strID) == "3953f16f-99aa-41e7-adaf-e0ca153d65ec" && m_fltCalculatedVal > 0.6 ) // && m_lpSim->Time() > 1
			//	i=6;

			//Scale the calculated value for robot performance matching if we are in simulation mode only.
			if(m_lpSim->InSimulation())
				m_fltCalculatedVal *= m_fltRobotIOScale;
		}

		//If we 5are configured to use a delay buffer then use it.
		if(m_eDelayBufferMode == eDelayBufferMode::DelayBufferAlwaysOn || 
		  (m_eDelayBufferMode == eDelayBufferMode::DelayBufferInSimOnly && m_lpSim->InSimulation()))
		{
			//First get the head element off the buffer.
			float fltNextVal = m_aryDelayBuffer.GetHead();
			//Now set the current value into the buffer.
			m_aryDelayBuffer.AddEnd(m_fltCalculatedVal);

			//Now reset the next val with the one from the buffer
			m_fltNextVal = fltNextVal;
		}
		else
			m_fltNextVal = m_fltCalculatedVal;

		m_lpTargetNode->AddExternalNodeInput(m_iTargetDataType, m_fltNextVal);

		m_iSynchCount++;
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
	TargetDataType(oXml.GetChildString("TargetDataType"));

	SetGain(LoadGain(m_lpSim, "Gain", oXml));
	
	Enabled(oXml.GetChildBool("Enabled"));

	DelayBufferMode((eDelayBufferMode) oXml.GetChildInt("DelayBufferMode", m_eDelayBufferMode));
	DelayBufferInterval(oXml.GetChildFloat("DelayBufferInterval", m_fltDelayBufferInterval));

	RobotIOScale(oXml.GetChildFloat("RobotIOScale", m_fltRobotIOScale));
	SynchWithRobot(oXml.GetChildBool("SynchWithRobot", m_bSynchWithRobot));
	SynchUpdateInterval(oXml.GetChildFloat("SynchUpdateInterval", m_fltSynchUpdateInterval));
	SynchUpdateStartInterval(oXml.GetChildFloat("SynchUpdateStartInterval", m_fltSynchUpdateStartInterval));
	InitIODisableDuration(oXml.GetChildFloat("InitIODisableDuration", m_fltInitIODisableDuration));

	oXml.OutOfElem(); //OutOf Adapter Element
}


	}			//Adapters
}			//AnimatSim
