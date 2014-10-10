/**
\file	RobotInterface.h

\brief	Declares the robotics inerface for animatlab.
**/

#pragma once

namespace AnimatSim
{
	namespace Robotics
	{

		/**
		\brief	The Robotics interface configures a simulation to run on a microcontroller board.
			
		\details The robotics interface has two main purposes. The first is that it helps synchronize the simulation
		so it runs more like it would on real hardware. The second purpose is to configure the robotics simulation to 
		run on the real robots microcomputer board. It contains the Robot IO controllers and robot part interfaces.

		\author	dcofer
		\date	9/8/2014
		**/
		class ANIMAT_PORT RobotInterface : public AnimatBase
		{
		protected:
			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RobotIOControl> m_aryIOControls;

			///This is the physics time step used by the robotics framework. This is not necessarily the same
			///thing as the physics time step used in the simulation because the robot IO is not going to be able
			///to go as fast as we will need the sim steps to go for it to be stable.
			float m_fltPhysicsTimeStep;

			///If this is true and we are in simulation mode then we will set some flags in the simulation object
			///to make it delay stepping of the physics adapter objects to more closely match the IO time step of
			///the real robot.
			bool m_bSynchSim;

			virtual RobotIOControl *LoadIOControl(CStdXml &oXml);
			virtual RobotIOControl *AddIOControl(std::string strXml);
			virtual void RemoveIOControl(std::string strID, bool bThrowError = true);
			virtual int FindChildListPos(std::string strID, bool bThrowError = true);

		public:
			RobotInterface(void);
			virtual ~RobotInterface(void);
						
			static RobotInterface *CastToDerived(AnimatBase *lpBase) {return static_cast<RobotInterface*>(lpBase);}

			virtual CStdPtrArray<RobotIOControl>* IOControls();

			virtual float PhysicsTimeStep();
			virtual void PhysicsTimeStep(float fltStep);

			virtual bool SynchSim();
			virtual void SynchSim(bool bVal);

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