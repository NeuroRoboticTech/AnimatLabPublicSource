// RbClassFactory.cpp: implementation of the RbClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbClassFactory.h"

#include "RbConstraintRelaxation.h"
#include "RbConstraintFriction.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbBox.h"
#include "RbCylinder.h"
#include "RbCone.h" 
#include "RbSphere.h"
#include "RbTorus.h"
#include "RbEllipsoid.h"
#include "RbMesh.h"

#include "RbHinge.h"
#include "RbPrismatic.h"
#include "RbBallSocket.h"
#include "RbRPRO.h"
#include "RbUniversal.h"
#include "RbFreeJoint.h"

#include "RbAttachment.h"
#include "RbMouth.h"
#include "RbOdorSensor.h"

#include "RbLine.h"
#include "RbLinearHillMuscle.h"
#include "RbLinearHillStretchReceptor.h"
#include "RbSpring.h"

#include "RbOrganism.h"
#include "RbStructure.h"

#include "RbSimulator.h"
#include "RbMaterialType.h"

#include "RbLANWirelessInterface.h"
#include "RbDynamixelUSB.h"
#include "RbDynamixelUSBServo.h"

#include "RbFirmataController.h"
#include "RbFirmataPart.h"
#include "RbFirmataAnalogInput.h"
#include "RbFirmataAnalogOutput.h"
#include "RbFirmataDigitalInput.h"
#include "RbFirmataDigitalOutput.h"
#include "RbFirmataHingeServo.h"
#include "RbFirmataPrismaticServo.h"
#include "RbFirmataPWMOutput.h"

#include "RbFirmataDynamixelServo.h"
#include "RbXBeeCommander.h"

#ifdef _WINDOWS
	extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
	extern "C" IStdClassFactory* GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new RbClassFactory;
	return lpFactory;
}

#ifdef _WINDOWS
	extern "C" __declspec(dllexport) int __cdecl BootstrapRunLibrary(int argc, const char **argv) 
#else
	extern "C" int BootstrapRunLibrary(int argc, const char **argv) 
#endif
{
	Simulator *lpSim = NULL;

try
{
	Simulator *lpSim = Simulator::CreateSimulator(argc, argv);

	lpSim->Load();
	lpSim->Initialize(argc, argv);
    lpSim->VisualSelectionMode(SIMULATION_SELECTION_MODE);

    lpSim->StartSimulation();
    lpSim->Simulate();

	if(lpSim) delete lpSim;

	return 0;
}
catch(CStdErrorInfo oError)
{
	if(lpSim) delete lpSim;
	printf("Error occurred: %s\n", oError.m_strError.c_str()) ;
	return (int) oError.m_lError;
}
catch(...)
{
	if(lpSim) delete lpSim;
	printf("An Unknown Error occurred.\n") ;
	return -1;
}
}



namespace RoboticsAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbClassFactory::RbClassFactory()
{

}

RbClassFactory::~RbClassFactory()
{

}

// ************* Body Type Conversion functions ******************************

RigidBody *RbClassFactory::CreateRigidBody(std::string strType, bool bThrowError)
{
	RigidBody *lpPart=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BOX")
		lpPart = new RbBox;
	else if(strType == "BOXCONTACTSENSOR")
	{
		lpPart = new RbBox;
		lpPart->IsContactSensor(true);
	}
	else if(strType == "CYLINDER")
		lpPart = new RbCylinder;
	else if(strType == "CYLINDERCONTACTSENSOR")
	{
		lpPart = new RbCylinder;
		lpPart->IsContactSensor(true);
	}
	else if(strType == "CONE")
		lpPart = new RbCone;
	else if(strType == "SPHERE")
		lpPart = new RbSphere;
	else if(strType == "ATTACHMENT")
		lpPart = new RbAttachment;
	else if(strType == "LINEARHILLMUSCLE")
		lpPart = new RbLinearHillMuscle;
	else if(strType == "LINEARHILLSTRETCHRECEPTOR")
		lpPart = new RbLinearHillStretchReceptor;
	else if(strType == "SPRING")
		lpPart = new RbSpring;
	else if(strType == "TORUS")
		lpPart = new RbTorus;
	else if(strType == "ELLIPSOID")
		lpPart = new RbEllipsoid;
	else if(strType == "MOUTH")
		lpPart = new RbMouth;
	else if(strType == "ODORSENSOR")
		lpPart = new RbOdorSensor;
	else if(strType == "MESH")
		lpPart = new RbMesh;
	else if(strType == "STOMACH")
		lpPart = new Stomach;
	else
	{
		lpPart = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidPartType, Al_Err_strInvalidPartType, "PartType", strType);
	}
	
	return lpPart;
}
catch(CStdErrorInfo oError)
{
	if(lpPart) delete lpPart;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpPart) delete lpPart;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Body Type Conversion functions ******************************


