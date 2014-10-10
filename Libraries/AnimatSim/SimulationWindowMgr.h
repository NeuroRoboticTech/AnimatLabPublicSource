/**
\file	SimulationWindowMgr.h

\brief	Declares the simulation window manager class.
**/

#pragma once

namespace AnimatSim
{
/**
\brief	Manager for simulation windows. 

\details This object manages all of the simulation windows in the entire system. You call its Add/Remove
methods to create/destroy new windows.

\author	dcofer
\date	3/25/2011
**/
class ANIMAT_PORT SimulationWindowMgr : public AnimatBase
{
protected:
	/// If this is a stand-alone group window then this is the position of the group window.
	CStdFPoint m_ptPosition;

	/// If this is a stand-alone group window then this is the size of the group window.
	CStdFPoint m_ptSize;

	/// Array of SimulationWindow objects.
	CStdPtrArray<SimulationWindow> m_aryWindows;

	/// Manager for heads-up display items.
	Hud *m_lpHudMgr;

	SimulationWindow *LoadSimulationWindow(CStdXml &oXml);

	virtual void SetupCameras();

public:
	SimulationWindowMgr(void);
	~SimulationWindowMgr(void);
						
	static SimulationWindowMgr *CastToDerived(AnimatBase *lpBase) {return static_cast<SimulationWindowMgr*>(lpBase);}

	virtual bool HasContainedWindow();

	/**
	\brief	Gets the windows array.
	
	\author	dcofer
	\date	3/25/2011
	
	\return	pointer to the array.
	**/
	virtual CStdPtrArray<SimulationWindow> *Windows() {return &m_aryWindows;};

	virtual bool Update();	

	/**
	\brief	Shows the windows.
	
	\author	dcofer
	\date	3/25/2011
	**/
	virtual void Realize() = 0;
	virtual void Close();
	virtual void Initialize();

	virtual void UpdateBackgroundColor();

	virtual SimulationWindow *FindSimulationWindow(HWND win, int &iIndex, bool bThrowError = true);

	virtual void ResetSimulation();
	virtual SimulationWindow *AddSimulationWindow(std::string strModule, std::string strType, bool bInit, HWND win, std::string strHudXml);
	virtual void RemoveSimulationWindow(HWND win);
	virtual void CloseAllWindows();
	virtual void Load(CStdXml &oXml);
};

}// end AnimatSim


