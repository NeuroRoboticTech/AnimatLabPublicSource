// OdorSensor.cpp: implementation of the OdorSensor class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a muscle attachment object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a muscle attachment. 
*/

OdorSensor::OdorSensor()
{
	m_fltOdorValue = 0;
	m_lpOdorType = NULL;
}

/*! \brief 
   Destroys the muscle attachment object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the muscle attachment object..	 
*/

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

