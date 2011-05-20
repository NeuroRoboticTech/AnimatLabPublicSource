/**
\file	LengthTensionGain.h

\brief	Declares an inverted quadratic gain class used to calculate length-tension relationship for muscle. 
**/

#pragma once

namespace AnimatSim
{
	namespace Gains
	{
			/**
			\brief	Length-Tension muscle gain. 
					
			\details Declares an inverted quadratic gain class used to calculate length-tension relationship for muscle

			\author	dcofer
			\date	3/16/2011
			**/
			class ANIMAT_PORT LengthTensionGain : public Gain 
			{
			protected:
				/// The resting length of the muscle.
				float m_fltRestingLength;

				/// The width parameter of the curve.
				float m_fltTLwidth;

				///precalculated value used in the determination of the length-tension curve.
				float m_fltTLc;

				///The percentage of the muscle length that is the Pe portion.
				float m_fltPeLengthPercentage;

				///The minimum length of the pe portion of the muscle as a percentage of its total length.
				///The pe length should never be able to go below this value.
				float m_fltMinPeLengthPercentage;

				///The minimum length of the pe portion of the muscle.
				///The pe length should never be able to go below this value.
				float m_fltMinPeLength;

				///Resting length of the se portion of the muscle. Used to calculate SE and PE lengths.
				float m_fltSeRestLength;

			public:
				LengthTensionGain();
				virtual ~LengthTensionGain();
				
				virtual float RestingLength();
				virtual void RestingLength(float fltVal);

				virtual float TLwidth();
				virtual void TLwidth(float fltVal);

				virtual float TLc();

				virtual float PeLengthPercentage();
				virtual void PeLengthPercentage(float fltVal);

				virtual float MinPeLengthPercentage();
				virtual void MinPeLengthPercentage(float fltVal);

				virtual float SeRestLength();
				virtual float MinPeLength();

				virtual float CalculateGain(float fltInput);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatSim
