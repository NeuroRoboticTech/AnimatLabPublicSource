// VsSnapshotKeyFrame.cpp: implementation of the VsSnapshotKeyFrame class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsSnapshotKeyFrame.h"
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

VsSnapshotKeyFrame::VsSnapshotKeyFrame()
{
	m_lpRecorder = NULL;
	m_aryBytes = NULL;
	m_lByteSize = 0;
}

VsSnapshotKeyFrame::~VsSnapshotKeyFrame()
{

try
{
	if(m_lpRecorder)
	{
		//MdtRecorderTerm(m_lpRecorder);
		MeMemoryAPI.destroy(m_lpRecorder);
	}

	if(m_aryBytes) delete m_aryBytes;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsSnapshotKeyFrame\r\n", "", -1, FALSE, TRUE);}
}

void VsSnapshotKeyFrame::GenerateID(Simulator *lpSim)
{
	//Lets verify the slice data is setup correctly.
	Std_IsAboveMin((long) -1, m_lStartSlice, TRUE, "StartSlice");
	m_lEndSlice = m_lStartSlice;

	m_strID = Type();

	char strTail[20];
	sprintf(strTail, "%010d", m_lStartSlice);

	m_strID += strTail;
	m_strID = Std_CheckString(m_strID);
}

void VsSnapshotKeyFrame::Initialize(Simulator *lpSim)
{
	VsSimulator *lpVsSim = VsSimulator::ConvertSimulator(lpSim);

	m_lpRecorder = (MdtRecorder *)MeMemoryAPI.createZeroed(sizeof(MdtRecorder));

	if(!m_lpRecorder)
		THROW_PARAM_ERROR(Vs_Err_lUnableToCreateRecorder, Vs_Err_strUnableToCreateRecorder, "Video Keyframe ID", m_strID);

  MdtRecorderInit(m_lpRecorder, lpVsSim->World(), MeAppRecorderDumpBMP, "frames/mp%02d-", lpVsSim->RenderContext());
}

void VsSnapshotKeyFrame::Activate(Simulator *lpSim)
{
	if(!m_lpRecorder)
		Initialize(lpSim);

	MdtRecorderStoreFrame(m_lpRecorder);

	long lIndex=0;
	if(m_aryBytes) delete m_aryBytes;

	m_lByteSize = lpSim->SnapshotByteSize();
	if(m_lByteSize)
	{
		m_aryBytes = new BYTE[m_lByteSize];
		memset((void *) m_aryBytes, 0, m_lByteSize);
		lpSim->SaveKeyFrameSnapshot(m_aryBytes, lIndex);
	}

	m_bBeenActivated = TRUE;
}

void VsSnapshotKeyFrame::StepSimulation(Simulator *lpSim)
{}

void VsSnapshotKeyFrame::Deactivate(Simulator *lpSim)
{}

void VsSnapshotKeyFrame::EnableVideoPlayback(Simulator *lpSim)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::DisableVideoPlayback(Simulator *lpSim)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::StartVideoPlayback(Simulator *lpSim)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::StopVideoPlayback(Simulator *lpSim)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::PlaybackVideoFrame(Simulator *lpSim)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::StepVideoPlayback(Simulator *lpSim, int iFrameCount)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::RecordVideoFrame(Simulator *lpSim)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::SaveVideo(Simulator *lpSim, string strPath)
{
	THROW_ERROR(Vs_Err_lVideoPlaybackNotSupported, Vs_Err_strVideoPlaybackNotSupported);
}

void VsSnapshotKeyFrame::MakeCurrentFrame(Simulator *lpSim)
{
	VsSimulator *lpVsSim = VsSimulator::ConvertSimulator(lpSim);

	m_lpRecorder->recording = 0;
	m_lpRecorder->integrating = 0;
	m_lpRecorder->frameStep = 1;
	m_lpRecorder->dumping = 0;
	m_lpRecorder->paused = 0;

	MstUniverseStepOrPlay(lpVsSim->Universe(), m_lpRecorder, lpVsSim->PhysicsTimeStep());

	//Now check to see if we need to look at a specific body.
	if(lpVsSim->TrackBody())
	{
		MeVector3 oTrackPos;
		MdtBodyGetPosition(lpVsSim->TrackBody()->BodyID(), oTrackPos);
		RCameraSetLookAt(lpVsSim->RenderContext(), oTrackPos);		
	}

	long lIndex = 0;
	if(m_aryBytes)
		lpVsSim->LoadKeyFrameSnapshot(m_aryBytes, lIndex);

	//Now lets reset the timeslice value
	lpVsSim->TimeSlice(m_lStartSlice);

	if(lpVsSim->SimulationHwnd())
	{
		PostMessage(lpVsSim->SimulationHwnd(), 0x8000, 0, 0); 
		PostMessage(lpVsSim->SimulationHwnd(), 0x8000, 0, 0); 
	}

	//Now lets reset the activated items to the new timeslice.
	//m_oDataChartMgr.ResetTimeSlice(this);
	//m_oExternalStimuliMgr.ResetTimeSlice(this);
	//if(m_lpSimRecorder)
	//	m_lpSimRecorder->ResetTimeSlice(this);
}

void VsSnapshotKeyFrame::Load(Simulator *lpSim, CStdXml &oXml)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);
		
	oXml.IntoElem();  //Into Item Element

	m_strID = Std_CheckString(oXml.GetChildString("ID"));
	if(Std_IsBlank(m_strID))
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_lStartSlice = oXml.GetChildLong("StartSlice");
	Std_IsAboveMin((long) -1, m_lStartSlice, TRUE, "StartSlice");

	m_lEndSlice = m_lStartSlice;

	oXml.OutOfElem(); //OutOf Item Element
}

		}			//KeyFrames
	}				//Recording
}					//VortexAnimatLibrary
