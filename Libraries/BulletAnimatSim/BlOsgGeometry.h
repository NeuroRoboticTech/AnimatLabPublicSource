
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
        btHeightfieldTerrainShape BULLET_PORT *CreateBtHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter);
        btConvexHullShape BULLET_PORT *OsgMeshToConvexHull(osg::Node *lpNode, bool bOptimize);
	}			// Environment
}				//BulletAnimatSim


