#ifndef __ANIMAT_LIB_DLL_H__
#define __ANIMAT_LIB_DLL_H__

#ifndef _ANIMAT_LIB_DLL_NOFORCELIBS
    #ifndef _WIN64
	    #ifdef _DEBUG
		    #pragma comment(lib, "AnimatSim_vc10D.lib")
	    #else
		    #pragma comment(lib, "AnimatSim_vc10.lib")
	    #endif      // _DEBUG  
    #else
	    #ifdef _DEBUG
		    #pragma comment(lib, "AnimatSim_vc10D_x64.lib")
	    #else
		    #pragma comment(lib, "AnimatSim_vc10_x64.lib")
	    #endif      // _DEBUG  
    #endif          // _WIN64
#endif          // _ANIMAT_LIB_DLL_NOFORCELIBS

#ifdef WIN32	
	#define ANIMAT_PORT __declspec( dllimport )
#else
	#define ANIMAT_PORT
#endif

#include "StdUtils.h"
#include "AnimatConstants.h"

//Simulation Objects
namespace AnimatSim
{
	class IMovableItemCallback;
	class IGUI_AppCallback;
	class AnimatBase;
	class Simulator;
	class Node;
	class Link;
	class ActivatedItem;
	class ActivatedItemMgr;
	class DelayLine;
	class SimulationWindow;
	class SimulationWindowMgr;
	class BoundingBox;
	class Hud;
	class HudItem;
	class HudText;
    class PidControl;
    class ThreadProcessor;
    class ModuleThreadProcessor;

	namespace Adapters
	{
		class Adapter;
		class ContactAdapter;
		class PropertyControlAdapter;
	}

	namespace Behavior
	{
		class NervousSystem;
		class NeuralModule;
		class PhysicsNeuralModule;
	}

	namespace Charting
	{
		class DataChart;
		class FileChart;
		class ArrayChart;
		class MemoryChart;
		class DataChartMgr;
		class DataColumn;

		namespace DataColumns
		{
		}
	}

	namespace Environment
	{
		class IPhysicsMovableItem;
		class IPhysicsBody;
		class IMotorizedJoint;
        class IRobotInterface;
		class BodyPart;
		class ConstraintLimit;
        class ConstratinRelaxation;
        class ConstratinFriction;
		class RigidBody;
		class Joint;
		class MotorizedJoint;
		class Structure;
		class Organism;
		class ContactSensor;
		class ReceptiveField;
		class ReceptiveFieldPair;
		class Odor;
		class OdorType;
		class Materials;
		class MaterialType;
		class Light;
		class LightManager;

		namespace Bodies
		{
			class Box;
			class Cylinder;
			class Cone;
			class Mesh;
			class LineBase;
			class MuscleBase;
			class Attachment;
			class LinearHillMuscle;
			class LinearHillStretchReceptor;
			class OdorSensor;
			class Plane;
			class FluidPlane;
			class Terrain;
			class Sensor;
			class Sphere;
			class Spring;
			class Stomach;
			class Mouth;
			class Ellipsoid;
			class Torus;
		}

		namespace Joints
		{
			class BallSocket;
			class Hinge;
			class Prismatic;
			class LinearJoint;
			class RPRO;
		}
	}

	namespace ExternalStimuli
	{
		class ExternalStimuliMgr;
		class ExternalStimulus;
		class ExternalInputStimulus;
		class CurrentStimulus;
		class EnablerStimulus;
		class VoltageClamp;
		class InverseMuscleCurrent;
		class PropertyControlStimulus;
        class MotorVelocityStimulus;
        class ForceStimulus;
	}

	namespace Gains
	{
		class BellGain;
		class EquationGain;
		class Gain;
		class PolynomialGain;
		class SigmoidGain;
		class LengthTensionGain;
	}

	namespace Recording
	{
		class KeyFrame;
		class SimulationRecorder;
	}

    namespace Robotics
    {
        class RobotInterface;
		class RobotIOControl;
        class RobotPartInterface;
    }
}

using namespace AnimatSim;
using namespace AnimatSim::Adapters;
using namespace AnimatSim::Behavior;
using namespace AnimatSim::Charting;
using namespace AnimatSim::Charting::DataColumns;
using namespace AnimatSim::Environment;
using namespace AnimatSim::Environment::Bodies;
using namespace AnimatSim::Environment::Joints;
using namespace AnimatSim::ExternalStimuli;
using namespace AnimatSim::Gains;
using namespace AnimatSim::Recording;
using namespace AnimatSim::Robotics;

#include "IMovableItemCallback.h"
#include "IMotorizedJoint.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"
#include "DelayLine.h"
#include "Gain.h"
#include "BellGain.h"
#include "EquationGain.h"
#include "PolynomialGain.h"
#include "SigmoidGain.h"
#include "LengthTensionGain.h"
#include "Node.h"
#include "Link.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "ReceptiveField.h"
#include "ReceptiveFieldPair.h"
#include "Adapter.h"
#include "PropertyControlAdapter.h"
#include "ContactAdapter.h"
#include "ConstraintLimit.h"
#include "ConstraintRelaxation.h"
#include "ConstraintFriction.h"
#include "Joint.h"
#include "MotorizedJoint.h"
#include "BallSocket.h"
#include "Hinge.h"
#include "Prismatic.h"
#include "LinearJoint.h"
#include "RPRO.h"
#include "ContactSensor.h"
#include "Odor.h"
#include "OdorType.h"
#include "RigidBody.h"
#include "Plane.h"
#include "FluidPlane.h"
#include "Cylinder.h"
#include "Cone.h"
#include "Ellipsoid.h"
#include "Torus.h"
#include "Box.h"
#include "Mesh.h"
#include "Terrain.h"
#include "Sensor.h"
#include "Attachment.h" 
#include "LineBase.h"
#include "MuscleBase.h" 
#include "LinearHillMuscle.h" 
#include "LinearHillStretchReceptor.h" 
#include "OdorSensor.h"
#include "Sphere.h"
#include "Spring.h"
#include "Stomach.h"
#include "Mouth.h"
#include "Structure.h"
#include "NervousSystem.h"
#include "NeuralModule.h"
#include "PhysicsNeuralModule.h"
#include "Organism.h"
#include "Light.h"
#include "LightManager.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataColumn.h"
#include "DataChart.h"
#include "FileChart.h"
#include "ArrayChart.h"
#include "MemoryChart.h"
#include "DataChartMgr.h"
#include "ExternalStimulus.h"
#include "ExternalStimuliMgr.h"
#include "ExternalInputStimulus.h"
#include "CurrentStimulus.h"
#include "EnablerStimulus.h"
#include "VoltageClamp.h"
#include "InverseMuscleCurrent.h"
#include "PropertyControlStimulus.h"
#include "MotorVelocityStimulus.h"
#include "ForceStimulus.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "MaterialType.h"
#include "Materials.h"
#include "HudItem.h"
#include "HudText.h"
#include "Hud.h"
#include "SimulationWindow.h"
#include "HudItem.h"
#include "Hud.h"
#include "SimulationWindowMgr.h"
#include "Simulator.h"

#include "AnimatUtils.h"

#endif // __ANIMAT_LIB_DLL_H__
