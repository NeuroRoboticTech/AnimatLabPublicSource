/**
\file	OdorType.cpp

\brief	Implements the odor type class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
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
\brief	Default constructor.

\author	dcofer
\date	3/23/2011
**/
OdorType::OdorType()
{
	m_fltDiffusionConstant = 1;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/23/2011
**/
OdorType::~OdorType()
{

try
{
	m_aryOdorSources.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Odor\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the diffusion constant.

\author	dcofer
\date	3/23/2011

\return	Diffusion constant.
**/
float OdorType::DiffusionConstant() {return m_fltDiffusionConstant;};

/**
\brief	Sets the diffusion constant.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void OdorType::DiffusionConstant(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Diffusion Constant");

	if(bUseScaling)
		fltVal = fltVal/(m_lpSim->DistanceUnits()*m_lpSim->DistanceUnits()); //Our diffusion constant is in m^2/s. We need to convert its distance units appropriately.

	m_fltDiffusionConstant = fltVal;
};

/**
\brief	Finds the odor source with the specified GUID ID.

\author	dcofer
\date	3/23/2011

\param	strOdorID  	GUID ID of the Odor to find. 
\param	bThrowError	If true and the odor is not found then throw an error, else return NULL if not found

\return	null if Odor is not found and bThrowError=False, else the found odor source.
**/
Odor *OdorType::FindOdorSource(string strOdorID, BOOL bThrowError)
{
	Odor *lpOdor = NULL;
	CStdMap<string, Odor *>::iterator oPos;
	oPos = m_aryOdorSources.find(Std_CheckString(strOdorID));

	if(oPos != m_aryOdorSources.end())
		lpOdor =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lOdorIDNotFound, Al_Err_strOdorIDNotFound, "OdorID", strOdorID);

	return lpOdor;
}

/**
\brief	Adds an odor source to the list of odors that are emitting this type of odor. 

\author	dcofer
\date	3/23/2011

\param [in,out]	lpOdor	Pointer to an odor emitting this OdorType. 
**/
void OdorType::AddOdorSource(Odor *lpOdor)
{
	if(!FindOdorSource(lpOdor->ID(), FALSE))
	{
		m_aryOdorSources.Add(lpOdor->ID(), lpOdor);
	}
}

/**
\brief	Calculates the odor value for this OdorType for a given sensor location.

\details This loops through all of the odors that are emitting this OdorType and 
calculates the odor strength for each one based on its distance from the specified
sensor. This is the total odor strength for the sensor.

\author	dcofer
\date	3/23/2011

\param [in,out]	oSensorPos	The sensor position. 

\return	The total calculated odor value.
**/
float OdorType::CalculateOdorValue(CStdFPoint &oSensorPos)
{
	//Loop through each of the associated odors and calculate the value for each one.
	CStdMap<string, Odor *>::iterator oIterator;
	Odor *lpOdor = NULL;
	float fltVal = 0;

	for(oIterator=m_aryOdorSources.begin(); 
	    oIterator!=m_aryOdorSources.end(); 
			++oIterator)
	{
		lpOdor = oIterator->second;
		fltVal += lpOdor->CalculateOdorValue(this, oSensorPos);
	}

	return fltVal;
}

void OdorType::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	DiffusionConstant(oXml.GetChildFloat("DiffusionConstant", m_fltDiffusionConstant));

	oXml.OutOfElem(); //OutOf Joint Element
}

	}			// Environment
}			//AnimatSim
