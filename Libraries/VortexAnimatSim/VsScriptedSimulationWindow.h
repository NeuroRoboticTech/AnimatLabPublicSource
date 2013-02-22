#pragma once

#include "OsgCubicSpline.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

class VORTEX_PORT VsScriptedSimulationWindow : public VsSimulationWindow
{
	protected:
		CStdPtrArray<OsgCubicSpline> m_aryCameraPaths;

		virtual void AddCameraPath(string strXml);
		virtual void RemoveCameraPath(string strID, BOOL bThrowError = TRUE);
		virtual OsgCubicSpline *LoadCameraPath(CStdXml &oXml);

	public:
		VsScriptedSimulationWindow(void);
		virtual ~VsScriptedSimulationWindow(void);

		virtual int FindCameraPath(string strID, BOOL bThrowError = TRUE);

		virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
		virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);
		virtual void Initialize();
		virtual void Load(CStdXml &oXml);
};

	}// end Visualization
}// end VortexAnimatSim

