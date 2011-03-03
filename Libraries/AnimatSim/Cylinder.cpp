// CAlCylinder.cpp: implementation of the CAlCylinder class.
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
#include "Cylinder.h"
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
   Constructs a cylinder object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a cylinder. 
*/

Cylinder::Cylinder()
{
	m_fltRadius = 1;
	m_fltHeight = 1;
	m_fltCollisionRadius = 1;
	m_fltCollisionHeight = 1;
}


/*! \brief 
   Destroys the cylinder object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the cylinder object..	 
*/

Cylinder::~Cylinder()
{

}

void Cylinder::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	m_fltRadius = oXml.GetChildFloat("Radius");
	m_fltHeight = oXml.GetChildFloat("Height");

	m_fltCollisionRadius = oXml.GetChildFloat("CollisionRadius");
	m_fltCollisionHeight = oXml.GetChildFloat("CollisionHeight");
	oXml.OutOfElem(); //OutOf RigidBody Element

	Std_IsAboveMin((float) 0,m_fltRadius, TRUE, "Radius");
	Std_IsAboveMin((float) 0, m_fltHeight, TRUE, "Height");
	
	Std_IsAboveMin((float) 0, m_fltCollisionRadius, TRUE, "CollisionRadius");
	Std_IsAboveMin((float) 0, m_fltCollisionHeight, TRUE, "CollisionHeight");
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
