#pragma once

namespace AnimatSim
{
	namespace Robotics
	{


		class ANIMAT_PORT RemoteControl : public RobotIOControl
		{
		protected:
			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RemoteControlLinkage> m_aryLinks;

			///Only the linkages that are inlinks
			CStdArray<RemoteControlLinkage *> m_aryInLinks;

			///Only the linkages that are outlinks
			CStdArray<RemoteControlLinkage *> m_aryOutLinks;

			///Array of button information to keep track of incoming/outgoing data.
			CStdMap<int, RemoteControlLinkage *> m_aryData;

			///Used to map property names to IDs when UseRemoteDataTypes is true.
			CStdMap<std::string, int> m_aryDataIDMap;

			///The number of simulation time slices to keep a start/stop signal active.
			int m_iChangeSimStepCount;

			///If this is true then it assumes that data types are explicitly setup in the derived class.
			///If it is false then it assumes that data will be created based on the remote linkage settings.
			///This will be set in the derived class based on what that class does.
			bool m_bUseRemoteDataTypes;

			virtual RemoteControlLinkage *LoadRemoteControlLinkage(CStdXml &oXml);
			virtual RemoteControlLinkage *AddRemoteControlLinkage(std::string strXml);
			virtual void RemoveRemoteControlLinkage(std::string strID, bool bThrowError = true);
			virtual int FindLinkageChildListPos(std::string strID, bool bThrowError = true);
			virtual int FindLinkageChildListPos(CStdArray<RemoteControlLinkage *> &aryLinks, std::string strID, bool bThrowError = true);

			virtual void SetDataValue(int iButtonID, float fltVal);
			virtual void ResetData();
			virtual void CheckStartedStopped();
			virtual void ClearStartStops();

			virtual void CreateDataIDMap();

		public:
			RemoteControl(void);
			virtual ~RemoteControl(void);
						
			static RemoteControl *CastToDerived(AnimatBase *lpBase) {return static_cast<RemoteControl*>(lpBase);}

			virtual CStdPtrArray<RemoteControlLinkage>* Links();
			virtual CStdArray<RemoteControlLinkage *>* InLinks();
			virtual CStdArray<RemoteControlLinkage *>* OutLinks();
			virtual CStdMap<int, RemoteControlLinkage *>* Data();
			
			virtual void ChangeSimStepCount(int iRate);
			virtual int ChangeSimStepCount();
			
			virtual bool UseRemoteDataTypes();
			virtual void CreateDataTypes();

			virtual RemoteControlLinkage *FindLinkageWithPropertyName(std::string strName, bool bThrowError = true);

			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

			virtual void SetupIO();
			virtual void StepIO();
			virtual void ShutdownIO();
			
			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void Initialize();
			virtual void StepSimulation();
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}