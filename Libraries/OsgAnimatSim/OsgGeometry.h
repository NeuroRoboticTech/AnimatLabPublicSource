
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		void ANIMAT_OSG_PORT ApplyVertexTransform(osg::Node *node, osg::Matrix omat);
		BOOL ANIMAT_OSG_PORT OsgMatricesEqual(osg::Matrix v1, osg::Matrix v2);
		osg::Quat ANIMAT_OSG_PORT EulerToQuaternion(float fX, float fY, float fZ);
		CStdFPoint ANIMAT_OSG_PORT QuaterionToEuler(osg::Quat vQuat);
		osg::Matrix ANIMAT_OSG_PORT SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot);
		osg::Matrix ANIMAT_OSG_PORT SetupMatrix(CStdFPoint &localPos, osg::Quat qRot);
		void ANIMAT_OSG_PORT SetNodeColor(osg::Node *osgNode, CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess);
		void ANIMAT_OSG_PORT AddNodeTexture(osg::Node *osgNode, string strTexture, osg::StateAttribute::GLMode eTextureMode);
		osg::Geometry ANIMAT_OSG_PORT *CreateBoxGeometry(float xsize, float ysize, float zsize, float fltXSegWidth, float fltYSegWidth, float fltZSegWidth);
		osg::Geometry ANIMAT_OSG_PORT *CreateConeGeometry(float height, float topradius, float botradius, int sides, bool doSide, bool doTop, bool doBottom);
		osg::Geometry ANIMAT_OSG_PORT *CreateSphereGeometry(int latres, int longres, float radius);
		osg::Geometry ANIMAT_OSG_PORT *CreateEllipsoidGeometry(int latres, int longres, float rSemiMajorAxis, float rSemiMinorAxis);
		osg::Geometry ANIMAT_OSG_PORT *CreatePlaneGeometry(float fltCornerX, float fltCornerY, float fltXSize, float fltYSize, float fltXGrid, float fltYGrid, BOOL bBothSides);
		osg::MatrixTransform ANIMAT_OSG_PORT *CreateLinearAxis(float fltGripScale, CStdFPoint vRotAxis);
		osg::Geode ANIMAT_OSG_PORT *CreateCircle( int plane, int approx, float radius, float width );
		osg::Vec3Array ANIMAT_OSG_PORT *CreateCircleVerts( int plane, int approx, float radius );
		osg::Geometry ANIMAT_OSG_PORT *CreateTorusGeometry(float innerRadius, float outerRadius, int sides, int rings);
		osg::Node ANIMAT_OSG_PORT *CreateHeightField(std::string heightFile, float fltSegWidth, float fltSegLength, float fltMaxHeight, osg::HeightField **osgMap);
		Vx::VxHeightField ANIMAT_OSG_PORT *CreateVxHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter);
		osg::Geometry ANIMAT_OSG_PORT *CreateOsgFromVxConvexMesh(Vx::VxConvexMesh *vxGeometry);

	}			// Environment
}				//OsgAnimatSim


