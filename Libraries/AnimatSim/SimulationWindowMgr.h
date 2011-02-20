#pragma once

namespace AnimatSim
{

class ANIMAT_PORT SimulationWindowMgr : public AnimatBase
{
protected:
	CStdFPoint m_ptPosition;
	CStdFPoint m_ptSize;

	CStdPtrArray<SimulationWindow> m_aryWindows;
	
	SimulationWindow *LoadSimulationWindow(Simulator *lpSim, CStdXml &oXml);

	virtual void SetupCameras();

public:
	SimulationWindowMgr(void);
	~SimulationWindowMgr(void);

	virtual BOOL HasContainedWindow();

	virtual CStdPtrArray<SimulationWindow> *Windows() {return &m_aryWindows;};
	virtual void Initialize(Simulator *lpSim) = 0;
	virtual BOOL Update(Simulator *lpSim);	
	virtual void Realize() = 0;
	virtual void Close();

	virtual SimulationWindow *FindSimulationWindow(HWND win, int &iIndex, BOOL bThrowError = TRUE);

	virtual SimulationWindow *AddSimulationWindow(Simulator *lpSim, string strModule, string strType, BOOL bInit, HWND win, string strHudXml);
	virtual void RemoveSimulationWindow(Simulator *lpSim, HWND win);
	virtual void CloseAllWindows();
	virtual void Load(Simulator *lpSim, CStdXml &oXml);
};

}// end AnimatSim


