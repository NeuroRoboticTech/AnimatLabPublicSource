/* File : AnimatSimPy.i */
%module(directors="1") AnimatSimPy
%{
#include "StdAfx.h"
using namespace StdUtils;
%}

#define SWIG_SHARED_PTR_SUBNAMESPACE tr1
#define STD_UTILS_PORT 
#define ANIMAT_PORT 

%include "std_vector.i"
%include "std_string.i"
%include "std_shared_ptr.i"
%include "std_map.i"

%include "typemaps.i"

typedef float StdVector4[4];
typedef float StdVector3[3];

%template(vector_string) std::vector<std::string>;
%template(vector_int) std::vector<int>;
%template(vector_long) std::vector<long>;
%template(vector_float) std::vector<float>;
%template(vector_double) std::vector<double>;

%template(vector_ActivatedItemPtr) std::vector<ActivatedItem *>;
%template(vector_AdapterPtr) std::vector<Adapter *>;
%template(vector_AttachmentPtr) std::vector<Attachment *>;
%template(vector_BodyPartPtr) std::vector<BodyPart *>;
%template(vector_CollisionPairPtr) std::vector<CollisionPair *>;
%template(vector_CStdVariablePtr) std::vector<CStdVariable *>;
%template(vector_DataColumnPtr) std::vector<DataColumn *>;
%template(vector_HudItemPtr) std::vector<HudItem *>;
%template(vector_KeyFramePtr) std::vector<KeyFrame *>;
%template(vector_LightPtr) std::vector<Light *>;
%template(vector_MaterialTypePtr) std::vector<MaterialType *>;
%template(vector_ModuleThreadProcessorPtr) std::vector<ModuleThreadProcessor *>;
%template(vector_ReceptiveFieldPairPtr) std::vector<ReceptiveFieldPair *>;
%template(vector_ReceptiveFieldPtr) std::vector<ReceptiveField *>;
%template(vector_RigidBodyPtr) std::vector<RigidBody *>;
%template(vector_RobotInterfacePtr) std::vector<RobotInterface *>;
%template(vector_RobotIOControlPtr) std::vector<RobotIOControl *>;
%template(vector_RobotPartInterfacePtr) std::vector<RobotPartInterface *>;
%template(vector_SimulationWindowPtr) std::vector<SimulationWindow *>;
%template(vector_TypePropertyPtr) std::vector<TypeProperty *>;
%template(vector_SimulationThread) std::vector<SimulationThread *>;

%template(map_string_ActivatedItem) std::map<std::string, ActivatedItem *>;
%template(map_string_AnimatBase) std::map<std::string, AnimatBase *>;
%template(map_string_DataColumn) std::map<std::string, DataColumn *>;
%template(map_string_IStdClassFactory) std::map<std::string, IStdClassFactory *>;
%template(map_string_Joint) std::map<std::string, Joint *>;
%template(map_string_NeuralModule) std::map<std::string, NeuralModule *>;
%template(map_string_Odor) std::map<std::string, Odor *>;
%template(map_string_OdorType) std::map<std::string, OdorType *>;
%template(map_string_Organism) std::map<std::string, Organism *>;
%template(map_string_RigidBody) std::map<std::string, RigidBody *>;
%template(map_string_Structure) std::map<std::string, Structure *>;

%include "../../include/StdADT.h"

%template(CStdPoint_int) CStdPoint<int>;
%template(CStdPoint_long) CStdPoint<long>;
%template(CStdPoint_float) CStdPoint<float>;
%template(CStdPoint_double) CStdPoint<double>;

%template(CStdArray_int) CStdArray<int>;
%template(CStdArray_long) CStdArray<long>;
%template(CStdArray_float) CStdArray<float>;
%template(CStdArray_double) CStdArray<double>;
%template(CStdArray_string) CStdArray<std::string>;
%template(CStdArray_ActivatedItem) CStdArray<ActivatedItem *>;
%template(CStdArray_RigidBody) CStdArray<RigidBody *>;
%template(CStdArray_BodyPart) CStdArray<BodyPart *>;
%template(CStdArray_RobotPartInterface) CStdArray<RobotPartInterface *>;
%template(CStdArray_Attachment) CStdArray<Attachment *>;
%template(CStdArray_Adapter) CStdArray<Adapter *>;
%template(CStdArray_KeyFrame) CStdArray<KeyFrame *>;
%template(CStdArray_ModuleThreadProcessor) CStdArray<ModuleThreadProcessor *>;

