#include "StdAfx.h"
#include "OsgCubicSpline.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::OsgCubicSpline
///
/// description: constructor
///
OsgCubicSpline::OsgCubicSpline()
:   osg::Referenced(),   
	AnimatBase(), 	
    m_ControlPoints(),
    m_CurveIsValid(false),
    m_Yp0(0.0, 0.0, 0.0),
    m_Ypn(0.0, 0.0, 0.0),
    m_InterpolateLow(0),
    m_InterpolateHigh(0),
    m_InterpolatedY(0.0, 0.0, 0.0),
    m_InterpolatedY1(0.0, 0.0, 0.0),
    m_InterpolatedY2(0.0, 0.0, 0.0),
	m_fltStartTime(0),
	m_fltEndTime(1),
	m_lpTrackBody(NULL),
	m_bVisibleInSim(FALSE)
{
}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::~OsgCubicSpline
///
/// description: destructor
///
OsgCubicSpline::~OsgCubicSpline()
{
	//Do not delete
	m_lpTrackBody = NULL;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::AddControlPoint
///
/// description: add a control point
///
/// param: point - new control point to add to the list\n
/// param: time - time of arrivate at control point\n
///
/// return: true if successfully added control point, else false
///
bool OsgCubicSpline::AddControlPoint(const osg::Vec3d point, 
                                  const double time)
{
    bool ret = false;

    osg::ref_ptr<ControlPoint> p = new ControlPoint(
        time, point, osg::Vec3d(0.0, 0.0, 0.0));

    ret = AddControlPoint(p.get());

    return ret;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::AddControlPoint
///
/// description: add a control point
///
/// param: p - control point to add\n
///
/// return: true if successfully added control point, else false
///
bool OsgCubicSpline::AddControlPoint(ControlPoint *p)
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
        }
    }

    // note curve as invalid
    InvalidateCurve();

    return ret;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::ClearAllControlPoints
///
/// description: clear all control points 
///
void OsgCubicSpline::ClearAllControlPoints()
{
    // clear the list of control points
    m_ControlPoints.clear();

    // note curve as invalid
    InvalidateCurve();
}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::InvalidateCurve
///
/// description: mark curve as invalid (needs to be regenerated)
///
void OsgCubicSpline::InvalidateCurve()
{
    // curve is not valid
    m_CurveIsValid = false;

    // reset previously used interpolation values
    m_InterpolateLow = 0;
    m_InterpolateHigh = 0;

}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::BuildCurve
///
/// description: figure the curve's coefficients at the control points
///     so that interpolation can be accomplished.
/// 
///     Reference: spline implementation
///      Chapter 3.3 Cubic Spline Interpolation
///          http://www.nrbook.com/a/bookcpdf/c3-3.pdf
///
bool OsgCubicSpline::BuildCurve()
{
    bool ret = false;

    // use a short named handle to list of points
    PointListType &pnts = m_ControlPoints;

    // total number of points
    unsigned int n = pnts.size();

    if (n > 1) {

        // running list of coefficients
        std::vector<osg::Vec3d> u;

        // add initial u to list (referenced as u[0])
        u.push_back(osg::Vec3d(0.0, 0.0, 0.0));

        // ----- Point 0 -----

        if ((m_Yp0.x() == 0.0) && (m_Yp0.y() == 0.0) && (m_Yp0.z() == 0.0)) {
            // initial point is set to be "natural" 
            
            // initialize second derivative
            pnts[0]->m_Y2.set(0.0, 0.0, 0.0);
            // add first entry for natural control point
            u[0].set(0.0, 0.0, 0.0);
        } else {
            // initialize second derivative
            pnts[0]->m_Y2.set(-0.5, -0.5, -0.5);
            // delta time
            double dt = pnts[1]->m_T - pnts[0]->m_T;
            // delta position
            osg::Vec3d dpos = pnts[1]->m_Pos - pnts[0]->m_Pos;

            for (unsigned int j = 0; j < 3; j++) {
                u[0][j] = (3.0 / dt) * ((dpos[j] / dt) - m_Yp0[j]);
            }
        }
        
        // ----- Points 1 to n-1 -----


        for (unsigned int i = 1; i < (n - 1); i++) {
            // Decompose the tridiagonal
            
            double sig = (pnts[i]->m_T - pnts[i-1]->m_T) / 
                (pnts[i+1]->m_T - pnts[i-1]->m_T);

            // add new entry (referenced as u[i])
            u.push_back(osg::Vec3d(0.0, 0.0, 0.0));

            double dt0 = pnts[i+1]->m_T - pnts[i]->m_T;
            double dt1 = pnts[i]->m_T - pnts[i-1]->m_T;
            double dt2 = pnts[i+1]->m_T - pnts[i-1]->m_T;

            for (unsigned int j = 0; j < 3; j++) {
                double p = (sig * pnts[i-1]->m_Y2[j]) + 2.0;
                pnts[i]->m_Y2[j] = (sig - 1.0) / p;

                u[i][j] = 
                    ((pnts[i+1]->m_Pos[j] - pnts[i]->m_Pos[j]) / dt0) - 
                    ((pnts[i]->m_Pos[j] - pnts[i-1]->m_Pos[j]) / dt1);

                u[i][j] = 
                    (6.0 * u[i][j]/dt2 - sig * u[i-1][j]) / p;
            }
        }
        
        // ----- Point n -----

        // add terminal u to list (referenced as u[n-1])
        u.push_back(osg::Vec3d(0.0, 0.0, 0.0));

        osg::Vec3d qn;

        if ((m_Ypn.x() == 0.0) && (m_Ypn.y() == 0.0) && (m_Ypn.z() == 0.0)) {
            // terminal point is set to be "natural" 
            
            // initialize second derivative
            qn.set(0.0, 0.0, 0.0);
            // add first entry for natural control point
            u[n-1].set(0.0, 0.0, 0.0);
        } else {
            // initialize second derivative
            qn.set(-0.5, -0.5, -0.5);
            // delta time
            double dt = pnts[n-1]->m_T - pnts[n-2]->m_T;
            // delta position
            osg::Vec3d dpos = pnts[n-1]->m_Pos - pnts[n-2]->m_Pos;

            for (unsigned int j = 0; j < 3; j++) {
                u[n-1][j] = (3.0 / dt) * (m_Ypn[j] - (dpos[j] / dt));
            }
        }

        for (unsigned int j = 0; j < 3; j++) {
            pnts[n-1]->m_Y2[j] = (u[n-1][j] - qn[j] * u[n-2][j]) / 
                (qn[j] * pnts[n-2]->m_Y2[j] + 1.0);
        }

        for (int k = n - 2; k >= 0; k--) {
            // back substitution loop of the tridiagonal algorithm
            for (unsigned int j = 0; j < 3; j++) {
                pnts[k]->m_Y2[j] = pnts[k]->m_Y2[j] * pnts[k+1]->m_Y2[j] + u[k][j];
            }
        }

        // mark curve as valid ready for interpolation
        m_CurveIsValid = true;

        ret = true;
    }

    return ret;
}

