// VsMaterialType.cpp: implementation of the VsMaterialType class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsMaterialType::VsMaterialType()
{
	m_fltFrictionLinearPrimary = 1;
	m_fltFrictionLinearSecondary = 1;
	m_fltFrictionAngularNormal = 0;
	m_fltFrictionAngularPrimary = 0;
	m_fltFrictionAngularSecondary = 0;
	m_fltFrictionLinearPrimaryMax = 500;
	m_fltFrictionLinearSecondaryMax = 500;
	m_fltFrictionAngularNormalMax = 500;
	m_fltFrictionAngularPrimaryMax = 500;
	m_fltFrictionAngularSecondaryMax = 500;
	m_fltCompliance = 1e-7f;
	m_fltDamping = 5e4f;
	m_fltRestitution = 0;
	m_fltSlipLinearPrimary = 0;
	m_fltSlipLinearSecondary= 0;
   	m_fltSlipAngularNormal= 0;
	m_fltSlipAngularPrimary= 0;
	m_fltSlipAngularSecondary= 0;
    m_fltSlideLinearPrimary= 0;
	m_fltSlideLinearSecondary= 0;
	m_fltSlideAngularNormal= 0;
	m_fltSlideAngularPrimary= 0;
	m_fltSlideAngularSecondary= 0;
	m_fltMaxAdhesive = 0;

	m_vxMaterialTable = NULL;
    m_vxMaterial = NULL;
}

VsMaterialType::~VsMaterialType()
{
}


/**
\brief	Gets the primary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float VsMaterialType::FrictionLinearPrimary() {return m_fltFrictionLinearPrimary;}

/**
\brief	Sets the primary linear friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void VsMaterialType::FrictionLinearPrimary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionLinearPrimary", true);
	
	m_fltFrictionLinearPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the secondary linear friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float VsMaterialType::FrictionLinearSecondary() {return m_fltFrictionLinearSecondary;}

/**
\brief	Sets the secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void VsMaterialType::FrictionLinearSecondary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionLinearSecondary", true);
	m_fltFrictionLinearSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular normal friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float VsMaterialType::FrictionAngularNormal() {return m_fltFrictionAngularNormal;}

/**
\brief	Sets the angular normal friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void VsMaterialType::FrictionAngularNormal(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionAngularNormal", true);
	
	m_fltFrictionAngularNormal = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular primary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float VsMaterialType::FrictionAngularPrimary() {return m_fltFrictionAngularPrimary;}

/**
\brief	Sets the angular primary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void VsMaterialType::FrictionAngularPrimary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionAngularPrimary", true);
	
	m_fltFrictionAngularPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float VsMaterialType::FrictionAngularSecondary() {return m_fltFrictionAngularSecondary;}

/**
\brief	Sets the angular secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void VsMaterialType::FrictionAngularSecondary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionAngularSecondary", true);
	
	m_fltFrictionAngularSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum primary friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float VsMaterialType::FrictionLinearPrimaryMax() {return m_fltFrictionLinearPrimaryMax;}

/**
\brief	Sets the maximum primary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::FrictionLinearPrimaryMax(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionLinearPrimaryMax", true);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltFrictionLinearPrimaryMax = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum secondary friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float VsMaterialType::FrictionLinearSecondaryMax() {return m_fltFrictionLinearSecondaryMax;}

/**
\brief	Sets the maximum secondary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::FrictionLinearSecondaryMax(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionLinearSecondaryMax", true);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltFrictionLinearSecondaryMax = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum angular normal friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float VsMaterialType::FrictionAngularNormalMax() {return m_fltFrictionAngularNormalMax;}

/**
\brief	Sets the maximum angular normal friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::FrictionAngularNormalMax(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionAngularNormalMax", true);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltFrictionAngularNormalMax = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum angular primary friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float VsMaterialType::FrictionAngularPrimaryMax() {return m_fltFrictionAngularPrimaryMax;}

/**
\brief	Sets the maximum angular primary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::FrictionAngularPrimaryMax(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionAngularPrimaryMax", true);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltFrictionAngularPrimaryMax = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum angular secondary friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float VsMaterialType::FrictionAngularSecondaryMax() {return m_fltFrictionAngularSecondaryMax;}

/**
\brief	Sets the maximum angular secondary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::FrictionAngularSecondaryMax(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionAngularSecondaryMax", true);

	if(bUseScaling)
		fltVal *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 

	m_fltFrictionAngularSecondaryMax = fltVal;
	SetMaterialProperties();
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
float VsMaterialType::SlipLinearPrimary() {return m_fltSlipLinearPrimary;}

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
void VsMaterialType::SlipLinearPrimary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlipLinearPrimary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltSlipLinearPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the secondary linear slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\return	slip value.
**/
float VsMaterialType::SlipLinearSecondary() {return m_fltSlipLinearSecondary;}


