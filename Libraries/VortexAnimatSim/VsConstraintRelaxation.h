// VsConstraintRelaxation.h: interface for the VsConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsConstraintRelaxation : public AnimatSim::Environment::ConstraintRelaxation
		{
		protected:
			virtual void SetRelaxationProperties();

		public:
			VsConstraintRelaxation();
			virtual ~VsConstraintRelaxation();

            virtual void Initialize();
		};

	}			// Visualization
}				//VortexAnimatSim
