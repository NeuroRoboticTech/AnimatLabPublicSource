/**
\file	Joint.cpp

\brief	Implements the joint class.
**/

#include "stdafx.h"
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
	m_bEnableLimits = TRUE;

    m_lpPrimaryAxisDisplacement = NULL;
    m_lpSecondaryAxisDisplacement = NULL;
    m_lpThirdAxisDisplacement = NULL;
    m_lpSecondaryAxisRotation = NULL;
    m_lpThirdAxisRotation = NULL;
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

    if(m_lpPrimaryAxisDisplacement) {delete m_lpPrimaryAxisDisplacement; m_lpPrimaryAxisDisplacement = NULL;}
    if(m_lpSecondaryAxisDisplacement) {delete m_lpSecondaryAxisDisplacement; m_lpSecondaryAxisDisplacement = NULL;}
    if(m_lpThirdAxisDisplacement) {delete m_lpThirdAxisDisplacement; m_lpThirdAxisDisplacement = NULL;}
    if(m_lpSecondaryAxisRotation) {delete m_lpSecondaryAxisRotation; m_lpSecondaryAxisRotation = NULL;}
    if(m_lpThirdAxisRotation) {delete m_lpThirdAxisRotation; m_lpThirdAxisRotation = NULL;}
    if(m_lpFriction) {delete m_lpFriction; m_lpFriction = NULL;}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Joint\r\n", "", -1, FALSE, TRUE);}
}

			
/**
\brief	Tells whether this joint uses radians or meters for its measurements.

\details This is defaulted to TRUE. You must override this and set it to the appropriate
value for your derived classes.

\author	dcofer
\date	3/22/2011

\return	true if it uses radians, false if it uses meters.
**/
BOOL Joint::UsesRadians() {return TRUE;}

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
void Joint::Size(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Joint.Size");
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
BOOL Joint::EnableLimits() {return m_bEnableLimits;};

/**
\brief	Sets whether ContrainLimits are enabled or not.

\author	dcofer
\date	3/22/2011

\param	bVal	true to enable. 
**/
void Joint::EnableLimits(BOOL bVal) {m_bEnableLimits = bVal;}

/**
\brief	Gets the pointer to the primary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::PrimaryAxisDisplacement() {return m_lpPrimaryAxisDisplacement;}

/**
\brief	Sets the pointer to the primary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::PrimaryAxisDisplacement(ConstraintRelaxation *lpRelax)
{
    if(m_lpPrimaryAxisDisplacement)
    {
        delete m_lpPrimaryAxisDisplacement;
        m_lpPrimaryAxisDisplacement = NULL;
    }

    m_lpPrimaryAxisDisplacement = lpRelax;
}

/**
\brief	Sets the primary axis displacement relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::PrimaryAxisDisplacement(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    PrimaryAxisDisplacement(LoadConstraintRelaxation(oXml, "PrimaryAxisDisplacement"));
}

/**
\brief	Gets the pointer to the secondary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::SecondaryAxisDisplacement() {return m_lpSecondaryAxisDisplacement;}

/**
\brief	Sets the pointer to the secondary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::SecondaryAxisDisplacement(ConstraintRelaxation *lpRelax)
{
    if(m_lpSecondaryAxisDisplacement)
    {
        delete m_lpSecondaryAxisDisplacement;
        m_lpSecondaryAxisDisplacement = NULL;
    }

    m_lpSecondaryAxisDisplacement = lpRelax;
}

/**
\brief	Sets the secondary axis displacement relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::SecondaryAxisDisplacement(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    SecondaryAxisDisplacement(LoadConstraintRelaxation(oXml, "SecondaryAxisDisplacement"));
}

/**
\brief	Gets the pointer to the secondary axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::ThirdAxisDisplacement() {return m_lpThirdAxisDisplacement;}

/**
\brief	Sets the pointer to the third axis displacement relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::ThirdAxisDisplacement(ConstraintRelaxation *lpRelax)
{
    if(m_lpThirdAxisDisplacement)
    {
        delete m_lpThirdAxisDisplacement;
        m_lpThirdAxisDisplacement = NULL;
    }

    m_lpThirdAxisDisplacement = lpRelax;
}

/**
\brief	Sets the third axis displacement relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::ThirdAxisDisplacement(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    ThirdAxisDisplacement(LoadConstraintRelaxation(oXml, "ThirdAxisDisplacement"));
}

/**
\brief	Gets the pointer to the secondary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::SecondaryAxisRotation() {return m_lpSecondaryAxisRotation;}

/**
\brief	Sets the pointer to the secondary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::SecondaryAxisRotation(ConstraintRelaxation *lpRelax)
{
    if(m_lpSecondaryAxisRotation)
    {
        delete m_lpSecondaryAxisRotation;
        m_lpSecondaryAxisRotation = NULL;
    }

    m_lpSecondaryAxisRotation = lpRelax;
}

/**
\brief	Sets the secondary axis rotation relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::SecondaryAxisRotation(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    SecondaryAxisRotation(LoadConstraintRelaxation(oXml, "SecondaryAxisRotation"));
}

/**
\brief	Gets the pointer to the thirdary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
ConstraintRelaxation *Joint::ThirdAxisRotation() {return m_lpThirdAxisRotation;}

/**
\brief	Sets the pointer to the thirdary axis rotation relaxation 

\author	dcofer
\date	3/2/2011

\return	nothing 
**/
void Joint::ThirdAxisRotation(ConstraintRelaxation *lpRelax)
{
    if(m_lpThirdAxisRotation)
    {
        delete m_lpThirdAxisRotation;
        m_lpThirdAxisRotation = NULL;
    }

    m_lpThirdAxisRotation = lpRelax;
}

/**
\brief	Sets the thirdary axis rotation relaxation

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the relaxation. 
**/
void Joint::ThirdAxisRotation(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	
    ThirdAxisRotation(LoadConstraintRelaxation(oXml, "ThirdAxisRotation"));
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
void Joint::Friction(string strXml)
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

    if(m_lpPrimaryAxisDisplacement) m_lpPrimaryAxisDisplacement->Initialize();
    if(m_lpSecondaryAxisDisplacement) m_lpSecondaryAxisDisplacement->Initialize();
    if(m_lpThirdAxisDisplacement) m_lpThirdAxisDisplacement->Initialize();
    if(m_lpSecondaryAxisRotation) m_lpSecondaryAxisRotation->Initialize();
    if(m_lpThirdAxisRotation) m_lpThirdAxisRotation->Initialize();
    if(m_lpFriction) m_lpFriction->Initialize();

}

void Joint::StepSimulation()
{
	UpdateData();
}


float *Joint::GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);

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

