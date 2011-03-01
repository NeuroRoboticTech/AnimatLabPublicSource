#include "stdafx.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
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
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{

ConstraintLimit::ConstraintLimit()
{
	m_lpSim = NULL;
	m_lpStructure = NULL;
	m_lpJoint = NULL;

	m_fltLimitPos = 0;
	m_fltDamping = 0;
	m_fltRestitution = 0;
	m_fltStiffness = 0;
	m_bIsLowerLimit = TRUE;
}

ConstraintLimit::~ConstraintLimit()
{
}

float ConstraintLimit::LimitPos() {return m_fltLimitPos;}

void ConstraintLimit::LimitPos(float fltVal, BOOL bUseScaling, BOOL bOverrideSameCheck) 
{
	if(bUseScaling && m_lpSim && m_lpJoint && !m_lpJoint->UsesRadians())
		m_fltLimitPos = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLimitPos = fltVal;
}

float ConstraintLimit::Damping() {return m_fltDamping;};

void ConstraintLimit::Damping(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Constraint::Damping", TRUE);

	if(bUseScaling)
		fltVal = fltVal/m_lpSim->DensityMassUnits();
	m_fltDamping = fltVal;
}

float ConstraintLimit::Restitution() {return m_fltRestitution;};

void ConstraintLimit::Restitution(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Constraint::Restitution", TRUE);
	m_fltRestitution = fltVal;
}

float ConstraintLimit::Stiffness() {return m_fltStiffness;};

void ConstraintLimit::Stiffness(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Constraint::Stiffness", TRUE);

	if(bUseScaling)
		fltVal *= m_lpSim->InverseMassUnits();

	m_fltStiffness = fltVal;
}

void ConstraintLimit::Color(float fltR, float fltG, float fltB, float fltA)
{m_vColor.Set(fltR, fltG, fltB, fltA);}

CStdColor *ConstraintLimit::Color() {return &m_vColor;}

void ConstraintLimit::Color(string strXml)
{
	m_vColor.Load(strXml, "Color");
}

float ConstraintLimit::Alpha() {return m_vColor.a();}

void ConstraintLimit::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, Joint *lpJoint)
{
	m_lpSim = lpSim;
	m_lpStructure = lpStructure;
	m_lpJoint = lpJoint;
}

void ConstraintLimit::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, Joint *lpJoint, float fltPosition)
{
	SetSystemPointers(lpSim, lpStructure, lpJoint);
	LimitPos(fltPosition);
}
#pragma region DataAccesMethods

float *ConstraintLimit::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "LIMITPOS")
		return &m_fltLimitPos;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

BOOL ConstraintLimit::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "LIMITPOS")
	{
		LimitPos(atof(strValue.c_str()));
		return true;
	}
	else if(strType == "DAMPING")
	{
		Damping(atof(strValue.c_str()));
		return true;
	}
	else if(strType == "RESTITUTION")
	{
		Restitution(atof(strValue.c_str()));
		return true;
	}
	else if(strType == "STIFFNESS")
	{
		Stiffness(atof(strValue.c_str()));
		return true;
	}
	else if(strType == "COLOR")
	{
		Color(strValue);
		return true;
	}
	else if(strType == "ALPHA")
	{
		Alpha(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void ConstraintLimit::Load(Simulator *lpSim, Structure *lpStructure, Joint *lpJoint, CStdXml &oXml, string strName)
{
	SetSystemPointers(lpSim, lpStructure, lpJoint);

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
