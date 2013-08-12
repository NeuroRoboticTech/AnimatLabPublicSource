
#pragma once

#include "VsMaterialType.h"
#include "VsIntersectionEvent.h"

/**
\namespace	VortexAnimatSim

\brief	Classes for implementing the cm-labs vortex physics engine for AnimatLab. 
**/
namespace VortexAnimatSim
{

	class VORTEX_PORT VsSimulator : public OsgSimulator
	{
	protected:
		//Vortex Universe
		VxUniverse *m_uUniverse;
		
		//Vortex Frame
		VxFrame *m_vxFrame;		

		VsIntersectionEvent m_vsIntersect;

		double m_dblTotalVortexStepTime;
		long m_lStepVortexTimeCount;

		virtual AnimatSim::Recording::SimulationRecorder *CreateSimulationRecorder();

		//helper functions
		void InitializeVortex(int argc, const char **argv);
		void InitializeVortexViewer(int argc, const char **argv);
		void SetSimulationStabilityParams();
		
		virtual void StepSimulation();
		virtual void SimulateEnd();
	
	public:
		VsSimulator();
		virtual ~VsSimulator();

		Vx::VxUniverse* Universe();		
		Vx::VxFrame* Frame();

#pragma region CreateMethods

		virtual Vx::VxTriangleMesh *CreatTriangleMeshFromOsg(osg::Node *osgNode);
		virtual Vx::VxConvexMesh *CreateConvexMeshFromOsg(osg::Node *osgNode);
		virtual void GenerateCollisionMeshFile(string strOriginalMeshFile, string strCollisionMeshFile, float fltScaleX, float fltScaleY, float fltScaleZ);
		virtual void ConvertV1MeshFile(string strOriginalMeshFile, string strNewMeshFile, string strTexture);

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
		virtual int GetMaterialID(string strID);

#pragma endregion
		
#pragma region HelperMethods

		virtual void GetPositionAndRotationFromD3DMatrix(float (&aryTransform)[4][4], CStdFPoint &vPos, CStdFPoint &vRot);

#pragma endregion

		virtual void Reset(); //Resets the entire application back to the default state 
		virtual void ResetSimulation(); //Resets the current simulation back to time 0.0
		virtual void Initialize(int argc, const char **argv);
	};

}			//VortexAnimatSim
