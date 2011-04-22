/**
\file	DataChart.cpp

\brief	Implements the data chart class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
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
#include "NeuralModule.h"
#include "Adapter.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataColumn.h"
#include "DataChart.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
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
DataChart::DataChart()
{
	m_lCollectTimeWindow = -1;
	m_fltCollectTimeWindow = -1;
	m_iCollectInterval = 0;
	m_aryDataBuffer = NULL;
	m_aryTimeBuffer = NULL;
	m_lRowCount = 0;
	m_lColumnCount = 0;
	m_lCurrentCol = 0;
	m_lCurrentRow = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
DataChart::~DataChart()
{

try
{
	if(m_aryDataBuffer) delete[] m_aryDataBuffer;
	if(m_aryTimeBuffer) delete[] m_aryTimeBuffer;
	m_aryDataColumns.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of DataChart\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	 Compares the Start slice time of two data charts to tell if one is less than the other

\details This is used within an operation that sorts the data charts.

\return	true if this items start slice is less than the one passed in, false if not.
**/
BOOL DataChart::operator<(ActivatedItem *lpItem)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(lpItem);

	if(!lpChart)
		THROW_ERROR(Al_Err_lItemNotDataChartType, Al_Err_strItemNotDataChartType);

	if(m_lStartSlice < lpChart->m_lStartSlice)
		return TRUE;

	if( (m_lStartSlice == lpChart->m_lStartSlice) && (m_lEndSlice < lpChart->m_lEndSlice))
		return TRUE;

	return FALSE;
}

string DataChart::Type() {return "DataChart";}

void DataChart::StartTime(float fltVal) 
{
	ActivatedItem::StartTime(fltVal);
	ReInitialize();
}

void DataChart::EndTime(float fltVal) 
{
	ActivatedItem::EndTime(fltVal);
	ReInitialize();
}

/**
\brief	Gets whether a start/end time is set for this chart.

\author	dcofer
\date	3/18/2011

\return	true if it start/end time is specified, false otherwise.
**/
BOOL DataChart::SetStartEndTime() {return m_bSetStartEndTime;}

/**
\brief	Sets whether a start/end time is specified for this chart.

\author	dcofer
\date	3/18/2011

\param	bVal	true to use the start/end time. 
**/
void DataChart::SetStartEndTime(BOOL bVal)
{
	m_bSetStartEndTime = bVal;
	ActivatedItem::AlwaysActive(!m_bSetStartEndTime);
	ReInitialize();
}

/**
\brief	Gets the buffer size.

\author	dcofer
\date	3/18/2011

\return	The length of the data buffer.
**/
long DataChart::BufferSize() {return m_lRowCount*m_lColumnCount;}

/**
\brief	Gets the size of buffer that is currently used.

\author	dcofer
\date	3/18/2011

\return	used buffer size.
**/
long DataChart::UsedBufferSize() {return m_lCurrentCol*m_lCurrentRow;}

/**
\brief	Gets the buffer byte size.

\author	dcofer
\date	3/18/2011

\return	Buffer byte size.
**/
long DataChart::BufferByteSize() {return BufferSize()*sizeof(float);}

/**
\brief	Gets the used buffer byte size.

\author	dcofer
\date	3/18/2011

\return	Used buffer byte size.
**/
long DataChart::UsedBufferByteSize() {return UsedBufferSize()*sizeof(float);}

/**
\brief	Gets the time buffer.

\author	dcofer
\date	3/18/2011

\return	Pointer to the time buffer.
**/
float *DataChart::TimeBuffer() {return m_aryTimeBuffer;}

/**
\brief	Gets the data buffer.

\author	dcofer
\date	3/18/2011

\return	Pointer to the data buffer.
**/
float *DataChart::DataBuffer() {return m_aryDataBuffer;}

/**
\brief	Gets the collect interval.

\details The collect data interval tells how many time slices should occur in-between
collecting data.

\author	dcofer
\date	3/18/2011

\return	The collect interval.
**/
int DataChart::CollectInterval() {return m_iCollectInterval;};

/**
\brief	Sets the Collect interval using the time slice.

\author	dcofer
\date	3/18/2011

\param	iVal	The new interval. 
**/
void DataChart::CollectInterval(int iVal)
{
	m_iCollectInterval = iVal;
	ReInitialize();
}

