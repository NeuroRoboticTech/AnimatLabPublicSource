
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		Vx::VxHeightField VORTEX_PORT *CreateVxHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter);
		osg::Geometry VORTEX_PORT *CreateOsgFromVxConvexMesh(Vx::VxConvexMesh *vxGeometry);

	}			// Environment
}				//VortexAnimatSim


