/**
\file	Cone.cpp

\brief	Implements the cone class. 
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
#include "Light.h"
#include "LightManager.h"
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
\date	3/4/2011
**/
Cone::Cone()
{
	m_fltLowerRadius = 1;
	m_fltUpperRadius = 1;
	m_fltHeight = 1;
	m_iSides = 10;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/4/2011
**/
Cone::~Cone()
{

}

float Cone::LowerRadius() {return m_fltLowerRadius;}

void Cone::LowerRadius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Cone.LowerRadius", true);
	if(fltVal == 0 && m_fltUpperRadius == 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidConeRadius, Al_Err_strInvalidConeRadius, "Body", m_strName);

	if(bUseScaling)
		m_fltLowerRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLowerRadius = fltVal;

	Resize();
}

float Cone::UpperRadius() {return m_fltUpperRadius;}

void Cone::UpperRadius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Cone.UpperRadius", true);
	if(m_fltLowerRadius == 0 && fltVal == 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidConeRadius, Al_Err_strInvalidConeRadius, "Body", m_strName);

	if(bUseScaling)
		m_fltUpperRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltUpperRadius = fltVal;

	Resize();
}


float Cone::Height() {return m_fltHeight;}

void Cone::Height(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Cone.Height");
	if(bUseScaling)
		m_fltHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltHeight = fltVal;

	Resize();
}

/**
\brief	Sets  the number of sides used to draw the cone.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Cone::Sides(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, true, "Cone.Sides", true);
	m_iSides = iVal;

	Resize();
}

/**
\brief	Gets the number of sides used to draw the cone.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Cone::Sides() {return m_iSides;}

bool Cone::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, false))
		return true;

	if(strType == "LOWERRADIUS")
	{
		LowerRadius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "UPPERRADIUS")
	{
		UpperRadius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "HEIGHT")
	{
		Height((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "SIDES")
	{
		Sides(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Cone::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	RigidBody::QueryProperties(aryNames, aryTypes);

	aryNames.Add("LowerRadius");
	aryTypes.Add("Float");

	aryNames.Add("UpperRadius");
	aryTypes.Add("Float");

	aryNames.Add("Height");
	aryTypes.Add("Float");

	aryNames.Add("Sides");
	aryTypes.Add("Integer");
}

void Cone::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	LowerRadius(oXml.GetChildFloat("LowerRadius", m_fltLowerRadius));
	UpperRadius(oXml.GetChildFloat("UpperRadius", m_fltUpperRadius));
	Height(oXml.GetChildFloat("Height"), m_fltHeight);
	Sides(oXml.GetChildInt("Sides", m_iSides));

	oXml.OutOfElem(); //OutOf RigidBody Element
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
