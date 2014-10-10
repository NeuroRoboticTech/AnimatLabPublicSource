
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ROBOTICS_PORT RbPrismaticLimit : public AnimatSim::Environment::ConstraintLimit
			{
			protected:
				virtual void SetLimitValues();

			public:
				RbPrismaticLimit();
				virtual ~RbPrismaticLimit();

                virtual void Alpha(float fltA) {};
				virtual void SetLimitPos();
                virtual void SetupGraphics() {};
                virtual void DeleteGraphics() {};
            };

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
