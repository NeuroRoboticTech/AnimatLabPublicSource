/**
\file	FileChart.cpp

\brief	Implements the file chart class.
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
#include "FileChart.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
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
FileChart::FileChart()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
FileChart::~FileChart()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of FileChart\r\n", "", -1, FALSE, TRUE);}
}

string FileChart::Type() {return "FileChart";}

/**
\brief	Gets the output filename.

\author	dcofer
\date	3/18/2011

\return	Output file name.
**/
string FileChart::OutputFilename() {return m_strOutputFilename;}

/**
\brief	Sets the Output filename.

\author	dcofer
\date	3/18/2011

\param	strVal	The new name. 
**/
void FileChart::OutputFilename(string strVal) 
{
	if(Std_IsBlank(strVal)) 
		THROW_ERROR(Al_Err_lFilenameBlank, Al_Err_strFilenameBlank);

	m_strOutputFilename = strVal;
}

void FileChart::Initialize()
{
	DataChart::Initialize();

	//Now open the file stream for output.
	oStream.open(AnimatSim::GetFilePath(m_strProjectPath, m_strOutputFilename).c_str());
}


void FileChart::Deactivate()
{
	DataChart::Deactivate();

	//Temporarry. We need to save the output file when deactivating.
	//Later we can change this to make it more flexible. Mabey periodically
	//dumping out the buffer once it gets full.
	SaveOutput();
}

void FileChart::ResetSimulation()
{
	DataChart::ResetSimulation();

	//re-open the file stream for output.
	oStream.open(AnimatSim::GetFilePath(m_strProjectPath, m_strOutputFilename).c_str());
}

void FileChart::Load(CStdXml &oXml)
{
	DataChart::Load(oXml);

	oXml.IntoElem();  //Into FileChart Element

	OutputFilename(oXml.GetChildString("OutputFilename"));

	oXml.OutOfElem(); //OutOf FileChart Element
}

/**
\brief	Saves the data to the specified output file in a tsv format.

\author	dcofer
\date	3/18/2011
**/
void FileChart::SaveOutput()
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
		fltTime = lTimeSlice * m_lpSim->TimeStep();

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

