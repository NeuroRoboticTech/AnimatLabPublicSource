// BlClassFactory.cpp: implementation of the BlClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlClassFactory.h"

#include "BlConstraintRelaxation.h"
#include "BlConstraintFriction.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlPlane.h"
#include "BlBox.h"
#include "BlCylinder.h"
#include "BlCone.h" 
#include "BlSphere.h"
#include "BlTorus.h"
#include "BlEllipsoid.h"
#include "BlFluidPlane.h"
#include "BlMeshBase.h"
#include "BlMesh.h"
#include "BlTerrain.h"

#include "BlHinge.h"
#include "BlPrismatic.h"
#include "BlBallSocket.h"
#include "BlRPRO.h"
#include "BlUniversal.h"
#include "BlDistanceJoint.h"

#include "BlLine.h"
#include "BlLinearHillMuscle.h"
#include "BlLinearHillStretchReceptor.h"
#include "BlSpring.h"

#include "BlSimulator.h"

#include "BlMaterialType.h"


#ifdef _WINDOWS
	extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
	extern "C" IStdClassFactory* GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new BlClassFactory;
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



namespace BulletAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlClassFactory::BlClassFactory()
{

}

BlClassFactory::~BlClassFactory()
{

}

// ************* Body Type Conversion functions ******************************