// ************* Body Joint Conversion functions ******************************

Joint *RbClassFactory::CreateJoint(std::string strType, bool bThrowError)
{
	Joint *lpJoint=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "HINGE")
		lpJoint = new RbHinge;
	else if(strType == "PRISMATIC")
		lpJoint = new RbPrismatic;
	else if(strType == "BALLSOCKET")
		lpJoint = new RbBallSocket;
	else if(strType == "RPRO")
		lpJoint = new RbRPRO;
	else if(strType == "STATIC")
		lpJoint = NULL;
	else if(strType == "UNIVERSAL")
		lpJoint = new RbUniversal;
	else if(strType == "FREEJOINT")
		lpJoint = new RbFreeJoint;
	else
	{
		lpJoint = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidJointType, Al_Err_strInvalidJointType, "JointType", strType);
	}

	return lpJoint;
}
catch(CStdErrorInfo oError)
{
	if(lpJoint) delete lpJoint;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpJoint) delete lpJoint;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Body Joint Conversion functions ******************************

// ************* Organism Type Conversion functions ******************************

Structure *RbClassFactory::CreateStructure(std::string strType, bool bThrowError)
{
	Structure *lpStructure=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BASIC")
		lpStructure = new RbOrganism;
	else if(strType == "ORGANISM")
		lpStructure = new RbOrganism;
	else if(strType == "STRUCTURE")
		lpStructure = new RbStructure;
	else
	{
		lpStructure = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidOrganismType, Al_Err_strInvalidOrganismType, "OrganismType", strType);
	}

	return lpStructure;
}
catch(CStdErrorInfo oError)
{
	if(lpStructure) delete lpStructure;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpStructure) delete lpStructure;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Organism Type Conversion functions ******************************


// ************* Simulator Type Conversion functions ******************************

Simulator *RbClassFactory::CreateSimulator(std::string strType, bool bThrowError)
{
	Simulator *lpSimulator=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "ROBOTICSSIMULATOR")
		lpSimulator = new RbSimulator;
	else if(strType == "")
		lpSimulator = new RbSimulator;
	else
	{
		lpSimulator = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidSimulatorType, Al_Err_strInvalidSimulatorType, "SimulatorType", strType);
	}

	return lpSimulator;
}
catch(CStdErrorInfo oError)
{
	if(lpSimulator) delete lpSimulator;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpSimulator) delete lpSimulator;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Organism Type Conversion functions ******************************



// ************* KeyFrame Type Conversion functions ******************************
KeyFrame *RbClassFactory::CreateKeyFrame(std::string strType, bool bThrowError)
{
	KeyFrame *lpFrame=NULL;

try
{
//	strType = Std_ToUpper(Std_Trim(strType));
//
//	if(strType == "VIDEO")
//		lpFrame = new VsVideoKeyFrame;
//	else if(strType == "SNAPSHOT")
//		lpFrame = new VsSnapshotKeyFrame;
//	else
//	{
//		lpFrame = NULL;
//		if(bThrowError)
//			THROW_PARAM_ERROR(Al_Err_lInvalidKeyFrameType, Al_Err_strInvalidKeyFrameType, "KeyFrameType", strType);
//	}

	return lpFrame;
}
catch(CStdErrorInfo oError)
{
	if(lpFrame) delete lpFrame;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpFrame) delete lpFrame;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* KeyFrame Type Conversion functions ******************************


// ************* DataChart Type Conversion functions ******************************

DataChart *RbClassFactory::CreateDataChart(std::string strType, bool bThrowError)
{
	DataChart *lpChart=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "TABFILE")
		lpChart = new FileChart;
	else if(strType == "FILECHART")
		lpChart = new FileChart;
	else if(strType == "MEMORYCHART")
		lpChart = new MemoryChart;
	else if(strType == "ARRAYCHART")
		lpChart = new ArrayChart;
	else
	{
		lpChart = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidDataChartType, Al_Err_strInvalidDataChartType, "DataChartType", strType);
	}

	return lpChart;
}
catch(CStdErrorInfo oError)
{
	if(lpChart) delete lpChart;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpChart) delete lpChart;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}
