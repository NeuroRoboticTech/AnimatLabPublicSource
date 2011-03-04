// Brain.cpp: implementation of the FiringRateModule class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "FiringRateModule.h"
#include "ClassFactory.h"

namespace FiringRateSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

FiringRateModule::FiringRateModule()
{
	m_lpClassFactory =  new FiringRateSim::ClassFactory;
	m_bActiveArray = FALSE;
}

FiringRateModule::~FiringRateModule()
{

try
{
	m_aryNeurons.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of FiringRateModule\r\n", "", -1, FALSE, TRUE);}
}

BOOL FiringRateModule::ActiveArray()
{return m_bActiveArray;}

void FiringRateModule::ActiveArray(BOOL bVal)
{
	m_bActiveArray = bVal;
}

BOOL FiringRateModule::InactiveArray()
{return !m_bActiveArray;}

void FiringRateModule::InactiveArray(BOOL bVal)
{
	m_bActiveArray = !bVal;
}



void FiringRateModule::Kill(BOOL bState)
{
	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex])
			m_aryNeurons[iIndex]->Kill(bState);
}

int FiringRateModule::FindNeuronListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Nl_Err_lNeuronNotFound, Nl_Err_strNeuronNotFound, "ID");

	return -1;
}

void FiringRateModule::ResetSimulation()
{
	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex])
			m_aryNeurons[iIndex]->ResetSimulation();
}

void FiringRateModule::Initialize()
{
	NeuralModule::Initialize();

	Organism *lpOrganism = dynamic_cast<Organism *>(m_lpStructure);
	if(!lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex])
			m_aryNeurons[iIndex]->Initialize();
}

void FiringRateModule::StepSimulation()
{
	NeuralModule::StepSimulation();

	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex])
			m_aryNeurons[iIndex]->StepSimulation();

	//Swap the active array.
	m_bActiveArray = !m_bActiveArray;
}

#pragma region DataAccesMethods

BOOL FiringRateModule::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(NeuralModule::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "TIMESTEP")
	{
		TimeStep(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void FiringRateModule::AddNeuron(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Neuron");

	Neuron *lpNeuron = LoadNeuron(oXml);
	lpNeuron->Initialize();
}

void FiringRateModule::RemoveNeuron(string strID, BOOL bThrowError)
{
	int iPos = FindNeuronListPos(strID, bThrowError);
	m_aryNeurons.RemoveAt(iPos);
}

BOOL FiringRateModule::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "NEURON")
	{
		AddNeuron(strXml);
		return TRUE;
	}
	//Synapses are stored in the destination neuron. They will be added there.


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL FiringRateModule::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "NEURON")
	{
		RemoveNeuron(strID, bThrowError);
		return TRUE;
	}
	//Synapses are stored in the destination neuron. They will be removed there.


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

void FiringRateModule::GenerateAutoSeed()
{
	SYSTEMTIME st;
	GetLocalTime(&st);

	int iSeed = (unsigned) (st.wSecond + st.wMilliseconds + Std_IRand(0, 1000));
	Std_SRand(iSeed);
}

long FiringRateModule::CalculateSnapshotByteSize()
{
	long lByteSize = 0;
	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex])
			lByteSize += m_aryNeurons[iIndex]->CalculateSnapshotByteSize();

	return lByteSize;
}

void FiringRateModule::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex])
			m_aryNeurons[iIndex]->SaveKeyFrameSnapshot(aryBytes, lIndex);
}

void FiringRateModule::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex])
			m_aryNeurons[iIndex]->LoadKeyFrameSnapshot(aryBytes, lIndex);
}


//This gets the Nervous system configuration file and loads in the filename
//It then opens that file and loads it. 
void FiringRateModule::Load(CStdXml &oXml)
{
	VerifySystemPointers();

	CStdXml oNetXml;

	//if(Std_IsBlank(m_strProjectPath)) 
	//	THROW_ERROR(Al_Err_lProjectPathBlank, Al_Err_strProjectPathBlank);

	m_arySourceAdapters.RemoveAll();
	m_aryTargetAdapters.RemoveAll();

	oXml.IntoElem();  //Into NeuralModule Element

	m_strNeuralNetworkFile = oXml.GetChildString("NeuralNetFile", "");

	TRACE_DEBUG("Loading nervous system config file.\r\nProjectPath: " + m_strProjectPath + "\r\nFile: " + m_strNeuralNetworkFile);

	if(!Std_IsBlank(m_strNeuralNetworkFile)) 
	{
		oNetXml.Load(AnimatSim::GetFilePath(m_strProjectPath, m_strNeuralNetworkFile));

		oNetXml.FindElement("NeuralModule");
		oNetXml.FindChildElement("NetworkSize");

		LoadNetworkXml(oNetXml);
	}
	else
		LoadNetworkXml(oXml);

	oXml.OutOfElem(); //OutOf NeuralModule Element

	//GenerateAutoSeed();

	TRACE_DEBUG("Finished loading nervous system config file.");
}

void FiringRateModule::LoadNetworkXml(CStdXml &oXml)
{
	short iNeuron, iTotalNeurons;
		
	m_aryNeurons.RemoveAll();

	ID(oXml.GetChildString("ID", m_strID));
	Type(oXml.GetChildString("Type", m_strType));
	Name(oXml.GetChildString("Name", m_strName));
	TimeStep(oXml.GetChildFloat("TimeStep", m_fltTimeStep));

	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);

	//*** Begin Loading Neurons. *****
	oXml.IntoChildElement("Neurons");

	iTotalNeurons = oXml.NumberOfChildren();
	for(iNeuron=0; iNeuron<iTotalNeurons; iNeuron++)
	{
		oXml.FindChildByIndex(iNeuron);
		LoadNeuron(oXml);
	}

	oXml.OutOfElem();
	//*** End Loading Neurons. *****
}


Neuron *FiringRateModule::LoadNeuron(CStdXml &oXml)
{
	Neuron *lpNeuron=NULL;
	string strType;

try
{
	//Now lets get the index and type of this neuron
	oXml.IntoElem();  //Into Neuron Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Neuron Element

	lpNeuron = dynamic_cast<Neuron *>(m_lpSim->CreateObject(Nl_NeuralModuleName(), "Neuron", strType));
	if(!lpNeuron)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Neuron");

	lpNeuron->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL);
	lpNeuron->Load(oXml);
	
	m_aryNeurons.Add(lpNeuron);
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

}				//FiringRateSim

