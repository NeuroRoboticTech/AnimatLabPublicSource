/**
\file	Light.cpp

\brief	Implements a light object. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "Light.h"
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
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/2/2011
**/
Light::Light(void)
{
	m_fltRadius = 1;
	m_iLatitudeSegments = 50;
	m_iLongtitudeSegments = 50;
	m_iLightNum = 0;
	m_fltConstantAttenRatio = 0;
	m_fltLinearAttenDistance = 0;
	m_fltQuadraticAttenDistance = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
Light::~Light(void)
{
}

#pragma region AccessorMutators

/**
\brief	Called to collect any body data for this part. 

\author	dcofer
\date	3/2/2011
**/
void Light::UpdateData()
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_CollectData();
}

/**
\brief	Called when this object has been resized.

\details This method is called when an item is resized. It is overloaded in the derived class and
allows that child class to perform any necessary graphics/physics calls for the resize event. 

\author	dcofer
\date	3/2/2011
**/
void Light::Resize() 
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_Resize();
}

float Light::Radius() {return m_fltRadius;}

void Light::Radius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Light.Radius");
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;

	Resize();
}

/**
\brief	Latitude segments.

\author	dcofer
\date	7/11/2011

\param	iVal	The new value.
**/
void Light::LatitudeSegments(int iVal) 
{
	Std_IsAboveMin((int) 10, iVal, TRUE, "Light.LatitudeSegments", TRUE);
	m_iLatitudeSegments = iVal;
	Resize();
}

/**
\brief	Gets the latitude segments.

\author	dcofer
\date	7/11/2011

\return	segments.
**/
int Light::LatitudeSegments() {return m_iLatitudeSegments;}

/**
\brief	Longtitude segments.

\author	dcofer
\date	7/11/2011

\param	iVal	The new value.
**/
void Light::LongtitudeSegments(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, TRUE, "Light.LongtitudeSegments", TRUE);
	m_iLongtitudeSegments = iVal;
	Resize();
}

/**
\brief	Gets the longtitude segments.

\author	dcofer
\date	7/11/2011

\return	segments.
**/
int Light::LongtitudeSegments() {return m_iLongtitudeSegments;}

#pragma endregion

void Light::Selected(BOOL bValue, BOOL bSelectMultiple)
{
	AnimatBase::Selected(bValue, bSelectMultiple);
	MovableItem::Selected(bValue, bSelectMultiple);
}

/**
\brief	Sets the light number.

\author	dcofer
\date	7/11/2011

\param	iVal	The new value.
**/
void Light::LightNumber(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, TRUE, "Light.LightNumber", TRUE);
	m_iLightNum = iVal;
}

/**
\brief	Gets the light number.

\author	dcofer
\date	7/11/2011

\return	number.
**/
int Light::LightNumber() {return m_iLightNum;}

void Light::ConstantAttenRatio(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "Receptive Field Index");
	m_fltConstantAttenRatio = fltVal;
	Resize();
}

float Light::ConstantAttenRatio() {return m_fltConstantAttenRatio;}

void Light::LinearAttenDistance(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Light.LinearAttenDistance", TRUE);
	if(bUseScaling)
		m_fltLinearAttenDistance = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLinearAttenDistance = fltVal;

	Resize();
}

float Light::LinearAttenDistance() {return m_fltLinearAttenDistance;}

void Light::QuadraticAttenDistance(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Light.QuadraticAttenDistance", TRUE);
	if(bUseScaling)
		m_fltQuadraticAttenDistance = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltQuadraticAttenDistance = fltVal;

	Resize();
}

float Light::QuadraticAttenDistance() {return m_fltQuadraticAttenDistance;}

/**
\brief	Called when the visual selection mode changed in GUI.

\details In the GUI the user can select several different types of visual selection modes This
method is called any time that the user switches the selection mode in the GUI. This allows us to
change the current Alpha value of the objects so the display is correct. 

\author	dcofer
\date	3/2/2011

\param	iNewMode	The new VisualSelectionMode. 
**/
void Light::VisualSelectionModeChanged(int iNewMode)
{
	MovableItem::VisualSelectionModeChanged(iNewMode);
}


#pragma region DataAccesMethods

void Light::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	m_lpMovableSim = lpSim;
}

float *Light::GetDataPointer(string strDataType)
{
	return MovableItem::GetDataPointer(strDataType);
}

BOOL Light::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, FALSE))
		return true;

	if(MovableItem::SetData(strType, strValue, FALSE))
		return true;

	if(strType == "RADIUS")
	{
		Radius(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LATITUDESEGMENTS")
	{
		LatitudeSegments(atoi(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LONGTITUDESEGMENTS")
	{
		LongtitudeSegments(atoi(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CONSTANTATTENUATION")
	{
		ConstantAttenRatio(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LINEARATTENUATIONDISTANCE")
	{
		LinearAttenDistance(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "QUADRATICATTENUATIONDISTANCE")
	{
		QuadraticAttenDistance(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void Light::Create()
{
}

void Light::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);
	MovableItem::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	Radius(oXml.GetChildFloat("Radius", m_fltRadius));
	LatitudeSegments(oXml.GetChildInt("LatitudeSegments", m_iLatitudeSegments));
	LongtitudeSegments(oXml.GetChildInt("LongtitudeSegments", m_iLongtitudeSegments));
	ConstantAttenRatio(oXml.GetChildFloat("ConstantAttenuation", m_fltConstantAttenRatio));
	LinearAttenDistance(oXml.GetChildFloat("LinearAttenuationDistance", m_fltLinearAttenDistance));
	QuadraticAttenDistance(oXml.GetChildFloat("QuadraticAttenuationDistance", m_fltQuadraticAttenDistance));
	oXml.OutOfElem(); //OutOf RigidBody Element
}

	}			//Environment
}				//AnimatSim