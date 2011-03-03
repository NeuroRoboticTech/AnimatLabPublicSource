// Odor.cpp: implementation of the Odor class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatSim
{
	namespace Environment
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

Odor::Odor(RigidBody *lpParent)
{
	m_lpParent = lpParent;
	m_lpOdorType = NULL;
	m_fltQuantity = 100;
	m_bUseFoodQuantity = FALSE;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

Odor::~Odor()
{

try
{
	m_lpParent = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Odor\r\n", "", -1, FALSE, TRUE);}
}

float Odor::Quantity() 
{
	if(m_bUseFoodQuantity)
		return m_lpParent->FoodQuantity();
	else
		return m_fltQuantity;
};

float Odor::CalculateOdorValue(Simulator *lpSim, OdorType *lpType, CStdFPoint &oSensorPos)
{
	if(!m_lpParent) return 0;

	CStdFPoint oOdorPos = m_lpParent->GetCurrentPosition();

	float fltDist = Std_CalculateDistance(oOdorPos, oSensorPos);
	float fltVal = (this->Quantity() * lpType->DiffusionConstant()) / (fltDist * fltDist); 
	
	return fltVal;
}

/*! \brief 
   Loads a joint from an xml configuration file.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                      this rigid body is a part of.
   \param oXml This is an xml object.

	 \return
	 No return value.

	 \remarks
	 This method is responsible for loading the joint from a XMl
	 configuration file. You should call this method even in your 
	 overriden function becuase it loads all of the base properties
	 for the Joint. 
*/

void Odor::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Odor Element

	string strOdorTypeID = Std_CheckString(oXml.GetChildString("OdorTypeID"));
	m_fltQuantity = oXml.GetChildFloat("Quantity", m_fltQuantity);
	Std_IsAboveMin((float) 0, m_fltQuantity, TRUE, "Quantity");

	m_bUseFoodQuantity = oXml.GetChildFloat("UseFoodQuantity", m_bUseFoodQuantity);

	oXml.OutOfElem(); //OutOf Odor Element

	//Now lets find the odor type for this odor and add this one to it.
	m_lpOdorType = m_lpSim->FindOdorType(strOdorTypeID);
	m_lpOdorType->AddOdorSource(this);
}

	}			// Environment
}			//AnimatSim
