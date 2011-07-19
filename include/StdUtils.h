#ifndef __STD_UTILS_DLL_H__
#define __STD_UTILS_DLL_H__

#ifndef _STD_UTILS_DLL_NOFORCELIBS
	#ifdef _DEBUG
		#pragma comment(lib, "StdUtils_vc10D.lib")
	#else
		#pragma comment(lib, "StdUtils_vc10.lib")
	#endif          
#endif          // _STD_UTILS_DLL_NOFORCELIBS

#define STD_UTILS_PORT __declspec( dllimport )

#pragma warning(disable: 4018 4244 4290 4786 4251 4275 4267 4311 4312 4800 4003 4482 4996)

#ifndef _WIN32_WCE
	#include <conio.h>
	#include <sys/types.h>
	#include <sys/timeb.h>
	#include <io.h>
	#include <direct.h>
#endif

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
#include <tchar.h> 
#include <string.h>
#include <wtypes.h>
#include <vfw.h>
using namespace std;

//#define STD_TRACING_ON
//#define STD_TRACE_DEBUG
//#define STD_TRACE_INFO
//#define STD_TRACE_DETAIL
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
	class CStdBitmap;
	class CStdAvi;
}

#ifndef THROW_ERROR
	#define THROW_ERROR(lError, strError) Std_ThrowError(lError, strError, __FILE__, __LINE__)
#endif

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
#include "StdCriticalSection.h"
#include "StdFixed.h"
#include "StdBitmap.h"
#include "StdAvi.h"
#include "StdColor.h"

#ifndef _WIN32_WCE
	#define STRING_TYPE LPCSTR
#else
	#define STRING_TYPE LPCWSTR
	#include "StdLogFile.h"
#endif

using namespace StdUtils;

#endif // __STD_UTILS_DLL_H__
