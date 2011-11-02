/**
\file	StdUtilFunctions.h

\brief	Declares the standard utility functions class.
**/

namespace StdUtils
{

//#define STD_LOG_DB_ON 
#define STD_LOG_DB_DSN ""
#define STD_LOG_DB_USER ""
#define STD_LOG_DB_PSWD ""

enum StdLogLevel
{
	StdLogNone = 0, // no trace
	StdLogError = 10, // only trace error
	StdLogInfo = 20, // some extra info
	StdLogDebug = 30, // debugging info
	StdLogDetail = 40 // detailed debugging info
};

/*
void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strValueName, long lVal);
void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strValueName, double dblVal);
void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strValueName, string strVal);
void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strAddToError = "");
void STD_UTILS_PORT RelayError(CStdErrorInfo oInfo);
*/

// -----------------------------------------------------------------------------
// MIN and MAX.  The Standard C++ template versions go by so many names (at
// at least in the MS implementation) that you never know what's available 
// -----------------------------------------------------------------------------
template<class Type>
inline const Type& SSMIN(const Type& arg1, const Type& arg2)
{
	return arg2 < arg1 ? arg2 : arg1;
}
template<class Type>
inline const Type& SSMAX(const Type& arg1, const Type& arg2)
{
	return arg2 > arg1 ? arg2 : arg1;
}

string STD_UTILS_PORT Std_CreateAppID();

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
																	 string strText = "");

#ifndef RELAY_ERROR
	#define RELAY_ERROR(oError) Std_RelayError(oError, __FILE__, __LINE__)
#endif

#ifndef THROW_PARAM_ERROR
	#define THROW_PARAM_ERROR(lError, strError, strValueName, Val) Std_ThrowError(lError, strError, __FILE__, __LINE__, strValueName, Val)
#endif

#ifndef THROW_TEXT_ERROR
	#define THROW_TEXT_ERROR(lError, strError, strText) Std_ThrowError(lError, strError, __FILE__, __LINE__, strText)
#endif

#ifndef THROW_ERROR
	#define THROW_ERROR(lError, strError) Std_ThrowError(lError, strError, __FILE__, __LINE__)
#endif

string STD_UTILS_PORT Std_ToStr(string strVal);
string STD_UTILS_PORT Std_ToStr(const char *strVal);
string STD_UTILS_PORT Std_ToStr(unsigned char iVal);
string STD_UTILS_PORT Std_ToStr(unsigned short iVal);
string STD_UTILS_PORT Std_ToStr(int iVal);
string STD_UTILS_PORT Std_ToStr(long lVal);
string STD_UTILS_PORT Std_ToStr(float fltVal);
string STD_UTILS_PORT Std_ToStr(double dblVal);
string STD_UTILS_PORT Std_ToStr(bool bVal);
string STD_UTILS_PORT Std_ToStr(string strFormat, unsigned char iVal);
string STD_UTILS_PORT Std_ToStr(string strFormat, unsigned short iVal);
string STD_UTILS_PORT Std_ToStr(string strFormat, int iVal);
string STD_UTILS_PORT Std_ToStr(string strFormat, long lVal);
string STD_UTILS_PORT Std_ToStr(string strFormat, float fltVal);
string STD_UTILS_PORT Std_ToStr(string strFormat, double dblVal);

bool STD_UTILS_PORT Std_ToBool(int iVal);
bool STD_UTILS_PORT Std_ToBool(string strVal);
string STD_UTILS_PORT Std_NullStr(string strFormat);
string STD_UTILS_PORT Std_ConvertToANSI(LPCWSTR strData);
LPWSTR STD_UTILS_PORT Std_ConvertFromANSI(string strData);

#define STR(v) Std_ToStr(v)
#define FSTR(f, v) Std_ToStr(f, v)
#define NULL_STR(s) Std_NullStr(s)

int STD_UTILS_PORT Std_VariantTypeToConst(string strType);
string STD_UTILS_PORT Std_ConstToVariantType(int iType);

int STD_UTILS_PORT  Std_Split(const string& input, const string& delimiter, CStdArray<string> &results);
string STD_UTILS_PORT Std_Combine(CStdArray<string> &aryParts, string strDelimiter);
string STD_UTILS_PORT Std_Trim(string strVal);
string STD_UTILS_PORT Std_TrimLeft(string strVal);
string STD_UTILS_PORT Std_TrimRight(string strVal);
BOOL STD_UTILS_PORT Std_IsNumeric(string strVal);
BOOL STD_UTILS_PORT Std_IsIntegerType(string strVal);
string STD_UTILS_PORT Std_Left(string strVal, int iCount);
string STD_UTILS_PORT Std_Right(string strVal, int iCount);
string STD_UTILS_PORT Std_ToUpper(string strVal);
string STD_UTILS_PORT Std_ToLower(string strVal);
string STD_UTILS_PORT Std_Replace(string strVal, string strFind, string strReplace);
string STD_UTILS_PORT Std_Format(const char* szFormat,...);

