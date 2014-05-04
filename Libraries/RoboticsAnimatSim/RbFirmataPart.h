// RbFirmataPart.h: interface for the RbFirmataPart class.
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

class ROBOTICS_PORT RbFirmataPart : public AnimatSim::Robotics::RobotPartInterface
{
protected:
		RbFirmataController *m_lpFirmata;

public:
	RbFirmataPart();
	virtual ~RbFirmataPart();

	virtual void ParentIOControl(RobotIOControl *lpParent);

	virtual void SetFirmata(RbFirmataController *lpFirmata);
	virtual RbFirmataController *GetFirmata();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

