/**
\file	CsAdapter.h

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
		class ANIMAT_CARL_SIM_PORT CsAdapter  : public AnimatSim::Adapters::Adapter
		{
		protected:
			///The spike generator this adapter is stimulating
			CsSpikeGeneratorGroup *m_lpSpikeGen;

			bool m_bStimWholePopulation;

			///An array of neuron indices for individual neurons we want to stimulate
			CStdMap<int, int> m_aryCellsToStim;

			///Last applied rate.
			float m_fltPrevAppliedRate;

			virtual void LoadCellsToStim(CStdXml &oXml);

			virtual void ApplyExternalNodeInput(int iTargetDataType, float fltRate);
			virtual bool RateChanged(float fltRate);

		public:
			CsAdapter();
			virtual ~CsAdapter();
			
			static CsAdapter *CastToDerived(AnimatBase *lpBase) {return static_cast<CsAdapter*>(lpBase);}

			virtual void Coverage(std::string strType);

			virtual bool StimWholePopulation();
			virtual void StimWholePopulation(bool bVal);

			virtual void CellsToStim(std::string strXml);

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual void Initialize();  
			virtual void ResetSimulation();  

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		};

}				//VortexAnimatSim
