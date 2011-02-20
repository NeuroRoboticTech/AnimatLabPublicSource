#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT BoundingBox
	{
	public:
		CStdFPoint Min;
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
