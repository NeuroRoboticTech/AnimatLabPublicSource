// Brain.cpp: implementation of the [*PROJECT_NAME*]NeuralModule class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "[*PROJECT_NAME*]NeuralModule.h"
#include "ClassFactory.h"

namespace [*PROJECT_NAME*]
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

[*PROJECT_NAME*]NeuralModule::[*PROJECT_NAME*]NeuralModule()
{
	m_lpClassFactory =  new [*PROJECT_NAME*]::ClassFactory;
	m_bActiveArray = FALSE;
}

[*PROJECT_NAME*]NeuralModule::~[*PROJECT_NAME*]NeuralModule()
{

try
{
	m_aryNeurons.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of [*PROJECT_NAME*]NeuralModule\r\n", "", -1, FALSE, TRUE);}
}

BOOL [*PROJECT_NAME*]NeuralModule::ActiveArray()
{return m_bActiveArray;}

void [*PROJECT_NAME*]NeuralModule::ActiveArray(BOOL bVal)
{
	m_bActiveArray = bVal;
}

BOOL [*PROJECT_NAME*]NeuralModule::InactiveArray()
{return !m_bActiveArray;}

void [*PROJECT_NAME*]NeuralModule::InactiveArray(BOOL bVal)
{
	m_bActiveArray = !bVal;
}

Node *[*PROJECT_NAME*]NeuralModule::FindNode(long lNodeID)
{
	short iX = (short) ((float) lNodeID / (float) (m_oNetworkSize.y * m_oNetworkSize.z));
	short iY = (short) ((float) (lNodeID - (iX*(m_oNetworkSize.y * m_oNetworkSize.z))) / (float) m_oNetworkSize.z);
	short iZ = (short) (lNodeID - (iX*(m_oNetworkSize.y * m_oNetworkSize.z)) - (iY*m_oNetworkSize.z));

	return GetNeuron(iX, iY, iZ);
}

Neuron *[*PROJECT_NAME*]NeuralModule::GetNeuron(short iXPos, short iYPos, short iZPos)
{
	unsigned int iIndex;

	if( (iXPos<0) || (iYPos<0) || (iZPos<0) ) return NULL;
	if( (iXPos>=m_oNetworkSize.x) || (iYPos>=m_oNetworkSize.y) || (iZPos>=m_oNetworkSize.z) ) return NULL;

	iIndex = (iXPos*m_oNetworkSize.y*m_oNetworkSize.z) + (iYPos*m_oNetworkSize.z) + iZPos;
	return m_aryNeurons[iIndex];
}


void [*PROJECT_NAME*]NeuralModule::SetNeuron(unsigned char iXPos, 
                         unsigned char iYPos, 
                         unsigned char iZPos, 
                         Neuron *lpNeuron)
{
	unsigned int iIndex;

	if( (iXPos>=m_oNetworkSize.x) || (iYPos>=m_oNetworkSize.y) || (iZPos>=m_oNetworkSize.z) )
		THROW_TEXT_ERROR(Nl_Err_lInvalidSizeSpec, Nl_Err_strInvalidSizeSpec, 
		                 " Neuron position (" + STR(iXPos) + ", " + STR(iYPos) + ", " + STR(iZPos) + ") Max Size (" +
							 			 STR(m_oNetworkSize.x) + ", " + STR(m_oNetworkSize.y) + ", " + STR(m_oNetworkSize.z) + ")");

	if(!lpNeuron)
		THROW_ERROR(Nl_Err_lNeuronToSetNull, Nl_Err_strNeuronToSetNull);
	
	iIndex = (iXPos*m_oNetworkSize.y*m_oNetworkSize.z) + (iYPos*m_oNetworkSize.z) + iZPos;
	m_aryNeurons[iIndex] = lpNeuron;
}

void [*PROJECT_NAME*]NeuralModule::Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState)
{
	unsigned char iXPos, iYPos, iZPos;
	Neuron *lpNeuron=NULL;

	for(iZPos=0; iZPos<m_oNetworkSize.z; iZPos++)
		for(iXPos=0; iXPos<m_oNetworkSize.x; iXPos++)
			for(iYPos=0; iYPos<m_oNetworkSize.y; iYPos++)
			{
				lpNeuron = GetNeuron(iXPos, iYPos, iZPos);

				if(lpNeuron) lpNeuron->Kill(lpSim, lpOrganism, bState);
			}
}

