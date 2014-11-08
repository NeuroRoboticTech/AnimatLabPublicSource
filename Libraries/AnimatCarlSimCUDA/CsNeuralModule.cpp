/**
\file	CsNeuralModule.cpp

\brief	Implements the firing rate module class.
**/

#include "StdAfx.h"

#include "CsNeuralModule.h"
#include "CsClassFactory.h"

namespace AnimatCarlSim
{
/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsNeuralModule::CsNeuralModule()
{
	m_lpClassFactory =  new AnimatCarlSim::CsClassFactory;
	m_lpSNN = NULL;
	m_iSimMode = GPU_MODE;
	m_fltTimeStep = 0.001f;
	m_bWaitingForPhysicsToCatchUp = false;
	m_bWaitingForNeuralToCatchUp = false;
	m_fltNeuralTime = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsNeuralModule::~CsNeuralModule()
{

try
{
	m_arySynapses.RemoveAll();
	m_aryNeuronGroups.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsNeuralModule\r\n", "", -1, false, true);}
}

std::string CsNeuralModule::ModuleName() {return "AnimatCarlSimCUDA";};

void CsNeuralModule::SimMode(int iMode)
{
	if(!iMode)
		m_iSimMode = CPU_MODE;
	else
		m_iSimMode = GPU_MODE;
}

int CsNeuralModule::SimMode() {return m_iSimMode;}

/**
\brief	Tells the number of simulation steps that other components, like stimuli, will need to use for their step interval. 

\author	dcofer
\date	10/11/2014

\return	Step interval for the UpdateSteps of this module.
**/
unsigned int CsNeuralModule::SimulationStepInterval()
{
	unsigned int iInterval = 1;
	if( m_lpSim->MinTimeStep() > 0)
		iInterval = (unsigned int) ((((float) (CARLSIM_STEP_SIZE*CARLSIM_STEP_INCREMENT)) / m_lpSim->MinTimeStep())+0.5);
	return iInterval;
}


/**
\brief	Searches for an item with the specified ID and sets its index in the array. 

\author	dcofer
\date	10/2/2014

\param	strID			ID of item to find. 
\param	bThrowError		true to throw error if not found. 

\return	If bThrowError is false and item is not found it returns NULL, otherwise
if it is found then it returns pointer to the item.
\exception If bThrowError is true and no item with the specified ID is found then
an exception is thrown.
**/
CsConnectionGenerator *CsNeuralModule::FindConnectionGenerator(std::string strID, bool bThrowError)
{
	CsConnectionGenerator *lpItem = NULL;
	CStdMap<std::string, CsConnectionGenerator *>::iterator oPos;
	oPos = m_aryGenerators.find(Std_CheckString(strID));

	if(oPos != m_aryGenerators.end())
		lpItem =  oPos->second;
	else if(bThrowError)
		THROW_TEXT_ERROR(Cs_Err_lConnectionGeneratorIDNotFound, Cs_Err_strConnectionGeneratorIDNotFound, " Connection Generator ID: " + strID);

	return lpItem;
}

void CsNeuralModule::AddConnectionGenerator(std::string strID, CsConnectionGenerator *lpGen)
{
	if(lpGen)
	{
		if(!FindConnectionGenerator(strID, false))
			m_aryGenerators.Add(strID, lpGen);
	}
}

void CsNeuralModule::Kill(bool bState)
{
	int iCount = m_aryNeuronGroups.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeuronGroups[iIndex])
			m_aryNeuronGroups[iIndex]->Kill(bState);
}

/**
\brief	Searches for the neuron with the specified ID and returns its position in the list.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID of the neruon to find. 
\param	bThrowError	true to throw error if nothing found. 

\return	The found neuron list position.
**/
int CsNeuralModule::FindNeuronGroupListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryNeuronGroups.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeuronGroups[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Cs_Err_lNeuronNotFound, Cs_Err_strNeuronNotFound, "ID");

	return -1;
}

void CsNeuralModule::SetCARLSimulation()
{
	if(m_lpSNN)
	{
		delete m_lpSNN;
		m_lpSNN = NULL;
	}

	m_aryGenerators.RemoveAll();

	m_lpSNN = new CpuSNN(m_strID.c_str());
	
	m_lpSNN->setStepFeedback(this);

	//Go through each of the neuron group items and set them up
	int iCount = m_aryNeuronGroups.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeuronGroups[iIndex])
			m_aryNeuronGroups[iIndex]->SetCARLSimulation();

