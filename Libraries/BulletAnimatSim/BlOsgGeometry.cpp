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

    btHeightfieldTerrainShape BULLET_PORT *CreateBtHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float &fltMinHeight, float &fltMaxHeight, float **aryHeightData)
    {
	    //Lets create the height array.
        fltMaxHeight = -10000;
        fltMinHeight = 10000;
	    int iCols = osgHeightField->getNumColumns();
	    int iRows = osgHeightField->getNumRows();
	    float* heightfieldData = new float[iCols*iRows];
	    for(int iRow=0; iRow<iRows; iRow++)
    	    for(int iCol=0; iCol<iCols; iCol++)
            {
                float fltHeight = osgHeightField->getHeight(iCol, iRow);
			    heightfieldData[(iRow*iCols) + iCol] = fltHeight;
                if(fltHeight > fltMaxHeight)
                    fltMaxHeight = fltHeight;
                if(fltHeight < fltMinHeight)
                    fltMinHeight = fltHeight;
            }


        btHeightfieldTerrainShape* heightFieldShape = new btHeightfieldTerrainShape(iCols, iRows, heightfieldData, 1, fltMinHeight, fltMaxHeight, 1, PHY_FLOAT , false);

        btVector3 localScaling(fltSegWidth, 1, fltSegLength);
        heightFieldShape->setLocalScaling(localScaling);
        heightFieldShape->setUseDiamondSubdivision(true);

        *aryHeightData = heightfieldData;

	    return heightFieldShape;
    }

    btConvexHullShape BULLET_PORT *OsgMeshToConvexHull(osg::Node *lpNode, bool bOptimize, float fltMargin)
    {
        btConvexHullShape *originalConvexShape =  osgbCollision::btConvexHullCollisionShapeFromOSG(lpNode);
        
        if(fltMargin >= 0)
            originalConvexShape->setMargin(fltMargin);

        btConvexHullShape* simplifiedConvexShape = NULL;
        if(bOptimize)
        {
	        //create a hull approximation
	        btShapeHull* hull = new btShapeHull(originalConvexShape);
	        btScalar margin = originalConvexShape->getMargin();
            if(fltMargin < 0)
                hull->buildHull(margin);
            else
                hull->buildHull(fltMargin);

	        simplifiedConvexShape = new btConvexHullShape((const btScalar *) hull->getVertexPointer(),hull->numVertices());
        }

        //For really simple meshes it seems like the "Simplified" version is actually larger than the original.
        if(simplifiedConvexShape && originalConvexShape->getNumVertices() > simplifiedConvexShape->getNumVertices())
        {
            if(originalConvexShape)
                delete originalConvexShape;

            return simplifiedConvexShape;
        }
        else
        {
            if(simplifiedConvexShape)
                delete simplifiedConvexShape;

            return originalConvexShape;
        }
    }

    /**
     \brief Takes an OSG node and finds a shrunken convex hull collision shape. When you create a btConvexHullShape
     it adds a margin around it that increases the total volume compared to the original mesh passed in. That is why
     we have to try and shrink the convex hull. However, it is still not perfect. Also, any convex mesh will end up 
     being a rough approximation because convex hull itself is optimized into 42 spherical point tests that wrap around
     the part, so you are losing a lot of data right there. So this convex hull will always at best be a rough approximation
     of the actual volume of a mesh. Once the convex hull is created it loops through all vertices to find the centroid. 
     It then loops through all triangles of the hull and creates a pyramid for each one with the triangle as the base and the
     centriod as the tip. It calculates the volume for all such pyrmaids in the hull and adds them to get an approximation of the
     volume for that hull.
    
     \author    David Cofer
     \date  10/15/2013
    
     \param [in,out]    node    Osg node that we need the volume for.
    
     \return   An approximation of the total volume of a mesh.
     */

    float BULLET_PORT OsgConvexHullVolume( osg::Node* node)
    {
        btConvexHullShape *lpHull = OsgConvexShrunkenHullCollisionShape(node);

        if(!lpHull)
            return 0;

        btShapeHull sh( lpHull );
        sh.buildHull( btScalar( 0. ) );
	    int nVerts( sh.numVertices () );
	    int nIdx( sh.numIndices() );
        if( (nVerts <= 0) || (nIdx <= 0) )
            return( NULL );

        const btVector3* bVerts( sh.getVertexPointer() );
        const unsigned int* bIdx( sh.getIndexPointer() );

        osg::ref_ptr<osg::Vec3Array> v(new osg::Vec3Array);
        v->resize( nVerts );
        unsigned int idx;
        for( idx = 0; idx < (unsigned int)nVerts; idx++ )
            ( *v )[ idx ] = osgbCollision::asOsgVec3( bVerts[ idx ] );

        //Find centriod
        float fltXSum = 0, fltYSum=0, fltZSum=0;
        for( idx = 0; idx < (unsigned int)nVerts; idx++ )
        {
            fltXSum += (*v)[idx][0];
            fltYSum += (*v)[idx][1];
            fltZSum += (*v)[idx][2];
        }

        //Find halway point between first and last vertex to use as the center of our pyrmaids.
        //osg::Vec3 vCenterPoint = vFirst + ((vLast - vFirst)/2.0);
        osg::Vec3 vCenterPoint( (fltXSum/nVerts), (fltYSum/nVerts), (fltZSum/nVerts)); // = vFirst + ((vLast - vFirst)/2.0);

        float fltTotalVolume = 0;
        if(nIdx >= 3)
        {
            for( idx = 2; idx < (unsigned int)nIdx; idx+=3 )
            {
                unsigned int iIdx1 = bIdx[ idx-2 ];
                osg::Vec3 v1 = (*v)[iIdx1];

                unsigned int iIdx2 = bIdx[ idx-1 ];
                osg::Vec3 v2 = (*v)[iIdx2];

                unsigned int iIdx3 = bIdx[ idx-0 ];
                osg::Vec3 v3 = (*v)[iIdx3];

                OsgAnimatSim::Visualization::OsgPyramid p(vCenterPoint, v1, v2, v3);

                fltTotalVolume += p.Volume();
            }
        }

        if(lpHull)
            delete lpHull;

        return fltTotalVolume;
    }

    /**
     \brief This method takes an osg node and creates a btConvexHullShape that is shrunken back down by its margin value.
     This is so the margin does not play as big of a role in the calculation of the volume of the mesh.
    
     \author    David Cofer
     \date  10/15/2013
    
     \param [in,out]    node    Osg node for which we want to calculate the volume.
    
     \return    Shrunken convex hull shape.
     */

    btConvexHullShape* OsgConvexShrunkenHullCollisionShape( osg::Node* node )
    {
        btConvexHullShape *lpHull = OsgMeshToConvexHull(node, true, 0);

        const btVector3 *aryPoints = lpHull->getUnscaledPoints();
        int iVerts = lpHull->getNumVertices();

        // Convert verts to array of Bullet scalars.
        btAlignedObjectArray<btVector3> aryVerts;
        for( int iIdx=0; iIdx<iVerts; iIdx++ )
        {
            btVector3 vVert = aryPoints[iIdx];
            aryVerts.push_back(btVector3(vVert[0], vVert[1], vVert[2]) );
        }

        delete lpHull;

        btAlignedObjectArray<btVector3> aryPlaneEquations;

        btGeometryUtil::getPlaneEquationsFromVertices(aryVerts, aryPlaneEquations);

        btConvexHullShape *lpConvexShape = new btConvexHullShape();
        float fltMargin = lpConvexShape->getMargin();
        //lpConvexShape->setMargin(fltMargin/2);
        lpConvexShape->setMargin(0);

        int sz = aryPlaneEquations.size();
        bool tooSmall = false;
        for (int i=0 ; i<sz ; ++i) {
           if ((aryPlaneEquations[i][3] += lpConvexShape->getMargin()) >= 0) {
              tooSmall = true;
              break;
           }
        }

        if (!tooSmall) {
           aryVerts.clear();
           btGeometryUtil::getVerticesFromPlaneEquations(aryPlaneEquations, aryVerts);
        }

        sz = aryVerts.size();
        for (int i=0 ; i<sz ; ++i) {
           lpConvexShape->addPoint(aryVerts[i]);
        }

        return lpConvexShape;
    }


#pragma endregion

        btVector3 Vec3AnimatToBullet(const CStdFPoint &vPoint)
        {
            btVector3 vVal(vPoint.x, vPoint.y, vPoint.z);
            return vVal;
        }

        CStdFPoint Vec3BulletToAnimat(const btVector3 &vPoint)
        {
            CStdFPoint vVal(vPoint[0], vPoint[1], vPoint[2]);
            return vVal;
        }

	}			// Environment
//}				//BulletAnimatSim

}