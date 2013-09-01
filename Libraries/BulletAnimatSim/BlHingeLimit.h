
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class BULLET_PORT BlHingeLimit : public AnimatSim::Environment::ConstraintLimit, public OsgAnimatSim::Environment::Joints::OsgHingeLimit
			{
			protected:
				virtual void SetLimitValues();
                virtual CStdColor GetLimitColor() {return m_vColor;};

			public:
				BlHingeLimit();
				virtual ~BlHingeLimit();

				virtual void Alpha(float fltA);
				virtual void SetLimitPos();
				
				virtual void SetupGraphics();
                virtual void DeleteGraphics();
            };

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
