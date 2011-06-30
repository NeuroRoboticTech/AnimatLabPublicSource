/**
\file	Plane.h

\brief	Declares the plane class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/*! \brief 
				A ground plane type of rigid body.

				\remarks
				This is a flat plane that can be used to define the ground
				or the surface of the water. You can only have ONE of each.
				If you attempt to define more than one of these types of objects
				then it will cause an exception. However, you do not have to 
				define one of these. There are other types that you can use
				to define the ground, and if you are not simulating anything 
				underwater, or are simulating deep water, then you do not need
				to simulate the surface of the water.
				 
				All rigid bodies in the system will have the force of gravity actin
				on it to push towards the ground plane. If you create objects that are
				below the ground plane then the physics engine will see this as a
				collision with the ground and attempt to push it back up above the
				surface.

				\sa
				Body, Joint, CAlBox, Plane, CAlCylinder, 
				CAlCone, CAlMuscle, CAlAttachment, CAlSphere                                
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Plane : public RigidBody 
			{
			protected:
				CStdFPoint m_ptSize;
				
				int m_iWidthSegments;
				int m_iLengthSegments;

			public:
				Plane();
				virtual ~Plane();

				virtual float CornerX();
				virtual float CornerY();
				
				virtual float GridX();
				virtual float GridY();

				virtual CStdFPoint Size();
				virtual void Size(CStdFPoint ptPoint, BOOL bUseScaling = TRUE);
				virtual void Size(string strXml, BOOL bUseScaling = TRUE);

				virtual int WidthSegments();
				virtual void WidthSegments(int iVal);

				virtual int LengthSegments();
				virtual void LengthSegments(int iVal);

				virtual BOOL AllowMouseManipulation();

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
