/**
\file	RigidBody.cpp

\brief	Implements the rigid body class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
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
RigidBody::RigidBody()
{
	m_bUsesJoint = TRUE;
	m_lpParent = NULL;
	m_lpStructure = NULL;
	m_fltDensity = 1.0;
	m_fltVolume = 0;
	m_fltXArea = 0;
	m_fltYArea = 0;
	m_fltZArea = 0;

	m_vAmbient.Set(0.1f, 0.1f, 0.1f, 1);
	m_vDiffuse.Set(1, 0, 0, 1);
	m_vSpecular.Set(0.25f, 0.25f, 0.25f, 1);
	m_fltShininess = 64;

	m_vCd[0] = m_vCd[1] = m_vCd[2] = 0;

	m_lpJointToParent = NULL;
	m_bFreeze = FALSE;
	m_bIsContactSensor = FALSE;
	m_bIsCollisionObject = FALSE;
	m_fltSurfaceContactCount= 0 ;
	m_fltLinearVelocityDamping = 0;
	m_fltAngularVelocityDamping = 0;
	m_lpContactSensor = NULL;
	m_bFoodSource = FALSE;
	m_fltFoodEaten = 0;
	m_lEatTime = 0;
	m_fltFoodQuantity = 0;
	m_fltMaxFoodQuantity = 10000;
	m_fltFoodReplenishRate = 0;
	m_fltFoodEnergyContent = 0;

	m_strMaterialID = "DEFAULT";
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
RigidBody::~RigidBody()
{

try
{
	m_lpParent = NULL;

	if(m_lpJointToParent) delete m_lpJointToParent;
	if(m_lpContactSensor) delete m_lpContactSensor;
	m_aryChildParts.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Body\r\n", "", -1, FALSE, TRUE);}
}

int RigidBody::VisualSelectionType()
{
	if(IsCollisionObject())
		return COLLISION_SELECTION_MODE;
	else
		return GRAPHICS_SELECTION_MODE;
}

/**
\brief	Gets the ambient color value. 

\author	dcofer
\date	3/2/2011

\return	Pointer to color data
**/
CStdColor *RigidBody::Ambient() {return &m_vAmbient;}

/**
\brief	Sets the Ambient color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void RigidBody::Ambient(CStdColor &aryColor)
{
	m_vAmbient = aryColor;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Sets the Ambient color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void RigidBody::Ambient(float *aryColor)
{
	m_vAmbient.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Loads the Ambient color from an XML data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/

void RigidBody::Ambient(string strXml)
{
	m_vAmbient.Load(strXml, "Ambient");
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Gets the diffuse color. 

\author	dcofer
\date	3/2/2011

\return	Pointer to color data
**/
CStdColor *RigidBody::Diffuse() {return &m_vDiffuse;}

/**
\brief	Sets the Diffuse color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void RigidBody::Diffuse(CStdColor &aryColor)
{
	m_vDiffuse = aryColor;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Sets the Diffuse color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void RigidBody::Diffuse(float *aryColor)
{
	m_vDiffuse.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Loads the Diffuse color from an XML data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/
void RigidBody::Diffuse(string strXml)
{
	m_vDiffuse.Load(strXml, "Diffuse");
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Gets the specular color. 

\author	dcofer
\date	3/2/2011

\return	Pointer to color data
**/
CStdColor *RigidBody::Specular() {return &m_vSpecular;}

/**
\brief	Sets the Specular color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void RigidBody::Specular(CStdColor &aryColor)
{
	m_vSpecular = aryColor;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Sets the Specular color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void RigidBody::Specular(float *aryColor)
{
	m_vSpecular.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Loads the Specular color from an XML data packet.  

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/
void RigidBody::Specular(string strXml)
{
	m_vSpecular.Load(strXml, "Specular");
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Gets the shininess. 

\author	dcofer
\date	3/2/2011

\return	Shininess value. 
**/

float RigidBody::Shininess() {return m_fltShininess;}

