/**
\file	Light.cpp

\brief	Implements a light object. 
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
	m_bEnabled = true;
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
\brief	Tells whether this light is enabled.

\author	dcofer
\date	2/24/2011

\return	true if it enabled, false if not. 
**/
bool Light::Enabled() {return m_bEnabled;}

/**
\brief	Enables the node.

\details Some types of nodes can be enabled/disabled. This sets the enabled state of the object. 

\author	dcofer
\date	2/24/2011

\param	bValue	true to enable. 
**/
void Light::Enabled(bool bValue) 
{
	m_bEnabled = bValue;
}

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

void Light::Radius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Light.Radius");
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
	Std_IsAboveMin((int) 10, iVal, true, "Light.LatitudeSegments", true);
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
	Std_IsAboveMin((int) 10, iVal, true, "Light.LongtitudeSegments", true);
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

void Light::Selected(bool bValue, bool bSelectMultiple)
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
	Std_IsAboveMin((int) 0, iVal, true, "Light.LightNumber", true);
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
	Std_InValidRange((float) 0, (float) 1, fltVal, true, "Receptive Field Index");
	m_fltConstantAttenRatio = fltVal;
	Resize();
}

float Light::ConstantAttenRatio() {return m_fltConstantAttenRatio;}

void Light::LinearAttenDistance(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Light.LinearAttenDistance", true);
	if(bUseScaling)
		m_fltLinearAttenDistance = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLinearAttenDistance = fltVal;

	Resize();
}

float Light::LinearAttenDistance() {return m_fltLinearAttenDistance;}

void Light::QuadraticAttenDistance(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Light.QuadraticAttenDistance", true);
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

void Light::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	m_lpMovableSim = lpSim;
}

float *Light::GetDataPointer(const string &strDataType)
{
	return MovableItem::GetDataPointer(strDataType);
}

bool Light::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(MovableItem::SetData(strType, strValue, false))
		return true;

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}

	if(strType == "RADIUS")
	{
		Radius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "LATITUDESEGMENTS")
	{
		LatitudeSegments(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "LONGTITUDESEGMENTS")
	{
		LongtitudeSegments(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "CONSTANTATTENUATION")
	{
		ConstantAttenRatio((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "LINEARATTENUATIONDISTANCE")
	{
		LinearAttenDistance((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "QUADRATICATTENUATIONDISTANCE")
	{
		QuadraticAttenDistance((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Light::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);
	MovableItem::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Enabled");
	aryTypes.Add("Boolean");

	aryNames.Add("Radius");
	aryTypes.Add("Float");

	aryNames.Add("LatitudeSegments");
	aryTypes.Add("Integer");

	aryNames.Add("LongtitudeSegments");
	aryTypes.Add("Integer");

	aryNames.Add("ConstantAttenuation");
	aryTypes.Add("Float");

	aryNames.Add("LinearAttenuationDistance");
	aryTypes.Add("Float");

	aryNames.Add("QuadraticAttenuationDistance");
	aryTypes.Add("Float");
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
	Enabled(oXml.GetChildBool("Enabled", m_bEnabled));
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