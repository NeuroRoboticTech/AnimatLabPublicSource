/**
\file	Joint.cpp

\brief	Implements the joint class.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
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
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/22/2011
**/
Joint::Joint()
{
	m_lpChild = NULL;
	m_fltPosition = 0;
	m_fltVelocity = 0;
	m_fltForce = 0;
	m_fltSize = 0.02f;
	m_bEnableLimits = true;

    for(int iIdx=0; iIdx<6; iIdx++)
        m_aryRelaxations[iIdx] = NULL;

    m_lpFriction = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
Joint::~Joint()
{

try
{
	//We also do not delete our references to these objects.
	m_lpParent = NULL;
	m_lpChild = NULL;

    ClearRelaxations();

    if(m_lpFriction) {delete m_lpFriction; m_lpFriction = NULL;}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Joint\r\n", "", -1, false, true);}
}

			
/**
\brief	Tells whether this joint uses radians or meters for its measurements.

\details This is defaulted to true. You must override this and set it to the appropriate
value for your derived classes.

\author	dcofer
\date	3/22/2011

\return	true if it uses radians, false if it uses meters.
**/
bool Joint::UsesRadians() {return true;}

/**
\brief	Gets the size of the graphical representation of this joint.

\author	dcofer
\date	3/22/2011

\return	Size for the graphics object.
**/
float Joint::Size() {return m_fltSize;};

/**
\brief	Sets the size of the graphical representation of this joint.

\author	dcofer
\date	3/22/2011

\param	fltVal	   	The new size value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Joint::Size(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Joint.Size");
	if(bUseScaling)
		m_fltSize = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSize = fltVal;

	Resize();
}



/**
\brief	Tells if ConstraintLimits are enabled.

\author	dcofer
\date	3/22/2011

\return	true if it limits are enabled, false otherwise.
**/
bool Joint::EnableLimits() {return m_bEnableLimits;};

/**
\brief	Sets whether ContrainLimits are enabled or not.

\author	dcofer
\date	3/22/2011

\param	bVal	true to enable. 
**/
void Joint::EnableLimits(bool bVal) {m_bEnableLimits = bVal;}

/**
\brief	Gets the pointer to the primary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::Relaxation1() {return m_aryRelaxations[0];}

/**
\brief	Sets the pointer to the primary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::Relaxation1(ConstraintRelaxation *lpRelax)
{
    if(m_aryRelaxations[0])
    {
        delete m_aryRelaxations[0];
        m_aryRelaxations[0] = NULL;
    }

    m_aryRelaxations[0] = lpRelax;
}

/**
\brief	Sets the primary axis displacement relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::Relaxation1(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    Relaxation1(LoadConstraintRelaxation(oXml, "Relaxation1"));
}

/**
\brief	Gets the pointer to the secondary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::Relaxation2() {return m_aryRelaxations[1];}

/**
\brief	Sets the pointer to the secondary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::Relaxation2(ConstraintRelaxation *lpRelax)
{
    if(m_aryRelaxations[1])
    {
        delete m_aryRelaxations[1];
        m_aryRelaxations[1] = NULL;
    }

    m_aryRelaxations[1] = lpRelax;
}

/**
\brief	Sets the secondary axis displacement relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::Relaxation2(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    Relaxation2(LoadConstraintRelaxation(oXml, "Relaxation2"));
}

/**
\brief	Gets the pointer to the secondary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::Relaxation3() {return m_aryRelaxations[2];}

/**
\brief	Sets the pointer to the third axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::Relaxation3(ConstraintRelaxation *lpRelax)
{
    if(m_aryRelaxations[2])
    {
        delete m_aryRelaxations[2];
        m_aryRelaxations[2] = NULL;
    }

    m_aryRelaxations[2] = lpRelax;
}

/**
\brief	Sets the third axis displacement relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::Relaxation3(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    Relaxation3(LoadConstraintRelaxation(oXml, "Relaxation3"));
}

/**
\brief	Gets the pointer to the secondary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::Relaxation4() {return m_aryRelaxations[3];}

/**
\brief	Sets the pointer to the secondary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::Relaxation4(ConstraintRelaxation *lpRelax)
{
    if(m_aryRelaxations[3])
    {
        delete m_aryRelaxations[3];
        m_aryRelaxations[3] = NULL;
    }

    m_aryRelaxations[3] = lpRelax;
}

/**
\brief	Sets the secondary axis rotation relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::Relaxation4(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    Relaxation4(LoadConstraintRelaxation(oXml, "Relaxation4"));
}

/**
\brief	Gets the pointer to the thirdary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::Relaxation5() {return m_aryRelaxations[4];}

/**
\brief	Sets the pointer to the thirdary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::Relaxation5(ConstraintRelaxation *lpRelax)
{
    if(m_aryRelaxations[4])
    {
        delete m_aryRelaxations[4];
        m_aryRelaxations[4] = NULL;
    }

    m_aryRelaxations[4] = lpRelax;
}

/**
\brief	Sets the thirdary axis rotation relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::Relaxation5(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    Relaxation5(LoadConstraintRelaxation(oXml, "Relaxation5"));
}

/**
\brief	Gets the pointer to the thirdary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::Relaxation6() {return m_aryRelaxations[5];}

/**
\brief	Sets the pointer to the thirdary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::Relaxation6(ConstraintRelaxation *lpRelax)
{
    if(m_aryRelaxations[5])
    {
        delete m_aryRelaxations[5];
        m_aryRelaxations[5] = NULL;
    }

    m_aryRelaxations[5] = lpRelax;
}

/**
\brief	Sets the thirdary axis rotation relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::Relaxation6(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    Relaxation6(LoadConstraintRelaxation(oXml, "Relaxation6"));
}

/**
\brief	Gets the pointer to the friction

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintFriction *Joint::Friction() {return m_lpFriction;}

/**
\brief	Sets the pointer to the friction 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::Friction(ConstraintFriction *lpRelax)
{
    if(m_lpFriction)
    {
        delete m_lpFriction;
        m_lpFriction = NULL;
    }

    m_lpFriction = lpRelax;
}

/**
\brief	Sets the friction

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the friction. 
**/
void Joint::Friction(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    Friction(LoadConstraintFriction(oXml));
}


/**
\brief	Gets a position value within the constraint limits.

\details If limits are enabled then it checks to see if the specified
position is within the limits. If it is not, then it adjusts the position
to be at the limit and returns that value.

\param	fltPos	The position to check. 

\return	The position within limits.
**/
float Joint::GetPositionWithinLimits(float fltPos)
{return fltPos;}

/**
\brief Gets the entire range of movement within the limits. If limits are
not enabled then it returns -1.

\return	The calculated limit range.
**/
float Joint::GetLimitRange()
{return -1;}

int Joint::VisualSelectionType() {return JOINT_SELECTION_MODE;}

/**
\brief	Gets the child RigidBody part for this joint.

\author	dcofer
\date	3/22/2011

\return	Pointer to the child RigidBody for this joint.
**/
RigidBody *Joint::Child() {return m_lpChild;}

/**
\brief	Sets the Child RigidBody part for this joint.

\author	dcofer
\date	3/22/2011

\param [in,out]	lpValue	IPointer to the child part. 
**/
void Joint::Child(RigidBody *lpValue) 
{
	m_lpChild = lpValue;
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_SetChild(lpValue);
}

/**
\brief	Gets the joint position.

\author	dcofer
\date	3/22/2011

\return	Joint position.
**/
float Joint::JointPosition() {return m_fltPosition;}

/**
\brief	Sets the joint position.

\author	dcofer
\date	3/22/2011

\param	fltPos	The new position. 
**/
void Joint::JointPosition(float fltPos) {m_fltPosition = fltPos;}

/**
\brief	Gets the joint velocity.

\author	dcofer
\date	3/22/2011

\return	Joint Velocity.
**/
float Joint::JointVelocity() {return m_fltVelocity;}

/**
\brief	Sets the joint velocity.

\author	dcofer
\date	3/22/2011

\param	fltVel	The new velocity. 
**/
void Joint::JointVelocity(float fltVel) {m_fltVelocity = fltVel;}

/**
\brief	Gets the joint force.

\author	dcofer
\date	3/22/2011

\return	Joint force.
**/
float Joint::JointForce() {return m_fltForce;}

/**
\brief	Sets the joint force.

\author	dcofer
\date	3/22/2011

\param	fltForce	The new force. 
**/
void Joint::JointForce(float fltForce) {m_fltForce = fltForce;}

void Joint::WakeDynamics()
{
    if(m_lpParent)
        m_lpParent->WakeDynamics();

    if(m_lpChild)
        m_lpChild->WakeDynamics();
}

/**
\brief	Creates the joint.

\details This method is called by the derived class in the physics library. It makes the calls necessary to create the actual joint
using the chosen phsyics API.

\author	dcofer
\date	3/22/2011
**/
void Joint::CreateJoint()
{}

void Joint::Initialize()
{
    BodyPart::Initialize();

    for(int iIdx=0; iIdx<6; iIdx++)
        if(m_aryRelaxations[iIdx]) m_aryRelaxations[iIdx]->Initialize();

    if(m_lpFriction) m_lpFriction->Initialize();

}

void Joint::StepSimulation()
{
    BodyPart::StepSimulation();
	UpdateData();
}


float *Joint::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "ENABLE")
		return &m_fltEnabled;

	if(m_lpPhysicsMovableItem)
	{
		float *lpData = NULL;
		lpData = m_lpPhysicsMovableItem->Physics_GetDataPointer(strDataType);
		if(lpData) return lpData;
	}

	return BodyPart::GetDataPointer(strDataType);
}