void [*PROJECT_NAME*]NeuralModule::Reset(Simulator *lpSim, Organism *lpOrganism)
{
	unsigned char iXPos, iYPos, iZPos;
	Neuron *lpNeuron=NULL;

	for(iZPos=0; iZPos<m_oNetworkSize.z; iZPos++)
		for(iXPos=0; iXPos<m_oNetworkSize.x; iXPos++)
			for(iYPos=0; iYPos<m_oNetworkSize.y; iYPos++)
			{
				lpNeuron = GetNeuron(iXPos, iYPos, iZPos);

				if(lpNeuron) lpNeuron->Reset(lpSim, lpOrganism);
			}
}

void [*PROJECT_NAME*]NeuralModule::Initialize(Simulator *lpSim, Structure *lpStructure)
{
	unsigned char iXPos, iYPos, iZPos;
	Neuron *lpNeuron=NULL;

	NeuralModule::Initialize(lpSim, lpStructure);

	Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
	if(!lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	for(iZPos=0; iZPos<m_oNetworkSize.z; iZPos++)
		for(iXPos=0; iXPos<m_oNetworkSize.x; iXPos++)
			for(iYPos=0; iYPos<m_oNetworkSize.y; iYPos++)
			{
				lpNeuron = GetNeuron(iXPos, iYPos, iZPos);

				if(lpNeuron) lpNeuron->Initialize(lpSim, lpOrganism, this);
			}
}

void [*PROJECT_NAME*]NeuralModule::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	unsigned char iXPos, iYPos, iZPos;
	Neuron *lpNeuron=NULL;
	
	NeuralModule::StepSimulation(lpSim, lpStructure);

	Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
	if(!lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	for(iZPos=0; iZPos<m_oNetworkSize.z; iZPos++)
		for(iXPos=0; iXPos<m_oNetworkSize.x; iXPos++)
			for(iYPos=0; iYPos<m_oNetworkSize.y; iYPos++)
			{
				//This is used for debug purposes only.
				//if(iXPos==4 && iYPos==2 && iZPos==1)
				//	iXPos=4;

				lpNeuron = GetNeuron(iXPos, iYPos, iZPos);
				if(lpNeuron) 
					lpNeuron->StepSimulation(lpSim, lpOrganism, this, iXPos, iYPos, iZPos);
			}

	//Swap the active array.
	m_bActiveArray = !m_bActiveArray;
}

void [*PROJECT_NAME*]NeuralModule::GenerateAutoSeed()
{
	SYSTEMTIME st;
	GetLocalTime(&st);

	int iSeed = (unsigned) (st.wSecond + st.wMilliseconds + Std_IRand(0, 1000));
	Std_SRand(iSeed);
}

long [*PROJECT_NAME*]NeuralModule::CalculateSnapshotByteSize()
{
	unsigned char iXPos, iYPos, iZPos;
	Neuron *lpNeuron=NULL;
	long lByteSize = 0;

	for(iZPos=0; iZPos<m_oNetworkSize.z; iZPos++)
		for(iXPos=0; iXPos<m_oNetworkSize.x; iXPos++)
			for(iYPos=0; iYPos<m_oNetworkSize.y; iYPos++)
			{
				lpNeuron = GetNeuron(iXPos, iYPos, iZPos);
				if(lpNeuron) 
					lByteSize += lpNeuron->CalculateSnapshotByteSize();
			}

	return lByteSize;
}

void [*PROJECT_NAME*]NeuralModule::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	unsigned char iXPos, iYPos, iZPos;
	Neuron *lpNeuron=NULL;

	for(iZPos=0; iZPos<m_oNetworkSize.z; iZPos++)
		for(iXPos=0; iXPos<m_oNetworkSize.x; iXPos++)
			for(iYPos=0; iYPos<m_oNetworkSize.y; iYPos++)
			{
				lpNeuron = GetNeuron(iXPos, iYPos, iZPos);
				if(lpNeuron) 
					lpNeuron->SaveKeyFrameSnapshot(aryBytes, lIndex);
			}
}

void [*PROJECT_NAME*]NeuralModule::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	unsigned char iXPos, iYPos, iZPos;
	Neuron *lpNeuron=NULL;

	for(iZPos=0; iZPos<m_oNetworkSize.z; iZPos++)
		for(iXPos=0; iXPos<m_oNetworkSize.x; iXPos++)
			for(iYPos=0; iYPos<m_oNetworkSize.y; iYPos++)
			{
				lpNeuron = GetNeuron(iXPos, iYPos, iZPos);
				if(lpNeuron) 
					lpNeuron->LoadKeyFrameSnapshot(aryBytes, lIndex);
			}
}


//This gets the Nervous system configuration file and loads in the filename
//It then opens that file and loads it. 
void [*PROJECT_NAME*]NeuralModule::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	CStdXml oNetXml;

	if(!lpSim)
		THROW_ERROR(Nl_Err_lSimulationNotDefined, Nl_Err_strSimulationNotDefined);

	//if(Std_IsBlank(m_strProjectPath)) 
	//	THROW_ERROR(Al_Err_lProjectPathBlank, Al_Err_strProjectPathBlank);

	m_arySourceAdapters.RemoveAll();
	m_aryTargetAdapters.RemoveAll();

	oXml.IntoElem();  //Into NeuralModule Element

	m_strNeuralNetworkFile = oXml.GetChildString("NeuralNetFile", "");

	TRACE_DEBUG("Loading nervous system config file.\r\nProjectPath: " + m_strProjectPath + "\r\nFile: " + m_strNeuralNetworkFile);

	if(!Std_IsBlank(m_strNeuralNetworkFile)) 
	{
		oNetXml.Load(AnimatLibrary::GetFilePath(m_strProjectPath, m_strNeuralNetworkFile));

		oNetXml.FindElement("NeuralModule");
		oNetXml.FindChildElement("NetworkSize");

		LoadNetworkXml(lpSim, lpStructure, oNetXml);
	}
	else
		LoadNetworkXml(lpSim, lpStructure, oXml);

	oXml.OutOfElem(); //OutOf NeuralModule Element

	//GenerateAutoSeed();

	TRACE_DEBUG("Finished loading nervous system config file.");
}

void [*PROJECT_NAME*]NeuralModule::LoadNetworkXml(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	short iNeuron, iTotalNeurons;
	long lTotalCells;
		
	m_aryNeurons.RemoveAll();

	m_fltTimeStep = oXml.GetChildFloat("TimeStep", m_fltTimeStep);
	Std_IsAboveMin((float) 0, (float) m_fltTimeStep, TRUE, "TimeStep");

	Std_LoadPoint(oXml, "NetworkSize", m_oNetworkSize);

	lTotalCells = m_oNetworkSize.x * m_oNetworkSize.y * m_oNetworkSize.z;
	m_aryNeurons.SetSize(lTotalCells);
	for(iNeuron=0; iNeuron<lTotalCells; iNeuron++)
		m_aryNeurons[iNeuron] = NULL;

	//*** Begin Loading Neurons. *****
	oXml.IntoChildElement("Neurons");

	iTotalNeurons = oXml.NumberOfChildren();
	for(iNeuron=0; iNeuron<iTotalNeurons; iNeuron++)
	{
		oXml.FindChildByIndex(iNeuron);
		LoadNeuron(lpSim, lpStructure, oXml);
	}

	oXml.OutOfElem();
	//*** End Loading Neurons. *****
}


Neuron *[*PROJECT_NAME*]NeuralModule::LoadNeuron(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Neuron *lpNeuron=NULL;
	string strType;
	CStdIPoint oPos;

try
{
	//Now lets get the index and type of this neuron
	oXml.IntoElem();  //Into Neuron Element
	Std_LoadPoint(oXml, "Position", oPos);
	string strModuleName = oXml.GetChildString("ModuleName", "[*PROJECT_NAME*]");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Neuron Element

	lpNeuron = dynamic_cast<Neuron *>(lpSim->CreateObject(strModuleName, "Neuron", strType));
	if(!lpNeuron)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Neuron");

	lpNeuron->Load(lpSim, lpStructure, oXml);

	SetNeuron(oPos.x, oPos.y, oPos.z, lpNeuron);
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


void [*PROJECT_NAME*]NeuralModule::Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Neuron *lpNeuron=NULL;
	short iX, iY, iZ;

	if(!lpSim)
		THROW_ERROR(Nl_Err_lSimulationNotDefined, Nl_Err_strSimulationNotDefined);


	oXml.AddChildElement("Brain");
	oXml.IntoElem();  //Into Brain Element

	Std_SavePoint(oXml, "NetworkSize", m_oNetworkSize);

	//*** Begin Saving Neurons. *****
	oXml.AddChildElement("Neurons");
	oXml.IntoChildElement("Neurons");

	for(iZ=0; iZ<m_oNetworkSize.z; iZ++)
		for(iX=0; iX<m_oNetworkSize.x; iX++)
			for(iY=0; iY<m_oNetworkSize.y; iY++)
			{
				lpNeuron = GetNeuron(iX, iY, iZ);
				if(lpNeuron)
				{
					oXml.AddChildElement("Neuron");
					oXml.IntoElem();  //Into Neuron Element

					oXml.AddChildElement("x", iX);
					oXml.AddChildElement("y", iY);
					oXml.AddChildElement("z", iZ);

					lpNeuron->Save(lpSim, lpStructure, oXml);

					oXml.OutOfElem();  //OutOf Neuron Element
				}
			}


	oXml.OutOfElem();
	//*** End Saving Neurons. *****


	oXml.OutOfElem();  //OutOf Brain Element
}

}				//[*PROJECT_NAME*]

