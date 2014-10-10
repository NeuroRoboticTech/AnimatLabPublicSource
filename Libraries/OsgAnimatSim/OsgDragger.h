#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

		class OsgDragger : public osgManipulator::CompositeDragger
		{
			protected:
				virtual ~OsgDragger(void);

				OsgMovableItem *m_lpVsParent;
				osg::ref_ptr< osg::MatrixTransform> m_osgGripperMT;
				osg::ref_ptr< osg::AutoTransform > _autoTransform;
				osg::ref_ptr< osg::MatrixTransform > _sizeTransform;
				osg::ref_ptr< OsgTranslateAxisDragger > _transDragger;
				osg::ref_ptr< OsgTrackballDragger >	_tbDragger;

				float m_fltUserDefinedRadius;

			public:
				OsgDragger(OsgMovableItem *lpParent, bool bAllowTranslateX, bool bAllowTranslateY, bool bAllowTranslateZ, 
					      bool bAllowRotateX, bool bAllowRotateY, bool bAllowRotateZ, float fltUserDefinedRadius);

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();

				virtual void AddToScene();
				virtual void RemoveFromScene();
				virtual bool IsInScene();
				virtual void SetupMatrix();
		};

	}// end Visualization
}// end OsgAnimatSim

