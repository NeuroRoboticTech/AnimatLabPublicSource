/**
\file	LinearJoint.cpp

\brief	Implements the linear 1-D, 2-D, and 3-D class.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "LinearJoint.h"
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
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
LinearJoint::LinearJoint()
{
	//Default to invalid type.
	m_iLinearType = -1;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
LinearJoint::~LinearJoint()
{

}

void LinearJoint::LinearType(string strType)
{
	string strTP = Std_ToUpper(Std_Trim(strType));

	if(strTP == "LINEAR1D")
		LinearType(0);
	else if(strTP == "LINEAR2D")
		LinearType(1);
	else if(strTP == "LINEAR3D")
		LinearType(2);
	else
		THROW_PARAM_ERROR(Al_Err_lInvalidLinearType, Al_Err_strInvalidLinearType, "Type", strType);
}

void LinearJoint::LinearType(int iType)
{
	Std_InValidRange((int) 0, (int) 2, iType, true, "LinearType");
	m_iLinearType = iType;
}

int LinearJoint::LinearType() {return m_iLinearType;}

float  LinearJoint::PlaneWidth() {return m_fltSize/100.0f;}

float  LinearJoint::PlaneSize() {return m_fltSize;}

float  LinearJoint::CylinderRadius() {return m_fltSize*0.1f;}

float  LinearJoint::CylinderHeight()  {return m_fltSize;}

#pragma region DataAccesMethods

bool LinearJoint::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Joint::SetData(strType, strValue, false))
		return true;

	if(strType == "LINEARTYPE")
	{
		LinearType(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void LinearJoint::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Joint::QueryProperties(aryNames, aryTypes);

	aryNames.Add("LinearType");
	aryTypes.Add("String");
}

#pragma endregion

void LinearJoint::Load(CStdXml &oXml)
{
	Joint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	LinearType(oXml.GetChildString("LinearType"));

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
