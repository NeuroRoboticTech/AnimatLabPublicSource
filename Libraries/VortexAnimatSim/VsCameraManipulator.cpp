#include "StdAfx.h"

#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsLight.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsCameraManipulator::VsCameraManipulator(Simulator *lpSim, osgViewer::Viewer *osgViewer, osg::Viewport *osgViewport)
{
	m_lpSim = lpSim;
	m_osgViewer = osgViewer;
	m_osgViewport = osgViewport;
	m_lpPicked = NULL;

	m_bShiftDown = false;
	m_bControlDown = false;
	m_bInDrag = false;

	m_fltPrevX = 0;
	m_fltPrevY = 0;

	m_v3Eye.set(0,0, 10);

    //Set this very small to prevent it from keeping you from zooming in as close as you need to for a part.
    this->setMinimumZoomScale(0.00001);
}

VsCameraManipulator::~VsCameraManipulator(void)
{
	VsMouseSpring::GetInstance()->SetRigidBody(NULL);
}


bool VsCameraManipulator::handle(const GUIEventAdapter& ea, GUIActionAdapter& aa)
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
				VsMouseSpring::GetInstance()->Visible(false);

				if(!m_bInDrag)
				{
					if(m_lpPicked && m_lpSim && m_lpSim->VisualSelectionMode() != SIMULATION_SELECTION_MODE)
					{
						//Change the selected vertex of the selected part.
						m_lpPicked->SelectedVertex(m_vSelectedVertex.x(), m_vSelectedVertex.y(), m_vSelectedVertex.z(), true, true);

						if(m_lpSim->AddBodiesMode())
							m_lpPicked->AddBodyClicked(m_vPickPoint.x(), m_vPickPoint.y(), m_vPickPoint.z(), 
													   m_vPickNormal.x(), m_vPickNormal.y(), m_vPickNormal.z());
						else
							m_lpPicked->Selected(true, m_bControlDown);
					}
				}

				m_bInDrag = false;
				break;
			}

			case(GUIEventAdapter::DRAG):
			{	

				m_bInDrag = true;
				//if both left control and left shift are pressed then
				//do the mouse spring
				if(m_bShiftDown && m_bControlDown && CanDoMouseSpring())
					bHandled = DoMouseSpring(ea, x, y);				
				//if just left shift is pressed then do pan
				else if(m_bShiftDown)
				{
					DoPan(ea, x, y);				
					bHandled = true;
				}
				////if just the left control is pressed then do a zoom
				//else if(m_bLeftControl)
				//{
				//	DoZoom(ea, x, y);				
				//}
				////else just do a rotate
				//else
				//{
				//	DoRotate(ea, x, y);				
				//}			
				break;
			}

			case(GUIEventAdapter::KEYDOWN):
			{
				//std::string strText = "Key Down" + STR(ea.getKey());
				//OutputDebugString(strText.c_str());
				if (ea.getKey() == osgGA::GUIEventAdapter::KEY_Shift_L)
				{
					m_bShiftDown = true;
					//bHandled = true;
				}

				if(ea.getKey() == osgGA::GUIEventAdapter::KEY_Control_L)
				{
					m_bControlDown = true;
					//bHandled = true;
				}

				if(m_bInDrag && m_bShiftDown && m_bControlDown)
				{
					bHandled = DoMouseSpring(ea, x, y);				
					//bHandled = true;
				}
				break;
			}

			case(GUIEventAdapter::KEYUP):
			{
				//std::string strText = "Key Up" + STR(ea.getKey());
				//OutputDebugString(strText.c_str());
				if (ea.getKey() == osgGA::GUIEventAdapter::KEY_Shift_L)
				{
					m_bShiftDown = false;
					//bHandled = true;
				}

				if(ea.getKey() == osgGA::GUIEventAdapter::KEY_Control_L)
				{
					m_bControlDown = false;
					//bHandled = true;
				}       

				if(ea.getKey() == osgGA::GUIEventAdapter::KEY_F10 && m_lpSim)
					m_lpSim->ResetSimulation();

				if(ea.getKey() == osgGA::GUIEventAdapter::KEY_F5 && m_lpSim)
					m_lpSim->ToggleSimulation();

				break;
			}
			default:
				bHandled = false;
		}//end switch statement

		if(!bHandled)
			bHandled = osgGA::TrackballManipulator::handle(ea, aa);

		m_fltPrevX = x;
		m_fltPrevY = y;

	}  //Eat any erorrs here.
	catch(CStdErrorInfo oError)
	{return false;}
	catch(...)
	{return false;}

	return bHandled;
}

