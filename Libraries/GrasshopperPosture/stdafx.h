// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once


#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>

#pragma warning(disable: 4005)

#define LOCUST_PORT __declspec( dllexport )

//#define STD_TRACING_ON

#include "StdUtils.h"
#include "AnimatSim.h"
#include "VortexAnimatSim.h"

#define Nl_Err_lInvalidExternalStimulusType -3027
#define Nl_Err_strInvalidExternalStimulusType "Invalid external stimulus type."

//Stimulus Types
#define POSTURE_CONTROL_STIM 0
#define SYNERGY_FITNESS_STIM 1

//Class Types
//#define EXTERNAL_STIMULUS_CLASS 0