bool Joint::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(BodyPart::SetData(strType, strValue, false))
		return true;

	if(strType == "ENABLELIMITS")
	{
		EnableLimits(Std_ToBool(strValue));
		return true;
	}

	if(strType == "SIZE")
	{
		Size((float) atof(strValue.c_str()));
		return true;
	}

    if(strType == "RELAXATION1")
    {
        Relaxation1(strValue);
    }

    if(strType == "RELAXATION2")
    {
        Relaxation2(strValue);
    }

    if(strType == "RELAXATION3")
    {
        Relaxation3(strValue);
    }

    if(strType == "RELAXATION4")
    {
        Relaxation4(strValue);
    }

    if(strType == "RELAXATION5")
    {
        Relaxation5(strValue);
    }

    if(strType == "RELAXATION6")
    {
        Relaxation6(strValue);
    }

	if(strType == "FRICTION")
    {
        Friction(strValue);
    }

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Joint::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	BodyPart::QueryProperties(aryNames, aryTypes);

	aryNames.Add("EnableLimits");
	aryTypes.Add("Boolean");

	aryNames.Add("Size");
	aryTypes.Add("Float");

	aryNames.Add("Relaxation1");
	aryTypes.Add("Xml");

	aryNames.Add("Relaxation2");
	aryTypes.Add("Xml");

	aryNames.Add("Relaxation3");
	aryTypes.Add("Xml");

	aryNames.Add("Relaxation4");
	aryTypes.Add("Xml");

	aryNames.Add("Relaxation5");
	aryTypes.Add("Xml");

	aryNames.Add("Relaxation6");
	aryTypes.Add("Xml");

	aryNames.Add("Friction");
	aryTypes.Add("Xml");
}