/**
\brief	Sets the shininess value. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new value. 
**/
void RigidBody::Shininess(float fltVal)
{
	Std_InValidRange((float) 0, (float) 128, fltVal, TRUE, "Shininess");
	m_fltShininess = fltVal;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

/**
\brief	Gets the texture filename. 

\author	dcofer
\date	3/2/2011

\return	Texture filename. 
**/
string RigidBody::Texture() {return m_strTexture;}

/**
\brief	Sets the Texture filename. 

\author	dcofer
\date	3/2/2011

\param	strValue	The texture filename. 
**/
void RigidBody::Texture(string strValue)
{
	m_strTexture = strValue;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_TextureChanged();
}

/**
\brief	Gets the user specified center of mass. 

\details If this is (0, 0, 0) then the default COM is used for the part. This is
only used if the user sets it to something.

\author	dcofer
\date	3/2/2011

\return	COM point. 
**/
CStdFPoint RigidBody::CenterOfMass() {return m_oCenterOfMass;}

/**
\brief	Sets the user specified center of mass for this part. 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The point. 
**/
void RigidBody::CenterOfMass(CStdFPoint &oPoint) {m_oCenterOfMass = oPoint;}

/**
\brief	Gets the array of child parts. 

\author	dcofer
\date	3/2/2011

\return	pointer to array of child parts. 
**/
CStdPtrArray<RigidBody> *RigidBody::ChildParts() {return &m_aryChildParts;}

/**
\brief	Gets the joint to parent. 

\author	dcofer
\date	3/2/2011

\return	Pointer to joint that connects this part to its parent.
**/
Joint *RigidBody::JointToParent() {return m_lpJointToParent;}

/**
\brief	Sets the joint to parent. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpValue	The pointer to the joint. 
**/
void RigidBody::JointToParent(Joint *lpValue) {m_lpJointToParent = lpValue;}

/**
\brief	Gets the receptive field contact sensor. 

\author	dcofer
\date	3/2/2011

\return	Pointer to the receptive field contact sensor object. 
**/
ContactSensor *RigidBody::ContactSensor() {return m_lpContactSensor;}

/**
\brief	Gets the uniform density. 

\author	dcofer
\date	3/2/2011

\return	Uniform density value of this part. 
**/
float RigidBody::Density() {return m_fltDensity;}

/**
\brief	Sets the uniform density of this part. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new density value. 
\exception Density must be greater than zero.
**/
void RigidBody::Density(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Density");

	m_fltDensity = fltVal;
	m_fltDensity /= m_lpSim->DensityMassUnits();	//Scale the mass units down to one. If we are using Kg then the editor will save it out as 1000. We need this to be 1 Kg though.
	m_fltDensity *=  pow(m_lpSim->DenominatorDistanceUnits(), 3); //Perform a conversion if necessary because we may be using different units in the denominator.

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetDensity(m_fltDensity);
};

/**
\brief	Gets the coefficient of drag array. 

\author	dcofer
\date	3/2/2011

\return	pointer to an array of drag coefficients for each dimension. 
**/
float *RigidBody::Cd() {return m_vCd;}

/**
\brief	Sets the drag coefficients for each dimension.

\author	dcofer
\date	3/2/2011

\param [in,out]	vVal Pointer to a dimension 3 array of drag coefficients.
**/
void RigidBody::Cd(float *vVal) 
{m_vCd[0] = vVal[0]; m_vCd[1] = vVal[1]; m_vCd[2] = vVal[2];}

/**
\brief	Gets the volume of this part. 

\author	dcofer
\date	3/2/2011

\return	Volume. 
**/
float RigidBody::Volume() {return m_fltVolume;}

/**
\brief	Gets the area of this part in the x dimension. 

\author	dcofer
\date	3/2/2011

\return	Area in x dimesion. 
**/

float RigidBody::XArea() {return m_fltXArea;}

/**
\brief	Gets the area of this part in the y dimension. 

\author	dcofer
\date	3/2/2011

\return	Area in y dimesion.
**/
float RigidBody::YArea() {return m_fltYArea;}

/**
\brief	Gets the area of this part in the z dimension. 

\author	dcofer
\date	3/2/2011

\return	Area in z dimesion.
**/
float RigidBody::ZArea() {return m_fltZArea;}

/**
\brief	Tells if this part is frozen or not

\details Specifies if the part should frozen in place to the world. If a rigid body 
is frozen then it is as if it is nailed in place and can not move. Gravity and 
and other forces will not act on it.

\author	dcofer
\date	3/2/2011

\return	true if frozen, false if else. 
**/
BOOL RigidBody::Freeze() {return m_bFreeze;}

/**
\brief	Freezes. 

\details Specifies if the part should frozen in place to the world. If a rigid body 
is frozen then it is as if it is nailed in place and can not move. Gravity and 
and other forces will not act on it.

\author	dcofer
\date	3/2/2011

\param	bVal	true to freeze. 
**/
void RigidBody::Freeze(BOOL bVal)
{
	m_bFreeze = bVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetFreeze(bVal);
}

/**
\brief	Query if this object is contact sensor. 

\author	dcofer
\date	3/2/2011

\return	true if contact sensor, false if not. 
**/
BOOL RigidBody::IsContactSensor() {return m_bIsContactSensor;}

/**
\brief	Sets whether this is a contact sensor. 

\author	dcofer
\date	3/2/2011

\param	bVal	true if it is a contact sensor. 
**/
void RigidBody::IsContactSensor(BOOL bVal) {m_bIsContactSensor = bVal;}

/**
\brief	Query if this object is collision object. 

\author	dcofer
\date	3/2/2011

\return	true if collision object, false if not. 
**/
BOOL RigidBody::IsCollisionObject() {return m_bIsCollisionObject;}

/**
\brief	Sets whether this part is a collision object. 

\author	dcofer
\date	3/2/2011

\param	bVal true if collision object, false else. 
**/
void RigidBody::IsCollisionObject(BOOL bVal) {m_bIsCollisionObject = bVal;}

/**
\brief	Query if this object is food source. 

\author	dcofer
\date	3/2/2011

\return	true if food source, false if not. 
**/
BOOL RigidBody::IsFoodSource() {return m_bFoodSource;}

/**
\brief	Sets if this is a food source. 

\author	dcofer
\date	3/2/2011

\param	bVal	true if food source, else false. 
**/
void RigidBody::IsFoodSource(BOOL bVal) {m_bFoodSource = bVal;}

/**
\brief	Gets the food quantity. 

\author	dcofer
\date	3/2/2011

\return	Food Quantity. 
**/
float RigidBody::FoodQuantity() {return m_fltFoodQuantity;}

/**
\brief	Sets the Food quantity. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new food quantity value. 
\exception Food Quantity must be zero or greater.
**/
void RigidBody::FoodQuantity(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FoodQuantity", TRUE);
	m_fltFoodQuantity = fltVal;
}

/**
\brief	Gets the amount of food eaten. 

\author	dcofer
\date	3/2/2011

\return	Amount of food eaten. 
**/
float RigidBody::FoodEaten() {return m_fltFoodEaten;}

/**
\brief	Sets the amount of food eaten. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The amount of food eaten. 
**/
void RigidBody::FoodEaten(float fltVal) {m_fltFoodEaten = fltVal;}

/**
\brief	Gets the food replenish rate. 

\author	dcofer
\date	3/2/2011

\return	Food replenish rate. 
**/
float RigidBody::FoodReplenishRate() {return m_fltFoodReplenishRate;}

/**
\brief	Sets the food replenish rate. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new replenish rate. 
**/
void RigidBody::FoodReplenishRate(float fltVal) {m_fltFoodReplenishRate = fltVal;}

/**
\brief	Gets the food energy content. 

\author	dcofer
\date	3/2/2011

\return	Food energy content. 
**/
float RigidBody::FoodEnergyContent() {return m_fltFoodEnergyContent;}

/**
\brief	Sets the food energy content. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new food energy content value. 
\exception Food Quantity must be zero or greater.
**/
void RigidBody::FoodEnergyContent(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FoodEnergyContent", TRUE);
	m_fltFoodEnergyContent = fltVal;
}

/**
\brief	Gets the linear velocity damping for this body part. 

\author	dcofer
\date	3/2/2011

\return	Linear velocity damping value. 
**/
float RigidBody::LinearVelocityDamping() {return m_fltLinearVelocityDamping;}

/**
\brief	Sets the Linear velocity damping. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new value. 
\exception Value must be zero or greater.
**/
void RigidBody::LinearVelocityDamping(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "LinearVelocityDamping", TRUE);
	m_fltLinearVelocityDamping = fltVal;
}

