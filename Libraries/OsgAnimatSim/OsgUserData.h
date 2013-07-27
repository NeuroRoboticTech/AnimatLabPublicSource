#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

		class ANIMAT_OSG_PORT OsgUserData : public osg::Referenced
		{
			protected:
				OsgMovableItem *m_lpItem;

			public:
				OsgUserData(OsgMovableItem *lpItem);
				~OsgUserData(void);

				OsgBody *GetBodyPart() {return dynamic_cast<OsgBody *>(m_lpItem);};

				OsgMovableItem *GetVsMovable() {return m_lpItem;};
				MovableItem *GetMovable() {return dynamic_cast<MovableItem *>(m_lpItem);};

				VsRigidBody *GetOsgBody() {return dynamic_cast<VsRigidBody *>(m_lpItem);};
				RigidBody *GetBody() {return dynamic_cast<RigidBody *>(m_lpItem);};

				OsgJoint *GetOsgJoint() {return dynamic_cast<OsgJoint *>(m_lpItem);};
				Joint *GetJoint() {return dynamic_cast<Joint *>(m_lpItem);};

				OsgStructure *GetVsStucture() {return dynamic_cast<OsgStructure *>(m_lpItem);};
				Structure *GetStructure() {return dynamic_cast<Structure *>(m_lpItem);};

				//OsgLight *GetOsgLight() {return dynamic_cast<OsgLight *>(m_lpItem);};
				Light *GetLight() {return dynamic_cast<Light *>(m_lpItem);};
		};

	}// end Visualization
}// end OsgAnimatSim

