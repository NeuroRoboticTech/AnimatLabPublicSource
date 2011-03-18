// Hinge.cpp: implementation of the Hinge class.
//
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a hinge joint.
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a hinge joint. 
*/

Hinge::Hinge()
{
	m_lpUpperLimit = NULL;
	m_lpLowerLimit = NULL;
	m_lpPosFlap = NULL;
	m_fltMaxTorque = 1000;
	m_bServoMotor = FALSE;
	m_ftlServoGain = 100;
}


/*! \brief 
   Destroys the hinge joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the hinge joint object..	 
*/

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

float Hinge::CylinderRadius() 
{
	return m_fltSize * 0.25f;
};

float Hinge::CylinderHeight() 
{
	return m_fltSize;
};

float Hinge::FlapWidth() 
{
	return m_fltSize * 0.05f;
};

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

	m_fltMaxTorque = oXml.GetChildFloat("MaxTorque", m_fltMaxTorque) * m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits() * m_lpSim->InverseDistanceUnits();

	//If max torque is over 1000 N then assume we mean infinity.
	if(m_fltMaxTorque >= 1000)
		m_fltMaxTorque = 1e35f;

	m_bServoMotor = oXml.GetChildBool("ServoMotor", m_bServoMotor);
	m_ftlServoGain = oXml.GetChildFloat("ServoGain", m_ftlServoGain);

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