/**
\brief	Gets the angular velocity damping. 

\author	dcofer
\date	3/2/2011

\return	Angular velocity damping for this part. 
**/
float RigidBody::AngularVelocityDamping() {return m_fltAngularVelocityDamping;}

/**
\brief	Sets the angular velocity damping. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new value. 
\exception Value must be zero or greater.
**/
void RigidBody::AngularVelocityDamping(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "AngularVelocityDamping", TRUE);
	m_fltAngularVelocityDamping = fltVal;
}

/**
\brief	Gets the material ID for this part.

\details Each rigid body part can be associated with a specific type of material. 
For example, a material like wood, or concrete. The material specifies things like 
the frictional resistance between that part and parts of other materials. Each material
is defined in the GUI and this is a unique ID string that specifies which one to use for this part.

\author	dcofer
\date	3/2/2011

\return	. 
**/
string RigidBody::MaterialID() {return m_strMaterialID;}

/**
\brief	Sets the Material ID for this part. 

\details Each rigid body part can be associated with a specific type of material. 
For example, a material like wood, or concrete. The material specifies things like 
the frictional resistance between that part and parts of other materials. Each material
is defined in the GUI and this is a unique ID string that specifies which one to use for this part.

\author	dcofer
\date	3/2/2011

\param	strID	ID of the material to use. 
**/
void RigidBody::MaterialID(string strID) {m_strMaterialID = strID;}

