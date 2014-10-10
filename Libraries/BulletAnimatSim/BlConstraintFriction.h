// BlConstraintFriction.h: interface for the BlConstraintFriction class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{

		class BULLET_PORT BlConstraintFriction : public AnimatSim::Environment::ConstraintFriction
		{
		protected:
            virtual void SetFrictionProperties();

		public:
			BlConstraintFriction();
			virtual ~BlConstraintFriction();

            virtual void Initialize();
		};

	}			// Visualization
}				//BulletAnimatSim
