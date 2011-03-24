/**
\file	BoundingBox.cpp

\brief	Implements the bounding box class.
**/

#include "stdafx.h"
#include "BoundingBox.h"

namespace AnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
BoundingBox::BoundingBox(void)
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
BoundingBox::~BoundingBox(void)
{
}

/**
\brief	Sets the minimum and maximum widths of the bounding box.

\author	dcofer
\date	3/24/2011

\param [in,out]	vMin	The minimum vertex. 
\param [in,out]	vMax	The maximum vertex. 
**/
void BoundingBox::Set(CStdFPoint &vMin, CStdFPoint &vMax)
{
	Min = vMin;
	Max = vMax;
}

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
void BoundingBox::Set(float MinX, float MinY, float MinZ, float MaxX, float MaxY, float MaxZ)
{
	Min.Set(MinX, MinY, MinZ);
	Max.Set(MaxX, MaxY, MaxZ);
}

/**
\brief	Gets the length = (Max.x - Min.x).

\author	dcofer
\date	3/24/2011

\return	Length of bounding box.
**/
float BoundingBox::Length()
{
	return Max.x - Min.x;
}

/**
\brief	Gets the width = (Max.z - Min.z).

\author	dcofer
\date	3/24/2011

\return	Width of bounding box.
**/
float BoundingBox::Width()
{
	return Max.z - Min.z;
}

/**
\brief	Gets the height = (Max.y - Min.y).

\author	dcofer
\date	3/24/2011

\return	Height of bounding box.
**/
float BoundingBox::Height()
{
	return Max.y - Min.y;
}

/**
\brief	Gets the maximum dimension.

\author	dcofer
\date	3/24/2011

\return	Max of any dimension.
**/
float BoundingBox::MaxDimension()
{
	float fltMax = -1;

	fltMax = max(fabs(Min.x), fabs(Min.y));
	fltMax = max(fltMax, fabs(Min.z));

	fltMax = max(fltMax, fabs(Max.x));
	fltMax = max(fltMax, fabs(Max.y));
	fltMax = max(fltMax, fabs(Max.z));

	return fltMax;
}

/**
\brief	Compares two bounding boxes to see if they are equal.

\author	dcofer
\date	3/24/2011

\param	oBox	The box to compare with. 
**/
void BoundingBox::operator=(const BoundingBox &oBox)
{
	Min=oBox.Min;
	Max=oBox.Max;
}

}//end namespace AnimatSim