#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		/**
		\brief	Vortex base body class.

		\details This is a base, secondary derived class for all body part objects within animatlab. It is
		derived from VsMovableItem, which contains all of the base OSG graphics manipulation code. It is 
		also derived from IPhysicsBody, which has the methods specifically required by the body part classes.
		
		\author	dcofer
		\date	5/2/2011
		**/
		class VORTEX_PORT VsBody : public VsMovableItem, public AnimatSim::Environment::IPhysicsBody
		{
		protected:
			BodyPart *m_lpThisBP;
			Vx::VxEntity::EntityControlTypeEnum m_eControlType;

			virtual void SetThisPointers();

		public:
			VsBody();
			virtual ~VsBody();

			virtual void SetFreeze(BOOL bVal) {};
			virtual void SetDensity(float fltVal) {};
			virtual void SetBody() = 0;

			virtual void Physics_UpdateNode() {};

		};

	}			// Environment
}				//VortexAnimatSim


