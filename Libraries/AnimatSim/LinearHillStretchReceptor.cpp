// LinearHillStretchReceptor.cpp: implementation of the LinearHillStretchReceptor class.
//
//////////////////////////////////////////////////////////////////////

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
#include "Attachment.h"
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

#include "ExternalStimulus.h"

#include "LineBase.h"
#include "MuscleBase.h" 
#include "LinearHillMuscle.h"
#include "LinearHillStretchReceptor.h"

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
   Constructs a muscle object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a muscle. 
*/

LinearHillStretchReceptor::LinearHillStretchReceptor()
{
	m_bApplyTension = FALSE;
	m_fltIaDischargeConstant = 100;
	m_fltIIDischargeConstant = 100;
	m_fltIaRate = 0;
	m_fltIIRate = 0;
}

/*! \brief 
   Destroys the muscle object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the muscle object..	 
*/

LinearHillStretchReceptor::~LinearHillStretchReceptor()
{
}

void LinearHillStretchReceptor::CalculateTension()
{
	LinearHillMuscle::CalculateTension();

	m_fltIaRate = m_fltIaDischargeConstant*m_fltSeLength;
	m_fltIIRate = m_fltIIDischargeConstant*m_fltPeLength;
}

float *LinearHillStretchReceptor::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	float *lpData = LinearHillMuscle::GetDataPointer(strDataType);
	if(lpData) return lpData;

	if(strType == "IA")
		lpData = &m_fltIaRate;
	else if(strType == "IB")
		lpData = &m_fltIbRate;
	else if(strType == "II")
		lpData = &m_fltIIRate;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
}

BOOL LinearHillStretchReceptor::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(strDataType == "POSITION")
	{
		//Position(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void LinearHillStretchReceptor::Load(CStdXml &oXml)
{
	LinearHillMuscle::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_bApplyTension = oXml.GetChildBool("ApplyTension", m_bApplyTension);

	m_fltIaDischargeConstant = oXml.GetChildFloat("IaDischarge", m_fltIaDischargeConstant);
	m_fltIIDischargeConstant = oXml.GetChildFloat("IIDischarge", m_fltIIDischargeConstant);

	Std_InValidRange((float) 0, (float) 1e11, m_fltIaDischargeConstant, TRUE, "IaDischargeConstant");
	Std_InValidRange((float) 0, (float) 1e11, m_fltIIDischargeConstant, TRUE, "IIDischargeConstant");

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
