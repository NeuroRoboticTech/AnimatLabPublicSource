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
	m_fltConstraintLow = (float) (-0.5*PI);
	m_fltConstraintHigh = (float) (0.5*PI);
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

}

BOOL Hinge::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Joint::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "MINANGLE")
	{
		ConstraintLow(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "MAXANGLE")
	{
		ConstraintHigh(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Hinge::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Joint::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into Joint Element

	if(oXml.FindChildElement("Constraint", FALSE))
	{
		oXml.IntoChildElement("Constraint");  //Into Constraint Element
		m_fltConstraintLow = oXml.GetAttribFloat("Low", FALSE, m_fltConstraintLow);
		m_fltConstraintHigh = oXml.GetAttribFloat("High", FALSE, m_fltConstraintHigh);
		oXml.OutOfElem(); //OutOf Constraint Element
	}

	m_fltMaxTorque = oXml.GetChildFloat("MaxTorque", m_fltMaxTorque) * lpSim->InverseMassUnits() * lpSim->InverseDistanceUnits() * lpSim->InverseDistanceUnits();

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
