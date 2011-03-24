/**
\file	Hinge.cpp

\brief	Implements the hinge class.
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "ConstraintLimit.h"
#include "Hinge.h"
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
Hinge::Hinge()
{
	m_lpUpperLimit = NULL;
	m_lpLowerLimit = NULL;
	m_lpPosFlap = NULL;
	m_fltMaxTorque = 1000;
	m_bServoMotor = FALSE;
	m_ftlServoGain = 100;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
Hinge::~Hinge()
{
try
{
	if(m_lpUpperLimit)
	{
		delete m_lpUpperLimit;
		m_lpUpperLimit = NULL;
	}

	if(m_lpLowerLimit)
	{
		delete m_lpLowerLimit;
		m_lpLowerLimit = NULL;
	}

	if(m_lpPosFlap)
	{
		delete m_lpPosFlap;
		m_lpPosFlap = NULL;
	}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Hinge\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the radius cylinder of the cylinder used to display the hinge in the environment.

\author	dcofer
\date	3/24/2011

\return	Radius of hinge cylinder.
**/
float Hinge::CylinderRadius() 
{
	return m_fltSize * 0.25f;
};

/**
\brief	Gets the height of the cylinder used to display the hinge in the environment.

\author	dcofer
\date	3/24/2011

\return	Height of hinge cylidner.
**/
float Hinge::CylinderHeight() 
{
	return m_fltSize;
};

/**
\brief	Gets the width of the flaps used to display the hinge in the environment.

\author	dcofer
\date	3/24/2011

\return	Flap width.
**/
float Hinge::FlapWidth() 
{
	return m_fltSize * 0.05f;
};

void Hinge::Enabled(BOOL bValue) 
{
	EnableMotor(bValue);
	m_bEnabled = bValue;
}

/**
\brief	Gets a pointer to the upper limit ConstraintLimit.

\author	dcofer
\date	3/24/2011

\return	Pointer to ConstraintLimit.
**/
ConstraintLimit *Hinge::UpperLimit() {return m_lpUpperLimit;}

/**
\brief	Gets a pointer to the lower limit ConstraintLimit.

\author	dcofer
\date	3/24/2011

\return	Pointer to ConstraintLimit.
**/
ConstraintLimit *Hinge::LowerLimit() {return m_lpLowerLimit;}

/**
\brief	Sets whether this is a servo motor or not.

\details A servo motor is one where the position of the joint is specified instead of the velocity that is normally
used to control the motor.

\author	dcofer
\date	3/24/2011

\param	bServo	true to set to be a servo motor. 
**/
void Hinge::ServoMotor(BOOL bServo) {m_bServoMotor = bServo;}

/**
\brief	Gets whether this is set to be a servo motor.

\author	dcofer
\date	3/24/2011

\return	true if it is a servo motor, false otherwise.
**/
BOOL Hinge::ServoMotor() {return m_bServoMotor;}

/**
\brief	Sets the servo gain used to calculate the new velocity for maintaining a position with a servo motor.

\author	dcofer
\date	3/24/2011

\param	fltVal	The new gain value. 
**/
void Hinge::ServoGain(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "ServoGain", TRUE);
	m_ftlServoGain= fltVal;
}

/**
\brief	Gets the servo gain.

\author	dcofer
\date	3/24/2011

\return	Servo gain.
**/
float Hinge::ServoGain() {return m_ftlServoGain;}

/**
\brief	Sets the Maximum torque.

\author	dcofer
\date	3/24/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Hinge::MaxTorque(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "MaxTorque");

	if(bUseScaling)
		fltVal *= m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits() * m_lpSim->InverseDistanceUnits();

	m_fltMaxTorque = fltVal;

	//If max torque is over 1000 N then assume we mean infinity.
	if(m_fltMaxTorque >= 1000)
		m_fltMaxTorque = 1e35f;
}

/**
\brief	Gets the maximum torque.

\author	dcofer
\date	3/24/2011

\return	Maximum torque the motor can apply.
**/
float Hinge::MaxTorque() {return m_fltMaxTorque;}


BOOL Hinge::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Joint::SetData(strType, strValue, FALSE))
		return TRUE;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Hinge::Load(CStdXml &oXml)
{
	Joint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	m_lpUpperLimit->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
	m_lpLowerLimit->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
	m_lpPosFlap->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, JointPosition());

	m_lpUpperLimit->Load(oXml, "UpperLimit");
	m_lpLowerLimit->Load(oXml, "LowerLimit");

	MaxTorque(oXml.GetChildFloat("MaxTorque", m_fltMaxTorque));
	ServoMotor(oXml.GetChildBool("ServoMotor", m_bServoMotor));
	ServoGain(oXml.GetChildFloat("ServoGain", m_ftlServoGain));

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
