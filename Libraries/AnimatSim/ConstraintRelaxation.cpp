/**
\file	ConstraintRelaxation.cpp

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
ConstraintRelaxation::ConstraintRelaxation()
{
    m_iCoordinateID = 0;
	m_fltStiffness = 1000e3;
	m_fltDamping = 500;
	m_fltLoss = 0;
    m_bEnabled = false;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
ConstraintRelaxation::~ConstraintRelaxation()
{
}

/**
\fn	bool Enabled()

\brief	Gets whether the item is enabled or not. 

\author	dcofer
\date	3/1/2011

\return	true if it enabled, false if not. 
**/
bool ConstraintRelaxation::Enabled()
{return m_bEnabled;}

/**
\fn	void Enabled(bool bVal)

\brief	Enables the item. 

\author	dcofer
\date	3/1/2011

\param	bVal	true to enable, false to disable. 
**/
void ConstraintRelaxation::Enabled(bool bVal)
{
	m_bEnabled = bVal;
	SetRelaxationProperties();
}

/**
\brief	Gets the ID of the coordiante that this relaxation is associated with. 

\author	dcofer
\date	3/1/2011

\return	integer ID. 
**/
int ConstraintRelaxation::CoordinateID()
{return m_iCoordinateID;}

/**
\fn	void Enabled(bool bVal)

\brief	Enables the item. 

\author	dcofer
\date	3/1/2011

\param	bVal	true to enable, false to disable. 
**/
void ConstraintRelaxation::CoordinateID(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "CoordinateID", true);

	m_iCoordinateID = iVal;
	SetRelaxationProperties();
}

/**
\brief	Gets the Stiffness for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\return	Stiffness.
**/
float ConstraintRelaxation::Stiffness() {return m_fltStiffness;}

/**
\brief	Sets the Stiffness for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void ConstraintRelaxation::Stiffness(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Stiffness", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseMassUnits();
	
	m_fltStiffness = fltVal;
	SetRelaxationProperties();
}

/**
\brief	Gets the damping for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\return	damping value.
**/
float ConstraintRelaxation::Damping() {return m_fltDamping;}

/**
\brief	Sets the damping for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void ConstraintRelaxation::Damping(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Damping", true);

	if(bUseScaling)
		fltVal = fltVal/m_lpSim->DisplayMassUnits();

	m_fltDamping = fltVal;
	SetRelaxationProperties();
}

/**
\brief	Gets the primary linear slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\return	slip value.
**/
float ConstraintRelaxation::Loss() {return m_fltLoss;}

/**
\brief	Sets the primary linear slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void ConstraintRelaxation::Loss(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Loss", true);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltLoss = fltVal;
	SetRelaxationProperties();
}

/**
\brief	This takes the default values defined in the constructor and scales them according to
the distance and mass units to be acceptable values.

\author	dcofer
\date	3/23/2011
**/
void ConstraintRelaxation::CreateDefaultUnits()
{
	m_fltStiffness = 1e-7f;
	m_fltDamping = 5e4f;
	m_fltLoss = 0;

	//scale the varios units to be consistent
	//Friction coefficients are unitless
	m_fltStiffness *= m_lpSim->InverseMassUnits(); 
	m_fltDamping *= m_lpSim->InverseMassUnits();
	m_fltLoss *= m_lpSim->MassUnits();  //Slip units are s/Kg
}

bool ConstraintRelaxation::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(strType == "COORDINATEID")
	{
		CoordinateID(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "STIFFNESS")
	{
		Stiffness((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "DAMPING")
	{
		Damping((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "LOSS")
	{
		Loss((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void ConstraintRelaxation::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("CoordinateID");
	aryTypes.Add("Integer");

	aryNames.Add("Enabled");
	aryTypes.Add("Boolean");

	aryNames.Add("Damping");
	aryTypes.Add("Float");

	aryNames.Add("Loss");
	aryTypes.Add("Float");

    aryNames.Add("Stiffness");
	aryTypes.Add("Float");
}

void ConstraintRelaxation::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into ConstraintRelaxation Element

    CoordinateID(oXml.GetChildInt("CoordinateID", m_iCoordinateID));
    Enabled(oXml.GetChildBool("Enabled", m_bEnabled));
	Stiffness(oXml.GetChildFloat("Stiffness", m_fltStiffness));
	Damping(oXml.GetChildFloat("Damping", m_fltDamping));
	Loss(oXml.GetChildFloat("Loss", m_fltLoss));

    oXml.OutOfElem(); //OutOf ConstraintRelaxation Element

}

	}			// Visualization
}				//VortexAnimatSim
