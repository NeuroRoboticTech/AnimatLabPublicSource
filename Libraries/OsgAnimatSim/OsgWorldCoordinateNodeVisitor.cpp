#include "StdAfx.h"
#include "OsgWorldCoordinateNodeVisitor.h"


namespace OsgAnimatSim
{
	namespace Visualization
	{


void OsgWorldCoordinateNodeVisitor::apply(osg::Node &node)
{
	if (!done)
	{
		if ( 0 == node.getNumParents() ) // no parents
		{		
			wcMatrix.set( osg::computeLocalToWorld(this->getNodePath()) );

			done = true;
		}
		traverse(node);
	}
}

	}// end Visualization
}//end OsgAnimatSim