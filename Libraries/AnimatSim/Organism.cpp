/**
\file	Organism.cpp

\brief	Implements the organism class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
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
#include "Sensor.h"
#include "Attachment.h"
#include "Structure.h"
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

/**
\brief	Default constructor. 

\author	dcofer
\date	2/25/2011
**/
Organism::Organism()
{
	m_bDead = false;
	m_lpNervousSystem = NULL;
    m_lpRobot = NULL;
}

/**
\brief	Destructor. 

\author	dcofer
\date	2/25/2011
**/
Organism::~Organism()
{

try
{
	if(m_lpNervousSystem) 
	{
		delete m_lpNervousSystem; 
		m_lpNervousSystem = NULL;
	}

    if(m_lpRobot)
    {
        delete m_lpRobot;
        m_lpRobot = NULL;
    }
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Organism\r\n", "", -1, false, true);}
}

/**
\brief	Query if this object is dead. 

\author	dcofer
\date	2/25/2011

\return	true if dead, false if not. 
**/
bool Organism::IsDead() 
{return m_bDead;}

void Organism::Kill(bool bState)
{
	Structure::Kill(bState);

	m_bDead = bState;
	
	if(m_lpBody)
		m_lpBody->Kill(bState);
	
	if(m_lpNervousSystem)
		m_lpNervousSystem->Kill(bState);
}

void Organism::Initialize()
{
	Structure::Initialize();

    if(m_lpNervousSystem)
    	m_lpNervousSystem->Initialize();

    if(m_lpRobot)
        m_lpRobot->Initialize();
}

void Organism::ResetSimulation()
{
	Structure::ResetSimulation();

	//Make sure to reset us from being killed if it happend during the sim.
	Kill(false);

    if(m_lpNervousSystem)   
    	m_lpNervousSystem->ResetSimulation();

    if(m_lpRobot)
        m_lpRobot->ResetSimulation();
}

/**
\fn	void Organism::StepNeuralEngine()

\brief	Step neural engine. 

\author	dcofer
\date	3/2/2011
**/
void Organism::StepNeuralEngine()
{
	if(!m_bDead)
	{
		m_lpNervousSystem->StepSimulation();

		if(m_lpScript)
			m_lpScript->StepPhysicsEngine();
	}
}

void Organism::StepPhysicsEngine()
{
    Structure::StepPhysicsEngine();

    if(m_lpRobot && m_lpRobot->Enabled())
        m_lpRobot->StepSimulation();
}

void Organism::MinTimeStep(float &fltMin) 
{
	m_lpNervousSystem->MinTimeStep(fltMin);
}

#pragma region DataAccesMethods

bool Organism::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Structure::SetData(strDataType, strValue, false))
		return true;

	//if(strType == "TIMESTEP")
	//{
	//	TimeStep((float) atof(strValue.c_str()));
	//	return true;
	//}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}


/**
\brief	Creates and adds a robot interface control. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the control node. 
**/
RobotInterface *Organism::AddRobotInterface(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("RobotInterface");

	RobotInterface *lpInterface = LoadRobotInterface(oXml);

	lpInterface->Initialize();

    return lpInterface;
}

/**
\brief	Removes the robot interface with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the body to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void Organism::RemoveRobotInterface(std::string strID, bool bThrowError)
{
	if(!m_lpRobot || Std_CheckString(m_lpRobot->ID()) != Std_CheckString(strID))
		THROW_PARAM_ERROR(Al_Err_lRobotInterfaceIDNotFound, Al_Err_strRobotInterfaceIDNotFound, "ID", strID);

	delete m_lpRobot;
	m_lpRobot = NULL;
}

bool Organism::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(Structure::AddItem(strItemType, strXml, false))
		return true;

	if(strType == "NEURALMODULE")
	{
		try
		{
			m_lpNervousSystem->AddNeuralModule(strXml);
			return true;
		}
		catch(CStdErrorInfo oError)
		{
			if(bThrowError)
				RELAY_ERROR(oError);
		}
	}

	if(strType == "ROBOTINTERFACE")
	{
		AddRobotInterface(strXml);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool Organism::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);
	
	if(Structure::RemoveItem(strItemType, strID, false))
		return true;

	if(strType == "NEURALMODULE")
	{
		try
		{
			m_lpNervousSystem->RemoveNeuralModule(strID);
			return true;
		}
		catch(CStdErrorInfo oError)
		{
			if(bThrowError)
				RELAY_ERROR(oError);
		}
	}

	if(strType == "ROBOTINTERFACE")
	{
		RemoveRobotInterface(strID, bThrowError);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

#pragma endregion

/**
\fn	AnimatSim::Behavior::NervousSystem *Organism::NervousSystem()

\brief	returns a pointer to the nervous system

\author	dcofer
\date	3/2/2011

\return	Pointer to the nervous system. 
**/
AnimatSim::Behavior::NervousSystem *Organism::GetNervousSystem()
{return m_lpNervousSystem;}

long Organism::CalculateSnapshotByteSize()
{return m_lpNervousSystem->CalculateSnapshotByteSize();}

void Organism::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{m_lpNervousSystem->SaveKeyFrameSnapshot(aryBytes, lIndex);}

void Organism::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{m_lpNervousSystem->LoadKeyFrameSnapshot(aryBytes, lIndex);}

/**
\brief	Loads a robot interface Control. 

\author	dcofer
\date	3/2/2011

\param [in,out]	oXml	The xml data definition of the part to load. 

\return	null if it fails, else the IO control. 
**/

RobotInterface *Organism::LoadRobotInterface(CStdXml &oXml)
{
	RobotInterface *lpInterface = NULL;
	std::string strType;

try
{
    if(oXml.FindChildElement("RobotInterface", false))
    {
		if(m_lpRobot)
		{
			delete m_lpRobot;
			m_lpRobot = NULL;
		}

	    oXml.IntoChildElement("RobotInterface");
	    std::string strModuleName = oXml.GetChildString("ModuleName", "");
	    std::string strType = oXml.GetChildString("Type");
	    oXml.OutOfElem(); //OutOf RobotInterface Element

	    lpInterface = dynamic_cast<RobotInterface *>(m_lpSim->CreateObject(strModuleName, "RobotInterface", strType));
	    if(!lpInterface)
		    THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RobotInterface");
        lpInterface->SetSystemPointers(m_lpSim, this, m_lpModule, NULL, true);
        lpInterface->Load(oXml);
		m_lpRobot = lpInterface;
    }

	return m_lpRobot;
}
catch(CStdErrorInfo oError)
{
	if(m_lpRobot) delete m_lpRobot;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(m_lpRobot) delete m_lpRobot;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

void Organism::Load(CStdXml &oXml)
{
	Structure::Load(oXml);

	oXml.IntoElem();  //Into Structure Element

	//dwc convert. Need to have a method to remove a nervous system. It needs to remove any added
	//modules from the list in the simulator.
	if(m_lpNervousSystem) {delete m_lpNervousSystem; m_lpNervousSystem = NULL;}
	m_lpNervousSystem = new AnimatSim::Behavior::NervousSystem;

	oXml.IntoChildElement("NervousSystem");

	m_lpNervousSystem->SetSystemPointers(m_lpSim, this, NULL, NULL, true);
	m_lpNervousSystem->Load(oXml);

	oXml.OutOfElem(); //OutOf NervousSystem Element

	LoadRobotInterface(oXml);

	oXml.OutOfElem(); //OutOf Structure Element
}
