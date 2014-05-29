#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT ScriptProcessor : public AnimatBase
	{
	protected:

	public:
		ScriptProcessor(void);
		virtual ~ScriptProcessor(void);

		virtual void BeforeStepPhysicsEngine() {};
		virtual void AfterStepPhysicsEngine() {};
		virtual void BeforeStepNeuralEngine() {};
		virtual void AfterStepNeuralEngine() {};
		virtual void BeforeStepSimulation() {};
		virtual void AfterStepSimulation() {};
	};

	void ANIMAT_PORT SetLastScriptError(std::string strError);
	std::string ANIMAT_PORT GetLastScriptError();
}