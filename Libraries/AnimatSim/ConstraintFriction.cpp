/**
\file	ConstraintFriction.cpp

\brief	Implements the material pair class.
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
#include "Sensor.h"
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
\date	3/22/2011
**/
ConstraintFriction::ConstraintFriction()
{
	m_fltCoefficient = 1;
	m_fltMaxForce = 10;
	m_fltLoss = 0;
    m_bEnabled = TRUE;
    m_bProportional = TRUE;
    m_fltStaticFrictionScale = 1;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
ConstraintFriction::~ConstraintFriction()
{
}

/**
\fn	BOOL Enabled()

\brief	Gets whether the item is enabled or not. 

\author	dcofer
\date	3/1/2011

\return	true if it enabled, false if not. 
**/
BOOL ConstraintFriction::Enabled()
{return m_bEnabled;}

/**
\fn	void Enabled(BOOL bVal)

\brief	Enables the item. 

\author	dcofer
\date	3/1/2011

\param	bVal	true to enable, false to disable. 
**/
void ConstraintFriction::Enabled(BOOL bVal)
{
	m_bEnabled = bVal;
	SetFrictionProperties();
}

/**
\brief	Gets the Coefficient of friction for this constraint.

\author	dcofer
\date	3/23/2011

\return	Coefficient.
**/
float ConstraintFriction::Coefficient() {return m_fltCoefficient;}

/**
\brief	Sets the Coefficient of friction for this constraint.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void ConstraintFriction::Coefficient(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Coefficient", TRUE);
	
	m_fltCoefficient = fltVal;
	SetFrictionProperties();
}

/**
\brief	Gets the MaxForce the friction can apply.

\author	dcofer
\date	3/23/2011

\return	MaxForce value.
**/
float ConstraintFriction::MaxForce() {return m_fltMaxForce;}

/**
\brief	Sets the MaxForce the friction can apply.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void ConstraintFriction::MaxForce(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "MaxForce", TRUE);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltMaxForce = fltVal;
	SetFrictionProperties();
}

/**
\brief	Gets the velocity loss for this constraint.

\details Velocity loss for this constraint.

\author	dcofer
\date	3/23/2011

\return	loss value.
**/
float ConstraintFriction::Loss() {return m_fltLoss;}

/**
\brief	Sets the velocity loss for this constraint.

\details the velocity loss for this constraint.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void ConstraintFriction::Loss(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Loss", TRUE);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltLoss = fltVal;
	SetFrictionProperties();
}

/**
\brief	Gets whether the friction force should be scaled based on the amount of force applied to the joint

\author	dcofer
\date	3/23/2011

\return	proportional flag.
**/
BOOL ConstraintFriction::Proportional() {return m_bProportional;}

/**
\brief	Sets whether the friction force should be scaled based on the amount of force applied to the joint

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void ConstraintFriction::Proportional(BOOL bVal) 
{
	m_bProportional = bVal;
	SetFrictionProperties();
}

/**
\brief	Gets the ratio between static and dynamic friction coefficients.

\author	dcofer
\date	3/23/2011

\return	Coefficient.
**/
float ConstraintFriction::StaticFrictionScale() {return m_fltCoefficient;}

/**
\brief	Sets the ratio between static and dynamic friction coefficients.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void ConstraintFriction::StaticFrictionScale(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "StaticFrictionScale", TRUE);
	
	m_fltStaticFrictionScale = fltVal;
	SetFrictionProperties();
}

/**
\brief	This takes the default values defined in the constructor and scales them according to
the distance and mass units to be acceptable values.

\author	dcofer
\date	3/23/2011
**/
void ConstraintFriction::CreateDefaultUnits()
{
	m_fltCoefficient = 1;
	m_fltMaxForce = 10;
	m_fltLoss = 0;
    m_bProportional = TRUE;
    m_bEnabled = TRUE;
    m_fltStaticFrictionScale = 1;

	//scale the varios units to be consistent
	//Friction coefficients are unitless
	m_fltMaxForce *= m_lpSim->InverseMassUnits();
	m_fltLoss *= m_lpSim->MassUnits();  //Slip units are s/Kg
}

BOOL ConstraintFriction::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "COEFFICIENT")
	{
		Coefficient(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "MAXFORCE")
	{
		MaxForce(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "LOSS")
	{
		Loss(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "PROPORTIONAL")
	{
		Proportional(Std_ToBool(strValue));
		return TRUE;
	}
	
	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return TRUE;
	}
	
	if(strType == "STATICFRICTIONSCALE")
	{
		StaticFrictionScale(atof(strValue.c_str()));
		return TRUE;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void ConstraintFriction::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

    aryNames.Add("Coefficient");
	aryTypes.Add("Float");

    aryNames.Add("Enabled");
	aryTypes.Add("Boolean");

	aryNames.Add("MaxForce");
	aryTypes.Add("Float");

	aryNames.Add("Loss");
	aryTypes.Add("Float");

    aryNames.Add("Proportional");
	aryTypes.Add("Boolean");

	aryNames.Add("StaticFrictionScale");
	aryTypes.Add("Float");
}

void ConstraintFriction::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into ConstraintFriction Element

    Enabled(oXml.GetChildBool("Enabled", m_bEnabled));
	Coefficient(oXml.GetChildFloat("Coefficient", m_fltCoefficient));
	MaxForce(oXml.GetChildFloat("MaxForce", m_fltMaxForce));
	Loss(oXml.GetChildFloat("Loss", m_fltLoss));
    Proportional(oXml.GetChildBool("Proportional", m_bProportional));
	StaticFrictionScale(oXml.GetChildFloat("StaticFrictionScale", m_fltStaticFrictionScale));

    oXml.OutOfElem(); //OutOf ConstraintFriction Element

}

	}			// Visualization
}				//VortexAnimatSim
