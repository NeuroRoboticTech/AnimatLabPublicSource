// IonChannel.cpp: implementation of the IonChannel class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "IonChannel.h"
#include "SynapseType.h"
#include "Connexion.h"
#include "CaActivation.h"
#include "Neuron.h"
#include "ElectricalSynapse.h"
#include "NonSpikingChemicalSynapse.h"
#include "SpikingChemicalSynapse.h"
#include "IntegrateFireModule.h"
#include "ClassFactory.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace IntegrateFireSim
{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

IonChannel::IonChannel()
{
	m_bEnabled = TRUE;
	m_fltGmax = 0;
	m_fltG = 0;
	m_fltMPower = 0;
	m_fltHPower = 0;
	m_fltEquilibriumPotential = 0;

	m_fltM = 0;
	m_fltNm = 1;
	m_lpMinf = NULL;
	m_lpTm = NULL;

	m_fltH = 0;
	m_fltNh = 1;
	m_lpHinf = NULL;
	m_lpTh = NULL;

	m_fltGInit = 0;
	m_fltMInit = 0;
	m_fltHInit = 0;

	m_fltTotalAct = 0;
	m_fltI = 0;
	m_fltMinf = 0;
	m_fltHinf = 0;
	m_fltTm = 0;
	m_fltTh = 0;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

IonChannel::~IonChannel()
{

try
{
	if(m_lpMinf) delete m_lpMinf;
	if(m_lpTm) delete m_lpTm;
	if(m_lpHinf) delete m_lpHinf;
	if(m_lpTh) delete m_lpTh;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of IonChannel\r\n", "", -1, FALSE, TRUE);}
}

void  IonChannel::Ginit(float fltVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	float fltDiff = fltVal - m_fltGInit;
	m_fltG += fltDiff;
	m_fltGInit = fltVal;
}; 

void  IonChannel::Hinit(float fltVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	float fltDiff = fltVal - m_fltHInit;
	m_fltH += fltDiff;
	m_fltHInit = fltVal;
}; 

void  IonChannel::Minit(float fltVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	float fltDiff = fltVal - m_fltMInit;
	m_fltM += fltDiff;
	m_fltMInit = fltVal;
}; 

void IonChannel::Initialize(Simulator *lpSim, Structure *lpStructure)
{
}

float IonChannel::CalculateCurrent(float fltStep, float fltVm)
{
	if(m_bEnabled)
	{
		if(m_fltMPower > 0)
		{
			m_fltMinf = m_lpMinf->CalculateGain(fltVm);
			m_fltTm = m_lpTm->CalculateGain(fltVm);
			m_fltM = m_fltM + fltStep*((m_fltMinf- m_fltM)/(m_fltNm*m_fltTm));
		}

		if(m_fltHPower > 0)
		{
			m_fltHinf = m_lpHinf->CalculateGain(fltVm);
			m_fltTh = m_lpTh->CalculateGain(fltVm);
			m_fltH = m_fltH + fltStep*((m_fltHinf- m_fltH)/(m_fltNh*m_fltTh));
		}

		m_fltTotalAct = pow(m_fltM, m_fltMPower)*pow(m_fltH, m_fltHPower);
		m_fltG = m_fltGmax*m_fltTotalAct;
		m_fltI = m_fltG*(m_fltEquilibriumPotential-fltVm);
	}
	else
		m_fltI = 0;

	return m_fltI;
}

#pragma region DataAccesMethods

float *IonChannel::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "G")
		return &m_fltG;

	if(strType == "M")
		return &m_fltM;

	if(strType == "H")
		return &m_fltH;

	if(strType == "I")
		return &m_fltI;

	if(strType == "ACT")
		return &m_fltTotalAct;

	if(strType == "TM")
		return &m_fltTm;

	if(strType == "TH")
		return &m_fltTh;

	if(strType == "MINF")
		return &m_fltMinf;

	if(strType == "HINF")
		return &m_fltHinf;

	//If it was not one of those above then we have a problem.
	THROW_PARAM_ERROR(Rn_Err_lInvalidNeuronDataType, Rn_Err_strInvalidNeuronDataType, "Ion Channel Data Type", strDataType);

	return NULL;
}

BOOL IonChannel::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
	
	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "GMAX")
	{
		Gmax(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "GINIT")
	{
		Ginit(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "MINIT")
	{
		Minit(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "HINIT")
	{
		Hinit(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "MPOWER")
	{
		MPower(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "HPOWER")
	{
		HPower(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "EQUILIBRIUMPOTENTIAL")
	{
		EquilibriumPotential(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "NM")
	{
		Nm(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "NH")
	{
		Nh(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}
#pragma endregion


void IonChannel::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into IonChannel Element

	Enabled(oXml.GetChildBool("Enabled", m_bEnabled));

	Gmax(oXml.GetChildFloat("Gmax"));
	MPower(oXml.GetChildFloat("MPower"));
	HPower(oXml.GetChildFloat("HPower"));
	EquilibriumPotential(oXml.GetChildFloat("EqPot"));

	Minit(oXml.GetChildFloat("Minit"));
	Nm(oXml.GetChildFloat("Nm"));

	//Load Minf
	oXml.IntoChildElement("Minf");
	string strModuleName = oXml.GetChildString("ModuleName", "");
	string strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpMinf = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpMinf)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpMinf->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL);
	m_lpMinf->Load(oXml);

	//Load Tm
	oXml.IntoChildElement("Tm");
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpTm = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpTm)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpTm->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL);
	m_lpTm->Load(oXml);

	Hinit(oXml.GetChildFloat("Hinit"));
	Nh(oXml.GetChildFloat("Nh"));

	//Load Hinf
	oXml.IntoChildElement("Hinf");
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpHinf = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpHinf)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpHinf->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL);
	m_lpHinf->Load(oXml);

	//Load Th
	oXml.IntoChildElement("Th");
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpTh = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpTh)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpTh->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL);
	m_lpTh->Load(oXml);

	oXml.OutOfElem(); //OutOf IonChannel Element
}

void IonChannel::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
}



}			//IntegrateFireSim
