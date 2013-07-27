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

				//OsgBody *GetBodyPart() {return dynamic_cast<OsgBody *>(m_lpItem);};  NEED TO REPAIR

				OsgMovableItem *GetOsgMovable() {return m_lpItem;};
				MovableItem *GetMovable() {return dynamic_cast<MovableItem *>(m_lpItem);};

				OsgStructure *GetOsgStucture() {return dynamic_cast<OsgStructure *>(m_lpItem);};
				Structure *GetStructure() {return dynamic_cast<Structure *>(m_lpItem);};

				Light *GetLight() {return dynamic_cast<Light *>(m_lpItem);};
		};

	}// end Visualization
}// end OsgAnimatSim

