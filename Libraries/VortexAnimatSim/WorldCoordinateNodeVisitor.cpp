#include "StdAfx.h"
#include "WorldCoordinateNodeVisitor.h"


namespace VortexAnimatSim
{
	namespace Visualization
	{


void WorldCoordinateNodeVisitor::apply(osg::Node &node)
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

    //  {
    //     if (!done)
    //     {
    //        if ( 0 == node.getNumParents() ) // no parents
    //        {
				//
				//osg::NodePath np = this->getNodePath();

				////while(np.size() > 1)
				////	np.erase(np.begin());

				////int iVal = np.size();
				////np.erase(np.begin());
				////np.erase(np.begin());
				////np.erase(np.begin());
				////np.erase(np.begin());
    //           wcMatrix.set( osg::computeLocalToWorld(np) );
    //           done = true;
    //        }
    //        traverse(node);
    //     }
    //  }

//
//osg::Vec3 WorldCoordinateNodeVisitor::getWorldCoordinates()
//{
//	osg::Vec3 v3Coords = m_osgWCMatrix->getTrans();
//
//	return v3Coords;
//}

	}// end Visualization
}//end VortexAnimatSim