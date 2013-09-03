#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsOsgUserData : public osg::Referenced
		{
			protected:
				VsMovableItem *m_lpItem;

			public:
				VsOsgUserData(VsMovableItem *lpItem);
				~VsOsgUserData(void);

				VsBody *GetBodyPart() {return dynamic_cast<VsBody *>(m_lpItem);};

				VsMovableItem *GetVsMovable() {return m_lpItem;};
				MovableItem *GetMovable() {return dynamic_cast<MovableItem *>(m_lpItem);};

				VsRigidBody *GetVsBody() {return dynamic_cast<VsRigidBody *>(m_lpItem);};
				RigidBody *GetBody() {return dynamic_cast<RigidBody *>(m_lpItem);};

				VsJoint *GetVsJoint() {return dynamic_cast<VsJoint *>(m_lpItem);};
				Joint *GetJoint() {return dynamic_cast<Joint *>(m_lpItem);};

				VsStructure *GetVsStucture() {return dynamic_cast<VsStructure *>(m_lpItem);};
				Structure *GetStructure() {return dynamic_cast<Structure *>(m_lpItem);};

				//VsLight *GetVsLight() {return dynamic_cast<VsLight *>(m_lpItem);};
				Light *GetLight() {return dynamic_cast<Light *>(m_lpItem);};
		};

	}// end Visualization
}// end VortexAnimatSim