const osg::Camera *VsCameraManipulator::GetCamera(float x, float y, float &local_x, float &local_y)
{
    const osg::Camera* camera = m_osgViewer->getCameraContainingPosition(x, y, local_x, local_y);
	if (!camera) camera = m_osgViewer->getCamera();
	return camera;
}

const osg::Camera *VsCameraManipulator::GetCamera(float x, float y)
{
	float local_x=0, local_y=0;
	const osg::Camera* camera = m_osgViewer->getCameraContainingPosition(x, y, local_x, local_y);
	if (!camera) camera = m_osgViewer->getCamera();
	return camera;
}

void VsCameraManipulator::pick(const osgGA::GUIEventAdapter& ea, GUIActionAdapter& aa)
{
    osgUtil::LineSegmentIntersector::Intersections intersections;

	m_lpPicked = NULL;

	//We have clicked something else so set the rigid body to null.
	VsMouseSpring::GetInstance()->SetRigidBody(NULL);

    float x = ea.getX();
    float y = ea.getY();	

	float local_x, local_y = 0.0;    
    const osg::Camera* camera = GetCamera(x, y, local_x, local_y);
	
	osgUtil::LineSegmentIntersector::CoordinateFrame cf = camera->getViewport() ? osgUtil::Intersector::WINDOW : osgUtil::Intersector::PROJECTION;
    osg::ref_ptr< osgUtil::LineSegmentIntersector > picker = new osgUtil::LineSegmentIntersector(cf, local_x, local_y);

	RigidBody *lpBody = NULL;
	Joint *lpJoint = NULL;
	Structure *lpStruct = NULL;
	Light *lpLight = NULL;
    if (m_osgViewer->computeIntersections(x,y,intersections))
    {

#pragma region LineSegmentIntersector

        for(osgUtil::LineSegmentIntersector::Intersections::iterator hitr = intersections.begin();
            hitr != intersections.end();
            ++hitr)
        {	
			//Look for the first drawable object that has a defined user data.
            if (hitr->drawable.valid() && hitr->drawable->getUserData())
			{
				//If the user data is a VsOsgUserData object then lets use it.
				VsOsgUserData *osgData = dynamic_cast<VsOsgUserData *>(hitr->drawable->getUserData());				
				
				if(osgData && m_lpSim)
				{
					lpBody = osgData->GetBody();
					lpJoint = osgData->GetJoint();
					lpStruct = osgData->GetStructure();
					lpLight = osgData->GetLight();

					m_vPickPoint = hitr->getWorldIntersectPoint();
					m_vPickNormal = hitr->getWorldIntersectNormal();
					m_vSelectedVertex = FindSelectedVertex(hitr);

					switch (m_lpSim->VisualSelectionMode())
					{
						case GRAPHICS_SELECTION_MODE:
						case COLLISION_SELECTION_MODE:
							//if(lpBody)
							//{ //For debugging.
							//	bool bManip = lpBody->AllowMouseManipulation();
							//	bool bVisible = lpBody->IsVisible();
							//	int iType = lpBody->VisualSelectionType();
							//	int iMode = m_lpSim->VisualSelectionMode();
                            //  iMode=iType;
							//}

							if(lpBody && lpBody->AllowMouseManipulation() && lpBody->IsVisible() && (lpBody->VisualSelectionType() & m_lpSim->VisualSelectionMode()) )
							{
								m_lpPicked = lpBody;
								return;
							}
							if(lpStruct && lpStruct->IsVisible() )
							{
								m_lpPicked = lpStruct;
								return;
							}
							if(lpLight && lpLight->IsVisible() )
							{
								m_lpPicked = lpLight;
								return;
							}
							break;

						case JOINT_SELECTION_MODE:
							if(lpJoint && lpJoint->AllowMouseManipulation() && lpJoint->IsVisible() && (lpJoint->VisualSelectionType() & m_lpSim->VisualSelectionMode()) )
							{
								m_lpPicked = lpJoint;
								return;
							}
							break;

						case RECEPTIVE_FIELD_SELECTION_MODE:
							if(lpBody && lpBody->AllowMouseManipulation() && lpBody->IsVisible() && (lpBody->VisualSelectionType() == COLLISION_SELECTION_MODE) )
							{
								m_lpPicked = lpBody;
								return;
							}
							break;

						case SIMULATION_SELECTION_MODE:
							if(lpBody && lpBody->AllowMouseManipulation() && lpBody->IsVisible() && (lpBody->VisualSelectionType() & m_lpSim->VisualSelectionMode()) )
							{
								VsMouseSpring::GetInstance()->SetRigidBody(osgData->GetVsBody());
								VsMouseSpring::GetInstance()->SetGrabPosition(hitr->getLocalIntersectPoint());            
								m_lpPicked = osgData->GetBody();
								return;
							}
							break;

						default:
							break;
					}
				}
			}
        }
#pragma endregion

	}   

	//If nothing was picked then set the mouse spring body to null
	VsMouseSpring::GetInstance()->SetRigidBody(NULL);
	m_lpPicked = NULL;
}

