#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

		class OsgTranslateAxisDragger : public osgManipulator::CompositeDragger
		{
			protected:
				osg::ref_ptr< osgManipulator::Translate1DDragger >	_xDragger;
				osg::ref_ptr< osgManipulator::Translate1DDragger >	_yDragger;
				osg::ref_ptr< osgManipulator::Translate1DDragger >	_zDragger;

				virtual ~OsgTranslateAxisDragger(void);

			public:
				OsgTranslateAxisDragger(BOOL bAllowTranslateX, BOOL bAllowTranslateY, BOOL bAllowTranslateZ);

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();
		};

	}// end Visualization
}// end OsgAnimatSim

