/**
\file	Synapse.cpp

\brief	Implements the synapse class.
**/

#include "StdAfx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Synapses
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
Synapse::Synapse()
{
	m_lpFRModule = NULL;

	m_lpFromNeuron = NULL;
	m_lpToNeuron = NULL;
	m_fltWeight=0;
	m_fltModulation=0;
	m_strType = "REGULAR";
	m_bHasDelay = false;
	m_fltDelayInterval = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
Synapse::~Synapse()
{

try
{
	m_arySynapses.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Synapse\r\n", "", -1, false, true);}
}

/**
\brief	Gets the synaptic weight.

\author	dcofer
\date	3/29/2011

\return	synaptic weight.
**/
float Synapse::Weight()
{return m_fltWeight;}

/**
\brief	Sets the synaptic weight.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Synapse::Weight(float fltVal)
{m_fltWeight=fltVal;}

/**
\brief	Gets whether this synapse has a delay associated with it.

\author	dcofer
\date	6/11/2014

\return	True if there is a synaptic delay.
**/
bool Synapse::HasDelay() {return m_bHasDelay;}

/**
\brief	Sets whether this synapse has a delay associated with it.

\author	dcofer
\date	6/11/2014

\param	fltVal	The new value. 
**/
void Synapse::HasDelay(bool bVal) 
{
	m_bHasDelay = bVal;
	SetDelayBufferSize();
}

/**
\brief	Gets the delay buffer size in time.

\author	dcofer
\date	6/11/2014

\return	delay buffer interval.
**/
float Synapse::DelayInterval() {return m_fltDelayInterval;}

/**
\brief	Sets the delay buffer interval.

\author	dcofer
\date	6/11/2014

\param	fltVal	The new value. 
**/
void Synapse::DelayInterval(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "DelayInterval", true);
	m_fltDelayInterval = fltVal;
	SetDelayBufferSize();
}

/**
\brief	If the time step is modified then we need to recalculate the length of the delay buffer.

\discussion If a neural module has been assigned to this adapter then that is its target module and
we need to use the time step associated with it to determine how big the delay buffer should be in length.
If the module is NULL then the target for this adapter is the physics engine and we should use the physics time step instead.

\author	dcofer
\date	5/15/2014
**/
void Synapse::TimeStepModified()
{
	SetDelayBufferSize();
}

/**
\brief	Recalculates the size of the delay buffer required.

\author	dcofer
\date	6/11/2014

**/
void Synapse::SetDelayBufferSize()
{
	if(m_bHasDelay && m_lpFRModule)
	{
		float fltTimeStep = m_lpFRModule->TimeStep();

		if(fltTimeStep > 0)
		{
			int iLength = (int) (m_fltDelayInterval/fltTimeStep);
			m_aryDelayBuffer.SetSize(iLength);
		}
	}
	else
		m_aryDelayBuffer.RemoveAll();
}

/**
\brief	Processes this synapse.

\author	dcofer
\date	6/17/2014

\return	Synaptic current.
**/
void Synapse::Process(float &fltCurrent)
{
	fltCurrent+=CalculateCurrent(); 
}

/**
\brief	Calculates the synaptic current for this synapse.

\author	dcofer
\date	6/11/2014

\return	Synaptic current.
**/
float Synapse::CalculateCurrent()
{
	//Test code
	//int i=5;
	//if(Std_ToLower(m_strID) == "3ce79124-1a4b-4a34-8399-56dfe1814515")
	//	i=6;

	float fltI = (this->FromNeuron()->FiringFreq(m_lpFRModule) * this->Weight() * this->CalculateModulation(m_lpFRModule) );

	if(m_bHasDelay)
	{
		float fltNextI = m_aryDelayBuffer.GetHead();
		//Now set the current value into the buffer.
		m_aryDelayBuffer.AddEnd(fltI);
		return fltNextI;
	}
	else
		return fltI;
}

/**
\brief	Gets a pointer to the synaptic weight.

\details This is so that other items can alter the synaptic weight.

\author	dcofer
\date	3/29/2011

\return	Pointer to the weight.
**/
float *Synapse::WeightPointer()
{return &m_fltWeight;}

/**
\brief	Gets the synaptic modulation.

\author	dcofer
\date	3/29/2011

\return	synaptic modulation.
**/
float Synapse::Modulation()
{return m_fltModulation;}

/**
\brief	Gets a pointer to the synaptic modulation.

\details This is so that other items can alter the synaptic modulation.

\author	dcofer
\date	3/29/2011

\return	pointer to the synaptic modulation.
**/
float *Synapse::ModulationPointer()
{return &m_fltModulation;}

/**
\brief	Gets a pointer to a compound synapse.

\author	dcofer
\date	3/29/2011

\param	iCompoundIndex	Zero-based index of the synapse. 

\return	Pointer to the compound synapse.
**/
Synapse *Synapse::GetCompoundSynapse(short iCompoundIndex)
{

	if( iCompoundIndex<0 || iCompoundIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	return m_arySynapses[iCompoundIndex];
}

/**
\brief	Adds a compound synapse to this one. 

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet to load. 
**/
void Synapse::AddSynapse(std::string strXml, bool bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("CompoundSynapse");

	Synapse *lpSynapse = LoadSynapse(oXml);

	if(!bDoNotInit)
		lpSynapse->Initialize();
}

/**
\brief	Removes the copmound synapse specified by ID.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID for the synaspe to remove. 
\param	bThrowError	true to throw error if synapse is not found. 
**/
void Synapse::RemoveSynapse(std::string strID, bool bThrowError)
{
	int iPos = FindSynapseListPos(strID, bThrowError);
	m_arySynapses.RemoveAt(iPos);
}

/**
\brief	Searches for the first synapse list position.

\author	dcofer
\date	3/29/2011

\param	strID	   	Identifier for the std::string. 
\param	bThrowError	true to throw error. 

\return	The found synapse list position.
**/
int Synapse::FindSynapseListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySynapses[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Nl_Err_lSynapseNotFound, Nl_Err_strSynapseNotFound, "ID");

	return -1;
}

/**
\brief	Calculates the synaptic modulation.

\author	dcofer
\date	3/29/2011

\param [in,out]	m_lpFRModule	Pointer to a fast module. 

\return	The calculated modulation.
**/
float Synapse::CalculateModulation(FiringRateModule *m_lpFRModule)
{
	m_fltModulation=1;
	int iCount, iIndex;
	Synapse *lpSynapse=NULL;

	iCount = m_arySynapses.GetSize();
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpSynapse = m_arySynapses[iIndex];
		m_fltModulation*=lpSynapse->CalculateModulation(m_lpFRModule);
	}

	return m_fltModulation;
}