// ************* DataChart Type Conversion functions ******************************


// ************* DataColumn Type Conversion functions ******************************

DataColumn *RbClassFactory::CreateDataColumn(std::string strType, bool bThrowError)
{
	DataColumn *lpColumn=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "DATACOLUMN")
		lpColumn = new DataColumn;
	else
	{
		lpColumn = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidDataColumnType, Al_Err_strInvalidDataColumnType, "DataColumnType", strType);
	}

	return lpColumn;
}
catch(CStdErrorInfo oError)
{
	if(lpColumn) delete lpColumn;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpColumn) delete lpColumn;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* DataColumn Type Conversion functions ******************************


// ************* Adapter Type Conversion functions ******************************

Adapter *RbClassFactory::CreateAdapter(std::string strType, bool bThrowError)
{
	Adapter *lpAdapter=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NODETONODE")
		lpAdapter = new Adapter;
	else if(strType == "NODETOPHYSICAL")
		lpAdapter = new Adapter;
	else if(strType == "PHYSICALTONODE")
		lpAdapter = new Adapter;
	else if(strType == "CONTACT")
		lpAdapter = new ContactAdapter;
	else if(strType == "PROPERTYCONTROLADAPTER")
		lpAdapter = new PropertyControlAdapter;
	else
	{
		lpAdapter = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidAdapterType, Al_Err_strInvalidAdapterType, "AdapterType", strType);
	}

	return lpAdapter;
}
catch(CStdErrorInfo oError)
{
	if(lpAdapter) delete lpAdapter;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpAdapter) delete lpAdapter;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Adpater Type Conversion functions ******************************


// ************* Gain Type Conversion functions ******************************

Gain *RbClassFactory::CreateGain(std::string strType, bool bThrowError)
{
	Gain *lpGain=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BELL")
		lpGain = new BellGain;
	else if(strType == "EQUATION")
		lpGain = new EquationGain;
	else if(strType == "POLYNOMIAL")
		lpGain = new PolynomialGain;
	else if(strType == "SIGMOID")
		lpGain = new SigmoidGain;
	else
	{
		lpGain = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidGainType, Al_Err_strInvalidGainType, "GainType", strType);
	}

	return lpGain;
}
catch(CStdErrorInfo oError)
{
	if(lpGain) delete lpGain;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpGain) delete lpGain;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Adpater Type Conversion functions ******************************


// ************* External Stimulus Type Conversion functions ******************************

ExternalStimulus *RbClassFactory::CreateExternalStimulus(std::string strType, bool bThrowError)
{
	ExternalStimulus *lpStimulus=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "MOTORVELOCITY")
		lpStimulus = new AnimatSim::ExternalStimuli::MotorVelocityStimulus;
	else if(strType == "FORCEINPUT")
		lpStimulus = new AnimatSim::ExternalStimuli::ForceStimulus;
	else if(strType == "NODEINPUT")
		lpStimulus = new ExternalInputStimulus;
	else if(strType == "RIGIDBODYINPUT")
		lpStimulus = new ExternalInputStimulus;
	else if(strType == "JOINTINPUT")
		lpStimulus = new ExternalInputStimulus;
	else if(strType == "ENABLERINPUT")
		lpStimulus = new EnablerStimulus;
	else if(strType == "INVERSEMUSCLECURRENT")
		lpStimulus = new InverseMuscleCurrent;
	else if(strType == "CURRENT")
		lpStimulus = new AnimatSim::ExternalStimuli::CurrentStimulus;
	else if(strType == "VOLTAGECLAMP")
		lpStimulus = new AnimatSim::ExternalStimuli::VoltageClamp;
	else if(strType == "PROPERTYCONTROLSTIMULUS")
		lpStimulus = new AnimatSim::ExternalStimuli::PropertyControlStimulus;
	else
	{
		lpStimulus = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidExternalStimulusType, Al_Err_strInvalidExternalStimulusType, "ExternalStimulusType", strType);
	}

	return lpStimulus;
}
catch(CStdErrorInfo oError)
{
	if(lpStimulus) delete lpStimulus;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpStimulus) delete lpStimulus;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* External Stimulus Type Conversion functions ******************************


