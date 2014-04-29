/**
\file	ConstraintLimit.cpp

\brief	Implements the constraint limit class.
**/

#include "StdAfx.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "ISimGUICallback.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "ConstraintLimit.h"
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
ConstraintLimit::ConstraintLimit()
{
	m_lpSim = NULL;
	m_lpStructure = NULL;
	m_lpJoint = NULL;

	m_fltLimitPos = 0;
	m_fltDamping = 0;
	m_fltRestitution = 0;
	m_fltStiffness = 0;
	m_bIsLowerLimit = true;
	m_bIsShowPosition = false;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
ConstraintLimit::~ConstraintLimit()
{
}

float ConstraintLimit::LimitPos() {return m_fltLimitPos;}

void ConstraintLimit::LimitPos(float fltVal, bool bUseScaling, bool bOverrideSameCheck) 
{
	//If the values are the same then skip setting this step to preven having to
	//recalculate the matrix positions repeatedly. Only do this when the new position is
	// different than the old one.
	if(fabs(fltVal - m_fltLimitPos) < 1e-5 && !bOverrideSameCheck)
		return;

	if(bUseScaling && m_lpSim && m_lpJoint && !m_lpJoint->UsesRadians())
		m_fltLimitPos = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLimitPos = fltVal;

	SetLimitPos();
}

float ConstraintLimit::Damping() {return m_fltDamping;};

void ConstraintLimit::Damping(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Constraint::Damping", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseMassUnits();

    m_fltDamping = fltVal;
	SetLimitValues();
}

float ConstraintLimit::Restitution() {return m_fltRestitution;};

void ConstraintLimit::Restitution(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Constraint::Restitution", true);
	m_fltRestitution = fltVal;
	SetLimitValues();
}

float ConstraintLimit::Stiffness() {return m_fltStiffness;};

void ConstraintLimit::Stiffness(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Constraint::Stiffness", true);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseMassUnits();

	m_fltStiffness = fltVal;
	SetLimitValues();
}

/**
\brief	Sets the color to use when displaying this contraint.

\details Colors in OSG are between 0 and 1.

\author	dcofer
\date	3/22/2011

\param	fltR	Red component of color. 
\param	fltG	Green component of color. 
\param	fltB	Blue component of color. 
\param	fltA	Alpha component of color. 
**/
void ConstraintLimit::Color(float fltR, float fltG, float fltB, float fltA)
{m_vColor.Set(fltR, fltG, fltB, fltA);}

/**
\brief	Gets the color that is used when displaying this constraint.

\author	dcofer
\date	3/22/2011

\return	CStdColor pointer.
**/
CStdColor *ConstraintLimit::Color() {return &m_vColor;}

/**
\brief	Sets the color of this constraint using an xml packet.

\author	dcofer
\date	3/22/2011

\param	strXml	The xml packet to use when loading the new color info. 
**/
void ConstraintLimit::Color(std::string strXml)
{
	m_vColor.Load(strXml, "Color");
}

float ConstraintLimit::Alpha() {return m_vColor.a();}

/**
\brief	Sets whether this is a lower limit or not..

\author	dcofer
\date	3/22/2011

\param	bVal	true if lower limit. 
**/
void ConstraintLimit::IsLowerLimit(bool bVal) {m_bIsLowerLimit = bVal;}

/**
\brief	Query if this object is lower limit.

\author	dcofer
\date	3/22/2011

\return	true if lower limit, false if not.
**/
bool ConstraintLimit::IsLowerLimit() {return m_bIsLowerLimit;}

/**
\brief	Sets whether this contstraint is actually just being used to show the current position of the joint,
as opposed to being used to show the limit of a constraint..

\author	dcofer
\date	4/11/2011

\param	bVal	true to set this to be a position limit.
**/
void ConstraintLimit::IsShowPosition(bool bVal) {m_bIsShowPosition = bVal;}

/**
\brief	Gets whether this contstraint is actually just being used to show the current position of the joint,
as opposed to being used to show the limit of a constraint..

\author	dcofer
\date	4/11/2011

\return	true if show position, false if not.
**/
bool ConstraintLimit::IsShowPosition() {return m_bIsShowPosition;}

/**
\brief	Sets the system pointers.
		
\details There are a number of system pointers that are needed for use in the objects. The
primariy one being a pointer to the simulation object itself so that you can get global
parameters like the scale units and so on. However, each object may need other types of pointers
as well, for example neurons need to have a pointer to their parent structure/organism, and to
the NeuralModule they reside within. So different types of objects will need different sets of
system pointers. We call this method to set the pointers just after creation and before Load is
called. We then call VerifySystemPointers here, during Load and during Initialize in order to
ensure that the correct pointers have been set for each type of objects. These pointers can then
be safely used throughout the rest of the system. 
		
\author	dcofer
\date	3/2/2011
		
\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to the parent structure. 
\param [in,out]	lpModule	The pointer to the parent module module. 
\param [in,out]	lpNode		The pointer to the parent node. 
\param	bVerify				true to call VerifySystemPointers. 
**/
void ConstraintLimit::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);
	m_lpJoint = dynamic_cast<Joint *>(lpNode);

	if(bVerify) VerifySystemPointers();
}

/**
\brief	Sets a system pointers.

\author	dcofer
\date	3/22/2011

\param [in,out]	lpSim	   	The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
\param [in,out]	lpModule   	The pointer to a module. 
\param [in,out]	lpNode	   	The pointer to a node. 
\param	fltPosition		   	The new position. 
\param	bVerify			   	true to verify. 
**/
void ConstraintLimit::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, float fltPosition, bool bVerify)
{
	SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	LimitPos(fltPosition);
}
		
void ConstraintLimit::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "ConstraintLimit: ", m_strName);

	if(!m_lpJoint)
		THROW_PARAM_ERROR(Al_Err_lJointNotDefined, Al_Err_strJointNotDefined, "ConstraintLimit: ", m_strName);
}

#pragma region DataAccesMethods

float *ConstraintLimit::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "LIMITPOS")
		return &m_fltLimitPos;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool ConstraintLimit::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	if(strType == "LIMITPOS")
	{
		LimitPos((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "DAMPING")
	{
		Damping((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "RESTITUTION")
	{
		Restitution((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "STIFFNESS")
	{
		Stiffness((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "COLOR")
	{
		Color(strValue);
		return true;
	}
	else if(strType == "ALPHA")
	{
		Alpha((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void ConstraintLimit::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("LimitPos", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Damping", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Restitution", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Stiffness", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Color", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Alpha", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

#pragma endregion

/**
\brief	Loads the constraint limit.

\author	dcofer
\date	3/22/2011

\param [in,out]	oXml	The xml packet to load. 
\param	strName			Name of the xml element to load in. 
**/
void ConstraintLimit::Load(CStdXml &oXml, std::string strName)
{
	oXml.FindChildElement(strName);

	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into ConstraintLimit Element

	LimitPos(oXml.GetChildFloat("LimitPos", m_fltLimitPos));
	Damping(oXml.GetChildFloat("Damping", m_fltDamping));
	Restitution(oXml.GetChildFloat("Restitution", m_fltRestitution));
	Stiffness(oXml.GetChildFloat("Stiffness", m_fltStiffness));

	oXml.OutOfElem(); //OutOf ConstraintLimit Element
}


	}			//Environment
}				//AnimatSim
