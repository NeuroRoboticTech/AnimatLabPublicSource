// Body.cpp: implementation of the Body class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a Rigid Body object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a rigid body. 
*/

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


/*! \brief 
   Destroys the Rigid Body object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Rigid Body object..	 
*/

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

void RigidBody::Freeze(BOOL bVal)
{
	m_bFreeze = bVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetFreeze(bVal);
};

void RigidBody::Density(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Density");

	m_fltDensity = fltVal;
	m_fltDensity /= m_lpSim->DensityMassUnits();	//Scale the mass units down to one. If we are using Kg then the editor will save it out as 1000. We need this to be 1 Kg though.
	m_fltDensity *=  pow(m_lpSim->DenominatorDistanceUnits(), 3); //Perform a conversion if necessary because we may be using different units in the denominator.

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetDensity(m_fltDensity);
};

void RigidBody::Ambient(CStdColor &aryColor)
{
	m_vAmbient = aryColor;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Ambient(float *aryColor)
{
	m_vAmbient.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Ambient(string strXml)
{
	m_vAmbient.Load(strXml, "Ambient");
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Diffuse(CStdColor &aryColor)
{
	m_vDiffuse = aryColor;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Diffuse(float *aryColor)
{
	m_vDiffuse.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Diffuse(string strXml)
{
	m_vDiffuse.Load(strXml, "Diffuse");
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Specular(CStdColor &aryColor)
{
	m_vSpecular = aryColor;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Specular(float *aryColor)
{
	m_vSpecular.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Specular(string strXml)
{
	m_vSpecular.Load(strXml, "Specular");
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Shininess(float fltVal)
{
	Std_InValidRange((float) 0, (float) 128, fltVal, TRUE, "Shininess");
	m_fltShininess = fltVal;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_SetColor();
}

void RigidBody::Texture(string strValue)
{
	m_strTexture = strValue;
	if(m_lpPhysicsBody) m_lpPhysicsBody->Physics_TextureChanged();
}
//
//string RigidBody::TextureFile(string strTexture)
//{
//	string strExt = Std_FileExtension(strTexture);
//
//	if(Std_IsBlank(strExt))
//		return AnimatSim::GetFilePath(m_lpSim->ProjectPath(), strTexture);
//	else
//	{
//		string strTex = strTexture;
//		strTex = Std_Replace(strTex, strExt, "");
//		return AnimatSim::GetFilePath(m_lpSim->ProjectPath(), strTex);
//	}
//}

void RigidBody::AddSurfaceContact(Simulator *lpSim, RigidBody *lpContactedSurface)
{
	m_fltSurfaceContactCount++;
}

void RigidBody::RemoveSurfaceContact(Simulator *lpSim, RigidBody *lpContactedSurface)
{
	if(m_fltSurfaceContactCount<=0)
		THROW_ERROR(Al_Err_lInvalidSurceContactCount, Al_Err_strInvalidSurceContactCount);

	m_fltSurfaceContactCount--;
}

void RigidBody::Eat(float fltVal, long lTimeSlice)
{
	if(m_lEatTime != lTimeSlice)
		m_fltFoodEaten = 0;

	m_fltFoodEaten += fltVal;
	m_lEatTime = lTimeSlice;
	m_fltFoodQuantity = fltVal;
}

	
/*! \brief 
   Enables collision between the past-in object and this object.
      
   \param lpBody This is a pointer to the body to enable collisions on.

	 \return
	 No return value.

	 \remarks
	 This method enables collision responses between the rigid body being past
	 in and this rigid body. This is a virtual method that should be overridden 
	 in the simulator system. You need to call physics engine API's to enable
	 the collision responses between these two objects. This method does nothing
	 by default.

   \sa
   EnableCollision, DisableCollision	
*/

void RigidBody::EnableCollision(Simulator *lpSim, RigidBody *lpBody)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_EnableCollision(lpSim, lpBody);
}


/*! \brief 
   Disables collision between the past-in object and this object.
      
   \param lpBody This is a pointer to the body to disable collisions on.

	 \return
	 No return value.

	 \remarks
	 This method disables collision responses between the rigid body being past
	 in and this rigid body. This is a virtual method that should be overridden 
	 in the simulator system. You need to call physics engine API's to disable
	 the collision responses between these two objects. This method does nothing
	 by default.

   \sa
   EnableCollision, DisableCollision	
*/

void RigidBody::DisableCollision(Simulator *lpSim, RigidBody *lpBody)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_DisableCollision(lpSim, lpBody);
}

void RigidBody::Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState)
{
	BodyPart::Kill(lpSim, lpOrganism, bState);

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->Kill(lpSim, lpOrganism, bState);

	if(m_lpJointToParent)
		m_lpJointToParent->Kill(lpSim, lpOrganism, bState);
}

void RigidBody::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->ResetSimulation(lpSim, lpStructure);

	if(m_lpJointToParent)
		m_lpJointToParent->ResetSimulation(lpSim, lpStructure);

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_ResetSimulation(lpSim, lpStructure);
}

void RigidBody::AfterResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->AfterResetSimulation(lpSim, lpStructure);

	if(m_lpJointToParent)
		m_lpJointToParent->AfterResetSimulation(lpSim, lpStructure);
}


/*! \brief 
   Allows the rigid body to create its parts using the chosen physics engine.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                  this rigid body is a part of.

	 \return
	 No return value.

	 \remarks
	 This function can not be truly implemented in the Animat library. It must
	 be implemented in the next layer sitting above it. The reason for this is
	 that the Animat library was made to be generalized so it could work with a
	 number of different physics engines. Therefore it is not tightly coupled with
	 any one engine. This in turn means that we can not implement the code in this
	 library neccessary to create a part or joint in the chosen engine. Several
	 overridable functions have been provided that allow you to do this. The two
	 that will always have to be overridden are the CreateParts and CreateJoints
	 methods. CreateParts makes the API calls to the physics engine to create the
	 collision models, graphics models and so on. You should still call the 
	 base class method at the end of your overridden method so the rigid body
	 can walk down the tree and create the parts for its children.

   \sa
   CreateParts, CreateJoints, StepSimulation	
*/

void RigidBody::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	if(m_bFoodSource)
	{
		lpSim->AddFoodSource(this);

		//We have the replenish rate in Quantity/s, but we need it in Quantity/timeslice. Lets recalculate it here.
		m_fltFoodReplenishRate = (m_fltFoodReplenishRate * lpSim->PhysicsTimeStep());
	}

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->CreateParts(lpSim, lpStructure);
}

/*! \brief 
   Allows the rigid body to create its joints using the chosen physics engine.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                  this rigid body is a part of.

	 \return
	 No return value.

	 \remarks
	 This function can not be truly implemented in the Animat library. It must
	 be implemented in the next layer sitting above it. The reason for this is
	 that the Animat library was made to be generalized so it could work with a
	 number of different physics engines. Therefore it is not tightly coupled with
	 any one engine. This in turn means that we can not implement the code in this
	 library neccessary to create a part or joint in the chosen engine. Several
	 overridable functions have been provided that allow you to do this. The two
	 that will always have to be overridden are the CreateParts and CreateJoints
	 methods. CreateJoints makes the API calls to the physics engine to create the
	 joint and constraints and motors. You should still call the 
	 base class method at the end of your overridden method so the rigid body
	 can walk down the tree and create the joints for its children.

   \sa
   CreateParts, CreateJoints, StepSimulation	
*/

void RigidBody::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->CreateJoints(lpSim, lpStructure);
}


/*! \brief 
   Copies the base value data elements of this rigid body.
   		
	 \param lpOrig This is the rigid body to copy.

	 \return
	 No return value.

   \remarks
   Copies the base value data elements of this rigid body.
*/

void RigidBody::Copy(RigidBody *lpOrig)
{
	m_strID = lpOrig->m_strID;
	m_strName = lpOrig->m_strName;
	m_oAbsPosition = lpOrig->m_oAbsPosition;
	m_oCenterOfMass = lpOrig->m_oCenterOfMass;
	m_fltDensity = lpOrig->m_fltDensity;
	m_lpParent = lpOrig->m_lpParent;
	m_lpJointToParent = lpOrig->m_lpJointToParent;
	CopyPtrArray(lpOrig->m_aryChildParts, m_aryChildParts);
}


/*! \brief 
   Dumps the key values of this object to a text stream.
      
   \param oOs Text stream.

	 \return
	 No return value.

	 \remarks
	 This method is used to trace the key values of an object.
	 You can dump these values to the debug window in Visual Studio or
	 to a log file. When you trace an array of objects this method is
	 automatically invoked for each object in the array.
*/

void RigidBody::Trace(ostream &oOs)
{
	/*
	ClassFactory *lpFactory = Al_ClassFactory();

	oOs << "ID: " << m_strName  << ", Type: " << lpFactory->RigidBodyTypeAbbrev(m_iType);
	oOs << ", RelPos: " << m_oRelPosition << ", AbsPos: " << m_oAbsPosition;
	oOs << ", Rot: " << m_oRotation << ", " << m_fltDensity;
	oOs << ", Cd: " << m_fltCd << ", Volume: " << m_fltVolume;
	oOs << ", Area (" << m_fltXArea << ", " <<m_fltYArea << ", " << m_fltZArea;
	oOs << "), Freeze: " << m_bFreeze;

	if(m_lpParent)
		oOs << ", Parent: " << m_lpParent->ID();

	if(m_lpJointToParent)
		oOs << ", Joint: " << m_lpJointToParent->ID();
	*/
}

//Node Overrides

void RigidBody::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput)
{
}

/*! \brief 
   Allows the rigid body to update itself for each timeslice.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                  this rigid body is a part of.
   \param lStep This is the current time slice.

	 \return
	 No return value.

	 \remarks
   This function is called for each rigid body on every
   time slice. It allows the body to update itself. For
   instance, if this is a muscle type of rigid body that is
   connected to a motor neuron then it may need to adjust the
   force it is applying based on the firing frequency of that
   neuron. If you are doing hydrodynamics then you the bodies
   will need to calcuate the buoyancy and drag forces to apply
   to simulate those effects. You need 
	 to be VERY careful to keep all code within the StepSimulation methods short, sweet, 
	 and very fast. They are in the main processing loop and even a small increase in the
	 amount of processing time that occurrs within this loop will lead to major impacts on
	 the ultimate performance of the system. 

   \sa
   Joint::StepSimulation, Simulator::StepPhysicsEngine
*/

void RigidBody::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	if(m_bFoodSource)
	{
		m_fltFoodQuantity = m_fltFoodQuantity + m_fltFoodReplenishRate;
		if(m_fltFoodQuantity > m_fltMaxFoodQuantity)
			m_fltFoodQuantity = m_fltMaxFoodQuantity;

		//Clear the food eaten variable if it has been around for too long.
		if(m_fltFoodEaten && m_lEatTime != lpSim->TimeSlice())
			m_fltFoodEaten = 0;
	}

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->StepSimulation(lpSim, lpStructure);

	if(m_lpJointToParent)
		m_lpJointToParent->StepSimulation(lpSim, lpStructure);

	UpdateData(lpSim, lpStructure);
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

void RigidBody::AddRigidBody(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("RigidBody");

	RigidBody *lpBody = LoadRigidBody(m_lpSim, m_lpStructure, oXml);

	lpBody->Initialize(m_lpSim, m_lpStructure, NULL);

	//First create all of the model objects.
	lpBody->CreateParts(m_lpSim, m_lpStructure);

	//Then create all of the joints between the models.
	lpBody->CreateJoints(m_lpSim, m_lpStructure);
}

void RigidBody::RemoveRigidBody(string strID, BOOL bThrowError)
{
	int iPos = FindChildListPos(strID, bThrowError);
	m_aryChildParts.RemoveAt(iPos);
}

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

#pragma endregion


/*! \brief 
   Loads a rigid body from an xml configuration file.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                  this rigid body is a part of.
   \param oXml This is an xml object.

	 \return
	 No return value.

	 \remarks
	 This method is responsible for loading the rigid body from a XMl
	 configuration file. You should call this method even in your 
	 overriden function becuase it loads all of the base properties
	 for the Body. This includes the functionality to load the
	 joint and any children.
*/

void RigidBody::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	m_fltDensity = 0;
	m_lpStructure = lpStructure;
	if(m_lpJointToParent) {delete m_lpJointToParent; m_lpJointToParent=NULL;}
	m_aryChildParts.RemoveAll();

	BodyPart::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_strTexture = oXml.GetChildString("Texture", "");

	if(oXml.FindChildElement("COM", FALSE))
		Std_LoadPoint(oXml, "COM", m_oCenterOfMass);
	else
		m_oCenterOfMass.Set(0, 0, 0);

	m_oCenterOfMass *= lpSim->InverseDistanceUnits();

	Density(oXml.GetChildFloat("Density", m_fltDensity));

	m_vCd[0] = m_vCd[1] = m_vCd[2] = oXml.GetChildFloat("Cd", 0);

	if(lpSim->SimulateHydrodynamics())
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
			LoadJoint(lpSim, lpStructure, oXml);
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
			LoadRigidBody(lpSim, lpStructure, oXml);
		}
		oXml.OutOfElem(); //OutOf ChildBodies Element
	}

	if(oXml.FindChildElement("ContactSensor", FALSE))
	{
		m_lpContactSensor = new AnimatSim::Environment::ContactSensor();
		m_lpContactSensor->Load(lpSim, lpStructure, oXml);
	}

	if(oXml.FindChildElement("OdorSources", FALSE))
	{
		oXml.IntoElem();  //Into OdorSources Element
		int iChildCount = oXml.NumberOfChildren();
		
		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadOdor(lpSim, oXml);
		}
		oXml.OutOfElem(); //OutOf OdorSources Element
	}	

	oXml.OutOfElem(); //OutOf RigidBody Element
}

