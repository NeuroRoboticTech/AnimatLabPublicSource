/**
\file	MaterialType.cpp

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
MaterialType::MaterialType()
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
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
MaterialType::~MaterialType()
{
}

/**
\brief	Gets the primary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float MaterialType::FrictionLinearPrimary() {return m_fltFrictionLinearPrimary;}

/**
\brief	Sets the primary linear friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void MaterialType::FrictionLinearPrimary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionLinearPrimary", TRUE);
	
	m_fltFrictionLinearPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the secondary linear friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float MaterialType::FrictionLinearSecondary() {return m_fltFrictionLinearSecondary;}

/**
\brief	Sets the secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
**/
void MaterialType::FrictionLinearSecondary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionLinearSecondary", TRUE);
	m_fltFrictionLinearSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular normal friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float MaterialType::FrictionAngularNormal() {return m_fltFrictionAngularNormal;}

/**
\brief	Sets the angular normal friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void MaterialType::FrictionAngularNormal(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionAngularNormal", TRUE);
	
	m_fltFrictionAngularNormal = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular primary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float MaterialType::FrictionAngularPrimary() {return m_fltFrictionAngularPrimary;}

/**
\brief	Sets the angular primary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void MaterialType::FrictionAngularPrimary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionAngularPrimary", TRUE);
	
	m_fltFrictionAngularPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float MaterialType::FrictionAngularSecondary() {return m_fltFrictionAngularSecondary;}

/**
\brief	Sets the angular secondary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void MaterialType::FrictionAngularSecondary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionAngularSecondary", TRUE);
	
	m_fltFrictionAngularSecondary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the maximum primary friction allowed.

\author	dcofer
\date	3/23/2011

\return	max friction.
**/
float MaterialType::FrictionLinearPrimaryMax() {return m_fltFrictionLinearPrimaryMax;}

