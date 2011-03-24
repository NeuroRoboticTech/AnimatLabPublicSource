/**
\file	BoundingBox.h

\brief	Declares the bounding box class.
**/

#pragma once

namespace AnimatSim
{
	/**
	\brief	Bounding box class for geometric objects. 
	
	\author	dcofer
	\date	3/24/2011
	**/
	class ANIMAT_PORT BoundingBox
	{
	public:
		/// A vertex with the minimum widths of the bounding box. 
		CStdFPoint Min;

		/// A vertex with the maximum widths of the bounding box. 
		CStdFPoint Max;

		BoundingBox(void);
		~BoundingBox(void);
		
		void Set(CStdFPoint &vMin, CStdFPoint &vMax);
		void Set(float MinX, float MinY, float MinZ, float MaxX, float MaxY, float MaxZ);

		float Length();
		float Width();
		float Height();

		float MaxDimension();
	
		void operator=(const BoundingBox &oBox);
	};

}//end namespace AnimatSim
