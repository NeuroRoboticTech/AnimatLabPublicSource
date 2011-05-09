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

float *OdorSensor::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	if(strType == "ODORVALUE")
		lpData = &m_fltOdorValue;

	lpData = RigidBody::GetDataPointer(strDataType);

	return lpData;
}

void OdorSensor::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	Sensor::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	string strOdorTypeID = oXml.GetChildString("OdorTypeID", "");

	oXml.OutOfElem(); //OutOf RigidBody Element

	if(!Std_IsBlank(strOdorTypeID))
		m_lpOdorType = m_lpSim->FindOdorType(strOdorTypeID);
}

		}		//Bodies
	}			//Environment
}				//AnimatSim

