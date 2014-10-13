/**
\file	CsFiringRateStimulus.h

\brief	Declares the current stimulus class. 
**/

#pragma once


namespace AnimatCarlSim
{
		/**
		\brief	Current stimulus for neural items. 

		\details 
		
		\author	dcofer
		\date	3/16/2011
		**/
		class ANIMAT_CARL_SIM_PORT CsFiringRateStimulus  : public ExternalStimulus
		{
		protected:
			/// GUID ID of the neuron we are stimulating.
			std::string m_strTargetNodeID;

			///The spike generator this object is stimulating
			CsSpikeGeneratorGroup *m_lpSpikeGen;

			/// The post-fix string current equation
			std::string m_strEquation;

			/// Pointer to the post-fix equation evaluator.
			CStdPostFixEval *m_lpEval;

			///The currently active firing rate
			float m_fltActiveRate;

			///The previous active firing rate
			float m_fltPrevAppliedRate;

			///The value to use for active rate if the type is constant
			float m_fltConstantRate;

			bool m_bStimWholePopulation;

			///An array of neuron indices for individual neurons we want to stimulate
			CStdMap<int, int> m_aryCellsToStim;

			virtual float CalculateFiringRate();
			virtual void LoadCellsToStim(CStdXml &oXml);
			virtual void ApplyRateChange();

		public:
			CsFiringRateStimulus();
			virtual ~CsFiringRateStimulus();
			
			static CsFiringRateStimulus *CastToDerived(AnimatBase *lpBase) {return static_cast<CsFiringRateStimulus*>(lpBase);}
			
			virtual std::string TargetNodeID();
			virtual void TargetNodeID(std::string strID);

			virtual std::string Equation();
			virtual void Equation(std::string strEquation);

			virtual float ConstantRate();
			virtual void ConstantRate(float fltVal);

			virtual float ActiveRate();
			virtual float PrevAppliedRate();

			virtual bool RateChanged();

			virtual void Coverage(std::string strType);

			virtual bool StimWholePopulation();
			virtual void StimWholePopulation(bool bVal);

			virtual void CellsToStim(std::string strXml);

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual void Initialize();  
			virtual void ResetSimulation();  
			virtual void Activate();
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		};

}				//VortexAnimatSim
