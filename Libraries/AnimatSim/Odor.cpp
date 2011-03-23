/**
\file	Odor.cpp

\brief	Implements the odor class.
**/

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


namespace AnimatSim
{
	namespace Environment
	{
/**
\brief	Constructor.

\author	dcofer
\date	3/23/2011

\param [in,out]	lpParent	If non-null, the pointer to a parent. 
**/
Odor::Odor(RigidBody *lpParent)
{
	m_lpParent = lpParent;
	m_lpOdorType = NULL;
	m_fltQuantity = 100;
	m_bUseFoodQuantity = FALSE;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/23/2011
**/
Odor::~Odor()
{

try
{
	m_lpParent = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Odor\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Sets the odor type to emit for this odorant.

\details This takes teh supplied odor type ID and finds that odor, and adds itself as a source.

\author	dcofer
\date	3/23/2011

\param	strType	OdorType ID. 
**/
void Odor::SetOdorType(string strType)
{
	//Now lets find the odor type for this odor and add this one to it.
	m_lpOdorType = m_lpSim->FindOdorType(strType);
	m_lpOdorType->AddOdorSource(this);
}

/**
\brief	Gets a pointer to the odor type.

\author	dcofer
\date	3/23/2011

\return	Pointer to OdorType.
**/
OdorType  *Odor::GetOdorType() {return m_lpOdorType;}

/**
\brief	Sets the quantity that can be used when calculating the odor concentration.

\details If m_bUseFoodQuantity is True then this value is not used.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void Odor::Quantity(float fltVal) 
{
	Std_IsAboveMin((float) 0, m_fltQuantity, TRUE, "Quantity");
	m_fltQuantity = fltVal;
}

/**
\brief	Gets the quantity that will be used when calculating the odor concentration.

\details If m_bUseFoodQuantity is True then this return FoodQuantity from the parent RigidBody,
else it returns m_fltQuantity.

\author	dcofer
\date	3/23/2011

\return	Current quantity.
**/
float Odor::Quantity() 
{
	if(m_bUseFoodQuantity)
		return m_lpParent->FoodQuantity();
	else
		return m_fltQuantity;
};

/**
\brief	Tells whether we should use the FoodQuantity of the parent RigidBody when calculating the odor value.

\author	dcofer
\date	3/23/2011

\return	true if it uses RigidBody food quantity, false else.
**/
BOOL Odor::UseFoodQuantity() {return m_bUseFoodQuantity;}

/**
\brief	Sets whether we should use the FoodQuantity of the parent RigidBody when calculating the odor value.

\author	dcofer
\date	3/23/2011

\param	bVal	true if it uses RigidBody food quantity, false else.
**/
void Odor::UseFoodQuantity(BOOL bVal) {m_bUseFoodQuantity = bVal;}

/**
\brief	Calculates the odor value for this Odorant for a given odor sensor somewhere in the environment.

\details The odor value that a sensor detects is determined by how far away a givent odorant is from it, the
diffusion constant, and the quantity of the odor present at the source. This equation describes the relationship.
fltVal = (this->Quantity() * lpType->DiffusionConstant()) / (fltDist * fltDist); 

\author	dcofer
\date	3/23/2011

\param [in,out]	lpType	  	Pointer to the OdorType for which we are calculating the odor. 
\param [in,out]	oSensorPos	The position to the odor sensor. 

\return	The calculated odor value.
**/
float Odor::CalculateOdorValue(OdorType *lpType, CStdFPoint &oSensorPos)
{
	if(!m_lpParent) return 0;

	CStdFPoint oOdorPos = m_lpParent->GetCurrentPosition();

	float fltDist = Std_CalculateDistance(oOdorPos, oSensorPos);
	float fltVal = (this->Quantity() * lpType->DiffusionConstant()) / (fltDist * fltDist); 
	
	return fltVal;
}

void Odor::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Odor Element

	SetOdorType(oXml.GetChildString("OdorTypeID"));
	Quantity(oXml.GetChildFloat("Quantity", m_fltQuantity));
	UseFoodQuantity(oXml.GetChildFloat("UseFoodQuantity", m_bUseFoodQuantity));

	oXml.OutOfElem(); //OutOf Odor Element
}

	}			// Environment
}			//AnimatSim
