
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
        //FIX PHYSICS
		//Vx::VxHeightField BULLET_PORT *CreateVxHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter);
		//osg::Geometry BULLET_PORT *CreateOsgFromVxConvexMesh(Vx::VxConvexMesh *vxGeometry);

        btConvexHullShape BULLET_PORT *OsgMeshToConvexHull(osg::Node *lpNode, bool bOptimize);
	}			// Environment
}				//BulletAnimatSim


