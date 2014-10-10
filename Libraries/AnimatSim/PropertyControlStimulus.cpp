/**
\file	PropertyControlStimulus.cpp

\brief	Implements the enabler stimulus class. 
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
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "ExternalStimulus.h"
#include "ExternalInputStimulus.h"
#include "PropertyControlStimulus.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/17/2011
**/
PropertyControlStimulus::PropertyControlStimulus()
{
	m_lpEval = NULL;
	m_lpTargetObject = NULL;
	m_fltSetThreshold = 0.5;
	m_fltPreviousSetVal = 0;
	m_fltInitialValue = 0;
	m_fltFinalValue = 1;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/17/2011
**/
PropertyControlStimulus::~PropertyControlStimulus()
{

try
{
	m_ePropertyType = AnimatPropertyType::Invalid;
	m_lpTargetObject = NULL;
	if(m_lpEval) delete m_lpEval;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PropertyControlStimulus\r\n", "", -1, false, true);}
}

std::string PropertyControlStimulus::Type() {return "PropertyControlStimulus";}

/**
\brief	Gets the GUID ID of the target node that will be enabled. 

\author	dcofer
\date	3/17/2011

\return	GUID ID of the node. 
**/
std::string PropertyControlStimulus::TargetID() {return m_strTargetID;}

/**
\brief	Sets the GUID ID of the target node to enable. 

\author	dcofer
\date	3/17/2011

\param	strID	GUID ID. 
**/
void PropertyControlStimulus::TargetID(std::string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);
	m_strTargetID = strID;

	//Reset the property name when switching objects.
	m_ePropertyType = AnimatPropertyType::Invalid;
	m_strPropertyName = "";

	//If we have already been initialized once then we need to re-call this.
	if(m_bInitialized)
		Initialize();
}

/**
\brief	Sets the velocity equation used to control the motor speed over time.

\author	dcofer
\date	4/3/2011

\param	strVal	The post-fix velocity equation string. 
**/
void PropertyControlStimulus::Equation(std::string strVal)
{
	//Initialize the postfix evaluator.
	if(m_lpEval) 
	{delete m_lpEval; m_lpEval = NULL;}

	m_strEquation = strVal;
	m_lpEval = new CStdPostFixEval;

	m_lpEval->AddVariable("t");
	m_lpEval->Equation(m_strEquation);
}


/**
\brief	Sets the name of the property that this adapter will be setting.

\author	dcofer
\date	2/6/2013

\return	nothing.
**/
void PropertyControlStimulus::PropertyName(std::string strPropName)
{
	//Reset the property name so we can get the property type setup correctly.
	//If it is not set then we need to assume that they will set it later.
	//Make sure the property type is set to invalid so the step sim method knows this.
	if(m_lpTargetObject && !Std_IsBlank(strPropName))
	{
		if(Std_Trim(strPropName).length() == 0)
			THROW_PARAM_ERROR(Al_Err_lPropertyNameBlank, Al_Err_strPropertyNameBlank, "Adapter ID", m_strID);

		if(!m_lpTargetObject->HasProperty(strPropName))
			THROW_PARAM_ERROR(Al_Err_lTargetDoesNotHaveProperty, Al_Err_strTargetDoesNotHaveProperty, "Property name", strPropName);

		AnimatPropertyType ePropertyType = m_lpTargetObject->PropertyType(strPropName);
		if(!(ePropertyType != AnimatPropertyType::Boolean || ePropertyType != AnimatPropertyType::Integer || ePropertyType != AnimatPropertyType::Float))
			THROW_PARAM_ERROR(Al_Err_lTargetInvalidPropertyType, Al_Err_strTargetInvalidPropertyType, "Property name", strPropName);

		m_ePropertyType = ePropertyType;
	}
	else
		m_ePropertyType = AnimatPropertyType::Invalid;

	m_strPropertyName = strPropName;
}

/**
\brief	Gets the name of the property that this adapter will be setting.

\author	dcofer
\date	2/6/2013

\return	Name of property that will be set.
**/
std::string PropertyControlStimulus::PropertyName() 
{return m_strPropertyName;}

/**
\brief	Sets the threshold used for setting the value on the target object.

\description If the absolute value of the current value - the last value when set is exceeded then
this will trigger the adapter to set the value again.

\author	dcofer
\date	2/6/2013

\return	Pointer to the target object.
**/
void PropertyControlStimulus::SetThreshold(float fltThreshold)
{
	if(fltThreshold < 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidSetThreshold, Al_Err_strInvalidSetThreshold, "Threshold", fltThreshold);

	m_fltSetThreshold = fltThreshold;
}

/**
\brief	Gets the threshold value used for determining when to set the value on the target object.

\author	dcofer
\date	2/6/2013

\return	threshold value.
**/
float PropertyControlStimulus::SetThreshold()
{return m_fltSetThreshold;}

/**
\brief	Sets the initial value used to set this property when the simulation starts.

\author	dcofer
\date	2/6/2013

\return Nothing.
**/
void PropertyControlStimulus::InitialValue(float fltVal)
{
	m_fltInitialValue = fltVal;
	m_fltPreviousSetVal = fltVal;
}

/**
\brief	Gets vthe initial value used to set this property when the simulation starts.

\author	dcofer
\date	2/6/2013

\return	Initial value.
**/
float PropertyControlStimulus::InitialValue()
{return m_fltInitialValue;}

/**
\brief	Sets the final value used to set this property when the simulation ends.

\author	dcofer
\date	2/6/2013

\return	Nothing.
**/
void PropertyControlStimulus::FinalValue(float fltVal)
{
	m_fltFinalValue = fltVal;
}

/**
\brief	Gets the final value used to set this property when the simulation ends.

\author	dcofer
\date	2/6/2013

\return	final value.
**/
float PropertyControlStimulus::FinalValue()
{return m_fltFinalValue;}

/**
\brief	Gets the target object.

\author	dcofer
\date	2/6/2013

\return	Pointer to the target object.
**/
AnimatBase *PropertyControlStimulus::TargetObject() {return m_lpTargetObject;}

void PropertyControlStimulus::Initialize()
{
	ExternalStimulus::Initialize();

	m_lpTargetObject = m_lpSim->FindByID(m_strTargetID);
	if(!m_lpTargetObject)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetID);

	PropertyName(m_strPropertyName);

	m_fltPreviousSetVal = m_fltInitialValue;
}

