/**
\file	DataColumn.cpp

\brief	Implements the data column class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBase.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
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

void DataColumn::Name(string strValue) 
{
	if(Std_IsBlank(strValue)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: ColumnName");
	m_strName = strValue;
}

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
	if(Std_IsBlank(strType)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: DataType");

	m_strDataType = strType;
}

/**
\brief	Gets the GUID ID of the item to chart.

\author	dcofer
\date	3/26/2011

\return	ID of item to chart.
**/
string DataColumn::TargetID() {return m_strTargetID;}

/**
\brief	Sets the GUID ID of teh item to chart.

\author	dcofer
\date	3/26/2011

\param	strID	ID of the item to chart. 
**/
void DataColumn::TargetID(string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: TargetID");
	m_strTargetID = strID;
}

/**
\brief	Gets how many spaces to append.

\author	dcofer
\date	3/26/2011

\return	Number of appends spaces.
**/
int DataColumn::AppendSpaces() {return m_iAppendSpaces;}

/**
\brief	Sets whether to append spaces.

\author	dcofer
\date	3/26/2011

\param	iSpaces	The spaces to append. 
**/
void DataColumn::AppendSpaces(int iSpaces)
{
	Std_InValidRange((int) 0, (int) 10, iSpaces, TRUE, "AppendSpaces");
	m_iAppendSpaces = iSpaces;
}

/**
\brief	Gets the column index.

\author	dcofer
\date	3/26/2011

\return	Column index.
**/
int DataColumn::ColumnIndex() {return m_iColumnIndex;}

/**
\brief	Sets the Column index.

\author	dcofer
\date	3/26/2011

\param	iIndex	Zero-based index of the column. 
**/
void DataColumn::ColumnIndex(int iIndex)
{
	Std_IsAboveMin((int) -1, iIndex, TRUE, "ColumnIndex", TRUE);
	m_iColumnIndex = iIndex;
}

/**
\brief	Gets the row index.

\author	dcofer
\date	3/26/2011

\return	row index.
**/
int DataColumn::RowIndex() {return m_iRowIndex;}

/**
\brief	Sets the Row index.

\author	dcofer
\date	3/26/2011

\param	iIndex	Zero-based index of the row. 
**/
void DataColumn::RowIndex(int iIndex)
{
	Std_IsAboveMin((int) -1, iIndex, TRUE, "RowIndex", TRUE);
	m_iRowIndex = iIndex;
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

	if(AnimatBase::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "COLUMNINDEX")
	{
		//ColumnIndex(atoi(strValue.c_str()));
		return TRUE;
	}

	if(strType == "DATATYPE")
	{
		DataType(strValue);
		Initialize();
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
	oStream << m_strName;

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
	if(this->m_iColumnIndex < lpColumn->m_iColumnIndex)
		return TRUE;

	return FALSE;
}

void DataColumn::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into DataColumn Element

	TargetID(oXml.GetChildString("TargetID"));
	DataType(oXml.GetChildString("DataType"));
	AppendSpaces(oXml.GetChildInt("AppendSpaces", m_iAppendSpaces));
	ColumnIndex(oXml.GetChildInt("Column", m_iColumnIndex));
	RowIndex(oXml.GetChildInt("Row", m_iColumnIndex));

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
