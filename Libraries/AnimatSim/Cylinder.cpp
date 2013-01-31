/**
\file	Cylinder.cpp

\brief	Implements the cylinder class. 
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
#include "BodyPart.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Cylinder.h"
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
\date	3/10/2011
**/
Cylinder::Cylinder()
{
	m_fltRadius = 1;
	m_fltHeight = 1;
	m_iSides = 10;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Cylinder::~Cylinder()
{

}

float Cylinder::Radius() {return m_fltRadius;}

void Cylinder::Radius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Cylinder.Radius");
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;

	Resize();
}


float Cylinder::Height() {return m_fltHeight;}

void Cylinder::Height(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Cylinder.Height");
	if(bUseScaling)
		m_fltHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltHeight = fltVal;

	Resize();
}
/**
\brief	Sets  the number of sides used to draw the cylinder.

\author	dcofer
\date	4/17/2011

\param	iVal	The new value.
**/
void Cylinder::Sides(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, TRUE, "Cone.Sides", TRUE);
	m_iSides = iVal;

	Resize();
}

/**
\brief	Gets the number of sides used to draw the cylinder.

\author	dcofer
\date	4/17/2011

\return	sections.
**/
int Cylinder::Sides() {return m_iSides;}

BOOL Cylinder::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "RADIUS")
	{
		Radius(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "HEIGHT")
	{
		Height(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SIDES")
	{
		Sides(atoi(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Cylinder::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	RigidBody::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Radius");
	aryTypes.Add("Float");

	aryNames.Add("Height");
	aryTypes.Add("Float");

	aryNames.Add("Sides");
	aryTypes.Add("Float");
}

void Cylinder::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	Radius(oXml.GetChildFloat("Radius", m_fltRadius));
	Height(oXml.GetChildFloat("Height"), m_fltHeight);
	Sides(oXml.GetChildInt("Sides", m_iSides));
	oXml.OutOfElem(); //OutOf RigidBody Element
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
