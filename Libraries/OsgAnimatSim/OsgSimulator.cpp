// OsgSimulator.cpp: implementation of the OsgSimulator class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include <osgUtil/Version>

#include "OsgMovableItem.h"
#include "OsgOrganism.h"
#include "OsgStructure.h"
#include "OsgSimulator.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"
#include "OsgMeshMinVertexDistanceVisitor.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


OsgSimulator::OsgSimulator()
{
	m_grpScene = NULL;
	m_vsWinMgr = NULL;
	m_vsWinMgr = new OsgSimulationWindowMgr;
	m_lpWinMgr = m_vsWinMgr;
	m_lpWinMgr->SetSystemPointers(this, NULL, NULL, NULL, true);
	m_dblTotalStepTime = 0;
	m_lStepTimeCount = 0;
	m_dblTotalStepTime= 0;
	m_lStepTimeCount = 0;
	m_lpMeshMgr = NULL;
	m_osgAlphafunc = NULL;
    m_lpMouseSpring = new OsgMouseSpring;
    m_lpMouseSpring->Initialize();
    m_lpMatrixUtil = NULL;
}

OsgSimulator::~OsgSimulator()
{

try
{
	if(m_lpMeshMgr)
	{
		delete m_lpMeshMgr;
		m_lpMeshMgr = NULL;
	}

	m_bShuttingDown = true;

    if(m_lpMouseSpring)
        delete m_lpMouseSpring;

    if(m_lpMatrixUtil)
        delete m_lpMatrixUtil;

	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Simulator\r\n", "", -1, false, true);}
}

void OsgSimulator::AlphaThreshold(float fltValue)
{
	Simulator::AlphaThreshold(fltValue);

	if(m_osgAlphafunc)
		    m_osgAlphafunc->setFunction(osg::AlphaFunc::GEQUAL, m_fltAlphaThreshold);
}

void OsgSimulator::Reset()
{
	Simulator::Reset();

	if(m_osgCmdMgr.valid())
		m_osgCmdMgr.release();

	if(m_lpMeshMgr)
	{
		delete m_lpMeshMgr;
		m_lpMeshMgr = NULL;
	}

}

void OsgSimulator::ToggleSimulation()
{
	if(m_bPaused)
		SimStarting();
	else
		SimPausing();

	m_bPaused = !m_bPaused;
}

void OsgSimulator::StopSimulation()
{
	SimStopping();
	if(!m_bPaused)
		ToggleSimulation();
	m_bSimRunning = false;
}


osg::NotifySeverity OsgSimulator::ConvertTraceLevelToOSG()
{
	int iLevel = Std_GetTraceLevel();

	switch (iLevel)
	{
	case 0:
		return osg::NotifySeverity::FATAL;
	case 10:
		return osg::NotifySeverity::WARN;
	case 20:
		return osg::NotifySeverity::INFO;
	case 30:
		return osg::NotifySeverity::DEBUG_INFO;
	case 40:
		return osg::NotifySeverity::DEBUG_FP;
	default:
		return osg::NotifySeverity::WARN;
	}
}

//Timer Methods
unsigned long long OsgSimulator::GetTimerTick()
{
	m_lLastTickTaken = osg::Timer::instance()->tick();
	return m_lLastTickTaken;
}

double OsgSimulator::TimerDiff_n(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_n(lStart, lEnd);}

double OsgSimulator::TimerDiff_u(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_u(lStart, lEnd);}

double OsgSimulator::TimerDiff_m(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_m(lStart, lEnd);}

double OsgSimulator::TimerDiff_s(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_s(lStart, lEnd);}

void OsgSimulator::MicroSleep(unsigned int iMicroTime)
{OpenThreads::Thread::microSleep(iMicroTime);}

void OsgSimulator::WriteToConsole(std::string strMessage)
{
	osg::notify(osg::NOTICE) << strMessage << std::endl;
}

void OsgSimulator::Initialize(int argc, const char **argv)
{
#ifndef WIN32
    std::string strVersion = osgUtilGetVersion();
    strVersion = "/usr/lib/osgPlugins-" + strVersion;
	osgDB::setLibraryFilePathList(strVersion);
#endif

	InitializeStructures();

	m_oDataChartMgr.Initialize();
	m_oExternalStimuliMgr.Initialize();
	if(m_lpSimRecorder) m_lpSimRecorder->Initialize();

	//realize the osg viewer
	m_vsWinMgr->Realize();
}

void OsgSimulator::UpdateSimulationWindows()
{
	m_bStopSimulation = !m_vsWinMgr->Update();
}

void OsgSimulator::ShutdownSimulation()
{
	SimStopping();
	m_bForceSimulationStop = true;
}

bool OsgSimulator::PauseSimulation()
{
	SimPausing();
	m_bPaused = true;
	return true;
}

bool OsgSimulator::StartSimulation()
{
	m_lStartSimTick = GetTimerTick();

	SimStarting();
	m_bSimRunning = true;
	m_bPaused = false;
	return true;
}

float *OsgSimulator::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "FRAMEDT")
	//	lpData = &m_fltFrameDt;
	//else
	//{
		lpData = Simulator::GetDataPointer(strDataType);
		if(!lpData)
			THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);
	//}

	return lpData;
}

void OsgSimulator::SnapshotStopFrame()
{
	if(m_lpSimStopPoint) delete m_lpSimStopPoint;
	m_lpSimStopPoint = dynamic_cast<KeyFrame *>(CreateObject("AnimatLab", "KeyFrame", "Snapshot"));
	if(!m_lpSimStopPoint)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "KeyFrame");

	m_lpSimStopPoint->StartSlice(m_lTimeSlice);
	m_lpSimStopPoint->EndSlice(m_lTimeSlice);
	m_lpSimStopPoint->Activate();
}

OsgSimulator *OsgSimulator::ConvertSimulator(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	OsgSimulator *lpVsSim = dynamic_cast<OsgSimulator *>(lpSim);

	if(!lpVsSim)
		THROW_ERROR(Osg_Err_lUnableToConvertToVsSimulator, Osg_Err_strUnableToConvertToVsSimulator);

	return lpVsSim;
}

void OsgSimulator::Save(std::string strFile)
{
	std::string strOsgFile = strFile + ".osg";

	osgDB::writeNodeFile(*OSGRoot(), strOsgFile.c_str());
}


}			//OsgAnimatSim
