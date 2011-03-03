// VsBallSocket.h: interface for the VsBallSocket class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSBALLSOCKETJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_VSBALLSOCKETJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsBallSocket : public VsJoint, public AnimatSim::Environment::Joints::BallSocket     
			{
			protected:
				Vx::VxBallAndSocket *m_vxSocket;

				virtual void SetVelocityToDesired();

			public:
				VsBallSocket();
				virtual ~VsBallSocket();

				//virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 

				virtual void EnableMotor(BOOL bVal);
				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSBALLSOCKETJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
