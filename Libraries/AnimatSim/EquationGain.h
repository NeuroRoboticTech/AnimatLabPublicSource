/**
\file	EquationGain.h

\brief	Declares the equation gain class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Gains
	{
			/**
			\brief	Equation gain. 

			\details This gain is a general type of gain that parses a string equation and evaluates it
			using the supplied input parameter. This <b>will</b> be slower than the gains that use a compiled
			equations, but this type of functionality provides some flexibility for the user to specify equations
			that have not been hard coded.
			
			\author	dcofer
			\date	3/16/2011
			**/
			class ANIMAT_PORT EquationGain : public Gain 
			{
			protected:
				/// The post-fix gain equation
				std::string m_strGainEquation;

				/// The pointer to the postfix equation evaluator
				CStdPostFixEval *m_lpEval;

			public:
				EquationGain();
				virtual ~EquationGain();
			
				static EquationGain *CastToDerived(AnimatBase *lpBase) {return static_cast<EquationGain*>(lpBase);}

				virtual std::string GainEquation();
				virtual void GainEquation(std::string strEquation);
											
				virtual void Copy(CStdSerialize *lpSource);
				virtual CStdSerialize *Clone();

				virtual float CalculateGain(float fltInput);

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void Load(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatSim
