#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsTranslateAxisDragger : public osgManipulator::CompositeDragger
		{
			protected:
				osg::ref_ptr< osgManipulator::Translate1DDragger >	_xDragger;
				osg::ref_ptr< osgManipulator::Translate1DDragger >	_yDragger;
				osg::ref_ptr< osgManipulator::Translate1DDragger >	_zDragger;

				virtual ~VsTranslateAxisDragger(void);

			public:
				VsTranslateAxisDragger(bool bAllowTranslateX, bool bAllowTranslateY, bool bAllowTranslateZ);

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();
		};

	}// end Visualization
}// end VortexAnimatSim

