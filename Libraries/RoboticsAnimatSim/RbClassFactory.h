// RbClassFactory.h: interface for the RbClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#pragma once


namespace RoboticsAnimatSim
{

	class ROBOTICS_PORT RbClassFactory : public IStdClassFactory  
	{
	public:
		RbClassFactory();
		virtual ~RbClassFactory();

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
		virtual RobotInterface *CreateRobotInterface(std::string strType, bool bThrowError = true);
		virtual RobotPartInterface *CreateRobotPartInterface(std::string strType, bool bThrowError = true);

		virtual CStdSerialize *CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError = true);
	};

	void ROBOTICS_PORT RunBootstrap(int argc, const char **argv);

}			//RoboticsAnimatSim
