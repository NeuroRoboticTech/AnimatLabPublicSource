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
	
	SimulationWindow *LoadSimulationWindow(CStdXml &oXml);

	virtual void SetupCameras();

public:
	SimulationWindowMgr(void);
	~SimulationWindowMgr(void);

	virtual BOOL HasContainedWindow();

	/**
	\brief	Gets the windows.
	
	\author	dcofer
	\date	3/25/2011
	
	\return	null if it fails, else.
	**/
	virtual CStdPtrArray<SimulationWindow> *Windows() {return &m_aryWindows;};

	virtual BOOL Update();	

	/**
	\brief	Shows the windows.
	
	\author	dcofer
	\date	3/25/2011
	**/
	virtual void Realize() = 0;
	virtual void Close();

	virtual SimulationWindow *FindSimulationWindow(HWND win, int &iIndex, BOOL bThrowError = TRUE);

	virtual SimulationWindow *AddSimulationWindow(string strModule, string strType, BOOL bInit, HWND win, string strHudXml);
	virtual void RemoveSimulationWindow(HWND win);
	virtual void CloseAllWindows();
	virtual void Load(CStdXml &oXml);
};

}// end AnimatSim


