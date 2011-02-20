// MaterialPair.h: interface for the MaterialPair class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace AnimatSim
{
	   // materialTable->registerMaterial("boxMaterial");

    //VxContactProperties *pM;
    //pM = materialTable->getContactProperties("groundMaterial", "boxMaterial");
    //pM->setFrictionType(VxContactProperties::kFrictionTypeTwoDirection);
    //pM->setFrictionModel(VxContactProperties::kFrictionModelScaledBox);
    //pM->setFrictionCoefficientPrimary(1.0f);
    //pM->setFrictionCoefficientSecondary(1.3f);
    //pM->setCompliance(5.0e-7f);
    //pM->setDamping(2.0e5f);
    //pM->setSlipPrimary(0.00001f);   // some slip is necessary to allow the vehicle to turn
    //pM->setSlipSecondary(0.00001f); // some slip is necessary to allow the vehicle to turn

	namespace Environment
	{

		class ANIMAT_PORT MaterialPair : public AnimatBase
		{
		protected:
			string m_strMaterial1;
			string m_strMaterial2;

			float m_fltFrictionPrimary;
			float m_fltFrictionSecondary;
			float m_fltMaxFrictionPrimary;
			float m_fltMaxFrictionSecondary;
			float m_fltCompliance;
			float m_fltDamping;
			float m_fltRestitution;
			float m_fltSlipPrimary;
			float m_fltSlipSecondary;
			float m_fltSlidePrimary;
			float m_fltSlideSecondary;
			float m_fltMaxAdhesive;

		public:
			MaterialPair();
			virtual ~MaterialPair();
			
			virtual string Material1() {return m_strMaterial1;};
			virtual void Material1(string strMat) {m_strMaterial1 = strMat;};

			virtual string Material2() {return m_strMaterial2;};
			virtual void Material2(string strMat) {m_strMaterial2 = strMat;};

			virtual float FrictionPrimary() {return m_fltFrictionPrimary;};
			virtual void FrictionPrimary(float fltVal) {m_fltFrictionPrimary = fltVal;};
			virtual float FrictionSecondary() {return m_fltFrictionSecondary;};
			virtual void FrictionSecondary(float fltVal) {m_fltFrictionSecondary = fltVal;};

			virtual float MaxFrictionPrimary() {return m_fltMaxFrictionPrimary;};
			virtual void MaxFrictionPrimary(float fltVal) {m_fltMaxFrictionPrimary = fltVal;};
			virtual float MaxFrictionSecondary() {return m_fltMaxFrictionSecondary;};
			virtual void MaxFrictionSecondary(float fltVal) {m_fltMaxFrictionSecondary = fltVal;};

			virtual float SlipPrimary() {return m_fltSlipPrimary;};
			virtual void SlipPrimary(float fltVal) {m_fltSlipPrimary = fltVal;};
			virtual float SlipSecondary() {return m_fltSlipSecondary;};
			virtual void SlipSecondary(float fltVal) {m_fltSlipSecondary = fltVal;};

			virtual float SlidePrimary() {return m_fltSlidePrimary;};
			virtual void SlidePrimary(float fltVal) {m_fltSlidePrimary = fltVal;};
			virtual float SlideSecondary() {return m_fltSlideSecondary;};
			virtual void SlideSecondary(float fltVal) {m_fltSlideSecondary = fltVal;};

			virtual float Compliance() {return m_fltCompliance;};
			virtual void Compliance(float fltVal) {m_fltCompliance = fltVal;};

			virtual float Damping() {return m_fltDamping;};
			virtual void Damping(float fltVal) {m_fltDamping = fltVal;};

			virtual float Restitution() {return m_fltRestitution;};
			virtual void Restitution(float fltVal) {m_fltRestitution = fltVal;};

			virtual float MaxAdhesive() {return m_fltMaxAdhesive;};
			virtual void MaxAdhesive(float fltVal) {m_fltMaxAdhesive = fltVal;};

			virtual void ScaleUnits(Simulator *lpSim);
			virtual int GetMaterialID(string strName) = 0;
			virtual void Initialize(Simulator *lpSim) = 0;
			virtual void RegisterMaterialTypes(Simulator *lpSim, CStdArray<string> aryMaterialTypes) = 0;
			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
