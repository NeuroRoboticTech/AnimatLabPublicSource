
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsPrismaticLimit : public AnimatSim::Environment::ConstraintLimit
			{
			protected:
				Vx::VxPrismatic *m_vxPrismatic;

				osg::ref_ptr<osg::Geometry> m_osgBox;
				osg::ref_ptr<osg::MatrixTransform> m_osgBoxTranslateMT;

				osg::ref_ptr<osg::Material> m_osgBoxMat;
				osg::ref_ptr<osg::StateSet> m_osgBoxSS;

			public:
				VsPrismaticLimit();
				virtual ~VsPrismaticLimit();

				virtual void PrismaticRef(Vx::VxPrismatic *vxPrismatic);

				virtual void Alpha(float fltA);
				virtual void SetLimitPos();

				osg::Geometry *BoxGeometry() {return m_osgBox.get();};
				osg::MatrixTransform *BoxTranslateMT() {return m_osgBoxTranslateMT.get();};
				osg::Material *BoxMat() {return m_osgBoxMat.get();};
				osg::StateSet *BoxSS() {return m_osgBoxSS.get();};

				virtual void SetupGraphics();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
