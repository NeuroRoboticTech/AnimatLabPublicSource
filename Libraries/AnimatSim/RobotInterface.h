#pragma once

namespace AnimatSim
{
	namespace Robotics
	{

		class ANIMAT_PORT RobotInterface : public AnimatBase
		{
		protected:
			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RobotIOControl> m_aryIOControls;

			/// Used to determine if we are running in a simulation, or in a real control mode.
			/// If we are in a simulatino then none of the sim or init methods are called for the robot code.
			bool m_bInSimulation;

			virtual RobotIOControl *LoadIOControl(CStdXml &oXml);
			virtual RobotIOControl *AddIOControl(std::string strXml);
			virtual void RemoveIOControl(std::string strID, bool bThrowError = true);
			virtual int FindChildListPos(std::string strID, bool bThrowError = true);

		public:
			RobotInterface(void);
			virtual ~RobotInterface(void);

			virtual CStdPtrArray<RobotIOControl>* IOControls();

			virtual bool InSimulation();
			virtual void InSimulation(bool bVal);

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