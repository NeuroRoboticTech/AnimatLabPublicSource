
#pragma once


namespace VortexAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	VortexAnimatSim::Environment::Bodies

		\brief	Body part classes that use the vortex physics engine. 
		**/
		namespace Bodies
		{

			class VORTEX_PORT VsBox : public AnimatSim::Environment::Bodies::Box, public VsRigidBody
			{
			protected:

			public:
				VsBox();
				virtual ~VsBox();

				virtual void CreateParts();
				virtual void CreateJoints();
				virtual void Resize();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