// ************* Hud Item Type Conversion functions ******************************

HudItem *RbClassFactory::CreateHudItem(std::string strType, bool bThrowError)
{
	HudItem *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	lpItem = NULL;
	if(bThrowError)
		THROW_PARAM_ERROR(Rb_Err_lInvalidHudItemType, Rb_Err_strInvalidHudItemType, "HudItem", strType);

	return lpItem;
}
catch(CStdErrorInfo oError)
{
	if(lpItem) delete lpItem;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpItem) delete lpItem;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Hud Item Type Conversion functions ******************************

// ************* Hud Type Conversion functions ******************************

Hud *RbClassFactory::CreateHud(std::string strType, bool bThrowError)
{
	Hud *lpHud=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	lpHud = NULL;
	if(bThrowError)
		THROW_PARAM_ERROR(Rb_Err_lInvalidHudItemType, Rb_Err_strInvalidHudItemType, "Hud", strType);

	return lpHud;
}
catch(CStdErrorInfo oError)
{
	if(lpHud) delete lpHud;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpHud) delete lpHud;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Hud Item Type Conversion functions ******************************
// 
// ************* Material Type Conversion functions ******************************

MaterialType *RbClassFactory::CreateMaterialItem(std::string strType, bool bThrowError)
{
	MaterialType *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BASIC" || strType == "DEFAULT" || strType == "BULLET")
		lpItem = new RbMaterialType;
	else
	{
		lpItem = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Rb_Err_lInvalidMaterialItemType, Rb_Err_strInvalidMaterialItemType, "Material Pair", strType);
	}

	return lpItem;
}
catch(CStdErrorInfo oError)
{
	if(lpItem) delete lpItem;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpItem) delete lpItem;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Material Type Conversion functions ******************************

// ************* Material Type Conversion functions ******************************

SimulationWindow *RbClassFactory::CreateWindowItem(std::string strType, bool bThrowError)
{
	SimulationWindow *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	lpItem = NULL;
	if(bThrowError)
		THROW_PARAM_ERROR(Rb_Err_lInvalidSimWindowType, Rb_Err_strInvalidSimWindowType, "Simulation Window", strType);

	return lpItem;
}
catch(CStdErrorInfo oError)
{
	if(lpItem) delete lpItem;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpItem) delete lpItem;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Material Type Conversion functions ******************************

// ************* Light Conversion functions ******************************

Light *RbClassFactory::CreateLight(std::string strType, bool bThrowError)
{
	Light *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	lpItem = NULL;
	if(bThrowError)
		THROW_PARAM_ERROR(Rb_Err_lInvalidLightType, Rb_Err_strInvalidLightType, "Light Type", strType);

	return lpItem;
}
catch(CStdErrorInfo oError)
{
	if(lpItem) delete lpItem;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpItem) delete lpItem;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Light Conversion functions ******************************

// ************* External Neural Module Conversion functions ******************************

NeuralModule *RbClassFactory::CreateNeuralModule(std::string strType, bool bThrowError)
{
	NeuralModule *lpModule=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "PHYSICSNEURALMODULE")
	{
		lpModule = new AnimatSim::Behavior::PhysicsNeuralModule;
		lpModule->ClassFactory(new RbClassFactory());
	}
	else
	{
		lpModule = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidNeuralModuleType, Al_Err_strInvalidNeuralModuleType, "NeuralModule", strType);
	}

	return lpModule;
}
catch(CStdErrorInfo oError)
{
	if(lpModule) delete lpModule;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpModule) delete lpModule;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Neural Module Type Conversion functions ******************************

// ************* Constraint Relaxation Conversion functions ******************************

