/**
\file	IonChannel.cpp

\brief	Implements the ion channel class.
**/

#include "stdafx.h"
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


namespace IntegrateFireSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
IonChannel::IonChannel()
{
	m_bEnabled = true;
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

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
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
{Std_TraceMsg(0, "Caught Error in desctructor of IonChannel\r\n", "", -1, false, true);}
}


#pragma region Accessor-Mutators

/**
\brief	Enables the ion channel.

\author	dcofer
\date	3/31/2011

\param	bVal	true to enable. 
**/
void  IonChannel::Enabled(bool bVal) {m_bEnabled = bVal;}

/**
\brief	Gets whether this channel is enabled.

\author	dcofer
\date	3/31/2011

\return	true if enabled, false else.
**/
bool  IonChannel::Enabled() {return m_bEnabled;}

/**
\brief	Sets maximum conductance.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::Gmax(float fltVal) {m_fltGmax = fltVal;}

/**
\brief	Gets maximum conductance.

\author	dcofer
\date	3/31/2011

\return	maximum conductance.
**/
float  IonChannel::Gmax() {return m_fltGmax;}

/**
\brief	Sets initial conductance.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::Ginit(float fltVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	float fltDiff = fltVal - m_fltGInit;
	m_fltG += fltDiff;
	m_fltGInit = fltVal;
}; 

/**
\brief	Sets initial conductance.

\author	dcofer
\date	3/31/2011

\return	conductance.
**/
float  IonChannel::Ginit() {return m_fltGInit;}

/**
\brief	Sets Hinit.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::Hinit(float fltVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	float fltDiff = fltVal - m_fltHInit;
	m_fltH += fltDiff;
	m_fltHInit = fltVal;
}; 

/**
\brief	Gets Hinit.

\author	dcofer
\date	3/31/2011

\return	Hinit.
**/
float  IonChannel::Hinit() {return m_fltHInit;}

/**
\brief	Sets Minit.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::Minit(float fltVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	float fltDiff = fltVal - m_fltMInit;
	m_fltM += fltDiff;
	m_fltMInit = fltVal;
}; 

/**
\brief	Gets Minit.

\author	dcofer
\date	3/31/2011

\return	Minit.
**/
float  IonChannel::Minit() {return m_fltMInit;}

/**
\brief	Sets MPower.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::MPower(float fltVal) {m_fltMPower = fltVal;}

/**
\brief	Gets MPower.

\author	dcofer
\date	3/31/2011

\return	MPower.
**/
float  IonChannel::MPower() {return m_fltMPower;}

/**
\brief	Sets HPower.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::HPower(float fltVal) {m_fltHPower = fltVal;}

/**
\brief	Gets HPower.

\author	dcofer
\date	3/31/2011

\return	HPower.
**/
float  IonChannel::HPower() {return m_fltHPower;}

/**
\brief	Sets the equilibrium potential.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::EquilibriumPotential(float fltVal) {m_fltEquilibriumPotential = fltVal;}

/**
\brief	Gets the equilibrium potential.

\author	dcofer
\date	3/31/2011

\return	equilibrium potential.
**/
float  IonChannel::EquilibriumPotential() {return m_fltEquilibriumPotential;}

/**
\brief	Sets Nm.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::Nm(float fltVal) {m_fltNm = fltVal;}

/**
\brief	Gets Nm.

\author	dcofer
\date	3/31/2011

\return	Nm.
**/
float  IonChannel::Nm() {return m_fltNm;}

/**
\brief	Sets Nh.

\author	dcofer
\date	3/31/2011

\param	fltVal	The new value. 
**/
void  IonChannel::Nh(float fltVal) {m_fltNh = fltVal;}

/**
\brief	Gets Nh.

\author	dcofer
\date	3/31/2011

\return	Nh.
**/
float  IonChannel::Nh() {return m_fltNh;}

/**
\brief	Gets the minf.

\author	dcofer
\date	9/28/2011

\return	null if it fails, else.
**/
AnimatSim::Gains::Gain *IonChannel::Minf() {return m_lpMinf;}

/**
\brief	Sets the Minf equation.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void IonChannel::Minf(AnimatSim::Gains::Gain *lpGain)
{
	if(m_lpMinf)
	{
		if(m_lpMinf) 
			{delete m_lpMinf; m_lpMinf = NULL;}
		m_lpMinf = lpGain;
	}
}

/**
\brief	Sets the Minf gain using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void IonChannel::Minf(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");
	Minf(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
}


/**
\brief	Gets the Tm.

\author	dcofer
\date	9/28/2011

\return	null if it fails, else.
**/
AnimatSim::Gains::Gain *IonChannel::Tm() {return m_lpTm;}

