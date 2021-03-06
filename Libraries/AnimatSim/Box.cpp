/**
\file	Box.cpp

\brief	Implements the box class. 
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
#include "Box.h"
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
Box::Box()
{
	m_fltLength = 1;
	m_fltWidth = 1;
	m_fltHeight = 1;
	m_iLengthSections = 1;
	m_iWidthSections = 1;
	m_iHeightSections = 1;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/4/2011
**/
Box::~Box()
{
}

float Box::Length() {return m_fltLength;}

void Box::Length(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "BoxSize.Length");
	if(bUseScaling)
		m_fltLength = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLength = fltVal;

	Resize();
}

float Box::Width() {return m_fltWidth;}

void Box::Width(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "BoxSize.Width");
	if(bUseScaling)
		m_fltWidth = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltWidth = fltVal;

	Resize();
}

float Box::Height() {return m_fltHeight;}

void Box::Height(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "BoxSize.Height");
	if(bUseScaling)
		m_fltHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltHeight = fltVal;

	Resize();
}

/**
\brief	Sets the number of length sections.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Box::LengthSections(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "BoxSize.LengthSections");
	m_iLengthSections = iVal;

	Resize();
}

/**
\brief	Gets the length sections.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Box::LengthSections() {return m_iLengthSections;}

/**
\brief	Sets the number of width sections.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Box::WidthSections(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "BoxSize.WidthSections");
	m_iWidthSections = iVal;

	Resize();
}

/**
\brief	Gets the width sections.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Box::WidthSections() {return m_iWidthSections;}

/**
\brief	Sets the number of height sections.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Box::HeightSections(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "BoxSize.HeightSections");
	m_iHeightSections = iVal;

	Resize();
}

/**
\brief	Gets the height sections.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Box::HeightSections() {return m_iHeightSections;}

/**
\brief	Gets the length segment size.

\author	dcofer
\date	4/17/2011

\return	size of segment.
**/
float Box::LengthSegmentSize() {return m_fltLength/(float) m_iLengthSections;}

/**
\brief	Gets the width segment size.

\author	dcofer
\date	4/17/2011

\return	size of segment.
**/
float Box::WidthSegmentSize() {return m_fltWidth/(float) m_iWidthSections;}

/**
\brief	Gets the height segment size.

\author	dcofer
\date	4/17/2011

\return	size of segment.
**/
float Box::HeightSegmentSize() {return m_fltHeight/(float) m_iHeightSections;}


bool Box::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, false))
		return true;

	if(strType == "LENGTH")
	{
		Length((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "WIDTH")
	{
		Width((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "HEIGHT")
	{
		Height((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "LENGTHSECTIONS")
	{
		LengthSections(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "WIDTHSECTIONS")
	{
		WidthSections(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "HEIGHTSECTIONS")
	{
		HeightSections(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Box::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RigidBody::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Length", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Width", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Height", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LengthSections", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("WidthSections", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("HeightSections", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

void Box::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	Length(oXml.GetChildFloat("Length", m_fltLength));
	Width(oXml.GetChildFloat("Width", m_fltWidth));
	Height(oXml.GetChildFloat("Height", m_fltHeight));

	LengthSections(oXml.GetChildInt("LengthSections", m_iLengthSections));
	WidthSections(oXml.GetChildInt("WidthSections", m_iWidthSections));
	HeightSections(oXml.GetChildInt("HeightSections", m_iHeightSections));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
