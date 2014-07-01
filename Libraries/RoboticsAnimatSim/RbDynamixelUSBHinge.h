// RbDynamixelUSBHinge.h: interface for the RbDynamixelUSBHinge class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace DynamixelUSB
			{

class ROBOTICS_PORT RbDynamixelUSBHinge : public AnimatSim::Robotics::RobotPartInterface, public RbDynamixelUSBServo
{
protected:
	RbDynamixelUSB *m_lpParentUSB;

    Hinge *m_lpHinge;

	///The number update cycles till we update all params. Typically we only want to update
	///position and speed of the servo every time. We will only update the other params every now 
	///and then. This tells how often to do this.
	int m_iUpdateAllParamsCount;

	///Keeps track of the number of update loops that have occurred since we last updated all params.
	int m_iUpdateIdx;

	///Keeps track of when to update this motor data from the real motor. If it is is -1 then it will
	///update it every time. If it is set to some other positive value then it will only update it
	///when that cycle comes around. This is for updating the motors in a round robin fashion.
	int m_iUpdateQueueIndex;

	///Keeps track of whether we want to pull the data back from this motor or not. 
	///If we are only setting values in it then there is no reason to do an update for it.
	bool m_bQueryMotorData;

public:
	RbDynamixelUSBHinge();
	virtual ~RbDynamixelUSBHinge();

	virtual bool IsMotorControl() {return true;};

	virtual void IOComponentID(int iID);

	virtual void UpdateAllParamsCount(int iVal);
	virtual int UpdateAllParamsCount();

	virtual void UpdateQueueIndex(int iVal);
	virtual int UpdateQueueIndex();

	virtual float QuantizeServoPosition(float fltPos);
	virtual float QuantizeServoVelocity(float fltVel);

	virtual void QueryMotorData(bool bVal);
	virtual bool QueryMotorData();

	virtual bool IncludeInPartsCycle();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion
	
	virtual void SetupIO();
	virtual void StepIO(int iPartIdx);
	virtual void ShutdownIO();

	virtual void Initialize();
    virtual void StepSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
	virtual void MicroSleep(unsigned int iTime);    
};

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

