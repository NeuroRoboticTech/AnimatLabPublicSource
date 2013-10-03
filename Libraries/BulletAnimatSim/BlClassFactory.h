// BlClassFactory.h: interface for the BlClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#pragma once


namespace BulletAnimatSim
{

	class BULLET_PORT BlClassFactory : public IStdClassFactory  
	{
	public:
		BlClassFactory();
		virtual ~BlClassFactory();

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

	void BULLET_PORT RunBootstrap(int argc, const char **argv);

}			//BulletAnimatSim
