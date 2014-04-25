#pragma once

namespace AnimatSim
{
	namespace Robotics
	{

		class ANIMAT_PORT RobotPartInterface : public AnimatBase
		{
		protected:
			RobotInterface *m_lpParentInterface;
			RobotIOControl *m_lpParentIOControl;

		public:
			RobotPartInterface(void);
			virtual ~RobotPartInterface(void);

			virtual void ParentIOControl(RobotIOControl *lpParent);
			virtual RobotIOControl *ParentIOControl();

			virtual void Initialize();
		};

	}
}