
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ANIMAT_OSG_PORT OsgHingeLimit : public AnimatSim::Environment::ConstraintLimit
			{
			protected:
				Vx::VxHinge *m_vxHinge;

				osg::ref_ptr<osg::Geometry> m_osgFlap;
				osg::ref_ptr<osg::MatrixTransform> m_osgFlapTranslateMT;
				osg::ref_ptr<osg::MatrixTransform> m_osgFlapRotateMT;

				osg::ref_ptr<osg::Material> m_osgFlapMat;
				osg::ref_ptr<osg::StateSet> m_osgFlapSS;

				virtual void SetLimitValues();

			public:
				OsgHingeLimit();
				virtual ~OsgHingeLimit();

				virtual void HingeRef(Vx::VxHinge *vxHinge);

				virtual void Alpha(float fltA);
				virtual void SetLimitPos();
				
				osg::Geometry *FlapGeometry() {return m_osgFlap.get();};
				osg::MatrixTransform *FlapTranslateMT() {return m_osgFlapTranslateMT.get();};
				osg::MatrixTransform *FlapRotateMT() {return m_osgFlapRotateMT.get();};
				osg::Material *FlapMat() {return m_osgFlapMat.get();};
				osg::StateSet *FlapSS() {return m_osgFlapSS.get();};

				virtual void SetupGraphics();
                virtual void DeleteGraphics();
            };

		}		//Joints
	}			// Environment
}				//OsgAnimatSim