/**
\brief	Sets the secondary linear slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlipLinearSecondary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlipLinearSecondary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltSlipLinearSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular normal slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\return	slip value.
**/
float VsMaterialType::SlipAngularNormal() {return m_fltSlipAngularNormal;}


/**
\brief	Sets the angular normal slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlipAngularNormal(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlipAngularNormal", true);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltSlipAngularNormal = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular primary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\return	slip value.
**/
float VsMaterialType::SlipAngularPrimary() {return m_fltSlipAngularPrimary;}

/**
\brief	Sets the angular primary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlipAngularPrimary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlipAngularPrimary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltSlipAngularPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular secondary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\return	slip value.
**/
float VsMaterialType::SlipAngularSecondary() {return m_fltSlipAngularSecondary;}


/**
\brief	Sets the angular secondary slip value.

\details Contact slip allows a tangential loss at the contact position to be defined. For example, this is a useful
parameter to set for the interaction between a cylindrical wheel and a terrain where, without a minimum
amount of slip, the vehicle would have a hard time turning.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlipAngularSecondary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlipAngularSecondary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltSlipAngularSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the linear primary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\return	slide value.
**/
float VsMaterialType::SlideLinearPrimary() {return m_fltSlideLinearPrimary;}

/**
\brief	Sets the linear primary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlideLinearPrimary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlideLinearPrimary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s

	m_fltSlideLinearPrimary = fltVal;
	SetMaterialProperties();
}


/**
\brief	Gets the linear secondary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\return	slide value.
**/
float VsMaterialType::SlideLinearSecondary() {return m_fltSlideLinearSecondary;}


/**
\brief	Sets the linear secondary slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlideLinearSecondary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlideLinearSecondary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s

	m_fltSlideLinearSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular normal slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\return	slide value.
**/
float VsMaterialType::SlideAngularNormal() {return m_fltSlideAngularNormal;}


/**
\brief	Sets the angular normal slide value.

\details The contact sliding parameter allows a desired relative linear velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlideAngularNormal(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlideAngularNormal", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s

	m_fltSlideAngularNormal = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular primary slide value.

\details The contact sliding parameter allows a desired relative Angular velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\return	slide value.
**/
float VsMaterialType::SlideAngularPrimary() {return m_fltSlideAngularPrimary;}

/**
\brief	Sets the angular primary slide value.

\details The contact sliding parameter allows a desired relative Angular velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlideAngularPrimary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlideAngularPrimary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s

	m_fltSlideAngularPrimary = fltVal;
	SetMaterialProperties();
}


/**
\brief	Gets the angular secondary slide value.

\details The contact sliding parameter allows a desired relative Angular velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\return	slide value.
**/
float VsMaterialType::SlideAngularSecondary() {return m_fltSlideAngularSecondary;}


/**
\brief	Sets the angular secondary slide value.

\details The contact sliding parameter allows a desired relative Angular velocity to be specified between the colliding
parts at the contact position. A conveyor belt would be an example of an application. The belt part itself
would not be moving.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::SlideAngularSecondary(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "SlideAngularSecondary", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s

	m_fltSlideAngularSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the compliance for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\return	compliance.
**/
float VsMaterialType::Compliance() {return m_fltCompliance;}