/**
\brief	Sets the maximum primary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::FrictionLinearPrimaryMax(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionLinearPrimaryMax", TRUE);

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
float MaterialType::FrictionLinearSecondaryMax() {return m_fltFrictionLinearSecondaryMax;}

/**
\brief	Sets the maximum secondary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::FrictionLinearSecondaryMax(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionLinearSecondaryMax", TRUE);

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
float MaterialType::FrictionAngularNormalMax() {return m_fltFrictionAngularNormalMax;}

/**
\brief	Sets the maximum angular normal friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::FrictionAngularNormalMax(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionAngularNormalMax", TRUE);

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
float MaterialType::FrictionAngularPrimaryMax() {return m_fltFrictionAngularPrimaryMax;}

/**
\brief	Sets the maximum angular primary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::FrictionAngularPrimaryMax(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionAngularPrimaryMax", TRUE);

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
float MaterialType::FrictionAngularSecondaryMax() {return m_fltFrictionAngularSecondaryMax;}

/**
\brief	Sets the maximum angular secondary friction allowed.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::FrictionAngularSecondaryMax(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FrictionAngularSecondaryMax", TRUE);

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
float MaterialType::SlipLinearPrimary() {return m_fltSlipLinearPrimary;}

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
void MaterialType::SlipLinearPrimary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlipLinearPrimary", TRUE);

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
float MaterialType::SlipLinearSecondary() {return m_fltSlipLinearSecondary;}


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
void MaterialType::SlipLinearSecondary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlipLinearSecondary", TRUE);

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
float MaterialType::SlipAngularNormal() {return m_fltSlipAngularNormal;}


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
void MaterialType::SlipAngularNormal(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlipAngularNormal", TRUE);

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
float MaterialType::SlipAngularPrimary() {return m_fltSlipAngularPrimary;}

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
void MaterialType::SlipAngularPrimary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlipAngularPrimary", TRUE);

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
float MaterialType::SlipAngularSecondary() {return m_fltSlipAngularSecondary;}


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
void MaterialType::SlipAngularSecondary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlipAngularSecondary", TRUE);

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
float MaterialType::SlideLinearPrimary() {return m_fltSlideLinearPrimary;}

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
void MaterialType::SlideLinearPrimary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlideLinearPrimary", TRUE);

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
float MaterialType::SlideLinearSecondary() {return m_fltSlideLinearSecondary;}


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
void MaterialType::SlideLinearSecondary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlideLinearSecondary", TRUE);

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
float MaterialType::SlideAngularNormal() {return m_fltSlideAngularNormal;}


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
void MaterialType::SlideAngularNormal(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlideAngularNormal", TRUE);

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
float MaterialType::SlideAngularPrimary() {return m_fltSlideAngularPrimary;}

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
void MaterialType::SlideAngularPrimary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlideAngularPrimary", TRUE);

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
float MaterialType::SlideAngularSecondary() {return m_fltSlideAngularSecondary;}


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
void MaterialType::SlideAngularSecondary(float fltVal, BOOL bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "SlideAngularSecondary", TRUE);

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
float MaterialType::Compliance() {return m_fltCompliance;}

/**
\brief	Sets the compliance for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::Compliance(float fltVal, BOOL bUseScaling) 
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
float MaterialType::Damping() {return m_fltDamping;}

/**
\brief	Sets the damping for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::Damping(float fltVal, BOOL bUseScaling) 
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
float MaterialType::Restitution() {return m_fltRestitution;}

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
void MaterialType::Restitution(float fltVal) 
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
float MaterialType::MaxAdhesive() {return m_fltMaxAdhesive;}

/**
\brief	Sets the maximum adhesive for collisions between RigidBodies with these two materials.

\details Adhesive force allows objects to stick together, as if they were glued. This property provides the minimal
force needed to separate the two objects.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void MaterialType::MaxAdhesive(float fltVal, BOOL bUseScaling) 
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
void MaterialType::CreateDefaultUnits()
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

BOOL MaterialType::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "FRICTIONLINEARPRIMARY")
	{
		FrictionLinearPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONLINEARSECONDARY")
	{
		FrictionLinearSecondary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONANGULARNORMAL")
	{
		FrictionAngularNormal(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONANGULARPRIMARY")
	{
		FrictionAngularPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONANGULARSECONDARY")
	{
		FrictionAngularSecondary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONLINEARPRIMARYMAX")
	{
		FrictionLinearPrimaryMax(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONLINEARSECONDARYMAX")
	{
		FrictionLinearSecondaryMax(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONANGULARNORMALMAX")
	{
		FrictionAngularNormalMax(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONANGULARPRIMARYMAX")
	{
		FrictionAngularPrimaryMax(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FRICTIONANGULARSECONDARYMAX")
	{
		FrictionAngularSecondaryMax(atof(strValue.c_str()));
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
	
	if(strType == "SLIPLINEARPRIMARY")
	{
		SlipLinearPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strDataType == "SLIPLINEARSECONDARY")
	{
		SlipLinearSecondary(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIPANGULARNORMAL")
	{
		SlipAngularNormal(atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "SLIPANGULARPRIMARY")
	{
		SlipAngularPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strDataType == "SLIPANGULARSECONDARY")
	{
		SlipAngularSecondary(atof(strValue.c_str()));
		return true;
	}

	if(strType == "SLIDELINEARPRIMARY")
	{
		SlideLinearPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strDataType == "SLIDELINEARSECONDARY")
	{
		SlideLinearSecondary(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SLIDEANGULARNORMAL")
	{
		SlideAngularNormal(atof(strValue.c_str()));
		return true;
	}

	if(strType == "SLIDEANGULARPRIMARY")
	{
		SlideAngularPrimary(atof(strValue.c_str()));
		return TRUE;
	}

	if(strDataType == "SLIDEANGULARSECONDARY")
	{
		SlideAngularSecondary(atof(strValue.c_str()));
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

void MaterialType::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("FrictionLinearPrimary");
	aryTypes.Add("Float");

	aryNames.Add("FrictionLinearSecondary");
	aryTypes.Add("Float");

	aryNames.Add("FrictionAngularNormal");
	aryTypes.Add("Float");

	aryNames.Add("FrictionAngularPrimary");
	aryTypes.Add("Float");

	aryNames.Add("FrictionAngularSecondary");
	aryTypes.Add("Float");

	aryNames.Add("FrictionLinearPrimaryMax");
	aryTypes.Add("Float");

	aryNames.Add("FrictionLinearSecondaryMax");
	aryTypes.Add("Float");

	aryNames.Add("FrictionAngularNormalMax");
	aryTypes.Add("Float");

	aryNames.Add("FrictionAngularPrimaryMax");
	aryTypes.Add("Float");

	aryNames.Add("FrictionAngularSecondaryMax");
	aryTypes.Add("Float");

	aryNames.Add("Compliance");
	aryTypes.Add("Float");

	aryNames.Add("Damping");
	aryTypes.Add("Float");

	aryNames.Add("SlipLinearPrimary");
	aryTypes.Add("Float");

	aryNames.Add("SlipLinearSecondary");
	aryTypes.Add("Float");

	aryNames.Add("SlipAngularNormal");
	aryTypes.Add("Float");

	aryNames.Add("SlipAngularPrimary");
	aryTypes.Add("Float");

	aryNames.Add("SlipAngularSecondary");
	aryTypes.Add("Float");

	aryNames.Add("SlideLinearPrimary");
	aryTypes.Add("Float");

	aryNames.Add("SlideLinearSecondary");
	aryTypes.Add("Float");

	aryNames.Add("SlideAngularNormal");
	aryTypes.Add("Float");

	aryNames.Add("SlideAngularPrimary");
	aryTypes.Add("Float");

	aryNames.Add("SlideAngularSecondary");
	aryTypes.Add("Float");

	aryNames.Add("MaxAdhesion");
	aryTypes.Add("Float");
}

void MaterialType::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

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

	}			// Visualization
}				//VortexAnimatSim
