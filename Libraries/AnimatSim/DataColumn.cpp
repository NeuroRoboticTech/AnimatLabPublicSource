// DataColumn.cpp: implementation of the DataColumn class.
//
//////////////////////////////////////////////////////////////////////


#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"


#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataColumn.h"
#include "DataChart.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "KeyFrame.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Charting
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

DataColumn::DataColumn()
{
	m_lpDataValue = NULL;
	m_iAppendSpaces = 0;
	m_bInitialized = FALSE;
	m_iIndex = -1;
	m_iColumnIndex = -1;
	m_iRowIndex = -1; 
}

DataColumn::~DataColumn()
{
	//Do not delete this pointer. It is just a reference.
	m_lpDataValue = NULL;
}

int DataColumn::ColumnCount()
{return 1;}

void DataColumn::DataType(string strType)
{
	m_strDataType = strType;
	Initialize();
}

void DataColumn::Initialize()
{
	AnimatBase::Initialize();

	m_lpTarget = m_lpSim->FindByID(m_strTargetID);
	m_lpDataValue = m_lpTarget->GetDataPointer(m_strDataType);

	if(!m_lpDataValue)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("DataColumn: " + m_strID + " TargetID: " + m_strTargetID +  " DataType: " + m_strDataType));

	m_bInitialized = TRUE;
}

void DataColumn::ReInitialize()
{
	if(!m_bInitialized)
		Initialize();
}

BOOL DataColumn::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "COLUMNINDEX")
	{
		Index(atoi(strValue.c_str()));
		return TRUE;
	}

	if(strType == "DATATYPE")
	{
		DataType(strValue);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void DataColumn::SaveColumnNames(ofstream &oStream)
{
	oStream << m_strColumnName;

	if(m_iAppendSpaces > 0)
	{
		for(int iIndex=0; iIndex<m_iAppendSpaces; iIndex++)
			oStream << " \t";
	}
}

void DataColumn::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, DataChart *lpChart)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode);
	m_lpChart = lpChart;
}
	
void DataColumn::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpChart)
		THROW_PARAM_ERROR(Al_Err_lChartNotDefined, Al_Err_strChartNotDefined, "DataColumn: ", m_strName);
}

void DataColumn::StepSimulation()
{
	m_lpChart->AddData(m_iColumnIndex, m_iRowIndex, *m_lpDataValue);
}

BOOL DataColumn::operator<(DataColumn *lpColumn)
{
	if(this->m_iIndex < lpColumn->m_iIndex)
		return TRUE;

	//if(this->m_strID < lpColumn->m_strID)
	//	return TRUE;

	return FALSE;
}

void DataColumn::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into DataColumn Element

	m_strTargetID = oXml.GetChildString("TargetID");
	if(Std_IsBlank(m_strTargetID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: TargetID");

	m_strDataType = oXml.GetChildString("DataType");
	if(Std_IsBlank(m_strDataType)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: DataType");

	m_strColumnName = oXml.GetChildString("ColumnName");
	if(Std_IsBlank(m_strColumnName)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: ColumnName");

	m_iAppendSpaces = oXml.GetChildInt("AppendSpaces", m_iAppendSpaces);
	Std_InValidRange((int) 0, (int) 10, m_iAppendSpaces, TRUE, "AppendSpaces");

	m_iIndex = oXml.GetChildInt("Index", m_iIndex);

	m_iColumnIndex = oXml.GetChildInt("Column", m_iColumnIndex);
	m_iRowIndex = oXml.GetChildInt("Row", m_iColumnIndex);

	oXml.OutOfElem(); //OutOf DataColumn Element

	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);
}


BOOL LessThanDataColumnCompare(DataColumn *lpColumn1, DataColumn *lpColumn2)
{
	return lpColumn1->operator<(lpColumn2);
}

	}			//Charting
}				//AnimatSim
