/**
\file	DataChartMgr.cpp

\brief	Implements the data chart manager class.
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
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
/**
\brief	Default constructor.

\author	dcofer
\date	3/18/2011
**/
DataChartMgr::DataChartMgr()
{}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
DataChartMgr::~DataChartMgr()
{}

/**
\brief	Searches for a DataColumn with the specified column name.

\author	dcofer
\date	3/18/2011

\param	strChartKey  	GUID ID to the chart that contains the column of interest. 
\param	strColumnName	Name of the DataColumn we are searching for. 
\param	bThrowError  	If no column is found and this is true, then an exception is thrown, otherwise NULL is returned. 

\return	Pointer to the found DataColumn, NULL if not found and bThrowError = FALSE.
**/
DataColumn *DataChartMgr::FindDataColumn(string strChartKey, string strColumnName, BOOL bThrowError)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey, bThrowError));
	if(!lpChart) return NULL;

	DataColumn *lpColumn = lpChart->FindColumn(strColumnName, bThrowError);
	return lpColumn;
}

/**
\brief	Removes the specified data column.

\author	dcofer
\date	3/18/2011

\param	strChartKey  	GUID ID to the chart that contains the column of interest. 
\param	strColumnName	Name of the DataColumn we are deleting. 
\param	bThrowError  	If no column is found and this is true, then an exception is thrown, otherwise NULL is returned. 
**/
void DataChartMgr::RemoveDataColumn(string strChartKey, string strColumnName, BOOL bThrowError)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey, bThrowError));
	if(!lpChart) return;

	lpChart->RemoveColumn(strColumnName, bThrowError);
}

/**
\brief	Adds a data column to the specified chart.

\author	dcofer
\date	3/18/2011

\param	strChartKey  	GUID ID to the chart that contains the column of interest. 
\param [in,out]	lpColumn	Pointer to the DataColumn to add. 
**/
void DataChartMgr::AddDataColumn(string strChartKey, DataColumn *lpColumn)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey));
	lpChart->AddColumn(lpColumn);	
}

/**
\brief	Adds a data chart to the manager. 

\details This method is primiarly used by the GUI to add a new chart to the system by specifying an xml packet to load.

\author	dcofer
\date	3/18/2011

\param	strXml	The xml data to load for the new chart. 

\return	true if it succeeds, false if it fails.
**/
BOOL DataChartMgr::AddDataChart(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("DataChart");
	LoadDataChart(oXml);
	ReInitialize();
	return TRUE;
}

/**
\brief	Removes the data chart described by ID.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID of the chart to remove. 

\return	true if it succeeds, false if it fails.
**/
BOOL DataChartMgr::RemoveDataChart(string strID)
{
	Remove(strID);
	ReInitialize();
	return TRUE;
}

void DataChartMgr::Load(CStdXml &oXml)
{
	VerifySystemPointers();

	Reset();

	if(oXml.FindChildElement("DataCharts", FALSE))
	{
		oXml.IntoElem(); //Into DataCharts Element

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadDataChart(oXml);
		}

		oXml.OutOfElem(); //OutOf Environment Element
	}
}

/**
\brief	Loads a data chart.

\author	dcofer
\date	3/18/2011

\param [in,out]	oXml	The xml used to load the chart. 

\return	Pointer to the new DataChart.
\exception Throws an exception if there is a problem creating or loading the new chart.
**/
DataChart *DataChartMgr::LoadDataChart(CStdXml &oXml)
{
	DataChart *lpChart = NULL;
	string strModuleName, strType, strFilename;

try
{
	oXml.IntoElem(); //Into DataChart Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	strFilename = oXml.GetChildString("Filename", "");
	oXml.OutOfElem(); //OutOf DataChart Element

	lpChart = dynamic_cast<DataChart *>(m_lpSim->CreateObject(strModuleName, "DataChart", strType));
	if(!lpChart)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "DataChart");

	lpChart->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
	if(!Std_IsBlank(strFilename))
		lpChart->Load(m_lpSim->ProjectPath(), strFilename);
	else
	{
		lpChart->ProjectPath(m_lpSim->ProjectPath());
		lpChart->Load(oXml);
	}

	Add(lpChart);
	return lpChart;
}
catch(CStdErrorInfo oError)
{
	if(lpChart) delete lpChart;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpChart) delete lpChart;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			//Charting
}				//AnimatSim