/**
\brief	Sets the Tm equation.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void IonChannel::Tm(AnimatSim::Gains::Gain *lpGain)
{
	if(m_lpTm)
	{
		if(m_lpTm) 
			{delete m_lpTm; m_lpTm = NULL;}
		m_lpTm = lpGain;
	}
}

/**
\brief	Sets the Tm gain using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void IonChannel::Tm(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");
	Tm(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
}


/**
\brief	Gets the Hinf.

\author	dcofer
\date	9/28/2011

\return	null if it fails, else.
**/
AnimatSim::Gains::Gain *IonChannel::Hinf() {return m_lpHinf;}

/**
\brief	Sets the Hinf equation.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void IonChannel::Hinf(AnimatSim::Gains::Gain *lpGain)
{
	if(m_lpHinf)
	{
		if(m_lpHinf) 
			{delete m_lpHinf; m_lpHinf = NULL;}
		m_lpHinf = lpGain;
	}
}

/**
\brief	Sets the Hinf gain using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void IonChannel::Hinf(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");
	Hinf(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
}


/**
\brief	Gets the Th.

\author	dcofer
\date	9/28/2011

\return	null if it fails, else.
**/
AnimatSim::Gains::Gain *IonChannel::Th() {return m_lpTh;}

/**
\brief	Sets the Hinf equation.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void IonChannel::Th(AnimatSim::Gains::Gain *lpGain)
{
	if(m_lpTh)
	{
		if(m_lpTh) 
			{delete m_lpTh; m_lpTh = NULL;}
		m_lpTh = lpGain;
	}
}

/**
\brief	Sets the Th gain using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void IonChannel::Th(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");
	Th(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
}

#pragma endregion

/**
\brief	Calculates the current.

\author	dcofer
\date	3/31/2011

\param	fltStep	The time step. 
\param	fltVm  	The membrane votlage. 

\return	The calculated current.
**/
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


void IonChannel::ResetSimulation()
{
	m_fltMinf = 0;
	m_fltTm = 0;
	m_fltM = m_fltMInit;
	m_fltHinf = 0;
	m_fltTh = 0;
	m_fltH = m_fltHInit;
	m_fltI = 0;
	m_fltTotalAct = 0;
	m_fltG = 0;
}

#pragma region DataAccesMethods

float *IonChannel::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

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

bool IonChannel::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
			
	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}

	if(strType == "GMAX")
	{
		Gmax(atof(strValue.c_str()));
		return true;
	}

	if(strType == "GINIT")
	{
		Ginit(atof(strValue.c_str()));
		return true;
	}

	if(strType == "MINIT")
	{
		Minit(atof(strValue.c_str()));
		return true;
	}

	if(strType == "HINIT")
	{
		Hinit(atof(strValue.c_str()));
		return true;
	}

	if(strType == "MPOWER")
	{
		MPower(atof(strValue.c_str()));
		return true;
	}

	if(strType == "HPOWER")
	{
		HPower(atof(strValue.c_str()));
		return true;
	}

	if(strType == "EQUILIBRIUMPOTENTIAL")
	{
		EquilibriumPotential(atof(strValue.c_str()));
		return true;
	}

	if(strType == "NM")
	{
		Nm(atof(strValue.c_str()));
		return true;
	}

	if(strType == "NH")
	{
		Nh(atof(strValue.c_str()));
		return true;
	}

	if(strType == "MINF")
	{
		Minf(strValue);
		return true;
	}

	if(strType == "TM")
	{
		Tm(strValue);
		return true;
	}

	if(strType == "HINF")
	{
		Hinf(strValue);
		return true;
	}

	if(strType == "TH")
	{
		Th(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void IonChannel::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Enabled", AnimatPropertyType::Boolean, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("Gmax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Ginit", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Minit", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Hinit", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MPower", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("HPower", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("EquilibriumPotential", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Nm", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Nh", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Minf", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Tm", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Hinf", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Th", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
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
	std::string strModuleName = oXml.GetChildString("ModuleName", "");
	std::string strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpMinf = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpMinf)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpMinf->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL, true);
	m_lpMinf->Load(oXml);

	//Load Tm
	oXml.IntoChildElement("Tm");
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpTm = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpTm)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpTm->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL, true);
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

	m_lpHinf->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL, true);
	m_lpHinf->Load(oXml);

	//Load Th
	oXml.IntoChildElement("Th");
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpTh = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpTh)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpTh->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL, true);
	m_lpTh->Load(oXml);

	oXml.OutOfElem(); //OutOf IonChannel Element
}


}			//IntegrateFireSim
