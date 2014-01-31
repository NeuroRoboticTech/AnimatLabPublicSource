
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ROBOTICS_PORT RbHingeLimit : public AnimatSim::Environment::ConstraintLimit
			{
			protected:
				virtual void SetLimitValues();

			public:
				RbHingeLimit();
				virtual ~RbHingeLimit();

                virtual void Alpha(float fltA) {};
				virtual void SetLimitPos();
                virtual void SetupGraphics() {};
                virtual void DeleteGraphics() {};
            };

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