void Synapse::Initialize()
{
	Link::Initialize();

	m_lpFromNeuron = dynamic_cast<Neuron *>(m_lpSim->FindByID(m_strFromID));
	if(!m_lpFromNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strFromID);

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->Initialize();

	SetDelayBufferSize();
}

void Synapse::SetSystemPointers(Simulator *m_lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Link::SetSystemPointers(m_lpSim, lpStructure, lpModule, lpNode, false);

	m_lpFRModule = dynamic_cast<FiringRateModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void Synapse::VerifySystemPointers()
{
	Link::VerifySystemPointers();

	if(!m_lpFRModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpFRModule->ID());

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

#pragma region DataAccesMethods

float *Synapse::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "WEIGHT")
		return &m_fltWeight;
	else if(strType == "MODULATION")
		return &m_fltModulation;

	return NULL;
}

bool Synapse::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(Link::SetData(strDataType, strValue, false))
		return true;

	if(strType == "WEIGHT")
	{
		Weight(atof(strValue.c_str()));
		return true;
	}

	if(strType == "HASDELAY")
	{
		HasDelay(Std_ToBool(strValue));
		return true;
	}

	if(strType == "DELAYINTERVAL")
	{
		DelayInterval(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Synapse::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Link::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Modulation", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Weight", AnimatPropertyType::Float, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("HasDelay", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("DelayInterval", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

bool Synapse::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "SYNAPSE")
	{
		AddSynapse(strXml, bDoNotInit);
		return true;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool Synapse::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "SYNAPSE")
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

void Synapse::ResetSimulation()
{
	int iSize = m_aryDelayBuffer.GetSize();
	for(int iIdx=0; iIdx<iSize; iIdx++)
		m_aryDelayBuffer[iIdx] = 0;

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->ResetSimulation();
}

void Synapse::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_lpToNeuron = dynamic_cast<Neuron *>(m_lpNode);
	if(!m_lpToNeuron)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNodeToDesiredType, Al_Err_strUnableToCastNodeToDesiredType, "ID: ", m_lpNode->ID());

	oXml.IntoElem();  //Into Synapse Element

	m_strFromID = oXml.GetChildString("FromID");
	if(Std_IsBlank(m_strFromID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: FromID");

	m_bEnabled = oXml.GetChildBool("Enabled", true);
	m_fltWeight = oXml.GetChildFloat("Weight");

	m_bHasDelay = oXml.GetChildBool("HasDelay", m_bHasDelay);
	m_fltDelayInterval = oXml.GetChildFloat("DelayInterval", m_fltDelayInterval);

	m_arySynapses.RemoveAll();

	//*** Begin Loading CompoundSynapses. *****
	if(oXml.FindChildElement("CompoundSynapses", false))
	{
		oXml.IntoChildElement("CompoundSynapses");

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadSynapse(oXml);
		}

		oXml.OutOfElem(); //OutOf CompoundSynapses Element
	}


	oXml.OutOfElem(); //OutOf Synapse Element
}

/**
\brief	Loads a synapse.

\author	dcofer
\date	3/29/2011

\param [in,out]	oXml	The xml packet to load. 

\return	Pointer to the loaded synapse.
**/
Synapse *Synapse::LoadSynapse(CStdXml &oXml)
{
	Synapse *lpSynapse=NULL;
	std::string strType;

try
{
	oXml.IntoElem();  //Into Synapse Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Synapse Element

	lpSynapse = dynamic_cast<Synapse *>(m_lpSim->CreateObject(Nl_NeuralModuleName(), "Synapse", strType));
	if(!lpSynapse)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Synapse");

	lpSynapse->SetSystemPointers(m_lpSim, m_lpStructure, m_lpFRModule, m_lpNode, true);
	lpSynapse->Load(oXml);
	m_arySynapses.Add(lpSynapse);

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

	}			//Synapses
}				//FiringRateSim






