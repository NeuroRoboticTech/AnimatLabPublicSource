// BlConstraintRelaxation.cpp: implementation of the BlConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlConstraintRelaxation.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlConstraintRelaxation::BlConstraintRelaxation()
{
    m_lpBlJoint = NULL;
    m_lpConstraint = NULL;
    m_fltMaxLimit = 0; 
    m_fltMinLimit = 0;
    m_fltEqPos = 0;
    m_fltInvDamping = 1/m_fltDamping;
    m_bDisallowSpringEnable = false;
}

BlConstraintRelaxation::~BlConstraintRelaxation()
{
}

/**
\brief	Gets the minimum value that this relaxation can move.

\author	dcofer
\date	3/23/2011

\return	minimum limit value.
**/
float BlConstraintRelaxation::MinLimit() {return m_fltMinLimit;}

/**
\brief	Sets the minimum value that this relaxation can move.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void BlConstraintRelaxation::MinLimit(float fltVal, bool bUseScaling) 
{
	if(bUseScaling)
		fltVal = fltVal * m_lpSim->InverseDistanceUnits();
	else
		fltVal = fltVal;

	m_fltMinLimit = fltVal;
	SetRelaxationProperties();
}

/**
\brief	Gets the maximum value that this relaxation can move.

\author	dcofer
\date	3/23/2011

\return	maximum limit value.
**/
float BlConstraintRelaxation::MaxLimit() {return m_fltMaxLimit;}

/**
\brief	Sets the maximum value that this relaxation can move.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void BlConstraintRelaxation::MaxLimit(float fltVal, bool bUseScaling) 
{
	if(bUseScaling)
		fltVal = fltVal * m_lpSim->InverseDistanceUnits();
	else
		fltVal = fltVal;

	m_fltMaxLimit = fltVal;
	SetRelaxationProperties();
}

/**
\brief	Gets the equilibrium position for the spring that controls this relaxation.

\author	dcofer
\date	3/23/2011

\return	Equilibruim position value.
**/
float BlConstraintRelaxation::EqPos() {return m_fltEqPos;}

/**
\brief	Sets the equilibrium position for the spring that controls this relaxation.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void BlConstraintRelaxation::EqPos(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "EqPos", true);

	if(bUseScaling)
		fltVal = fltVal * m_lpSim->InverseDistanceUnits();
	else
		fltVal = fltVal;

	m_fltEqPos = fltVal;
	SetRelaxationProperties();
}
/**
\brief	Sets the damping for collisions between RigidBodies with these two materials.

\author	dcofer
\date	3/23/2011

\param	fltVal	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void BlConstraintRelaxation::Damping(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Damping", true);

	if(bUseScaling)
		fltVal = fltVal/m_lpSim->DisplayMassUnits();

	m_fltDamping = fltVal;
    
    if(m_fltDamping <= 0)
        m_fltInvDamping = 1;
    else
        m_fltInvDamping = 1/m_fltDamping;

	SetRelaxationProperties();
}

/**
 \brief Gets the disallow spring enable.

 \description For joints like hinge and prismatic I am allowing them to set a spring in the direction of the joint motion (Z axis). However, the joint
              itself needs to be the one to determine when to enable/disable the spring because it has to control the motor as well. This flag tells
              the relaxation that it should not directly set the spring enabled/disabled, that it should allow the joint to do that itself.

 \author    David Cofer
 \date  2/16/2014

 \return    true if it disallowed, false if otherwise.
 */
bool BlConstraintRelaxation::DisallowSpringEnable()
{
    return m_bDisallowSpringEnable;
}

/**
 \brief Disallow spring enable.
 
 \description For joints like hinge and prismatic I am allowing them to set a spring in the direction of the joint motion (Z axis). However, the joint
              itself needs to be the one to determine when to enable/disable the spring because it has to control the motor as well. This flag tells
              the relaxation that it should not directly set the spring enabled/disabled, that it should allow the joint to do that itself.

 \author    David Cofer
 \date  2/16/2014

 \param bVal    value.
 */
void BlConstraintRelaxation::DisallowSpringEnable(bool bVal)
{
    m_bDisallowSpringEnable = bVal;
}

void BlConstraintRelaxation::Initialize()
{
	ConstraintRelaxation::Initialize();

    if(m_lpSim && m_lpNode)
    {
        m_lpBlJoint = dynamic_cast<BlJoint *>(m_lpNode);
        if(m_lpBlJoint)
            m_lpConstraint = dynamic_cast<btAnimatGeneric6DofConstraint *>(m_lpBlJoint->Constraint());
    }

	SetRelaxationProperties();
}

/**
\brief	This takes the default values defined in the constructor and scales them according to
the distance and mass units to be acceptable values.

\author	dcofer
\date	3/23/2011
**/
void BlConstraintRelaxation::CreateDefaultUnits()
{
    ConstraintRelaxation::CreateDefaultUnits();

    if(m_fltDamping <= 0)
        m_fltInvDamping = 1;
    else
        m_fltInvDamping = 1/m_fltDamping;

    m_fltMinLimit *= m_lpSim->InverseDistanceUnits();
    m_fltMaxLimit *= m_lpSim->InverseDistanceUnits();
    m_fltEqPos *= m_lpSim->InverseDistanceUnits();
}

bool BlConstraintRelaxation::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(ConstraintRelaxation::SetData(strType, strValue, false))
		return true;

	if(strType == "MINLIMIT")
	{
		MinLimit((float) atof(strValue.c_str()));
		return true;
	}

    if(strType == "MAXLIMIT")
	{
		MaxLimit((float) atof(strValue.c_str()));
		return true;
	}

    if(strType == "EQPOS")
	{
		EqPos((float) atof(strValue.c_str()));
		return true;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlConstraintRelaxation::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	ConstraintRelaxation::QueryProperties(aryNames, aryTypes);

	aryNames.Add("MinLimit");
	aryTypes.Add("Float");

	aryNames.Add("MaxLimit");
	aryTypes.Add("Float");

	aryNames.Add("EqPos");
	aryTypes.Add("Float");
}

void BlConstraintRelaxation::Load(CStdXml &oXml)
{
	ConstraintRelaxation::Load(oXml);

	oXml.IntoElem();  //Into ConstraintRelaxation Element

	MinLimit(oXml.GetChildFloat("MinLimit", m_fltMinLimit));
	MaxLimit(oXml.GetChildFloat("MaxLimit", m_fltMaxLimit));
	EqPos(oXml.GetChildFloat("EqPos", m_fltEqPos));

    oXml.OutOfElem(); //OutOf ConstraintRelaxation Element

}

void BlConstraintRelaxation::SetRelaxationProperties()
{
    if(m_lpSim && m_lpNode && m_lpConstraint && m_lpBlJoint)
    {
        if(!m_bDisallowSpringEnable)
            m_lpConstraint->enableSpring(m_iCoordinateID, m_bEnabled);

        m_lpConstraint->setEquilibriumPoint(m_iCoordinateID, m_fltEqPos);
        m_lpConstraint->setStiffness(m_iCoordinateID, m_fltStiffness);
        m_lpConstraint->setDamping(m_iCoordinateID, m_fltInvDamping);

        m_lpBlJoint->SetLimitValues();
    }
}

	}			// Visualization
}				//BulletAnimatSim
