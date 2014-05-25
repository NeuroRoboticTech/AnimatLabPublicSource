#pragma once

namespace AnimatSimPy
{

	class ANIMATSIMPY_PORT ScriptProcessorPy : public AnimatSim::ScriptProcessor
	{
	protected:
		std::string m_strInitPy;
		std::string m_strResetSimPy;
		std::string m_strStepPhysicsEnginePy;
		std::string m_strStepNeuralEnginePy;
		std::string m_strStepSimulationPy;
		std::string m_strKillPy;
		std::string m_strSimStartingPy;
		std::string m_strSimPausingPy;
		std::string m_strSimStoppingPy;
		
		virtual void ExecutePythonScript(const std::string &strPy);

	public:
		ScriptProcessorPy(void);
		virtual ~ScriptProcessorPy(void);

		virtual void InitPy(std::string strVal);
		virtual std::string InitPy();

		virtual void ResetSimPy(std::string strVal);
		virtual std::string ResetSimPy();

		virtual void StepPhysicsEnginePy(std::string strVal);
		virtual std::string StepPhysicsEnginePy();

		virtual void StepNeuralEnginePy(std::string strVal);
		virtual std::string StepNeuralEnginePy();

		virtual void StepSimulationPy(std::string strVal);
		virtual std::string StepSimulationPy();

		virtual void KillPy(std::string strVal);
		virtual std::string KillPy();

		virtual void SimStartingPy(std::string strVal);
		virtual std::string SimStartingPy();

		virtual void SimPausingPy(std::string strVal);
		virtual std::string SimPausingPy();

		virtual void SimStoppingPy(std::string strVal);
		virtual std::string SimStoppingPy();

		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void Kill(bool bState = true);
		virtual void StepPhysicsEngine();
		virtual void StepNeuralEngine();
		virtual void StepSimulation();

		virtual void SimStarting();
		virtual void SimPausing();
		virtual void SimStopping();
		virtual void Load(StdUtils::CStdXml &oXml);
	};

}