void Joint::AddExternalNodeInput(float fltInput) {}

void Joint::ResetSimulation()
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_ResetSimulation();

	JointPosition(0);
	JointVelocity(0);
	JointForce(0);
}

void Joint::AfterResetSimulation()
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_AfterResetSimulation();
}

void Joint::UpdatePhysicsPosFromGraphics()
{
	UpdateAbsolutePosition();
}

/**
 \brief Clears the relaxation pointers.

 \author    David Cofer
 \date  2/16/2014
 */
void Joint::ClearRelaxations()
{
    for(int iIdx=0; iIdx<6; iIdx++)
    if(m_aryRelaxations[iIdx]) 
    {
        delete m_aryRelaxations[iIdx]; 
        m_aryRelaxations[iIdx] = NULL;
    }
}

void Joint::Load(CStdXml &oXml)
{
	BodyPart::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	if(!m_lpParent)
		THROW_PARAM_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined, "JointID", m_strName);

	if(!m_lpChild)
		THROW_PARAM_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined, "JointID", m_strName);

	//Reset the absolute position differently for a joint. It is derived from the child object, not the parent.
	//AbsolutePosition(m_lpChild->AbsolutePosition() + m_oPosition);

	EnableLimits(oXml.GetChildBool("EnableLimits", m_bEnableLimits));

	Size(oXml.GetChildFloat("Size", m_fltSize));

    ClearRelaxations();

    if(m_lpFriction) {delete m_lpFriction; m_lpFriction = NULL;} 

    if(oXml.FindChildElement("Relaxation1", false))
        m_aryRelaxations[0] = LoadConstraintRelaxation(oXml, "Relaxation1");

    if(oXml.FindChildElement("Relaxation2", false))
        m_aryRelaxations[1] = LoadConstraintRelaxation(oXml, "Relaxation2");

    if(oXml.FindChildElement("Relaxation3", false))
        m_aryRelaxations[2] = LoadConstraintRelaxation(oXml, "Relaxation3");

    if(oXml.FindChildElement("Relaxation4", false))
        m_aryRelaxations[3] = LoadConstraintRelaxation(oXml, "Relaxation4");

    if(oXml.FindChildElement("Relaxation5", false))
        m_aryRelaxations[4] = LoadConstraintRelaxation(oXml, "Relaxation5");

    if(oXml.FindChildElement("Relaxation6", false))
        m_aryRelaxations[5] = LoadConstraintRelaxation(oXml, "Relaxation6");

    if(oXml.FindChildElement("Friction", false))
        m_lpFriction = LoadConstraintFriction(oXml);    

	oXml.OutOfElem(); //OutOf Joint Element
}

