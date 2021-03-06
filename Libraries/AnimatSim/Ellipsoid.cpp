/**
\file	Ellipsoid.cpp

\brief	Implements the Ellipsoid class. 
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
#include "Ellipsoid.h"
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
Ellipsoid::Ellipsoid()
{
	m_fltMajorRadius = 0.1f;
	m_fltMinorRadius = 0.3f;
	m_iLatSegments = 20;
	m_iLongSegments = 20;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/4/2011
**/
Ellipsoid::~Ellipsoid()
{

}

float Ellipsoid::MajorRadius() {return m_fltMajorRadius;}

void Ellipsoid::MajorRadius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Ellipsoid.MajorRadius");

	if(bUseScaling)
		m_fltMajorRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMajorRadius = fltVal;

	Resize();
}

float Ellipsoid::MinorRadius() {return m_fltMinorRadius;}

void Ellipsoid::MinorRadius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Ellipsoid.MinorRadius");

	if(bUseScaling)
		m_fltMinorRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMinorRadius = fltVal;

	Resize();
}

/**
\brief	Sets the number of segments used to draw the ellipsoid in the latitude direction.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Ellipsoid::LatSegments(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, true, "Ellipsoid.LatSegments", true);
	m_iLatSegments = iVal;

	Resize();
}

/**
\brief	Gets the number of segments used to draw the ellipsoid in the latitude direction.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Ellipsoid::LatSegments() {return m_iLatSegments;}

/**
\brief	Sets the number of segments used to draw the ellipsoid in the longtitude direction.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Ellipsoid::LongSegments(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, true, "Ellipsoid.LongSegments", true);
	m_iLongSegments = iVal;

	Resize();
}

/**
\brief	Gets the number of segments used to draw the ellipsoid in the longtitude direction.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Ellipsoid::LongSegments() {return m_iLongSegments;}

bool Ellipsoid::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, false))
		return true;

	if(strType == "MAJORRADIUS")
	{
		MajorRadius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MINORRADIUS")
	{
		MinorRadius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "LATITUDESEGMENTS")
	{
		LatSegments((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "LONGTITUDESEGMENTS")
	{
		LongSegments(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Ellipsoid::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RigidBody::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("MajorRadius", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MinorRadius", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LatitudeSegments", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LongtitudeSegments", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

void Ellipsoid::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	MajorRadius(oXml.GetChildFloat("MajorRadius", m_fltMajorRadius));
	MinorRadius(oXml.GetChildFloat("MinorRadius", m_fltMinorRadius));
	LatSegments(oXml.GetChildInt("LatitudeSegments", m_iLatSegments));
	LongSegments(oXml.GetChildInt("LongtitudeSegments", m_iLongSegments));

	oXml.OutOfElem(); //OutOf RigidBody Element
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
