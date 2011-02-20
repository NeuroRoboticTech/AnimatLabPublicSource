#ifndef __TEST_LIB_DLL_H__
#define __TEST_LIB_DLL_H__

#ifndef _TEST_LIB_DLL_NOFORCELIBS
	#if _MSC_VER > 1300  // VC 7
		#ifdef _DEBUG
			#pragma comment(lib, "Test_vc7D.lib")
		#else
			#pragma comment(lib, "Test_vc7.lib")
		#endif
	#else   // VC 6
		#ifdef _DEBUG
			#pragma comment(lib, "Test_vc6D.lib")
		#else
			#pragma comment(lib, "Test_vc6.lib")
		#endif
	#endif
#endif          // _TEST_LIB_DLL_NOFORCELIBS

#define TEST_PORT __declspec( dllimport )

#include "StdUtils.h"
#include "TestConstants.h"

//Simulation Objects
namespace Test
{
	class ClassFactory;
	class TestNeuralModule;

	namespace DataColumns
	{
		class NeuronData;
	}

	namespace Neurons
	{
		class Neuron;
	}

	namespace Synapses
	{
		class Synapse;
	}
}

using namespace Test;
using namespace Test::DataColumns;
using namespace Test::ExternalStimuli;
using namespace Test::Neurons;
using namespace Test::Synapses;

#include "NeuralUtils.h"

#endif // __TEST_LIB_DLL_H__
