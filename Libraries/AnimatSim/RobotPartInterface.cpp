#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
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
#include "Gain.h"

#include "RobotInterface.h"
#include "RobotIOControl.h"
#include "RobotPartInterface.h"


namespace AnimatSim
{
	namespace Robotics
	{

RobotPartInterface::RobotPartInterface(void)
{
	m_lpParentInterface = NULL;
	m_lpParentIOControl = NULL;
	m_lpPart = NULL;
	m_lpProperty = NULL;
	m_iIOComponentID = 0;
	m_fltIOValue = 0;
	m_iIOValue = 0;
	m_lpGain = NULL;
	m_bChanged= false;
}

RobotPartInterface::~RobotPartInterface(void)
{

try
{
	//We do not own any of these.
	m_lpParentInterface = NULL;
	m_lpParentIOControl = NULL;
	m_lpPart = NULL;
	m_lpProperty = NULL;
	if(m_lpGain) delete m_lpGain;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RobotPartInterface\r\n", "", -1, false, true);}
}

void RobotPartInterface::ParentIOControl(RobotIOControl *lpParent) 
{
	m_lpParentIOControl = lpParent;

	if(m_lpParentIOControl)
		m_lpParentInterface = m_lpParentIOControl->ParentInterface();
}

RobotIOControl *RobotPartInterface::ParentIOControl() {return m_lpParentIOControl;}

int RobotPartInterface::IOComponentID() {return m_iIOComponentID;}

void RobotPartInterface::IOComponentID(int iID) {m_iIOComponentID = iID;}

float RobotPartInterface::IOValue() {return m_fltIOValue;}

void RobotPartInterface::IOValue(float fltVal) {m_fltIOValue = fltVal;}
						
int RobotPartInterface::IOValueInt() {return m_iIOValue;}

void RobotPartInterface::IOValueInt(int iVal) {m_iIOValue = iVal;}
			
bool RobotPartInterface::Changed() {return m_bChanged;}

void RobotPartInterface::Changed(bool bVal) {m_bChanged = bVal;}

void RobotPartInterface::LinkedPartID(std::string strID)
{
	m_strPartID = strID;
	m_strPropertyName = "";
	Initialize();
}

std::string RobotPartInterface::LinkedPartID() {return m_strPartID;}

void RobotPartInterface::PropertyName(std::string strName)
{
	m_strPropertyName = strName;
	Initialize();
}

std::string RobotPartInterface::PropertyName() {return m_strPropertyName;}

/**
\brief	Gets the poitner to the gain function.

\author	dcofer
\date	3/18/2011

\return	Pointer to the gain.
**/
Gain *RobotPartInterface::GetGain() {return m_lpGain;}

void RobotPartInterface::SetGain(Gain *lpGain)
{
	if(m_lpGain)
	{
		delete m_lpGain;
		m_lpGain = NULL;
	}

	m_lpGain = lpGain;
	m_lpGain->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, true);
}

/**
\brief	Creates and adds a gain object. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the gain. 
**/
void RobotPartInterface::AddGain(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");

	SetGain(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
}

#pragma region DataAccesMethods

float *RobotPartInterface::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "IOVALUE")
		return &m_fltIOValue;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RobotPartInterface::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(strType == "IOCOMPONENTID")
	{
		IOComponentID((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "LINKEDPARTID")
	{
		LinkedPartID(strValue);
		return true;
	}

	if(strType == "PROPERTYNAME")
	{
		PropertyName(strValue);
		return true;
	}
	
	if(strType == "GAIN")
	{
		AddGain(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RobotPartInterface::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("IOValue", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("IOComponentID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PropertyName", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LinkedPartID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

#pragma endregion

/**
\brief	This method is called after all connections to whatever control board have been made. It is meant
to be used to setup connection information specific to this part. For example, We connect to a Firmata
microcontroller like an Arduino, and then do a setup that could take some time. We should not attempt to
setup any of the pins until after the board itself has been setup. After that we need to loop through and setup
all the parts. That is what this method is for.

\author	dcofer
\date	5/1/2014

**/
void RobotPartInterface::SetupIO()
{
}

/**
\brief	This method is used to send/recieve the actual IO. This will often be in a seperate thread than
the StepSimulation. StepSimulation gets/sets the values in the sim and gets it read for the thread that
does the IO. Once that thread is ready to send/receive it uses that value to perform the operation.

\author	dcofer
\date	5/2/2014

**/
void RobotPartInterface::StepIO()
{
}

void RobotPartInterface::Initialize()
{
	//We need to find the referenced body part and set its robot part interface to this one.
	if(!Std_IsBlank(m_strPartID))
		m_lpPart = dynamic_cast<AnimatBase *>(m_lpSim->FindByID(m_strPartID));

	if(m_lpPart)
	{
		//m_lpBodyPart->AddRobotPartInterface(this);

		if(!Std_IsBlank(m_strPropertyName))
			m_lpProperty = m_lpPart->GetDataPointer(m_strPropertyName);
	}
	else
		m_lpProperty = NULL;
}

void RobotPartInterface::ResetSimulation()
{
	AnimatBase::ResetSimulation();

	m_fltIOValue = 0;
	m_iIOValue = 0;
	m_bChanged= false;
}

void RobotPartInterface::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	m_strPartID = oXml.GetChildString("LinkedPartID", "");
	m_strPropertyName = oXml.GetChildString("PropertyName", "");
	m_iIOComponentID = oXml.GetChildInt("IOComponentID", m_iIOComponentID);
	
	SetGain(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

	}
}