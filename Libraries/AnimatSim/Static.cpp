/**
\file	Static.cpp

\brief	Implements the static class.
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Static.h"
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
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
Static::Static()
{

}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
Static::~Static()
{

}

void Static::Load(CStdXml &oXml)
{
	Joint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
