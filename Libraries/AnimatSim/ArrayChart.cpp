/**
\file	ArrayChart.cpp

\brief	Implements the array chart class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
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
/**
\brief	Default constructor.

\author	dcofer
\date	3/18/2011
**/
ArrayChart::ArrayChart()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
ArrayChart::~ArrayChart()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ArrayChart\r\n", "", -1, FALSE, TRUE);}
}

string ArrayChart::Type() {return "ArrayChart";}

/**
\brief	Sets the Current row.

\detals We need to disable the ability to reset the current row. It should always be the same size as the array.

\author	dcofer
\date	3/18/2011

\param	iVal	The new current row value. 
**/
void ArrayChart::CurrentRow(long iVal) {}

void ArrayChart::Initialize()
{
	ActivatedItem::Initialize();

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
		m_aryDataColumns[iCol]->Initialize();
}

void ArrayChart::ReInitialize()
{
}

void ArrayChart::StepSimulation()
{
	if(!(m_lpSim->TimeSlice()%m_iCollectInterval))
	{
		int iCount = m_aryDataColumns.GetSize();
		for(int iCol=0; iCol<iCount; iCol++)
			m_aryDataColumns[iCol]->StepSimulation();
	}
}

void ArrayChart::Load(CStdXml &oXml)
{
	DataChart::Load(oXml);

	oXml.IntoElem();  //Into DataChart Element	
	Std_LoadPoint(oXml, "Size", m_vArraySize);
	oXml.OutOfElem(); //OutOf DataChart Element
}


	}			//Charting
}				//AnimatSim

