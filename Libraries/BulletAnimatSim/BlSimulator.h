
#pragma once

#include "BlMaterialType.h"
#include "BlAnimatCollisionDispatcher.h"

/**
\namespace	BulletAnimatSim

\brief	Classes for implementing the cm-labs vortex physics engine for AnimatLab. 
**/
namespace BulletAnimatSim
{

	class BULLET_PORT BlSimulator : public OsgSimulator
	{
	protected:
        btDefaultCollisionConfiguration *m_lpCollisionConfiguration;
        BlAnimatCollisionDispatcher *m_lpDispatcher;
        btConstraintSolver *m_lpSolver;
        btBroadphaseInterface *m_lpBroadPhase;
        btDiscreteDynamicsWorld *m_lpDynamicsWorld;
        osgbCollision::GLDebugDrawer m_dbgDraw;

        std::list<FluidPlane *> m_aryFluidPlanes;

        bool m_bDrawDebug;

		double m_dblTotalVortexStepTime;
		long m_lStepVortexTimeCount;

		virtual AnimatSim::Recording::SimulationRecorder *CreateSimulationRecorder();

		//helper functions
		void InitializeBullet(int argc, const char **argv);
		void InitializeBulletViewer(int argc, const char **argv);
		void SetSimulationStabilityParams();
		
		virtual void StepSimulation();
		virtual void SimulateEnd();
	
	public:
		BlSimulator();
		virtual ~BlSimulator();

        btDefaultCollisionConfiguration *CollisionConfig() {return m_lpCollisionConfiguration;};
        btCollisionDispatcher *Dispatcher() {return m_lpDispatcher;};
        btConstraintSolver *Solver() {return m_lpSolver;};
        btBroadphaseInterface *BroadPhase() {return m_lpBroadPhase;}
        btDiscreteDynamicsWorld *DynamicsWorld() {return m_lpDynamicsWorld;};

#pragma region CreateMethods

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

		virtual void Gravity(float fltVal, bool bUseScaling = true);

#pragma endregion
		
#pragma region HelperMethods

		virtual void GetPositionAndRotationFromD3DMatrix(float (&aryTransform)[4][4], CStdFPoint &vPos, CStdFPoint &vRot);
        
#pragma endregion

#pragma region FluidMethods

        virtual void AddFluidPlane(FluidPlane *lpPlane);
        virtual void RemoveFluidPlane(FluidPlane *lpPlane);
        virtual bool HasFluidPlane(FluidPlane *lpPlane);
        virtual void SortFluidPlanes();
        virtual FluidPlane *FindFluidPlaneForDepth(float fltDepth);

#pragma endregion

		virtual void Reset(); //Resets the entire application back to the default state 
		virtual void ResetSimulation(); //Resets the current simulation back to time 0.0
		virtual void Initialize(int argc, const char **argv);
	};

}			//BulletAnimatSim