long STD_UTILS_PORT Std_RGB(unsigned char iRed, unsigned char iGreen, unsigned char iBlue);
long STD_UTILS_PORT Std_LoadRGB(CStdXml &oXml, string strParamName, bool bThrowError = true, long lDefault = 0);
void STD_UTILS_PORT Std_LoadColor(CStdXml &oXml, string strParamName, float *aryColor, bool bThrowError = true);
void STD_UTILS_PORT Std_LoadColor(string strXml, string strParamName, float *aryColor, bool bThrowError = true);

int STD_UTILS_PORT Std_Sign(float fltVal);
void STD_UTILS_PORT Std_SRand(unsigned long lSeed);
int STD_UTILS_PORT Std_IRand( int low, int high );
long STD_UTILS_PORT Std_LRand( long low, long high );
float STD_UTILS_PORT Std_FRand( float low, float high );
double STD_UTILS_PORT Std_DRand( double low, double high );


bool STD_UTILS_PORT Std_InValidRange(int iMinVal, int iMaxVal, int iVal, bool bThrowError = true, string strParamName = "");
bool STD_UTILS_PORT Std_InValidRange(long lMinVal, long lMaxVal, long lVal, bool bThrowError = true, string strParamName = "");
bool STD_UTILS_PORT Std_InValidRange(float fltMinVal, float fltMaxVal, float fltVal, bool bThrowError = true, string strParamName = "");
bool STD_UTILS_PORT Std_InValidRange(double dblMinVal, double dblMaxVal, double dblVal, bool bThrowError = true, string strParamName = "");

bool STD_UTILS_PORT Std_IsBelowMax(int iMaxVal, int iVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);
bool STD_UTILS_PORT Std_IsBelowMax(long lMaxVal, long lVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);
bool STD_UTILS_PORT Std_IsBelowMax(float fltMaxVal, float fltVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);
bool STD_UTILS_PORT Std_IsBelowMax(double dblMaxVal, double dblVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);

bool STD_UTILS_PORT Std_IsAboveMin(int iMinVal, int iVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);
bool STD_UTILS_PORT Std_IsAboveMin(long lMinVal, long lVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);
bool STD_UTILS_PORT Std_IsAboveMin(float fltMinVal, float fltVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);
bool STD_UTILS_PORT Std_IsAboveMin(double dblMinVal, double dblVal, bool bThrowError = true, string strParamName = "", bool bInclusiveLimit = false);

string STD_UTILS_PORT Std_CheckString(string strVal);
bool STD_UTILS_PORT Std_IsBlank(string strVal);


//***************************************************************************************************************
// Byte Array Functions

long STD_UTILS_PORT Std_GetBitSize(CStdArray<unsigned char> &aryRawData);

string STD_UTILS_PORT Std_ByteArrayToHexString(CStdArray<unsigned char> &aryBytes);
void STD_UTILS_PORT Std_ByteArrayToHexString(CStdArray<unsigned char> &aryBytes, string &strHex); 
void STD_UTILS_PORT Std_HexStringToByteArray(string strHex, CStdArray<unsigned char> &aryBytes);
unsigned char STD_UTILS_PORT *Std_HexStringToByteArray(string strHex, long &lArraySize);
unsigned char STD_UTILS_PORT Std_HexToByte(string strVal);
unsigned char STD_UTILS_PORT Std_HexCharToByte(char cVal);

void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned char &cOut);
void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned short &iOut);
void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned long &lOut);
void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, CStdArray<unsigned char> &aryOutData);
unsigned char STD_UTILS_PORT Std_GetByteFromArray(CStdArray<unsigned char> &aryRawData, long lEndBit, long lBitsNeeded);
bool STD_UTILS_PORT Std_GetBitFromArray(CStdArray<unsigned char> &aryRawData, long lBit);


void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned char cIn);
void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned short iIn);
void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned long lIn);
void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, CStdArray<unsigned char> &aryInData);
void STD_UTILS_PORT Std_SetByteInArray(CStdArray<unsigned char> &aryRawData, unsigned char cIn, long lEndBit, long lBitsNeeded);
void STD_UTILS_PORT Std_SetBitInArray(CStdArray<unsigned char> &aryRawData, long lBit, bool bVal);
void STD_UTILS_PORT Std_SetBitRangeInArray(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, bool bVal);
void STD_UTILS_PORT Std_FlipBitInArray(CStdArray<unsigned char> &aryRawData, long lBit);
void STD_UTILS_PORT Std_FlipBitRangeInArray(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength);

