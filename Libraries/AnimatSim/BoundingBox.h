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
		
		/**
		\brief	Sets the minimum and maximum widths of the bounding box.

		\author	dcofer
		\date	3/24/2011

		\param [in,out]	vMin	The minimum vertex. 
		\param [in,out]	vMax	The maximum vertex. 
		**/
		void Set(CStdFPoint &vMin, CStdFPoint &vMax);

		/**
		\brief	Sets the minimum and maximum widths of the bounding box.

		\author	dcofer
		\date	3/24/2011

		\param	MinX	The minimum x coordinate. 
		\param	MinY	The minimum y coordinate. 
		\param	MinZ	The minimum z coordinate. 
		\param	MaxX	The maximum x coordinate. 
		\param	MaxY	The maximum y coordinate. 
		\param	MaxZ	The maximum z coordinate. 
		**/
		void Set(float MinX, float MinY, float MinZ, float MaxX, float MaxY, float MaxZ);

		/**
		\brief	Gets the length = (Max.x - Min.x).

		\author	dcofer
		\date	3/24/2011

		\return	Length of bounding box.
		**/
		float Length();

		/**
		\brief	Gets the width = (Max.z - Min.z).

		\author	dcofer
		\date	3/24/2011

		\return	Width of bounding box.
		**/
		float Width();

		/**
		\brief	Gets the height = (Max.y - Min.y).

		\author	dcofer
		\date	3/24/2011

		\return	Height of bounding box.
		**/
		float Height();

		/**
		\brief	Gets the maximum dimension.

		\author	dcofer
		\date	3/24/2011

		\return	Max of any dimension.
		**/
		float MaxDimension();
	
		/**
		\brief	Compares two bounding boxes to see if they are equal.

		\author	dcofer
		\date	3/24/2011

		\param	oBox	The box to compare with. 
		**/
		void operator=(const BoundingBox &oBox);
	};

}//end namespace AnimatSim
