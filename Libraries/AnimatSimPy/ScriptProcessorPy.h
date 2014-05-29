#pragma once

namespace AnimatSimPy
{

	class ANIMATSIMPY_PORT ScriptProcessorPy : public AnimatSim::ScriptProcessor
	{
	protected:
		std::string m_strInitPy;
		std::string m_strResetSimPy;
		std::string m_strBeforeStepPhysicsEnginePy;
		std::string m_strAfterStepPhysicsEnginePy;
		std::string m_strBeforeStepNeuralEnginePy;
		std::string m_strAfterStepNeuralEnginePy;
		std::string m_strKillPy;
		std::string m_strSimStartingPy;
		std::string m_strSimPausingPy;
		std::string m_strSimStoppingPy;
		
		virtual bool ExecutePythonScript(const std::string &strPy, bool bThrowError = true);

	public:
		ScriptProcessorPy(void);
		virtual ~ScriptProcessorPy(void);

		virtual void InitPy(std::string strVal);
		virtual std::string InitPy();

		virtual void ResetSimPy(std::string strVal);
		virtual std::string ResetSimPy();

		virtual void BeforeStepPhysicsEnginePy(std::string strVal);
		virtual std::string BeforeStepPhysicsEnginePy();

		virtual void AfterStepPhysicsEnginePy(std::string strVal);
		virtual std::string AfterStepPhysicsEnginePy();

		virtual void BeforeStepNeuralEnginePy(std::string strVal);
		virtual std::string BeforeStepNeuralEnginePy();

		virtual void AfterStepNeuralEnginePy(std::string strVal);
		virtual std::string AfterStepNeuralEnginePy();

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

		virtual void BeforeStepPhysicsEngine();
		virtual void AfterStepPhysicsEngine();
		virtual void BeforeStepNeuralEngine();
		virtual void AfterStepNeuralEngine();

		virtual void SimStarting();
		virtual void SimPausing();
		virtual void SimStopping();
		virtual void Load(StdUtils::CStdXml &oXml);
	};

}