// Cone.cpp: implementation of the Cone class.
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
#include "Cone.h"
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
   Constructs a Cone object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a Cone. 
*/

Cone::Cone()
{
	m_fltLowerRadius = 1;
	m_fltUpperRadius = 1;
	m_fltHeight = 1;
	m_fltCollisionLowerRadius = 1;
	m_fltCollisionUpperRadius = 1;
	m_fltCollisionHeight = 1;
}


/*! \brief 
   Destroys the Cone object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Cone object..	 
*/

Cone::~Cone()
{

}

void Cone::Trace(ostream &oOs)
{}

void Cone::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	RigidBody::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element
	m_fltLowerRadius = oXml.GetChildFloat("LowerRadius");
	m_fltUpperRadius = oXml.GetChildFloat("UpperRadius");
	m_fltHeight = oXml.GetChildFloat("Height");

	m_fltCollisionLowerRadius = oXml.GetChildFloat("CollisionLowerRadius");
	m_fltCollisionUpperRadius = oXml.GetChildFloat("CollisionUpperRadius");
	m_fltCollisionHeight = oXml.GetChildFloat("CollisionHeight");
	oXml.OutOfElem(); //OutOf RigidBody Element

	Std_IsAboveMin((float) 0,m_fltLowerRadius, TRUE, "LowerRadius", TRUE);
	Std_IsAboveMin((float) 0,m_fltUpperRadius, TRUE, "UpperRadius", TRUE);
	Std_IsAboveMin((float) 0, m_fltHeight, TRUE, "Height");
	
	if(m_fltLowerRadius == 0 && m_fltUpperRadius == 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidConeRadius, Al_Err_strInvalidConeRadius, "Body", m_strName);

	Std_IsAboveMin((float) 0, m_fltCollisionLowerRadius, TRUE, "CollisionLowerRadius", TRUE);
	Std_IsAboveMin((float) 0, m_fltCollisionUpperRadius, TRUE, "CollisionUpperRadius", TRUE);
	Std_IsAboveMin((float) 0, m_fltCollisionHeight, TRUE, "CollisionHeight", FALSE);

	if(m_fltCollisionLowerRadius == 0 && m_fltCollisionUpperRadius == 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidConeRadius, Al_Err_strInvalidConeRadius, "Body", m_strName);

	lpSim->HasConvexMesh(TRUE);
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
