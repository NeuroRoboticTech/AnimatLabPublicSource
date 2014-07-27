#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
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

RemoteControlLinkage::RemoteControlLinkage(void)
{
	m_lpParentRemoteControl = NULL;
	m_fltSourceData = NULL;
	m_lpGain = NULL;
}

RemoteControlLinkage::~RemoteControlLinkage(void)
{
try
{
	m_fltSourceData = NULL;

	if(m_lpGain)
	{
		delete m_lpGain;
		m_lpGain = NULL;
	}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RemoteControlLinkage\r\n", "", -1, false, true);}
}

void RemoteControlLinkage::ParentRemoteControl(RemoteControl *lpParent) {m_lpParentRemoteControl = lpParent;}

RemoteControl *RemoteControlLinkage::ParentRemoteControl() {return m_lpParentRemoteControl;}


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
std::string RemoteControlLinkage::LinkedNodeID() {return m_strLinkedNodeID;}

/**
\brief	Sets the target data type.

\author	dcofer
\date	3/18/2011

\param	strType	Target DataType. 
**/
void RemoteControlLinkage::LinkedNodeID(std::string strID)
{
	m_strLinkedNodeID = "";
	m_strTargetDataTypeID = "";
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

/**
\brief	Gets the poitner to the gain function.

\author	dcofer
\date	3/18/2011

\return	Pointer to the gain.
**/
Gain *RemoteControlLinkage::GetGain() {return m_lpGain;}

void RemoteControlLinkage::SetGain(Gain *lpGain)
{
	if(m_lpGain)
	{
		delete m_lpGain;
		m_lpGain = NULL;
	}

	m_lpGain = lpGain;
	m_lpGain->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL, true);
}

/**
\brief	Creates and adds a gain object. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the gain. 
**/
void RemoteControlLinkage::AddGain(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");

	SetGain(LoadGain(m_lpSim, "Gain", oXml));
}

#pragma region DataAccesMethods

float *RemoteControlLinkage::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "STEPIODURATION")
	//	return &m_fltStepIODuration;
	//else
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
	else if(strType == "LINKEDNODEID")
	{
		LinkedNodeID(strValue);
		return true;
	}
	else if(strType == "GAIN")
	{
		AddGain(strValue);
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
	aryProperties.Add(new TypeProperty("LinkedNodeID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
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
}

void RemoteControlLinkage::ResetSimulation()
{
}

void RemoteControlLinkage::AfterResetSimulation()
{
}

void RemoteControlLinkage::StepSimulation()
{
    AnimatBase::StepSimulation();

}

void RemoteControlLinkage::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Link Element

	LinkedNodeID(oXml.GetChildString("LinkedNodeID", ""));
	SourceDataTypeID(oXml.GetChildString("SourceDataTypeID", ""));
	TargetDataTypeID(oXml.GetChildString("TargetDataTypeID", ""));

	if(oXml.FindChildElement("Gain", false))
		SetGain(LoadGain(m_lpSim, "Gain", oXml));

	oXml.OutOfElem(); //OutOf Link Element
}



	}
}