ConstraintRelaxation *RbClassFactory::CreateConstraintRelaxation(std::string strType, bool bThrowError)
{
	ConstraintRelaxation *lpRelax=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CONSTRAINTRELAXATION" || strType == "DEFAULT")
	{
		lpRelax = new RbConstraintRelaxation;
	}
	else
	{
		lpRelax = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidRelaxationType, Al_Err_strInvalidRelaxationType, "Relaxation", strType);
	}

	return lpRelax;
}
catch(CStdErrorInfo oError)
{
	if(lpRelax) delete lpRelax;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpRelax) delete lpRelax;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Constraint Relaxation Type Conversion functions ******************************

// ************* Constraint Friction Conversion functions ******************************

ConstraintFriction *RbClassFactory::CreateConstraintFriction(std::string strType, bool bThrowError)
{
	ConstraintFriction *lpFriction=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CONSTRAINTRELAXATION" || strType == "DEFAULT")
	{
		lpFriction = new RbConstraintFriction;
	}
	else
	{
		lpFriction = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidFrictionType, Al_Err_strInvalidFrictionType, "Friction", strType);
	}

	return lpFriction;
}
catch(CStdErrorInfo oError)
{
	if(lpFriction) delete lpFriction;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpFriction) delete lpFriction;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Constraint Friction Type Conversion functions ******************************


// ************* Robot Interface Conversion functions ******************************

RobotInterface *RbClassFactory::CreateRobotInterface(std::string strType, bool bThrowError)
{
	RobotInterface *lpInterface=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "LANWIRELESSINTERFACE" || strType == "DEFAULT")
	{
		lpInterface = new RbLANWirelessInterface;
	}
	else
	{
		lpInterface = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidRobotInterfaceType, Al_Err_strInvalidRobotInterfaceType, "RobotInterface", strType);
	}

	return lpInterface;
}
catch(CStdErrorInfo oError)
{
	if(lpInterface) delete lpInterface;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpInterface) delete lpInterface;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Robot Interface Type Conversion functions ******************************



// ************* Robot IO Control Conversion functions ******************************

RobotIOControl *RbClassFactory::CreateRobotIOControl(std::string strType, bool bThrowError)
{
	RobotIOControl *lpControl=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "DYNAMIXELUSB")
	{
		lpControl = new RbDynamixelUSB;
	}
	else if(strType == "FIRMATACONTROLLER")
	{
		lpControl = new RbFirmataController;
	}
	else if(strType == "XBEECOMMANDER")
	{
		lpControl = new RbXBeeCommander;
	}
	else
	{
		lpControl = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidRobotIOControlType, Al_Err_strInvalidRobotIOControlType, "RobotartIOControl", strType);
	}

	return lpControl;
}
catch(CStdErrorInfo oError)
{
	if(lpControl) delete lpControl;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpControl) delete lpControl;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Robot IO Control Conversion functions ******************************


// ************* Robot Part Interface Conversion functions ******************************

RobotPartInterface *RbClassFactory::CreateRobotPartInterface(std::string strType, bool bThrowError)
{
	RobotPartInterface *lpInterface=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "DYNAMIXELUSBHINGE")
	{
		lpInterface = new RbDynamixelUSBServo;
	}
	else if(strType == "DYNAMIXELUSBPRISMATIC")
	{
		lpInterface = new RbDynamixelUSBServo;
	}
	else if(strType == "FIRMATAANALOGINPUT")
	{
		lpInterface = new RbFirmataAnalogInput;
	}
	else if(strType == "FIRMATAANALOGOUTPUT")
	{
		lpInterface = new RbFirmataAnalogInput;
	}
	else if(strType == "FIRMATADIGITALINPUT")
	{
		lpInterface = new RbFirmataDigitalInput;
	}
	else if(strType == "FIRMATADIGITALOUTPUT")
	{
		lpInterface = new RbFirmataDigitalOutput;
	}
	else if(strType == "FIRMATAHINGESERVO")
	{
		lpInterface = new RbFirmataHingeServo;
	}
	else if(strType == "FIRMATAPRISMATICSERVO")
	{
		lpInterface = new RbFirmataPrismaticServo;
	}
	else if(strType == "FIRMATAPWMOUTPUT")
	{
		lpInterface = new RbFirmataPWMOutput;
	}
	else if(strType == "FIRMATADYNAMIXELHINGESERVO")
	{
		lpInterface = new RbFirmataDynamixelServo;
	}
	else if(strType == "FIRMATADYNAMIXELPRISMATICSERVO")
	{
		lpInterface = new RbFirmataDynamixelServo;
	}
	else
	{
		lpInterface = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidRobotPartInterfaceType, Al_Err_strInvalidRobotPartInterfaceType, "RobotartInterface", strType);
	}

	return lpInterface;
}
catch(CStdErrorInfo oError)
{
	if(lpInterface) delete lpInterface;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpInterface) delete lpInterface;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Robot Part Interface Type Conversion functions ******************************

