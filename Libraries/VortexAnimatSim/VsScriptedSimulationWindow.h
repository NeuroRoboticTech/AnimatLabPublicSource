#pragma once

#include "OsgLinearPath.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

class VORTEX_PORT VsScriptedSimulationWindow : public VsSimulationWindow
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
		virtual void RemoveCameraPath(string strID, bool bThrowError = true);
		virtual OsgLinearPath *LoadCameraPath(CStdXml &oXml);
		virtual void TrackCamera();
		virtual void FindNextCameraPath();

	public:
		VsScriptedSimulationWindow(void);
		virtual ~VsScriptedSimulationWindow(void);

		virtual string DefaultPartID();
		virtual void DefaultPartID(string strID);

		virtual CStdFPoint DefaultPosition();
		virtual void DefaultPosition(CStdFPoint &oPoint, bool bUseScaling = true);
		virtual void DefaultPosition(float fltX, float fltY, float fltZ, bool bUseScaling = true);
		virtual void DefaultPosition(string strXml, bool bUseScaling = true);

		virtual int FindCameraPath(string strID, bool bThrowError = true);
		virtual void SortPaths();

		virtual void SimStarting();

		virtual void Update();

		virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
		virtual bool AddItem(const string &strItemType, const string &strXml, bool bThrowError = true, bool bDoNotInit = false);
		virtual bool RemoveItem(const string &strItemType, const string &strID, bool bThrowError = true);
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void Load(CStdXml &oXml);
};

	}// end Visualization
}// end VortexAnimatSim

