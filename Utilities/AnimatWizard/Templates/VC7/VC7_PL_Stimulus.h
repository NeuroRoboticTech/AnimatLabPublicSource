// EnablerStimulus.h: interface for the EnablerStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace [*PROJECT_NAME*]
{
	namespace ExternalStimuli
	{

		class [*TAG_NAME*]_PORT [*STIMULUS_NAME*]  : public AnimatLibrary::ExternalStimuli::RigidBodyInputStimulus
		{
		protected:

		public:
			[*STIMULUS_NAME*]();
			virtual ~[*STIMULUS_NAME*]();

			virtual void Activate(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "[*STIMULUS_NAME*]";};
			virtual void Initialize(Simulator *lpSim);
		};

	}			//ExternalStimuli
}				//[*PROJECT_NAME*]
