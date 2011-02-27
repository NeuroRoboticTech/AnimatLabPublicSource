// Joint.cpp: implementation of the Joint class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a joint object..
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a joint. 
*/

Joint::Joint()
{
	m_lpChild = NULL;
	m_fltSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltMaxVelocity = 100;
	m_fltPrevVelocity = -1000000;
	m_bEnableMotor = FALSE;
	m_bEnableMotorInit = FALSE;
	m_fltPosition = 0;
	m_fltVelocity = 0;
	m_fltForce = 0;
	m_fltSize = 0.02f;
}


/*! \brief 
   Destroys the joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the joint object..	 
*/

Joint::~Joint()
{
	//We also do not delete our references to these objects.
	m_lpParent = NULL;
	m_lpChild = NULL;
}

float Joint::Size() {return m_fltSize;};

void Joint::Size(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Joint.Size");
	if(bUseScaling)
		m_fltSize = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSize = fltVal;

	Resize();
}

float Joint::EnableMotor() {return m_bEnableMotor;};

void Joint::EnableMotor(BOOL bVal)
{
	m_bEnableMotor = bVal;
	//TODO Add sim code here.
}

float Joint::MaxVelocity() {return m_fltMaxVelocity;};

void Joint::MaxVelocity(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Joint.MaxVelocity");

	if(bUseScaling && !UsesRadians())
		m_fltMaxVelocity = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMaxVelocity = fltVal;

	//TODO Add sim code here.
}

//Node Overrides
void Joint::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput)
{
	m_fltDesiredVelocity += fltInput;
}

void Joint::SetVelocityToDesired()
{
	m_fltSetVelocity = m_fltDesiredVelocity;
	m_fltDesiredVelocity = 0;
}

void Joint::CreateJoint(Simulator *lpSim, Structure *lpStructure)
{}


/*! \brief 
   Allows the joint to update itself for each timeslice.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                      this rigid body is a part of.
   \param lStep This is the current time slice.

	 \return
	 No return value.

	 \remarks
   This function is called for each joint on every
   time slice. It allows the joint to update itself. You need 
	 to be VERY careful to keep all code within the StepSimulation methods short, sweet, 
	 and very fast. They are in the main processing loop and even a small increase in the
	 amount of processing time that occurrs within this loop will lead to major impacts on
	 the ultimate performance of the system. 

   \sa
   Joint::StepSimulation, Simulator::StepSimulation
*/

void Joint::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	UpdateData(lpSim, lpStructure);
}


float *Joint::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	float *lpData = BodyPart::GetDataPointer(strDataType);
	if(lpData)
		return lpData;

	if(strType == "ENABLE")
		return &m_fltEnabled;

	if(m_lpPhysicsBody)
	{
		float *lpData = NULL;
		lpData = m_lpPhysicsBody->Physics_GetDataPointer(strDataType);
		if(lpData) return lpData;
	}

	return NULL;
}

BOOL Joint::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(BodyPart::SetData(strType, strValue, FALSE))
		return TRUE;

	//if(strType == "LENGTH")
	//{
	//	Length(atof(strValue.c_str()));
	//	return TRUE;
	//}

	//if(strType == "WIDTH")
	//{
	//	Width(atof(strValue.c_str()));
	//	return TRUE;
	//}

	//if(strType == "HEIGHT")
	//{
	//	Height(atof(strValue.c_str()));
	//	return TRUE;
	//}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Joint::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	m_fltSetVelocity = 0;
	m_fltDesiredVelocity = 0;
	m_fltPrevVelocity = 0;

	EnableMotor(m_bEnableMotorInit);
	//EnableLimits(m_bEnableLimitsInit);

	//BOOL m_bEnableMotor;
	//BOOL m_bEnableLimits;

	m_fltPosition = 0;
	m_fltVelocity = 0;
	m_fltForce = 0;
}

/*! \brief 
   Loads a joint from an xml configuration file.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                      this rigid body is a part of.
   \param oXml This is an xml object.

	 \return
	 No return value.

	 \remarks
	 This method is responsible for loading the joint from a XMl
	 configuration file. You should call this method even in your 
	 overriden function becuase it loads all of the base properties
	 for the Joint. 
*/

void Joint::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	BodyPart::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into Joint Element

	if(!m_lpParent)
		THROW_PARAM_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined, "JointID", m_strName);

	if(!m_lpChild)
		THROW_PARAM_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined, "JointID", m_strName);

	//Reset the absolute position differently for a joint. It is derived from the child object, not the parent.
	m_oAbsPosition = m_lpChild->AbsolutePosition() + m_oLocalPosition;
	m_oReportLocalPosition = m_oLocalPosition * lpSim->DistanceUnits();
	m_oReportWorldPosition = m_oAbsPosition * lpSim->DistanceUnits();

	m_bEnableMotor = m_bEnableMotorInit = oXml.GetChildBool("EnableMotor", m_bEnableMotor);
	m_fltMaxVelocity = oXml.GetChildFloat("MaxVelocity", m_fltMaxVelocity);

	if(!this->UsesRadians())
		m_fltMaxVelocity *= lpSim->InverseDistanceUnits();  //Convert distance units.

	Size(oXml.GetChildFloat("Size", m_fltSize));

	oXml.OutOfElem(); //OutOf Joint Element
}



