
#pragma once

#include "OsgMovableItem.h"
#include "OsgMeshMgr.h"
#include "OsgHud.h"
#include "OsgMouseSpring.h"
#include "OsgCameraManipulator.h"
#include "OsgSimulationWindowMgr.h"

/**
\namespace	OsgAnimatSim

\brief	Classes for implementing the cm-labs vortex physics engine for AnimatLab. 
**/
namespace OsgAnimatSim
{

	class ANIMAT_OSG_PORT OsgSimulator : public AnimatSim::Simulator  
	{
	protected:
		OsgSimulationWindowMgr *m_vsWinMgr;

		//osg group node for the main scene
		osg::ref_ptr<osg::MatrixTransform> m_grpScene;

		//Command manager for gripper manipulators in the scene.
		osg::ref_ptr<osgManipulator::CommandManager> m_osgCmdMgr;

		osg::AlphaFunc *m_osgAlphafunc;

		double m_dblTotalStepTime;
		long m_lStepTimeCount;

		OsgMeshMgr *m_lpMeshMgr;

		virtual void SnapshotStopFrame();

		virtual void UpdateSimulationWindows();

		osg::NotifySeverity ConvertTraceLevelToOSG();

		osg::ref_ptr<osg::Node> m_Spline;
	
        OsgMouseSpring *m_lpMouseSpring;

        OsgMatrixUtil *m_lpMatrixUtil;

	public:
		OsgSimulator();
		virtual ~OsgSimulator();

		OsgMovableItem *TrackBody();
		osg::MatrixTransform *OSGRoot() {return m_grpScene.get();};
		osgManipulator::CommandManager *OsgCmdMgr() {return m_osgCmdMgr.get();};
		OsgMeshMgr *MeshMgr() 
		{
			if(!m_lpMeshMgr)
				m_lpMeshMgr = new OsgMeshMgr();

			return m_lpMeshMgr;
		};
        OsgMouseSpring *MouseSpring() {return m_lpMouseSpring;};
		
#pragma region HelperMethods

		//Timer Methods
		virtual unsigned long long GetTimerTick();
		virtual double TimerDiff_n(unsigned long long lStart, unsigned long long lEnd);
		virtual double TimerDiff_u(unsigned long long lStart, unsigned long long lEnd);
		virtual double TimerDiff_m(unsigned long long lStart, unsigned long long lEnd);
		virtual double TimerDiff_s(unsigned long long lStart, unsigned long long lEnd);
		virtual void MicroSleep(unsigned int iMicroTime);

		virtual void WriteToConsole(string strMessage);

#pragma endregion

		virtual void AlphaThreshold(float fltValue);

		virtual float *GetDataPointer(const string &strDataType);

		virtual void Reset(); //Resets the entire application back to the default state 

		virtual void Initialize(int argc, const char **argv);
		virtual void ShutdownSimulation();
		virtual void ToggleSimulation();
		virtual void StopSimulation();
		virtual bool StartSimulation();
		virtual bool PauseSimulation();
		virtual void Save(string strFile);

		static OsgSimulator *ConvertSimulator(Simulator *lpSim);
	};

}			//OsgAnimatSim
