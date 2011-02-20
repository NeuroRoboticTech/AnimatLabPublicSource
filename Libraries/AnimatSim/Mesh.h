// Mesh.h: interface for the Mesh class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALMESH_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_)
#define AFX_ALMESH_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_

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
				A box type of rigid body.
				 
				\remarks
				This is a box type of rigid body. You can specify the dimensions in 
				the x, y, and z directions for both the collision box and for the 
				graphic box that will acutally be displayed. This is useful because 
				you might want to graphic box to be bigger than the collision box.
				Typically you will want to remove collision detection between any
				parts that are connected by a joint. But by making the graphics box
				larger than the collision box it allows you to make it appear that 
				two rigid bodies overlap, without them actually coming into contact.

				\sa
				Body, Joint, CAlBox, CAlPlane, CAlCylinder, 
				CAlCone, CAlMuscle, CAlAttachment, CAlSphere     
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Mesh : public RigidBody
			{
			protected:
				CStdFPoint m_oCollisionBoxSize;
				CStdFPoint m_oGraphicsBoxSize;

				string m_strGraphicsMesh;
				string m_strCollisionMesh;
				string m_strReceptiveFieldMesh;
				string m_strCollisionMeshType;

			public:
				//This is a test comment.
				Mesh();
				virtual ~Mesh();

				CStdFPoint CollisionBoxSize() {return m_oCollisionBoxSize;};
				CStdFPoint GraphicsBoxSize() {return m_oGraphicsBoxSize;};

				virtual void Trace(ostream &oOs);
				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALMESH_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_)
