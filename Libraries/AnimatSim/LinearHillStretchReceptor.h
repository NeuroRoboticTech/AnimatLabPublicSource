// LinearHillStretchReceptor.h: interface for the LinearHillStretchReceptor class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LINEAR_HILL_STRETCH_RECEPTOR_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_)
#define AFX_LINEAR_HILL_STRETCH_RECEPTOR_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_

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

			class ANIMAT_PORT LinearHillStretchReceptor : public LinearHillMuscle  
			{
			protected:
				///Determines whether the receptor applies tension or not.
				BOOL m_bApplyTension;

				///Constant that relates length of muscle segment to discharge rate of type Ia fibers.
				float m_fltIaDischargeConstant;

				///Constant that relates length of muscle segment to discharge rate of type II fibers.
				float m_fltIIDischargeConstant;

				///Ia fiber discharge rate.
				float m_fltIaRate;

				///II fiber discharge rate.
				float m_fltIIRate;

				virtual void CalculateTension();

			public:
				LinearHillStretchReceptor();
				virtual ~LinearHillStretchReceptor();

				virtual BOOL ApplyTension();
				virtual void ApplyTension(BOOL bVal);

				virtual float IaDischargeConstant();
				virtual void IaDischargeConstant(float fltVal);

				virtual float IIDischargeConstant();
				virtual void IIDischargeConstant(float fltVal);

				virtual float IaRate();
				virtual float IIRate();

				virtual float *GetDataPointer(string strDataType);
				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_LINEAR_HILL_STRETCH_RECEPTOR_H__C0EE52F2_D31B_48CD_B5F7_B18EEE81BE72__INCLUDED_)