/**
\brief	Gets the surface contact count. 

\author	dcofer
\date	3/2/2011

\return	Returns m_fltSurfaceContactCount. 
**/
float RigidBody::SurfaceContactCount() {return m_fltSurfaceContactCount;}

/**
\brief	Increments the surface contact count when this part collides with something in the
virtual world

\details If this item is setup to be a contact sensor then when the physics engine detects a
collision between two objects it will provide this back to us. We then call this method to update
the number of contacts that this object is undergoing. This value can then be used to detect
whether, and how many, contacts are currently happening. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpContactedSurface	The pointer to the other contacted surface. 
**/
void RigidBody::AddSurfaceContact(RigidBody *lpContactedSurface)
{
	m_fltSurfaceContactCount++;
}

/**
\brief	Decrements the surface contact count when this part stops colliding with something in the
virtual world

\details If this item is setup to be a contact sensor then when the physics engine detects when a
collision between two objects stops, and it will provide this back to us. We then call this
method to update the number of contacts that this object is undergoing. This value can then be
used to detect whether, and how many, contacts are currently happening. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpContactedSurface	If non-null, the pointer to a contacted surface. 
**/
void RigidBody::RemoveSurfaceContact(RigidBody *lpContactedSurface)
{
	if(m_fltSurfaceContactCount<=0)
		THROW_ERROR(Al_Err_lInvalidSurceContactCount, Al_Err_strInvalidSurceContactCount);

	m_fltSurfaceContactCount--;
}

/**
\brief	This item is eating the specified amount of food. 

\author	dcofer
\date	3/2/2011

\param	fltVal		The amount of food to eat. 
\param	lTimeSlice	The time slice during which the eating is occuring. 
**/
void RigidBody::Eat(float fltVal, long lTimeSlice)
{
	if(m_lEatTime != lTimeSlice)
		m_fltFoodEaten = 0;

	m_fltFoodEaten += fltVal;
	m_lEatTime = lTimeSlice;
	m_fltFoodQuantity = fltVal;
}

/**
\brief	Enables collision between the past-in object and this object.

\details This method enables collision responses between the rigid body being past in and this
rigid body. This is a virtual method that should be overridden in the simulator system. You need
to call physics engine API's to enable the collision responses between these two objects. This
method does nothing by default. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpBody	The pointer to a body. 
**/
void RigidBody::EnableCollision(RigidBody *lpBody)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_EnableCollision(lpBody);
}

