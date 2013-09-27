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
	m_vxMaterialTable = NULL;
    m_vxMaterial = NULL;
}

VsMaterialType::~VsMaterialType()
{
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
