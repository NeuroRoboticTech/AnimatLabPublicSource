/**
\file	SimulationWindow.h

\brief	Declares the simulation Windows Form.
**/

#pragma once

namespace AnimatSim
{
	/**
	\brief	Base class for a simulation window.

	\details Any number of simulation windows can be created for viewing the 3-D simulation environment. 
	This has to be implemented by deriving a window for the type of graphics library you are using, osg for example.
	
	\author	dcofer
	\date	3/24/2011
	**/
	class ANIMAT_PORT SimulationWindow : public AnimatBase
	{
	protected:
		/// true if the window is seperate from all other windows.
		bool m_bStandAlone;

		/// Handle of the hwnd
		HWND m_HWND;

		/// The position of the window
		CStdFPoint m_ptPosition;

		/// Size of the window
		CStdFPoint m_ptSize;

		///The ID of the structure that the camera should look at initially. If this
		///is blank or missing then the camera looks in its default direction. If 
		///m_bTrackCamera is true then it will follow the structure identfied here.
		string m_strLookAtStructureID;

		///The ID of the body item within the structure that the camera should look at initially. If this
		///is blank or missing then the camera looks in its default direction. If 
		///m_bTrackCamera is true then it will follow the structure identfied here.
		string m_strLookAtBodyID;

		///If this is true then the camera will update its position and orientation so
		///that it stays focues on the structure specified by the m_strLookAtID element.
		///The camera always looks at the center of the root rigid body object of the 
		///structure.
		bool m_bTrackCamera;

		/**
		\brief	Implements code to do the camera tracking.
		
		\author	dcofer
		\date	3/24/2011
		**/
		virtual void TrackCamera() = 0;

	public:
		SimulationWindow(void);
		SimulationWindow(HWND win);
		~SimulationWindow(void);

		virtual void LookAtStructureID(string strID);
		virtual string LookAtStructureID();

		virtual void LookAtBodyID(string strID);
		virtual string LookAtBodyID();

		virtual void UsingTrackCamera(bool bVal);
		virtual bool UsingTrackCamera();

		virtual HWND WindowID();
		virtual void WindowID(HWND win);

		virtual bool StandAlone();
		virtual void StandAlone(bool bVal);

		virtual CStdFPoint GetCameraPosition() = 0;

		/**
		\brief	Sets up the camera tracking.
		
		\author	dcofer
		\date	3/24/2011
		**/
		virtual void SetupTrackCamera(bool bResetEyePos) = 0;
		virtual void SetupTrackCamera(bool bTrackCamera, string strLookAtStructureID, string strLookAtBodyID);
		virtual void SetCameraLookAt(CStdFPoint oTarget, bool bResetEyePos);
		virtual void SetCameraPositionAndLookAt(CStdFPoint oCameraPos, CStdFPoint oTarget);
		virtual void SetCameraPostion(CStdFPoint vCameraPos);

		virtual void UpdateBackgroundColor();

#pragma region DataAccesMethods

			virtual float *GetDataPointer(const string &strDataType);
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

		/**
		\brief	Updates this window.
		
		\author	dcofer
		\date	3/24/2011
		**/
		virtual void Update() = 0;

		/**
		\brief	Closes this window.
		
		\author	dcofer
		\date	3/24/2011
		**/
		virtual void Close() = 0;

		virtual void Load(CStdXml &oXml);

		/**
		\brief	Loads this window from an xml string.
		
		\author	dcofer
		\date	3/24/2011
		
		\param	strXml	The xml string to load. 
		**/
		virtual void Load(string strXml);

		/**
		\brief	Called by the GUI when this window gets the focus.
		
		\author	dcofer
		\date	7/19/2011
		**/
		virtual void OnGetFocus();

		/**
		\brief	Called by the GUI when this window loses the focus.
		
		\author	dcofer
		\date	7/19/2011
		**/
		virtual void OnLoseFocus();
	};

}//end namespace AnimatSim
