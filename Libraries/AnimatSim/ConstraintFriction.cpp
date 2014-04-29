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
    m_bEnabled = true;
    m_bProportional = true;
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
\fn	bool Enabled()

\brief	Gets whether the item is enabled or not. 

\author	dcofer
\date	3/1/2011

\return	true if it enabled, false if not. 
**/
bool ConstraintFriction::Enabled()
{return m_bEnabled;}

/**
\fn	void Enabled(bool bVal)

\brief	Enables the item. 

\author	dcofer
\date	3/1/2011

\param	bVal	true to enable, false to disable. 
**/
void ConstraintFriction::Enabled(bool bVal)
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
	Std_IsAboveMin((float) 0, fltVal, true, "Coefficient", true);
	
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
void ConstraintFriction::MaxForce(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "MaxForce", true);

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
void ConstraintFriction::Loss(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Loss", true);

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
bool ConstraintFriction::Proportional() {return m_bProportional;}

/**
\brief	Sets whether the friction force should be scaled based on the amount of force applied to the joint

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void ConstraintFriction::Proportional(bool bVal) 
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
	Std_IsAboveMin((float) 0, fltVal, true, "StaticFrictionScale", true);
	
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
    m_bProportional = true;
    m_bEnabled = true;
    m_fltStaticFrictionScale = 1;

	//scale the varios units to be consistent
	//Friction coefficients are unitless
	m_fltMaxForce *= m_lpSim->InverseMassUnits();
	m_fltLoss *= m_lpSim->MassUnits();  //Slip units are s/Kg
}

bool ConstraintFriction::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(strType == "COEFFICIENT")
	{
		Coefficient((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "MAXFORCE")
	{
		MaxForce((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "LOSS")
	{
		Loss((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "PROPORTIONAL")
	{
		Proportional(Std_ToBool(strValue));
		return true;
	}
	
	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}
	
	if(strType == "STATICFRICTIONSCALE")
	{
		StaticFrictionScale((float) atof(strValue.c_str()));
		return true;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void ConstraintFriction::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Coefficient", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Enabled", AnimatPropertyType::Boolean, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("MaxForce", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Loss", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Proportional", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StaticFrictionScale", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
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