/**
\brief	Sets the Collect interval using the time.

\details This lets the user specify the collect interval using a time value.
It then calculates the number of time slices for the collect interval.

\author	dcofer
\date	3/18/2011

\param	fltVal	The new time value. 
**/
void DataChart::CollectInterval(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "CollectInterval");

	//Lets calculate the number of slices for the collect interval.
	m_iCollectInterval = (int) (fltVal/m_lpSim->TimeStep());
	if(m_iCollectInterval<=0) m_iCollectInterval = 1;

	ReInitialize();
}

/**
\brief	Gets the collect time window time slices.

\author	dcofer
\date	3/18/2011

\return	Number of time slices that data is collected.
**/
long DataChart::CollectTimeWindow() {return m_lCollectTimeWindow;}

/**
\brief	Sets the Collect time window using time slices.

\author	dcofer
\date	3/18/2011

\param	lVal	The new value in time slices. 
**/
void DataChart::CollectTimeWindow(long lVal)
{
	m_lCollectTimeWindow = lVal;
	ReInitialize();
}

/**
\brief	Sets the collect time window in time value.

\author	dcofer
\date	3/18/2011

\param	fltVal	The new time value. 
**/
void DataChart::CollectTimeWindow(float fltVal)
{
	m_fltCollectTimeWindow = fltVal;

	if(m_fltCollectTimeWindow <= 0)
		m_lCollectTimeWindow = m_lEndSlice - m_lStartSlice;
	else
		m_lCollectTimeWindow = (long) (m_fltCollectTimeWindow / m_lpSim->TimeStep() + 0.5);

	ReInitialize();
}

/**
\brief	Gets the project path.

\author	dcofer
\date	3/18/2011

\return	project path.
**/
string DataChart::ProjectPath() {return m_strProjectPath;}

/**
\brief	Sets the Project path.

\author	dcofer
\date	3/18/2011

\param	strVal	The new value. 
**/
void DataChart::ProjectPath(string strVal) {m_strProjectPath = strVal;}

/**
\brief	Gets the column count.

\author	dcofer
\date	3/18/2011

\return	number of DataColumn objects.
**/
long DataChart::ColumnCount() {return m_lColumnCount;}

/**
\brief	Gets the current row.

\author	dcofer
\date	3/18/2011

\return	The currently selected row in the data buffer.
**/
long DataChart::CurrentRow() {return m_lCurrentRow;}

/**
\brief	Sets the Current row.

\author	dcofer
\date	3/18/2011

\param	iVal	The new current row value. 
**/
void DataChart::CurrentRow(long iVal) {m_lCurrentRow = iVal;}

/**
\brief	Locks this data chart buffer from being written by any other process.

\details This is for multi-threaded environments. It is called just before data is added
to the buffer to prevent any other process from modifying the buffer while we are doing so.

\author	dcofer
\date	3/18/2011

\return	true if it locks the buffer, false if it is already locked.
**/
BOOL DataChart::Lock() {return TRUE;}

/**
\brief	Unlocks this data chart buffer from being written by any other process.

\details This is for multi-threaded environments. It is called just after data is added
to the buffer to remove the lock so that other process can modify the buffer.

\author	dcofer
\date	3/18/2011
**/
void DataChart::Unlock() {}

