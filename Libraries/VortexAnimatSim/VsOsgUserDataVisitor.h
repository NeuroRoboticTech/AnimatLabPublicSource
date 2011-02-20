#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsOsgUserDataVisitor : public osg::NodeVisitor
		{
			protected:
				VsRigidBody *m_lpVsBody;
				RigidBody *m_lpBody;

				VsJoint *m_lpVsJoint;
				Joint *m_lpJoint;

			public:
				VsOsgUserDataVisitor(VsRigidBody *lpVsBody);
				VsOsgUserDataVisitor(VsJoint *lpVsJoint);				
				~VsOsgUserDataVisitor(void);

				virtual void apply(osg::Geode &osgGeode);
		};

	}// end Visualization
}// end VortexAnimatSim

