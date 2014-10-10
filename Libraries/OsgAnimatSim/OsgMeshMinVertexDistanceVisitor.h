
#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

// Visitor to return the world coordinates of a node.
// It traverses from the starting node to the parent.
// The first time it reaches a root node, it stores the world coordinates of 
// the node it started from.  The world coordinates are found by concatenating all 
// the matrix transforms found on the path from the start node to the root node.

class ANIMAT_OSG_PORT OsgMeshMinVertexDistanceVisitor : public osg::NodeVisitor 
{
public:
   OsgMeshMinVertexDistanceVisitor():
      osg::NodeVisitor(NodeVisitor::TRAVERSE_ALL_CHILDREN)
      {
			m_fltMinVertDist = -1;
			m_iP = -1;
			m_iQ = -1;
			m_lpGeom = NULL;
      }
      virtual void apply(osg::Node &node);

      float MinVertexDistance() 
      {
         return m_fltMinVertDist;
      }
protected:
	void FindMinVertex(osg::Geometry *osgGeom, osg::Vec3Array *aryVert);

	float m_fltMinVertDist;
	osg::Vec3 m_vP;
	osg::Vec3 m_vQ;
	int m_iP;
	int m_iQ;
	osg::Geometry *m_lpGeom;
};

	}// end Visualization
}//end OsgAnimatSim


