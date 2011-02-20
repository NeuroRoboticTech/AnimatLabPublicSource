#ifndef __ANIMAT_INCLUDES_H__
#define __ANIMAT_INCLUDES_H__

#define ANIMAT_PORT __declspec( dllexport )

#define STD_TRACING_ON

#include "StdUtils.h"
#include "AnimatConstants.h"
#include "AnimatUtils.h"

//Simulation Objects
namespace AnimatSim
{
	class Simulator;
	class Node;
	class ActivatedItem;
	class ActivatedItemMgr;

	namespace Adapters
	{
		class Adapter;
	}

	namespace Behavior
	{
		class NervousSystem;
		class NeuralModule;
	}

	namespace Charting
	{
		class DataChart;
		class DataChartMgr;
		class DataColumn;
	}

	namespace Environment
	{
		class RigidBody;
		class Joint;
		class Structure;
		class Organism;

		namespace Bodies
		{
			class Box;
			class Cylinder;
			class Cone;
			class Muscle;
			class Attachment;
			class Plane;
			class Sensor;
			class Sphere;
		}

		namespace Joints
		{
			class Hinge;
			class Static;
		}
	}

	namespace ExternalStimuli
	{
		class ExternalStimuliMgr;
		class ExternalStimulus;
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

#endif // __ANIMAT_INCLUDES_H__
