// BlMaterialType.cpp: implementation of the BlMaterialType class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlMaterialType::BlMaterialType()
{
    //FIX PHYSICS
    //m_vxMaterialTable = NULL;
    //m_vxMaterial = NULL;
}

BlMaterialType::~BlMaterialType()
{
}

int BlMaterialType::GetMaterialID(string strName)
{
    //FIX PHYSICS
 //   if(!m_vxMaterialTable)
	//	THROW_ERROR(Bl_Err_lMaterial_Table_Not_Defined, Bl_Err_strMaterial_Table_Not_Defined);

	//return m_vxMaterialTable->getMaterialID(strName.c_str());
    return 0;
}

void BlMaterialType::RegisterMaterialType()
{
    //FIX PHYSICS
    //if(!m_vxMaterial)
    //{
	   // BlSimulator *lpVsSim = dynamic_cast<BlSimulator *>(m_lpSim);
	   // if(!lpVsSim)
		  //  THROW_ERROR(Bl_Err_lUnableToConvertToBlSimulator, Bl_Err_strUnableToConvertToBlSimulator);
	
    //    m_vxMaterialTable = lpVsSim->Frame()->getMaterialTable();

	   // m_vxMaterial = m_vxMaterialTable->registerMaterial(m_strID.c_str());
    //}
}

void BlMaterialType::Initialize()
{
	MaterialType::Initialize();

    RegisterMaterialType();
	SetMaterialProperties();
}

void BlMaterialType::SetMaterialProperties()
{
    //FIX PHYSICS
	//if(m_lpSim && m_vxMaterial)
	//{
 //       m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisLinear, VxContactMaterial::kFrictionModelScaledBox);

 //       m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltFrictionLinearPrimary);
 //       m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltFrictionLinearPrimaryMax);

 //       m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltFrictionLinearSecondary);
 //       m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltFrictionLinearSecondaryMax);

 //       if(m_fltFrictionAngularNormal > 0)
 //       {
 //           m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularNormal, VxContactMaterial::kFrictionModelScaledBox);
 //           m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisAngularNormal, m_fltFrictionAngularNormal);
 //           m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisAngularNormal, m_fltFrictionAngularNormalMax);
 //       }
 //       else
 //           m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularNormal, VxContactMaterial::kFrictionModelNeutral);

 //       if(m_fltFrictionAngularPrimary > 0)
 //       {
 //           m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularPrimary, VxContactMaterial::kFrictionModelScaledBox);
 //           m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltFrictionAngularPrimary);
 //           m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltFrictionAngularPrimaryMax);
 //       }
 //       else
 //           m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularPrimary, VxContactMaterial::kFrictionModelNeutral);

 //       if(m_fltFrictionAngularSecondary > 0)
 //       {
 //           m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularSecondary, VxContactMaterial::kFrictionModelScaledBox);
 //           m_vxMaterial->setFrictionCoefficient(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltFrictionAngularSecondary);
 //           m_vxMaterial->setBoxFrictionForce(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltFrictionAngularSecondaryMax);
 //       }
 //       else
 //           m_vxMaterial->setFrictionModel(VxContactMaterial::kFrictionAxisAngularSecondary, VxContactMaterial::kFrictionModelNeutral);

 //       m_vxMaterial->setCompliance(m_fltCompliance);
	//    m_vxMaterial->setDamping(m_fltDamping);
	//    m_vxMaterial->setRestitution(m_fltRestitution);

 //       m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltSlipLinearPrimary);
 //       m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltSlipLinearSecondary);
 //       m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisAngularNormal, m_fltSlipAngularNormal);
 //       m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltSlipAngularPrimary);
 //       m_vxMaterial->setSlip(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltSlipAngularSecondary);

 //       m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisLinearPrimary, m_fltSlideLinearPrimary);
 //       m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisLinearSecondary, m_fltSlideLinearSecondary);
 //       m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisAngularNormal, m_fltSlideAngularNormal);
 //       m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisAngularPrimary, m_fltSlideAngularPrimary);
 //       m_vxMaterial->setSlide(VxContactMaterial::kFrictionAxisAngularSecondary, m_fltSlideAngularSecondary);

	//    m_vxMaterial->setAdhesiveForce(m_fltMaxAdhesive);
	//}
}

	}			// Visualization
}				//BulletAnimatSim
