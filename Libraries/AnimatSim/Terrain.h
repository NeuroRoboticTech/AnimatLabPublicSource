/**
\file	C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatSim\Terrain.h

\brief	Declares the terrain class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	The Terrain base class. 

			\details This is a mesh terrain object that defines the ground surface. You can have multiple
			terrains defined within a simulation. Terrains can be translated around, but they cannot be 
			rotated. 
			
			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT Terrain : public Mesh
			{
			protected:
				float m_fltSegmentWidth;
				float m_fltSegmentLength;
				float m_fltMaxHeight;

			public:
				Terrain();
				virtual ~Terrain();

				virtual float SegmentWidth();							
				virtual void SegmentWidth(float fltVal, BOOL bUseScaling = TRUE);

				virtual float SegmentLength();							
				virtual void SegmentLength(float fltVal, BOOL bUseScaling = TRUE);

				virtual float MaxHeight();							
				virtual void MaxHeight(float fltVal, BOOL bUseScaling = TRUE);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