/**
\brief	Disables collision between the past-in object and this object.

\details This method disables collision responses between the rigid body being past in and this
rigid body. This is a virtual method that should be overridden in the simulator system. You need
to call physics engine API's to disable the collision responses between these two objects. This
method does nothing by default. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpBody	The pointer to a body. 
**/
void RigidBody::DisableCollision(RigidBody *lpBody)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_DisableCollision(lpBody);
}

void RigidBody::Kill(BOOL bState)
{
	BodyPart::Kill(bState);

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->Kill(bState);

	if(m_lpJointToParent)
		m_lpJointToParent->Kill(bState);
}

void RigidBody::ResetSimulation()
{
	///It is <b>very</b> important that the physcis of the rigid body is reset
    /// before the joint and the child parts. The reason is that we want the position
    /// of this part to be rest first and then child parts because if we do not then
    /// when we do an UpdateFromNode on the physcis object it will be in the wrong location.
    /// We must reset the position from the root out, not leafs in.
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_ResetSimulation();

	if(m_lpJointToParent)
		m_lpJointToParent->ResetSimulation();

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->ResetSimulation();
}

void RigidBody::AfterResetSimulation()
{
	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->AfterResetSimulation();

	if(m_lpJointToParent)
		m_lpJointToParent->AfterResetSimulation();
}

/**
\brief	Allows the rigid body to create its parts using the chosen physics engine.

\details This function can not be truly implemented in the Animat library. It must be implemented
in the next layer sitting above it. The reason for this is that the Animat library was made to be
generalized so it could work with a number of different physics engines. Therefore it is not
tightly coupled with any one engine. This in turn means that we can not implement the code in
this library neccessary to create a part or joint in the chosen engine. Several overridable
functions have been provided that allow you to do this. The two that will always have to be
overridden are the CreateParts and CreateJoints methods. CreateParts makes the API calls to the
physics engine to create the collision models, graphics models and so on. You should still call
the base class method at the end of your overridden method so the rigid body can walk down the
tree and create the parts for its children. 

\author	dcofer
\date	3/2/2011
**/

void RigidBody::CreateParts()
{
	if(m_bFoodSource)
	{
		m_lpSim->AddFoodSource(this);

		//We have the replenish rate in Quantity/s, but we need it in Quantity/timeslice. Lets recalculate it here.
		m_fltFoodReplenishRate = (m_fltFoodReplenishRate * m_lpSim->PhysicsTimeStep());
	}

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->CreateParts();
}

/**
\brief	Allows the rigid body to create its joints using the chosen physics engine.

\details This function can not be truly implemented in the Animat library. It must be implemented
in the next layer sitting above it. The reason for this is that the Animat library was made to be
generalized so it could work with a number of different physics engines. Therefore it is not
tightly coupled with any one engine. This in turn means that we can not implement the code in
this library neccessary to create a part or joint in the chosen engine. Several overridable
functions have been provided that allow you to do this. The two that will always have to be
overridden are the CreateParts and CreateJoints methods. CreateJoints makes the API calls to the
physics engine to create the joint and constraints and motors. You should still call the base
class method at the end of your overridden method so the rigid body can walk down the tree and
create the joints for its children. 

\author	dcofer
\date	3/2/2011
**/
void RigidBody::CreateJoints()
{
	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->CreateJoints();
}

void RigidBody::AddExternalNodeInput(float fltInput)
{
}

void RigidBody::StepSimulation()
{
	if(m_bFoodSource)
	{
		m_fltFoodQuantity = m_fltFoodQuantity + m_fltFoodReplenishRate;
		if(m_fltFoodQuantity > m_fltMaxFoodQuantity)
			m_fltFoodQuantity = m_fltMaxFoodQuantity;

		//Clear the food eaten variable if it has been around for too long.
		if(m_fltFoodEaten && m_lEatTime != m_lpSim->TimeSlice())
			m_fltFoodEaten = 0;
	}

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->StepSimulation();

	if(m_lpJointToParent)
		m_lpJointToParent->StepSimulation();

	UpdateData();
}


#pragma region DataAccesMethods