void STD_UTILS_PORT Std_CopyBinarySection(CStdArray<unsigned char> &aryInData, long lStartInBit,  
 																		  CStdArray<unsigned char> &aryOutData, long lStartOutBit,
																			long lBitLength);

bool STD_UTILS_PORT Std_ReadBinaryData(CStdArray<unsigned char> &aryBinaryData, long lLastBit, long &lStartBit, long lBitLength, unsigned char &cOut);
bool STD_UTILS_PORT Std_ReadBinaryData(CStdArray<unsigned char> &aryBinaryData, long lLastBit, long &lStartBit, long lBitLength, unsigned short &iOut);
bool STD_UTILS_PORT Std_ReadBinaryData(CStdArray<unsigned char> &aryBinaryData, long lLastBit, long &lStartBit, long lBitLength, unsigned long &lOut);

void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned char cIn);
void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned short iIn);
void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned long lIn);

unsigned char STD_UTILS_PORT Std_BinaryToGreyCode(unsigned char cVal);
unsigned short STD_UTILS_PORT Std_BinaryToGreyCode(unsigned short iVal);
unsigned long STD_UTILS_PORT Std_BinaryToGreyCode(unsigned long lVal);

unsigned char STD_UTILS_PORT Std_GreyCodeToBinary(unsigned char cVal);
unsigned short STD_UTILS_PORT Std_GreyCodeToBinary(unsigned short iVal);
unsigned long STD_UTILS_PORT Std_GreyCodeToBinary(unsigned long lVal);

// Byte Array Functions
//***************************************************************************************************************

//***************************************************************************************************************
// Logging Functions

int STD_UTILS_PORT Std_GetTraceLevel();
void STD_UTILS_PORT Std_SetTraceLevel(const int iVal);
void STD_UTILS_PORT Std_SetLogLevel(const int iLevel);
void STD_UTILS_PORT Std_LogMsg(const int iLevel, string strMessage, string strSourceFile = "", int iSourceLine = -1, bool bPrintHeader = true);
void STD_UTILS_PORT Std_TraceMsg(const int iLevel, string strMessage, string strSourceFile = "", int iSourceLine = -1, bool bLogToFile = true, bool bPrintHeader = true);
void STD_UTILS_PORT Std_ResetLog();

#ifndef _WIN32_WCE
	void STD_UTILS_PORT Std_SetLogFilePrefix(string strFilePrefix);
	string STD_UTILS_PORT Std_GetLogFilePrefix();
	void STD_UTILS_PORT Std_Log(const int nLevel, bool bPrintHeader, LPCTSTR strFormat, ...);
#endif

#define LOG_MSG(l, m) Std_LogMsg(l, m, __FILE__, __LINE__)
#define LOG_ERROR(m) Std_LogMsg(StdLogError, m, __FILE__, __LINE__)

