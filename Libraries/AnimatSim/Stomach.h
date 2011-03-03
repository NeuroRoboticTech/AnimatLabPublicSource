// Stomach.h: interface for the Stomach class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSTOMACH_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_ALSTOMACH_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

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
				A Stomach type of joint.
			   
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
				Joint, Stomach, CAlStaticJoint
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Stomach : public RigidBody   
			{
			protected:
				float m_fltMaxEnergyLevel;
				float m_fltEnergyLevel;
				float m_fltConsumptionRate;
				float m_fltAdapterConsumptionRate;
				float m_fltBaseConsumptionRate;
				float m_fltConsumptionForStep;
				BOOL m_bKillOrganism;
				BOOL m_bKilled;

			public:
				Stomach();
				virtual ~Stomach();

				float EnergyLevel() {return m_fltEnergyLevel;};
				void EnergyLevel(float fltVal);

				float ConsumptionRate() {return m_fltConsumptionRate;};
				void ConsumptionRate(float fltVal) {m_fltConsumptionRate = fltVal;};

				float BaseConsumptionRate() {return m_fltBaseConsumptionRate;};
				void BaseConsumptionRate(float fltVal) {m_fltBaseConsumptionRate = fltVal;};

				float MaxEnergyLevel() {return m_fltMaxEnergyLevel;};
				void MaxEnergyLevel(float fltVal) {m_fltMaxEnergyLevel = fltVal;};

				BOOL KillOrganism() {return m_bKillOrganism;};
				void KillOrganism(BOOL bVal) {m_bKillOrganism = bVal;};

				virtual void CreateParts();

				//Node Overrides
				virtual void AddExternalNodeInput(float fltInput);
				virtual float *GetDataPointer(string strDataType);
				virtual void StepSimulation();
				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALSTOMACH_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
