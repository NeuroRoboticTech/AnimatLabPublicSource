#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT ScriptProcessor : public AnimatBase
	{
	protected:

	public:
		ScriptProcessor(void);
		virtual ~ScriptProcessor(void);

		virtual void StepPhysicsEngine() {};
		virtual void StepNeuralEngine() {};
	};

}