float *RigidBody::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	float *lpData = BodyPart::GetDataPointer(strDataType);
	if(lpData)
		return lpData;

	if(strType == "FOODQUANTITY")
		return &m_fltFoodQuantity;
	if(strType == "FOODEATEN")
		return &m_fltFoodEaten;
	if(strType == "ENABLE")
		return &m_fltEnabled;

	if(m_lpPhysicsBody)
	{
		float *lpData = NULL;
		lpData = m_lpPhysicsBody->Physics_GetDataPointer(strDataType);
		if(lpData) return lpData;
	}

	return NULL;
}


BOOL RigidBody::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(BodyPart::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "FREEZE")
	{
		Freeze(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "DENSITY")
	{
		Density(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "AMBIENT")
	{
		Ambient(strValue);
		return TRUE;
	}
	
	if(strType == "DIFFUSE")
	{
		Diffuse(strValue);
		return TRUE;
	}
	
	if(strType == "SPECULAR")
	{
		Specular(strValue);
		return TRUE;
	}
	
	if(strType == "SHININESS")
	{
		Shininess(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "TEXTURE")
	{
		Texture(strValue);
		return TRUE;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

BOOL RigidBody::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "RIGIDBODY")
	{
		AddRigidBody(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL RigidBody::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "RIGIDBODY")
	{
		RemoveRigidBody(strID);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

/**
\brief	Creates and adds a rigid body. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the body. 
**/
void RigidBody::AddRigidBody(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("RigidBody");

	RigidBody *lpBody = LoadRigidBody(oXml);

	lpBody->Initialize();

	//First create all of the model objects.
	lpBody->CreateParts();

	//Then create all of the joints between the models.
	lpBody->CreateJoints();
}

/**
\brief	Removes the rigid body with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the body to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void RigidBody::RemoveRigidBody(string strID, BOOL bThrowError)
{
	int iPos = FindChildListPos(strID, bThrowError);
	m_aryChildParts.RemoveAt(iPos);
	m_lpStructure->RemoveRigidBody(strID);
}

/**
\brief	Finds the array index for the child part with the specified ID

\author	dcofer
\date	3/2/2011

\param	strID ID of part to find
\param	bThrowError	If true and ID is not found then it will throw an error, else return NULL
\exception If bThrowError is true and ID is not found.

\return	If bThrowError is false and ID is not found returns NULL, 
else returns the pointer to the found part.
**/
int RigidBody::FindChildListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryChildParts[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lBodyOrJointIDNotFound, Al_Err_strBodyOrJointIDNotFound, "ID");

	return -1;
}

void RigidBody::UpdatePhysicsPosFromGraphics()
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_UpdateNode();

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->UpdatePhysicsPosFromGraphics();
}

#pragma endregion


void RigidBody::Load(CStdXml &oXml)
{
	m_fltDensity = 0;
	if(m_lpJointToParent) {delete m_lpJointToParent; m_lpJointToParent=NULL;}
	m_aryChildParts.RemoveAll();

	BodyPart::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_strTexture = oXml.GetChildString("Texture", "");

	if(oXml.FindChildElement("COM", FALSE))
		Std_LoadPoint(oXml, "COM", m_oCenterOfMass);
	else
		m_oCenterOfMass.Set(0, 0, 0);

	m_oCenterOfMass *= m_lpSim->InverseDistanceUnits();

	Density(oXml.GetChildFloat("Density", m_fltDensity));

	m_vCd[0] = m_vCd[1] = m_vCd[2] = oXml.GetChildFloat("Cd", 0);

	if(m_lpSim->SimulateHydrodynamics())
	{
		Std_IsAboveMin((float) 0, m_vCd[0], TRUE, "Cd x", true);
		Std_IsAboveMin((float) 0, m_vCd[1], TRUE, "Cd y", true);
		Std_IsAboveMin((float) 0, m_vCd[2], TRUE, "Cd z", true);
	}

	m_strMaterialID = Std_ToUpper(oXml.GetChildString("MaterialID", m_strMaterialID));

	m_bFreeze = oXml.GetChildBool("Freeze", m_bFreeze);
	m_bIsContactSensor = oXml.GetChildBool("IsContactSensor", m_bIsContactSensor);
	m_bIsCollisionObject = oXml.GetChildBool("IsCollisionObject", m_bIsCollisionObject);

	m_bFoodSource = oXml.GetChildBool("FoodSource", m_bFoodSource);
	m_fltFoodQuantity = oXml.GetChildFloat("FoodQuantity", m_fltFoodQuantity);
	m_fltMaxFoodQuantity = oXml.GetChildFloat("MaxFoodQuantity", m_fltMaxFoodQuantity);
	m_fltFoodReplenishRate = oXml.GetChildFloat("FoodReplenishRate", m_fltFoodReplenishRate);
	m_fltFoodEnergyContent = oXml.GetChildFloat("FoodEnergyContent", m_fltFoodEnergyContent);

	Std_InValidRange((float) 0, (float) 100000, m_fltFoodQuantity, TRUE, "FoodQuantity");
	Std_InValidRange((float) 0, (float) 100000, m_fltFoodQuantity, TRUE, "MaxFoodQuantity");
	Std_InValidRange((float) 0, (float) 100000, m_fltFoodReplenishRate, TRUE, "FoodReplenishRate");
	Std_InValidRange((float) 0, (float) 100000, m_fltFoodEnergyContent, TRUE, "FoodEnergyContent");

	m_fltLinearVelocityDamping = oXml.GetChildFloat("LinearVelocityDamping", m_fltLinearVelocityDamping);
	m_fltAngularVelocityDamping = oXml.GetChildFloat("AngularVelocityDamping", m_fltAngularVelocityDamping);

	Std_InValidRange((float) 0, (float) 1000, m_fltLinearVelocityDamping, TRUE, "LinearVelocityDamping");
	Std_InValidRange((float) 0, (float) 1000, m_fltAngularVelocityDamping, TRUE, "AngularVelocityDamping");

	m_vDiffuse.Load(oXml, "Diffuse", false);
	m_vAmbient.Load(oXml, "Ambient", false);
	m_vSpecular.Load(oXml, "Specular", false);
	m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess);

	//Only load the joint if there is a parent object and this body uses joints.
	if(m_lpParent && m_bUsesJoint)
	{
		//Static joints do not have joints specified. Any time that there is a parent but not joint defined
		//then this signals that we need to statically add that part to the parent object geometry.
		if(oXml.FindChildElement("Joint", FALSE))
			LoadJoint(oXml);
		else
			m_lpJointToParent = NULL;
	}

	if(oXml.FindChildElement("ChildBodies", FALSE))
	{
		oXml.IntoElem();  //Into ChildBodies Element
		int iChildCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadRigidBody(oXml);
		}
		oXml.OutOfElem(); //OutOf ChildBodies Element
	}

	if(oXml.FindChildElement("ContactSensor", FALSE))
	{
		m_lpContactSensor = new AnimatSim::Environment::ContactSensor();
		m_lpContactSensor->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
		m_lpContactSensor->Load(oXml);
	}

	if(oXml.FindChildElement("OdorSources", FALSE))
	{
		oXml.IntoElem();  //Into OdorSources Element
		int iChildCount = oXml.NumberOfChildren();
		
		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadOdor(oXml);
		}
		oXml.OutOfElem(); //OutOf OdorSources Element
	}	

	oXml.OutOfElem(); //OutOf RigidBody Element
}

