#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT ScriptProcessor : public AnimatBase
	{
	protected:

	public:
		ScriptProcessor(void);
		virtual ~ScriptProcessor(void);
						
		static ScriptProcessor *CastToDerived(AnimatBase *lpBase) {return static_cast<ScriptProcessor*>(lpBase);}

		virtual void BeforeStepPhysicsEngine() {};
		virtual void AfterStepPhysicsEngine() {};
		virtual void BeforeStepNeuralEngine() {};
		virtual void AfterStepNeuralEngine() {};
	};

	void ANIMAT_PORT SetLastScriptError(std::string strError);
	std::string ANIMAT_PORT GetLastScriptError();
}