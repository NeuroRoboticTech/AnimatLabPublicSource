// Sphere.cpp: implementation of the Sphere class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sphere.h"
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
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a Sphere object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a Sphere. 
*/

Sphere::Sphere()
{
	m_fltRadius = 1;
	m_fltCollisionRadius = 1;
}


/*! \brief 
   Destroys the Sphere object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Sphere object..	 
*/

Sphere::~Sphere()
{

}

void Sphere::Trace(ostream &oOs)
{}

void Sphere::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	RigidBody::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element
	m_fltRadius = oXml.GetChildFloat("Radius");
	m_fltCollisionRadius = oXml.GetChildFloat("CollisionRadius");
	oXml.OutOfElem(); //OutOf RigidBody Element

	Std_IsAboveMin((float) 0,m_fltRadius, TRUE, "Radius");
	Std_IsAboveMin((float) 0, m_fltCollisionRadius, TRUE, "CollisionRadius");
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
