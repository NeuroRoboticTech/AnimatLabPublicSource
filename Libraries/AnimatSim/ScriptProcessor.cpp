#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "Link.h"
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
#include "ScriptProcessor.h"

namespace AnimatSim
{

std::string g_strLastScriptError;
boost::mutex g_mtiScriptErrorLock;


void SetLastScriptError(std::string strError)
{
	boost::unique_lock<boost::mutex> scriptLock(g_mtiScriptErrorLock);
	g_strLastScriptError = strError;
}

std::string GetLastScriptError()
{
	return g_strLastScriptError;
}

ScriptProcessor::ScriptProcessor(void)
{
}

ScriptProcessor::~ScriptProcessor(void)
{
try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ScriptProcessor\r\n", "", -1, false, true);}
}


}