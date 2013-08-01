#include "StdAfx.h"
#include <stdarg.h>
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsClassFactory.h"
#include "VsSimulator.h"
#include "VsOsgGeometry.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

#pragma region CreateGeometry_Code


osg::Matrix VsMatrixUtil::SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot)
{
	Vx::VxReal3 vLoc = {localPos.x, localPos.y, localPos.z};
	Vx::VxReal3 vRot = {localRot.x, localRot.y, localRot.z};
	Vx::VxTransform vTrans = Vx::VxTransform::createFromTranslationAndEulerAngles(vLoc, vRot);

	osg::Matrix osgLocalMatrix;
	VxOSG::copyVxReal44_to_OsgMatrix(osgLocalMatrix, vTrans.m);

	return osgLocalMatrix;
}

CStdFPoint VsMatrixUtil::EulerRotationFromMatrix(osg::Matrix osgMT)
{
	//Now lets get the euler angle rotation
	Vx::VxReal44 vxTM;
	VxOSG::copyOsgMatrix_to_VxReal44(osgMT, vxTM);
	Vx::VxTransform vTrans(vxTM);
	Vx::VxReal3 vEuler;
	vTrans.getRotationEulerAngles(vEuler);
	CStdFPoint vRot(vEuler[0], vEuler[1] ,vEuler[2]);
	vRot.ClearNearZero();
    return vRot;
}

Vx::VxHeightField VORTEX_PORT *CreateVxHeightField(osg::HeightField *osgHeightField, float fltSegWidth, float fltSegLength, float fltBaseHeight, float fltXCenter, float fltYCenter)
{
	Vx::VxHeightField *vxHeightField = new Vx::VxHeightField();

	VxArray<VxReal> vHeights;

	//Lets create the height array.
	int iCols = osgHeightField->getNumColumns();
	int iRows = osgHeightField->getNumRows();
	for(int iRow=0; iRow<iRows; iRow++)
		for(int iCol=0; iCol<iCols; iCol++)
			vHeights.push_back(osgHeightField->getHeight(iCol, iRow) + fltBaseHeight);

	int iSize = vHeights.size();
	osg::Vec3 vCenter(fltXCenter, fltYCenter, 0);
	osg::Vec3 vOrigin = osgHeightField->getOrigin();
	vOrigin += vCenter;

	vxHeightField->build((iCols-1), (iRows-1), fltSegWidth, fltSegLength, vOrigin.x(), vOrigin.y(), vHeights);

	return vxHeightField;
}


osg::Geometry VORTEX_PORT *CreateOsgFromVxConvexMesh(Vx::VxConvexMesh *vxGeometry)
{
    // calc the vertices
	osg::ref_ptr<osg::Vec3Array> verts = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> norms = new osg::Vec3Array(); 
    osg::Geometry* osgGeom = new osg::Geometry();

	Vx::VxReal3 vVertex;
	int iCurVertex=0;
	int iPolyCount = vxGeometry->getPolygonCount();
	for(int iPoly=0; iPoly<iPolyCount; iPoly++)
	{
		int iVertexCount = vxGeometry->getPolygonVertexCount(iPoly);

		osg::ref_ptr<osg::DrawElementsUInt> osgPolygon = new osg::DrawElementsUInt(osg::PrimitiveSet::POLYGON, 0);

		for(int iVertex=0; iVertex<iVertexCount; iVertex++)
		{
			vxGeometry->getPolygonVertex(iPoly, iVertex, vVertex);
			verts->push_back( osg::Vec3( vVertex[0], vVertex[1], vVertex[2]) ); 

			osgPolygon->push_back(iCurVertex);
			iCurVertex++;
		}

		osgGeom->addPrimitiveSet(osgPolygon.get()); 

		//Get the normal for this polygon.
		vxGeometry->getPolygonNormal(iPoly, vVertex);
		norms->push_back( osg::Vec3( vVertex[0], vVertex[1], vVertex[2]) ); 
	}
 
    // create the geometry
     osgGeom->setVertexArray(verts.get());

	 osgGeom->setNormalArray(norms.get());
     osgGeom->setNormalBinding(osg::Geometry::BIND_PER_PRIMITIVE_SET);

     osg::Vec4Array* colors = new osg::Vec4Array;
     colors->push_back(osg::Vec4(1,1,1,1));
     osgGeom->setColorArray(colors);
     osgGeom->setColorBinding(osg::Geometry::BIND_OVERALL);

    return osgGeom;
}


#pragma endregion

	}			// Environment
//}				//VortexAnimatSim

}