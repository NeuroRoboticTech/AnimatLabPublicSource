
#pragma once

#include "VsMeshMgr.h"
#include "VsMaterialType.h"
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
		VsSimulationWindowMgr *m_vsWinMgr;

		//osg group node for the main scene
		osg::ref_ptr<osg::Group> m_grpScene;

		//Command manager for gripper manipulators in the scene.
		osg::ref_ptr<osgManipulator::CommandManager> m_osgCmdMgr;

		//Vortex Universe
		VxUniverse *m_uUniverse;
		
		//Vortex Frame
		VxFrame *m_vxFrame;		
		
		osg::AlphaFunc *m_osgAlphafunc;

		VsIntersectionEvent m_vsIntersect;

		double m_dblTotalVortexStepTime;
		long m_lStepVortexTimeCount;

		double m_dblTotalStepTime;
		long m_lStepTimeCount;

		VsMeshMgr *m_lpMeshMgr;

		virtual AnimatSim::Recording::SimulationRecorder *CreateSimulationRecorder();
		virtual void SnapshotStopFrame();

		//helper functions
		void InitializeVortex(int argc, const char **argv);
		void InitializeVortexViewer(int argc, const char **argv);
		void SetSimulationStabilityParams();
		
		virtual void StepSimulation();
		virtual void SimulateEnd();

		virtual void UpdateSimulationWindows();

		osg::NotifySeverity ConvertTraceLevelToOSG();

		osg::ref_ptr<osg::Node> m_Spline;
	
	public:
		VsSimulator();
		virtual ~VsSimulator();

		Vx::VxUniverse* Universe();		
		Vx::VxFrame* Frame();
		VsRigidBody *TrackBody();
		osg::Group *OSGRoot() {return m_grpScene.get();};
		osgManipulator::CommandManager *OsgCmdMgr() {return m_osgCmdMgr.get();};
		VsMeshMgr *MeshMgr() 
		{
			if(!m_lpMeshMgr)
				m_lpMeshMgr = new VsMeshMgr();

			return m_lpMeshMgr;
		};

#pragma region CreateMethods

		virtual Vx::VxTriangleMesh *CreatTriangleMeshFromOsg(osg::Node *osgNode);
		virtual Vx::VxConvexMesh *CreateConvexMeshFromOsg(osg::Node *osgNode);
		virtual void GenerateCollisionMeshFile(std::string strOriginalMeshFile, std::string strCollisionMeshFile, float fltScaleX, float fltScaleY, float fltScaleZ);
		virtual void ConvertV1MeshFile(std::string strOriginalMeshFile, std::string strNewMeshFile, std::string strTexture);

#pragma endregion

#pragma region MutatorOverrides

		virtual void StabilityScale(float fltVal);
		virtual void LinearCompliance(float fltVal, bool bUseScaling = true);
		virtual void AngularCompliance(float fltVal, bool bUseScaling = true);
		virtual void LinearDamping(float fltVal, bool bUseScaling = true);
		virtual void AngularDamping(float fltVal, bool bUseScaling = true);
		virtual void LinearKineticLoss(float fltVal, bool bUseScaling = true);
		virtual void AngularKineticLoss(float fltVal, bool bUseScaling = true);

		virtual void PhysicsTimeStep(float fltVal);
		virtual void Gravity(float fltVal, bool bUseScaling = true);
		virtual int GetMaterialID(std::string strID);

#pragma endregion
		
#pragma region HelperMethods

		virtual void GetPositionAndRotationFromD3DMatrix(float (&aryTransform)[4][4], CStdFPoint &vPos, CStdFPoint &vRot);

		virtual void WriteToConsole(std::string strMessage);

#pragma endregion

		virtual void AlphaThreshold(float fltValue);

		virtual float *GetDataPointer(const std::string &strDataType);

		virtual void Reset(); //Resets the entire application back to the default state 
		virtual void ResetSimulation(); //Resets the current simulation back to time 0.0

		virtual void Initialize(int argc, const char **argv);
		virtual void ShutdownSimulation();
		virtual void ToggleSimulation();
		virtual void StopSimulation();
		virtual bool StartSimulation();
		virtual bool PauseSimulation();
		virtual void Save(std::string strFile);

		static VsSimulator *ConvertSimulator(Simulator *lpSim);
	};

}			//VortexAnimatSim
