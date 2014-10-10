#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsTrackballDragger : public osgManipulator::CompositeDragger
		{
			protected:
				osg::ref_ptr<osgManipulator::RotateCylinderDragger> _xDragger;
				osg::ref_ptr<osgManipulator::RotateCylinderDragger> _yDragger;
				osg::ref_ptr<osgManipulator::RotateCylinderDragger> _zDragger;

			virtual ~VsTrackballDragger(void);

			public:
				VsTrackballDragger(bool bAllowRotateX, bool bAllowRotateY, bool bAllowRotateZ);

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();
		};

	}// end Visualization
}// end VortexAnimatSim

