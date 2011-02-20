// VsVideoKeyFrame.cpp: implementation of the VsVideoKeyFrame class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsSnapshotKeyFrame.h"
#include "VsVideoKeyFrame.h"
#include "VsClassFactory.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"

namespace VortexAnimatLibrary
{
	namespace Recording
	{
		namespace KeyFrames
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsVideoKeyFrame::VsVideoKeyFrame()
{
	m_lpRecorder = NULL;
	m_lpSimStopPoint = NULL;
	m_bSingleStepMode = FALSE;
}

VsVideoKeyFrame::~VsVideoKeyFrame()
{

try
{
	m_aryFrameTimeSlices.RemoveAll();

	if(m_lpRecorder)
	{
		//MdtRecorderTerm(m_lpRecorder);
		MeMemoryAPI.destroy(m_lpRecorder);
	}

	if(m_lpSimStopPoint) delete m_lpSimStopPoint;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsVideoKeyFrame\r\n", "", -1, FALSE, TRUE);}
}

void VsVideoKeyFrame::Initialize(Simulator *lpSim)
{
	VsSimulator *lpVsSim = VsSimulator::ConvertSimulator(lpSim);

	m_aryFrameTimeSlices.RemoveAll();

	m_lpRecorder = (MdtRecorder *)MeMemoryAPI.createZeroed(sizeof(MdtRecorder));

	if(!m_lpRecorder)
		THROW_PARAM_ERROR(Vs_Err_lUnableToCreateRecorder, Vs_Err_strUnableToCreateRecorder, "Video Keyframe ID", m_strID);

  MdtRecorderInit(m_lpRecorder, lpVsSim->World(), MeAppRecorderDumpBMP, "C:\\Projects\\Documentation\\Results\\Siggraph Poster\\Robot Frames\\mp%02d-", lpVsSim->RenderContext());
}

void VsVideoKeyFrame::Activate(Simulator *lpSim)
{
	if(!m_lpRecorder)
		THROW_PARAM_ERROR(Vs_Err_lRecorderNotDefined, Vs_Err_strRecorderNotDefined, "Video KeyFrame", m_strID);

	m_aryFrameTimeSlices.RemoveAll();

	lpSim->VideoRecorder(this);
	MdtRecorderStartRecording(m_lpRecorder);

	m_bBeenActivated = TRUE;
}

void VsVideoKeyFrame::RecordVideoFrame(Simulator *lpSim)
{
	VsSimulator *lpVsSim = VsSimulator::ConvertSimulator(lpSim);
	MstUniverseStepOrPlay(lpVsSim->Universe(),  m_lpRecorder, lpVsSim->PhysicsTimeStep());
	m_aryFrameTimeSlices.Add(lpVsSim->TimeSlice());
	int iCount = m_aryFrameTimeSlices.GetSize();
}

void VsVideoKeyFrame::Deactivate(Simulator *lpSim)
{
	lpSim->VideoRecorder(NULL);
	MdtRecorderStopRecording(m_lpRecorder);
}

void VsVideoKeyFrame::EnableVideoPlayback(Simulator *lpSim)
{
	if(m_bBeenActivated)
	{
		//First lets take a snapshot of our current simulator position so 
		//we can return to it after we finish playing back the recording.
		if(m_lpSimStopPoint) delete m_lpSimStopPoint;
		m_lpSimStopPoint = new VsSnapshotKeyFrame;
		m_lpSimStopPoint->StartSlice(lpSim->TimeSlice());
		m_lpSimStopPoint->EndSlice(lpSim->TimeSlice());
		m_lpSimStopPoint->Activate(lpSim);

		lpSim->VideoRecorder(NULL);
		lpSim->VideoPlayback(this);

		MdtRecorderStartSingleStep(m_lpRecorder, 1, 0, 0);
		lpSim->StepSimulation();

		m_bSingleStepMode = TRUE;

		lpSim->VideoSliceCount(m_lStartSlice);
		lpSim->VideoLoops(0);

		//I have no idea why I need to send a message to the subclassed window
		//but this does not work without it. The screen just stays locked on the
		//last image.
		if(lpSim->SimulationHwnd())
		{
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
		}
	}
}

void VsVideoKeyFrame::DisableVideoPlayback(Simulator *lpSim)
{
	if(m_bBeenActivated)
	{
		lpSim->VideoPlayback(NULL);
		lpSim->VideoSliceCount(0);
		lpSim->VideoLoops(0);
		m_bSingleStepMode = FALSE;

		//Now lets put the simulation back into the state it was in when we started playing the recording.
		if(m_lpSimStopPoint)
			m_lpSimStopPoint->MakeCurrentFrame(lpSim);

		if(lpSim->SimulationHwnd())
		{
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
		}
	}
}

void VsVideoKeyFrame::StartVideoPlayback(Simulator *lpSim)
{
	if(m_bBeenActivated)
	{
		MdtRecorderStartPlayback(m_lpRecorder, 1, 0, 0);

		lpSim->VideoSliceCount(m_lStartSlice);
		lpSim->VideoLoops(0);

		lpSim->StartSimulation();

		if(lpSim->SimulationHwnd())
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
	}
}

void VsVideoKeyFrame::StopVideoPlayback(Simulator *lpSim)
{
	if(m_bBeenActivated)
	{
		MdtRecorderStopPlayback(m_lpRecorder);	
		lpSim->PauseSimulation();

		if(lpSim->SimulationHwnd())
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
	}
}

void VsVideoKeyFrame::StepSimulation(Simulator *lpSim)
{
}

void VsVideoKeyFrame::PlaybackVideoFrame(Simulator *lpSim)
{
	VsSimulator *lpVsSim = VsSimulator::ConvertSimulator(lpSim);
	MstUniverseStepOrPlay(lpVsSim->Universe(),  m_lpRecorder, lpVsSim->PhysicsTimeStep());
	//m_aryFrameTimeSlices.Add(lpVsSim->TimeSlice());
}

void VsVideoKeyFrame::StepVideoPlayback(Simulator *lpSim, int iFrameCount)
{
	if(m_bBeenActivated)
	{
		if(m_bSingleStepMode)
			MdtRecorderSingleStep(m_lpRecorder, iFrameCount);
		else
		{
			MdtRecorderStartSingleStep(m_lpRecorder, iFrameCount, 0, 0);
			m_bSingleStepMode = TRUE;
		}

		lpSim->StepSimulation();

		if(lpSim->SimulationHwnd())
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
	}
}

void VsVideoKeyFrame::SaveVideo(Simulator *lpSim, string strPath)
{
	if(m_bBeenActivated)
	{
		char strcPath[512];
		strcpy(strcPath, strPath.c_str());

		MdtRecorderStartPlayback(m_lpRecorder, 2, 1, 0);
		m_lpRecorder->dumpSequential = 1;

		lpSim->VideoSliceCount(m_lStartSlice);
		lpSim->VideoLoops(0);

		lpSim->StartSimulation();

		if(lpSim->SimulationHwnd())
			PostMessage(lpSim->SimulationHwnd(), 0x8000, 0, 0); 
	}
}

void VsVideoKeyFrame::MakeCurrentFrame(Simulator *lpSim)
{
	THROW_ERROR(Vs_Err_lMoveToKeyFrameNotSupported, Vs_Err_strMoveToKeyFrameNotSupported);
}

		}			//KeyFrames
	}				//Recording
}					//VortexAnimatLibrary



