/**
\file	BellGain.h

\brief	Declares the bell gain class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Gains
	{
			/**
			\brief	Bell gain class.

			\details This gain calculates a bell shaped distribution curve. Out = B*e^(-C*(In-A)^2)+D
			
			\author	dcofer
			\date	3/16/2011
			**/
			class ANIMAT_PORT BellGain : public Gain 
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
				BellGain();
				virtual ~BellGain();
														
				static BellGain *CastToDerived(AnimatBase *lpBase) {return static_cast<BellGain*>(lpBase);}

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
