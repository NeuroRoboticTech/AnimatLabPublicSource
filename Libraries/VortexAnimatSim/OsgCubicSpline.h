#pragma once

#include <osg/Referenced>

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class OsgCubicSpline;
		class VsScriptedSimulationWindow;

class VORTEX_PORT ControlPoint : public osg::Referenced, public AnimatSim::AnimatBase
{
public:
    // constructor
    ControlPoint(const double t, const osg::Vec3d pos, const osg::Vec3d y2) 
    :   AnimatBase(), 
		m_Pos(pos),
        m_Y2(y2),
        m_T(t),
        m_TrPos(osg::Vec3d(0.0, 0.0, 0.0)),
        m_IsDirty(true), 
		m_lpParentSpline(NULL)
    {}

	ControlPoint()
    :   AnimatBase(), 
		m_Pos(0, 0, 0),
        m_Y2(0, 0, 0),
        m_T(0),
        m_TrPos(osg::Vec3d(0.0, 0.0, 0.0)),
        m_IsDirty(true), 
		m_lpParentSpline(NULL)
	{}

protected:
    // destructor
    ~ControlPoint()
    {}

    // time at the position
    double m_T;

public:
    // position of the point
    osg::Vec3d m_Pos;
	//report position
    osg::Vec3d m_ReportPos;
	// second derivatives (accelerations) for the point
    osg::Vec3d m_Y2;
    // transformed position of the point
    osg::Vec3d m_TrPos;
    // flag if the transformed position is dirty
    bool m_IsDirty;

	OsgCubicSpline *m_lpParentSpline;

	virtual osg::Vec3d Position();
	virtual void Position(CStdFPoint &oPoint, BOOL bUseScaling = TRUE);
	virtual void Position(float fltX, float fltY, float fltZ, BOOL bUseScaling = TRUE);
	virtual void Position(string strXml, BOOL bUseScaling = TRUE);

	virtual double Time();
	virtual void Time(double dblVal);

	virtual double PointTime();

	virtual OsgCubicSpline *ParentSpline() {return m_lpParentSpline;}; 
	virtual void ParentSpline(OsgCubicSpline *lpParentSpline) {m_lpParentSpline = lpParentSpline;};

	virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
	virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
	virtual void Load(StdUtils::CStdXml &oXml);
};


class VORTEX_PORT OsgCubicSpline : public osg::Referenced, public AnimatSim::AnimatBase
{

public:

    class VORTEX_PORT ControlPointTransformFunctor
    {
    public:

        // destructor
        ControlPointTransformFunctor() {}
        // constructor
        ~ControlPointTransformFunctor() {}

    public:

        // transform a control point position to user frame position
        virtual osg::Vec3d Transform(const osg::Vec3d p) { return p; }
    };


public:

    // list of control points
    typedef std::vector<osg::ref_ptr<ControlPoint> > PointListType;

public:
    // constructor
    OsgCubicSpline();
    // destructor
    ~OsgCubicSpline();

public:

    // Control Point methods

    // add a control point
    bool AddControlPoint(const osg::Vec3d point, const double time);
    
    // add a control point
    bool AddControlPoint(ControlPoint *p);
    
    // clear all control points 
    void ClearAllControlPoints();
    
    // return the number of control points in the curve
    unsigned int GetNumberOfControlPoints() { return m_ControlPoints.size(); }

    // return control point by index
    ControlPoint* GetControlPoint(const unsigned int i) { return m_ControlPoints[i].get(); }

    // set initial velocity (dbu/s) <first derivative> of spline
    void SetInitialVelocity(const osg::Vec3d vel) { m_Yp0 = vel; }
    // retrieve the initial velocity  (dbu/s) <first derivative> of the spline
    void GetInitialVelocity(osg::Vec3d &vel) { vel = m_Yp0; }

    // set terminal velocity (dbu/s) <first derivative> of spline
    void SetTerminalVelocity(const osg::Vec3d vel) { m_Ypn = vel; }
    // retrieve the terminal velocity (dbu/s)  <first derivative> of spline 
    void GetTerminalVelocity(osg::Vec3d &vel) { vel = m_Ypn; }

    // Method to create and retrieve output curve

    /// create and return a scene graph making up the curve
    virtual osg::Node *CreateTestGeom(const bool showControlPoints = false,
        const unsigned int numSamples = 10, 
        ControlPointTransformFunctor* xform = NULL);

