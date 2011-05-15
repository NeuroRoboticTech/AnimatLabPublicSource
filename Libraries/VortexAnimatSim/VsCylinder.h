// VsCylinder.h: interface for the VsCylinder class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSCYLINDER_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
#define AFX_VSCYLINDER_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsCylinder : public AnimatSim::Environment::Bodies::Cylinder, public VsRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsCylinder();
				virtual ~VsCylinder();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSCYLINDER_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
