
#pragma once

#include "VsMaterialPair.h"
#include "VsHud.h"
#include "VsIntersectionEvent.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsSimulationWindowMgr.h"

/**
\namespace	VortexAnimatSim

\brief	Classes for implementing the cm-labs vortex physics engine for AnimatLab. 
**/
namespace VortexAnimatSim
{

	class VORTEX_PORT VsSimulator : public AnimatSim::Simulator  
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

		virtual AnimatSim::Recording::SimulationRecorder *CreateSimulationRecorder();
		virtual void SnapshotStopFrame();

		//helper functions
		void InitializeVortex(int argc, const char **argv);
		void InitializeVortexViewer(int argc, const char **argv);

		virtual void StepSimulation();
		virtual void StepVideoFrame();
		virtual void SimulateEnd();

		virtual void GetPositionAndRotationFromMatrix(float *aryMatrix, CStdFPoint &vPos, CStdFPoint &vRot);

	public:
		VsSimulator();
		virtual ~VsSimulator();

		Vx::VxUniverse* Universe();		
		Vx::VxFrame* Frame();
		VsRigidBody *TrackBody();
		osg::Group *OSGRoot() {return m_grpScene.get();};
		osgManipulator::CommandManager *OsgCmdMgr() {return m_osgCmdMgr.get();};

#pragma region CreateMethods

		virtual Vx::VxTriangleMesh *CreatTriangleMeshFromOsg(osg::Node *osgNode);
		virtual Vx::VxConvexMesh *CreateConvexMeshFromOsg(osg::Node *osgNode);
		virtual void GenerateCollisionMeshFile(string strOriginalMeshFile, string strCollisionMeshFile);

#pragma endregion

#pragma region MutatorOverrides

		virtual void LinearCompliance(float fltVal, BOOL bUseScaling = TRUE);
		virtual void AngularCompliance(float fltVal, BOOL bUseScaling = TRUE);
		virtual void LinearDamping(float fltVal, BOOL bUseScaling = TRUE);
		virtual void AngularDamping(float fltVal, BOOL bUseScaling = TRUE);
		virtual void LinearKineticLoss(float fltVal);
		virtual void AngularKineticLoss(float fltVal);
		virtual void PhysicsTimeStep(float fltVal);
		virtual void Gravity(float fltVal, BOOL bUseScaling = TRUE);
		virtual int GetMaterialID(string strID);

#pragma endregion

		virtual float *GetDataPointer(string strDataType);

		virtual void Reset(); //Resets the entire application back to the default state 
		virtual void ResetSimulation(); //Resets the current simulation back to time 0.0

		virtual void Initialize(int argc, const char **argv);
		virtual void ShutdownSimulation();
		virtual void ToggleSimulation();
		virtual void StopSimulation();
		virtual BOOL StartSimulation();
		virtual BOOL PauseSimulation();
		virtual void Save(string strFile);

		static VsSimulator *ConvertSimulator(Simulator *lpSim);
	};

}			//VortexAnimatSim
