// Mouth.h: interface for the Mouth class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALMOUTH_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_ALMOUTH_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

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
				A Mouth type of joint.
			   
				\remarks
				This type of joint is constrained so that it can only
				rotate about one axis. You can define which axis it rotates
				around in the configuration file using the normalized 
				RotationAxis vector element. You can also specify the
				rotational constraints for this joint. This prevents it
				from rotating further than the constrained value.

				Also, this joint is motorized. So you can specify a desired
				velocity of motion at a given time step using the CNlInjectionMgr
				and the physics engine will automatically apply the forces
				necessary to move the joint at the desired velocity.

				\sa
				Joint, Mouth, CAlStaticJoint
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Mouth : public Sensor   
			{
			protected:
				Stomach *m_lpStomach;
				float m_fltEatingRate;
				float m_fltBiteSize;
				float m_fltMinFoodRadius;

			public:
				Mouth();
				virtual ~Mouth();

				//Node Overrides
				virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
				virtual float *GetDataPointer(string strDataType);
				virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALMOUTH_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