RigidBody *BlClassFactory::CreateRigidBody(std::string strType, bool bThrowError)
{
	RigidBody *lpPart=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BOX")
		lpPart = new BlBox;
	else if(strType == "BOXCONTACTSENSOR")
	{
		lpPart = new BlBox;
		lpPart->IsContactSensor(true);
	}
	else if(strType == "CYLINDER")
		lpPart = new BlCylinder;
	else if(strType == "CYLINDERCONTACTSENSOR")
	{
		lpPart = new BlCylinder;
		lpPart->IsContactSensor(true);
	}
	else if(strType == "CONE")
		lpPart = new BlCone;
	else if(strType == "SPHERE")
		lpPart = new BlSphere;
	else if(strType == "PLANE")
		lpPart = new BlPlane;
	else if(strType == "ATTACHMENT")
		lpPart = new OsgAnimatSim::Environment::Bodies::OsgAttachment;
	else if(strType == "LINEARHILLMUSCLE")
		lpPart = new BlLinearHillMuscle;
	else if(strType == "LINEARHILLSTRETCHRECEPTOR")
		lpPart = new BlLinearHillStretchReceptor;
	else if(strType == "SPRING")
		lpPart = new BlSpring;
	else if(strType == "TORUS")
		lpPart = new BlTorus;
	else if(strType == "ELLIPSOID")
		lpPart = new BlEllipsoid;
	else if(strType == "MOUTH")
		lpPart = new OsgAnimatSim::Environment::Bodies::OsgMouth;
	else if(strType == "ODORSENSOR")
		lpPart = new OsgAnimatSim::Environment::Bodies::OsgOdorSensor;
	else if(strType == "FLUIDPLANE")
		lpPart = new BlFluidPlane;
	else if(strType == "TERRAIN")
		lpPart = new BlTerrain;
	else if(strType == "MESH")
		lpPart = new BlMesh;
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

Joint *BlClassFactory::CreateJoint(std::string strType, bool bThrowError)
{
	Joint *lpJoint=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "HINGE")
		lpJoint = new BlHinge;
	else if(strType == "PRISMATIC")
		lpJoint = new BlPrismatic;
	else if(strType == "BALLSOCKET")
		lpJoint = new BlBallSocket;
	else if(strType == "RPRO")
		lpJoint = new BlRPRO;
	else if(strType == "STATIC")
		lpJoint = NULL;
	else if(strType == "UNIVERSAL")
		lpJoint = new BlUniversal;
	else if(strType == "FREEJOINT")
		lpJoint = new OsgAnimatSim::Environment::Joints::OsgFreeJoint;
	else if(strType == "DISTANCE")
		lpJoint = new BlDistanceJoint;
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

Structure *BlClassFactory::CreateStructure(std::string strType, bool bThrowError)
{
	Structure *lpStructure=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BASIC")
		lpStructure = new OsgOrganism;
	else if(strType == "ORGANISM")
		lpStructure = new OsgOrganism;
	else if(strType == "STRUCTURE")
		lpStructure = new OsgStructure;
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

Simulator *BlClassFactory::CreateSimulator(std::string strType, bool bThrowError)
{
	Simulator *lpSimulator=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

    if(strType == "VORTEXSIMULATOR" || strType == "BULLETSIMULATOR")
		lpSimulator = new BlSimulator;
	else if(strType == "")
		lpSimulator = new BlSimulator;
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
KeyFrame *BlClassFactory::CreateKeyFrame(std::string strType, bool bThrowError)
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

DataChart *BlClassFactory::CreateDataChart(std::string strType, bool bThrowError)
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

DataColumn *BlClassFactory::CreateDataColumn(std::string strType, bool bThrowError)
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

Adapter *BlClassFactory::CreateAdapter(std::string strType, bool bThrowError)
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

Gain *BlClassFactory::CreateGain(std::string strType, bool bThrowError)
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

ExternalStimulus *BlClassFactory::CreateExternalStimulus(std::string strType, bool bThrowError)
{
	ExternalStimulus *lpStimulus=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "MOTORVELOCITY" || strType == "MOTORPOSITION")
		lpStimulus = new AnimatSim::ExternalStimuli::MotorStimulus;
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

HudItem *BlClassFactory::CreateHudItem(std::string strType, bool bThrowError)
{
	HudItem *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "HUDTEXT")
		lpItem = new OsgHudText;
	else
	{
		lpItem = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Bl_Err_lInvalidHudItemType, Bl_Err_strInvalidHudItemType, "HudItem", strType);
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

// ************* Hud Item Type Conversion functions ******************************

// ************* Hud Type Conversion functions ******************************

Hud *BlClassFactory::CreateHud(std::string strType, bool bThrowError)
{
	Hud *lpHud=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "HUD")
		lpHud = new OsgHud;
	else
	{
		lpHud = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Bl_Err_lInvalidHudItemType, Bl_Err_strInvalidHudItemType, "Hud", strType);
	}

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

MaterialType *BlClassFactory::CreateMaterialItem(std::string strType, bool bThrowError)
{
	MaterialType *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BASIC" || strType == "DEFAULT" || strType == "BULLET")
		lpItem = new BlMaterialType;
	else
	{
		lpItem = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Bl_Err_lInvalidMaterialItemType, Bl_Err_strInvalidMaterialItemType, "Material Pair", strType);
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

SimulationWindow *BlClassFactory::CreateWindowItem(std::string strType, bool bThrowError)
{
	SimulationWindow *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "BASIC" || strType == "DEFAULT")
		lpItem = new OsgSimulationWindow;
	else if(strType == "SCRIPTEDSIMWINDOW")
		lpItem = new OsgScriptedSimulationWindow;
	else
	{
		lpItem = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Bl_Err_lInvalidSimWindowType, Bl_Err_strInvalidSimWindowType, "Simulation Window", strType);
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

// ************* Light Conversion functions ******************************

Light *BlClassFactory::CreateLight(std::string strType, bool bThrowError)
{
	Light *lpItem=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "LIGHT")
		lpItem = new OsgLight;
	else
	{
		lpItem = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Bl_Err_lInvalidLightType, Bl_Err_strInvalidLightType, "Light Type", strType);
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

// ************* Light Conversion functions ******************************

// ************* External Neural Module Conversion functions ******************************

NeuralModule *BlClassFactory::CreateNeuralModule(std::string strType, bool bThrowError)
{
	NeuralModule *lpModule=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "PHYSICSNEURALMODULE")
	{
		lpModule = new AnimatSim::Behavior::PhysicsNeuralModule;
		lpModule->ClassFactory(new BlClassFactory());
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

ConstraintRelaxation *BlClassFactory::CreateConstraintRelaxation(std::string strType, bool bThrowError)
{
	ConstraintRelaxation *lpRelax=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CONSTRAINTRELAXATION" || strType == "DEFAULT")
	{
		lpRelax = new BlConstraintRelaxation;
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

ConstraintFriction *BlClassFactory::CreateConstraintFriction(std::string strType, bool bThrowError)
{
	ConstraintFriction *lpFriction=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CONSTRAINTRELAXATION" || strType == "DEFAULT")
	{
		lpFriction = new BlConstraintFriction;
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

// ************* RemoteControlLinkage Conversion functions ******************************

RemoteControlLinkage *BlClassFactory::CreateRemoteControlLinkage(std::string strType, bool bThrowError)
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

CStdSerialize *BlClassFactory::CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError)
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


void BULLET_PORT RunBootstrap(int argc, const char **argv)
{
	BootstrapRunLibrary(argc, argv);
}


}			//BulletAnimatSim
