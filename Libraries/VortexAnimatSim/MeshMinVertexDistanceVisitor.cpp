#include "StdAfx.h"
#include "MeshMinVertexDistanceVisitor.h"


namespace VortexAnimatSim
{
	namespace Visualization
	{


void MeshMinVertexDistanceVisitor::FindMinVertex(osg::Geometry *osgGeom, osg::Vec3Array *aryVert)
{
	float fltDist;

	//First make a copy of the vertex array so we can mess with it.
	int iLen = aryVert->size();
	for(int iP=0; iP<iLen; iP++)
		for(int iQ=0; iQ<iLen; iQ++)
			if(iQ != iP)
			{
				osg::Vec3 vP = ((*aryVert)[iP]);
				osg::Vec3 vQ = ((*aryVert)[iQ]);
				
				fltDist = Std_CalculateDistance(vP.x(), vP.y(), vP.z(), vQ.x(), vQ.y(), vQ.z());
				//I am making 0.05f a hard-limit for the minimum distance. I don't want vertex sphere radius smaller than this.
				if(fltDist > 0.05f && fltDist < m_fltMinVertDist || m_fltMinVertDist == -1)
				{
					m_fltMinVertDist = fltDist;
					m_lpGeom = osgGeom;
					m_iP = iP;
					m_iQ = iQ;
				}
			}
}

void MeshMinVertexDistanceVisitor::apply(osg::Node &node)
{
	osg::Geode *osgGeode = dynamic_cast<osg::Geode *>(&node);

	if(osgGeode && osgGeode->getNumDrawables() > 0)
	{
		int iDrawCount = osgGeode->getNumDrawables();
		
		for(int iDraw=0; iDraw<iDrawCount; iDraw++)
		{
			osg::Geometry *osgGeom = dynamic_cast<osg::Geometry *>(osgGeode->getDrawable(iDraw));

			if(osgGeom)
			{
				osg::ref_ptr<osg::Vec3Array> aryVert = dynamic_cast<osg::Vec3Array*>(osgGeom->getVertexArray());
				FindMinVertex(osgGeom, aryVert.get());
			}
		}
	}

	traverse(node);
}

	}// end Visualization
}//end VortexAnimatSim