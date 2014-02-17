// VsConstraintRelaxation.cpp: implementation of the VsConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsConstraintRelaxation.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsConstraintRelaxation::VsConstraintRelaxation()
{
	m_fltLoss = 0;
}

VsConstraintRelaxation::~VsConstraintRelaxation()
{
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
float VsConstraintRelaxation::Loss() {return m_fltLoss;}

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
void VsConstraintRelaxation::Loss(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "Loss", true);

	if(bUseScaling)
		fltVal *= m_lpSim->MassUnits();  //Slip units are s/Kg

	m_fltLoss = fltVal;
	SetRelaxationProperties();
}

void VsConstraintRelaxation::Initialize()
{
	ConstraintRelaxation::Initialize();

	SetRelaxationProperties();
}

/**
\brief	This takes the default values defined in the constructor and scales them according to
the distance and mass units to be acceptable values.

\author	dcofer
\date	3/23/2011
**/
void VsConstraintRelaxation::CreateDefaultUnits()
{
    ConstraintRelaxation::CreateDefaultUnits();

    m_fltLoss *= m_lpSim->MassUnits();  //Slip units are s/Kg
}

bool VsConstraintRelaxation::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(ConstraintRelaxation::SetData(strType, strValue, false))
		return true;

	if(strType == "LOSS")
	{
		Loss((float) atof(strValue.c_str()));
		return true;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void VsConstraintRelaxation::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	ConstraintRelaxation::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Loss");
	aryTypes.Add("Float");
}

void VsConstraintRelaxation::Load(CStdXml &oXml)
{
	ConstraintRelaxation::Load(oXml);

	oXml.IntoElem();  //Into ConstraintRelaxation Element

	Loss(oXml.GetChildFloat("Loss", m_fltLoss));

    oXml.OutOfElem(); //OutOf ConstraintRelaxation Element

}

void VsConstraintRelaxation::SetRelaxationProperties()
{
    if(m_lpSim && m_lpNode)
    {
        VsJoint *lpJoint = dynamic_cast<VsJoint *>(m_lpNode);
        if(lpJoint)
        {
            Vx::VxConstraint *vxConstraint = lpJoint->Constraint();

            if(vxConstraint)
               vxConstraint->setRelaxationParameters(m_iCoordinateID, m_fltStiffness, m_fltDamping, m_fltLoss, m_bEnabled);
        }
    }
}

	}			// Visualization
}				//VortexAnimatSim
