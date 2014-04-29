/**
\file	Torus.cpp

\brief	Implements the Torus class. 
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
#include "Torus.h"
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
Torus::Torus()
{
	m_fltOutsideRadius = 0.5f;
	m_fltInsideRadius = 0.1f;
	m_iSides = 20;
	m_iRings = 20;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/4/2011
**/
Torus::~Torus()
{

}

float Torus::OutsideRadius() {return m_fltOutsideRadius;}

void Torus::OutsideRadius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Torus.OutsideRadius");

	if(bUseScaling)
		m_fltOutsideRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltOutsideRadius = fltVal;

	Resize();
}

float Torus::InsideRadius() {return m_fltInsideRadius;}

void Torus::InsideRadius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Torus.InsideRadius");

	if(bUseScaling)
		m_fltInsideRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltInsideRadius = fltVal;

	Resize();
}

/**
\brief	Sets the number of sides used to draw the Torus.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Torus::Sides(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, true, "Torus.Sides", true);
	m_iSides = iVal;

	Resize();
}

/**
\brief	Gets the number of sides used to draw the Torus.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Torus::Sides() {return m_iSides;}

/**
\brief	Sets the number of rings used to draw the Torus.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Torus::Rings(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, true, "Torus.Rings", true);
	m_iRings = iVal;

	Resize();
}

/**
\brief	Gets the number of rings used to draw the Torus.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Torus::Rings() {return m_iRings;}

bool Torus::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, false))
		return true;

	if(strType == "OUTSIDERADIUS")
	{
		OutsideRadius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "INSIDERADIUS")
	{
		InsideRadius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "SIDES")
	{
		Sides(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "RINGS")
	{
		Rings(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Torus::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RigidBody::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("OutsideRadius", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("InsideRadius", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Sides", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Rings", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

void Torus::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	OutsideRadius(oXml.GetChildFloat("OutsideRadius", m_fltOutsideRadius));
	InsideRadius(oXml.GetChildFloat("InsideRadius", m_fltInsideRadius));
	Sides(oXml.GetChildInt("Sides", m_iSides));
	Rings(oXml.GetChildInt("Rings", m_iRings));

	oXml.OutOfElem(); //OutOf RigidBody Element
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
