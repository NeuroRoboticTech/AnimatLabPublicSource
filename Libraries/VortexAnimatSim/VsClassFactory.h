// VsClassFactory.h: interface for the VsClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSCLASSFACTORY_H__DCB5874D_000C_499E_833C_89D8D1629818__INCLUDED_)
#define AFX_VSCLASSFACTORY_H__DCB5874D_000C_499E_833C_89D8D1629818__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{

	class VORTEX_PORT VsClassFactory : public IStdClassFactory  
	{
	public:
		VsClassFactory();
		virtual ~VsClassFactory();

		virtual RigidBody *CreateRigidBody(string strType, BOOL bThrowError = TRUE);
		virtual Joint *CreateJoint(string strType, BOOL bThrowError = TRUE);
		virtual Structure *CreateStructure(string strType, BOOL bThrowError = TRUE);
		virtual Simulator *CreateSimulator(string strType = "", BOOL bThrowError = TRUE);
		virtual KeyFrame *CreateKeyFrame(string strType = "", BOOL bThrowError = TRUE);
		virtual DataChart *CreateDataChart(string strType, BOOL bThrowError = TRUE);
		virtual DataColumn *CreateDataColumn(string strType, BOOL bThrowError = TRUE);
		virtual Adapter *CreateAdapter(string strType, BOOL bThrowError = TRUE);
		virtual Gain *CreateGain(string strType, BOOL bThrowError = TRUE);
		virtual ExternalStimulus *CreateExternalStimulus(string strType, BOOL bThrowError = TRUE);
		virtual HudItem *CreateHudItem(string strType, BOOL bThrowError = TRUE);
		virtual Hud *CreateHud(string strType, BOOL bThrowError = TRUE);
		virtual MaterialPair *CreateMaterialItem(string strType, BOOL bThrowError = TRUE);
		virtual SimulationWindow *CreateWindowItem(string strType, BOOL bThrowError = TRUE);
		virtual Light *CreateLight(string strType, BOOL bThrowError = TRUE);

		virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, BOOL bThrowError = TRUE);
	};

}			//VortexAnimatSim

#endif // !defined(AFX_VSCLASSFACTORY_H__DCB5874D_000C_499E_833C_89D8D1629818__INCLUDED_)
