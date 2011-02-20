#if !defined STD_UTILS_INCLUDES
#define STD_UTILS_INCLUDES

#define STD_UTILS_PORT __declspec( dllexport )

#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

#pragma warning(disable: 4018 4244 4290 4786 4251 4275 4267 4311 4312 4800 )

#ifndef _WIN32_WCE
	#include <conio.h>
	#include <io.h>
	#include <sys/types.h>
	#include <sys/timeb.h>
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
using namespace std;

//#define STD_TRACING_ON
//#define STD_TRACE_DEBUG
//#define STD_TRACE_INFO
//#define STD_TRACE_DETAIL

#define STD_TRACE_TO_DEBUGGER
#define STD_TRACE_TO_FILE true

class CStdFont;
class CStdVariable;
class CStdPostFixEval;
class CMarkupSTL;
class CStdXml;
class CStdCriticalSection;

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
#include "StdColor.h"

#ifndef _WIN32_WCE
	#include "XYTrace.h"
#else
	#include "StdLogFile.h"
#endif

#endif STD_UTILS_INCLUDES