osg::Vec3 VsCameraManipulator::FindSelectedVertex(osgUtil::LineSegmentIntersector::Intersections::iterator &hitr)
{
	//Get the selected vertex.
	const osgUtil::LineSegmentIntersector::Intersection::IndexList& vil = hitr->indexList;
	const osgUtil::LineSegmentIntersector::Intersection::RatioList& ral = hitr->ratioList;
	osg::Geometry *osgGeom = dynamic_cast<osg::Geometry *>(hitr->drawable.get());
	if(vil.size() > 0 && osgGeom)
	{
		//First get the vertex array.
		osg::ref_ptr<osg::Vec3Array> aryVert = dynamic_cast<osg::Vec3Array*>(osgGeom->getVertexArray());

		//Then we need to find the vertex that was closest to the hit. We do this by first interpolating the 
		//hit position using the barycentric coords. Then find the vertex that is closest to that point.
		if(vil.size() != ral.size())
			THROW_ERROR(Vs_Err_lHitArrayMismatch, Vs_Err_strHitArrayMismatch);

		int iSize = vil.size();
		osg::Vec3 osgHit(0, 0, 0);
		for(int iIdx=0; iIdx<iSize; iIdx++)
		{
			osgHit += ((*aryVert)[vil[iIdx]])*ral[iIdx];
		}

		double dblMin = 10000, dblDist = 0;
		osg::Vec3 vSelVert;
		for(int iIdx=0; iIdx<iSize; iIdx++)
		{
			osg::Vec3 osgVert = (*aryVert)[vil[iIdx]];
			dblDist = Std_CalculateDistance(osgHit.x(), osgHit.y(), osgHit.z(), osgVert.x(), osgVert.y(), osgVert.z());
			if(dblDist < dblMin)
			{
				dblMin =  dblDist;
				vSelVert = osgVert;
			}
		}
		
		return vSelVert;
	}
	else
		return osg::Vec3(0, 0, 0);

}

/*
void VsCameraManipulator::GetPickedFace(int iIndex, osg::Geometry *osgGeo)
{
	osg::Vec3Array *aryVerts = (osg::Vec3Array *) geo->getVertexArray();
	iSize = aryVerts->getNumElements();
	if(iIndex >=0 && iIndex < iSize)
	{
		m_vPickedFace = (osg::Vec3) aryVerts->at(iIndex);
		
	}
}
*/

bool VsCameraManipulator::CanDoMouseSpring()
{
	VsRigidBody *osgRBBody = VsMouseSpring::GetInstance()->GetRigidBody();

	if (!osgRBBody)
		return false;

	RigidBody *rbBody = dynamic_cast<RigidBody *>(osgRBBody);
	
	if (!rbBody)
		return false;

	return true;
}

