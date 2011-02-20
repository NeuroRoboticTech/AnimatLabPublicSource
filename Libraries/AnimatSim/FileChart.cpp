// FileChart.cpp: implementation of the FileChart class.
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
#include "FileChart.h"
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

FileChart::FileChart()
{
}

FileChart::~FileChart()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of FileChart\r\n", "", -1, FALSE, TRUE);}
}

void FileChart::Initialize(Simulator *lpSim)
{
	DataChart::Initialize(lpSim);

	//Now open the file stream for output.
	oStream.open(AnimatSim::GetFilePath(m_strProjectPath, m_strOutputFilename).c_str());
}


void FileChart::Deactivate(Simulator *lpSim)
{
	DataChart::Deactivate(lpSim);

	//Temporarry. We need to save the output file when deactivating.
	//Later we can change this to make it more flexible. Mabey periodically
	//dumping out the buffer once it gets full.
	SaveOutput(lpSim);
}

void FileChart::ResetSimulation(Simulator *lpSim)
{
	DataChart::ResetSimulation(lpSim);

	//re-open the file stream for output.
	oStream.open(AnimatSim::GetFilePath(m_strProjectPath, m_strOutputFilename).c_str());
}

void FileChart::Load(Simulator *lpSim, CStdXml &oXml)
{
	DataChart::Load(lpSim, oXml);

	oXml.IntoElem();  //Into FileChart Element

	m_strOutputFilename = oXml.GetChildString("OutputFilename");
	if(Std_IsBlank(m_strOutputFilename)) 
		THROW_ERROR(Al_Err_lFilenameBlank, Al_Err_strFilenameBlank);

	oXml.OutOfElem(); //OutOf FileChart Element
}

void FileChart::SaveOutput(Simulator *lpSim)
{
	long lRow, lCol, lIndex;

	//Add the time columns
	oStream << "TimeSlice\t" << "Time\t";


	//Lets go through and save the column names
	lIndex = m_aryDataColumns.GetSize();
	for(lCol=0; lCol<lIndex; lCol++)
	{
		m_aryDataColumns[lCol]->SaveColumnNames(oStream);

		if(lCol < (lIndex-1)) oStream << "\t";
	}
	oStream << "\n";


	//Lets go through and dump the buffer
	long lTimeSlice=0;
	float fltTime = 0;
	for(lRow=0; lRow<m_lRowCount; lRow++)
	{
		lTimeSlice = m_lStartSlice + (lRow * m_iCollectInterval);
		fltTime = lTimeSlice * lpSim->TimeStep();

		//Add the time
		oStream << lTimeSlice << "\t" << fltTime << "\t";

		for(lCol=0; lCol<m_lColumnCount; lCol++)
		{
			lIndex = (lRow*m_lColumnCount) + lCol;

			oStream << m_aryDataBuffer[lIndex];

			if(lCol < (m_lColumnCount-1)) oStream << "\t";
		}

		oStream << "\n";
	}

	oStream.close();

	//Now get rid of the buffer.
	if(m_aryDataBuffer) delete[] m_aryDataBuffer;
	m_aryDataBuffer = NULL;
}

	}			//Charting
}				//AnimatSim

