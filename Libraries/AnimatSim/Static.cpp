// Static.cpp: implementation of the Static class.
//
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a static joint object..
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a static joint. 
*/

Static::Static()
{

}

/*! \brief 
   Destroys the static joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the static joint object..	 
*/

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
