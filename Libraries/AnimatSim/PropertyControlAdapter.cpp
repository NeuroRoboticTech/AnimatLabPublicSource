/**
\file	PropertyControlAdapter.cpp

\brief	Implements the property control adapter class.
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
#include "Gain.h"
#include "Adapter.h"
#include "PropertyControlAdapter.h"
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
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"


namespace AnimatSim
{
	namespace Adapters
	{
/**
\brief	Default constructor.

\author	dcofer
\date	3/18/2011
**/
PropertyControlAdapter::PropertyControlAdapter()
{
	m_lpTargetObject = NULL;
	m_fltSetThreshold = 0.5;
	m_fltPreviousSetVal = 0;
	m_fltInitialValue = 0;
	m_fltFinalValue = 1;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
PropertyControlAdapter::~PropertyControlAdapter()
{

try
{
	m_ePropertyType = AnimatBase::AnimatPropertyType::Invalid;
	m_lpTargetObject = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PropertyControlAdapter\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Sets the name of the property that this adapter will be setting.

\author	dcofer
\date	2/6/2013

\return	nothing.
**/
void PropertyControlAdapter::PropertyName(string strPropName)
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

		AnimatBase::AnimatPropertyType ePropertyType = m_lpTargetObject->PropertyType(strPropName);
		if(!(ePropertyType != AnimatBase::AnimatPropertyType::Boolean || ePropertyType != AnimatBase::AnimatPropertyType::Integer || ePropertyType != AnimatBase::AnimatPropertyType::Float))
			THROW_PARAM_ERROR(Al_Err_lTargetInvalidPropertyType, Al_Err_strTargetInvalidPropertyType, "Property name", strPropName);

		m_ePropertyType = ePropertyType;
	}
	else
		m_ePropertyType = AnimatBase::AnimatPropertyType::Invalid;

	m_strPropertyName = strPropName;
}

/**
\brief	Gets the name of the property that this adapter will be setting.

\author	dcofer
\date	2/6/2013

\return	Name of property that will be set.
**/
string PropertyControlAdapter::PropertyName() 
{return m_strPropertyName;}

/**
\brief	Sets the threshold used for setting the value on the target object.

\description If the absolute value of the current value - the last value when set is exceeded then
this will trigger the adapter to set the value again.

\author	dcofer
\date	2/6/2013

\return	Pointer to the target object.
**/
void PropertyControlAdapter::SetThreshold(float fltThreshold)
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
float PropertyControlAdapter::SetThreshold()
{return m_fltSetThreshold;}

/**
\brief	Sets the initial value used to set this property when the simulation starts.

\author	dcofer
\date	2/6/2013

\return Nothing.
**/
void PropertyControlAdapter::InitialValue(float fltVal)
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
float PropertyControlAdapter::InitialValue()
{return m_fltInitialValue;}

/**
\brief	Sets the final value used to set this property when the simulation ends.

\author	dcofer
\date	2/6/2013

\return	Nothing.
**/
void PropertyControlAdapter::FinalValue(float fltVal)
{
	m_fltFinalValue = fltVal;
}

/**
\brief	Gets the final value used to set this property when the simulation ends.

\author	dcofer
\date	2/6/2013

\return	final value.
**/
float PropertyControlAdapter::FinalValue()
{return m_fltFinalValue;}

/**
\brief	Gets the target object.

\author	dcofer
\date	2/6/2013

\return	Pointer to the target object.
**/
AnimatBase *PropertyControlAdapter::TargetObject() {return m_lpTargetObject;}

BOOL PropertyControlAdapter::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Adapter::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "PROPERTYNAME")
	{
		PropertyName(strValue);
		return TRUE;
	}

	if(strType == "SETTHRESHOLD")
	{
		SetThreshold(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "INITIALVALUE")
	{
		InitialValue(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FINALVALUE")
	{
		FinalValue(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Data Type", strDataType);

	return FALSE;
}

void PropertyControlAdapter::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Adapter::QueryProperties(aryNames, aryTypes);

	aryNames.Add("PropertyName");
	aryTypes.Add("String");

	aryNames.Add("SetThreshold");
	aryTypes.Add("Float");

	aryNames.Add("InitialValue");
	aryTypes.Add("Float");

	aryNames.Add("FinalValue");
	aryTypes.Add("Float");
}

void PropertyControlAdapter::ResetSimulation()
{
	Adapter::ResetSimulation();

	m_fltPreviousSetVal = m_fltInitialValue;
	if(m_ePropertyType != AnimatBase::AnimatPropertyType::Invalid)
		m_lpTargetObject->SetData(m_strPropertyName, STR(m_fltFinalValue));
}

void PropertyControlAdapter::SimStarting() 
{
	m_fltPreviousSetVal = m_fltInitialValue;
	if(m_ePropertyType != AnimatBase::AnimatPropertyType::Invalid)
		m_lpTargetObject->SetData(m_strPropertyName, STR(m_fltPreviousSetVal));
}

void PropertyControlAdapter::Initialize()
{
	//Not calling Adapter initialize because we are doing a slightly different implementation
	Node::Initialize();

	m_lpSourceNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strSourceID));
	if(!m_lpSourceNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strSourceID);

	m_lpSourceData = m_lpSourceNode->GetDataPointer(m_strSourceDataType);

	if(!m_lpSourceData)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Adapter: " + m_strID + " StructureID: " + m_lpStructure->ID() + "SourceID: " + m_strSourceID + " DataType: " + m_strSourceDataType));

	m_lpTargetNode = NULL; //This is always NULL for this object
	m_lpTargetObject = m_lpSim->FindByID(m_strTargetID);
	if(!m_lpTargetObject)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetID);

	PropertyName(m_strPropertyName);

	m_fltPreviousSetVal = m_fltInitialValue;

	m_lpSim->AttachSourceAdapter(m_lpStructure, this);
	m_lpSim->AttachTargetAdapter(m_lpStructure, this);
}

