#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

		class ANIMAT_OSG_PORT OsgUserDataVisitor : public osg::NodeVisitor
		{
			protected:
				OsgMovableItem *m_lpItem;

			public:
				OsgUserDataVisitor(OsgMovableItem *lpItem);
				~OsgUserDataVisitor(void);

				virtual void apply(osg::Geode &osgGeode);
		};

	}// end Visualization
}// end OsgAnimatSim