// ************* RemoteControlLinkage Conversion functions ******************************

RemoteControlLinkage *RbClassFactory::CreateRemoteControlLinkage(std::string strType, bool bThrowError)
{
	RemoteControlLinkage *lpLink=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "PASSTHROUGHLINKAGE")
	{
		lpLink = new PassThroughLinkage;
	}
	else if(strType == "PULSEDLINKAGE")
	{
		lpLink = new PulsedLinkage;
	}
	else
	{
		lpLink = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidFrictionType, Al_Err_strInvalidFrictionType, "Friction", strType);
	}

	return lpLink;
}
catch(CStdErrorInfo oError)
{
	if(lpLink) delete lpLink;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpLink) delete lpLink;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* RemoteControlLinkage Type Conversion functions ******************************




// ************* IStdClassFactory functions ******************************

CStdSerialize *RbClassFactory::CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));

	if(strClassType == "RIGIDBODY")
		lpObject = CreateRigidBody(strObjectType, bThrowError);
	else if(strClassType == "JOINT")
		lpObject = CreateJoint(strObjectType, bThrowError);
	else if(strClassType == "ORGANISM")
		lpObject = CreateStructure(strObjectType, bThrowError);
	else if(strClassType == "STRUCTURE")
		lpObject = CreateStructure(strObjectType, bThrowError);
	else if(strClassType == "SIMULATOR")
		lpObject = CreateSimulator(strObjectType, bThrowError);
	else if(strClassType == "KEYFRAME")
		lpObject = CreateKeyFrame(strObjectType, bThrowError);
	else if(strClassType == "DATACHART")
		lpObject = CreateDataChart(strObjectType, bThrowError);
	else if(strClassType == "DATACOLUMN")
		lpObject = CreateDataColumn(strObjectType, bThrowError);
	else if(strClassType == "EXTERNALSTIMULUS")
		lpObject = CreateExternalStimulus(strObjectType, bThrowError);
	else if(strClassType == "ADAPTER")
		lpObject = CreateAdapter(strObjectType, bThrowError);
	else if(strClassType == "GAIN")
		lpObject = CreateGain(strObjectType, bThrowError);
	else if(strClassType == "HUDITEM")
		lpObject = CreateHudItem(strObjectType, bThrowError);
	else if(strClassType == "HUD")
		lpObject = CreateHud(strObjectType, bThrowError);
	else if(strClassType == "MATERIAL")
		lpObject = CreateMaterialItem(strObjectType, bThrowError);
	else if(strClassType == "SIMULATIONWINDOW")
		lpObject = CreateWindowItem(strObjectType, bThrowError);
	else if(strClassType == "LIGHT")
		lpObject = CreateLight(strObjectType, bThrowError);
	else if(strClassType == "NEURALMODULE")
		lpObject = CreateNeuralModule(strObjectType, bThrowError);
	else if(strClassType == "CONSTRAINTRELAXATION")
		lpObject = CreateConstraintRelaxation(strObjectType, bThrowError);
	else if(strClassType == "CONSTRAINTFRICTION")
		lpObject = CreateConstraintFriction(strObjectType, bThrowError);
	else if(strClassType == "ROBOTINTERFACE")
		lpObject = CreateRobotInterface(strObjectType, bThrowError);
	else if(strClassType == "ROBOTIOCONTROL")
		lpObject = CreateRobotIOControl(strObjectType, bThrowError);
	else if(strClassType == "ROBOTPARTINTERFACE")
		lpObject = CreateRobotPartInterface(strObjectType, bThrowError);
	else if(strClassType == "REMOTECONTROLLINKAGE")
		lpObject = CreateRemoteControlLinkage(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}
// ************* IStdClassFactory functions ******************************


void ROBOTICS_PORT RunBootstrap(int argc, const char **argv)
{
	BootstrapRunLibrary(argc, argv);
}


}			//RoboticsAnimatSim
