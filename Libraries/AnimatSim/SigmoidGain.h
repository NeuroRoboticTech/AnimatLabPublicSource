/**
\file	C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatSim\SigmoidGain.h

\brief	Declares the sigmoid gain class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Gains
	{
			/**
			\brief	Sigmoid gain. 
					
			\details This gain calculates a sigmoidally shaped distribution curve. Out = D+(B/(1+e^(C*(A-In))))

			\author	dcofer
			\date	3/16/2011
			**/
			class ANIMAT_PORT SigmoidGain : public Gain 
			{
			protected:
				/// The A parameter of the gain.
				float m_fltA;

				/// The B parameter of the gain.
				float m_fltB;

				/// The C parameter of the gain.
				float m_fltC;

				/// The D parameter of the gain.
				float m_fltD;

			public:
				SigmoidGain();
				virtual ~SigmoidGain();
						
				static SigmoidGain *CastToDerived(AnimatBase *lpBase) {return static_cast<SigmoidGain*>(lpBase);}
				
				float A();
				void A(float fltVal);

				float B();
				void B(float fltVal);

				float C();
				void C(float fltVal);

				float D();
				void D(float fltVal);
											
				virtual void Copy(CStdSerialize *lpSource);
				virtual CStdSerialize *Clone();

				virtual float CalculateGain(float fltInput);

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void Load(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatSim
