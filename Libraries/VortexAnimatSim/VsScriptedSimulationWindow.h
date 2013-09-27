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
		std::string m_strDefaultPartID;

		CStdFPoint m_vDefaultPosition;

		virtual void AddCameraPath(std::string strXml);
		virtual void RemoveCameraPath(std::string strID, bool bThrowError = true);
		virtual OsgLinearPath *LoadCameraPath(CStdXml &oXml);
		virtual void TrackCamera();
		virtual void FindNextCameraPath();

	public:
		VsScriptedSimulationWindow(void);
		virtual ~VsScriptedSimulationWindow(void);

		virtual std::string DefaultPartID();
		virtual void DefaultPartID(std::string strID);

		virtual CStdFPoint DefaultPosition();
		virtual void DefaultPosition(CStdFPoint &oPoint, bool bUseScaling = true);
		virtual void DefaultPosition(float fltX, float fltY, float fltZ, bool bUseScaling = true);
		virtual void DefaultPosition(std::string strXml, bool bUseScaling = true);

		virtual int FindCameraPath(std::string strID, bool bThrowError = true);
		virtual void SortPaths();

		virtual void SimStarting();

		virtual void Update();

		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
		virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void Load(CStdXml &oXml);
};

	}// end Visualization
}// end VortexAnimatSim

