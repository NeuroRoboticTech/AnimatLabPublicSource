// RbFirmataController.cpp: implementation of the RbFirmataController class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbHingeLimit.h"
#include "RbHinge.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbFirmataPart.h"
#include "RbFirmataController.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbFirmataController::RbFirmataController() 
{
	m_strComPort = "COM3";
	m_iBaudRate = 57600;

	// listen for EInitialized notification. this indicates that
	// the arduino is ready to receive commands and it is safe to
	// call setupArduino()
	m_EInitializedConnection = this->EInitialized.connect(boost::bind(&RbFirmataController::setupArduino, this, _1));

}

RbFirmataController::~RbFirmataController()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbFirmataController\r\n", "", -1, false, true);}
}

void RbFirmataController::ComPort(std::string strPort)
{
	if(Std_IsBlank(strPort))
		THROW_PARAM_ERROR(Rb_Err_lInvalidPort, Rb_Err_strInvalidPort, "ComPort", strPort);

	m_strComPort = strPort;
}

std::string RbFirmataController::ComPort() {return m_strComPort;}

void RbFirmataController::BaudRate(int iRate)
{
	if( iRate <= 0 )
		THROW_PARAM_ERROR(Rb_Err_lInvalidBaudRate, Rb_Err_strInvalidBaudRate, "Baud rate", iRate);
	m_iBaudRate = iRate;
}

int RbFirmataController::BaudRate() {return m_iBaudRate;}

#pragma region DataAccesMethods

float *RbFirmataController::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "LIMITPOS")
	//	return &m_fltLimitPos;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RbFirmataController::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RobotIOControl::SetData(strDataType, strValue, false))
		return true;

	if(strType == "COMPORT")
	{
		ComPort(strValue);
		return true;
	}
	else if(strType == "BAUDRATE")
	{
		BaudRate((int) atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbFirmataController::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotIOControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ComPort", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("BaudRate", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbFirmataController::Initialize()
{
	// Open device. Do this before calling the Initialize on the parts so they can have communications.
	if(!m_lpSim->InSimulation())
	{
		std::cout << "opening connection\r\n";
		
		//Try to connect to the arduino board.
		if(!connect(m_strComPort, m_iBaudRate))
			THROW_PARAM_ERROR(Rb_Err_lErrorConnectingToArduino, Rb_Err_strErrorConnectingToArduino, "ComPort", m_strComPort);

		StartIOThread();
	}

	RobotIOControl::Initialize();
}

void RbFirmataController::ProcessIO()
{
	try
	{
		m_bIOThreadProcessing = true;
		
		std::cout << "Sending firmware version request\r\n";

		//Need to do this to init the pins, get the firmware version, and  call setupArduino.
		//Will stay in update loop looking for signal. When it arrives Setup will be called
		//and we can start processing.
		sendFirmwareVersionRequest();

		while(!m_bStopIO)
		{
			//Update the firmata IO.
			update();

			//Do not try and step IO until it has been setup correctly.
			if(m_bSetupComplete)
			{
				StepIO();

				//Execute any synch moves that were setup for this IO loop in StepIO
				//If none were setup it will ignore this call.
				sendDynamixelSynchMoveExecute();
			}

			//platformstl::micro_sleep(5);
		}
	}
	catch(CStdErrorInfo oError)
	{
		m_bIOThreadProcessing = false;
	}
	catch(...)
	{
		m_bIOThreadProcessing = false;
	}

	m_bIOThreadProcessing = false;
}


void RbFirmataController::setupArduino(const int & version)
{
	m_EInitializedConnection.disconnect();
    
    // it is now safe to send commands to the Arduino
    m_bSetupComplete = true;

    // print firmware name and version to the console
    std::cout << this->getFirmwareName(); 
	std::cout << "firmata v" << this->getMajorFirmwareVersion() << "." << this->getMinorFirmwareVersion();

	SetupIO();

 //       
 //   // Note: pins A0 - A5 can be used as digital input and output.
 //   // Refer to them as pins 14 - 19 if using StandardFirmata from Arduino 1.0.
 //   // If using Arduino 0022 or older, then use 16 - 21.
 //   // Firmata pin numbering changed in version 2.3 (which is included in Arduino 1.0)
 //   
 //   // set pins D2 and A5 to digital input
 //   this->sendDigitalPinMode(2, ARD_INPUT);

 //   // set pin A0 to analog input
 //   this->sendAnalogPinReporting(0, ARD_ANALOG);
 //   
 //   // set pin D13 as digital output
	//this->sendDigitalPinMode(13, ARD_OUTPUT);
 //   // set pin A4 as digital output
 //   this->sendDigitalPinMode(18, ARD_OUTPUT);  // pin 20 if using StandardFirmata from Arduino 0022 or older

 //   // set pin D11 as PWM (analog output)
	//this->sendDigitalPinMode(11, ARD_PWM);
 //   
 //   // attach a servo to pin D9
 //   // servo motors can only be attached to pin D3, D5, D6, D9, D10, or D11
 //   this->sendServoAttach(9);
	//this->sendServo(9, 0, true);

    // Listen for changes on the digital and analog pins
	//m_EDigitalPinChanged = this->EDigitalPinChanged.connect(boost::bind(&RbFirmataController::digitalPinChanged, this, _1));
	//m_EAnalogPinChanged = this->EAnalogPinChanged.connect(boost::bind(&RbFirmataController::analogPinChanged, this, _1));

	m_WaitForIOSetupCond.notify_all();
}

void RbFirmataController::ExitIOThread()
{
	RobotIOControl::ExitIOThread();

	_port.close();
}

// digital pin event handler, called whenever a digital pin value has changed
// note: if an analog pin has been set as a digital pin, it will be handled
// by the digitalPinChanged function rather than the analogPinChanged function.

//--------------------------------------------------------------
void RbFirmataController::digitalPinChanged(const int & pinNum) 
{
 //   // do something with the digital input. here we're simply going to print the pin number and
 //   // value to the screen each time it changes
	//int iVal = this->getDigital(pinNum);
    //std::cout << "digital pin: " << pinNum << " = " << iVal << "\r\n";

	//this->sendDigital(13, iVal);
}

// analog pin event handler, called whenever an analog pin value has changed

//--------------------------------------------------------------
void RbFirmataController::analogPinChanged(const int & pinNum) 
{
 //   // do something with the analog input. here we're simply going to print the pin number and
 //   // value to the screen each time it changes
	//int iVal = this->getAnalog(pinNum);
 //   std::cout << "analog pin: " << pinNum << " = " << iVal << "\r\n";
}

void RbFirmataController::Load(StdUtils::CStdXml &oXml)
{
	RobotIOControl::Load(oXml);

	oXml.IntoElem();
	ComPort(oXml.GetChildString("ComPort", m_strComPort));
	BaudRate(oXml.GetChildInt("BaudRate", m_iBaudRate));
	oXml.OutOfElem();
}


			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

