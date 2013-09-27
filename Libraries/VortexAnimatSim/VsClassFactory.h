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

		virtual RigidBody *CreateRigidBody(std::string strType, bool bThrowError = true);
		virtual Joint *CreateJoint(std::string strType, bool bThrowError = true);
		virtual Structure *CreateStructure(std::string strType, bool bThrowError = true);
		virtual Simulator *CreateSimulator(std::string strType = "", bool bThrowError = true);
		virtual KeyFrame *CreateKeyFrame(std::string strType = "", bool bThrowError = true);
		virtual DataChart *CreateDataChart(std::string strType, bool bThrowError = true);
		virtual DataColumn *CreateDataColumn(std::string strType, bool bThrowError = true);
		virtual Adapter *CreateAdapter(std::string strType, bool bThrowError = true);
		virtual Gain *CreateGain(std::string strType, bool bThrowError = true);
		virtual ExternalStimulus *CreateExternalStimulus(std::string strType, bool bThrowError = true);
		virtual HudItem *CreateHudItem(std::string strType, bool bThrowError = true);
		virtual Hud *CreateHud(std::string strType, bool bThrowError = true);
		virtual MaterialType *CreateMaterialItem(std::string strType, bool bThrowError = true);
		virtual SimulationWindow *CreateWindowItem(std::string strType, bool bThrowError = true);
		virtual Light *CreateLight(std::string strType, bool bThrowError = true);
		virtual NeuralModule *CreateNeuralModule(std::string strType, bool bThrowError = true);
		virtual ConstraintRelaxation *CreateConstraintRelaxation(std::string strType, bool bThrowError = true);
		virtual ConstraintFriction *CreateConstraintFriction(std::string strType, bool bThrowError = true);

		virtual CStdSerialize *CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError = true);
	};

}			//VortexAnimatSim

#endif // !defined(AFX_VSCLASSFACTORY_H__DCB5874D_000C_499E_833C_89D8D1629818__INCLUDED_)