%template(CStdPtrArray_CStdVariable) CStdPtrArray<CStdVariable>;
%template(CStdPtrArray_TypeProperty) CStdPtrArray<TypeProperty>;
%template(CStdPtrArray_ReceptiveFieldPair) CStdPtrArray<ReceptiveFieldPair>;
%template(CStdPtrArray_ReceptiveField) CStdPtrArray<ReceptiveField>;
%template(CStdPtrArray_DataColumn) CStdPtrArray<DataColumn>;
%template(CStdPtrArray_HudItem) CStdPtrArray<HudItem>;
%template(CStdPtrArray_Light) CStdPtrArray<Light>;
%template(CStdPtrArray_MaterialType) CStdPtrArray<MaterialType>;
%template(CStdPtrArray_Adapter) CStdPtrArray<Adapter>;
%template(CStdPtrArray_RigidBody) CStdPtrArray<RigidBody>;
%template(CStdPtrArray_RobotIOControl) CStdPtrArray<RobotIOControl>;
%template(CStdPtrArray_RobotPartInterface) CStdPtrArray<RobotPartInterface>;
%template(CStdPtrArray_SimulationWindow) CStdPtrArray<SimulationWindow>;
%template(CStdPtrArray_CollisionPair) CStdPtrArray<CollisionPair>;
%template(CStdPtrArray_SimulationThread) CStdPtrArray<SimulationThread>;

%template(CStdCircularArray_float) CStdCircularArray<float>;

%template(CStdStack_double) CStdStack<double>;

%template(CStdMap_string_DataColumn) CStdMap<std::string, DataColumn *>;
%template(CStdMap_string_Odor) CStdMap<std::string, Odor *>;
%template(CStdMap_string_Structure) CStdMap<std::string, Structure *>;
%template(CStdMap_string_IStdClassFactory) CStdMap<std::string, IStdClassFactory *>;
%template(CStdMap_string_AnimatBase) CStdMap<std::string, AnimatBase *>;
%template(CStdMap_string_RigidBody) CStdMap<std::string, RigidBody *>;
%template(CStdMap_string_Joint) CStdMap<std::string, Joint *>;

%template(CStdPtrMap_string_ActivatedItem) CStdPtrMap<std::string, ActivatedItem>;
%template(CStdPtrMap_string_NeuralModule) CStdPtrMap<std::string, NeuralModule>;
%template(CStdPtrMap_string_Odor) CStdPtrMap<std::string, Odor>;
%template(CStdPtrMap_string_OdorType) CStdPtrMap<std::string, OdorType>;
%template(CStdPtrMap_string_Organism) CStdPtrMap<std::string, Organism>;
%template(CStdPtrMap_string_Structure) CStdPtrMap<std::string, Structure>;


%{ 
	namespace swig 
	{ 
		template <>  struct traits<AnimatBase> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "AnimatBase *"; } 
		}; 

		template <>  struct traits<IStdClassFactory> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "IStdClassFactory *"; } 
		}; 

		template <>  struct traits<Joint> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "Joint *"; } 
		}; 

		template <>  struct traits<NeuralModule> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "NeuralModule *"; } 
		}; 

		template <>  struct traits<Odor> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "Odor *"; } 
		}; 

		template <>  struct traits<OdorType> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "OdorType *"; } 
		}; 

		template <>  struct traits<Organism> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "Organism *"; } 
		}; 

		template <>  struct traits<Structure> 
		{ 
		   typedef pointer_category category; 
		   static const char* type_name() { return "Structure *"; } 
		}; 
	} 
%} 


%include "../StdUtils/StdSerialize.h"
%include "../StdUtils/MarkupSTL.h"
%include "../StdUtils/StdXml.h"
%include "../StdUtils/StdSerialize.h"
%include "../StdUtils/StdFont.h"
%include "../StdUtils/StdVariable.h"
%include "../StdUtils/StdPostFixEval.h"
%include "../StdUtils/StdVariant.h"
%include "../StdUtils/StdClassFactory.h"
%include "../StdUtils/StdLookupTable.h"
%include "../StdUtils/StdFixed.h"
%include "../StdUtils/StdColor.h"
%include "../StdUtils/StdCriticalSection.h"
%include "../StdUtils/StdPID.h"
%include "../StdUtils/StdVariable.h"

%exception {
	try {
	$action
	}
	catch(CStdErrorInfo oError)
	{
		//A critical simulation error has occurred if we catch an exception here. We need to shut the app down.
		std::string strError = "AnimatLab Error occurred.\nError: " + oError.m_strError;
	 	PyErr_SetString(PyExc_IndexError, strError.c_str());
		SWIG_fail;
	}
	catch(...)
	{
		//A critical simulation error has occurred if we catch an exception here. We need to shut the app down.
	 	PyErr_SetString(PyExc_IndexError, "unknown system error has occurred.");
		SWIG_fail;
	}	
}

