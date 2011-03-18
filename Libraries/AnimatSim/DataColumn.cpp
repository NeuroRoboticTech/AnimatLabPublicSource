/**
\file	DataColumn.cpp

\brief	Implements the data column class.
**/

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
/**
\brief	Default constructor.

\author	dcofer
\date	3/18/2011
**/
DataColumn::DataColumn()
{
	m_lpDataValue = NULL;
	m_iAppendSpaces = 0;
	m_bInitialized = FALSE;
	m_iIndex = -1;
	m_iColumnIndex = -1;
	m_iRowIndex = -1; 
}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
DataColumn::~DataColumn()
{
	//Do not delete this pointer. It is just a reference.
	m_lpDataValue = NULL;
}

/**
\brief	Gets the number of columns for this DataColumn.

\author	dcofer
\date	3/18/2011

\return	Number of columns in the data buffer.
**/
int DataColumn::ColumnCount()
{return 1;}

/**
\brief	Gets the column name.

\author	dcofer
\date	3/18/2011

\return	Column name.
**/
string DataColumn::ColumnName() {return m_strColumnName;}

/**
\brief	Sets the Column name.

\author	dcofer
\date	3/18/2011

\param	strName	Name of the column. 
**/
void DataColumn::ColumnName(string strName) {m_strColumnName = strName;}

/**
\brief	Gets the show index. This determines the order in which the columns are saved out to the file.

\author	dcofer
\date	3/18/2011

\return	.
**/
int DataColumn::Index() {return m_iIndex;}

/**
\brief	Sets the show index. This determines the order in which the columns are saved out to the file.

\author	dcofer
\date	3/18/2011

\param	iIndex	Zero-based index of where to show this column in the chart. 
**/
void DataColumn::Index(int iIndex) {m_iIndex = iIndex;}

/**
\brief	Gets the data type of the variable we are collecting. This is the value passed into GetDataPointer.

\author	dcofer
\date	3/18/2011

\return	name of the data type to collect.
**/
string DataColumn::DataType() {return m_strDataType;}

/**
\brief	Sets the Data type of the variable we are collecting. This is the value passed into GetDataPointer.

\author	dcofer
\date	3/18/2011

\param	strType	Data Type of the data to collect. 
**/
void DataColumn::DataType(string strType)
{
	m_strDataType = strType;
	Initialize();
}

/**
\brief	Query if this object is initialized.

\author	dcofer
\date	3/18/2011

\return	true if initialized, false if not.
**/
BOOL DataColumn::IsInitialized() {return m_bInitialized;}

/**
\brief	Sets whether this column is initialized.

\author	dcofer
\date	3/18/2011

\param	bVal	true to set that it is initialized. 
**/
void DataColumn::IsInitialized(BOOL bVal) {m_bInitialized = bVal;}

/**
\brief	Gets the pointer to the data value we are collecting.

\author	dcofer
\date	3/18/2011

\return	Pointer.
**/
float *DataColumn::DataValue() {return m_lpDataValue;};

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

/**
\brief	Saves this DataColumn name to the out stream for the file.

\author	dcofer
\date	3/18/2011

\param [in,out]	oStream	The file stream. 
**/
void DataColumn::SaveColumnNames(ofstream &oStream)
{
	oStream << m_strColumnName;

	if(m_iAppendSpaces > 0)
	{
		for(int iIndex=0; iIndex<m_iAppendSpaces; iIndex++)
			oStream << " \t";
	}
}

void DataColumn::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule,
	                               Node *lpNode, DataChart *lpChart, BOOL bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, FALSE);
	m_lpChart = lpChart;

	if(bVerify) VerifySystemPointers();
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

/**
\brief	 Determines if this column has an index value less than the index value of the column being passed in.

\details This is used to sort the columns based on the index value.

\return	true if this objects index value is less than the object passed in, false otherwise.
**/
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

/**
\brief	Compares two DataColumn items to find the one that is less than the other.

\details This is used to sort DataColumns based on their index values.

\author	dcofer
\date	3/18/2011

\param [in,out]	lpColumn1	Pointer to the first data column to test. 
\param [in,out]	lpColumn2	Pointer to the second data column to test. 

\return	true if index of column1 is less than column2.
**/
BOOL LessThanDataColumnCompare(DataColumn *lpColumn1, DataColumn *lpColumn2)
{
	return lpColumn1->operator<(lpColumn2);
}

	}			//Charting
}				//AnimatSim
