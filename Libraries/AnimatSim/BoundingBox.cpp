#include "stdafx.h"
#include "BoundingBox.h"

namespace AnimatSim
{

BoundingBox::BoundingBox(void)
{
}

BoundingBox::~BoundingBox(void)
{
}


void BoundingBox::Set(CStdFPoint &vMin, CStdFPoint &vMax)
{
	Min = vMin;
	Max = vMax;
}

void BoundingBox::Set(float MinX, float MinY, float MinZ, float MaxX, float MaxY, float MaxZ)
{
	Min.Set(MinX, MinY, MinZ);
	Max.Set(MaxX, MaxY, MaxZ);
}

float BoundingBox::Length()
{
	return Max.x - Min.x;
}

float BoundingBox::Width()
{
	return Max.z - Min.z;
}

float BoundingBox::Height()
{
	return Max.y - Min.y;
}

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

void BoundingBox::operator=(const BoundingBox &oBox)
{
	Min=oBox.Min;
	Max=oBox.Max;
}

}//end namespace AnimatSim