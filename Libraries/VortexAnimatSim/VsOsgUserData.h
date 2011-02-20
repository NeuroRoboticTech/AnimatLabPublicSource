#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsOsgUserData : public osg::Referenced
		{
			protected:
				VsBody *m_lpVsBodyPart;

				VsRigidBody *m_lpVsBody;
				RigidBody *m_lpBody;
				
				VsJoint *m_lpVsJoint;
				Joint *m_lpJoint;

			public:
				VsOsgUserData(VsBody *lpBody);
				VsOsgUserData(VsRigidBody *lpBody);
				VsOsgUserData(VsJoint *lpJoint);
				~VsOsgUserData(void);

				VsBody *GetBodyPart() {return m_lpVsBodyPart;};

				VsRigidBody *GetVsBody() {return m_lpVsBody;};
				RigidBody *GetBody() {return m_lpBody;};

				VsJoint *GetVsJoint() {return m_lpVsJoint;};
				Joint *GetJoint() {return m_lpJoint;};
		};

	}// end Visualization
}// end VortexAnimatSim

