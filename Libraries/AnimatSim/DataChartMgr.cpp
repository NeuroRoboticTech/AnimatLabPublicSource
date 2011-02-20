// DataChartMgr.cpp: implementation of the DataChartMgr class.
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

DataChartMgr::DataChartMgr()
{}

DataChartMgr::~DataChartMgr()
{}

DataColumn *DataChartMgr::FindDataColumn(string strChartKey, string strColumnName, BOOL bThrowError)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey, bThrowError));
	if(!lpChart) return NULL;

	DataColumn *lpColumn = lpChart->FindColumn(strColumnName, bThrowError);
	return lpColumn;
}

void DataChartMgr::RemoveDataColumn(string strChartKey, string strColumnName, BOOL bThrowError)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey, bThrowError));
	if(!lpChart) return;

	lpChart->RemoveColumn(strColumnName, bThrowError);
}

void DataChartMgr::AddDataColumn(string strChartKey, DataColumn *lpColumn)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey));
	lpChart->AddColumn(lpColumn);	
}

void DataChartMgr::ModifyDataColumn(string strChartKey, string strColumnName, string strDataType)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey));
	if(!lpChart) return;

	DataColumn *lpColumn = lpChart->FindColumn(strColumnName, TRUE);
	lpColumn->DataType(strDataType);
	lpColumn->IsInitialized(FALSE);
}

void DataChartMgr::SetDataColumnIndex(string strChartKey, string strColumnName, int iIndex)
{
	DataChart *lpChart = dynamic_cast<DataChart *>(Find(strChartKey));
	if(!lpChart) return;

	DataColumn *lpColumn = lpChart->FindColumn(strColumnName, TRUE);
	lpColumn->Index(iIndex);
}

BOOL DataChartMgr::AddDataChart(Simulator *lpSim, string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("DataChart");
	LoadDataChart(lpSim, oXml);
	ReInitialize(lpSim);
	return TRUE;
}

BOOL DataChartMgr::RemoveDataChart(Simulator *lpSim, string strID)
{
	Remove(lpSim, strID);
	ReInitialize(lpSim);
	return TRUE;
}

void DataChartMgr::Load(Simulator *lpSim, CStdXml &oXml)
{
	TRACE_DEBUG("Loading data charts from Xml.");

	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	Reset();

	if(oXml.FindChildElement("DataCharts", FALSE))
	{
		oXml.IntoElem(); //Into DataCharts Element

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadDataChart(lpSim, oXml);
		}

		oXml.OutOfElem(); //OutOf Environment Element
	}
}


DataChart *DataChartMgr::LoadDataChart(Simulator *lpSim, CStdXml &oXml)
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

	lpChart = dynamic_cast<DataChart *>(lpSim->CreateObject(strModuleName, "DataChart", strType));
	if(!lpChart)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "DataChart");

	if(!Std_IsBlank(strFilename))
		lpChart->Load(lpSim, lpSim->ProjectPath(), strFilename);
	else
	{
		lpChart->ProjectPath(lpSim->ProjectPath());
		lpChart->Load(lpSim, oXml);
	}

	Add(lpSim, lpChart);
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

