
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
        btHeightfieldTerrainShape BULLET_PORT *CreateBtHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float &fltMinHeight, float &fltMaxHeight, float **aryHeightData);
        btConvexHullShape BULLET_PORT *OsgMeshToConvexHull(osg::Node *lpNode, bool bOptimize, float fltMargin);
        btConvexHullShape* OsgConvexShrunkenHullCollisionShape( osg::Node* node );
        float BULLET_PORT OsgConvexHullVolume( osg::Node* nod);

        btVector3 Vec3AnimatToBullet(const CStdFPoint &vPoint);
        CStdFPoint Vec3BulletToAnimat(const btVector3 &vPoint);
	}			// Environment
}				//BulletAnimatSim


