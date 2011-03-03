// LinearHillMuscle.h: interface for the LinearHillMuscle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LINEAR_HILL_MUSCLE_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_)
#define AFX_LINEAR_HILL_MUSCLE_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/*! \brief 
				A muscle that is connected between two attachment points.
			   
				\remarks
				A muscle generates a contractile force between two rigid bodies 
				in order to create a torque. Typically one or more motor neurons 
				will be connected to a Muscle. At each time step the each of
				those muscles will apply add a force value to the muscle based
				on their firing frequency and output function. These force values
				are additive. So if you have two motor neurons and one is firing
				enough to add 5 Newtons of force and the other is firing enough
				to add 2 Newtons of force then the total magnitude of the contractile
				force for this muscle is 7 Newtons.

				This specifies the magnitude of the force to apply, but force is a 
				vector. We need to know not only the magnitude of the force but where
				that force is centered and the direction. This is calculated using the
				muscle attachment points. Two vectors are calculated. The first from the
				child attachment point to the parent attachment point, and the second
				in the opposite direction. A force is then added to both the child and
				parent centered at their attachment points and directed toward the 
				attachment point on the other body. The magnitude for each is half 
				the total force magnitude. The muscle pulls on each body.

				Typically the muscle does not have a graphical represntation in the 
				simulation. It is only a virtual body part. It also does not take
				part in any of the collisions. Its sole purpose is to apply forces
				to the other body parts.

				\sa
				Body, Joint, CAlBox, CAlPlane, CAlCylinder, 
				CAlCone, Muscle, Attachment, CAlSphere                                
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT LinearHillMuscle : public MuscleBase  
			{
			protected:
				///Spring constant for the series spring.
				float m_fltKse;
				
				///Spring constant for the parrallel spring.
				float m_fltKpe;
				
				///Damping coefficient.
				float m_fltB;

				///Precalculation of Kse divided by B
				float m_fltKseByB;

				///Precalculation of (1+(Kpe/Kse))
				float m_fltKpeByKse;

				///resting length of the muscle.
				float m_fltMuscleRestingLength;

				///Resting length of the se portion of the muscle. Used to calculate SE and PE lengths.
				float m_fltSeRestLength;

				///The length of the SE section of the muscle. The resting length is one half the total muscle length
				float m_fltSeLength;

				///The displacement of the SE section of the muscle.
				float m_fltSeDisplacement;

				///The length of the PE section of the muscle. The resting length is one half the total muscle length
				float m_fltPeLength;

				///The previous length of the se section of the muscle.
				float m_fltSeLPrev;

				///The previous length of the pe section of the muscle.
				float m_fltPeLPrev;

				///The velocity of change of the SE section of the muscle.
				float m_fltVse;

				///The velocity of change of the PE section of the muscle.
				float m_fltVpe;

				///The percentage of the muscle length that is the Pe portion.
				float m_fltPeLengthPercentage;

				///The minimum length of the pe portion of the muscle as a percentage of its total length.
				///The pe length should never be able to go below this value.
				float m_fltMinPeLengthPercentage;

				///The minimum length of the pe portion of the muscle.
				///The pe length should never be able to go below this value.
				float m_fltMinPeLength;

				///First coefficient of the sigmoidal active force curve. This curve is of the form:
				///A = A1/(1 + exp(-(FiringFreq-A2)/A3)). A1 is the maximum force generated at tetanus.
				float m_fltA1;

				///Second coefficient of the sigmoidal active force curve. This curve is of the form:
				///A = A1/(1 + exp(-(FiringFreq-A2)/A3)). A2 is the center of the sigmoidal curve.
				float m_fltA2;

				///Third coefficient of the sigmoidal active force curve. This curve is of the form:
				///A = A1/(1 + exp(-(FiringFreq-A2)/A3)). A3 is the how fast the curve slopes up.
				float m_fltA3;

				///Fourth coefficient of the sigmoidal active force curve. This curve is of the form:
				///A = A1/(1 + exp(-(FiringFreq-A2)/A3)). A3 is the how fast the curve slopes up.
				float m_fltA4;

				///Determines if the limits are used when calculating the stimulus tension curve.
				BOOL m_bUseStimLimits;

				///Lower limit (x) for use in stimulus-tension curve. Only used if m_bUseStimLimits is true.
				float m_fltStimLowerLimit;

				///uppper limit (x) for use in stimulus-tension curve. Only used if m_bUseStimLimits is true.
				float m_fltStimUpperLimit;

				///Lower output (y) for use in stimulus-tension curve. Only used if m_bUseStimLimits is true.
				float m_fltStimLowerOutput;

				///Lower output (y) for use in stimulus-tension curve. Only used if m_bUseStimLimits is true.
				float m_fltStimUpperOutput;

				///This is the amount of displacement of this muscle from its resting length.
				float m_fltDisplacement;

				///This is the ratio of displacement of this muscle from its resting length.
				float m_fltDisplacementRatio;

				///The proportion of active force developed that can actually be used at this muscle length. ranges from 0-1
				float m_fltTL;

				///The Tl percentage. This is primarily used for reporting purposes. It is m_fltTl*100. It ranges from 0-100 %
				float m_fltTLPerc;

				///The width of the tension length curve.
				float m_fltTLwidth;

				///precalculated value used in the determination of the length-tension curve.
				float m_fltTLc;

				///The active force that is developed from the current stimulus level before the tension-length relation is considered.
				float m_fltAct;

				///The total active force that is developed from both the current stimulus level and the tension-length relationship.
				float m_fltA;

				///Used to store the previous value of A when calculating the inverse muscle dynamics.
				float m_fltPrevA;

				///Internal tension. 
				float m_fltInternalTension;

				///The velocity of shortenting of the muscle.
				float m_fltVmuscle;

				///Constant that relates length of muscle segment to discharge rate of type Ib fibers.
				float m_fltIbDischargeConstant;

				///Ib fiber discharge rate.
				float m_fltIbRate;

				///An array that is used to store the instanaeous muscle velocities so we can get the 
				///averaged velocity over some number of steps.
				float *m_aryMuscleVelocities;

				///The number of instantanious velocity readings to average to calculate the average velocity
				int m_iMuscleVelAvgCount;

				///The muscle velocities array is a simple ring. We go around the ring and put in the current
				///int. velocity. Then to get the avg velocity at that step we average all the values in the array.
				int m_iVelAvgIndex;

				//Averaged muscle velocity.
				float m_fltAvgMuscleVel;

				float Ftl(float fltLce);
				float Fact(float fltStim);
				//float Tspring(float fltKq, float fltKl, float fltB, float fltLimit, float fltE);

				virtual void CalculateTension(Simulator *lpSim);

			public:
				LinearHillMuscle();
				virtual ~LinearHillMuscle();

				float Kse() {return m_fltKse;};
				float Kpe() {return m_fltKpe;};
				float B() {return m_fltB;};
				float RestingLength() {return m_fltMuscleRestingLength;};
				float SeRestLength() {return m_fltSeRestLength;};
				float SeLength() {return m_fltSeLength;};
				float PeLength() {return m_fltPeLength;};
				float PeLengthPercentage() {return m_fltPeLengthPercentage;};
				float MinPeLengthPercentage() {return m_fltMinPeLengthPercentage;};
				float MinPeLength() {return m_fltMinPeLength;};

				float A1() {return m_fltA1;};
				float A2() {return m_fltA2;};
				float A3() {return m_fltA3;};
				float A4() {return m_fltA4;};

				BOOL UseStimLimits() {return m_bUseStimLimits;};

				float StimLowerLimit() {return m_fltStimLowerLimit;};
				float StimUpperLimit() {return m_fltStimUpperLimit;};
				float StimLowerOutput() {return m_fltStimLowerOutput;};
				float StimUpperOutput() {return m_fltStimUpperOutput;};

				float Displacement() {return m_fltDisplacement;};
				float DisplacementRatio() {return m_fltDisplacementRatio;};

				float TL() {return m_fltTL;};
				float TLwidth() {return m_fltTLwidth;};
				float TLc() {return m_fltTLc;};

				float Act() {return m_fltAct;};
				float A() {return m_fltA;};
				float InternalTension() {return m_fltInternalTension;};

				float Vmuscle() {return m_fltVmuscle;};

				virtual void Enabled(BOOL bVal);

				virtual void CalculateInverseDynamics(Simulator *lpSim, float fltLength, float fltVelocity, float fltT, float &fltVm, float &fltA);

				virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);
				virtual float *GetDataPointer(string strDataType);

				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_LINEAR_HILL_MUSCLE_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_)
