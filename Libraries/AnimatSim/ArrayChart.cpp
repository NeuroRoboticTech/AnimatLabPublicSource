// ArrayChart.cpp: implementation of the ArrayChart class.
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
#include "ArrayChart.h"
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

ArrayChart::ArrayChart()
{
}

ArrayChart::~ArrayChart()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ArrayChart\r\n", "", -1, FALSE, TRUE);}
}

void ArrayChart::Initialize(Simulator *lpSim)
{
	ActivatedItem::Initialize(lpSim);

	m_lColumnCount = m_vArraySize.x;
	m_lRowCount = m_vArraySize.y;
	m_lCurrentRow = m_lRowCount;	//Set this to row count so it will always put back that there are that many rows to go back 

	//First lets determine the buffer space that will be required for this chart.
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

	//Now initialize the data columns.
	int iCount = m_aryDataColumns.GetSize();
	for(int iCol=0; iCol<iCount; iCol++)
		m_aryDataColumns[iCol]->Initialize(lpSim);
}

void ArrayChart::ReInitialize(Simulator *lpSim)
{
}

void ArrayChart::StepSimulation(Simulator *lpSim)
{
	if(!(lpSim->TimeSlice()%m_iCollectInterval))
	{
		int iCount = m_aryDataColumns.GetSize();
		for(int iCol=0; iCol<iCount; iCol++)
			m_aryDataColumns[iCol]->StepSimulation(lpSim, this);
	}
}

void ArrayChart::Load(Simulator *lpSim, CStdXml &oXml)
{
	DataChart::Load(lpSim, oXml);

	oXml.IntoElem();  //Into DataChart Element	
	Std_LoadPoint(oXml, "Size", m_vArraySize);
	oXml.OutOfElem(); //OutOf DataChart Element
}


void ArrayChart::LoadChart(Simulator *lpSim, CStdXml &oXml)
{
	DataChart::LoadChart(lpSim, oXml);

	oXml.IntoElem();
	Std_LoadPoint(oXml, "Size", m_vArraySize);
	oXml.OutOfElem();
}

	}			//Charting
}				//AnimatSim

