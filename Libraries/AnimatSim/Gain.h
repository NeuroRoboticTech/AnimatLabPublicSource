/**
\file	Gain.h

\brief	Declares the gain base class. 
**/

#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Gains

	\brief	Contains the different gain type classes that can be used. 

	\details A gain is essentially an equation to convert one parameter into a different value. It is 
	used extensively throughout the system, for example in the adapters.
	**/
	namespace Gains
	{
		/**
		\brief	The Gain base class. 

		\details Gains are objects that convert one numeric value into a different value by applying 
		it to a function. For speed purposes, we do not want to use parsed functions within the simulation.
		To get around this you can define different gain functions that implement specific equations that are
		then compiled. The user can specify parameters for the function that are then evaluated at runtime.
		This is the base class for all of these types of objects. Just derive your new gain from this class 
		for it to be usable in the system.
		
		\author	dcofer
		\date	3/16/2011
		**/
		class ANIMAT_PORT Gain : public AnimatBase 
		{
		protected:
			/// Determines whether or not the gain uses upper and lower limits during its calculations.
			bool m_bUseLimits;

			/// The lower limit value that is checked if UseLimits is true.
			float m_fltLowerLimit;

			/// The lower limit output that is used if UseLimits is true and the input is below the lower limit.
			float m_fltLowerOutput;

			/// The upper limit value that is checked if UseLimits is true.
			float m_fltUpperLimit;

			/// The upper limit output that is used if UseLimits is true and the input is above the upper limit.
			float m_fltUpperOutput;

			/**
			\brief	Tells whether the input value is within the defined limit ranges. 
			
			\author	dcofer
			\date	3/16/2011
			
			\param	fltInput	The input value. 
			
			\return	true if it is within limits, false otherwise. 
			**/
			bool InLimits(float fltInput)
			{
				if( m_bUseLimits && ( (fltInput < m_fltLowerLimit) || (fltInput > m_fltUpperLimit) ) ) 
					return false;
				else
					return true;
			}

			/**
			\brief	Calculates the output when the input is outside of the limit ranges. 
			
			\author	dcofer
			\date	3/16/2011
			
			\param	fltInput	The input value. 
			
			\return	The calculated limit output. 
			**/
			float CalculateLimitOutput(float fltInput)
			{
				if(fltInput < m_fltLowerLimit)
					return m_fltLowerOutput;

				if(fltInput > m_fltUpperLimit)
					return m_fltUpperOutput;

				return 0;
			}

		public:
			Gain();
			virtual ~Gain();

			bool UseLimits();
			void UseLimits(bool bVal);

			float LowerLimit();
			void LowerLimit(float fltVal);

			float UpperLimit();
			void UpperLimit(float fltVal);

			float LowerOutput();
			void LowerOutput(float fltVal);

			float UpperOutput();
			void UpperOutput(float fltVal);

			/**
			\brief	Calculates the gain. 

			\details This evaluates the gain function to convert the input value into an output value.
			This is the method that you must override in your derived class. It is where you place the
			new gain functionality.
			
			\author	dcofer
			\date	3/16/2011
			
			\param	fltInput	The flt input. 
			
			\return	The calculated gain. 
			**/
			virtual float CalculateGain(float fltInput) = 0;

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);
			virtual void Load(CStdXml &oXml);
		};

		Gain ANIMAT_PORT *LoadGain(Simulator *lpSim, std::string strName, CStdXml &oXml);

	}			//Gains
}				//AnimatSim
