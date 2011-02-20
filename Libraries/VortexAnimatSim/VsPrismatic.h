// VsPrismatic.h: interface for the VsPrismatic class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSPRISMATICJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_VSPRISMATICJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsPrismatic : public VsJoint, public Prismatic     
			{
			protected:
				Vx::VxPrismatic *m_vxPrismatic;
				float m_fltDistanceUnits;

				//virtual void UpdateData(Simulator *lpSim);
				virtual void SetVelocityToDesired();
				void CalculateServoVelocity();

			public:
				VsPrismatic();
				virtual ~VsPrismatic();

				virtual void Enabled(BOOL bValue) 
				{
					EnableMotor(bValue);
					m_bEnabled = bValue;
				};
				//virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 

				virtual void EnableMotor(BOOL bVal);
				virtual void CreateJoint(Simulator *lpSim, Structure *lpStructure);
				virtual float *GetDataPointer(string strDataType);
				//virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
				virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSPRISMATICJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
