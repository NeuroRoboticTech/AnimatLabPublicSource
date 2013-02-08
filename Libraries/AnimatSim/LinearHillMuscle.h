/**
\file	LinearHillMuscle.h

\brief	Declares the linear hill muscle class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{


			/**
			\brief	A muscle that is connected between two attachment points.
			
			\details
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

			\author	dcofer
			\date	3/10/2011
			**/
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

				///This is the amount of displacement of this muscle from its resting length.
				float m_fltDisplacement;

				///This is the ratio of displacement of this muscle from its resting length.
				float m_fltDisplacementRatio;

				///The proportion of active force developed that can actually be used at this muscle length. ranges from 0-1
				float m_fltTL;

				///The Tl percentage. This is primarily used for reporting purposes. It is m_fltTl*100. It ranges from 0-100 %
				float m_fltTLPerc;

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

				virtual void CalculateTension();

			public:
				LinearHillMuscle();
				virtual ~LinearHillMuscle();

				virtual float Kse();
				virtual void Kse(float fltVal);

				virtual float Kpe();
				virtual void Kpe(float fltVal);

				virtual float B();
				virtual void B(float fltVal);

				virtual float RestingLength();
				virtual void RestingLength(float fltVal);

				virtual float IbDischargeConstant();
				virtual void IbDischargeConstant(float fltVal);

				virtual float SeLength();
				virtual float PeLength();
				virtual float Displacement();
				virtual float DisplacementRatio();
				virtual float TL();
				virtual float Act();
				virtual float A();
				virtual float InternalTension();
				virtual float Vmuscle();

				virtual void Enabled(BOOL bVal);

				virtual void CalculateInverseDynamics(float fltLength, float fltVelocity, float fltT, float &fltVm, float &fltA);
								
				virtual void ResetSimulation();
				virtual void AfterResetSimulation();

				virtual void CreateJoints();
				virtual float *GetDataPointer(const string &strDataType);
				virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
