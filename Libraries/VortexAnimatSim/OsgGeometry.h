
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		void VORTEX_PORT ApplyVertexTransform(osg::Node *node, osg::Matrix omat);
		bool VORTEX_PORT OsgMatricesEqual(osg::Matrix v1, osg::Matrix v2);
		osg::Quat VORTEX_PORT EulerToQuaternion(float fX, float fY, float fZ);
		CStdFPoint VORTEX_PORT QuaterionToEuler(osg::Quat vQuat);
		osg::Matrix VORTEX_PORT SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot);
		osg::Matrix VORTEX_PORT SetupMatrix(CStdFPoint &localPos, osg::Quat qRot);
        osg::Matrix VORTEX_PORT LoadMatrix(CStdXml &oXml, std::string strElementName); 
        std::string VORTEX_PORT SaveMatrixString(osg::Matrix osgMT);
        void VORTEX_PORT SaveMatrix(CStdXml &oXml, std::string strElementName, osg::Matrix osgMT); 
		void VORTEX_PORT SetNodeColor(osg::Node *osgNode, CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess);
		void VORTEX_PORT AddNodeTexture(osg::Node *osgNode, std::string strTexture, osg::StateAttribute::GLMode eTextureMode);
		osg::Geometry VORTEX_PORT *CreateBoxGeometry(float xsize, float ysize, float zsize, float fltXSegWidth, float fltYSegWidth, float fltZSegWidth);
		osg::Geometry VORTEX_PORT *CreateConeGeometry(float height, float topradius, float botradius, int sides, bool doSide, bool doTop, bool doBottom);
		osg::Geometry VORTEX_PORT *CreateSphereGeometry(int latres, int longres, float radius);
		osg::Geometry VORTEX_PORT *CreateEllipsoidGeometry(int latres, int longres, float rSemiMajorAxis, float rSemiMinorAxis);
		osg::Geometry VORTEX_PORT *CreatePlaneGeometry(float fltCornerX, float fltCornerY, float fltXSize, float fltYSize, float fltXGrid, float fltYGrid, bool bBothSides);
		osg::MatrixTransform VORTEX_PORT *CreateLinearAxis(float fltGripScale, CStdFPoint vRotAxis);
		osg::Geode VORTEX_PORT *CreateCircle( int plane, int approx, float radius, float width );
		osg::Vec3Array VORTEX_PORT *CreateCircleVerts( int plane, int approx, float radius );
		osg::Geometry VORTEX_PORT *CreateTorusGeometry(float innerRadius, float outerRadius, int sides, int rings);
		osg::Node VORTEX_PORT *CreateHeightField(std::string heightFile, float fltSegWidth, float fltSegLength, float fltMaxHeight, osg::HeightField **osgMap);
		Vx::VxHeightField VORTEX_PORT *CreateVxHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter);
		osg::Geometry VORTEX_PORT *CreateOsgFromVxConvexMesh(Vx::VxConvexMesh *vxGeometry);

	}			// Environment
}				//VortexAnimatSim


