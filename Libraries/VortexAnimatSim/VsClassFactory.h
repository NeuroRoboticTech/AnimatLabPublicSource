// VsClassFactory.h: interface for the VsClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#pragma once


namespace VortexAnimatSim
{

	class VORTEX_PORT VsClassFactory : public IStdClassFactory  
	{
	public:
		VsClassFactory();
		virtual ~VsClassFactory();

		virtual RigidBody *CreateRigidBody(string strType, bool bThrowError = true);
		virtual Joint *CreateJoint(string strType, bool bThrowError = true);
		virtual Structure *CreateStructure(string strType, bool bThrowError = true);
		virtual Simulator *CreateSimulator(string strType = "", bool bThrowError = true);
		virtual KeyFrame *CreateKeyFrame(string strType = "", bool bThrowError = true);
		virtual DataChart *CreateDataChart(string strType, bool bThrowError = true);
		virtual DataColumn *CreateDataColumn(string strType, bool bThrowError = true);
		virtual Adapter *CreateAdapter(string strType, bool bThrowError = true);
		virtual Gain *CreateGain(string strType, bool bThrowError = true);
		virtual ExternalStimulus *CreateExternalStimulus(string strType, bool bThrowError = true);
		virtual HudItem *CreateHudItem(string strType, bool bThrowError = true);
		virtual Hud *CreateHud(string strType, bool bThrowError = true);
		virtual MaterialType *CreateMaterialItem(string strType, bool bThrowError = true);
		virtual SimulationWindow *CreateWindowItem(string strType, bool bThrowError = true);
		virtual Light *CreateLight(string strType, bool bThrowError = true);
		virtual NeuralModule *CreateNeuralModule(string strType, bool bThrowError = true);
		virtual ConstraintRelaxation *CreateConstraintRelaxation(string strType, bool bThrowError = true);
		virtual ConstraintFriction *CreateConstraintFriction(string strType, bool bThrowError = true);

		virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, bool bThrowError = true);
	};

}			//VortexAnimatSim
