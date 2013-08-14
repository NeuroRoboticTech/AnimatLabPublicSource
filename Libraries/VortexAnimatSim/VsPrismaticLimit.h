
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsPrismaticLimit : public AnimatSim::Environment::ConstraintLimit, public OsgAnimatSim::Environment::OsgPrismaticLimit
			{
			protected:
				Vx::VxPrismatic *m_vxPrismatic;

				virtual void SetLimitValues();

			public:
				VsPrismaticLimit();
				virtual ~VsPrismaticLimit();

				virtual void PrismaticRef(Vx::VxPrismatic *vxPrismatic);

				virtual void Alpha(float fltA);
				virtual void SetLimitPos();

				virtual void SetupGraphics();
                virtual void DeleteGraphics();
            };

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
