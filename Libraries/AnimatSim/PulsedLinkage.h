#pragma once

namespace AnimatSim
{
	namespace Robotics
	{
		class RemoteControl;

		class ANIMAT_PORT PulsedLinkage : public RemoteControlLinkage
		{
		protected:
			unsigned int m_iMatchValue;
			float m_fltPulseDuration;
			float m_fltPulseCurrent;

		public:
			PulsedLinkage(void);
			virtual ~PulsedLinkage(void);
						
			static PulsedLinkage *CastToDerived(AnimatBase *lpBase) {return static_cast<PulsedLinkage*>(lpBase);}

			virtual void MatchValue(unsigned int iVal);
			virtual unsigned int MatchValue();

			virtual void PulseDuration(float fltVal);
			virtual float PulseDuration();

			virtual void PulseCurrent(float fltVal);
			virtual float PulseCurrent();

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}