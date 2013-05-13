// VsConstraintFriction.h: interface for the VsConstraintFriction class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsConstraintFriction : public AnimatSim::Environment::ConstraintFriction
		{
		protected:
			virtual void SetFrictionProperties();

		public:
			VsConstraintFriction();
			virtual ~VsConstraintFriction();

            virtual void Initialize();
		};

	}			// Visualization
}				//VortexAnimatSim
