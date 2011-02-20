
#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

// Visitor to return the world coordinates of a node.
// It traverses from the starting node to the parent.
// The first time it reaches a root node, it stores the world coordinates of 
// the node it started from.  The world coordinates are found by concatenating all 
// the matrix transforms found on the path from the start node to the root node.

	class WorldCoordinateNodeVisitor : public osg::NodeVisitor 
{
public:
   WorldCoordinateNodeVisitor():
      osg::NodeVisitor(NodeVisitor::TRAVERSE_PARENTS), done(false)
      {
      }
      virtual void apply(osg::Node &node);

      osg::Matrixd MatrixTransform() 
      {
         return wcMatrix;
      }
private:
   bool done;
   osg::Matrixd wcMatrix;
};

	}// end Visualization
}//end VortexAnimatSim


