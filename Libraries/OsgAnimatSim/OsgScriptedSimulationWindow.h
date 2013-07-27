#pragma once

#include "OsgLinearPath.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

class ANIMAT_OSG_PORT OsgScriptedSimulationWindow : public OsgSimulationWindow
{
	protected:
		CStdPtrArray<OsgLinearPath> m_aryCameraPaths;
		CStdMap<double, OsgLinearPath *> m_arySortedCameraPaths;
		CStdMap<double, OsgLinearPath *>::iterator m_iCurrentPathIter;

		OsgLinearPath *m_lpCurrentPath;

		/// Used to keep track of the body part that was being tracked before sim started.
		BodyPart *m_lpOriginalTrackBody;
		//This is the body part that we will switch to once sim starts.
		BodyPart *m_lpDefaultTrackBody;
		string m_strDefaultPartID;

		CStdFPoint m_vDefaultPosition;

		virtual void AddCameraPath(string strXml);
		virtual void RemoveCameraPath(string strID, BOOL bThrowError = TRUE);
		virtual OsgLinearPath *LoadCameraPath(CStdXml &oXml);
		virtual void TrackCamera();
		virtual void FindNextCameraPath();

	public:
		OsgScriptedSimulationWindow(void);
		virtual ~OsgScriptedSimulationWindow(void);

		virtual string DefaultPartID();
		virtual void DefaultPartID(string strID);

		virtual CStdFPoint DefaultPosition();
		virtual void DefaultPosition(CStdFPoint &oPoint, BOOL bUseScaling = TRUE);
		virtual void DefaultPosition(float fltX, float fltY, float fltZ, BOOL bUseScaling = TRUE);
		virtual void DefaultPosition(string strXml, BOOL bUseScaling = TRUE);

		virtual int FindCameraPath(string strID, BOOL bThrowError = TRUE);
		virtual void SortPaths();

		virtual void SimStarting();

		virtual void Update();

		virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
		virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void Load(CStdXml &oXml);
};

	}// end Visualization
}// end OsgAnimatSim

