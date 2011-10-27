// VsMaterialPair.cpp: implementation of the VsMaterialPair class.
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

VsMaterialPair::VsMaterialPair()
{
	m_vxMaterialTable = NULL;
}

VsMaterialPair::~VsMaterialPair()
{
}

int VsMaterialPair::GetMaterialID(string strName)
{
	if(!m_vxMaterialTable)
		THROW_ERROR(Vs_Err_lMaterial_Table_Not_Defined, Vs_Err_strMaterial_Table_Not_Defined);

	return m_vxMaterialTable->getMaterialID(strName.c_str());
}

void VsMaterialPair::RegisterMaterialType(string strID)
{
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);
	
    m_vxMaterialTable = lpVsSim->Frame()->getMaterialTable();

	m_vxMaterialTable->registerMaterial(strID.c_str());
}

void VsMaterialPair::Initialize()
{
	MaterialPair::Initialize();

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);
	
	m_vxMaterialTable = lpVsSim->Frame()->getMaterialTable();

	VxContactProperties *pM = m_vxMaterialTable->getContactProperties(m_strMaterial1ID.c_str(), m_strMaterial2ID.c_str());
    pM->setFrictionType(VxContactProperties::kFrictionTypeTwoDirection);
    pM->setFrictionModel(VxContactProperties::kFrictionModelScaledBox);
    pM->setFrictionCoefficientPrimary(m_fltFrictionPrimary);
    pM->setFrictionCoefficientSecondary(m_fltFrictionSecondary);
	pM->setBoxFrictionForcePrimary(m_fltMaxFrictionPrimary);
	pM->setBoxFrictionForceSecondary(m_fltMaxFrictionSecondary);
    pM->setCompliance(m_fltCompliance);
    pM->setDamping(m_fltDamping);
    pM->setRestitution(m_fltRestitution);
    pM->setSlipPrimary(m_fltSlipPrimary);
    pM->setSlipSecondary(m_fltSlipSecondary);
    pM->setSlidePrimary(m_fltSlidePrimary);
    pM->setSlideSecondary(m_fltSlideSecondary);
    pM->setAdhesiveForce(m_fltMaxAdhesive);

	pM = NULL;
	pM = m_vxMaterialTable->getContactProperties(m_strMaterial1ID.c_str(), m_strMaterial2ID.c_str());
	float fltVal = pM->getFrictionCoefficientPrimary();
}

	}			// Visualization
}				//VortexAnimatSim
