#ifndef __STD_UTILS_DLL_H__
#define __STD_UTILS_DLL_H__

#ifndef _STD_UTILS_DLL_NOFORCELIBS
	#ifdef _DEBUG
		#pragma comment(lib, "StdUtils_vc10D.lib")
	#else
		#pragma comment(lib, "StdUtils_vc10.lib")
	#endif          
#endif          // _STD_UTILS_DLL_NOFORCELIBS

#ifdef WIN32
	#define STD_UTILS_PORT __declspec( dllimport )
#else
	#define STD_UTILS_PORT 
#endif

#pragma warning(disable: 4018 4244 4290 4786 4251 4275 4267 4311 4312 4800 4003 4482 4996)


#ifdef WIN32
	#ifndef _WIN32_WCE
		#include <conio.h>
		#include <io.h>
		#include <tchar.h> 
	#endif
	
	#include <direct.h>
	#include <wtypes.h>
#else
	#include <linux/types.h>
	#include <stdbool.h>
	#include <dlfcn.h>
	
	#define BOOL bool
	#define DWORD unsigned long
	#define LPCSTR const char *
	#define FALSE 0
	#define TRUE 1
	#define LONG long
	#define ULONG unsigned long
	#define LPLONG long *
	#define __int64 int64_t
	#define LPCTSTR const char*
	#define byte unsigned char
#endif

#include <sys/types.h>
#include <sys/timeb.h>

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
#include <list>
#include <stdio.h>
#include <stdlib.h>
#include <cctype>
#include <math.h>
#include <memory.h>
#include <algorithm>
#include <string.h>
//#include <vfw.h>
using namespace std;

#define STD_TRACING_ON
//#define STD_TRACE_DEBUG
//#define STD_TRACE_INFO
#define STD_TRACE_DETAIL
#define STD_TRUNCATE_LARGE_LOG

#define STD_TRACE_TO_DEBUGGER 
#define STD_TRACE_TO_FILE true

namespace StdUtils
{
	class CStdFont;
	class CStdVariable;
	class CStdPostFixEval;
	class CMarkupSTL;
	class CStdXml;
	class CStdCriticalSection;
	class CStdErrorInfo;

#ifndef THROW_ERROR
	#define THROW_ERROR(lError, strError) Std_ThrowError(lError, strError, __FILE__, __LINE__, "")
#endif
	
	void STD_UTILS_PORT Std_RelayError(CStdErrorInfo oInfo, string strSourceFile, long lSourceLine);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strValueName, unsigned char iVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strValueName, unsigned short iVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strValueName, int iVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strValueName, long lVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strValueName, float fltVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strValueName, double dblVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strValueName, string strVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																		 string strText);
}

#include "tree.hh"
#include "tree_util.hh"

#include "StdConstants.h"
#include "StdADT.h"
#include "StdADTCopy.h"
#include "StdErrorInfo.h"
#include "StdUtilFunctions.h"
#include "MarkupSTL.h"
#include "StdXml.h"
#include "StdSerialize.h"
#include "StdFont.h"
#include "StdVariable.h"
#include "StdPostFixEval.h"
#include "StdVariant.h"
#include "StdClassFactory.h"
#include "StdLookupTable.h"
#include "StdFixed.h"
#include "StdColor.h"
#include "StdCriticalSection.h"

using namespace StdUtils;

#endif // __STD_UTILS_DLL_H__
