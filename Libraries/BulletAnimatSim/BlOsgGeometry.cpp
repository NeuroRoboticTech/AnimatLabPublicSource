#include "StdAfx.h"
#include <stdarg.h>
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlClassFactory.h"
#include "BlSimulator.h"
#include "BlOsgGeometry.h"

namespace BulletAnimatSim
{
	namespace Environment
	{

#pragma region CreateGeometry_Code

    btHeightfieldTerrainShape BULLET_PORT *CreateBtHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter)
    {
	    //Lets create the height array.
        float fltMaxHeight = -1;
	    int iCols = osgHeightField->getNumColumns();
	    int iRows = osgHeightField->getNumRows();
	    float* heightfieldData = new float[iCols*iRows];
	    for(int iRow=0; iRow<iRows; iRow++)
    	    for(int iCol=0; iCol<iCols; iCol++)
            {
                float fltHeight = (osgHeightField->getHeight(iCol, iRow) + fltBaseHeight);
			    heightfieldData[(iRow*iCols) + iCol] = fltHeight;
                if(fltHeight > fltMaxHeight)
                    fltMaxHeight = fltHeight;
            }

        float fltHeightOffset = -(fltMaxHeight/2);
        btHeightfieldTerrainShape* heightFieldShape = new btHeightfieldTerrainShape(iCols, iRows, heightfieldData, 1, fltHeightOffset, (fltMaxHeight+fltHeightOffset), 2, PHY_FLOAT , false);

        btVector3 localScaling(fltSegWidth, fltSegLength, 1);
        heightFieldShape->setLocalScaling(localScaling);
        heightFieldShape->setUseDiamondSubdivision(true);

	    return heightFieldShape;
    }

    btConvexHullShape BULLET_PORT *OsgMeshToConvexHull(osg::Node *lpNode, bool bOptimize)
    {
        btConvexHullShape *originalConvexShape =  osgbCollision::btConvexHullCollisionShapeFromOSG(lpNode);

	    //create a hull approximation
	    btShapeHull* hull = new btShapeHull(originalConvexShape);
	    btScalar margin = originalConvexShape->getMargin();
	    hull->buildHull(margin);
	    btConvexHullShape* simplifiedConvexShape = new btConvexHullShape((const btScalar *) hull->getVertexPointer(),hull->numVertices());

        if(originalConvexShape)
            delete originalConvexShape;

        return simplifiedConvexShape;
    }


#pragma endregion

	}			// Environment
//}				//BulletAnimatSim

}