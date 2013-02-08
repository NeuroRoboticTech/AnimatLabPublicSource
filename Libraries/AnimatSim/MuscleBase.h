/**
\file	MuscleBase.h

\brief	Declares the muscle base class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/**
			\brief	Muscle base class.

			\details This is a base class for all muscle part types.
			
			\author	dcofer
			\date	5/19/2011
			**/
			class ANIMAT_PORT MuscleBase : public LineBase  
			{
			protected:
				///The maximum tension that this muscle can ever generate. This is an upper limit to prevent unrealistic tension values.
				float m_fltMaxTension;

				///Keeps track of the total stimulation being supplied to this muscle.
				///This is the summation of the firing frequenies from all neurons
				///that are stimulating this muscle.
				float m_fltVm;

				///The derivative of tension at the current time step.
				float m_fltTdot;

				///Tension of the muscle
				float m_fltTension;

				///Tension of the muscle in the last time slice.
				float m_fltPrevTension;

				/// The stimulus-tension gain
				SigmoidGain m_gainStimTension;

				/// The length-tension gain
				LengthTensionGain m_gainLengthTension;

				/**
				\brief	Calculates the tension. 
				
				\author	dcofer
				\date	3/10/2011
				**/
				virtual void CalculateTension() = 0;

			public:
				MuscleBase();
				virtual ~MuscleBase();

				float Tension();
				void Tension(float fltVal);

				float MaxTension();
				void MaxTension(float fltVal);

				float Vm();
				float Tdot();
				float PrevTension();

				virtual BOOL Enabled();
				virtual void Enabled(BOOL bVal);

				virtual SigmoidGain *StimTension();
				virtual void StimTension(string strXml);

				virtual LengthTensionGain *LengthTension();
				virtual void LengthTension(string strXml);

				/**
				\brief	Calculates the activation needed for a given tension value. 
				
				\author	dcofer
				\date	3/10/2011
				
				\param	fltLength		Length of the muscle. 
				\param	fltVelocity		The velocity of change in muscle length. 
				\param	fltT			The tension. 
				\param [in,out]	fltVm	The required voltage activation level. 
				\param [in,out]	fltA	The required activation level. 
				**/
				virtual void CalculateInverseDynamics(float fltLength, float fltVelocity, float fltT, float &fltVm, float &fltA) = 0;
				virtual void AddExternalNodeInput(float fltInput);

				virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
				virtual void VerifySystemPointers();

				virtual void ResetSimulation();

				virtual float *GetDataPointer(const string &strDataType);
				virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
