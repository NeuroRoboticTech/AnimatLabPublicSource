// OsgLine.h: interface for the OsgLine class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

		class ANIMAT_OSG_PORT OsgPyramid
		{
		protected:
            osg::Vec3d m_vPoint;
            osg::Vec3d m_vBase[3];

		public:
			OsgPyramid();
			OsgPyramid(osg::Vec3d vPoint, osg::Vec3d vBase[3]);
			OsgPyramid(osg::Vec3d vPoint, osg::Vec3d vBase1, osg::Vec3d vBase2, osg::Vec3d vBase3);
			virtual ~OsgPyramid();

            float Height();
            float BaseArea();
            float Volume();
		};

	}			// Visualization
}				//OsgAnimatSim