/**
\brief	Sets the compliance for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::Compliance(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Compliance", true);

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
float VsMaterialType::Damping() {return m_fltDamping;}

/**
\brief	Sets the damping for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::Damping(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Damping", true);

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
float VsMaterialType::Restitution() {return m_fltRestitution;}

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
void VsMaterialType::Restitution(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, true, "Restitution");
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
float VsMaterialType::MaxAdhesive() {return m_fltMaxAdhesive;}

/**
\brief	Sets the maximum adhesive for collisions between RigidBodies with these two materials.

\details Adhesive force allows objects to stick together, as if they were glued. This property provides the minimal
force needed to separate the two objects.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void VsMaterialType::MaxAdhesive(float fltVal, bool bUseScaling) 
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
void VsMaterialType::CreateDefaultUnits()
{
	m_fltFrictionLinearPrimary = 1;
	m_fltFrictionLinearSecondary = 1;
	m_fltFrictionAngularNormal = 0;
	m_fltFrictionAngularPrimary = 0;
	m_fltFrictionAngularSecondary = 0;
	m_fltFrictionLinearPrimaryMax = 5;
	m_fltFrictionLinearSecondaryMax = 5;
	m_fltFrictionAngularNormalMax = 5;
	m_fltFrictionAngularPrimaryMax = 5;
	m_fltFrictionAngularSecondaryMax = 5;
	m_fltCompliance = 1e-7f;
	m_fltDamping = 5e4f;
	m_fltRestitution = 0;
	m_fltSlipLinearPrimary = 0;
	m_fltSlipLinearSecondary= 0;
   	m_fltSlipAngularNormal= 0;
	m_fltSlipAngularPrimary= 0;
	m_fltSlipAngularSecondary= 0;
    m_fltSlideLinearPrimary= 0;
	m_fltSlideLinearSecondary= 0;
	m_fltSlideAngularNormal= 0;
	m_fltSlideAngularPrimary= 0;
	m_fltSlideAngularSecondary= 0;
	m_fltMaxAdhesive = 0;

	//scale the varios units to be consistent
	//Friction coefficients are unitless
	m_fltFrictionLinearPrimaryMax *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 
	m_fltFrictionLinearSecondaryMax *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 
	m_fltCompliance *= m_lpSim->MassUnits();  //Compliance units are m/N or s^2/Kg
	m_fltDamping *= m_lpSim->InverseMassUnits();

	m_fltSlipLinearPrimary *= m_lpSim->MassUnits();  //Slip units are s/Kg
	m_fltSlipLinearSecondary *= m_lpSim->MassUnits();  
	m_fltSlipAngularNormal *= m_lpSim->MassUnits();  
   	m_fltSlipAngularPrimary *= m_lpSim->MassUnits();  
	m_fltSlipAngularSecondary *= m_lpSim->MassUnits();  

    m_fltSlideLinearPrimary *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s
	m_fltSlideLinearSecondary *= m_lpSim->InverseDistanceUnits();
	m_fltSlideAngularNormal *= m_lpSim->InverseDistanceUnits();
	m_fltSlideAngularPrimary *= m_lpSim->InverseDistanceUnits();
	m_fltSlideAngularSecondary *= m_lpSim->InverseDistanceUnits();

    m_fltMaxAdhesive *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force.
}

bool VsMaterialType::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(MaterialType::SetData(strType, strValue, false))
		return true;

	if(strType == "FRICTIONLINEARPRIMARY")
	{
		FrictionLinearPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONLINEARSECONDARY")
	{
		FrictionLinearSecondary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONANGULARNORMAL")
	{
		FrictionAngularNormal((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONANGULARPRIMARY")
	{
		FrictionAngularPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONANGULARSECONDARY")
	{
		FrictionAngularSecondary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONLINEARPRIMARYMAX")
	{
		FrictionLinearPrimaryMax((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONLINEARSECONDARYMAX")
	{
		FrictionLinearSecondaryMax((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONANGULARNORMALMAX")
	{
		FrictionAngularNormalMax((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONANGULARPRIMARYMAX")
	{
		FrictionAngularPrimaryMax((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONANGULARSECONDARYMAX")
	{
		FrictionAngularSecondaryMax((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "COMPLIANCE")
	{
		Compliance((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "DAMPING")
	{
		Damping((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "RESTITUTION")
	{
		Restitution((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "SLIPLINEARPRIMARY")
	{
		SlipLinearPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIPLINEARSECONDARY")
	{
		SlipLinearSecondary((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIPANGULARNORMAL")
	{
		SlipAngularNormal((float) atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "SLIPANGULARPRIMARY")
	{
		SlipAngularPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIPANGULARSECONDARY")
	{
		SlipAngularSecondary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "SLIDELINEARPRIMARY")
	{
		SlideLinearPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIDELINEARSECONDARY")
	{
		SlideLinearSecondary((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIDEANGULARNORMAL")
	{
		SlideAngularNormal((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "SLIDEANGULARPRIMARY")
	{
		SlideAngularPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIDEANGULARSECONDARY")
	{
		SlideAngularSecondary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXADHESION")
	{
		MaxAdhesive((float) atof(strValue.c_str()));
		return true;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void VsMaterialType::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	MaterialType::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("FrictionLinearPrimary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionLinearSecondary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionAngularNormal", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionAngularPrimary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionAngularSecondary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionLinearPrimaryMax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionLinearSecondaryMax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionAngularNormalMax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionAngularPrimaryMax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("FrictionAngularSecondaryMax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Compliance", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Damping", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Restitution", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlipLinearPrimary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlipLinearSecondary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlipAngularNormal", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlipAngularPrimary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlipAngularSecondary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlideLinearPrimary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlideLinearSecondary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlideAngularNormal", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlideAngularPrimary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SlideAngularSecondary", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxAdhesion", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

void VsMaterialType::Load(CStdXml &oXml)
{
	MaterialType::Load(oXml);

	oXml.IntoElem();  //Into MaterialType Element

	FrictionLinearPrimary(oXml.GetChildFloat("FrictionLinearPrimary", m_fltFrictionLinearPrimary));
	FrictionLinearSecondary(oXml.GetChildFloat("FrictionLinearSecondary", m_fltFrictionLinearSecondary));
	FrictionAngularNormal(oXml.GetChildFloat("FrictionAngularNormal", m_fltFrictionAngularNormal));
	FrictionAngularPrimary(oXml.GetChildFloat("FrictionAngularPrimary", m_fltFrictionAngularPrimary));
	FrictionAngularSecondary(oXml.GetChildFloat("FrictionAngularSecondary", m_fltFrictionAngularSecondary));
	FrictionLinearPrimaryMax(oXml.GetChildFloat("FrictionLinearPrimaryMax", m_fltFrictionLinearPrimaryMax));
	FrictionLinearSecondaryMax(oXml.GetChildFloat("FrictionLinearSecondaryMax", m_fltFrictionLinearSecondaryMax));
	FrictionAngularNormalMax(oXml.GetChildFloat("FrictionAngularNormalMax", m_fltFrictionAngularNormalMax));
	FrictionAngularPrimaryMax(oXml.GetChildFloat("FrictionAngularPrimaryMax", m_fltFrictionAngularPrimaryMax));
	FrictionAngularSecondaryMax(oXml.GetChildFloat("FrictionAngularSecondaryMax", m_fltFrictionAngularSecondaryMax));
	Compliance(oXml.GetChildFloat("Compliance", m_fltCompliance));
	Damping(oXml.GetChildFloat("Damping", m_fltDamping));
	Restitution(oXml.GetChildFloat("Restitution", m_fltRestitution));

	SlipLinearPrimary(oXml.GetChildFloat("SlipLinearPrimary", m_fltSlipLinearPrimary));
	SlipLinearSecondary(oXml.GetChildFloat("SlipLinearSecondary", m_fltSlipLinearSecondary));
	SlipAngularNormal(oXml.GetChildFloat("SlipAngularNormal", m_fltSlipAngularNormal));
	SlipAngularPrimary(oXml.GetChildFloat("SlipAngularPrimary", m_fltSlipAngularPrimary));
	SlipAngularSecondary(oXml.GetChildFloat("SlipAngularSecondary", m_fltSlipAngularSecondary));
	
    SlideLinearPrimary(oXml.GetChildFloat("SlideLinearPrimary", m_fltSlideLinearPrimary));
	SlideLinearSecondary(oXml.GetChildFloat("SlideLinearSecondary", m_fltSlideLinearSecondary));
	SlideAngularNormal(oXml.GetChildFloat("SlideAngularNormal", m_fltSlideAngularNormal));
	SlideAngularPrimary(oXml.GetChildFloat("SlideAngularPrimary", m_fltSlideAngularPrimary));
	SlideAngularSecondary(oXml.GetChildFloat("SlideAngularSecondary", m_fltSlideAngularSecondary));

    MaxAdhesive(oXml.GetChildFloat("MaxAdhesive", m_fltMaxAdhesive));

    oXml.OutOfElem(); //OutOf MaterialType Element

}

int VsMaterialType::GetMaterialID(std::string strName)
{
	if(!m_vxMaterialTable)
		THROW_ERROR(Vs_Err_lMaterial_Table_Not_Defined, Vs_Err_strMaterial_Table_Not_Defined);

	return m_vxMaterialTable->getMaterialID(strName.c_str());
}

void VsMaterialType::RegisterMaterialType()
{
    if(!m_vxMaterial)
    {
	    VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	    if(!lpVsSim)
		    THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);
	
        m_vxMaterialTable = lpVsSim->Frame()->getMaterialTable();

	    m_vxMaterial = m_vxMaterialTable->registerMaterial(m_strID.c_str());
    }
}

void VsMaterialType::Initialize()
{
	MaterialType::Initialize();

    RegisterMaterialType();
	SetMaterialProperties();
}

void VsMaterialType::SetMaterialProperties()
{
	if(m_lpSim && m_vxMaterial)
	{
        m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisLinear, VxContactMaterial::kFrictionModelScaledBox);

        m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltFrictionLinearPrimary);
        m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltFrictionLinearPrimaryMax);

        m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltFrictionLinearSecondary);
        m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltFrictionLinearSecondaryMax);

        if(m_fltFrictionAngularNormal > 0)
        {
            m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularNormal, VxContactMaterial::kFrictionModelScaledBox);
            m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisAngularNormal, m_fltFrictionAngularNormal);
            m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisAngularNormal, m_fltFrictionAngularNormalMax);
        }
        else
            m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularNormal, VxContactMaterial::kFrictionModelNeutral);

        if(m_fltFrictionAngularPrimary > 0)
        {
            m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularPrimary, VxContactMaterial::kFrictionModelScaledBox);
            m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltFrictionAngularPrimary);
            m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltFrictionAngularPrimaryMax);
        }
        else
            m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularPrimary, VxContactMaterial::kFrictionModelNeutral);

        if(m_fltFrictionAngularSecondary > 0)
        {
            m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularSecondary, VxContactMaterial::kFrictionModelScaledBox);
            m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltFrictionAngularSecondary);
            m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltFrictionAngularSecondaryMax);
        }
        else
            m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularSecondary, VxContactMaterial::kFrictionModelNeutral);

        m_vxMaterial->setCompliance(m_fltCompliance);
	    m_vxMaterial->setDamping(m_fltDamping);
	    m_vxMaterial->setRestitution(m_fltRestitution);

        m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltSlipLinearPrimary);
        m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltSlipLinearSecondary);
        m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisAngularNormal, m_fltSlipAngularNormal);
        m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltSlipAngularPrimary);
        m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltSlipAngularSecondary);

        m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltSlideLinearPrimary);
        m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltSlideLinearSecondary);
        m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisAngularNormal, m_fltSlideAngularNormal);
        m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltSlideAngularPrimary);
        m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltSlideAngularSecondary);

	    m_vxMaterial->setAdhesiveForce(m_fltMaxAdhesive);
	}
}

	}			// Visualization
}				//VortexAnimatSim
