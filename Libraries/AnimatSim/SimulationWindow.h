#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT SimulationWindow : public AnimatBase
	{
	protected:
		BOOL m_bStandAlone;

		HWND m_HWND;
		CStdFPoint m_ptPosition;
		CStdFPoint m_ptSize;

		///The ID of the structure that the camera should look at initially. If this
		///is blank or missing then the camera looks in its default direction. If 
		///m_bTrackCamera is TRUE then it will follow the structure identfied here.
		string m_strLookAtStructureID;

		///The ID of the body item within the structure that the camera should look at initially. If this
		///is blank or missing then the camera looks in its default direction. If 
		///m_bTrackCamera is TRUE then it will follow the structure identfied here.
		string m_strLookAtBodyID;

		///If this is TRUE then the camera will update its position and orientation so
		///that it stays focues on the structure specified by the m_strLookAtID element.
		///The camera always looks at the center of the root rigid body object of the 
		///structure.
		BOOL m_bTrackCamera;

		virtual void TrackCamera() = 0;

	public:
		SimulationWindow(void);
		SimulationWindow(HWND win);
		~SimulationWindow(void);

		virtual void LookAtStructureID(string strID) 
		{
			m_strLookAtStructureID = strID;
			SetupTrackCamera();
		};
		virtual string LookAtStructureID() {return m_strLookAtStructureID;};

		virtual void LookAtBodyID(string strID) 
		{
			m_strLookAtBodyID = strID;
			SetupTrackCamera();
		};
		virtual string LookAtBodyID() {return m_strLookAtBodyID;};

		virtual void UsingTrackCamera(BOOL bVal) 
		{
			m_bTrackCamera = bVal;
			SetupTrackCamera();
		};
		virtual BOOL UsingTrackCamera() {return m_bTrackCamera;};

		virtual HWND WindowID() {return m_HWND;};
		virtual void WindowID(HWND win) {m_HWND = win;};

		virtual BOOL StandAlone() {return m_bStandAlone;}
		virtual void StandAlone(BOOL bVal) {m_bStandAlone = bVal;};

		virtual void SetupTrackCamera() = 0;
		virtual void SetupTrackCamera(BOOL bTrackCamera, string strLookAtStructureID, string strLookAtBodyID);

#pragma region DataAccesMethods

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

		virtual void Initialize(Simulator *lpSim) = 0;
		virtual void Update(Simulator *lpSim) = 0;
		virtual void Close() = 0;
		virtual void Load(CStdXml &oXml);
		virtual void Load(string strXml);
	};

}//end namespace AnimatSim
