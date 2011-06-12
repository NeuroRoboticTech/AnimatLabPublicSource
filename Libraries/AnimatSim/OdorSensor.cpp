/**
\file	OdorSensor.cpp

\brief	Implements the odor sensor class. 
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
#include "Sensor.h"
#include "OdorType.h"
#include "Odor.h"
#include "OdorSensor.h"
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/10/2011
**/
OdorSensor::OdorSensor()
{
	m_fltOdorValue = 0;
	m_lpOdorType = NULL;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
OdorSensor::~OdorSensor()
{
	m_lpOdorType = NULL;
}

/**
\brief	Sets the odor type identifier.

\author	dcofer
\date	6/12/2011

\param	strID	Identifier for the odor type.
**/
void OdorSensor::OdorTypeID(string strID)
{
	SetOdorTypePointer(strID);
	m_strOdorTypeID = strID;
}

/**
\brief	Gets the odor type identifier.

\author	dcofer
\date	6/12/2011

\return	ID.
**/
string OdorSensor::OdorTypeID() {return m_strOdorTypeID;}

/**
\brief	Sets the odor type pointer.

\author	dcofer
\date	6/12/2011

\param	strID	Identifier for the odor type.
**/
void OdorSensor::SetOdorTypePointer(string strID)
{
	if(Std_IsBlank(strID))
		m_lpOdorType = NULL;
	else
	{
		m_lpOdorType = dynamic_cast<OdorType *>(m_lpSim->FindByID(strID));
		if(!m_lpOdorType)
			THROW_PARAM_ERROR(Al_Err_lPartTypeNotOdorType, Al_Err_strPartTypeNotOdorType, "ID", strID);
	}
}

BOOL OdorSensor::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "ODORTYPEID")
	{
		OdorTypeID(strValue);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

float *OdorSensor::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	if(strType == "ODORVALUE")
		lpData = &m_fltOdorValue;

	lpData = RigidBody::GetDataPointer(strDataType);

	return lpData;
}

void OdorSensor::StepSimulation()
{
	Sensor::StepSimulation();

	if(m_lpOdorType)
		m_fltOdorValue = m_lpOdorType->CalculateOdorValue(m_oAbsPosition);
}

void OdorSensor::Load(CStdXml &oXml)
{
	Sensor::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	OdorTypeID(oXml.GetChildString("OdorTypeID", ""));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim

