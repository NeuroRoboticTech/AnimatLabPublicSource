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

void VsMaterialPair::RegisterMaterialTypes(Simulator *lpSim, CStdArray<string> aryMaterialTypes)
{
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);
	
    m_vxMaterialTable = lpVsSim->Frame()->getMaterialTable();

	string strMat;
	int iCount = aryMaterialTypes.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		strMat = aryMaterialTypes[iIndex];
		m_vxMaterialTable->registerMaterial(strMat.c_str());
	}
}

void VsMaterialPair::Initialize(Simulator *lpSim)
{
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);
	
	m_vxMaterialTable = lpVsSim->Frame()->getMaterialTable();

	VxContactProperties *pM = m_vxMaterialTable->getContactProperties(m_strMaterial1.c_str(), m_strMaterial2.c_str());
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
	pM = m_vxMaterialTable->getContactProperties(m_strMaterial1.c_str(), m_strMaterial2.c_str());
	float fltVal = pM->getFrictionCoefficientPrimary();
}

	}			// Visualization
}				//VortexAnimatSim
