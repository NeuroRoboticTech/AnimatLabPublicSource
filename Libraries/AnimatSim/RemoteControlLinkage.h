#pragma once

namespace AnimatSim
{
	namespace Robotics
	{
		class RemoteControl;

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

			///integer index of the target data type
			int m_iTargetDataType;

			/// Pointer to the source data variable.
			float *m_lpSourceData;
			
			///Pointer to the external value of the linked target.
			float *m_lpTargetData;

			///The total value applied during a time step
			float m_fltAppliedValue;

			virtual float CalculateAppliedValue() = 0;
			virtual void ApplyValue();

		public:
			RemoteControlLinkage(void);
			virtual ~RemoteControlLinkage(void);
						
			static RemoteControlLinkage *CastToDerived(AnimatBase *lpBase) {return static_cast<RemoteControlLinkage*>(lpBase);}

			virtual void ParentRemoteControl(RemoteControl *lpParent);
			virtual RemoteControl *ParentRemoteControl();

			virtual std::string SourceID();
			virtual void SourceID(std::string strID);

			virtual std::string TargetID();
			virtual void TargetID(std::string strID);

			virtual std::string SourceDataTypeID();
			virtual void SourceDataTypeID(std::string strTypeID);

			virtual std::string TargetDataTypeID();
			virtual void TargetDataTypeID(std::string strTypeID);

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void SetupIO();
			virtual void StepIO();
			virtual void ShutdownIO();

			virtual void Initialize();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}