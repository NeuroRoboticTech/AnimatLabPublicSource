#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

class OsgMouseSpring
{
	private:
		OsgMouseSpring(void);
		static OsgMouseSpring *m_instance;

		osg::ref_ptr<osg::Geode> m_gdeLine;
		osg::ref_ptr<osg::Geometry> m_linesGeom;
		osg::ref_ptr<osg::LineWidth> m_lineWidth;
		osg::Vec3 m_v3Start;
		osg::Vec3 m_v3End;
		osg::ref_ptr<osg::Vec3Array> m_aryLines;
		osg::Vec3 m_v3Grab;
		VsRigidBody *m_osgRB;

	protected:
		void Update();

	public:
		
		static OsgMouseSpring* GetInstance()
		{
			if(!m_instance)
			{
				m_instance = new OsgMouseSpring();
				m_instance->Initialize();
			}
			
			return m_instance;
		};

		~OsgMouseSpring(void);

		osg::Node* GetNode(){return m_gdeLine.get();};
		void SetStart(osg::Vec3 v3Start);
		osg::Vec3 GetStart() { return m_v3Start; };
		
		void SetEnd(osg::Vec3 v3End);
		osg::Vec3 GetEnd() { return m_v3End; };

		void SetGrabPosition(osg::Vec3 v3Grab) {m_v3Grab = v3Grab;};
		osg::Vec3 GetGrabPosition() {return m_v3Grab;}

		void SetRigidBody (VsRigidBody *osgRB) {m_osgRB = osgRB;};
		VsRigidBody* GetRigidBody() {return m_osgRB;};

		void Visible(BOOL bVal);
		void Initialize();
};

	}// end Visualization
}// end OsgAnimatSim
