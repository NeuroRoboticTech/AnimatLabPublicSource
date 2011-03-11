// MuscleBase.h: interface for the MuscleBase class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_MUSCLE_BASE_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_)
#define AFX_MUSCLE_BASE_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
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
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_MUSCLE_BASE_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_)
