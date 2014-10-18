/**
\file	CsNeuronDataColumn.h

\brief	Declares the data column class.
**/

#pragma once

namespace AnimatCarlSim
{
		/**
		\brief	Carlsim Data column class.

		\details Carlsim data columns only update their data at the end of the data chart run. The data, like spike times, are stored in the various
		CsNeuronGroup classes from the SpikeMonitor. These run in a separate thread, so we cannot update them using the standard StepSimulation mechanism
		that other DataColumns use. The easiest solution was to simply make sure the other threads are paused and then update the chart data when the chart
		was deactivated.
		
		\author	dcofer
		\date	10/12/2014
		**/
		class ANIMAT_CARL_SIM_PORT CsNeuronDataColumn : public AnimatSim::Charting::DataColumn  
		{
		protected:
			///The Target item is queried during initialization to see if the data type for this column needs for its
			///data to be updated at the end when it is deactivated, or if it is okay to do it the normal way.
			bool m_bAddDataAtEnd;

			CsNeuronGroup *m_lpNeuron;

			int m_iNeuronID;

			std::string m_strNeuronID;

			virtual void FillInDeactivateData();

		public:
			CsNeuronDataColumn();
			virtual ~CsNeuronDataColumn();
			
			static CsNeuronDataColumn *CastToDerived(AnimatBase *lpBase) {return static_cast<CsNeuronDataColumn*>(lpBase);}

			virtual void NeuronID(const std::string &strValue);
			virtual unsigned int NeuronID();

			virtual void DataType(std::string strType);

#pragma region DataAccesMethods
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
#pragma endregion

			virtual void Initialize();
			virtual void Deactivate();
			virtual void Load(CStdXml &oXml);
		};

}				//AnimatSim
