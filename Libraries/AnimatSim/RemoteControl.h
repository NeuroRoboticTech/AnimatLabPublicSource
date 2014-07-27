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

			virtual RemoteControlLinkage *LoadRemoteControlLinkage(CStdXml &oXml);
			virtual RemoteControlLinkage *AddRemoteControlLinkage(std::string strXml);
			virtual void RemoveRemoteControlLinkage(std::string strID, bool bThrowError = true);
			virtual int FindLinkageChildListPos(std::string strID, bool bThrowError = true);

		public:
			RemoteControl(void);
			virtual ~RemoteControl(void);
						
			static RemoteControl *CastToDerived(AnimatBase *lpBase) {return static_cast<RemoteControl*>(lpBase);}

			virtual CStdPtrArray<RemoteControlLinkage>* Links();
			
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

			virtual void SetupIO();
			virtual void StepIO();
			virtual void ShutdownIO();

			virtual void Initialize();
			virtual void StepSimulation();
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}