void PropertyControlStimulus::Activate()
{
	ExternalStimulus::Activate();

	if(m_bEnabled)
	{
		m_fltPreviousSetVal = m_fltInitialValue;
		if(m_ePropertyType != AnimatPropertyType::Invalid)
			m_lpTargetObject->SetData(m_strPropertyName, STR(m_fltPreviousSetVal));
	}
}

void PropertyControlStimulus::SetPropertyValue(float fltVal)
{
	//If they have not set the property name yet then we cannot set the property value
	if(m_ePropertyType != AnimatPropertyType::Invalid)
	{
		float fltDiff = fltVal - m_fltPreviousSetVal;
		if(fabs(fltDiff) > m_fltSetThreshold)
		{
			m_fltPreviousSetVal = fltVal;

			if(m_ePropertyType == AnimatPropertyType::Boolean)
			{
				if(fltDiff > 0)
					m_lpTargetObject->SetData(m_strPropertyName, "1");
				else
					m_lpTargetObject->SetData(m_strPropertyName, "0");
			}
			else if(m_ePropertyType == AnimatPropertyType::Integer)
			{
				int iVal = (int) fltVal;
				m_lpTargetObject->SetData(m_strPropertyName, STR(iVal));
			}
			else
				m_lpTargetObject->SetData(m_strPropertyName, STR(fltVal));
		}
	}
}

void PropertyControlStimulus::StepSimulation()
{

	try
	{
		if(m_bEnabled)
		{
			//IMPORTANT! This stimulus applies a stimulus to the physics engine, so it should ONLY be called once for every time the physcis
			//engine steps. If you do not do this then the you will accumulate forces being applied during the neural steps, and the total
			//for you apply will be greater than what it should be. To get around this we will only call the code in step simulation if
			//the physics step count is equal to the step interval.
			if(m_lpSim->PhysicsStepCount() == m_lpSim->PhysicsStepInterval())
			{
				m_lpEval->SetVariable("t", (m_lpSim->Time()-m_fltStartTime) );

				float fltInput = m_lpEval->Solve();

				SetPropertyValue(fltInput);
			}
		}
	}
	catch(...)
	{
		LOG_ERROR("Error Occurred while setting Joint Velocity");
	}
}

void PropertyControlStimulus::Deactivate()
{
	ExternalStimulus::Deactivate();

	if(m_bEnabled)
	{
		m_fltPreviousSetVal = m_fltInitialValue;
		if(m_ePropertyType != AnimatPropertyType::Invalid)
			m_lpTargetObject->SetData(m_strPropertyName, STR(m_fltFinalValue));
	}
}

bool PropertyControlStimulus::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(ExternalStimulus::SetData(strDataType, strValue, false))
		return true;
	
	if(strType == "TARGETID")
	{
		TargetID(strValue);
		return true;
	}

	if(strType == "PROPERTYNAME")
	{
		PropertyName(strValue);
		return true;
	}

	if(strType == "SETTHRESHOLD")
	{
		SetThreshold(atof(strValue.c_str()));
		return true;
	}

	if(strType == "INITIALVALUE")
	{
		InitialValue(atof(strValue.c_str()));
		return true;
	}

	if(strType == "FINALVALUE")
	{
		FinalValue(atof(strValue.c_str()));
		return true;
	}

	if(strType == "EQUATION")
	{
		Equation(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void PropertyControlStimulus::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	ExternalStimulus::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("TargetID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PropertyName", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SetThreshold", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("InitialValue", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FinalValue", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Equation", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

void PropertyControlStimulus::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	TargetID(oXml.GetChildString("TargetID"));
	PropertyName(oXml.GetChildString("PropertyName", m_strPropertyName));
	SetThreshold(oXml.GetChildFloat("SetThreshold", m_fltSetThreshold));
	InitialValue(oXml.GetChildFloat("InitialValue", m_fltInitialValue));
	FinalValue(oXml.GetChildFloat("FinalValue", m_fltFinalValue));
	Equation(oXml.GetChildString("Equation", m_strEquation));

	oXml.OutOfElem(); //OutOf Simulus Element
}

	}			//ExternalStimuli
}				//VortexAnimatSim




