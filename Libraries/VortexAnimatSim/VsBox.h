
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

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();
                virtual void CalculateEstimatedMassAndVolume();

			public:
				VsBox();
				virtual ~VsBox();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