void DataChart::Initialize()
{
	ActivatedItem::Initialize();

	if(m_fltCollectTimeWindow <= 0)
		m_lCollectTimeWindow = m_lEndSlice - m_lStartSlice;
	else
		m_lCollectTimeWindow = (long) (m_fltCollectTimeWindow / m_lpSim->TimeStep() + 0.5);

	//First lets determine the buffer space that will be required for this chart.
	m_lColumnCount = CalculateChartColumnCount();
	//We add 10 because we want the buffer to be bigger than the actual amount of data that is collected.
	m_lRowCount = (m_lCollectTimeWindow/m_iCollectInterval) + 10;

	long lBuffSize = m_lColumnCount * m_lRowCount;
	
	if(lBuffSize > MAX_DATA_CHART_BUFFER)
		THROW_PARAM_ERROR(Al_Err_lExceededMaxBuffer, Al_Err_strExceededMaxBuffer, "Buffer Size", lBuffSize);

	if(m_aryDataBuffer) delete[] m_aryDataBuffer;
	m_aryDataBuffer = NULL;

	if(m_aryTimeBuffer) delete[] m_aryTimeBuffer;
	m_aryTimeBuffer = NULL;

	//Create the buffer and initialize it.
	m_aryDataBuffer = new float[lBuffSize];
	memset(m_aryDataBuffer, 0, (sizeof(float) * lBuffSize));

	m_aryTimeBuffer = new float[m_lRowCount];
	memset(m_aryTimeBuffer, 0, (sizeof(float) * m_lRowCount));

	//Now sort the data columns based on their ID value.
	stable_sort(m_aryDataColumns.begin(), m_aryDataColumns.end(), LessThanDataColumnCompare);

	//Now initialize the data columns.
	int iCount = m_aryDataColumns.GetSize();
	for(int iCol=0; iCol<iCount; iCol++)
		m_aryDataColumns[iCol]->Initialize();
}

void DataChart::ReInitialize()
{
	if(!m_bInitialized)
		Initialize();
	else
	{
		if(m_fltCollectTimeWindow <= 0)
			m_lCollectTimeWindow = m_lEndSlice - m_lStartSlice;
		else
			m_lCollectTimeWindow = (long) (m_fltCollectTimeWindow / m_lpSim->TimeStep() + 0.5);

		long lColumnCount = CalculateChartColumnCount();
		//We add 10 because we want the buffer to be bigger than the actual amount of data that is collected.
		long lRowCount = (m_lCollectTimeWindow/m_iCollectInterval) + 10;

		if(!m_aryDataBuffer || !m_aryTimeBuffer || lColumnCount != m_lColumnCount || lRowCount != m_lRowCount)
		{
			m_lColumnCount = lColumnCount;
			m_lRowCount = lRowCount;

			long lBuffSize = m_lColumnCount * m_lRowCount;
			
			if(lBuffSize > MAX_DATA_CHART_BUFFER)
				THROW_PARAM_ERROR(Al_Err_lExceededMaxBuffer, Al_Err_strExceededMaxBuffer, "Buffer Size", lBuffSize);

			if(m_aryDataBuffer) delete[] m_aryDataBuffer;
			m_aryDataBuffer = NULL;

			if(m_aryTimeBuffer) delete[] m_aryTimeBuffer;
			m_aryTimeBuffer = NULL;

			//Create the buffer and initialize it.
			m_aryDataBuffer = new float[lBuffSize];
			memset(m_aryDataBuffer, 0, (sizeof(float) * lBuffSize));

			m_aryTimeBuffer = new float[m_lRowCount];
			memset(m_aryTimeBuffer, 0, (sizeof(float) * m_lRowCount));

			//Start the current row back over at 0 again.
			m_lCurrentRow = 0;
		}

		//Now sort the data columns based on their ID value.
		stable_sort(m_aryDataColumns.begin(), m_aryDataColumns.end(), LessThanDataColumnCompare);

		//Now initialize the data columns.
		int iCount = m_aryDataColumns.GetSize();
		for(int iCol=0; iCol<iCount; iCol++)
			m_aryDataColumns[iCol]->ReInitialize();
	}
}

void DataChart::ResetSimulation()
{
	ActivatedItem::ResetSimulation();
	m_lCurrentCol = 0;
	m_lCurrentRow = 0;
	ReInitialize();	
}

/**
\brief	Calculates the chart column count.

\details Some data columns can have more than on column in the data buffer. We
call the ColumnCount property of each column to find the total number for the chart.
\author	dcofer
\date	3/18/2011

\return	The calculated chart column count.
**/
long DataChart::CalculateChartColumnCount()
{
	long lColCount=0;
	int iCount = m_aryDataColumns.GetSize();
	for(int iCol=0; iCol<iCount; iCol++)
		lColCount += m_aryDataColumns[iCol]->ColumnCount();

	return lColCount;
}

/**
\brief	Creates and adds a DataColumn from XML.

\details This method is primarily used by the GUI to create a new data column when an xml packet is provided.

\author	dcofer
\date	3/18/2011

\param	strXml	The xml packet to load for the data column. 
**/
void DataChart::AddColumn(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("DataColumn");

	LoadDataColumn(oXml);
	ReInitialize();
}

