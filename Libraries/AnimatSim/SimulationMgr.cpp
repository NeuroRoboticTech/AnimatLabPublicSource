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
#include "SimulationThread.h"
#include "SimulationMgr.h"

#include "RobotInterface.h"
#include "RobotIOControl.h"
#include "RobotPartInterface.h"

namespace AnimatSim
{

SimulationMgr::SimulationMgr(void)
{
}

SimulationMgr::~SimulationMgr(void)
{
try
{
	ShutdownAllSimulations();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of SimulationMgr\r\n", "", -1, false, true);}
}

SimulationMgr &SimulationMgr::Instance()
{
	static SimulationMgr g_SimMgrInstance;
	return g_SimMgrInstance;
}

SimulationThread *SimulationMgr::CreateSimulation(std::string strSimFile, bool bForceNoWindows)
{
	SimulationThread *lpThread = NULL;
	
	try
	{
		lpThread = new SimulationThread();
		lpThread->StartSimulation(strSimFile, bForceNoWindows);
		m_arySimThreads.Add(lpThread);
		return lpThread;
	}
	catch(CStdErrorInfo oError)
	{
		if(lpThread) delete lpThread;
		RELAY_ERROR(oError);
		return NULL;
	}
	catch(...)
	{
		if(lpThread) delete lpThread;
		THROW_ERROR(Al_Err_lUnknownError, Al_Err_strUnknownError);
		return NULL;
	}
}

void SimulationMgr::ShutdownAllSimulations()
{
	int iSize = m_arySimThreads.GetSize();
	for(int iIdx=0; iIdx<iSize; iIdx++)
		m_arySimThreads[iIdx]->ShutdownSimulation();

	m_arySimThreads.RemoveAll();
}

AnimatBase *SimulationMgr::FindByID(std::string strID, bool bThrowError)
{
	AnimatBase *lpFound = NULL;
	int iSize = m_arySimThreads.GetSize();
	for(int iIdx=0; iIdx<iSize; iIdx++)
	{
		Simulator *lpSim = m_arySimThreads[iIdx]->Sim();
		if(lpSim)
			lpFound = lpSim->FindByID(strID, false);
		if(lpFound)
			return lpFound;
	}

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lIDNotFound, Al_Err_strIDNotFound, "ID", strID);

	return NULL;
}


}