	//Then go through each of the connections and set them up
	iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySynapses[iIndex])
			m_arySynapses[iIndex]->SetCARLSimulation();

	//Now run through each of the connection generators and set them up.
	CsConnectionGenerator *lpGen = NULL;
	CStdPtrMap<std::string, CsConnectionGenerator>::iterator oPos;
	for(oPos=m_aryGenerators.begin(); oPos!=m_aryGenerators.end(); ++oPos)
	{
		lpGen = oPos->second;
		lpGen->SetCARLSimulation();
	}

	//m_iSimMode = CPU_MODE;

	//Initalize the network
	m_lpSNN->runNetwork(0, 0, m_iSimMode);
}

bool CsNeuralModule::stepUpdate(CpuSNN* s, int step)
{
	
	//If we have been told to pause then lets loop till we can continue
	while(m_bPauseThread || m_lpSim->Paused())
	{
		m_bThreadPaused = true;

		//If we have been told to stop then exit immediately
		if(m_bStopThread)
			return true;

		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
	}

	//Otherwise keep going.
	return false;
}

void CsNeuralModule::updateMonitors(CpuSNN* s, int step)
{
	//If stop threads has been set then just exit out of here.
	if(m_bStopThread)
		return;

	m_fltNeuralTime = (float) ((step+1)*CARLSIM_STEP_INCREMENT);
	if(!m_bWaitingForNeuralToCatchUp && m_lpSim->Time() < m_fltNeuralTime)
		WaitForPhysicsToCatchUp();
}

void CsNeuralModule::WaitForPhysicsToCatchUp()
{
	m_bWaitingForPhysicsToCatchUp = true;

	while(m_lpSim->Time() <= m_fltNeuralTime)
	{
		if(m_bStopThread || m_bWaitingForNeuralToCatchUp)
			return;

		if(m_bPauseThread)
			m_bThreadPaused = true;
	}

	m_bWaitingForPhysicsToCatchUp = false;

	//std::string strMessage = "P:: Neural: " + STR(m_fltNeuralTime) + ", Physics: " + STR(m_lpSim->Time()) + "\r\n";
	//OutputDebugString(strMessage.c_str());
	//std::cout << "Neural: " << m_fltNeuralTime << ", Physics: " << m_lpSim->Time() << "\r\n";
}


void CsNeuralModule::WaitForNeuralToCatchUp()
{
	m_bWaitingForNeuralToCatchUp = true;

	while(m_fltNeuralTime <= m_lpSim->Time())
	{
		if(m_bStopThread || m_bWaitingForPhysicsToCatchUp)
			return;
	}

	m_bWaitingForNeuralToCatchUp = false;

	//std::string strMessage = "N:: Neural: " + STR(m_fltNeuralTime) + ", Physics: " + STR(m_lpSim->Time()) + "\r\n";
	//OutputDebugString(strMessage.c_str());
	//std::cout << "Neural: " << m_fltNeuralTime << ", Physics: " << m_lpSim->Time() << "\r\n";
}

void CsNeuralModule::StepThread()
{
	if(m_lpSNN)
		m_lpSNN->runNetwork(100, 0, m_iSimMode);
}

void CsNeuralModule::CloseThread()
{
		m_bWaitingForPhysicsToCatchUp = false;
	m_bWaitingForNeuralToCatchUp = false;

}

/**
\brief	When the simulation is starting we need to configure our NN and start our processing thread.

\discussion When the simulation starts we need to re-run our configuration to setup the neural network in CARLsim and 
then start our processing loop. 

\author	dcofer
\date	10/1/2014
**/
void CsNeuralModule::SimStarting()
{
	NeuralModule::SimStarting();

	//Do not do this again if the thread is already running. For example, if we pause the sim and hit play again.
	if(!m_bThreadProcessing)
	{
		//Only bother running the simulation if there are some synpases in it.
		if(m_arySynapses.size() > 0)
		{
			SetCARLSimulation();
			StartThread();
		}
	}
}

void CsNeuralModule::SimStopping()
{
	NeuralModule::SimStopping();

	ShutdownThread();
}

void CsNeuralModule::ResetSimulation()
{
	NeuralModule::ResetSimulation();

	m_fltNeuralTime = 0;

	ShutdownThread();

	int iCount = m_aryNeuronGroups.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeuronGroups[iIndex])
			m_aryNeuronGroups[iIndex]->ResetSimulation();

	iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySynapses[iIndex])
			m_arySynapses[iIndex]->ResetSimulation();
}

