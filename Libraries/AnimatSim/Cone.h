/**
\file	Cone.h

\brief	Declares the cone class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	Cone.
			
			\details This is a Cone type of rigid body. You can specify the dimensions of the radius and
			height for both the collision model and for the graphics model. 
			
			\author	dcofer
			\date	3/4/2011
			**/
			class ANIMAT_PORT Cone : public RigidBody
			{
			protected:
				/// The lower radius of the cone
				float m_fltLowerRadius;

				/// The upper radius of the cone
				float m_fltUpperRadius;
				
				/// The height of the cone
				float m_fltHeight;

			public:
				Cone();
				virtual ~Cone();

				virtual float LowerRadius();
				virtual void LowerRadius(float fltVal, BOOL bUseScaling = TRUE);

				virtual float UpperRadius();
				virtual void UpperRadius(float fltVal, BOOL bUseScaling = TRUE);

				virtual float Height();
				virtual void Height(float fltVal, BOOL bUseScaling = TRUE);
				
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