RigidBody *RigidBody::LoadRigidBody(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	RigidBody *lpChild = NULL;
	string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	string strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpChild = dynamic_cast<RigidBody *>(lpSim->CreateObject(strModule, "RigidBody", strType));
	if(!lpChild)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RigidBody");
	lpChild->Parent(this);
	lpChild->SetSystemPointers(lpSim, lpStructure, NULL);

	lpChild->Load(lpSim, lpStructure, oXml);

	m_aryChildParts.Add(lpChild);

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

Joint *RigidBody::LoadJoint(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	string strType;

try
{
	oXml.IntoChildElement("Joint"); //Into Joint Element
	string strModule = oXml.GetChildString("ModuleName", "");
	string strJointType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Joint Element

	m_lpJointToParent = dynamic_cast<Joint *>(lpSim->CreateObject(strModule, "Joint", strJointType));
	if(m_lpJointToParent)
	{
		m_lpJointToParent->Parent(m_lpParent);
		m_lpJointToParent->Child(this);
		m_lpJointToParent->SetSystemPointers(lpSim, lpStructure, NULL);

		m_lpJointToParent->Load(lpSim, lpStructure, oXml);
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

void RigidBody::CompileIDLists(Simulator *lpSim, Structure *lpStructure)
{
	if(m_lpJointToParent)
		lpStructure->AddJoint(m_lpJointToParent);

	//Add me and then add child parts
	lpStructure->AddRigidBody(this);

	int iCount = m_aryChildParts.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryChildParts[iIndex]->CompileIDLists(lpSim, lpStructure);
}

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

Odor *RigidBody::LoadOdor(Simulator *lpSim, CStdXml &oXml)
{
	Odor *lpOdor = NULL;

try
{
	lpOdor = new Odor(this);
	lpOdor->Load(lpSim, oXml);
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

void RigidBody::Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	//Currently not implemented
}

float *RigidBody::LoadMeshVertices(CStdXml &oXml, string strTagName, int &iVertCount, BOOL bThrowError)
{
	float *aryVerts = NULL;

	if(oXml.FindChildElement(strTagName, bThrowError))
	{
		oXml.IntoChildElement(strTagName);

		iVertCount = oXml.NumberOfChildren();

		if(!iVertCount)
			THROW_PARAM_ERROR(Al_Err_lNoVerticesDefined, Al_Err_strNoVerticesDefined, "BodyID", m_strName);

		aryVerts = new float[3*iVertCount];

		int iVerIndex=0;
		for(int iIndex=0; iIndex<iVertCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);

			aryVerts[iVerIndex] = oXml.GetChildAttribFloat("x");
			aryVerts[iVerIndex+1] = oXml.GetChildAttribFloat("y");
			aryVerts[iVerIndex+2] = oXml.GetChildAttribFloat("z");
			iVerIndex+=3;
		}

		oXml.OutOfElem();
	}

	return aryVerts;
}

void RigidBody::AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_AddBodyForce(lpSim, fltPx, fltPy, fltPz, fltFx, fltFy, fltFz, bScaleUnits);
}

void RigidBody::AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz, BOOL bScaleUnits)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_AddBodyTorque(lpSim, fltTx, fltTy, fltTz, bScaleUnits);
}

CStdFPoint RigidBody::GetVelocityAtPoint(float x, float y, float z)
{
	CStdFPoint vLoc(0, 0, 0);

	if(m_lpPhysicsBody)
		vLoc = m_lpPhysicsBody->Physics_GetVelocityAtPoint(x, y, z);

	return vLoc;
}

float RigidBody::GetMass()
{
	float fltMass = 0;

	if(m_lpPhysicsBody)
		fltMass = m_lpPhysicsBody->Physics_GetMass();

	return fltMass;
}

/*! \fn unsigned char Body::Type()
   \brief
   Rigid body type property.
      
   \remarks
	 The type for this rigid body. Examples are Box, Plane, etc.. 
	 This is the read-only accessor function for the m_iType element.
*/


/*! \fn string Body::ID()
   \brief
   Rigid body ID property.
      
   \remarks
	 The unique Id for this body. It is unique for each structure, 
	 but not across structures. So you could have two rigid bodies with the
	 same ID in two different organisms.
	 This is the accessor function for the m_strID element.
*/
/*! \fn void Body::ID(string strValue)
   \brief
   Rigid body Body::ID property.
      
   \remarks
	 The unique Id for this body. It is unique for each structure, 
	 but not across structures. So you could have two rigid bodies with the
	 same ID in two different organisms.
	 This is the mutator function for the m_strID element.
*/


/*! \fn string Body::Texture() 
   \brief
   Texture property.
      
   \remarks
	 An optional texture to apply to the rigid body.
	 This is the accessor function for the m_strTexture element.
*/
/*! \fn void Body::Texture(string strValue)
   \brief
   Texture property.
      
   \remarks
	 An optional texture to apply to the rigid body.
	 This is the mutator function for the m_strTexture element.
*/


/*! \fn CStdFPoint Body::AbsolutePosition()
   \brief
   AbsolutePosition property.
      
   \remarks
	 The absolute position of the rigid body in world coordinates.
	 This is calcualted during loading of the part using the position of 
	 the parent part and the relative position specified in the configuration file.
	 This is the accessor function for the m_oAbsPosition element.
*/
/*! \fn void Body::AbsolutePosition(CStdFPoint &oPoint)
   \brief
   AbsolutePosition property.
      
   \remarks
	 The absolute position of the rigid body in world coordinates.
	 This is calcualted during loading of the part using the position of 
	 the parent part and the relative position specified in the configuration file.
	 This is the mutator function for the m_oAbsPosition element.
*/


/*! \fn CStdFPoint Body::Rotation()	
   \brief
   Rotation property.
      
   \remarks
	 The rotation to apply to this rigid body. It is defined by the three
	 euler angles in radians.
	 This is the accessor function for the m_oRotation element.
*/
/*! \fn void Body::Rotation(CStdFPoint &oPoint)
   \brief
   Rotation property.
      
   \remarks
	 The rotation to apply to this rigid body. It is defined by the three
	 euler angles in radians.
	 This is the mutator function for the m_oRotation element.
*/


/*! \fn Body *Body::Parent()
   \brief
   Parent property.
      
   \remarks
	 The parent rigid body of this part. If this value is null
	 then it is assumed that this is the root part of a structure.
	 This is the accessor function for the m_lpParent element.
*/
/*! \fn void Body::Parent(Body *lpValue)
   \brief
   Parent property.
      
   \remarks
	 The parent rigid body of this part. If this value is null
	 then it is assumed that this is the root part of a structure.
	 This is the mutator function for the m_lpParent element.
*/


/*! \fn float Body::Density()
   \brief
   Density property.
      
   \remarks
	 Uniform density for the rigid body.
	 This is the accessor function for the m_fltDensity element.
*/
/*! \fn void Body::Density(float fltVal)
   \brief
   Density property.
      
   \remarks
	 Uniform density for the rigid body.
	 This is the mutator function for the m_fltDensity element.
*/


/*! \fn float Body::Cd() 
   \brief
   Cd property.
      
   \remarks
	 Drag Coefficient
	 This is the accessor function for the m_fltCd element.
*/
/*! \fn void Body::Cd(float fltVal)
   \brief
   Cd property.
      
   \remarks
	 Drag Coefficient
	 This is the mutator function for the m_fltCd element.
*/


/*! \fn float Body::Volume()
   \brief
   Volume property.
      
   \remarks
	 Total volume for the rigid body. This is used in calculating the buoyancy.
	 This is the read-only accessor function for the m_fltVolume element.
*/


/*! \fn float Body::XArea()
   \brief
   XArea property.
      
   \remarks
	 The area of this rigid body in the x direction. This is used to calculate the
	 drag force in this direction.
	 This is the read-only accessor function for the m_fltXArea element.
*/


/*! \fn float Body::YArea()
   \brief
   YArea property.
       
   \remarks
	 The area of this rigid body in the y direction. This is used to calculate the
	 drag force in this direction.
	 This is the read-only accessor function for the m_fltYArea element.
*/


/*! \fn float Body::ZArea()
   \brief
   ZArea property.
      
   \remarks
	 The area of this rigid body in the z direction. This is used to calculate the
	 drag force in this direction.
	 This is the read-only accessor function for the m_fltZArea element.
*/


/*! \fn BOOL Body::Freeze()
   \brief
   Freeze property.
      
   \remarks
	 Specifies if the part should frozen in place to the world. If a rigid body 
	 is frozen then it is as if it is nailed in place and can not move. Gravity and 
	 and other forces will not act on it.
	 This is the accessor function for the m_bFreeze element.
*/
/*! \fn void Body::Freeze(BOOL bVal)
   \brief
   Freeze property.
      
   \remarks
	 Specifies if the part should frozen in place to the world. If a rigid body 
	 is frozen then it is as if it is nailed in place and can not move. Gravity and 
	 and other forces will not act on it.
	 This is the mutator function for the m_bFreeze element.
*/

	}			//Environment
}				//AnimatSim