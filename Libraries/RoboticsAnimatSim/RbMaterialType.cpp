// RbMaterialType.cpp: implementation of the RbMaterialType class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbMaterialType::RbMaterialType()
{
	m_fltFrictionLinearPrimary = 1;
	m_fltFrictionAngularPrimary = 0;
	m_fltRestitution = 0;
}

RbMaterialType::~RbMaterialType()
{
}

/**
\brief	Gets the primary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float RbMaterialType::FrictionLinearPrimary() {return m_fltFrictionLinearPrimary;}

/**
\brief	Sets the primary linear friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void RbMaterialType::FrictionLinearPrimary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionLinearPrimary", true);
	
	m_fltFrictionLinearPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular primary friction coefficient.

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float RbMaterialType::FrictionAngularPrimary() {return m_fltFrictionAngularPrimary;}


/**
\brief	Sets the angular primary friction coefficient.

\author	dcofer
\date	3/23/2011

\param	fltVal	   	The new value. 
**/
void RbMaterialType::FrictionAngularPrimary(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "FrictionAngularPrimary", true);
	
	m_fltFrictionAngularPrimary = fltVal;
	SetMaterialProperties();
}

/**
\brief	Gets the angular primary friction coefficient converted to match vortex values.

\description We need the angular friction value defined and used to match the results from the
vortex simulation as closely as possible. These two systems use different methods to implement 
rolling friction, so they will never match exactly. However, I wanted them to produce at least similar
results. I compared output for a sim that had a cylinder rolling on a plane and getting pushed and 
found how far it rolled in vortex. I then determined the friction required to make the value match in
bullet and came up with the conversion factor below. 

\author	dcofer
\date	3/23/2011

\return	friction coefficient.
**/
float RbMaterialType::FrictionAngularPrimaryConverted() 
{
    return m_fltFrictionAngularPrimary*6.3;
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
float RbMaterialType::Restitution() {return m_fltRestitution;}

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
void RbMaterialType::Restitution(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, true, "Restitution");
	m_fltRestitution = fltVal;
	SetMaterialProperties();
}

/**
\brief	This takes the default values defined in the constructor and scales them according to
the distance and mass units to be acceptable values.

\author	dcofer
\date	3/23/2011
**/
void RbMaterialType::CreateDefaultUnits()
{
	m_fltFrictionLinearPrimary = 1;
	m_fltFrictionAngularPrimary = 0;
	m_fltRestitution = 0;
}

bool RbMaterialType::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(MaterialType::SetData(strType, strValue, false))
		return true;

	if(strType == "FRICTIONLINEARPRIMARY")
	{
		FrictionLinearPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "FRICTIONANGULARPRIMARY")
	{
		FrictionAngularPrimary((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "RESTITUTION")
	{
		Restitution((float) atof(strValue.c_str()));
		return true;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbMaterialType::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	MaterialType::QueryProperties(aryNames, aryTypes);

	aryNames.Add("FrictionLinearPrimary");
	aryTypes.Add("Float");

	aryNames.Add("FrictionAngularPrimary");
	aryTypes.Add("Float");

	aryNames.Add("Restitution");
	aryTypes.Add("Float");
}

void RbMaterialType::Load(CStdXml &oXml)
{
	MaterialType::Load(oXml);

	oXml.IntoElem();  //Into MaterialType Element

	FrictionLinearPrimary(oXml.GetChildFloat("FrictionLinearPrimary", m_fltFrictionLinearPrimary));
	FrictionAngularPrimary(oXml.GetChildFloat("FrictionAngularPrimary", m_fltFrictionAngularPrimary));
	Restitution(oXml.GetChildFloat("Restitution", m_fltRestitution));

    oXml.OutOfElem(); //OutOf MaterialType Element

}

/**
 \brief	Associates a rigid body with this material type. This is used when you change something about this material type.
 It loops through all associated rigid bodies and informs them.

 \author	Dcofer
 \date	10/27/2013

 \param [in,out]	lpBody	If non-null, the pointer to a body.
 */
void RbMaterialType::AddRigidBodyAssociation(RigidBody *lpBody)
{
	if(lpBody)
	{
		//If we do not already have this body in the list then add it.
		if(!FindBodyByID(lpBody->ID(), false))
			m_aryBodies.Add(lpBody->ID(), lpBody);
	}
}

/**
 \brief	Removes the rigid body association described by lpBody from this material.

 \author	Dcofer
 \date	10/27/2013

 \param [in,out]	lpBody	If non-null, the pointer to a body.
 */
void RbMaterialType::RemoveRigidBodyAssociation(RigidBody *lpBody)
{
	if(lpBody)
	{
		if(FindBodyByID(lpBody->ID(), false))
			m_aryBodies.Remove(lpBody->ID());
	}
}

RigidBody *RbMaterialType::FindBodyByID(std::string strID, bool bThrowError)
{
	RigidBody *lpFind = NULL;
	CStdMap<std::string, RigidBody *>::iterator oPos;
	oPos = m_aryBodies.find(Std_CheckString(strID));

	if(oPos != m_aryBodies.end())
		lpFind =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lIDNotFound, Al_Err_strIDNotFound, "ID", strID);

	return lpFind;
}

int RbMaterialType::GetMaterialID(std::string strName)
{
    return -1;
}

void RbMaterialType::RegisterMaterialType()
{
}

void RbMaterialType::Initialize()
{
	//MaterialType::Initialize();

 //   RegisterMaterialType();
	//SetMaterialProperties();
}

void RbMaterialType::SetMaterialProperties()
{
	//CStdMap<std::string, RigidBody *>::iterator oPos;
	//RigidBody *lpBody = NULL;
	//RbRigidBody *lpRbBody = NULL;
	//for(oPos=m_aryBodies.begin(); oPos!=m_aryBodies.end(); ++oPos)
	//{
	//	lpBody = oPos->second;
	//	lpRbBody = dynamic_cast<RbRigidBody *>(lpBody);

	//	if(lpRbBody)
	//		lpRbBody->MaterialTypeModified();
	//}
}

	}			// Visualization
}				//RoboticsAnimatSim
