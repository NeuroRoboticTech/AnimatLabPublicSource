#ifndef __[*TAG_NAME*]_LIB_DLL_H__
#define __[*TAG_NAME*]_LIB_DLL_H__

#ifndef _[*TAG_NAME*]_LIB_DLL_NOFORCELIBS
	#if _MSC_VER > 1300  // VC 7
		#ifdef _DEBUG
			#pragma comment(lib, "[*PROJECT_NAME*]_vc7D.lib")
		#else
			#pragma comment(lib, "[*PROJECT_NAME*]_vc7.lib")
		#endif
	#else   // VC 6
		#ifdef _DEBUG
			#pragma comment(lib, "[*PROJECT_NAME*]_vc6D.lib")
		#else
			#pragma comment(lib, "[*PROJECT_NAME*]_vc6.lib")
		#endif
	#endif
#endif          // _[*TAG_NAME*]_LIB_DLL_NOFORCELIBS

#define [*TAG_NAME*]_PORT __declspec( dllimport )

#include "StdUtils.h"
#include "[*PROJECT_NAME*]Constants.h"

//Simulation Objects
namespace [*PROJECT_NAME*]
{
	class ClassFactory;
	class [*PROJECT_NAME*]NeuralModule;

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

using namespace [*PROJECT_NAME*];
using namespace [*PROJECT_NAME*]::DataColumns;
using namespace [*PROJECT_NAME*]::ExternalStimuli;
using namespace [*PROJECT_NAME*]::Neurons;
using namespace [*PROJECT_NAME*]::Synapses;

#include "NeuralUtils.h"

#endif // __[*TAG_NAME*]_LIB_DLL_H__
