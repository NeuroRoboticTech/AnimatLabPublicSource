// DataChart.cpp: implementation of the DataChart class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "Node.h"
#include "IPhysicsBody.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

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

void DataChart::Trace(ostream &oOs)
{
	oOs << "(Start: " << m_lStartSlice << ", End: "<< m_lEndSlice << ") ";// ", CollectInterval: " << m_iCollectInterval;
	//oOs << ", Output: " << m_strOutputFilename << ") ";
}

void DataChart::StartTime(float fltVal) 
{
	ActivatedItem::StartTime(fltVal);

	//If we are changing the always active then we need to re-initialize the chart mgr so it knows.
	Simulator *lpSim = GetSimulator();
	ReInitialize(lpSim);
}

void DataChart::EndTime(float fltVal) 
{
	ActivatedItem::EndTime(fltVal);

	//If we are changing the always active then we need to re-initialize the chart mgr so it knows.
	Simulator *lpSim = GetSimulator();
	ReInitialize(lpSim);
}

void DataChart::SetStartEndTime(BOOL bVal)
{
	m_bSetStartEndTime = bVal;
	ActivatedItem::AlwaysActive(!m_bSetStartEndTime);
	
	//If we are changing the always active then we need to re-initialize the chart mgr so it knows.
	Simulator *lpSim = GetSimulator();
	ReInitialize(lpSim);
}

void DataChart::CollectInterval(int iVal)
{
	m_iCollectInterval = iVal;
	ReInitialize(GetSimulator());
}

void DataChart::CollectInterval(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "CollectInterval");

	Simulator *lpSim = GetSimulator();
	//Lets calculate the number of slices for the collect interval.
	m_iCollectInterval = (int) (fltVal/lpSim->TimeStep());
	if(m_iCollectInterval<=0) m_iCollectInterval = 1;

	ReInitialize(lpSim);
}

void DataChart::CollectTimeWindow(long lVal)
{
	m_lCollectTimeWindow = lVal;
	ReInitialize(GetSimulator());
}

void DataChart::CollectTimeWindow(float fltVal)
{
	m_fltCollectTimeWindow = fltVal;
	ReInitialize(GetSimulator());
}

void DataChart::Initialize(Simulator *lpSim)
{
	ActivatedItem::Initialize(lpSim);

	if(m_fltCollectTimeWindow <= 0)
		m_lCollectTimeWindow = m_lEndSlice - m_lStartSlice;
	else
		m_lCollectTimeWindow = (long) (m_fltCollectTimeWindow / lpSim->TimeStep() + 0.5);

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
		m_aryDataColumns[iCol]->Initialize(lpSim);
}

void DataChart::ReInitialize(Simulator *lpSim)
{
	if(!m_bInitialized)
		Initialize(lpSim);
	else
	{
		if(m_fltCollectTimeWindow <= 0)
			m_lCollectTimeWindow = m_lEndSlice - m_lStartSlice;
		else
			m_lCollectTimeWindow = (long) (m_fltCollectTimeWindow / lpSim->TimeStep() + 0.5);

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
			m_aryDataColumns[iCol]->ReInitialize(lpSim);
	}
}

void DataChart::ResetSimulation(Simulator *lpSim)
{
	ActivatedItem::ResetSimulation(lpSim);
	m_lCurrentCol = 0;
	m_lCurrentRow = 0;
	ReInitialize(lpSim);	
}

long DataChart::CalculateChartColumnCount()
{
	long lColCount=0;
	int iCount = m_aryDataColumns.GetSize();
	for(int iCol=0; iCol<iCount; iCol++)
		lColCount += m_aryDataColumns[iCol]->ColumnCount();

	return lColCount;
}

void DataChart::AddColumn(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("DataColumn");

	Simulator *lpSim = GetSimulator();
	LoadDataColumn(lpSim, oXml);
	ReInitialize(lpSim);
}

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

void DataChart::RemoveColumn(string strID, BOOL bThrowError)
{
	int iIndex=0;
	DataColumn *lpColumn = FindColumn(strID, iIndex, bThrowError);
	if(!lpColumn) return;
	
	m_aryColumnsMap.Remove(Std_CheckString(strID));
	m_aryDataColumns.RemoveAt(iIndex);
}

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

