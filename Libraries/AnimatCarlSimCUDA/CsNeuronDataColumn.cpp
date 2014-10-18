/**
\file	DataColumn.cpp

\brief	Implements the data column class.
**/

#include "StdAfx.h"

#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsSpikeGeneratorGroup.h"
#include "CsNeuralModule.h"
#include "CsNeuronDataColumn.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	10/12/2014
**/
CsNeuronDataColumn::CsNeuronDataColumn()
{
	m_bAddDataAtEnd = false;
	m_lpNeuron = NULL;
	m_iNeuronID = -1;
}

/**
\brief	Destructor.

\author	dcofer
\date	10/12/2014
**/
CsNeuronDataColumn::~CsNeuronDataColumn()
{
}

void CsNeuronDataColumn::NeuronID(const std::string &strValue)
{
	if(!Std_IsNumeric(strValue))
		THROW_PARAM_ERROR(Cs_Err_lInvalidNeuralIndex, Cs_Err_strInvalidNeuralIndex, "NeuronID is not an integer", strValue);

	m_strNeuronID = strValue;

	if(m_lpNeuron)
	{
		int iId = atoi(strValue.c_str());

		if(iId < 0 || iId >=  m_lpNeuron->NeuronCount())
			THROW_PARAM_ERROR(Cs_Err_lInvalidNeuralIndex, Cs_Err_strInvalidNeuralIndex, "NeuronID is out of range", strValue);

		if(Std_CheckString(m_strDataType) == "SPIKE" && m_iNeuronID >= 0)
			m_lpNeuron->DecrementCollectSpikeDataForNeuron(m_iNeuronID);

		m_iNeuronID = iId;

		if(Std_CheckString(m_strDataType) == "SPIKE" && m_iNeuronID >= 0)
			m_lpNeuron->IncrementCollectSpikeDataForNeuron(m_iNeuronID);
	}
}

unsigned int CsNeuronDataColumn::NeuronID() {return 0;}

void CsNeuronDataColumn::DataType(std::string strType)
{
	std::string strOldDataType = Std_CheckString(m_strDataType);

	DataColumn::DataType(strType);

	std::string strData = Std_CheckString(strType);

	m_bAddDataAtEnd = false;
	if(strData == "SPIKE")
		m_bAddDataAtEnd = true;

	std::string strNewDataType = Std_CheckString(m_strDataType);

	if(strOldDataType == "SPIKE" && strNewDataType != "SPIKE" && m_iNeuronID >= 0)
		m_lpNeuron->DecrementCollectSpikeDataForNeuron(m_iNeuronID);
	else if(strNewDataType == "SPIKE" && strOldDataType != "SPIKE" && m_iNeuronID >= 0)
		m_lpNeuron->IncrementCollectSpikeDataForNeuron(m_iNeuronID);
}

#pragma region DataAccesMethods

bool CsNeuronDataColumn::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(DataColumn::SetData(strDataType, strValue, false))
		return true;

	if(strType == "SUBDATA")
	{
		NeuronID(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsNeuronDataColumn::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	DataColumn::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("SubData", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

#pragma endregion

void CsNeuronDataColumn::Initialize()
{
	DataColumn::Initialize();

	//Lets check this target and data type to see if it should be updated at the end or not.
	if(m_lpTarget)
	{
		m_lpNeuron = dynamic_cast<CsNeuronGroup *>(m_lpTarget);

		if(!m_lpNeuron)
			THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Neuron: ", m_lpTarget->ID());

		if(m_strNeuronID.length() > 0)
			NeuronID(m_strNeuronID);
	}
}

void CsNeuronDataColumn::Deactivate()
{
	//Only try to set the data if this chart has been set to collect its
	//data at the end of the run.
	if(m_bAddDataAtEnd && m_lpChart && m_lpChart->SetStartEndTime())
		FillInDeactivateData();
}

void CsNeuronDataColumn::FillInDeactivateData()
{
	if(m_bAddDataAtEnd && m_lpNeuron && m_lpChart && m_iNeuronID >= 0)
	{
		float fltTimeBase = m_lpChart->ChartTimeBase();
		long lColumnCount = m_lpChart->ColumnCount();
		long lRowCount = m_lpChart->RowCount();

		if(fltTimeBase > 0)
		{
			std::pair<std::multimap<int, unsigned long>::iterator, std::multimap<int, unsigned long>::iterator> arySpikeTimes = m_lpNeuron->SpikeTimes()->equal_range(m_iNeuronID);

			for (std::multimap<int, unsigned long>::iterator it = arySpikeTimes.first; it != arySpikeTimes.second; ++it)
			{
				int iTime = (int) (*it).second;
				float fltTime = iTime*CARLSIM_STEP_INCREMENT;

				int iRow = (int) ((fltTime/fltTimeBase)+0.5);

				if( (m_iColumnIndex>=0) && (m_iColumnIndex<lColumnCount) && (iRow>=0) && (iRow<lRowCount))
					m_lpChart->SetData(m_iColumnIndex, iRow, 1.0);
			}
		}
	}
}

void CsNeuronDataColumn::Load(CStdXml &oXml)
{
	DataColumn::Load(oXml);

	oXml.IntoElem();

	if(oXml.FindChildElement("SubData", false))
		NeuronID(oXml.GetChildString("SubData"));

	oXml.OutOfElem();
}

}				//AnimatSim
