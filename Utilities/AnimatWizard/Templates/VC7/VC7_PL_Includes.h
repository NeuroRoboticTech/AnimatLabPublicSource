#ifndef __[*TAG_NAME*]_INCLUDES_H__
#define __[*TAG_NAME*]_INCLUDES_H__

#define [*TAG_NAME*]_PORT __declspec( dllexport )

#include "StdUtils.h"
#include "AnimatLibrary.h"
#include "VortexAnimatLibrary.h"
#include "[*PROJECT_NAME*]Constants.h"

//Simulation Objects
namespace [*PROJECT_NAME*]
{
	class ClassFactory;

	namespace Bodies
	{
		class [*BODY_PART_NAME*];
		class [*MUSCLE_NAME*];
	}

	namespace ExternalStimuli
	{
		class [*STIMULUS_NAME*];
	}
}

using namespace [*PROJECT_NAME*];
using namespace [*PROJECT_NAME*]::Bodies;
using namespace [*PROJECT_NAME*]::ExternalStimuli;

#endif // __[*TAG_NAME*]_INCLUDES_H__
