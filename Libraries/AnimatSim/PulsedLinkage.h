#pragma once

namespace AnimatSim
{
	namespace Robotics
	{
		class RemoteControl;

		class ANIMAT_PORT PulsedLinkage : public RemoteControlLinkage
		{
		protected:
#ifndef STD_DO_NOT_ADD_BOOST
			///mutex used to try and access matches variable.
			boost::interprocess::interprocess_mutex m_AccessMatchesMutex;
#endif

			///Counts the number of matches found in StepIO to let the sim know
			int m_iMatches;

			///Used to report up the number of matches
			float m_fltMatchesReport;

			///The value we are trying to match in order to apply a pulse
			int m_iMatchValue;

			///If true then it will only performa a match check when the value has changed.
			bool m_bMatchOnChange;

			///Keeps track of the previous source value for the change check
			unsigned int m_iPrevValue;

			///The duration for which a single pulse should be applied
			float m_fltPulseDuration;

			///The amount of current to apply for a single pulse
			float m_fltPulseCurrent;

			///Each time a pulse happens it will add a new element to the array with the value as the 
			///pulse duration. At each time step we will loop through these to add the pulse current 
			///for each one and subtract the current step duration from the remaining value. 
			///Once the value goes below zero then that element will be removed.
			CStdArray<float> m_aryPulses;

			virtual float CalculateAppliedValue(float fltData);
			void CullPulses();
			void IncrementMatches();

		public:
			PulsedLinkage(void);
			virtual ~PulsedLinkage(void);
						
			static PulsedLinkage *CastToDerived(AnimatBase *lpBase) {return static_cast<PulsedLinkage*>(lpBase);}

			virtual void MatchValue(int iVal);
			virtual int MatchValue();

			virtual void MatchOnChange(bool bVal);
			virtual bool MatchOnChange();

			virtual void PulseDuration(float fltVal);
			virtual float PulseDuration();

			virtual void PulseCurrent(float fltVal);
			virtual float PulseCurrent();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void StepIO();

			virtual void ResetSimulation();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}