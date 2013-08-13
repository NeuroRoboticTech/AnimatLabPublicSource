
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ANIMAT_OSG_PORT OsgPrismaticLimit
			{
			protected:
				osg::ref_ptr<osg::Geometry> m_osgCylinder;
				osg::ref_ptr<osg::Geode> m_osgCylinderGeode;
				osg::ref_ptr<osg::MatrixTransform> m_osgCylinderMT;
				osg::ref_ptr<osg::Material> m_osgCylinderMat;
				osg::ref_ptr<osg::StateSet> m_osgCylinderSS;

				osg::ref_ptr<osg::Geometry> m_osgBox;
				osg::ref_ptr<osg::MatrixTransform> m_osgBoxMT;
				osg::ref_ptr<osg::Material> m_osgBoxMat;
				osg::ref_ptr<osg::StateSet> m_osgBoxSS;

			public:
				OsgPrismaticLimit();
				virtual ~OsgPrismaticLimit();

				virtual void LimitAlpha(float fltA);
				virtual void SetLimitPos(float fltRadius, float fltLimitPos);

				osg::Geometry *BoxGeometry();
				osg::MatrixTransform *BoxMT();
				osg::Material *BoxMat();
				osg::StateSet *BoxSS();

				osg::Geometry *CylinderGeometry();
				osg::MatrixTransform *CylinderMT();
				osg::Material *CylinderMat();
				osg::StateSet *CylinderSS();

                virtual void SetupLimitGraphics(float fltBoxSize, float fltRadius, float fltLimitPos, bool bIsShowPosition, CStdColor vColor);
                virtual void DeleteLimitGraphics();
			};

		}		//Joints
	}			// Environment
}				//OsgAnimatSim
