// Sphere.h: interface for the Sphere class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSPHERE_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
#define AFX_ALSPHERE_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/*! \brief 
				A Sphere type of rigid body.

				\remarks
				This is a Sphere type of rigid body. You can specify the dimensions of 
				the radius and height for both the collision model and for the graphics model.

				\sa
				Body, Joint, CAlBox, CAlPlane, CAlSphere, 
				CAlCone, CAlMuscle, CAlAttachment, CAlSphere                                
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Sphere : public RigidBody
			{
			protected:
				float m_fltRadius;
				float m_fltCollisionRadius;

			public:
				Sphere();
				virtual ~Sphere();

				virtual void Trace(ostream &oOs);
				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALSPHERE_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
