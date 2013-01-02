#pragma once

#include "VsAutoTransform.h"

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

				float m_fltUserDefinedRadius;

			public:
				VsDragger(VsMovableItem *lpParent, BOOL bAllowTranslateX, BOOL bAllowTranslateY, BOOL bAllowTranslateZ, 
					      BOOL bAllowRotateX, BOOL bAllowRotateY, BOOL bAllowRotateZ, float fltUserDefinedRadius);

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();

				virtual void AddToScene();
				virtual void RemoveFromScene();
				virtual BOOL IsInScene();
				virtual void SetupMatrix();
		};

	}// end Visualization
}// end VortexAnimatSim