/**
\brief	Adds a column. 

\author	dcofer
\date	3/18/2011

\param [in,out]	lpColumn	Pointer to a DataColumn to add to the chart. 
**/
void DataChart::AddColumn(DataColumn *lpColumn)
{
	//First lets make sure this is a unique item key.
	try
	{
		m_aryColumnsMap.Add(Std_CheckString(lpColumn->ID()), lpColumn);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate data column ID: " + lpColumn->ID(); 
		THROW_ERROR(oError.m_lError, oError.m_strError);
	}

	m_aryDataColumns.Add(lpColumn);
}

/**
\brief	Removes the column with the specified ID.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID of the DataColumn to remove.
\param	bThrowError	If true and the ID is not found then it throws an error, otherwise it does nothing.
**/
void DataChart::RemoveColumn(string strID, BOOL bThrowError)
{
	int iIndex=0;
	DataColumn *lpColumn = FindColumn(strID, iIndex, bThrowError);
	if(!lpColumn) return;
	
	m_aryColumnsMap.Remove(Std_CheckString(strID));
	m_aryDataColumns.RemoveAt(iIndex);
}

/**
\brief	Searches for a column that matches the specified ID.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID of the column to find.
\param	bThrowError	If true and the ID is not found then it throws an error, otherwise it does nothing.

\return	null if it the column is not found and bThrowError is false, else a pointer to the found column.
**/
DataColumn *DataChart::FindColumn(string strID, BOOL bThrowError)
{
	DataColumn *lpColumn = NULL;
	CStdMap<string, DataColumn *>::iterator oPos;
	oPos = m_aryColumnsMap.find(Std_CheckString(strID));

	if(oPos != m_aryColumnsMap.end())
		lpColumn =  oPos->second;
	else if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lDataColumnIDNotFound, Al_Err_strDataColumnIDNotFound, " DataColumn ID: " + strID);

	return lpColumn;
}

/**
\brief	Searches for a column that matches the specified ID.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID of the column to find.
\param [in,out]	iIndex	Zero-based index of the found column in the array. 
\param	bThrowError	If true and the ID is not found then it throws an error, otherwise it does nothing.

\return	null if it the column is not found and bThrowError is false, else a pointer to the found column.
**/
DataColumn *DataChart::FindColumn(string strID, int &iIndex, BOOL bThrowError)
{
	int iCount = m_aryDataColumns.GetSize();
	DataColumn *lpColumn = NULL;
	strID = Std_CheckString(strID);
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpColumn = m_aryDataColumns[iIndex];

		if(lpColumn->ID() == strID)
			return lpColumn;
	}

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lDataColumnIDNotFound, Al_Err_strDataColumnIDNotFound, " DataColumn ID: " + strID);

	return NULL;
}

#pragma region DataAccesMethods

