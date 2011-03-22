/**
\file	MaterialPair.cpp

\brief	Implements the material pair class.
**/

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
#include "Sensor.h"
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
/**
\brief	Default constructor.

\author	dcofer
\date	3/22/2011
**/
MaterialPair::MaterialPair()
{
	m_fltFrictionPrimary = 1;
	m_fltFrictionSecondary = 1;
	m_fltMaxFrictionPrimary = 500;
	m_fltMaxFrictionSecondary = 500;
	m_fltCompliance = 1e-7f;
	m_fltDamping = 5e4f;
	m_fltRestitution = 0;
	m_fltSlipPrimary = 0;
	m_fltSlipSecondary= 0;
	m_fltSlidePrimary= 0;
	m_fltSlideSecondary= 0;
	m_fltMaxAdhesive = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
MaterialPair::~MaterialPair()
{
}

string MaterialPair::Material1() {return m_strMaterial1;}

void MaterialPair::Material1(string strMat) {m_strMaterial1 = strMat;}

string MaterialPair::Material2() {return m_strMaterial2;}

void MaterialPair::Material2(string strMat) {m_strMaterial2 = strMat;}

float MaterialPair::FrictionPrimary() {return m_fltFrictionPrimary;}

void MaterialPair::FrictionPrimary(float fltVal) {m_fltFrictionPrimary = fltVal;}

float MaterialPair::FrictionSecondary() {return m_fltFrictionSecondary;}

void MaterialPair::FrictionSecondary(float fltVal) {m_fltFrictionSecondary = fltVal;}

float MaterialPair::MaxFrictionPrimary() {return m_fltMaxFrictionPrimary;}

void MaterialPair::MaxFrictionPrimary(float fltVal) {m_fltMaxFrictionPrimary = fltVal;}

float MaterialPair::MaxFrictionSecondary() {return m_fltMaxFrictionSecondary;}

void MaterialPair::MaxFrictionSecondary(float fltVal) {m_fltMaxFrictionSecondary = fltVal;}

float MaterialPair::SlipPrimary() {return m_fltSlipPrimary;}

void MaterialPair::SlipPrimary(float fltVal) {m_fltSlipPrimary = fltVal;}

float MaterialPair::SlipSecondary() {return m_fltSlipSecondary;}

void MaterialPair::SlipSecondary(float fltVal) {m_fltSlipSecondary = fltVal;}

float MaterialPair::SlidePrimary() {return m_fltSlidePrimary;}

void MaterialPair::SlidePrimary(float fltVal) {m_fltSlidePrimary = fltVal;}

float MaterialPair::SlideSecondary() {return m_fltSlideSecondary;}

void MaterialPair::SlideSecondary(float fltVal) {m_fltSlideSecondary = fltVal;}

float MaterialPair::Compliance() {return m_fltCompliance;}

void MaterialPair::Compliance(float fltVal) {m_fltCompliance = fltVal;}

float MaterialPair::Damping() {return m_fltDamping;}

void MaterialPair::Damping(float fltVal) {m_fltDamping = fltVal;}

float MaterialPair::Restitution() {return m_fltRestitution;}

void MaterialPair::Restitution(float fltVal) {m_fltRestitution = fltVal;}

float MaterialPair::MaxAdhesive() {return m_fltMaxAdhesive;}

void MaterialPair::MaxAdhesive(float fltVal) {m_fltMaxAdhesive = fltVal;}

void MaterialPair::ScaleUnits()
{
	//scale the varios units to be consistent
	//Friction coefficients are unitless
	m_fltMaxFrictionPrimary *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 
	m_fltMaxFrictionSecondary *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force. 
	m_fltCompliance *= m_lpSim->MassUnits();  //Compliance units are m/N or s^2/Kg
	m_fltDamping *= m_lpSim->InverseMassUnits();
	m_fltSlipPrimary *= m_lpSim->MassUnits();  //Slip units are s/Kg
	m_fltSlipSecondary *= m_lpSim->MassUnits();  
	m_fltSlidePrimary *= m_lpSim->InverseDistanceUnits(); //slide is a velocity so units are m/s
	m_fltSlidePrimary *= m_lpSim->InverseDistanceUnits();
	m_fltMaxAdhesive *= (m_lpSim->InverseMassUnits() * m_lpSim->InverseDistanceUnits()); //This is a force.
}

void MaterialPair::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into MaterialPair Element

	m_strMaterial1 = Std_ToUpper(oXml.GetChildString("Material1"));
	if(Std_IsBlank(m_strMaterial1))
		THROW_ERROR(Al_Err_lMaterialPairBlank, Al_Err_strMaterialPairBlank);

	m_strMaterial2 = Std_ToUpper(oXml.GetChildString("Material2"));
	if(Std_IsBlank(m_strMaterial2))
		THROW_ERROR(Al_Err_lMaterialPairBlank, Al_Err_strMaterialPairBlank);

	m_fltFrictionPrimary = oXml.GetChildFloat("FrictionPrimary", m_fltFrictionPrimary);
	m_fltFrictionSecondary = oXml.GetChildFloat("FrictionSecondary", m_fltFrictionSecondary);
	m_fltMaxFrictionPrimary = oXml.GetChildFloat("MaxFrictionPrimary", m_fltMaxFrictionPrimary);
	m_fltMaxFrictionSecondary = oXml.GetChildFloat("MaxFrictionSecondary", m_fltMaxFrictionSecondary);
	m_fltCompliance = oXml.GetChildFloat("Compliance", m_fltCompliance);
	m_fltDamping = oXml.GetChildFloat("Damping", m_fltDamping);
	m_fltRestitution = oXml.GetChildFloat("Restitution", m_fltRestitution);
	m_fltSlipPrimary = oXml.GetChildFloat("SlipPrimary", m_fltSlipPrimary);
	m_fltSlipSecondary = oXml.GetChildFloat("SlipSecondary", m_fltSlipSecondary);
	m_fltSlidePrimary = oXml.GetChildFloat("SlidePrimary", m_fltSlidePrimary);
	m_fltSlideSecondary = oXml.GetChildFloat("SlideSecondary", m_fltSlideSecondary);
	m_fltMaxAdhesive = oXml.GetChildFloat("MaxAdhesive", m_fltMaxAdhesive);

	if(m_fltRestitution < 0)
		m_fltRestitution = 0;
	if(m_fltRestitution > 1)
		m_fltRestitution = 1;

	ScaleUnits();

	oXml.OutOfElem(); //OutOf MaterialPair Element

}

	}			// Visualization
}				//VortexAnimatSim