osg::Vec3d OsgCubicSpline::GetPositionAtTime(const double t)
{
	Interpolate(t);

    osg::Vec3d splineOrigin(0.0, 0.0, 0.0);
    if (m_ControlPoints.size() > 0) 
	{
        ControlPoint *wp0 = m_ControlPoints[0].get();
        splineOrigin.set(wp0->m_Pos);
    }

    osg::Vec3d pos = GetInterpPosition() + splineOrigin;
	return pos;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::Interpolate
///
/// description: Interpolate spline at time t
/// 
///     Reference: splint implementation
///      Chapter 3.3 Cubic Spline Interpolation
///          http://www.nrbook.com/a/bookcpdf/c3-3.pdf
///
/// param: t - interpolation time in seconds \n
///
/// return: true if successfully interpolated the spline, else false
///
bool OsgCubicSpline::Interpolate(const double t)
{
    bool ret = false;

    double interpTime = t;

    // clear last interpolated values
    m_InterpolatedY.set(0.0, 0.0, 0.0);
    m_InterpolatedY1.set(osg::Vec3d(0.0, 0.0, 0.0));
    m_InterpolatedY2.set(osg::Vec3d(0.0, 0.0, 0.0));

    if (!m_CurveIsValid) {
        // reconstruct the curve
        BuildCurve();
    }

    // shortcut names to member variables
    PointListType &pnts = m_ControlPoints;
    unsigned int &lo = m_InterpolateLow;
    unsigned int &hi = m_InterpolateHigh;
    // number of control points
    const unsigned int n = pnts.size();

    // nothing to do if we have no control points
    if (n > 0) {

        if (interpTime > pnts[n-1]->m_T) {
            // maximum time exceeded, cap time
            interpTime = pnts[n-1]->m_T;
        } 

        if (interpTime < pnts[0]->m_T) {
            // minimum time exceeded, cap time
            interpTime = pnts[0]->m_T;
        }

        if ((hi == lo) || (interpTime > pnts[hi]->m_T) || (interpTime < pnts[lo]->m_T)) {

            // previous values were out of scope, so reset
            // and find right place using bisection

            lo = 0;
            hi = pnts.size() - 1;
            while ((hi - lo) > 1) {
                unsigned int k = (hi + lo) / 2;
                if (pnts[k]->m_T > t) {
                    hi = k;
                } else {
                    lo = k;
                }
            }
        }

        double dt = pnts[hi]->m_T - pnts[lo]->m_T;
        
        if (dt != 0.0) {
            ret = true;
            
            // (eq. 3.3.2)
            double a = (pnts[hi]->m_T - interpTime) / dt;
            double b = (interpTime - pnts[lo]->m_T) / dt;
            // (eq 3.3.4)
            double dt2div6 = (dt*dt) / 6.0;
            double c = (a*a*a - a) * dt2div6;
            double d = (b*b*b - b) * dt2div6;

            for (unsigned int i = 0; i < 3; i++) {
                // find position (eq 3.3.3)
                m_InterpolatedY[i] = 
                    a * pnts[lo]->m_Pos[i] + 
                    b * pnts[hi]->m_Pos[i] +
                    c * pnts[lo]->m_Y2[i] +
                    d * pnts[hi]->m_Y2[i];

                // find first derivative (eq. 3.3.5)
                m_InterpolatedY1[i] = 
                    ((pnts[hi]->m_Pos[i] - pnts[lo]->m_Pos[i]) / dt) -
                    (((3 * a * a - 1.0) / 6.0) * dt * pnts[lo]->m_Y2[i]) +
                    (((3 * b * b - 1.0) / 6.0) * dt * pnts[hi]->m_Y2[i]);


                // find second derivative (eq. 3.3.6)
                m_InterpolatedY2[i] = 
                    a * pnts[lo]->m_Y2[i] + 
                    b * pnts[hi]->m_Y2[i];
            }
        }
    }


    return ret;
}

///////////////////////////////////////////////////////////////////////////////
/// OsgCubicSpline::CreateTestGeom
///
/// description: create and return a scene graph making up the curve
/// 
/// param: showControlPoints - true if scene graph includes control points\n
/// param: numSamples - number of points between each control point\n
/// param: xform - position transformation functor\n
///
/// return: scene graph root node
///
osg::Node *OsgCubicSpline::CreateTestGeom(const bool showControlPoints, 
                                       const unsigned int numSamples,
                                       ControlPointTransformFunctor* xform)
{
    osg::Node *ret = NULL;

    if (!m_CurveIsValid) {
        // reconstruct the curve
        BuildCurve();
    }

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
                    OsgCubicSpline::Interpolate(t);

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
            color->push_back(osg::Vec4(1.0, 0.0, 0.0, 1.0));
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
CStdColor *OsgCubicSpline::LineColor() {return &m_vLineColor;}

/**
\brief	Sets the LineColor color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void OsgCubicSpline::LineColor(CStdColor &aryColor)
{
	m_vLineColor = aryColor;
	InvalidateCurve();
}

/**
\brief	Sets the LineColor color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void OsgCubicSpline::LineColor(float *aryColor)
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

void OsgCubicSpline::LineColor(string strXml)
{
	CStdColor vColor(1);
	vColor.Load(strXml, "Color");
	LineColor(vColor);
}

/**
\brief	Gets the GUID ID of the part this camera will track while on this path.

\author	dcofer
\date	3/18/2011

\return	GUID ID of the part.
**/
string OsgCubicSpline::PartID() {return m_strPartID;}

/**
\brief	Sets the GUID ID of the part this camera will track while on this path.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID for the part. 
**/
void OsgCubicSpline::PartID(string strID)
{
	m_strPartID = strID;
}

float OsgCubicSpline::StartTime() {return m_fltStartTime;}

void OsgCubicSpline::StartTime(float fltTime)
{
	Std_IsAboveMin((float) 0, fltTime, TRUE, "StartTime", TRUE);
	m_fltStartTime = fltTime;
}

float OsgCubicSpline::EndTime() {return m_fltEndTime;}

void OsgCubicSpline::EndTime(float fltTime)
{
	Std_IsAboveMin(m_fltStartTime, fltTime, TRUE, "EndTime", TRUE);
	m_fltEndTime = fltTime;
}

BOOL OsgCubicSpline::VisibleInSim() 
{return m_bVisibleInSim;}

void OsgCubicSpline::VisibleInSim(BOOL bVal)
{m_bVisibleInSim = bVal;}

BOOL OsgCubicSpline::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "TRACKPARTID")
	{
		PartID(strValue);
		Initialize();
		return TRUE;
	}

	if(strType == "STARTTIME")
	{
		StartTime(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ENDTIME")
	{
		EndTime(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LINECOLOR")
	{
		LineColor(strValue);
		return TRUE;
	}
	
	if(strDataType == "LINECOLOR.RED")
	{
		float aryVal[4] = {atof(strValue.c_str()), m_vLineColor.g(), m_vLineColor.b(), m_vLineColor.a()};
		LineColor(aryVal);
		return TRUE;
	}

	if(strDataType == "LINECOLOR.GREEN")
	{
		float aryVal[4] = {m_vLineColor.r(), atof(strValue.c_str()), m_vLineColor.b(), m_vLineColor.a()};
		LineColor(aryVal);
		return TRUE;
	}

	if(strDataType == "LINECOLOR.BLUE")
	{
		float aryVal[4] = {m_vLineColor.r(), m_vLineColor.g(), atof(strValue.c_str()), m_vLineColor.a()};
		LineColor(aryVal);
		return TRUE;
	}

	if(strDataType == "LINECOLOR.ALPHA")
	{
		float aryVal[4] = {m_vLineColor.r(), m_vLineColor.g(), m_vLineColor.b(), atof(strValue.c_str())};
		LineColor(aryVal);
		return TRUE;
	}

	if(strDataType == "VISIBLEINSIM")
	{
		VisibleInSim(Std_ToBool(strValue));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void OsgCubicSpline::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
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

BOOL OsgCubicSpline::AddItem(const string &strItemType, const string &strXml, BOOL bThrowError, BOOL bDoNotInit)
{
	string strType = Std_CheckString(strItemType);

	/*if(strType == "RIGIDBODY")
	{
		AddRoot(strXml);
		return TRUE;
	}*/

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL OsgCubicSpline::RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	/*if(strType == "RIGIDBODY")
	{
		RemoveRoot(strID);
		return TRUE;
	}
*/
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

void OsgCubicSpline::Initialize()
{
	AnimatBase::Initialize();

	m_lpTrackBody = NULL;
	if(!Std_IsBlank(m_strPartID))
		m_lpTrackBody = dynamic_cast<BodyPart *>(m_lpSim->FindByID(m_strPartID));
}

void OsgCubicSpline::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_vLineColor.Load(oXml, "LineColor", false);

	EndTime(oXml.GetChildFloat("EndTime", m_fltEndTime));
	StartTime(oXml.GetChildFloat("StartTime", m_fltStartTime));
	PartID(oXml.GetChildString("PartID", m_strPartID));
	VisibleInSim(oXml.GetChildBool("VisibleInSim", m_bVisibleInSim));

	oXml.OutOfElem(); //OutOf RigidBody Element
}



	}			// Visualization
//}				//VortexAnimatSim

}

