// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#ifdef WIN32
	#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
	
	#include <windows.h>

	#define BOOTSTRAP_LOADER_PORT __declspec( dllexport )

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
#include <boost/filesystem.hpp>