/*! \fn virtual void Joint::EnableMotor(BOOL bVal)
   \brief
   Enables the motor for a motorized joint.
      
   \param bVal Turns the motor on or off.

	 \return
	 No return value.

	 \remarks
	 If this is a motorized joint then when you turn it on the
	 physics engine will calculate the torque that needs to be
	 applied to this joint in order for it to have the desired
	 Velocity for its current load. This is a pure virtual function
	 that must be overridden because you will need to call the
	 physics engine API to enable/disable the joint motor.

	 \sa
	 Velocity
*/


/*! \fn virtual void Joint::CreateJoint(Simulator *lpSim, Structure *lpStructure)
   \brief
   Enables the motor for a motorized joint.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                      this rigid body is a part of.

	 \return
	 No return value.

	 \remarks
	 This method is used to create the joint and attach it to the
	 parent and child parts.

	 \sa
	 CreateJoints, CreateParts
*/


/*! \fn unsigned char Joint::Type()
   \brief
   Joint type property.
      
   \remarks
   The type for this joint. Examples are Static, Hinge, etc..
	 This is the read-only accessor function for the m_iType element.
*/


/*! \fn string Joint::ID()
   \brief
   Joint ID property.
      
   \remarks
	 The unique Id for this joint. It is unique for each structure, 
	 but not across structures. So you could have two joints with the
	 same ID in two different organisms.
	 This is the accessor function for the m_strID element.
*/
/*! \fn void Joint::ID(string strValue)
   \brief
   Joint ID property.
      
   \remarks
	 The unique Id for this joint. It is unique for each structure, 
	 but not across structures. So you could have two joints with the
	 same ID in two different organisms.
	 This is the mutator function for the m_strID element.
*/


/*! \fn CStdFPoint Joint::RelativePosition()
   \brief
   RelativePosition property.
      
   \remarks
	 The relative position of the of this joint
	 in relation to the center of its parent rigid body. 
	 This is the accessor function for the m_oRelPosition element.
*/
/*! \fn void Joint::RelativePosition(CStdFPoint &oPoint)
   \brief
   RelativePosition property.
      
   \remarks
	 The relative position of the of this joint
	 in relation to the center of its parent rigid body. 
	 This is the mutator function for the m_oRelPosition element.
*/


/*! \fn CStdFPoint Joint::AbsolutePosition()
   \brief
   AbsolutePosition property.
      
   \remarks
	 The absolute position of the joint in world coordinates.
	 This is calcualted during loading of the joint using the position of 
	 the parent part and the relative position specified in the configuration file.
	 This is the accessor function for the m_oAbsPosition element.
*/
/*! \fn void Joint::AbsolutePosition(CStdFPoint &oPoint)
   \brief
   AbsolutePosition property.
      
   \remarks
	 The absolute position of the joint in world coordinates.
	 This is calcualted during loading of the joint using the position of 
	 the parent part and the relative position specified in the configuration file.
	 This is the mutator function for the m_oAbsPosition element.
*/


/*! \fn Body *Joint::Parent()
   \brief
   Parent property.
      
   \remarks
	 The parent rigid body for this joint. 
	 This is the accessor function for the m_lpParent element.
*/
/*! \fn void Joint::Parent(Body *lpValue)
   \brief
   Parent property.
      
   \remarks
	 The parent rigid body for this joint. 
	 This is the mutator function for the m_lpParent element.
*/


/*! \fn Body *Joint::Child()
   \brief
   Child property.
      
   \remarks
	 The child rigid body for this joint. 
	 This is the accessor function for the m_lpChild element.
*/
/*! \fn void Joint::Child(Body *lpValue)
   \brief
   Child property.
      
   \remarks
	 The child rigid body for this joint. 
	 This is the mutator function for the m_lpChild element.
*/


/*! \fn float Joint::Velocity()
   \brief
   Velocity property.
      
   \remarks
	 This is the velocity to use for the motorized joint. The motor must be enabled
	 for this parameter to have any effect.
	 This is the accessor functions for the m_fltVelocity element.
*/
/*! \fn virtual void Joint::Velocity(float fltVelocity)
   \brief
   Velocity property.
      
   \remarks
	 This is the velocity to use for the motorized joint. The motor must be enabled
	 for this parameter to have any effect.
	 This is the mutator functions for the m_fltVelocity element.
	 The mutator function is actually a pure virtual function that needs to
	 be overloaded this is because in the particular joint when you set the
	 velocity if the motor is enabled then you need to make the physics engine
	 API calls to set the velocity of that motor.
*/

	}			//Environment
}				//AnimatSim