/**
\brief	Loads constraint relaxation object.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml	The xml that defines the relaxation to load. 

\return	Pointer to the relaxation object.
\exception Throws an exception if there is a problem creating or loading the relaxation.

**/
ConstraintRelaxation *Joint::LoadConstraintRelaxation(CStdXml &oXml, std::string strName)
{
	ConstraintRelaxation *lpConstraintRelaxation = NULL;
	std::string strModule;
	std::string strType;

try
{
    oXml.FindChildElement(strName);
	oXml.IntoElem(); //Into Child Element
	strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpConstraintRelaxation = dynamic_cast<ConstraintRelaxation *>(m_lpSim->CreateObject(strModule, "ConstraintRelaxation", strType));
	if(!lpConstraintRelaxation)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "ConstraintRelaxation");

	lpConstraintRelaxation->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, true);
	lpConstraintRelaxation->Load(oXml);

	return lpConstraintRelaxation;
}
catch(CStdErrorInfo oError)
{
	if(lpConstraintRelaxation) delete lpConstraintRelaxation;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpConstraintRelaxation) delete lpConstraintRelaxation;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


/**
\brief	Loads constraint Friction object.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml	The xml that defines the Friction to load. 

\return	Pointer to the Friction object.
\exception Throws an exception if there is a problem creating or loading the Friction.

**/
ConstraintFriction *Joint::LoadConstraintFriction(CStdXml &oXml)
{
	ConstraintFriction *lpConstraintFriction = NULL;
	std::string strModule;
	std::string strType;

try
{
    oXml.FindChildElement("Friction");
	oXml.IntoElem(); //Into Child Element
	strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpConstraintFriction = dynamic_cast<ConstraintFriction *>(m_lpSim->CreateObject(strModule, "ConstraintFriction", strType));
	if(!lpConstraintFriction)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "ConstraintFriction");

	lpConstraintFriction->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, true);
	lpConstraintFriction->Load(oXml);

	return lpConstraintFriction;
}
catch(CStdErrorInfo oError)
{
	if(lpConstraintFriction) delete lpConstraintFriction;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpConstraintFriction) delete lpConstraintFriction;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


	}			//Environment
}				//AnimatSim