%include "../AnimatSim/AnimatUtils.h"
%include "../AnimatSim/IMovableItemCallback.h"
%include "../AnimatSim/IMotorizedJoint.h"
%include "../AnimatSim/ISimGUICallback.h"
%include "../AnimatSim/AnimatBase.h"
%include "../AnimatSim/DelayLine.h"
%include "../AnimatSim/Gain.h"
%include "../AnimatSim/BellGain.h"
%include "../AnimatSim/EquationGain.h"
%include "../AnimatSim/PolynomialGain.h"
%include "../AnimatSim/SigmoidGain.h"
%include "../AnimatSim/LengthTensionGain.h"
%include "../AnimatSim/Node.h"
%include "../AnimatSim/Link.h"
%include "../AnimatSim/IPhysicsMovableItem.h"
%include "../AnimatSim/IPhysicsBody.h"
%include "../AnimatSim/BoundingBox.h"
%include "../AnimatSim/MovableItem.h"
%include "../AnimatSim/BodyPart.h"

%include "../AnimatSim/ReceptiveFieldPair.h"
%include "../AnimatSim/Adapter.h"
%include "../AnimatSim/PropertyControlAdapter.h"
%include "../AnimatSim/ContactAdapter.h"
%include "../AnimatSim/ConstraintLimit.h"
%include "../AnimatSim/ConstraintRelaxation.h"
%include "../AnimatSim/ConstraintFriction.h"
%include "../AnimatSim/Joint.h"
%include "../AnimatSim/MotorizedJoint.h"
%include "../AnimatSim/BallSocket.h"
%include "../AnimatSim/Hinge.h"
%include "../AnimatSim/Prismatic.h"
%include "../AnimatSim/LinearJoint.h"
%include "../AnimatSim/RPRO.h"
%include "../AnimatSim/ContactSensor.h"
%include "../AnimatSim/Odor.h"
%include "../AnimatSim/OdorType.h"
%include "../AnimatSim/RigidBody.h"
%include "../AnimatSim/Plane.h"
%include "../AnimatSim/FluidPlane.h"
%include "../AnimatSim/Cylinder.h"
%include "../AnimatSim/Cone.h"
%include "../AnimatSim/Ellipsoid.h"
%include "../AnimatSim/Torus.h"
%include "../AnimatSim/Box.h"
%include "../AnimatSim/Mesh.h"
%include "../AnimatSim/Terrain.h"
%include "../AnimatSim/Sensor.h"
%include "../AnimatSim/Attachment.h" 
%include "../AnimatSim/LineBase.h"
%include "../AnimatSim/MuscleBase.h" 
%include "../AnimatSim/LinearHillMuscle.h" 
%include "../AnimatSim/LinearHillStretchReceptor.h" 
%include "../AnimatSim/OdorSensor.h"
%include "../AnimatSim/Sphere.h"
%include "../AnimatSim/Spring.h"
%include "../AnimatSim/Stomach.h"
%include "../AnimatSim/Mouth.h"
%include "../AnimatSim/Structure.h"
%include "../AnimatSim/NervousSystem.h"
%include "../AnimatSim/NeuralModule.h"
%include "../AnimatSim/PhysicsNeuralModule.h"
%include "../AnimatSim/Organism.h"
%include "../AnimatSim/Light.h"
%include "../AnimatSim/LightManager.h"
%include "../AnimatSim/ActivatedItem.h"
%include "../AnimatSim/ActivatedItemMgr.h"
%include "../AnimatSim/DataColumn.h"
%include "../AnimatSim/DataChart.h"
%include "../AnimatSim/FileChart.h"
%include "../AnimatSim/ArrayChart.h"
%include "../AnimatSim/MemoryChart.h"
%include "../AnimatSim/DataChartMgr.h"
%include "../AnimatSim/ExternalStimulus.h"
%include "../AnimatSim/ExternalStimuliMgr.h"
%include "../AnimatSim/ExternalInputStimulus.h"
%include "../AnimatSim/CurrentStimulus.h"
%include "../AnimatSim/EnablerStimulus.h"
%include "../AnimatSim/VoltageClamp.h"
%include "../AnimatSim/InverseMuscleCurrent.h"
%include "../AnimatSim/PropertyControlStimulus.h"
%include "../AnimatSim/MotorVelocityStimulus.h"
%include "../AnimatSim/ForceStimulus.h"
%include "../AnimatSim/KeyFrame.h"
%include "../AnimatSim/SimulationRecorder.h"
%include "../AnimatSim/MaterialType.h"
%include "../AnimatSim/Materials.h"
%include "../AnimatSim/HudItem.h"
%include "../AnimatSim/HudText.h"
%include "../AnimatSim/Hud.h"
%include "../AnimatSim/SimulationWindow.h"
%include "../AnimatSim/HudItem.h"
%include "../AnimatSim/Hud.h"
%include "../AnimatSim/SimulationWindowMgr.h"
%include "../AnimatSim/Simulator.h"
%include "../AnimatSim/ObjectScript.h"
%include "../AnimatSim/SimulationThread.h"
%include "../AnimatSim/SimulationMgr.h"

