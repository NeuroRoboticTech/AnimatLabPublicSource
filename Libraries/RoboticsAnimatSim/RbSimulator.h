
#pragma once

#include "RbMaterialType.h"

/**
\namespace	RoboticsAnimatSim

\brief	Classes for implementing the cm-labs vortex physics engine for AnimatLab. 
**/
namespace RoboticsAnimatSim
{

	class ROBOTICS_PORT RbSimulator : public AnimatSim::Simulator
	{
	protected:
		double m_dblTotalVortexStepTime;
		long m_lStepVortexTimeCount;

		virtual AnimatSim::Recording::SimulationRecorder *CreateSimulationRecorder();

		//helper functions
		void InitializeRobotics(int argc, const char **argv);
		void SetSimulationStabilityParams();
		
		virtual void StepSimulation();
		virtual void SimulateEnd();
	
        virtual void UpdateSimulationWindows() {};
        virtual void SnapshotStopFrame() {};

	public:
		RbSimulator();
		virtual ~RbSimulator();

#pragma region CreateMethods

        virtual void GenerateCollisionMeshFile(std::string strOriginalMeshFile, std::string strCollisionMeshFile, float fltScaleX, float fltScaleY, float fltScaleZ) {};
        virtual void ConvertV1MeshFile(std::string strOriginalMeshFile, std::string strNewMeshFile, std::string strTexture) {};

#pragma endregion

#pragma region MutatorOverrides


#pragma endregion
		
#pragma region HelperMethods

        virtual void GetPositionAndRotationFromD3DMatrix(float (&aryTransform)[4][4], CStdFPoint &vPos, CStdFPoint &vRot) {};

        //TODO Need to fix these and get them working again.
		//Timer Methods
        virtual unsigned long long GetTimerTick() {return 0;};
		virtual double TimerDiff_n(unsigned long long lStart, unsigned long long lEnd) {return 0;};
		virtual double TimerDiff_u(unsigned long long lStart, unsigned long long lEnd) {return 0;};
		virtual double TimerDiff_m(unsigned long long lStart, unsigned long long lEnd) {return 0;};
		virtual double TimerDiff_s(unsigned long long lStart, unsigned long long lEnd) {return 0;};
        virtual void MicroSleep(unsigned int iMicroTime) {}
        virtual void WriteToConsole(std::string strMessage) {};

#pragma endregion

#pragma region FluidMethods

#pragma endregion

		virtual void Reset(); //Resets the entire application back to the default state 
		virtual void ResetSimulation(); //Resets the current simulation back to time 0.0
		virtual void Initialize(int argc, const char **argv);
		virtual void ShutdownSimulation();
		virtual void ToggleSimulation();
		virtual void StopSimulation();
		virtual bool StartSimulation();
		virtual bool PauseSimulation();
	};

}			//RoboticsAnimatSim