void DataChart::Activate(Simulator *lpSim)
{
	ActivatedItem::Activate(lpSim);
}

void DataChart::StepSimulation(Simulator *lpSim)
{
	if(!(lpSim->TimeSlice()%m_iCollectInterval))
	{
		m_lCurrentCol = 0;

		if(m_aryTimeBuffer)
		{
			float fltTime = lpSim->Time();
			m_aryTimeBuffer[m_lCurrentRow] = lpSim->Time();
		}

		int iCount = m_aryDataColumns.GetSize();
		for(int iCol=0; iCol<iCount; iCol++)
			m_aryDataColumns[iCol]->StepSimulation(lpSim, this);

		m_lCurrentRow++;
	}
}


void DataChart::Deactivate(Simulator *lpSim)
{
	ActivatedItem::Deactivate(lpSim);
}

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

void DataChart::Load(Simulator *lpSim, string strProjectPath, string strConfigFile)
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

	Load(lpSim, oXml);

	TRACE_DEBUG("Finished loading data chart config file.");
}


void DataChart::Load(Simulator *lpSim, CStdXml &oXml)
{
	short iColumn, iTotalColumns;

	ActivatedItem::Load(lpSim, oXml);

	oXml.IntoElem();  //Into DataChart Element

	if(m_aryDataBuffer) delete[] m_aryDataBuffer;
	m_aryDataBuffer = NULL;
	m_aryDataColumns.RemoveAll();

	float fltCollectInterval = oXml.GetChildFloat("CollectInterval");
	Std_IsAboveMin((float) 0, fltCollectInterval, TRUE, "CollectInterval");

	//Lets calculate the number of slices for the collect interval.
	m_iCollectInterval = (int) (fltCollectInterval/lpSim->TimeStep());
	if(m_iCollectInterval<=0) m_iCollectInterval = 1;

	m_bSetStartEndTime = oXml.GetChildBool("SetStartEndTime", FALSE);
	m_fltCollectTimeWindow = oXml.GetChildFloat("CollectTimeWindow", -1);
	
	//If we are not setting start/end time then it is always active.
	ActivatedItem::AlwaysActive(!m_bSetStartEndTime);

	//*** Begin Loading DataColumns. *****
	oXml.IntoChildElement("DataColumns");

	iTotalColumns = oXml.NumberOfChildren();
	DataColumn *lpColumn = NULL;
	for(iColumn=0; iColumn<iTotalColumns; iColumn++)
	{
		oXml.FindChildByIndex(iColumn);
		lpColumn = LoadDataColumn(lpSim, oXml);

		if(lpColumn->Index() < 0)
			lpColumn->Index(iColumn);
	}

	oXml.OutOfElem();
	//*** End Loading DataColumns. *****

	oXml.OutOfElem(); //OutOf DataChart Element
}


DataColumn *DataChart::LoadDataColumn(Simulator *lpSim, CStdXml &oXml)
{
	DataColumn *lpColumn=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpColumn = dynamic_cast<DataColumn *>(lpSim->CreateObject(strModuleName, "DataColumn", strType));
	if(!lpColumn)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "DataColumn");

	lpColumn->Load(lpSim, oXml);

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

void DataChart::LoadChart(Simulator *lpSim, CStdXml &oXml)
{
	oXml.FindElement("ChartConfiguration");
	oXml.FindChildElement("DataChart");

	ActivatedItem::Load(lpSim, oXml);

	oXml.IntoElem();

	float fltCollectInterval = oXml.GetChildFloat("CollectInterval");
	Std_IsAboveMin((float) 0, fltCollectInterval, TRUE, "CollectInterval");

	//Lets calculate the number of slices for the collect interval.
	m_iCollectInterval = (int) (fltCollectInterval/lpSim->TimeStep());
	if(m_iCollectInterval<=0) m_iCollectInterval = 1;

	m_bSetStartEndTime = oXml.GetChildBool("SetStartEndTime", FALSE);
	m_fltCollectTimeWindow = oXml.GetChildFloat("CollectTimeWindow", -1);

	//If we are not setting start/end time then it is always active.
	ActivatedItem::AlwaysActive(!m_bSetStartEndTime);

	oXml.OutOfElem();
}

	}			//Charting
}				//AnimatSim

