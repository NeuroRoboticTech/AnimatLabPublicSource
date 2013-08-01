#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgOrganism.h"
#include "OsgStructure.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"
#include "OsgMeshMinVertexDistanceVisitor.h"

#include "OsgLinearPath.h"
#include "OsgSimulationWindow.h"
#include "OsgScriptedSimulationWindow.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::OsgLinearPath
///
/// description: constructor
///
OsgLinearPath::OsgLinearPath()
:   osg::Referenced(),   
	AnimatBase(), 	
    m_ControlPoints(),
    m_CurveIsValid(false),
    m_Interpolated(0.0, 0.0, 0.0),
	m_lpTrackBody(NULL),
	m_bVisible(true),
	m_bVisibleInSim(false),
	m_bShowWaypoints(false),
	m_dblStartTime(0),
	m_dblEndTime(1),
	m_lpParentWindow(NULL),
	m_lpCurrentWaypoint(NULL),
	m_iCurrentWaypointIdx(-1)
{
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::~OsgLinearPath
///
/// description: destructor
///
OsgLinearPath::~OsgLinearPath()
{
	//Do not delete
	m_lpTrackBody = NULL;
	m_lpCurrentWaypoint = NULL;
	RemoveCurve();
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::AddControlPoint
///
/// description: add a control point
///
/// param: point - new control point to add to the list\n
/// param: time - time of arrivate at control point\n
///
/// return: true if successfully added control point, else false
///
bool OsgLinearPath::AddControlPoint(const osg::Vec3d point, 
                                  const double time)
{
    bool ret = false;

    osg::ref_ptr<ControlPoint> p = new ControlPoint(
        time, point, osg::Vec3d(0.0, 0.0, 0.0));

    ret = AddControlPoint(p.get());

    return ret;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::AddControlPoint
///
/// description: add a control point
///
/// param: p - control point to add\n
///
/// return: true if successfully added control point, else false
///
bool OsgLinearPath::AddControlPoint(ControlPoint *p)
{
    bool ret = false;

    assert(p != NULL);

    if ((p != NULL) && (p->m_T >= 0.0)) {

        bool valid = true;
        unsigned int afterI = -1;

        for (unsigned int i = 0; i < m_ControlPoints.size(); ++i) {
            if (p->m_T < m_ControlPoints[i]->m_T) {
                // insert point will be previous index
                afterI = i - 1;
                break;
            } else if (p->m_T == m_ControlPoints[i]->m_T) {
                // invalid time
                valid = false;
                break;
            }

            // update insert point
            afterI = i;
        }

        if (valid) {

            // index to insert waypoint into
            unsigned int index = afterI + 1;

            // insert the new waypoint into the list
            PointListType::iterator iter = m_ControlPoints.begin() + index;
            m_ControlPoints.insert(iter, p);
			ret = true;
        }
    }

    // note curve as invalid
    InvalidateCurve();

    return ret;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::ClearAllControlPoints
///
/// description: clear all control points 
///
void OsgLinearPath::ClearAllControlPoints()
{
    // clear the list of control points
    m_ControlPoints.clear();

    // note curve as invalid
    InvalidateCurve();
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::InvalidateCurve
///
/// description: mark curve as invalid (needs to be regenerated)
///
void OsgLinearPath::InvalidateCurve()
{
    // curve is not valid
    m_CurveIsValid = false;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::BuildCurve
///
/// description: Calculate unit-vectors and velocities between points. 
///
bool OsgLinearPath::BuildCurve()
{
	int iCount = m_ControlPoints.size();
	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		ControlPoint *lpP0 = m_ControlPoints[iIdx].get();

		//If there are more points then calculate vel and unit vector to next one.
		if(iIdx<(iCount-1))
		{
			ControlPoint *lpP1 = m_ControlPoints[iIdx+1].get();
			
			lpP0->m_vDist = lpP1->m_Pos - lpP0->m_Pos;
			lpP0->m_dblDist = sqrt(pow(lpP0->m_vDist.x(), 2.0) + pow(lpP0->m_vDist.y(), 2.0) + pow(lpP0->m_vDist.z(), 2.0));
			lpP0->m_V = lpP0->m_vDist / (lpP1->m_T - lpP0->m_T);
			lpP0->m_Uv = lpP0->m_vDist;
			lpP0->m_Uv.normalize();
			lpP0->m_Tnext = lpP1->m_T;
		}
		//Otherwise set them to 0
		else
		{
			lpP0->m_vDist.set(0, 0, 0);
			lpP0->m_dblDist = 0;
			lpP0->m_V.set(0, 0, 0);
			lpP0->m_Uv.set(0, 0, 0);
			lpP0->m_Tnext = lpP0->m_T;
		}
	}

    // mark curve as valid ready for interpolation
    m_CurveIsValid = true;
    return true;
}

osg::Vec3d OsgLinearPath::GetPositionAtTime(const double t)
{
	Interpolate(t);
	return GetInterpPosition();
}

void OsgLinearPath::FindCurrentWaypoint(const double t)
{
	int iCount = m_ControlPoints.size();
	if(m_lpCurrentWaypoint && (t > m_lpCurrentWaypoint->m_Tnext) && (m_iCurrentWaypointIdx < (iCount-1)) )
	{
		m_iCurrentWaypointIdx++;
		m_lpCurrentWaypoint = m_ControlPoints[m_iCurrentWaypointIdx].get();

		//If the new one is within the specified time then return
		if(m_lpCurrentWaypoint && ( (t >= m_lpCurrentWaypoint->m_T) && (t <= m_lpCurrentWaypoint->m_Tnext) ) )
			return;
	}

	//If we got here then we could not find the waypoint the easy way, so just go through the list of them.
	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		ControlPoint *lpP0 = m_ControlPoints[iIdx].get();
		
		if( (t >= lpP0->m_T) && (t <= lpP0->m_Tnext) )
		{
			m_iCurrentWaypointIdx= iIdx;
			m_lpCurrentWaypoint = lpP0;
			return;
		}
	}

	//If we got here we could not find anything, so set it to null
	m_iCurrentWaypointIdx = -1;
	m_lpCurrentWaypoint = NULL;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::Interpolate
///
/// description: Interpolate spline at time t
/// 
///
/// param: t - interpolation time in seconds \n
///
/// return: true if successfully interpolated the spline, else false
///
bool OsgLinearPath::Interpolate(const double t)
{
    double interpTime = t;

    // clear last interpolated values
    m_Interpolated.set(0.0, 0.0, 0.0);

    if (!m_CurveIsValid) {
        // reconstruct the curve
        BuildCurve();
    }

	if(!m_lpCurrentWaypoint || (m_lpCurrentWaypoint && ( (t < m_lpCurrentWaypoint->m_T) || (t > m_lpCurrentWaypoint->m_Tnext) ) ) )
		FindCurrentWaypoint(t);

	if(m_lpCurrentWaypoint)
	{
		//Time from start of this waypoint
		double dblWPT = (t-m_lpCurrentWaypoint->m_T);
		m_Interpolated = (m_lpCurrentWaypoint->m_V*dblWPT) + m_lpCurrentWaypoint->m_Pos;
	}

    return true;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgLinearPath::CreateTestGeom
///
/// description: create and return a scene graph making up the curve
/// 
/// param: showControlPoints - true if scene graph includes control points\n
/// param: numSamples - number of points between each control point\n
/// param: xform - position transformation functor\n
///
/// return: scene graph root node
///
osg::Node *OsgLinearPath::CreateTestGeom(const bool showControlPoints, 
                                       const unsigned int numSamples,
                                       ControlPointTransformFunctor* xform)
{
    osg::Node *ret = NULL;

    if (!m_CurveIsValid) {
        // reconstruct the curve
        BuildCurve();
    }

	if(m_ControlPoints.size() < 2)
		return NULL;

    if (m_CurveIsValid) {

        osg::Group *group = new osg::Group();
        group->setNodeMask(0xffffffff);

        if (showControlPoints) {

            // create the control point vertexes
            osg::ref_ptr<osg::Vec3Array> controlVerts = new osg::Vec3Array;

            for (unsigned int i = 0; i < m_ControlPoints.size() - 1; ++i) {
                osg::Vec3d p1 = m_ControlPoints[i]->m_Pos;
                osg::Vec3d p2 = m_ControlPoints[i+1]->m_Pos;

                if (xform != NULL) {
                    p1 = xform->Transform(p1);
                    p2 = xform->Transform(p2);
                }

                controlVerts->push_back(p1);
                controlVerts->push_back(p2);
            }

            osg::ref_ptr<osg::Geometry> geometry = new osg::Geometry;
            geometry->setVertexArray(controlVerts.get());

            osg::ref_ptr<osg::Vec4Array> color = new osg::Vec4Array;
            color->push_back(osg::Vec4(1.0, 1.0, 1.0, 1.0));
            geometry->setColorArray(color.get());
            geometry->setColorBinding(osg::Geometry::BIND_OVERALL);

            geometry->addPrimitiveSet(
                new osg::DrawArrays(osg::PrimitiveSet::LINES, 
                    0, controlVerts->size()));

            osg::ref_ptr<osg::Geode> geode = new osg::Geode;
            geode->addDrawable(geometry.get());

            // attach geode to scene graph
            group->addChild(geode.get());

        }

        {
            // -------  Curve Geometry  ------

            // create the curve vertexes
            osg::ref_ptr<osg::Vec3Array> curveVerts = new osg::Vec3Array;

            for (unsigned int i = 0; i < m_ControlPoints.size() - 1; ++i) {

                // interpolate from one point to the next based on time

                double t = m_ControlPoints[i]->m_T;
                double dt = (m_ControlPoints[i+1]->m_T - t) / 
                    double(numSamples-1);

                for (unsigned int ti = 0; ti < numSamples; ++ti) {

                    // get next point
                    OsgLinearPath::Interpolate(t);

                    osg::Vec3d p = GetInterpPosition(); 

                    if (xform != NULL) {
                        p = xform->Transform(p);
                    }

                    // add point to the list
                    curveVerts->push_back(p);

                    if (ti == (numSamples - 1)) {
                        // last point, complete the line strip with an extra point
                        curveVerts->push_back(p);
                    }

                    // increment time
                    t += dt;

                }
            }

            osg::ref_ptr<osg::Geometry> geometry = new osg::Geometry;
            geometry->setVertexArray(curveVerts.get());

            osg::ref_ptr<osg::Vec4Array> color = new osg::Vec4Array;
            color->push_back(osg::Vec4(m_vLineColor.r(), m_vLineColor.g(), m_vLineColor.b(), m_vLineColor.a()));
            geometry->setColorArray(color.get());
            geometry->setColorBinding(osg::Geometry::BIND_OVERALL);

            geometry->addPrimitiveSet(
                new osg::DrawArrays(osg::PrimitiveSet::LINE_STRIP, 
                    0, curveVerts->size()));

            osg::ref_ptr<osg::Geode> geode = new osg::Geode;
            geode->addDrawable(geometry.get());

            // attach geode to scene graph
            group->addChild(geode.get());

            // no lighting
            group->getOrCreateStateSet()->setMode(GL_LIGHTING, 
                osg::StateAttribute::OFF);
        }

        ret = group;
    }

    return ret;
}

/**
\brief	Gets the LineColor color value. 

\author	dcofer
\date	3/2/2011

\return	Pointer to color data
**/
CStdColor *OsgLinearPath::LineColor() {return &m_vLineColor;}

/**
\brief	Sets the LineColor color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void OsgLinearPath::LineColor(CStdColor &aryColor)
{
	m_vLineColor = aryColor;
	RedrawCurve();
}

/**
\brief	Sets the LineColor color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void OsgLinearPath::LineColor(float *aryColor)
{
	CStdColor vColor(aryColor[0], aryColor[1], aryColor[2], aryColor[3], 1);
	LineColor(vColor);
}

/**
\brief	Loads the LineColor color from an XML data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/

void OsgLinearPath::LineColor(string strXml)
{
	CStdColor vColor(1);
	vColor.Load(strXml, "LineColor");
	LineColor(vColor);
}

/**
\brief	Gets the GUID ID of the part this camera will track while on this path.

\author	dcofer
\date	3/18/2011

\return	GUID ID of the part.
**/
string OsgLinearPath::PartID() {return m_strPartID;}

/**
\brief	Sets the GUID ID of the part this camera will track while on this path.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID for the part. 
**/
void OsgLinearPath::PartID(string strID)
{
	m_strPartID = strID;
}

double OsgLinearPath::StartTime() {return m_dblStartTime;}

void OsgLinearPath::StartTime(double dblTime, bool bSortPaths)
{
	Std_IsAboveMin((double) 0, dblTime, true, "StartTime", true);
	m_dblStartTime = dblTime;
	RedrawCurve();

	if(bSortPaths && m_lpParentWindow)
		m_lpParentWindow->SortPaths();
}

double OsgLinearPath::EndTime() {return m_dblStartTime;}

void OsgLinearPath::EndTime(double dblTime, bool bSortPaths)
{
	Std_IsAboveMin((double) m_dblStartTime, dblTime, true, "EndTime", true);
	m_dblEndTime = dblTime;

	if(bSortPaths && m_lpParentWindow)
		m_lpParentWindow->SortPaths();
}

bool OsgLinearPath::Visible() 
{return m_bVisible;}

void OsgLinearPath::Visible(bool bVal)
{
	m_bVisible = bVal;
	MakeVisible(bVal);
}

bool OsgLinearPath::VisibleInSim() 
{return m_bVisibleInSim;}

void OsgLinearPath::VisibleInSim(bool bVal)
{m_bVisibleInSim = bVal;}

void OsgLinearPath::MakeVisible(bool bVal)
{
	if(m_osgSpline)
	{
		if(bVal)
			m_osgSpline->setNodeMask(0x1);
		else
			m_osgSpline->setNodeMask(0x0);
	}
}

bool OsgLinearPath::ShowWaypoints() 
{return m_bShowWaypoints;}

void OsgLinearPath::ShowWaypoints(bool bVal)
{
	m_bShowWaypoints = bVal;
	RedrawCurve();
}
bool OsgLinearPath::BeforePathTime(double dblTime)
{
	if(dblTime < m_dblStartTime)
		return true;
	else
		return false;
}

bool OsgLinearPath::WithinPathTime(double dblTime)
{
	if(dblTime >= m_dblStartTime && dblTime <= m_dblEndTime)
		return true;
	else
		return false;
}

bool OsgLinearPath::AfterPathTime(double dblTime)
{
	if(dblTime > m_dblEndTime)
		return true;
	else
		return false;
}

void OsgLinearPath::RemoveCurve()
{
	OsgSimulator *lpVsSim = dynamic_cast<OsgSimulator *>(m_lpSim);

	if(lpVsSim && lpVsSim->OSGRoot())
	{
		//If we already have a curve then get rid of it first.
		if(m_osgSpline.valid())
		{
			lpVsSim->OSGRoot()->removeChild(m_osgSpline.get());
			m_osgSpline.release();
		}
	}
}

void OsgLinearPath::RedrawCurve()
{
	OsgSimulator *lpVsSim = dynamic_cast<OsgSimulator *>(m_lpSim);

	RemoveCurve();

	if(lpVsSim && lpVsSim->OSGRoot())
	{
		//Now recreate it.
		InvalidateCurve();
		m_osgSpline = CreateTestGeom(m_bShowWaypoints);
		MakeVisible(m_bVisible);
		lpVsSim->OSGRoot()->addChild(m_osgSpline.get());
	}
}


void OsgLinearPath::SimStarting()
{
	MakeVisible(m_bVisibleInSim);
}

void OsgLinearPath::ResetSimulation()
{
	MakeVisible(m_bVisible);

	int iCount = m_ControlPoints.size();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_ControlPoints[iIndex]->ResetSimulation();
}


bool OsgLinearPath::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(strType == "TRACKPARTID")
	{
		PartID(strValue);
		Initialize();
		return true;
	}

	if(strType == "STARTTIME")
	{
		StartTime((double) atof(strValue.c_str()));
		return true;
	}

	if(strType == "ENDTIME")
	{
		EndTime((double) atof(strValue.c_str()));
		return true;
	}

	if(strType == "LINECOLOR")
	{
		LineColor(strValue);
		return true;
	}
	
	if(strDataType == "LINECOLOR.RED")
	{
		float aryVal[4] = {atof(strValue.c_str()), m_vLineColor.g(), m_vLineColor.b(), m_vLineColor.a()};
		LineColor(aryVal);
		return true;
	}

	if(strDataType == "LINECOLOR.GREEN")
	{
		float aryVal[4] = {m_vLineColor.r(), atof(strValue.c_str()), m_vLineColor.b(), m_vLineColor.a()};
		LineColor(aryVal);
		return true;
	}

	if(strDataType == "LINECOLOR.BLUE")
	{
		float aryVal[4] = {m_vLineColor.r(), m_vLineColor.g(), atof(strValue.c_str()), m_vLineColor.a()};
		LineColor(aryVal);
		return true;
	}

	if(strDataType == "LINECOLOR.ALPHA")
	{
		float aryVal[4] = {m_vLineColor.r(), m_vLineColor.g(), m_vLineColor.b(), atof(strValue.c_str())};
		LineColor(aryVal);
		return true;
	}

	if(strDataType == "VISIBLE")
	{
		Visible(Std_ToBool(strValue));
		return true;
	}

	if(strDataType == "VISIBLEINSIM")
	{
		VisibleInSim(Std_ToBool(strValue));
		return true;
	}

	if(strDataType == "SHOWWAYPOINTS")
	{
		ShowWaypoints(Std_ToBool(strValue));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void OsgLinearPath::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("TrackPartID");
	aryTypes.Add("String");

	aryNames.Add("StartTime");
	aryTypes.Add("Float");

	aryNames.Add("EndTime");
	aryTypes.Add("Float");

	aryNames.Add("LineColor");
	aryTypes.Add("Xml");

	aryNames.Add("LineColor.Red");
	aryTypes.Add("Float");

	aryNames.Add("LineColor.Green");
	aryTypes.Add("Float");

	aryNames.Add("LineColor.Blue");
	aryTypes.Add("Float");

	aryNames.Add("LineColor.Alpha");
	aryTypes.Add("Float");

	aryNames.Add("VisibleInSim");
	aryTypes.Add("Boolean");
}


/**
\brief	Creates and adds a waypoint. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the waypoint. 
**/
void OsgLinearPath::AddWaypoint(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Waypoint");

	osg::ref_ptr<ControlPoint> lpWaypoint = LoadWaypoint(oXml);

	lpWaypoint->Initialize();
	RedrawCurve();
}

/**
\brief	Removes the waypoint with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the waypoint to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void OsgLinearPath::RemoveWaypoint(string strID, bool bThrowError)
{
	int iPos = FindWaypointPos(strID, bThrowError);
	m_ControlPoints.erase(m_ControlPoints.begin()+iPos);
	RedrawCurve();
}

/**
\brief	Finds the array index for the waypoint with the specified ID

\author	dcofer
\date	3/2/2011

\param	strID ID of waypoint to find
\param	bThrowError	If true and ID is not found then it will throw an error, else return NULL
\exception If bThrowError is true and ID is not found.

\return	If bThrowError is false and ID is not found returns NULL, 
else returns the pointer to the found part.
**/
int OsgLinearPath::FindWaypointPos(string strID, bool bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_ControlPoints.size();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_ControlPoints[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lBodyOrJointIDNotFound, Al_Err_strBodyOrJointIDNotFound, "ID");

	return -1;
}

bool OsgLinearPath::AddItem(const string &strItemType, const string &strXml, bool bThrowError, bool bDoNotInit)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "WAYPOINT")
	{
		AddWaypoint(strXml);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool OsgLinearPath::RemoveItem(const string &strItemType, const string &strID, bool bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "WAYPOINT")
	{
		RemoveWaypoint(strID);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

void OsgLinearPath::Initialize()
{
	AnimatBase::Initialize();

	m_lpTrackBody = NULL;
	if(!Std_IsBlank(m_strPartID))
		m_lpTrackBody = dynamic_cast<BodyPart *>(m_lpSim->FindByID(m_strPartID));

	RedrawCurve();
}

void OsgLinearPath::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Spline Element

	m_vLineColor.Load(oXml, "LineColor", false);

	EndTime(oXml.GetChildDouble("EndTime", m_dblEndTime), false);
	StartTime(oXml.GetChildDouble("StartTime", m_dblStartTime), false);
	PartID(oXml.GetChildString("LinkedBodyPartID", m_strPartID));
	VisibleInSim(oXml.GetChildBool("VisibleInSim", m_bVisibleInSim));

	if(oXml.FindChildElement("Waypoints", false))
	{
		oXml.IntoElem(); //IntoOf Waypoints Element

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadWaypoint(oXml);
		}

		oXml.OutOfElem(); //OutOf Waypoints Element
	}

	oXml.OutOfElem(); //OutOf Spline Element

    if(m_lpParentWindow)
        m_lpParentWindow->SortPaths();
}

/**
\brief	Loads a camera path waypoint. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml The xml data packet to load. 

\return	The root. 
**/
osg::ref_ptr<ControlPoint> OsgLinearPath::LoadWaypoint(CStdXml &oXml)
{
	ControlPoint *lpPoint = NULL;

try
{
	osg::ref_ptr<ControlPoint> lpPoint(new ControlPoint());

	lpPoint->SetSystemPointers(m_lpSim, NULL, NULL, NULL, true);

	lpPoint->Load(oXml);
	lpPoint->ParentSpline(this);

	if(!AddControlPoint(lpPoint.get()))
		THROW_PARAM_ERROR(Osg_Err_lUnableToAddWaypoint, Osg_Err_strUnableToAddWaypoint, "Waypoint ID", lpPoint->ID());

	return lpPoint;
}
catch(CStdErrorInfo oError)
{
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


/**
\brief	Gets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
osg::Vec3d ControlPoint::Position() {return m_Pos;}

/**
\brief	Sets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint		The new point to use to set the local position. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void ControlPoint::Position(CStdFPoint &oPoint, bool bUseScaling) 
{
	CStdFPoint oNewPoint, oReportPosition;
	if(bUseScaling && m_lpSim)
	{
		oNewPoint = oPoint * m_lpSim->InverseDistanceUnits();
		oReportPosition = oNewPoint * m_lpSim->DistanceUnits();
	}
	else
	{
		oNewPoint = oPoint;
		oReportPosition = oPoint;
	}

	m_Pos.set(oNewPoint.x, oNewPoint.y, oNewPoint.z);
	m_ReportPos.set(oReportPosition.x, oReportPosition.y, oReportPosition.z);

	if(m_lpParentSpline)
		m_lpParentSpline->RedrawCurve();
}

/**
\brief	Sets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void ControlPoint::Position(float fltX, float fltY, float fltZ, bool bUseScaling) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	Position(vPos, bUseScaling);
}

/**
\brief	Sets the local position. (m_oPosition). This method is primarily used by the GUI to
reset the local position using an xml data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the position. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void ControlPoint::Position(string strXml, bool bUseScaling)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Position");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Position", vPos);
	Position(vPos, bUseScaling);
}

double ControlPoint::Time()
{return m_T;}

void ControlPoint::Time(double dblVal)
{
	Std_IsAboveMin((double) 0, dblVal, true, "Waypoint Time", true);
	m_T = dblVal;
	if(m_lpParentSpline)
		m_lpParentSpline->RedrawCurve();
}

bool ControlPoint::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(strDataType == "POSITION")
	{
		Position(strValue);
		return true;
	}

	if(strDataType == "POSITION.X")
	{
		Position(atof(strValue.c_str()), m_ReportPos.y(), m_ReportPos.z());
		return true;
	}

	if(strDataType == "POSITION.Y")
	{
		Position(m_ReportPos.x(), atof(strValue.c_str()), m_ReportPos.z());
		return true;
	}

	if(strDataType == "POSITION.Z")
	{
		Position(m_ReportPos.x(), m_ReportPos.y(), atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "TIME")
	{
		Time((double) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}


void ControlPoint::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Position");
	aryTypes.Add("Xml");

	aryNames.Add("Position.X");
	aryTypes.Add("Float");

	aryNames.Add("Position.Y");
	aryTypes.Add("Float");

	aryNames.Add("Position.Z");
	aryTypes.Add("Float");

	aryNames.Add("Time");
	aryTypes.Add("Float");
}

void ControlPoint::Load(StdUtils::CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Spline Element

	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Position", vPos);
	Position(vPos);

	Time(oXml.GetChildDouble("Time", 0));

	oXml.OutOfElem(); //OutOf Spline Element
}

	}			// Visualization
//}				//OsgAnimatSim

}


