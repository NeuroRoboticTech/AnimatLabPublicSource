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

	//The number update cycles till we update all params. Typically we only want to update
	//position and speed of the servo every time. We will only update the other params every now 
	//and then. This tells how often to do this.
	int m_iUpdateAllParamsCount;

	//Keeps track of the number of update loops that have occurred since we last updated all params.
	int m_iUpdateIdx;

public:
	RbDynamixelUSBHinge();
	virtual ~RbDynamixelUSBHinge();

	virtual void IOComponentID(int iID);

	virtual void UpdateAllParamsCount(int iVal);
	virtual int UpdateAllParamsCount();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion
	
	virtual void SetupIO();
	virtual void StepIO();
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

