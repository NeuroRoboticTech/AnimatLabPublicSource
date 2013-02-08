/**
\file	MaterialPair.cpp

\brief	Implements the material pair class.
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
MaterialPair::MaterialPair()
{
	m_fltFrictionPrimary = 1;
	m_fltFrictionSecondary = 1;
	m_fltMaxFrictionPrimary = 500;
	m_fltMaxFrictionSecondary = 500;
	m_fltCompliance = 1e-7f;
	m_fltDamping = 5e4f;
	m_fltRestitution = 0;
	m_fltSlipPrimary = 0;
	m_fltSlipSecondary= 0;
	m_fltSlidePrimary= 0;
	m_fltSlideSecondary= 0;
	m_fltMaxAdhesive = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
MaterialPair::~MaterialPair()
{
}

/**
\brief	Gets the unique name for material 1.

\author	dcofer
\date	3/23/2011

\return	string name.
**/
string MaterialPair::Material1ID() {return m_strMaterial1ID;}

/**
\brief	Sets the unique name for material 1.

\author	dcofer
\date	3/23/2011

\param	strMat	The string name. 
**/
void MaterialPair::Material1ID(string strMat) 
{
	if(Std_IsBlank(strMat))
		THROW_ERROR(Al_Err_lMaterialPairBlank, Al_Err_strMaterialPairBlank);
	m_strMaterial1ID = Std_ToUpper(strMat);
}

/**
\brief	Gets the unique name for material 2.

\author	dcofer
\date	3/23/2011

\return	String name.
**/
string MaterialPair::Material2ID() {return m_strMaterial2ID;}

/**
\brief	Sets the unique name for material 2.

\author	dcofer
\date	3/23/2011

\param	strMat	The string name. 
**/
void MaterialPair::Material2ID(string strMat) 
{
	if(Std_IsBlank(strMat))
		THROW_ERROR(Al_Err_lMaterialPairBlank, Al_Err_strMaterialPairBlank);
	m_strMaterial2ID = Std_ToUpper(strMat);
}

/**
\brief	Gets the primary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float MaterialPair::FrictionPrimary() {return m_fltFrictionPrimary;}

/**
\brief	Sets the primary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void MaterialPair::FrictionPrimary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionPrimary", TRUE);
	
	m_fltFrictionPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float MaterialPair::FrictionSecondary() {return m_fltFrictionSecondary;}

/**
\brief	Sets the secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void MaterialPair::FrictionSecondary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionSecondary", TRUE);
	m_fltFrictionSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum primary friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float MaterialPair::MaxFrictionPrimary() {return m_fltMaxFrictionPrimary;}

/**
\brief	Sets the maximum primary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::MaxFrictionPrimary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "MaxFrictionPrimary", TRUE);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltMaxFrictionPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum secondary friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float MaterialPair::MaxFrictionSecondary() {return m_fltMaxFrictionSecondary;}

/**
\brief	Sets the maximum secondary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::MaxFrictionSecondary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "MaxFrictionSecondary", TRUE);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltMaxFrictionSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the primary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\return	slip value.
**/
float MaterialPair::SlipPrimary() {return m_fltSlipPrimary;}

/**
\brief	Sets the primary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::SlipPrimary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlipPrimary", TRUE);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltSlipPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the secondary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\return	slip value.
**/
float MaterialPair::SlipSecondary() {return m_fltSlipSecondary;}


/**
\brief	Sets the secondary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::SlipSecondary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlipSecondary", TRUE);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltSlipSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the primary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\return	slide value.
**/
float MaterialPair::SlidePrimary() {return m_fltSlidePrimary;}

/**
\brief	Sets the primary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::SlidePrimary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlidePrimary", TRUE);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s

	m_fltSlidePrimary = fltVal;
	SetMaterialProperties();
}


/**
\brief	Gets the secondary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\return	slide value.
**/
float MaterialPair::SlideSecondary() {return m_fltSlideSecondary;}


/**
\brief	Sets the secondary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::SlideSecondary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlideSecondary", TRUE);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s

	m_fltSlideSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the compliance for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\return	compliance.
**/
float MaterialPair::Compliance() {return m_fltCompliance;}

/**
\brief	Sets the compliance for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::Compliance(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Compliance", TRUE);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Compliance units are m/N or s^2/Kg
	
	m_fltCompliance = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the damping for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\return	damping value.
**/
float MaterialPair::Damping() {return m_fltDamping;}

/**
\brief	Sets the damping for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::Damping(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Damping", TRUE);

	if(bUseScaling)
		fltVal = fltVal/m_lpSim->DisplayMassUnits();

	m_fltDamping = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the restitution for collisions between RigidBodies with these two materials.

\details When a collision occurs between two rigid bodies, the impulse corresponding to the force is equal to the
total change in momentum that each body undergoes. This change of momentum is affected by the degree
of resilience of each body, that is, the extent to which energy is diffused.<br>
The coefficient of restitution is a parameter representing the degree of resilience of a particular material pair.
To make simulations more efficient, it is best to set a restitution threshold as well. Impacts that measure less
than the threshold will be ignored, to avoid jitter in the simulation. Small impulses do not add to the realism
of most simulations.

\author	dcofer
\date	3/23/2011

\return	Restitution value.
**/
float MaterialPair::Restitution() {return m_fltRestitution;}

