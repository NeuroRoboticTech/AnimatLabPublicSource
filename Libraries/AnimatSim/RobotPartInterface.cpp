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

RobotPartInterface::RobotPartInterface(void)
{
	m_lpParentInterface = NULL;
	m_lpParentIOControl = NULL;
}

RobotPartInterface::~RobotPartInterface(void)
{

try
{
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

void RobotPartInterface::Initialize()
{

}

	}
}