BOOL DataChart::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(ActivatedItem::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "STARTTIME")
	{
		StartTime(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ENDTIME")
	{
		EndTime(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SETSTARTENDTIME")
	{
		SetStartEndTime(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "COLLECTTIMEWINDOW")
	{
		CollectTimeWindow((float) atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "COLLECTINTERVAL")
	{
		CollectInterval((float) atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

BOOL DataChart::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "DATACOLUMN")
	{
		AddColumn(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL DataChart::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "DATACOLUMN")
	{
		RemoveColumn(strID, bThrowError);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

void DataChart::StepSimulation()
{
	if(!(m_lpSim->TimeSlice()%m_iCollectInterval))
	{
		m_lCurrentCol = 0;

		if(m_aryTimeBuffer)
		{
			float fltTime = m_lpSim->Time();
			m_aryTimeBuffer[m_lCurrentRow] = m_lpSim->Time();
		}

		int iCount = m_aryDataColumns.GetSize();
		for(int iCol=0; iCol<iCount; iCol++)
			m_aryDataColumns[iCol]->StepSimulation();

		m_lCurrentRow++;
	}
}

/**
\brief	Adds a data element to the data buffer.

\author	dcofer
\date	3/18/2011

\param	iColumn	The column in the data buffer where we need to add the value. 
\param	iRow   	The row in the data buffer where we need to add the value.
\param	fltVal 	The new value to add. 
**/
void DataChart::AddData(int iColumn, int iRow, float fltVal)
{
	if(iColumn == -1 && iRow == -1)
	{
		if( (m_lCurrentCol>=m_lColumnCount) )
			THROW_PARAM_ERROR(Std_Err_lAboveMaxValue, Std_Err_strAboveMaxValue, "Current Col Count", m_lCurrentCol);

		if( (m_lCurrentRow>=m_lRowCount) )
			THROW_PARAM_ERROR(Std_Err_lAboveMaxValue, Std_Err_strAboveMaxValue, "Current Row Count", m_lCurrentRow);

		m_aryDataBuffer[(m_lCurrentRow*m_lColumnCount) + m_lCurrentCol] = fltVal;
		m_lCurrentCol++;
	}
	else
		m_aryDataBuffer[(iRow*m_lColumnCount) + iColumn] = fltVal;
}

/**
\brief	Loads a data chart from a file.

\author	dcofer
\date	3/18/2011

\param	strProjectPath	Full pathname of the string project file. 
\param	strConfigFile 	The string configuration file. 
**/
void DataChart::Load(string strProjectPath, string strConfigFile)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading data chart config file.\r\nFile: " + strConfigFile);

	if(Std_IsBlank(strProjectPath)) 
		THROW_ERROR(Al_Err_lProjectPathBlank, Al_Err_strProjectPathBlank);

	if(Std_IsBlank(strConfigFile)) 
		THROW_ERROR(Al_Err_lFilenameBlank, Al_Err_strFilenameBlank);

	m_strProjectPath = strProjectPath;
	m_strConfigFilename = strConfigFile;
	
	oXml.Load(AnimatSim::GetFilePath(strProjectPath, strConfigFile));

	oXml.FindElement("ChartConfiguration");
	oXml.FindChildElement("DataChart");

	Load(oXml);

	TRACE_DEBUG("Finished loading data chart config file.");
}


void DataChart::Load(CStdXml &oXml)
{
	short iColumn, iTotalColumns;

	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into DataChart Element

	if(m_aryDataBuffer) delete[] m_aryDataBuffer;
	m_aryDataBuffer = NULL;
	m_aryDataColumns.RemoveAll();

	CollectInterval(oXml.GetChildFloat("CollectInterval"));

	SetStartEndTime(oXml.GetChildBool("SetStartEndTime", FALSE));
	CollectTimeWindow(oXml.GetChildFloat("CollectTimeWindow", -1));
	
	//If we are not setting start/end time then it is always active.
	ActivatedItem::AlwaysActive(!m_bSetStartEndTime);

	//*** Begin Loading DataColumns. *****
	oXml.IntoChildElement("DataColumns");

	iTotalColumns = oXml.NumberOfChildren();
	DataColumn *lpColumn = NULL;
	for(iColumn=0; iColumn<iTotalColumns; iColumn++)
	{
		oXml.FindChildByIndex(iColumn);
		lpColumn = LoadDataColumn(oXml);
	}

	oXml.OutOfElem();
	//*** End Loading DataColumns. *****

	oXml.OutOfElem(); //OutOf DataChart Element
}

/**
\brief	Loads a data column.

\author	dcofer
\date	3/18/2011

\param [in,out]	oXml	The xml to use when loading the data column. 

\return	Pointer to the new DataColumn.
\exception Throws an exception if there is an error creating or loading the DataColumn.
**/
DataColumn *DataChart::LoadDataColumn(CStdXml &oXml)
{
	DataColumn *lpColumn=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpColumn = dynamic_cast<DataColumn *>(m_lpSim->CreateObject(strModuleName, "DataColumn", strType));
	if(!lpColumn)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "DataColumn");

	lpColumn->SetSystemPointers(m_lpSim, NULL, NULL, NULL, this, TRUE);
	lpColumn->Load(oXml);

	AddColumn(lpColumn);

	return lpColumn;
}
catch(CStdErrorInfo oError)
{
	if(lpColumn) delete lpColumn;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpColumn) delete lpColumn;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


	}			//Charting
}				//AnimatSim