BOOL Joint::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(BodyPart::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "ENABLELIMITS")
	{
		EnableLimits(Std_ToBool(strValue));
		return true;
	}

	if(strType == "SIZE")
	{
		Size(atof(strValue.c_str()));
		return true;
	}

    if(strType == "PRIMARYAXISDISPLACEMENT")
    {
        PrimaryAxisDisplacement(strValue);
    }

    if(strType == "SECONDARYAXISDISPLACEMENT")
    {
        SecondaryAxisDisplacement(strValue);
    }

    if(strType == "THIRDAXISDISPLACEMENT")
    {
        ThirdAxisDisplacement(strValue);
    }

    if(strType == "SECONDARYAXISROTATION")
    {
        SecondaryAxisRotation(strValue);
    }

    if(strType == "THIRDAXISROTATION")
    {
        ThirdAxisRotation(strValue);
    }

    if(strType == "FRICTION")
    {
        Friction(strValue);
    }

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Joint::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	BodyPart::QueryProperties(aryNames, aryTypes);

	aryNames.Add("EnableLimits");
	aryTypes.Add("Boolean");

	aryNames.Add("Size");
	aryTypes.Add("Float");

	aryNames.Add("PrimaryAxisDisplacement");
	aryTypes.Add("Xml");

	aryNames.Add("SecondaryAxisDisplacement");
	aryTypes.Add("Xml");

	aryNames.Add("ThirdAxisDisplacement");
	aryTypes.Add("Xml");

	aryNames.Add("SecondaryAxisRotation");
	aryTypes.Add("Xml");

	aryNames.Add("ThirdAxisRotation");
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

    if(m_lpPrimaryAxisDisplacement) {delete m_lpPrimaryAxisDisplacement; m_lpPrimaryAxisDisplacement = NULL;} 
    if(m_lpSecondaryAxisDisplacement) {delete m_lpSecondaryAxisDisplacement; m_lpSecondaryAxisDisplacement = NULL;} 
    if(m_lpThirdAxisDisplacement) {delete m_lpThirdAxisDisplacement; m_lpThirdAxisDisplacement = NULL;} 
    if(m_lpSecondaryAxisRotation) {delete m_lpSecondaryAxisRotation; m_lpSecondaryAxisRotation = NULL;} 
    if(m_lpThirdAxisRotation) {delete m_lpThirdAxisRotation; m_lpThirdAxisRotation = NULL;} 
    if(m_lpFriction) {delete m_lpFriction; m_lpFriction = NULL;} 

    if(oXml.FindChildElement("PrimaryAxisDisplacement", false))
        m_lpPrimaryAxisDisplacement = LoadConstraintRelaxation(oXml, "PrimaryAxisDisplacement");

    if(oXml.FindChildElement("SecondaryAxisDisplacement", false))
        m_lpSecondaryAxisDisplacement = LoadConstraintRelaxation(oXml, "SecondaryAxisDisplacement");

    if(oXml.FindChildElement("ThirdAxisDisplacement", false))
        m_lpThirdAxisDisplacement = LoadConstraintRelaxation(oXml, "ThirdAxisDisplacement");

    if(oXml.FindChildElement("SecondaryAxisRotation", false))
        m_lpSecondaryAxisRotation = LoadConstraintRelaxation(oXml, "SecondaryAxisRotation");

    if(oXml.FindChildElement("ThirdAxisRotation", false))
        m_lpThirdAxisRotation = LoadConstraintRelaxation(oXml, "ThirdAxisRotation");

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
ConstraintRelaxation *Joint::LoadConstraintRelaxation(CStdXml &oXml, string strName)
{
	ConstraintRelaxation *lpConstraintRelaxation = NULL;
	string strModule;
	string strType;

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

	lpConstraintRelaxation->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, TRUE);
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
	string strModule;
	string strType;

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

	lpConstraintFriction->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, TRUE);
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
