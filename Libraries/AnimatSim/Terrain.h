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
				int m_iTextureLengthSegments;
				int m_iTextureWidthSegments;

			public:
				Terrain();
				virtual ~Terrain();
						
				static Terrain *CastToDerived(AnimatBase *lpBase) {return static_cast<Terrain*>(lpBase);}

				virtual bool AllowRotateDragX();
				virtual bool AllowRotateDragY();
				virtual bool AllowRotateDragZ();

				virtual float SegmentWidth();							
				virtual void SegmentWidth(float fltVal, bool bUseScaling = true);

				virtual float SegmentLength();							
				virtual void SegmentLength(float fltVal, bool bUseScaling = true);

				virtual float MaxHeight();							
				virtual void MaxHeight(float fltVal, bool bUseScaling = true);

				virtual int TextureLengthSegments();
				virtual void TextureLengthSegments(int iVal);

				virtual int TextureWidthSegments();
				virtual void TextureWidthSegments(int iVal);

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
