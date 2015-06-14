#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "Link.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "Gain.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
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

#include "RobotInterface.h"
#include "RobotIOControl.h"
#include "RemoteControlLinkage.h"
#include "RemoteControl.h"

namespace AnimatSim
{
	namespace Robotics
	{


void RemoteControlData::ClearStartStops()
{
	if((fabs(m_fltStart) > 0 || fabs(m_fltStop) > 0))
	{
		if(m_iSimStepped >= m_iChangeSimStepCount)
		{
			////Test Code
			//if(m_fltStart > 0)
			//	OutputDebugString("Cleared Start\r\n");
			//if(m_fltStop > 0)
			//	OutputDebugString("Cleared Stop\r\n");

			m_fltStart = 0;
			m_fltStop = 0;
			m_iSimStepped = 0;
		}
		else
			m_iSimStepped++;
	}
}

void RemoteControlData::CheckStartedStopped()
{
	if(m_fltValue == m_fltPrev)
		m_iCount++;
	else
		m_iCount = 0;

	if(m_iCount == 3)
	{
		if(!m_bStarted && m_fltValue != 0)
		{
			m_iStartDir = Std_Sign(m_fltValue);
			m_fltStart = 1*m_iStartDir;
			m_bStarted = true;
			////Test Code
			//OutputDebugString("Start\r\n");
		}
		else if(m_bStarted && m_fltValue == 0)
		{
			m_fltStop = m_iStartDir;
			m_bStarted = false;
			////Test Code
			//OutputDebugString("Stop\r\n");
		}

		m_iCount = 0;
	}

	m_fltPrev = m_fltValue;

	////Test Code
	//if(m_fltValue > 0)
	//{
	//	std::string strVal = "Val: " + STR(m_fltValue) + " Prev: " + STR(m_fltPrev) + " Count: " + STR(m_iCount) + " Started: " + STR(m_bStarted) + " Start: " + STR((int) m_fltStart) + " Stop: " + STR((int) m_fltStop) + "\r\n";
	//	OutputDebugString(strVal.c_str());
	//}
}

RemoteControlLinkage::RemoteControlLinkage(void)
{
	m_lpParentRemoteControl = NULL;
	m_lpSource = NULL;
	m_lpTarget = NULL;
	m_lpSourceData = NULL;
	m_lpTargetData = NULL;
	m_iTargetDataType = -1;
	m_fltAppliedValue = 0;
	m_bInLink = true;
	m_iPropertyID = -1;
}

RemoteControlLinkage::~RemoteControlLinkage(void)
{
try
{
	m_lpSource = NULL;
	m_lpTarget = NULL;
	m_lpSourceData = NULL;
	m_lpTargetData = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RemoteControlLinkage\r\n", "", -1, false, true);}
}

void RemoteControlLinkage::ParentRemoteControl(RemoteControl *lpParent) {m_lpParentRemoteControl = lpParent;}

RemoteControl *RemoteControlLinkage::ParentRemoteControl() {return m_lpParentRemoteControl;}


/**
\brief	Gets the source ID.

\author	dcofer
\date	3/18/2011

\return	Source ID.
**/
std::string RemoteControlLinkage::SourceID() {return m_strSourceID;}

/**
\brief	Sets the source ID.

\author	dcofer
\date	3/18/2011

\param	strType	Target DataType. 
**/
void RemoteControlLinkage::SourceID(std::string strID)
{
	m_strSourceID = strID;
	Initialize();
}

/**
\brief	Gets the target ID.

\author	dcofer
\date	3/18/2011

\return	Target ID.
**/
std::string RemoteControlLinkage::TargetID() {return m_strTargetID;}

/**
\brief	Sets the target ID.

\author	dcofer
\date	3/18/2011

\param	strType	Target ID. 
**/
void RemoteControlLinkage::TargetID(std::string strID)
{
	m_strTargetID = strID;
	Initialize();
}

/**
\brief	Gets the target data type.

\author	dcofer
\date	3/18/2011

\return	Target data type.
**/
std::string RemoteControlLinkage::SourceDataTypeID() {return m_strSourceDataTypeID;}

/**
\brief	Sets the target data type.

\author	dcofer
\date	3/18/2011

\param	strType	Target DataType. 
**/
void RemoteControlLinkage::SourceDataTypeID(std::string strTypeID)
{
	m_strSourceDataTypeID = strTypeID;
	Initialize();
}

/**
\brief	Gets the target data type.

\author	dcofer
\date	3/18/2011

\return	Target data type.
**/
std::string RemoteControlLinkage::TargetDataTypeID() {return m_strTargetDataTypeID;}

/**
\brief	Sets the target data type.

\author	dcofer
\date	3/18/2011

\param	strType	Target DataType. 
**/
void RemoteControlLinkage::TargetDataTypeID(std::string strTypeID)
{
	m_strTargetDataTypeID = strTypeID;
	Initialize();
}

int RemoteControlLinkage::PropertyID() {return m_iPropertyID;}

void RemoteControlLinkage::PropertyID(int iID, bool bCreateDataTypes) 
{
	m_iPropertyID = iID;
	m_Data.m_iButtonID = iID;
	if(m_lpParentRemoteControl && bCreateDataTypes)
		m_lpParentRemoteControl->CreateDataTypes();
}

/**
\brief	Gets the property name.

\author	dcofer
\date	3/18/2011

\return	property name.
**/
std::string RemoteControlLinkage::PropertyName() {return m_strPropertyName;}

/**
\brief	Sets the property name.

\author	dcofer
\date	3/18/2011

\param	strType	property name. 
**/
void RemoteControlLinkage::PropertyName(std::string strTypeID)
{
	m_strPropertyName = strTypeID;
	m_Data.m_strProperty = strTypeID;
	if(m_lpParentRemoteControl)
		m_lpParentRemoteControl->CreateDataTypes();
}

/**
\brief	Gets the inlink value.

\author	dcofer
\date	3/18/2011

\return	inlink.
**/
bool RemoteControlLinkage::InLink() {return m_bInLink;}

/**
\brief	Sets the inlink property.

\author	dcofer
\date	3/18/2011

\param	strType	inlink value. 
**/
void RemoteControlLinkage::InLink(bool bVal)
{
	m_bInLink = bVal;
}

float RemoteControlLinkage::AppliedValue() {return m_fltAppliedValue;}

void RemoteControlLinkage::AppliedValue(float fltVal) {m_fltAppliedValue = fltVal;}

#pragma region DataAccesMethods

float *RemoteControlLinkage::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "APPLIEDCURRENT" || strType == "APPLIEDVALUE")
		return &m_fltAppliedValue;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RemoteControlLinkage::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}
	else if(strType == "SOURCEDATATYPEID")
	{
		SourceDataTypeID(strValue);
		return true;
	}
	else if(strType == "TARGETDATATYPEID")
	{
		TargetDataTypeID(strValue);
		return true;
	}
	else if(strType == "SOURCEID")
	{
		SourceID(strValue);
		return true;
	}
	else if(strType == "TARGETID")
	{
		TargetID(strValue);
		return true;
	}
	else if(strType == "PROPERTYNAME")
	{
		PropertyName(strValue);
		return true;
	}
	else if(strType == "PROPERTYID")
	{
		PropertyID(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RemoteControlLinkage::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Enabled", AnimatPropertyType::Boolean, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("SourceDataTypeID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TargetDataTypeID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SourceID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TargetID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PropertyName", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PropertyID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AppliedValue", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
}

#pragma endregion


/**
\brief	This method is called after all connections to whatever control board have been made. It calls
each parts SetupIO method. For example, We connect to a Firmata
microcontroller like an Arduino, and then do a setup that could take some time. We should not attempt to
setup any of the pins until after the board itself has been setup. After that we need to loop through and setup
all the parts.

\author	dcofer
\date	5/1/2014

**/
void RemoteControlLinkage::SetupIO()
{
}

/**
\brief	This method is called from within the IO thread. It calls StepIO for each part.

\author	dcofer
\date	5/2/2014

**/
void RemoteControlLinkage::StepIO()
{
}

/**
\brief	This method is called just before the IO thread is closed down. It gives the IO objects a chance to do
any required cleanup.

\author	dcofer
\date	5/12/2014

**/
void RemoteControlLinkage::ShutdownIO()
{
}

void RemoteControlLinkage::Initialize()
{
	if(!Std_IsBlank(m_strSourceID))
	{
		m_lpSource = dynamic_cast<AnimatBase *>(m_lpSim->FindByID(m_strSourceID));
		if(!m_lpSource)
			THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strSourceID);

		if(m_lpSource && !Std_IsBlank(m_strSourceDataTypeID) && m_lpParentRemoteControl && 
			(!m_bInLink || (m_bInLink && m_lpParentRemoteControl->FindLinkageWithPropertyName(m_strSourceDataTypeID, false))))
			m_lpSourceData = m_lpSource->GetDataPointer(m_strSourceDataTypeID);
		else
			m_lpSourceData = NULL;	
	}
	else
	{
		m_lpSource = NULL;
		m_lpSourceData = NULL;
	}

	if(!Std_IsBlank(m_strTargetID))
	{
		m_lpTarget = dynamic_cast<AnimatBase *>(m_lpSim->FindByID(m_strTargetID));
		if(!m_lpTarget)
			THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetID);

		if(m_lpTarget && !Std_IsBlank(m_strTargetDataTypeID) && 
			(m_bInLink || (!m_bInLink && m_lpParentRemoteControl->FindLinkageWithPropertyName(m_strTargetDataTypeID, false))))
			m_lpTargetData = m_lpTarget->GetDataPointer(m_strTargetDataTypeID);
		else
			m_lpTargetData = NULL;	
	}
	else
	{
		m_lpTarget = NULL;
		m_lpTargetData = NULL;
	}
}