bool VsCameraManipulator::DoMouseSpring(const GUIEventAdapter& ea, float x, float y)
{	
	VsMouseSpring::GetInstance()->Visible(false);

	VsRigidBody *osgRBBody = VsMouseSpring::GetInstance()->GetRigidBody();

	if (!osgRBBody)
		return false;

	RigidBody *rbBody = dynamic_cast<RigidBody *>(osgRBBody);
	
	if (!rbBody)
		return false;

	VsMouseSpring::GetInstance()->Visible(true);

	//char strDest[150];

	//get the grab position on the body
	osg::Vec3 vGrabPos = VsMouseSpring::GetInstance()->GetGrabPosition() *osgRBBody->GetCameraMatrixTransform()->getWorldMatrices().at(0);
	VsMouseSpring::GetInstance()->SetStart(vGrabPos);

	//get the 3D mouse coords
	osg::Vec3 v3End = ConvertMouseTo3D(ea, x, y, vGrabPos);
	VsMouseSpring::GetInstance()->SetEnd(v3End);

	//Calculate the force from the spring
	osg::Vec3 vSpringLength = ((v3End - vGrabPos)); 
	osg::Vec3 vSpringForce = vSpringLength * m_lpSim->MouseSpringStiffness();

	//sprintf(strDest, "Spring Force: (%3.3f, %3.3f, %3.3f)\n", vSpringForce[0], vSpringForce[1], vSpringForce[2]);
	//OutputDebugString(strDest);

	//And the force from damping of the spring.
	CStdFPoint fpBodyVel = rbBody->GetVelocityAtPoint(vGrabPos.x(), vGrabPos.y(), vGrabPos.z());
	osg::Vec3 v3BodyVel(fpBodyVel.x, fpBodyVel.y, fpBodyVel.z);
	osg::Vec3 v3DampForce = (v3BodyVel) * m_lpSim->MouseSpringDamping();

	//sprintf(strDest, "Damping Force: (%3.3f, %3.3f, %3.3f)\n", v3DampForce[0], v3DampForce[1], v3DampForce[2]);
	//OutputDebugString(strDest);

	//Then the total force = Spring force - damping force
	osg::Vec3 vTotalForce = vSpringForce - v3DampForce;
	rbBody->AddForce(vGrabPos.x(), vGrabPos.y(), vGrabPos.z(), vTotalForce.x(), vTotalForce.y(), vTotalForce.z(), false);

	//sprintf(strDest, "Length: (%3.3f, %3.3f, %3.3f)   MS: (%3.1f, %3.1f, %3.1f)-(%3.1f, %3.1f, %3.1f)  FORCE: (%3.1f, %3.1f, %3.1f)\n\n", vSpringLength[0], vSpringLength[1], vSpringLength[2], vGrabPos[0], vGrabPos[1], vGrabPos[2], v3End[0], v3End[1], v3End[2], vTotalForce[0], vTotalForce[1], vTotalForce[2]);
	//OutputDebugString(strDest);

	return true;
}

osg::Vec3 VsCameraManipulator::ConvertMouseTo3D(const GUIEventAdapter& ea, int x, int y, osg::Vec3 vGrabPos)
{	

	//get the camera of the view
	const osg::Camera *camera = m_osgViewer->getCamera();
	
	//get the viewport of the camera
	const osg::Viewport* viewport = camera->getViewport();

	if(!viewport)
		viewport = m_osgViewport.get();

	//convert normalized mouse points to pixels
	float mx = viewport->x() + (int)((float)viewport->width()*(ea.getXnormalized()*0.5f+0.5f));
    float my = viewport->y() + (int)((float)viewport->height()*(ea.getYnormalized()*0.5f+0.5f));

	//compute the final window matrix
	osg::Matrix matrix = camera->getViewMatrix() * camera->getProjectionMatrix() * viewport->computeWindowMatrix();

	//invert it so I can get world coords from the window matrix for the mouse
	osg::Matrix inverseWM ;  	
	inverseWM.invert(matrix);

	// x, y come from ea.getX() and ea.getY()
	osg::Vec3 near_point = osg::Vec3(mx, my, 0.0) * inverseWM;
	osg::Vec3 far_point  = osg::Vec3(mx, my, 1.0) * inverseWM;
	float mouseDistance = (near_point - vGrabPos).length();

	osg::Vec3 clickDir = far_point - near_point;
	clickDir.normalize();

	osg::Vec3 desiredPosition = near_point + (clickDir * mouseDistance);

	//char strDest[150];
	//sprintf(strDest, "XY: (%3.1f, %3.1f, %3.1f),(%3.1f, %3.1f, %3.1f)\n", vGrabPos[0], vGrabPos[1], vGrabPos[2], desiredPosition[0], desiredPosition[1], desiredPosition[2]);
	//OutputDebugString(strDest);


	return desiredPosition;
}

