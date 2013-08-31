
#pragma once

#include "BlMaterialType.h"
#include "BlIntersectionEvent.h"

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
        btCollisionDispatcher *m_lpDispatcher;
        btConstraintSolver *m_lpSolver;
        btBroadphaseInterface *m_lpBroadPhase;
        btDiscreteDynamicsWorld *m_lpDynamicsWorld;
        osgbCollision::GLDebugDrawer m_dbgDraw;

        bool m_bDrawDebug;

        //FIX PHYSICS
		//BlIntersectionEvent m_vsIntersect;

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
		BlSimulator();
		virtual ~BlSimulator();

        btDefaultCollisionConfiguration *CollisionConfig() {return m_lpCollisionConfiguration;};
        btCollisionDispatcher *Dispatcher() {return m_lpDispatcher;};
        btConstraintSolver *Solver() {return m_lpSolver;};
        btBroadphaseInterface *BroadPhase() {return m_lpBroadPhase;}
        btDiscreteDynamicsWorld *DynamicsWorld() {return m_lpDynamicsWorld;};

#pragma region CreateMethods

        //FIX PHYSICS
		//virtual Vx::VxTriangleMesh *CreatTriangleMeshFromOsg(osg::Node *osgNode);
		//virtual Vx::VxConvexMesh *CreateConvexMeshFromOsg(osg::Node *osgNode);
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

}			//BulletAnimatSim
