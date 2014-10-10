// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__AC7276DB_F5B0_4434_A20E_8194C391BF3C__INCLUDED_)
#define AFX_STDAFX_H__AC7276DB_F5B0_4434_A20E_8194C391BF3C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

#ifdef WIN32
	#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
	 
	#include <windows.h>
	
	#define ANIMAT_PORT __declspec( dllexport )
#else
	#define ANIMAT_PORT	
#endif

#define STD_TRACING_ON

//#ifdef _DEBUG
	#define STD_TRACE_DETAIL
//#else
//	#define STD_TRACE_DETAIL
//#endif

#include "StdUtils.h"
#include "AnimatConstants.h"
#include "AnimatUtils.h"

//Simulation Objects
namespace AnimatSim
{
	class IMovableItemCallback;
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
    class ThreadProcessor;
    class ModuleThreadProcessor;
	class TypeProperty;

	namespace Adapters
	{
		class Adapter;
		class ContactAdapter;
	}

	namespace Behavior
	{
		class NervousSystem;
		class NeuralModule;
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
		class BodyPart;
		class ConstraintLimit;
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
		class Material;
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
        class MotorVelocityStimulus;
	}

	namespace Gains
	{
		class BellGain;
		class EquationGain;
		class Gain;
		class PolynomialGain;
		class SigmoidGain;
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
using namespace AnimatSim::Environment;
using namespace AnimatSim::Environment::Bodies;
using namespace AnimatSim::Environment::Joints;
using namespace AnimatSim::ExternalStimuli;
using namespace AnimatSim::Gains;
using namespace AnimatSim::Recording;
using namespace AnimatSim::Robotics;


#include "AnimatUtils.h"

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__AC7276DB_F5B0_4434_A20E_8194C391BF3C__INCLUDED_)
