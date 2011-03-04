// BallSocket.cpp: implementation of the BallSocket class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "BallSocket.h"
#include "Structure.h"
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
   Constructs a BallSocket joint.
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a BallSocket joint. 
*/

BallSocket::BallSocket()
{
	m_fltConstraintAngle = (float) (0.25*PI);
	m_fltStiffness = AL_INFINITY;
	m_fltDamping = AL_INFINITY;
}


/*! \brief 
   Destroys the BallSocket joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the BallSocket joint object..	 
*/

BallSocket::~BallSocket()
{

}

void BallSocket::Load(CStdXml &oXml)
{
	Joint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element
	
	Std_LoadPoint(oXml, "ConstraintAxis", m_oConstraintAxis);
	m_fltConstraintAngle = oXml.GetChildFloat("ConstraintHalfAngle");

	Std_IsAboveMin((float) 0, m_fltConstraintAngle, TRUE, "ConstraintAngle");

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
