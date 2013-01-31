/**
\file	PolynomialGain.h

\brief	Declares the polynomial gain class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Gains
	{
			/**
			\brief	Polynomial gain. 
		
			\details This gain calculates a polynomial shaped distribution curve. Out = A*In^3 + B*In^2 + C*In + D

			\author	dcofer
			\date	3/16/2011
			**/
			class ANIMAT_PORT PolynomialGain : public Gain 
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
				PolynomialGain();
				virtual ~PolynomialGain();

				float A();
				void A(float fltVal);

				float B();
				void B(float fltVal);

				float C();
				void C(float fltVal);

				float D();
				void D(float fltVal);

				virtual float CalculateGain(float fltInput);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
				virtual void Load(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatSim
