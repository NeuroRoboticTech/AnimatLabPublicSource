/**
\file	Torus.cpp

\brief	Implements the Torus class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
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

void Torus::OutsideRadius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Torus.OutsideRadius");

	if(bUseScaling)
		m_fltOutsideRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltOutsideRadius = fltVal;

	Resize();
}

float Torus::InsideRadius() {return m_fltInsideRadius;}

void Torus::InsideRadius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Torus.InsideRadius");

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
	Std_IsAboveMin((int) 10, iVal, TRUE, "Torus.Sides", TRUE);
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
	Std_IsAboveMin((int) 10, iVal, TRUE, "Torus.Rings", TRUE);
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

BOOL Torus::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "OUTSIDERADIUS")
	{
		OutsideRadius(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "INSIDERADIUS")
	{
		InsideRadius(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SIDES")
	{
		Sides(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "RINGS")
	{
		Rings(atoi(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
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
