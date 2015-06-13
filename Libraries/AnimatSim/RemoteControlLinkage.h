#pragma once

namespace AnimatSim
{
	namespace Robotics
	{
		class RemoteControl;

		class ANIMAT_PORT RemoteControlData
		{
		public:
			///The array index of this button
			std::string m_strProperty;
			int m_iButtonID;
			float m_fltStart;
			float m_fltValue;      // vertical stick movement = forward speed
			float m_fltStop;
			float m_fltPrev;
			int m_iCount;
			bool m_bStarted;
			int m_iStartDir;

			///This keeps track of whether the sim has already stepped over a change or not.
			///The first time start or stop is set and sim is called we do not want to reset it to zero
			///right then so that the rest of the app can see it. We do want to reset it the next time though.
			int m_iSimStepped;

			///The number of simulation time slices to keep a start/stop signal active.
			int m_iChangeSimStepCount;

			RemoteControlData()
			{
				m_iButtonID = -1;
				m_iChangeSimStepCount = 5;
				ClearData();
			}

			RemoteControlData(std::string strName, int iButtonID, int iChangeSimStepCount)
			{
				m_strProperty = strName;
				m_iButtonID = iButtonID;
				m_iChangeSimStepCount = iChangeSimStepCount;
				ClearData();
			}

			void ClearData()
			{
				m_fltStart = 0;
				m_fltValue = 0;      
				m_fltStop = 0;
				m_fltPrev = 0;
				m_iCount = 0;
				m_bStarted = 0;
				m_iSimStepped = 0;
				m_iStartDir = 1;
			}

			void CheckStartedStopped();
			void ClearStartStops();
		};

		class ANIMAT_PORT RemoteControlLinkage : public AnimatBase
		{
		protected:
			///Pointer tho the parent remote control
			RemoteControl *m_lpParentRemoteControl;

			///Source object we are inserting from
			AnimatBase *m_lpSource;

			///Target object we are inserting into
			AnimatBase *m_lpTarget;

			///ID of the source. This is only used during loading.
			std::string m_strSourceID;

			///ID of the target. This is only used during loading.
			std::string m_strTargetID;

			///ID of the source data type. This is only used during loading.
			std::string m_strSourceDataTypeID;

			///ID of the target data type. This is only used during loading.
			std::string m_strTargetDataTypeID;

			///This is used when UseRemoteDataTypes is set to false. It is either the
			///source or target data type ID. It depends on whether InLink is true or false.
			int m_iPropertyID;

			///Name of the property that will be used for the data type if required
			std::string m_strPropertyName;

			///integer index of the target data type
			int m_iTargetDataType;

			/// Pointer to the source data variable.
			float *m_lpSourceData;
			
			///Pointer to the external value of the linked target.
			float *m_lpTargetData;

			///The total value applied during a time step
			float m_fltAppliedValue;

			///True if this is an incoming link from the remote device to the simulation
			///False if this is an outgoing link from the simulation to the remote device.
			bool m_bInLink;

			virtual float CalculateAppliedValue(float fltData) = 0;
			virtual void ApplyValue(float fltData);

		public:
			RemoteControlLinkage(void);
			virtual ~RemoteControlLinkage(void);
						
			static RemoteControlLinkage *CastToDerived(AnimatBase *lpBase) {return static_cast<RemoteControlLinkage*>(lpBase);}

			///This is a set of data that is associated with this linkage.
			RemoteControlData m_Data;

			virtual void ParentRemoteControl(RemoteControl *lpParent);
			virtual RemoteControl *ParentRemoteControl();

			virtual std::string SourceID();
			virtual void SourceID(std::string strID);

			virtual std::string TargetID();
			virtual void TargetID(std::string strID);

			virtual int PropertyID();
			virtual void PropertyID(int iID, bool bCreateDataTypes = true);

			virtual std::string SourceDataTypeID();
			virtual void SourceDataTypeID(std::string strTypeID);

			virtual std::string TargetDataTypeID();
			virtual void TargetDataTypeID(std::string strTypeID);

			virtual std::string PropertyName();
			virtual void PropertyName(std::string strName);

			virtual bool InLink();
			virtual void InLink(bool bVal);

			virtual float AppliedValue();
			virtual void AppliedValue(float fltVal);

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void SetupIO();
			virtual void StepIO();
			virtual void ShutdownIO();

			virtual void Initialize();
			virtual void ResetSimulation();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}