/**
\brief	Loads a child rigid body. 

\author	dcofer
\date	3/2/2011

\param [in,out]	oXml	The xml data definition of the part to load. 

\return	null if it fails, else the rigid body. 
**/

RigidBody *RigidBody::LoadRigidBody(CStdXml &oXml)
{
	RigidBody *lpChild = NULL;
	string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	string strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpChild = dynamic_cast<RigidBody *>(m_lpSim->CreateObject(strModule, "RigidBody", strType));
	if(!lpChild)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RigidBody");
	lpChild->Parent(this);

	lpChild->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
	lpChild->Load(oXml);

	m_aryChildParts.Add(lpChild);
	m_lpStructure->AddRigidBody(lpChild);

	return lpChild;
}
catch(CStdErrorInfo oError)
{
	if(lpChild) delete lpChild;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpChild) delete lpChild;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Loads a child joint. 

\author	dcofer
\date	3/2/2011

\param [in,out]	oXml	The xml data definition of the part to load. 

\return	null if it fails, else the joint. 
**/

Joint *RigidBody::LoadJoint(CStdXml &oXml)
{
	string strType;

try
{
	oXml.IntoChildElement("Joint"); //Into Joint Element
	string strModule = oXml.GetChildString("ModuleName", "");
	string strJointType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Joint Element

	m_lpJointToParent = dynamic_cast<Joint *>(m_lpSim->CreateObject(strModule, "Joint", strJointType));
	if(m_lpJointToParent)
	{
		m_lpJointToParent->Parent(m_lpParent);
		m_lpJointToParent->Child(this);

		m_lpJointToParent->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
		m_lpJointToParent->Load(oXml);

		m_lpStructure->AddJoint(m_lpJointToParent);
	}

	return m_lpJointToParent;
}
catch(CStdErrorInfo oError)
{
	if(m_lpJointToParent) delete m_lpJointToParent;
	m_lpJointToParent = NULL;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(m_lpJointToParent) delete m_lpJointToParent;
	m_lpJointToParent = NULL;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Adds an odor source to this body part. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpOdor	The pointer to an odor source to add. 
**/

void RigidBody::AddOdor(Odor *lpOdor)
{
	if(!lpOdor)
		THROW_ERROR(Al_Err_lOdorNotDefined, Al_Err_strOdorNotDefined);

	try
	{
			m_aryOdorSources.Add(lpOdor->ID(), lpOdor);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate odor type Key: " + lpOdor->ID(); 
		RELAY_ERROR(oError);
	}
}

/**
\brief	Loads an odor source. 

\author	dcofer
\date	3/2/2011

\param [in,out]	oXml	The xml data to use when loading the odor. 

\return	Pointer to the new odor. 
**/

Odor *RigidBody::LoadOdor(CStdXml &oXml)
{
	Odor *lpOdor = NULL;

try
{
	lpOdor = new Odor(this);

	lpOdor->SetSystemPointers(m_lpSim, m_lpStructure, NULL, this, TRUE);
	lpOdor->Load(oXml);

	AddOdor(lpOdor);

	return lpOdor;
}
catch(CStdErrorInfo oError)
{
	if(lpOdor) delete lpOdor;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpOdor) delete lpOdor;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Adds a force to this body at a specified position. 

\author	dcofer
\date	3/2/2011

\param	fltPx		The x position. 
\param	fltPy		The y position.  
\param	fltPz		The z position.  
\param	fltFx		The x force. 
\param	fltFy		The y force. 
\param	fltFz		The z force. 
\param	bScaleUnits	If true then the force and value is scaled by the ScaleUnits, otherwise it is
					applied as provided. 
**/

void RigidBody::AddForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_AddBodyForce(fltPx, fltPy, fltPz, fltFx, fltFy, fltFz, bScaleUnits);
}

/**
\brief	Adds a torque to this body about its center. 

\author	dcofer
\date	3/2/2011

\param	fltTx		The torque about the x axis. 
\param	fltTy		The torque about the y axis. 
\param	fltTz		The torque about the z axis. 
\param	bScaleUnits	If true then the force and value is scaled by the ScaleUnits, otherwise it is
					applied as provided. 
**/
void RigidBody::AddTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_AddBodyTorque(fltTx, fltTy, fltTz, bScaleUnits);
}

/**
\brief	Gets a velocity of this body at specified point in the body. 

\author	dcofer
\date	3/2/2011

\param	x	The x coordinate. 
\param	y	The y coordinate. 
\param	z	The z coordinate. 

\return	The velocity at point. 
**/
CStdFPoint RigidBody::GetVelocityAtPoint(float x, float y, float z)
{
	CStdFPoint vLoc(0, 0, 0);

	if(m_lpPhysicsBody)
		vLoc = m_lpPhysicsBody->Physics_GetVelocityAtPoint(x, y, z);

	return vLoc;
}

/**
\brief	Gets the mass of this part. 

\author	dcofer
\date	3/2/2011

\return	The mass. 
**/
float RigidBody::GetMass()
{
	float fltMass = 0;

	if(m_lpPhysicsBody)
		fltMass = m_lpPhysicsBody->Physics_GetMass();

	return fltMass;
}

	}			//Environment
}				//AnimatSim