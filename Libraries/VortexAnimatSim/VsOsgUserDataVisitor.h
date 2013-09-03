#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsOsgUserDataVisitor : public osg::NodeVisitor
		{
			protected:
				VsMovableItem *m_lpItem;

			public:
				VsOsgUserDataVisitor(VsMovableItem *lpItem);
				~VsOsgUserDataVisitor(void);

				virtual void apply(osg::Geode &osgGeode);
		};

	}// end Visualization
}// end VortexAnimatSim

