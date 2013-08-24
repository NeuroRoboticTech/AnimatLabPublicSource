
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
        class BULLET_PORT BlMatrixUtil : public OsgMatrixUtil
        {
           public:
               BlMatrixUtil() {};
               ~BlMatrixUtil() {};

              virtual osg::Matrix SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot);
              virtual CStdFPoint EulerRotationFromMatrix(osg::Matrix osgMT);
        };

		Vx::VxHeightField BULLET_PORT *CreateVxHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter);
		osg::Geometry BULLET_PORT *CreateOsgFromVxConvexMesh(Vx::VxConvexMesh *vxGeometry);

	}			// Environment
}				//BulletAnimatSim


