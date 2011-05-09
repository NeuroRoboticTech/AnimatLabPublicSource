#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsDragger : public osgManipulator::CompositeDragger
		{
			protected:
				virtual ~VsDragger(void);

				VsMovableItem *m_lpVsParent;
				osg::ref_ptr< osg::MatrixTransform> m_osgGripperMT;
				osg::ref_ptr< osg::AutoTransform > _autoTransform;
				osg::ref_ptr< osg::MatrixTransform > _sizeTransform;
				osg::ref_ptr< VsTranslateAxisDragger > _transDragger;
				osg::ref_ptr< VsTrackballDragger >	_tbDragger;

			public:
				VsDragger(VsMovableItem *lpParent, BOOL bAllowTranslateX, BOOL bAllowTranslateY, BOOL bAllowTranslateZ, 
					      BOOL bAllowRotateX, BOOL bAllowRotateY, BOOL bAllowRotateZ);

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();

				virtual void AddToScene();
				virtual void RemoveFromScene();
				virtual void SetupMatrix();
		};

	}// end Visualization
}// end VortexAnimatSim

