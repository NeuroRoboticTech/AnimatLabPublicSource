// VsSimulator.h: interface for the VsSimulator class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

#include "VsMaterialPair.h"
#include "VsHudItem.h"
#include "VsHud.h"
#include "VsIntersectionEvent.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsSimulationWindowMgr.h"

namespace VortexAnimatSim
{

	class VORTEX_PORT VsSimulator : public Simulator  
	{
	protected:

		osg::Timer_t m_iLastFrame;
		osg::Timer_t m_iCurrentFrame;
		float m_fltFrameDt;

		VsSimulationWindowMgr *m_vsWinMgr;

		//osg group node for the main scene
		osg::ref_ptr<osg::Group> m_grpScene;

		//Command manager for gripper manipulators in the scene.
		osg::ref_ptr<osgManipulator::CommandManager> m_osgCmdMgr;

		//Vortex Universe
		VxUniverse *m_uUniverse;
		
		//Vortex Frame
		VxFrame *m_vxFrame;		
		
		VsIntersectionEvent m_vsIntersect;

		long m_lTimer;

		virtual AnimatSim::Recording::SimulationRecorder *CreateSimulationRecorder();
		virtual void SnapshotStopFrame();

		//helper functions
		void InitializeVortex(int argc, const char **argv);
		void InitializeVortexViewer(int argc, const char **argv);
		void CreateLights();

	public:
		VsSimulator();
		virtual ~VsSimulator();

		Vx::VxUniverse* Universe();		
		Vx::VxFrame* Frame();
		VsRigidBody *TrackBody();
		osg::Group *OSGRoot() {return m_grpScene.get();};
		osgManipulator::CommandManager *OsgCmdMgr() {return m_osgCmdMgr.get();};

		virtual void PlaybackRate(float fltVal);

		virtual float *GetDataPointer(string strDataType);

		virtual void Reset(); //Resets the entire application back to the default state 
		virtual void ResetSimulation(); //Resets the current simulation back to time 0.0

		virtual void Initialize(int argc, const char **argv);
		virtual void StepSimulation();
		virtual void Simulate();
		virtual void ShutdownSimulation();
		virtual void ToggleSimulation();
		virtual void StopSimulation();
		virtual BOOL StartSimulation();
		virtual BOOL PauseSimulation();

		static VsSimulator *ConvertSimulator(Simulator *lpSim);
	};

}			//VortexAnimatSim
