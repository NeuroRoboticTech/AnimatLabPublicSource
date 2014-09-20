/**
\file	RobotIOControl.cpp

\brief	Implements the base class for Robot IO controllers.
**/

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

#include "RobotInterface.h"
#include "RobotIOControl.h"
#include "RobotPartInterface.h"

namespace AnimatSim
{
	namespace Robotics
	{

/**
\brief	Default constructor.

\author	dcofer
\date	9/8/2014
**/
RobotIOControl::RobotIOControl(void)
{
	m_lpParentInterface = NULL;
	m_bSetupStarted = false;
	m_bSetupComplete	= false;	// flag so we setup when its ready, you don't need to touch this :)
	m_bStopIO = false;
	m_bIOThreadProcessing = false;
	m_fltStepIODuration = 0;
	m_iCyclePartIdx = 0;
	m_iCyclePartCount = 0;
	m_bPauseIO = false;
	m_bIOPaused = false;
	m_bWaitingForThreadNotify = false;
}


/**
\brief	Destructor.

\author	dcofer
\date	9/8/2014
**/
RobotIOControl::~RobotIOControl(void)
{
try
{
	m_aryParts.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RobotIOControl\r\n", "", -1, false, true);}
}

/**
\brief	Sets the parent robot interface pointer. 

\author	dcofer
\date	9/8/2014

\param	lpParent	pointer to parent robot interface
**/
void RobotIOControl::ParentInterface(RobotInterface *lpParent) {m_lpParentInterface = lpParent;}

/**
\brief	Gets the parent robot interface pointer. 

\author	dcofer
\date	9/8/2014

\return	pointer to parent robot interface. 
**/
RobotInterface *RobotIOControl::ParentInterface() {return m_lpParentInterface;}

/**
\brief	Gets the array of IO controls. 

\author	dcofer
\date	9/8/2014

\return	pointer to array of IO controls. 
**/
void RobotIOControl::PauseIO(bool bVal)
{
	m_bPauseIO = bVal;
	m_bIOPaused = false;
}

bool RobotIOControl::PauseIO() {return m_bPauseIO;}

bool RobotIOControl::IOPaused() {return m_bIOPaused;}

/**
\brief	Gets the array of IO controls.

\author	dcofer
\date	3/2/2011

\return	pointer to array of IO controls.
**/
CStdPtrArray<RobotPartInterface> *RobotIOControl::Parts() {return &m_aryParts;}

/**
\brief	Gets the time duration required to perform one step of the IO for all parts in this control.

\author	dcofer
\date	5/7/2014

\return	Step duration.
**/
float RobotIOControl::StepIODuration() {return m_fltStepIODuration;}

#pragma region DataAccesMethods

float *RobotIOControl::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "STEPIODURATION")
		return &m_fltStepIODuration;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RobotIOControl::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RobotIOControl::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("StepIODuration", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
}

bool RobotIOControl::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "ROBOTPARTINTERFACE")
	{
		AddPartInterface(strXml);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool RobotIOControl::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "ROBOTPARTINTERFACE")
	{
		RemovePartInterface(strID);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

/**
\brief	Creates and adds a robot IO control.

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the control node.
**/
RobotPartInterface *RobotIOControl::AddPartInterface(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("RobotPartInterface");

	RobotPartInterface *lpPart = LoadPartInterface(oXml);

	lpPart->Initialize();

    return lpPart;
}

/**
\brief	Removes the rigid body with the specified ID.

\author	dcofer
\date	3/2/2011

\param	strID	ID of the body to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void RobotIOControl::RemovePartInterface(std::string strID, bool bThrowError)
{
	int iPos = FindChildListPos(strID, bThrowError);

    RobotPartInterface *lpPart = m_aryParts[iPos];

	m_aryParts.RemoveAt(iPos);
}


/**
\brief	Finds the array index for the child part with the specified ID

\author	dcofer
\date	3/2/2011

\param	strID ID of part to find
\param	bThrowError	If true and ID is not found then it will throw an error, else return NULL
\exception If bThrowError is true and ID is not found.

\return	If bThrowError is false and ID is not found returns NULL,
else returns the pointer to the found part.
**/
int RobotIOControl::FindChildListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryParts[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lPartInterfaceIDNotFound, Al_Err_strPartInterfaceIDNotFound, "ID", strID);

	return -1;
}

#pragma endregion

void RobotIOControl::StartIOThread()
{
	int iWaitTime = 30;
#ifdef _DEBUG
	iWaitTime = 200;
#endif

	boost::posix_time::ptime pt = boost::posix_time::microsec_clock::universal_time() +  boost::posix_time::seconds(iWaitTime);

	boost::interprocess::scoped_lock<boost::interprocess::interprocess_mutex> lock(m_WaitForIOSetupMutex);

	m_bWaitingForThreadNotify = false;
	m_ioThread = boost::thread(&RobotIOControl::ProcessIO, this);

	std::cout << "Waiting for IO thread return\r\n";
	m_bWaitingForThreadNotify = true;
	bool bWaitRet = m_WaitForIOSetupCond.timed_wait(lock, pt);

	if(!bWaitRet)
	{
		std::cout << "IO thread Timed out\r\n";
		ShutdownIO();
		THROW_ERROR(Al_Err_lErrorSettingUpIOThread, Al_Err_strErrorSettingUpIOThread);
	}

	std::cout << "IO thread returned\r\n";
}

void RobotIOControl::WaitForThreadNotifyReady()
{
	//Wait to fire the signal until we get notification that it is waiting.
	while(!m_bWaitingForThreadNotify)
		boost::this_thread::sleep(boost::posix_time::microseconds(500));
}

void RobotIOControl::ProcessIO()
{
	try
	{
		m_bIOThreadProcessing = true;

		SetupIO();

		m_bSetupComplete = true;

		WaitForThreadNotifyReady();

		//Notify it back that we are ready.
		m_WaitForIOSetupCond.notify_all();

		while(!m_bStopIO)
		{
			if(m_bPauseIO || m_lpSim->Paused())
			{
				m_bIOPaused = true;
				boost::this_thread::sleep(boost::posix_time::microseconds(1000));
			}
			else
			{
				m_bIOPaused = false;
				StepIO();
			}
		}
	}
	catch(CStdErrorInfo oError)
	{
		m_bIOThreadProcessing = false;
	}
	catch(...)
	{
		m_bIOThreadProcessing = false;
	}

	m_bIOThreadProcessing = false;
}

void RobotIOControl::ExitIOThread()
{
	TRACE_DEBUG("ExitIOThread.");

	if(m_bIOThreadProcessing)
	{
		//Close the comm channel.
		CloseIO();

		//Tell the IO thread to shutdown
		m_bStopIO = true;

		TRACE_DEBUG("Joint Thread.\r\n");

	bool bTryJoin = false;
#if (BOOST_VERSION >= 105000)
	bTryJoin = m_ioThread.try_join_for(boost::chrono::seconds(30));
#else
	m_ioThread.join();
#endif
	}
}

/**
\brief	This method is called after all connections to whatever control board have been made. It calls
each parts SetupIO method. For example, We connect to a Firmata
microcontroller like an Arduino, and then do a setup that could take some time. We should not attempt to
setup any of the pins until after the board itself has been setup. After that we need to loop through and setup
all the parts.

\author	dcofer
\date	5/1/2014

**/
void RobotIOControl::SetupIO()
{
	if(m_bEnabled)
	{
		m_iCyclePartCount = 0;
		int iCount = m_aryParts.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			if(m_aryParts[iIndex]->Enabled())
			{
				if(m_aryParts[iIndex]->IncludeInPartsCycle())
					m_iCyclePartCount++;

				m_aryParts[iIndex]->SetupIO();
			
				//Give it just smidge of time to finish processing the last set of data so we do not overload the arduino's buffer and so it has time to process the requests.
				boost::this_thread::sleep(boost::posix_time::microseconds(1000));
			}
	}
	else
		//Give the wait thread a chance to start waiting if we are not enabled.
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
}

/**
\brief	This method is called from within the IO thread. It calls StepIO for each part.

\author	dcofer
\date	5/2/2014

**/
void RobotIOControl::StepIO()
{
	if(m_bEnabled)
	{
		unsigned long long lStepStartTick = m_lpSim->GetTimerTick();

		int iCount = m_aryParts.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			if(m_aryParts[iIndex]->Enabled())
				m_aryParts[iIndex]->StepIO(m_iCyclePartIdx);

		unsigned long long lEndStartTick = m_lpSim->GetTimerTick();
		m_fltStepIODuration = m_lpSim->TimerDiff_m(lStepStartTick, lEndStartTick);

		m_iCyclePartIdx++;
		if(m_iCyclePartIdx >= m_iCyclePartCount)
			m_iCyclePartIdx = 0;
	}
}

/**
\brief	This method is waits until the m_bPauseIO flag is set back to false.

\author	dcofer
\date	5/12/2014

**/
void RobotIOControl::WaitWhilePaused()
{
	m_bIOPaused = true;
	while(m_bPauseIO)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
	m_bIOPaused = false;
}

/**
\brief	This method is waits until the m_bIOPaused flag is set to true.

\author	dcofer
\date	9/20/2014

**/
void RobotIOControl::WaitTillPaused()
{
	while(!m_bIOPaused)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
}

/**
\brief	This method is waits until the m_bIOPaused flag is set to true.

\author	dcofer
\date	5/12/2014

**/
void RobotIOControl::StartPause()
{
	m_bPauseIO = true;

	if(!m_bEnabled || (m_lpSim->InSimulation() && m_lpSim->Paused()))
		m_bIOPaused = true;

	while(!m_bIOPaused)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
}

/**
\brief	This method is waits until the m_bIOPaused flag is set back to false.

\author	dcofer
\date	5/12/2014

**/
void RobotIOControl::ExitPause()
{
	m_bPauseIO = false;

	if(!m_bEnabled || (m_lpSim->InSimulation() && m_lpSim->Paused()))
		m_bIOPaused = false;

	while(m_bIOPaused)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
}

/**
\brief	This method is called just before the IO thread is closed down. It gives the IO objects a chance to do
any required cleanup.

\author	dcofer
\date	5/12/2014

**/
void RobotIOControl::ShutdownIO()
{
	//Prevent any more attempts to write to the comm channel.
	if(m_bIOThreadProcessing)
	{
		StartPause();
		WaitTillPaused();
	}

	if(m_bEnabled)
	{
		int iCount = m_aryParts.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			if(m_aryParts[iIndex]->Enabled())
			{
				TRACE_DEBUG("Shutting down IO: " + m_aryParts[iIndex]->Name());
				m_aryParts[iIndex]->ShutdownIO();
			}
	}

	ExitIOThread();
}

void RobotIOControl::Initialize()
{
	if(m_bEnabled)
	{
		// Open device. Do this before calling the Initialize on the parts so they can have communications.
		if(!m_lpSim->InSimulation())
		{
			OpenIO();

			StartIOThread();
		}

		int iCount = m_aryParts.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			m_aryParts[iIndex]->Initialize();
	}
}

void RobotIOControl::ResetSimulation()
{
	AnimatBase::ResetSimulation();

	if(m_bEnabled)
	{
		m_iCyclePartIdx = 0;
		int iCount = m_aryParts.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			m_aryParts[iIndex]->ResetSimulation();
	}
}

void RobotIOControl::AfterResetSimulation()
{
	AnimatBase::AfterResetSimulation();

	if(m_bEnabled)
	{
		int iCount = m_aryParts.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			m_aryParts[iIndex]->AfterResetSimulation();
	}
}

void RobotIOControl::SimStopping()
{
	AnimatBase::SimStopping();

	ShutdownIO();
}

void RobotIOControl::StepSimulation()
{
	if(m_bEnabled)
	{
		AnimatBase::StepSimulation();

		int iCount = m_aryParts.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			if(m_aryParts[iIndex]->Enabled())
				m_aryParts[iIndex]->StepSimulation();
	}
}

void RobotIOControl::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	if(oXml.FindChildElement("Parts", false))
	{
		oXml.IntoElem();  //Into ChildBodies Element
		int iChildCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadPartInterface(oXml);
		}
		oXml.OutOfElem(); //OutOf ChildBodies Element
	}

	oXml.OutOfElem(); //OutOf RigidBody Element
}

/**
\brief	Loads a child IO Control.

\author	dcofer
\date	3/2/2011

\param [in,out]	oXml	The xml data definition of the part to load.

\return	null if it fails, else the IO control.
**/

RobotPartInterface *RobotIOControl::LoadPartInterface(CStdXml &oXml)
{
	RobotPartInterface *lpChild = NULL;
	std::string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	std::string strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpChild = dynamic_cast<RobotPartInterface *>(m_lpSim->CreateObject(strModule, "RobotPartInterface", strType));
	if(!lpChild)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RobotPartInterface");

	lpChild->ParentIOControl(this);
	lpChild->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, true);

	lpChild->Load(oXml);

	m_aryParts.Add(lpChild);

	return lpChild;
}
catch(CStdErrorInfo oError)
{
	if(lpChild) delete lpChild;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpChild) delete lpChild;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


	}
}
