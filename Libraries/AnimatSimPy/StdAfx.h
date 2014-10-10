// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "AnimatSim.h"
#include "FiringRateSim.h"
#include "AnimatSimPyConstants.h"

#if _MSC_VER > 1000
#pragma once
#endif 

#ifdef WIN32
	#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
	 
	#include <windows.h>
	
	#define ANIMATSIMPY_PORT __declspec( dllexport )
#else
	#define ANIMATSIMPY_PORT	
#endif