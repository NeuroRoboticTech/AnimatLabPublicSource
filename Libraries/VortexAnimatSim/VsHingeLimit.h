
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsHingeLimit : public AnimatSim::Environment::ConstraintLimit, public OsgAnimatSim::Environment::OsgHingeLimit
			{
			protected:
				Vx::VxHinge *m_vxHinge;

				virtual void SetLimitValues();
                virtual CStdColor GetLimitColor() {return m_vColor;};

			public:
				VsHingeLimit();
				virtual ~VsHingeLimit();

				virtual void HingeRef(Vx::VxHinge *vxHinge);

				virtual void Alpha(float fltA);
				virtual void SetLimitPos();
				
				virtual void SetupGraphics();
                virtual void DeleteGraphics();
            };

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