void VsCameraManipulator::DoPan(const GUIEventAdapter& ea, float x, float y)
{
	float dx = x - m_fltPrevX;
	float dy = y - m_fltPrevY;	

	unsigned int buttonMask = ea.getButtonMask();
	if(buttonMask==(GUIEventAdapter::LEFT_MOUSE_BUTTON))
	{	
		
		osg::Vec3 v3Eye(dx, 0, dy);
		m_v3Eye = m_v3Eye - v3Eye;
	}
}

void VsCameraManipulator::DoZoom(const GUIEventAdapter& ea, float x, float y)
{	
	float dy = y - m_fltPrevY;	

	unsigned int buttonMask = ea.getButtonMask();
	if(buttonMask==(GUIEventAdapter::LEFT_MOUSE_BUTTON))
	{			
		osg::Vec3 v3Eye(0, 0, dy);
		m_v3Eye = m_v3Eye + v3Eye;
	}
}

void VsCameraManipulator::DoRotate(const GUIEventAdapter& ea, float x, float y)
{	
	float dx = x - m_fltPrevX;
	float dy = y - m_fltPrevY;	

	unsigned int buttonMask = ea.getButtonMask();
	if(buttonMask==(GUIEventAdapter::LEFT_MOUSE_BUTTON))
	{	
		osg::Matrix rotation_matrix(m_quatRotation);

		osg::Vec3 uv = osg::Vec3(0.0f,1.0f,0.0f)*rotation_matrix;
		osg::Vec3 sv = osg::Vec3(1.0f,0.0f,0.0f)*rotation_matrix;
		osg::Vec3 lv = osg::Vec3(0.0f,0.0f,-1.0f)*rotation_matrix;

		osg::Vec3 p2 = sv * x + uv * y - lv * tb_project_to_sphere(0.8f, x, y);
	    osg::Vec3 p1 = sv * m_fltPrevX + uv * m_fltPrevY - lv * tb_project_to_sphere(0.8f, m_fltPrevX, m_fltPrevY);

		osg::Vec3 axis;
		axis = p2^p1;
		axis.normalize();		 
		
		float t = (p2 - p1).length() / (2.0 * 0.8f);
		
		if (t > 1.0) t = 1.0;
		if (t < -1.0) t = -1.0;
		
		float angle = osg::inRadians(asin(t));

		osg::Quat new_rotate;
        new_rotate.makeRotate(angle,axis);
        
        m_quatRotation = m_quatRotation*new_rotate;

		/*osg::Matrixd matRot;
		matRot =  osg::Matrix::rotate(-dx, osg::Vec3(0,1,0)) *
					osg::Matrix::rotate(dy, osg::Vec3(1,0,0));

		osg::Quat q = matRot.getRotate();
		m_quatRotation = m_quatRotation * q;*/
	}	
}

float VsCameraManipulator::tb_project_to_sphere(float r, float x, float y)
{
    float d, t, z;

    d = sqrt(x*x + y*y);
                                 /* Inside sphere */
    if (d < r * 0.70710678118654752440)
    {
        z = sqrt(r*r - d*d);
    }                            /* On hyperbola */
    else
    {
        t = r / 1.41421356237309504880;
        z = t*t / d;
    }
    return z;
}

	}// end Visualization
}// end VortexAnimatSim