/**
\brief	Sets the restitution for collisions between RigidBodies with these two materials.

\details When a collision occurs between two rigid bodies, the impulse corresponding to the force is equal to the
total change in momentum that each body undergoes. This change of momentum is affected by the degree
of resilience of each body, that is, the extent to which energy is diffused.<br>
The coefficient of restitution is a parameter representing the degree of resilience of a particular material pair.
To make simulations more efficient, it is best to set a restitution threshold as well. Impacts that measure less
than the threshold will be ignored, to avoid jitter in the simulation. Small impulses do not add to the realism
of most simulations.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void MaterialPair::Restitution(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "Restitution");
	m_fltRestitution = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum adhesive for collisions between RigidBodies with these two materials.

\details Adhesive force allows objects to stick together, as if they were glued. This property provides the minimal
force needed to separate the two objects.

\author	dcofer
\date	3/23/2011

\return	Maximum adhesive force.
**/
float MaterialPair::MaxAdhesive() {return m_fltMaxAdhesive;}

/**
\brief	Sets the maximum adhesive for collisions between RigidBodies with these two materials.

\details Adhesive force allows objects to stick together, as if they were glued. This property provides the minimal
force needed to separate the two objects.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialPair::MaxAdhesive(float fltVal, BOOL bUseScaling) 
{
	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force.

	m_fltMaxAdhesive = fltVal;
	SetMaterialProperties();
}

/**
\brief	This takes the default values defined in the constructor and scales them according to
the distance and mass units to be acceptable values.

\author	dcofer
\date	3/23/2011
**/
void MaterialPair::CreateDefaultUnits()
{
	m_fltFrictionPrimary = 1;
	m_fltFrictionSecondary = 1;
	m_fltMaxFrictionPrimary = 500;
	m_fltMaxFrictionSecondary = 500;
	m_fltCompliance = 1e-7f;
	m_fltDamping = 5e4f;
	m_fltRestitution = 0;
	m_fltSlipPrimary = 0;
	m_fltSlipSecondary= 0;
	m_fltSlidePrimary= 0;
	m_fltSlideSecondary= 0;
	m_fltMaxAdhesive = 0;

	//scale the varios units to be consistent
	//Friction coefficients are unitless
	m_fltMaxFrictionPrimary *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 
	m_fltMaxFrictionSecondary *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 
	m_fltCompliance *= m_lpSim->MassUnits();  //Compliance units are m/N or s^2/Kg
	m_fltDamping *= m_lpSim->InverseMassUnits();
	m_fltSlipPrimary *= m_lpSim->MassUnits();  //Slip units are s/Kg
	m_fltSlipSecondary *= m_lpSim->MassUnits();  
	m_fltSlidePrimary *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s
	m_fltSlidePrimary *= m_lpSim->InverseDistanceUnits();
	m_fltMaxAdhesive *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force.
}

BOOL MaterialPair::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "FRICTIONPRIMARY")
	{
		FrictionPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONSECONDARY")
	{
		FrictionSecondary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "MAXFRICTIONPRIMARY")
	{
		MaxFrictionPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "MAXFRICTIONSECONDARY")
	{
		MaxFrictionSecondary(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "COMPLIANCE")
	{
		Compliance(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "DAMPING")
	{
		Damping(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "RESTITUTION")
	{
		Restitution(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "PRIMARYSLIP")
	{
		SlipPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strDataType == "SECONDARYSLIP")
	{
		SlipSecondary(atof(strValue.c_str()));
		return true;
	}

	if(strType == "PRIMARYSLIDE")
	{
		SlidePrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strDataType == "SECONDARYSLIDE")
	{
		SlideSecondary(atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXADHESION")
	{
		MaxAdhesive(atof(strValue.c_str()));
		return TRUE;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void MaterialPair::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("FrictionPrimary");
	aryTypes.Add("Float");

	aryNames.Add("FrictionSecondary");
	aryTypes.Add("Float");

	aryNames.Add("MaxFrictionPrimary");
	aryTypes.Add("Float");

	aryNames.Add("Compliance");
	aryTypes.Add("Float");

	aryNames.Add("Damping");
	aryTypes.Add("Float");

	aryNames.Add("PrimarySlip");
	aryTypes.Add("Float");

	aryNames.Add("SecondarySlip");
	aryTypes.Add("Float");

	aryNames.Add("PrimarySlide");
	aryTypes.Add("Float");

	aryNames.Add("SecondarySlide");
	aryTypes.Add("Float");

	aryNames.Add("MaxAdhesion");
	aryTypes.Add("Float");
}

void MaterialPair::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into MaterialPair Element

	Material1ID(oXml.GetChildString("Material1ID"));
	Material2ID(oXml.GetChildString("Material2ID"));

	FrictionPrimary(oXml.GetChildFloat("FrictionPrimary", m_fltFrictionPrimary));
	FrictionSecondary(oXml.GetChildFloat("FrictionSecondary", m_fltFrictionSecondary));
	MaxFrictionPrimary(oXml.GetChildFloat("MaxFrictionPrimary", m_fltMaxFrictionPrimary));
	MaxFrictionSecondary(oXml.GetChildFloat("MaxFrictionSecondary", m_fltMaxFrictionSecondary));
	Compliance(oXml.GetChildFloat("Compliance", m_fltCompliance));
	Damping(oXml.GetChildFloat("Damping", m_fltDamping));
	Restitution(oXml.GetChildFloat("Restitution", m_fltRestitution));
	SlipPrimary(oXml.GetChildFloat("SlipPrimary", m_fltSlipPrimary));
	SlipSecondary(oXml.GetChildFloat("SlipSecondary", m_fltSlipSecondary));
	SlidePrimary(oXml.GetChildFloat("SlidePrimary", m_fltSlidePrimary));
	SlideSecondary(oXml.GetChildFloat("SlideSecondary", m_fltSlideSecondary));
	MaxAdhesive(oXml.GetChildFloat("MaxAdhesive", m_fltMaxAdhesive));

	oXml.OutOfElem(); //OutOf MaterialPair Element

}

	}			// Visualization
}				//VortexAnimatSim
