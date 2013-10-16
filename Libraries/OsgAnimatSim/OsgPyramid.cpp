// OsgLine.cpp: implementation of the OsgLine class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgSimulator.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"
#include "OsgPyramid.h"


namespace OsgAnimatSim
{
	namespace Visualization
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

OsgPyramid::OsgPyramid()
{
}

OsgPyramid::OsgPyramid(osg::Vec3d vPoint, osg::Vec3d vBase[3])
{
    m_vPoint = vPoint;
    for(int iIdx=0; iIdx<3; iIdx++)
        m_vBase[iIdx] = vBase[iIdx];
}

OsgPyramid::OsgPyramid(osg::Vec3d vPoint, osg::Vec3d vBase1, osg::Vec3d vBase2, osg::Vec3d vBase3)
{
    m_vPoint = vPoint;
    m_vBase[0] = vBase1;
    m_vBase[1] = vBase2;
    m_vBase[2] = vBase3;
}

OsgPyramid::~OsgPyramid()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of OsgPyramid\r\n", "", -1, false, true);}
}

float OsgPyramid::Height()
{
    // Construct a vector from the Pyramid base to the top point
    float dx = m_vPoint[0] - m_vBase[0][0];
    float dy = m_vPoint[1] - m_vBase[0][1];
    float dz = m_vPoint[2] - m_vBase[0][2];
    osg::Vec3d vt(dx, dy, dz);   

    // Calculate the perpendicular
    // distance from the base to the top point.
    // The distance d is the projection of vt in the normal direction.
    // Because a right-hand coordinate system is assumed, the value of d
    // may be negative, so the absolute value is returned.
    osg::Vec3 normal = (m_vBase[1]-m_vBase[0])^(m_vBase[2]-m_vBase[1]);
    normal.normalize();

    float d = normal *vt;

    float result = 0.0; 
    if( d < 0.0 )
        result = fabs(d);
    else
        result = d;
    return result;
}

float OsgPyramid::BaseArea()
{
    float fltA = (m_vBase[0] - m_vBase[1]).length();
    float fltB = (m_vBase[1] - m_vBase[2]).length();
    float fltC = (m_vBase[2] - m_vBase[0]).length();

    float fltS = (fltA + fltB + fltC)/2.0;

    float fltVal = fltS * (fltS - fltA) * (fltS - fltB) * (fltS - fltC);
    float fltArea = sqrt(fltVal);

    return fltArea;
}

float OsgPyramid::Volume()
{
    // from the base to the top point
    float d = Height();
 
    // Calculate the area of the base
    float baseArea = BaseArea();

    // Calculate the volume of the polygon's pyramid
    float fltVolume = (d * baseArea) / 3.0;
    return fltVolume;   
}


	}			// Visualization
}				//OsgAnimatSim