void CsNeuralModule::Initialize()
{
	NeuralModule::Initialize();

	Organism *lpOrganism = dynamic_cast<Organism *>(m_lpStructure);
	if(!lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	int iCount = m_aryNeuronGroups.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeuronGroups[iIndex])
			m_aryNeuronGroups[iIndex]->Initialize();

	iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySynapses[iIndex])
			m_arySynapses[iIndex]->Initialize();
}

void CsNeuralModule::StepSimulation()
{
	NeuralModule::StepSimulation();

	if(m_bThreadProcessing && !m_bWaitingForPhysicsToCatchUp && m_fltNeuralTime < m_lpSim->Time())
		WaitForNeuralToCatchUp();
}

#pragma region DataAccesMethods

bool CsNeuralModule::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(NeuralModule::SetData(strDataType, strValue, false))
		return true;

	if(strType == "TIMESTEP")
	{
		TimeStep(atof(strValue.c_str()));
		return true;
	}

	if(strType == "SIMMODE")
	{
		SimMode(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsNeuralModule::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	NeuralModule::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("TimeStep", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SimMode", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

/**
\brief	Adds a neuron to the module. 

\author	dcofer
\date	3/29/2011

\param	strXml	The xml to use when loading the neuron. 
**/
void CsNeuralModule::AddNeuronGroup(std::string strXml, bool bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("NeuronGroup");

	CsNeuronGroup *lpNeuron = LoadNeuronGroup(oXml);
	if(!bDoNotInit)
		lpNeuron->Initialize();
}

/**
\brief	Removes the neuron with the specified ID.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID for the neuron. 
\param	bThrowError	true to throw error if neuron found. 
**/
void CsNeuralModule::RemoveNeuronGroup(std::string strID, bool bThrowError)
{
	int iPos = FindNeuronGroupListPos(strID, bThrowError);
	m_aryNeuronGroups.RemoveAt(iPos);
}

/**
\brief	Gets a pointer to the synapses array.

\author	dcofer
\date	3/29/2011

\return	Pointer to the synapses.
**/
CStdPtrArray<CsSynapseGroup> *CsNeuralModule::GetSynapses()
{return &m_arySynapses;}


void CsNeuralModule::AddSynapse(CsSynapseGroup *lpSynapse)
{
	if(!lpSynapse) 
		THROW_ERROR(Cs_Err_lSynapseToAddNull, Cs_Err_strSynapseToAddNull);
	m_arySynapses.Add(lpSynapse);
}

/**
\brief	Adds a synapse using an xml packet. 

\author	dcofer
\date	3/29/2011

\param	strXml	The xml of the synapse to add. 
**/
void CsNeuralModule::AddSynapse(std::string strXml, bool bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Synapse");

	CsSynapseGroup *lpSynapse = LoadSynapse(oXml);
	if(!bDoNotInit)
		lpSynapse->Initialize();
}

/**
\brief	Removes the synapse described by iIndex.

\author	dcofer
\date	3/29/2011

\param	iIndex	Zero-based index of the synapse in the array. 
**/
void CsNeuralModule::RemoveSynapse(int iIndex)
{
	if( iIndex<0 || iIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	m_arySynapses.RemoveAt(iIndex);
}

/**
\brief	Removes the synapse by the GUID ID.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID for the synapse to remove. 
\param	bThrowError	true to throw error if synaspe not found. 
**/
void CsNeuralModule::RemoveSynapse(std::string strID, bool bThrowError)
{
	int iPos = FindSynapseListPos(strID, bThrowError);
	m_arySynapses.RemoveAt(iPos);
}

/**
\brief	Gets a synapse by its index in the array.

\author	dcofer
\date	3/29/2011

\param	iIndex	Zero-based index of the synaspe to return. 

\return	null if it fails, else the synapse.
**/
CsSynapseGroup *CsNeuralModule::GetSynapse(int iIndex)
{
	if( iIndex<0 || iIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	return m_arySynapses[iIndex];
}

/**
\brief	Searches for a synapse with the specified ID and returns its position in the list.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID of the synapse to find. 
\param	bThrowError	true to throw error if no synapse is found. 

\return	The found synapse list position.
**/
int CsNeuralModule::FindSynapseListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySynapses[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Cs_Err_lSynapseNotFound, Cs_Err_strSynapseNotFound, "ID");

	return -1;
}

/**
\brief	Gets the total number of synapses.

\author	dcofer
\date	3/29/2011

\return	The total number of synapses.
**/
int CsNeuralModule::TotalSynapses()
{return m_arySynapses.GetSize();}

/**
\brief	Clears the synapses list.

\author	dcofer
\date	3/29/2011
**/
void CsNeuralModule::ClearSynapses()
{m_arySynapses.RemoveAll();}

bool CsNeuralModule::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "NEURONGROUP")
	{
		AddNeuronGroup(strXml, bDoNotInit);
		return true;
	}
	else if(strType == "SYNAPSE")
	{
		AddSynapse(strXml, bDoNotInit);
		return true;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool CsNeuralModule::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "NEURONGROUP")
	{
		RemoveNeuronGroup(strID, bThrowError);
		return true;
	}
	else if(strType == "SYNAPSE")
	{
		RemoveSynapse(strID, bThrowError);
		return true;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}


#pragma endregion

void CsNeuralModule::Load(CStdXml &oXml)
{
	VerifySystemPointers();

	CStdXml oNetXml;

	m_arySourceAdapters.RemoveAll();
	m_aryTargetAdapters.RemoveAll();

	oXml.IntoElem();  //Into NeuralModule Element

	LoadNetworkXml(oXml);

	oXml.OutOfElem(); //OutOf NeuralModule Element

	TRACE_DEBUG("Finished loading nervous system config file.");
}

/**
\brief	Loads the network configuration.

\author	dcofer
\date	3/29/2011

\param [in,out]	oXml	The xml to load. 
**/
void CsNeuralModule::LoadNetworkXml(CStdXml &oXml)
{
	short iNeuron, iTotalNeurons;
		
	m_aryNeuronGroups.RemoveAll();

	ID(oXml.GetChildString("ID", m_strID));
	Type(oXml.GetChildString("Type", m_strType));
	Name(oXml.GetChildString("Name", m_strName));
	SimMode(oXml.GetChildInt("SimMode", m_iSimMode));

	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);

	//*** Begin Loading Neurons groups. *****
	oXml.IntoChildElement("NeuronGroups");

	iTotalNeurons = oXml.NumberOfChildren();
	for(iNeuron=0; iNeuron<iTotalNeurons; iNeuron++)
	{
		oXml.FindChildByIndex(iNeuron);
		LoadNeuronGroup(oXml);
	}

	oXml.OutOfElem();
	//*** End Loading Neurons groups. *****

	//*** Begin Loading Synapses. *****
	if(oXml.FindChildElement("Synapses", false))
	{
		oXml.IntoElem();  //Into Synapses Element

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadSynapse(oXml);
		}

		oXml.OutOfElem();
	}
	//*** End Loading Synapses. *****

}

/**
\brief	Loads a neuron.

\author	dcofer
\date	3/29/2011

\param [in,out]	oXml	The xml to load for the neuron. 

\return	Pointer to the loaded neuron.
**/
CsNeuronGroup *CsNeuralModule::LoadNeuronGroup(CStdXml &oXml)
{
	CsNeuronGroup *lpNeuron=NULL;
	std::string strType;

try
{
	//Now lets get the index and type of this neuron
	oXml.IntoElem();  //Into Neuron Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Neuron Element

	lpNeuron = dynamic_cast<CsNeuronGroup *>(m_lpSim->CreateObject("AnimatCarlSimCUDA", "Neuron", strType));
	if(!lpNeuron)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Neuron");

	lpNeuron->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL, true);
	lpNeuron->Load(oXml);
	
	m_aryNeuronGroups.Add(lpNeuron);
	return lpNeuron;
}
catch(CStdErrorInfo oError)
{
	if(lpNeuron) delete lpNeuron;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpNeuron) delete lpNeuron;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Loads a synapse.

\author	dcofer
\date	3/29/2011

\param [in,out]	oXml	The xml to load. 

\return	Pointer to the created synapse.
**/
CsSynapseGroup *CsNeuralModule::LoadSynapse(CStdXml &oXml)
{
	std::string strType;
	CsSynapseGroup *lpSynapse=NULL;

try
{
	oXml.IntoElem();  //Into Synapse Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Synapse Element

	lpSynapse = dynamic_cast<CsSynapseGroup *>(m_lpSim->CreateObject("AnimatCarlSimCUDA", "Synapse", strType));
	if(!lpSynapse)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Synapse");

	lpSynapse->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL, true);
	lpSynapse->Load(oXml);
	AddSynapse(lpSynapse);

	return lpSynapse;
}
catch(CStdErrorInfo oError)
{
	if(lpSynapse) delete lpSynapse;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSynapse) delete lpSynapse;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

}				//AnimatCarlSim