    /// Interpolate spline at time t
    virtual bool Interpolate(const double t);

    // return last interpolated position (dbu)
    osg::Vec3d &GetInterpPosition() { return m_InterpolatedY; }
    // return last interpolated first derivative as velocity (dbu/s)
    osg::Vec3d &GetInterpVelocity() { return m_InterpolatedY1; }
    // return last interpolated second derivative as acceleration (dbu/s*s)
    osg::Vec3d &GetInterpAcceleration() { return m_InterpolatedY2; }

	osg::Vec3d GetPositionAtTime(const double t);

	virtual CStdColor *LineColor();
	virtual void LineColor(CStdColor &aryColor);
	virtual void LineColor(float *aryColor);
	virtual void LineColor(string strXml);
	
	virtual string PartID();
	virtual void PartID(string strID);

	virtual double StartTime();
	virtual void StartTime(double dblTime);

	virtual double EndTime();
	virtual void EndTime(double dblTime);

	virtual BOOL Visible();
	virtual void Visible(BOOL bVal);

	virtual BOOL VisibleInSim();
	virtual void VisibleInSim(BOOL bVal);

	virtual BOOL ShowWaypoints();
	virtual void ShowWaypoints(BOOL bVal);

	virtual BodyPart *TrackBody() {return m_lpTrackBody;};

	virtual void SimStarting();
	virtual void ResetSimulation();

	virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
	virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
	virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
	virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);
	virtual void Initialize();
	virtual void Load(StdUtils::CStdXml &oXml);

	virtual void RedrawCurve();

	virtual bool BeforePathTime(double dblTime);
	virtual bool WithinPathTime(double dblTime);
	virtual bool AfterPathTime(double dblTime);

	virtual VsScriptedSimulationWindow *ParentWindow() {return m_lpParentWindow;};
	virtual void ParentWindow(VsScriptedSimulationWindow *lpWnd) {m_lpParentWindow = lpWnd;};

protected:

    /// mark curve as invalid (needs to be regenerated)
    void InvalidateCurve();
    /// figure the curve's coefficients at the control points
    /// so that Evaluation can be accomplished.
    bool BuildCurve();

	void RemoveCurve();

	osg::ref_ptr<ControlPoint> LoadWaypoint(CStdXml &oXml);

	virtual void AddWaypoint(string strXml);
	virtual void RemoveWaypoint(string strID, BOOL bThrowError = TRUE);
	virtual int FindWaypointPos(string strID, BOOL bThrowError = TRUE);
	virtual void MakeVisible(BOOL bVal);

protected:

    // if true, curve holds valid points
    bool m_CurveIsValid;

    // list of 3D control points sorted by time
    PointListType m_ControlPoints;
    
    // initial first derivative of spline as a velocity (dbu/s)
    osg::Vec3d m_Yp0;
    // terminal first derivative of spline as a velocity (dbu/s)
    osg::Vec3d m_Ypn;

    // last spline interpolate low value
    unsigned int m_InterpolateLow;
    // last spline interpolate high value
    unsigned int m_InterpolateHigh;

    // last interpolated position (dbu)
    osg::Vec3d m_InterpolatedY;
    // last interpolated first derivative as velocity (dbu/s)
    osg::Vec3d m_InterpolatedY1;
    // last interpolated second derivative as acceleration (dbu/s*s)
    osg::Vec3d m_InterpolatedY2;

	//ID of the part to look at while this path is playing out.
	std::string m_strPartID;

	//Part to look at during camera path playback
	BodyPart *m_lpTrackBody;

	//Start time for this path.
	double m_dblStartTime;

	//End time for this path
	double m_dblEndTime;

	//Line color to use
	CStdColor m_vLineColor;

	//Turns visibility of line on/off while not running.
	BOOL m_bVisible;

	//Turns visibility of line on/off while running simulation
	BOOL m_bVisibleInSim;

	//Tells if we should show the waypoints or not.
	BOOL m_bShowWaypoints;

	//The osg spline node
	osg::ref_ptr<osg::Node> m_osgSpline;

	VsScriptedSimulationWindow *m_lpParentWindow;
};


	}			// Visualization
//}				//VortexAnimatSim

}


