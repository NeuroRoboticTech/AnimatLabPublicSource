// Prismatic.cpp: implementation of the Prismatic class.
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
#include "Prismatic.h"
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
   Constructs a Prismatic joint.
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a Prismatic joint. 
*/

Prismatic::Prismatic()
{
	m_fltConstraintLow = (float) (-0.5*PI);
	m_fltConstraintHigh = (float) (0.5*PI);
	m_fltMaxForce = 1000;
	m_bServoMotor = FALSE;
	m_ftlServoGain = 100;
	m_fltPosition = 0;
}


/*! \brief 
   Destroys the Prismatic joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Prismatic joint object..	 
*/

Prismatic::~Prismatic()
{

}

void Prismatic::Load(CStdXml &oXml)
{
	Joint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	if(oXml.FindChildElement("Constraint", FALSE))
	{
		oXml.IntoChildElement("Constraint");  //Into Constraint Element
		m_fltConstraintLow = oXml.GetAttribFloat("Low", m_fltConstraintLow);
		m_fltConstraintHigh = oXml.GetAttribFloat("High", m_fltConstraintHigh);
		oXml.OutOfElem(); //OutOf Constraint Element
	}

	//For a prismatic it is really max force, not max torque. I am leaving the naming alone to be consistent, but the unit conversion should be correct.
	m_fltMaxForce = oXml.GetChildFloat("MaxForce", m_fltMaxForce) * m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits();

	m_bServoMotor = oXml.GetChildBool("ServoMotor", m_bServoMotor);
	m_ftlServoGain = oXml.GetChildFloat("ServoGain", m_ftlServoGain);

	//If max torque is over 1000 N then assume we mean infinity.
	if(m_fltMaxForce >= 1000)
		m_fltMaxForce = 1e35f;

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
