
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
        class VORTEX_PORT VsMatrixUtil : public OsgMatrixUtil
        {
           public:
               VsMatrixUtil() {};
               ~VsMatrixUtil() {};

              virtual osg::Matrix SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot);
              virtual CStdFPoint EulerRotationFromMatrix(osg::Matrix osgMT);
        };

		Vx::VxHeightField VORTEX_PORT *CreateVxHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter);
		osg::Geometry VORTEX_PORT *CreateOsgFromVxConvexMesh(Vx::VxConvexMesh *vxGeometry);

	}			// Environment
}				//VortexAnimatSim


