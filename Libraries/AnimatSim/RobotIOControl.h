#pragma once

namespace AnimatSim
{
	namespace Robotics
	{

		class ANIMAT_PORT RobotIOControl : public AnimatBase
		{
		protected:
			RobotInterface *m_lpParentInterface;

			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RobotPartInterface> m_aryParts;

			virtual RobotPartInterface *LoadPartInterface(CStdXml &oXml);
			virtual RobotPartInterface *AddPartInterface(std::string strXml);
			virtual void RemovePartInterface(std::string strID, bool bThrowError = true);
			virtual int FindChildListPos(std::string strID, bool bThrowError = true);

		public:
			RobotIOControl(void);
			virtual ~RobotIOControl(void);

			virtual void ParentInterface(RobotInterface *lpParent);
			virtual RobotInterface *ParentInterface();

			virtual CStdPtrArray<RobotPartInterface>* Parts();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

			virtual void Initialize();
			virtual void StepSimulation();
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}