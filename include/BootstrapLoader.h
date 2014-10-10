#ifndef __BOOTSTRAP_LOADER_DLL_H__
#define __BOOTSTRAP_LOADER_DLL_H__

#ifndef _BOOTSTRAP_LOADER_DLL_NOFORCELIBS
    #ifndef _WIN64
	    #ifdef _DEBUG
		    #pragma comment(lib, "BootstrapLoader_vc10D.lib")
	    #else
		    #pragma comment(lib, "BootstrapLoader_vc10.lib")
	    #endif      // _DEBUG  
    #else
	    #ifdef _DEBUG
		    #pragma comment(lib, "BootstrapLoader_vc10D_x64.lib")
	    #else
		    #pragma comment(lib, "BootstrapLoader_vc10_x64.lib")
	    #endif      // _DEBUG  
    #endif          // _WIN64
#endif          // _BOOTSTRAP_LOADER_DLL_NOFORCELIBS

#ifdef WIN32
	#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
	
	#include <windows.h>

	#define BOOTSTRAP_LOADER_PORT __declspec( dllimport )

	#include <direct.h>
	#include <conio.h>
	#include <io.h>
#else
	#define BOOTSTRAP_LOADER_PORT
	
	#include <linux/types.h>
	#include <stdbool.h>
	#include <dlfcn.h>
    #include <limits.h>
    #include <unistd.h>

	#define DWORD unsigned long
	#define LPCSTR const char *
	#define FALSE 0
	#define TRUE 1
	#define LONG long
	#define ULONG unsigned long
	#define LPLONG long *
	#define __int64 int64_t
	#define LPCTSTR const char*
	#define _stricmp strcmp
#endif

#pragma warning(disable: 4018 4244 4290 4786 4251 4275 4267 4311)

#include <exception>
#include <string>
#include <iosfwd>
#include <sstream>
#include <fstream>
#include <iterator>
#include <iostream>
#include <vector>
#include <deque>
#include <stack>
#include <map>
#include <stdio.h>
#include <cctype>
#include <math.h>
#include <memory.h>
#include <sys/types.h>
#include <sys/timeb.h>

int BOOTSTRAP_LOADER_PORT BootStrap_RunLibrary(int argc, const char **argv);


#endif // __BOOTSTRAP_LOADER_DLL_H__
