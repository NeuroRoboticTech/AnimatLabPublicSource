// RbFirmataHingeServo.h: interface for the RbFirmataHingeServo class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

class ROBOTICS_PORT RbFirmataHingeServo : public RbFirmataPart
{
protected:
    MotorizedJoint *m_lpJoint;

	int m_iMaxPulse;
	int m_iMinPulse;
	bool m_bResetToStartPos;

	int m_iMaxAngle;
	int m_iMinAngle;
	int m_iCenterAngle;

	float m_fltMaxAngle;
	float m_fltMinAngle;

	///The conversion factor to convert radians to FP angle position.
	float m_fltPosFPToRadSlope;
	float m_fltPosFPToRadIntercept;

	float m_fltPosRadToFPSlope;
	float m_fltPosRadToFPIntercept;

	int m_iLastGoalPos;

public:
	RbFirmataHingeServo();
	virtual ~RbFirmataHingeServo();

	virtual int MaxPulse();
	virtual void MaxPulse(int iPulse);

	virtual int MinPulse();
	virtual void MinPulse(int iPulse);

	virtual bool ResetToStartPos();
	virtual void ResetToStartPos(bool bVal);

	virtual void SetupIO();
	virtual void StepIO();
	
	virtual float ConvertPosFPToRad(int iPos);
	virtual int ConvertPosRadToFP(float fltPos);

	virtual void SetGoalPosition_FP(int iPos);
	virtual void SetGoalPosition(float fltPos);

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void Initialize();
    virtual void StepSimulation();
	virtual void Load(CStdXml &oXml);
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