void PropertyControlAdapter::SetPropertyValue(float fltVal)
{
	//If they have not set the property name yet then we cannot set the property value
	if(m_ePropertyType != AnimatBase::AnimatPropertyType::Invalid)
	{
		float fltDiff = fltVal - m_fltPreviousSetVal;
		if(fabs(fltDiff) > m_fltSetThreshold)
		{
			m_fltPreviousSetVal = fltVal;

			if(m_ePropertyType == AnimatBase::AnimatPropertyType::Boolean)
			{
				if(fltDiff > 0)
					m_lpTargetObject->SetData(m_strPropertyName, "1");
				else
					m_lpTargetObject->SetData(m_strPropertyName, "0");
			}
			else if(m_ePropertyType == AnimatBase::AnimatPropertyType::Integer)
			{
				int iVal = (int) fltVal;
				m_lpTargetObject->SetData(m_strPropertyName, STR(iVal));
			}
			else
				m_lpTargetObject->SetData(m_strPropertyName, STR(fltVal));
		}
	}
}

void PropertyControlAdapter::StepSimulation()
{
	if(m_bEnabled)
	{
		float fltInput = m_lpGain->CalculateGain(*m_lpSourceData);
		SetPropertyValue(fltInput);
	}
}

void PropertyControlAdapter::SetDestinationID(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Adapter");

	oXml.IntoElem();  //Into Adapter Element

	//Load Source Data
	TargetModule(oXml.GetChildString("TargetModule"));
	TargetID(oXml.GetChildString("TargetID"));
	PropertyName(oXml.GetChildString("PropertyName", m_strPropertyName));
	SetThreshold(oXml.GetChildFloat("SetThreshold", m_fltSetThreshold));
	InitialValue(oXml.GetChildFloat("InitialValue", m_fltInitialValue));
	FinalValue(oXml.GetChildFloat("FinalValue", m_fltFinalValue));

	oXml.OutOfElem(); //OutOf Adapter Element

	//Remove the adatper settings.
	m_lpSim->RemoveSourceAdapter(m_lpStructure, this);
	m_lpSim->RemoveTargetAdapter(m_lpStructure, this);

	Initialize();
}

void PropertyControlAdapter::Load(CStdXml &oXml)
{
	Adapter::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	//Load Target Data
	PropertyName(oXml.GetChildString("PropertyName", m_strPropertyName));
	SetThreshold(oXml.GetChildFloat("SetThreshold", m_fltSetThreshold));
	InitialValue(oXml.GetChildFloat("InitialValue", m_fltInitialValue));
	FinalValue(oXml.GetChildFloat("FinalValue", m_fltFinalValue));

	oXml.OutOfElem(); //OutOf Adapter Element
}


	}			//Adapters
}			//AnimatSim
