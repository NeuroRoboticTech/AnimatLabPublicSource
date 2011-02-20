#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsDragger : public osgManipulator::CompositeDragger
		{
			protected:
				virtual ~VsDragger(void);

				VsBody *m_lpVsBody;
				osg::ref_ptr< osg::MatrixTransform> m_osgGripperMT;
				osg::ref_ptr< osg::AutoTransform > _autoTransform;
				osg::ref_ptr< osg::MatrixTransform > _sizeTransform;
				osg::ref_ptr< VsTranslateAxisDragger > _transDragger;
				osg::ref_ptr< VsTrackballDragger >	_tbDragger;

			public:
				VsDragger(VsBody *lpParent);

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();

				virtual void AddToScene();
				virtual void RemoveFromScene();
				virtual void SetupMatrix();
		};

	}// end Visualization
}// end VortexAnimatSim

