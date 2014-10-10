#pragma once 

#ifndef _ANIMATSIMPY_LIB_DLL_NOFORCELIBS
    #ifndef _WIN64
	    #ifdef _DEBUG
		    #pragma comment(lib, "AnimatSimPyD.lib")
	    #else
		    #pragma comment(lib, "AnimatSimPy.lib")
	    #endif      // _DEBUG  
    #else
	    #ifdef _DEBUG
		    #pragma comment(lib, "AnimatSimPyD_x64.lib")
	    #else
		    #pragma comment(lib, "AnimatSimPy_x64.lib")
	    #endif      // _DEBUG  
    #endif          // _WIN64
#endif          // _ANIMATSIMPY_LIB_DLL_NOFORCELIBS

#define ANIMATSIMPY_PORT __declspec( dllimport )

#include "Python.h"
#include "AnimatSim.h"
#include "AnimatSimPyConstants.h"

//Simulation Objects
namespace AnimatSimPy
{
	class ObjectScriptPy;
}

using namespace AnimatSimPy;

#include "ScriptProcessorPy.h"
#include "PyClassFactory.h"