void RemoteControlLinkage::ApplyValue(float fltData)
{
	if(m_bEnabled && m_lpSourceData && m_lpTarget && m_lpTargetData)
	{	
		////Test Code
		//int i=5; //Std_ToLower(m_strID) == "079087db-7a2b-4e2b-82ab-cdd407ad3d85")   
		//if(GetSimulator()->Time() >= 0.2 && fabs(fltData) > 0)
		//	i=6;

		//std::string strVal = "WalkV: " + STR((float) *m_lpSourceData) + "\r\n";
		//OutputDebugString(strVal.c_str());

		//Remove any previously applied current from this linkage.
		*m_lpTargetData = *m_lpTargetData - m_fltAppliedValue;

		//Calculate the new current to apply.
		m_fltAppliedValue = CalculateAppliedValue(fltData);

		//Add the new applied current
		*m_lpTargetData = *m_lpTargetData + m_fltAppliedValue;
	}
}

void RemoteControlLinkage::ResetSimulation()
{
	m_fltAppliedValue = 0;
}

void RemoteControlLinkage::StepSimulation()
{
	if(m_bEnabled && m_lpSourceData)
	{
		if(m_bInLink)
			ApplyValue(*m_lpSourceData);
		else
			m_Data.m_fltValue = *m_lpSourceData;
	}
}

void RemoteControlLinkage::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Link Element

	InLink(oXml.GetChildBool("InLink", true));
	SourceID(oXml.GetChildString("SourceID", ""));
	TargetID(oXml.GetChildString("TargetID", ""));
	SourceDataTypeID(oXml.GetChildString("SourceDataTypeID", ""));
	TargetDataTypeID(oXml.GetChildString("TargetDataTypeID", ""));
	PropertyName(oXml.GetChildString("PropertyName", ""));
	PropertyID(oXml.GetChildInt("PropertyID", -1));

	oXml.OutOfElem(); //OutOf Link Element
}



	}
}
