#pragma once

namespace AnimatSim
{
	namespace Robotics
	{
		class RemoteControl;

		class ANIMAT_PORT PassThroughLinkage : public RemoteControlLinkage
		{
		protected:
			/// Pointer to the Gain that will be used to convert the source value into the target value.
			Gain *m_lpGain;

			virtual void AddGain(std::string strXml);
			virtual float CalculateAppliedValue();

		public:
			PassThroughLinkage(void);
			virtual ~PassThroughLinkage(void);
						
			static PassThroughLinkage *CastToDerived(AnimatBase *lpBase) {return static_cast<PassThroughLinkage*>(lpBase);}

			virtual Gain *GetGain();
			virtual void SetGain(Gain *lpGain);

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void Load(CStdXml &oXml);
		};

	}
}