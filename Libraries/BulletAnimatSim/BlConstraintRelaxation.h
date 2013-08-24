// BlConstraintRelaxation.h: interface for the BlConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{

		class BULLET_PORT BlConstraintRelaxation : public AnimatSim::Environment::ConstraintRelaxation
		{
		protected:
			virtual void SetRelaxationProperties();

		public:
			BlConstraintRelaxation();
			virtual ~BlConstraintRelaxation();

            virtual void Initialize();
		};

	}			// Visualization
}				//BulletAnimatSim
