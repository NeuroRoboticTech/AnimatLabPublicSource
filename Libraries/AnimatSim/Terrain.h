// Terrain.h: interface for the Terrain class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALTERRAIN_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_)
#define AFX_ALTERRAIN_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_

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
				A ground Terrain type of rigid body.

				\remarks
				This is a flat Terrain that can be used to define the ground
				or the surface of the water. You can only have ONE of each.
				If you attempt to define more than one of these types of objects
				then it will cause an exception. However, you do not have to 
				define one of these. There are other types that you can use
				to define the ground, and if you are not simulating anything 
				underwater, or are simulating deep water, then you do not need
				to simulate the surface of the water.
				 
				All rigid bodies in the system will have the force of gravity actin
				on it to push towards the ground Terrain. If you create objects that are
				below the ground Terrain then the physics engine will see this as a
				collision with the ground and attempt to push it back up above the
				surface.

				\sa
				Body, Joint, CAlBox, Terrain, CAlCylinder, 
				CAlCone, CAlMuscle, CAlAttachment, CAlSphere                                
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Terrain : public RigidBody 
			{
			protected:
				///The height of the Terrain on the Y axis.
				float m_fltHeight;
				string m_strTerrainFile;
				CStdIPoint m_ptGrid;

			public:
				Terrain();
				virtual ~Terrain();

				float Height();
				void Height(float fltVal);

				CStdIPoint Grid() {return m_ptGrid;};
				void Grid(CStdIPoint ptPoint);

				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALTERRAIN_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_)
