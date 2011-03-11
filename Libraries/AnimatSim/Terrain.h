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
			class ANIMAT_PORT Terrain : public RigidBody 
			{
			protected:
				string m_strTerrainFile;
				CStdIPoint m_ptGrid;

			public:
				Terrain();
				virtual ~Terrain();

				CStdIPoint Grid() {return m_ptGrid;};
				void Grid(CStdIPoint ptPoint);

				virtual BOOL AllowMouseManipulation();

				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