//Turn on logging depending on the detail desired.
#ifdef STD_TRACING_ON
	#ifdef STD_TRACE_DETAIL
		#ifdef _DEBUG
			#define TRACE_DEBUG_NS(m) Std_TraceMsg(StdLogDebug, m, "", -1, STD_TRACE_TO_FILE, false)
			#define TRACE_DEBUG(m) Std_TraceMsg(StdLogDebug, m, __FILE__, __LINE__, STD_TRACE_TO_FILE, true)
			#define TRACE_INFO(m) Std_TraceMsg(StdLogInfo, m, __FILE__, __LINE__, STD_TRACE_TO_FILE, true)
			#define TRACE_INFO_NS(m) Std_TraceMsg(StdLogInfo, m, "", -1, STD_TRACE_TO_FILE, false)
			#define TRACE_DETAIL(m) Std_TraceMsg(StdLogDetail, m, __FILE__, __LINE__, STD_TRACE_TO_FILE, true)
			#define TRACE_DETAIL_NS(m) Std_TraceMsg(StdLogDetail, m, "", -1, STD_TRACE_TO_FILE, false)
		#else
			#define TRACE_DEBUG_NS(m) Std_LogMsg(StdLogDebug, m, "", -1, false)
			#define TRACE_DEBUG(m) Std_LogMsg(StdLogDebug, m, __FILE__, __LINE__, true)
			#define TRACE_INFO(m) Std_LogMsg(StdLogInfo, m, __FILE__, __LINE__, true)
			#define TRACE_INFO_NS(m) Std_LogMsg(StdLogInfo, m, "", -1, false)
			#define TRACE_DETAIL(m) Std_LogMsg(StdLogDetail, m, __FILE__, __LINE__, true)
			#define TRACE_DETAIL_NS(m) Std_LogMsg(StdLogDetail, m, "", -1, false)
		#endif          
	#else
		#ifdef STD_TRACE_INFO
			#ifdef _DEBUG
				#define TRACE_DEBUG_NS(m) Std_TraceMsg(StdLogDebug, m, "", -1, STD_TRACE_TO_FILE, false)
				#define TRACE_DEBUG(m) Std_TraceMsg(StdLogDebug, m, __FILE__, __LINE__, STD_TRACE_TO_FILE, true)
				#define TRACE_INFO(m) Std_TraceMsg(StdLogInfo, m, __FILE__, __LINE__, STD_TRACE_TO_FILE, true)
				#define TRACE_INFO_NS(m) Std_TraceMsg(StdLogInfo, m, "", -1, STD_TRACE_TO_FILE, false)
				#define TRACE_DETAIL(m)
				#define TRACE_DETAIL_NS(m)
			#else
				#define TRACE_DEBUG_NS(m) Std_LogMsg(StdLogDebug, m, "", -1, false)
				#define TRACE_DEBUG(m) Std_LogMsg(StdLogDebug, m, __FILE__, __LINE__, true)
				#define TRACE_INFO(m) Std_LogMsg(StdLogInfo, m, __FILE__, __LINE__, true)
				#define TRACE_INFO_NS(m) Std_LogMsg(StdLogInfo, m, "", -1, false)
				#define TRACE_DETAIL(m)
				#define TRACE_DETAIL_NS(m)
			#endif          
		#else
			#ifdef STD_TRACE_DEBUG
				#ifdef _DEBUG
					#define TRACE_DEBUG_NS(m) Std_TraceMsg(StdLogDebug, m, "", -1, STD_TRACE_TO_FILE, false)
					#define TRACE_DEBUG(m) Std_TraceMsg(StdLogDebug, m, __FILE__, __LINE__, STD_TRACE_TO_FILE, true)
					#define TRACE_INFO(m)
					#define TRACE_INFO_NS(m)
					#define TRACE_DETAIL(m)
					#define TRACE_DETAIL_NS(m)
				#else
					#define TRACE_DEBUG_NS(m) Std_LogMsg(StdLogDebug, m, "", -1, false)
					#define TRACE_DEBUG(m) Std_LogMsg(StdLogDebug, m, __FILE__, __LINE__, true)
					#define TRACE_INFO(m)
					#define TRACE_INFO_NS(m)
					#define TRACE_DETAIL(m)
					#define TRACE_DETAIL_NS(m)
				#endif          
			#else
				#define TRACE_DEBUG_NS(m)
				#define TRACE_DEBUG(m)
				#define TRACE_INFO(m)
				#define TRACE_INFO_NS(m)
				#define TRACE_DETAIL(m)
				#define TRACE_DETAIL_NS(m)
			#endif          
		#endif          
	#endif          
#else
	#define TRACE_DEBUG_NS(m)
	#define TRACE_DEBUG(m)
	#define TRACE_INFO(m)
	#define TRACE_INFO_NS(m)
	#define TRACE_DETAIL(m)
	#define TRACE_DETAIL_NS(m)
#endif          


#ifdef STD_LOG_DB_ON
	#define LOG_DB(m) Std_LogDBMsg(m, STD_LOG_DB_DSN, STD_LOG_DB_USER, STD_LOG_DB_PSWD)
#else
	#define LOG_DB(m)
#endif          

// Logging Functions
//***************************************************************************************************************


//***************************************************************************************************************
// Timing Functions

unsigned long STD_UTILS_PORT Std_GetTick();

// Timing Functions
//***************************************************************************************************************


//***************************************************************************************************************
// File Functions

bool STD_UTILS_PORT Std_IsFullPath(string strPath);
void STD_UTILS_PORT Std_SplitPathAndFile(string strFullPath, string &strPath, string &strFile);
string STD_UTILS_PORT Std_FileExtension(string &strFile);
BOOL STD_UTILS_PORT Std_DirectoryExists(string strPath);

// File Functions
//***************************************************************************************************************

//***************************************************************************************************************
// Vector Functions

typedef float StdVector4[4];
typedef float StdVector3[3];

#define V3_DIST(a, b) sqrt( pow(a[0]-b[0], 2) +  pow(a[1]-b[1], 2) +  pow(a[2]-b[2], 2) )
#define V4_DIST(a, b) sqrt( pow(a[0]-b[0], 2) +  pow(a[1]-b[1], 2) +  pow(a[2]-b[2], 2) +  pow(a[3]-b[3], 2) )

#define V3_MAG(a) sqrt( pow(a[0], 2) +  pow(a[1], 2) +  pow(a[2], 2) )
#define V4_MAG(a) sqrt( pow(a[0], 2) +  pow(a[1], 2) +  pow(a[2], 2) +  pow(a[3], 2) )

// Vector Functions
//***************************************************************************************************************

}				//StdUtils
