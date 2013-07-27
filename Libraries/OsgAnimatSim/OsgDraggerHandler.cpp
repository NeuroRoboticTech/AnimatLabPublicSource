#include "StdAfx.h"

#include "OsgMouseSpring.h"
#include "OsgDraggerHandler.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

OsgDraggerHandler::OsgDraggerHandler(Simulator *lpSim, osgViewer::Viewer *osgViewer, osg::Viewport *osgViewport)
{
	m_lpSim = lpSim;
	m_osgViewer = osgViewer;
	m_osgViewport = osgViewport;
	m_osgActiveDragger = NULL;
}

OsgDraggerHandler::~OsgDraggerHandler(void)
{
}


bool OsgDraggerHandler::handle(const GUIEventAdapter& ea, GUIActionAdapter& aa)
{
	bool bHandled = false;

	try
	{
		float x = ea.getX();
		float y = ea.getY();

		//Take an action based on the event type
		switch(ea.getEventType())
		{
			case(GUIEventAdapter::PUSH):
			{
				pick(ea, aa);
				break;
			}

			case(GUIEventAdapter::RELEASE):
			{
				if(m_osgActiveDragger)
				{
                    m_osgManipInfo._hitIter = m_osgManipInfo._hitList.begin();
					m_osgManipInfo.setCamera((osg::Camera *) m_osgViewer->getCamera());
                    m_osgManipInfo.setMousePosition(ea.getX(), ea.getY());

                    bHandled = m_osgActiveDragger->handle(m_osgManipInfo, ea, aa);
					EndGripDrag();
				}
				break;
			}

			case(GUIEventAdapter::DRAG):
			{	

				//If a grip element is selected then process that.
				if(m_osgActiveDragger)
				{
                    m_osgManipInfo._hitIter = m_osgManipInfo._hitList.begin();
					m_osgManipInfo.setCamera((osg::Camera *) m_osgViewer->getCamera());
                    m_osgManipInfo.setMousePosition(ea.getX(), ea.getY());

                    bHandled = m_osgActiveDragger->handle(m_osgManipInfo, ea, aa);
				}
				break;
			}

			default:
				bHandled = false;
		}//end switch statement

		//If this was a release event then make sure the dragger stuff gets reset.
        if (ea.getEventType() == osgGA::GUIEventAdapter::RELEASE)
        {
            m_osgActiveDragger = NULL;
            m_osgManipInfo.reset();
        }

	}  //Eat any erorrs here.
	catch(CStdErrorInfo oError)
	{return FALSE;}
	catch(...)
	{return FALSE;}

	return bHandled;
}

void OsgDraggerHandler::EndGripDrag()
{
	if(m_osgActiveDragger && m_osgActiveDragger->getUserData())
	{
		OsgUserData *osgData = dynamic_cast<OsgUserData *>(m_osgActiveDragger->getUserData());
		if(osgData && osgData->GetMovable())
		{
			OsgMovableItem *lpItem = osgData->GetVsMovable();
			lpItem->EndGripDrag();
		}
	}
}

void OsgDraggerHandler::pick(const osgGA::GUIEventAdapter& ea, GUIActionAdapter& aa)
{
    osgUtil::LineSegmentIntersector::Intersections intersections;

    float x = ea.getX();
    float y = ea.getY();	

	//float local_x, local_y = 0.0;    
    const osg::Camera* camera = m_osgViewer->getCamera();
	
	osgUtil::LineSegmentIntersector::CoordinateFrame cf = camera->getViewport() ? osgUtil::Intersector::WINDOW : osgUtil::Intersector::PROJECTION;
    osg::ref_ptr< osgUtil::LineSegmentIntersector > picker = new osgUtil::LineSegmentIntersector(cf, x, y);

	m_osgManipInfo.reset();

	RigidBody *lpBody = NULL;
	Joint *lpJoint = NULL;
    if (m_osgViewer->computeIntersections(x,y,intersections))
    {
		m_osgManipInfo.setCamera((osg::Camera *) camera);
		m_osgManipInfo.setMousePosition(x, y);

#pragma region LineSegmentIntersector

        for(osgUtil::LineSegmentIntersector::Intersections::iterator hitr = intersections.begin();
            hitr != intersections.end();
            ++hitr)
        {	
			//Add the intersection to the manipulator info
            m_osgManipInfo.addIntersection(hitr->nodePath, hitr->getLocalIntersectPoint());
        }
#pragma endregion

		for (osg::NodePath::iterator itr = m_osgManipInfo._hitList.front().first.begin();
			 itr != m_osgManipInfo._hitList.front().first.end();
			 ++itr)
		{
			osgManipulator::Dragger* dragger = dynamic_cast<osgManipulator::Dragger*>(*itr);
			if (dragger)
			{

				dragger->handle(m_osgManipInfo, ea, aa);
				m_osgActiveDragger = dragger;
				break;
			}                   
		}

	}   
}

	}// end Visualization
}// end OsgAnimatSim