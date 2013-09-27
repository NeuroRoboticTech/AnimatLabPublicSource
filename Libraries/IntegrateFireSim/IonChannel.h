
#pragma once

namespace IntegrateFireSim
{
	/**
	\brief	Ion channel. 

	\details This implemements a basic Hodgkin-Huxely ion channel that can be added to the Neuron.
	
	\author	dcofer
	\date	3/31/2011
	**/
	class ADV_NEURAL_PORT IonChannel : public AnimatSim::AnimatBase 
	{
	protected:
		/// The initial conductance.
		float m_fltGInit;

		/// The initial activation
		float m_fltMInit;

		/// The initial inactivation
		float m_fltHInit;

		/// true to enable, false to disable
		bool m_bEnabled;

		/// The maximum conductance
		float m_fltGmax;

		/// The active conductance
		float m_fltG;

		/// The activation exponent in the equation.
		float m_fltMPower;

		/// The inactivation exponent in the equation.
		float m_fltHPower;

		/// The equilibrium potential
		float m_fltEquilibriumPotential;

		/// The current activation level
		float m_fltM;

		/// The Nm
		float m_fltNm;

		/// The Minf gain function
		AnimatSim::Gains::Gain *m_lpMinf;

		/// The Tm gain function
		AnimatSim::Gains::Gain *m_lpTm;

		/// The current inactivation level.
		float m_fltH;

		/// The Nh
		float m_fltNh;

		/// The Hinf gain function
		AnimatSim::Gains::Gain *m_lpHinf;

		/// The Th gain function
		AnimatSim::Gains::Gain *m_lpTh;

		//Calculated variables.
		/// The total activation level
		float m_fltTotalAct;

		/// The current
		float m_fltI;

		/// The Minf value
		float m_fltMinf;

		/// The Hinf value
		float m_fltHinf;

		/// The Tm value
		float m_fltTm;

		/// The Th value
		float m_fltTh;

	public:
		IonChannel();
		virtual ~IonChannel();

#pragma region Accessor-Mutators

		void Enabled(bool bVal);
		bool Enabled();

		void Gmax(float fltVal);
		float Gmax();

		void Ginit(float fltVal);
		float Ginit();

		void Hinit(float fltVal);
		float Hinit();

		void Minit(float fltVal);
		float Minit();

		void MPower(float fltVal);
		float MPower();

		void HPower(float fltVal);
		float HPower();

		void EquilibriumPotential(float fltVal);
		float EquilibriumPotential();

		void Nm(float fltVal);
		float Nm();

		void Nh(float fltVal);
		float Nh();

		AnimatSim::Gains::Gain *Minf();
		void Minf(AnimatSim::Gains::Gain *lpGain);
		void Minf(std::string strXml);

		AnimatSim::Gains::Gain *Tm();
		void Tm(AnimatSim::Gains::Gain *lpGain);
		void Tm(std::string strXml);

		AnimatSim::Gains::Gain *Hinf();
		void Hinf(AnimatSim::Gains::Gain *lpGain);
		void Hinf(std::string strXml);

		AnimatSim::Gains::Gain *Th();
		void Th(AnimatSim::Gains::Gain *lpGain);
		void Th(std::string strXml);

#pragma endregion

#pragma region DataAccesMethods

		virtual float *GetDataPointer(const std::string &strDataType);
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

#pragma endregion

		virtual float CalculateCurrent(float fltStep, float fltVm);

		virtual void Load(CStdXml &oXml);
		virtual void ResetSimulation();
	};
}				//AnimatSim
