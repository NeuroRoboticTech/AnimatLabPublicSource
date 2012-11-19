/**
\file	StdUtilFunctions.cpp

\brief	Implements the standard utility functions class.
**/

#include "stdafx.h"

namespace StdUtils
{

/////////////////////////////////////
//Error functions

/*
void STD_UTILS_PORT RelayError(CStdErrorInfo oInfo)
{throw oInfo;}


void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strValueName, long lVal)
{
	ostringstream oStream;

	oStream << "   " << strValueName << ": " << lVal; 

	ThrowError(lError, strSource, strError, oStream.str());
}

void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strValueName, double dblVal)
{
	ostringstream oStream;

	oStream << "   " << strValueName << ": " << dblVal; 

	ThrowError(lError, strSource, strError, oStream.str());
}

void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strValueName, string strVal)
{
	strVal = strError + "  " + strValueName + ": " + strVal;

	ThrowError(lError, strSource, strVal);
}

void STD_UTILS_PORT ThrowError(long lError, string strSource, string strError, string strAddToError)
{
	CStdErrorInfo oInfo;

	oInfo.m_lError = lError;
	oInfo.m_strSourceFile = strSource;
	oInfo.m_strError = strError + strAddToError;

	Std_TraceMsg(StdLogError, oInfo.m_strError, oInfo.m_strSourceFile, oInfo.m_lSourceLine, true, true);

	throw oInfo;
}
*/

int g_iAppID = 1000;

/**
\brief	Gets the standard create application identifier.

\author	dcofer
\date	5/3/2011

\return	ID.
**/
string STD_UTILS_PORT Std_CreateAppID()
{
	//Need to make this threadsafe at somet point.
	
	g_iAppID++;
	return STR(g_iAppID);
}

/**
\brief	Adds to the error call chain. 

\author	dcofer
\date	5/3/2011

\param [in,out]	oInfo	The error information. 
\param	strSourceFile	The string source file. 
\param	lSourceLine  	Source line. 
**/
void AddToErrorCallChain(CStdErrorInfo &oInfo, string strSourceFile, long lSourceLine)
{
	if(lSourceLine > 0)
	{
		ostringstream oStream;
		oStream << strSourceFile << " (" << lSourceLine << ")"; 
		oInfo.m_aryCallChain.Add(oStream.str());
	}
}

/**
\brief	Standard relay error.

\author	dcofer
\date	5/3/2011

\exception	oInfo	Thrown when information. 

\param	oInfo		 	The information. 
\param	strSourceFile	The string source file. 
\param	lSourceLine  	Source line. 

\return	.
**/
void STD_UTILS_PORT Std_RelayError(CStdErrorInfo oInfo, string strSourceFile, long lSourceLine)
{
	AddToErrorCallChain(oInfo, strSourceFile, lSourceLine);
	throw oInfo;
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strValueName 	Name of the string value. 
\param	iVal		 	The value. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, unsigned char iVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << (int) iVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strValueName 	Name of the string value. 
\param	iVal		 	The value. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, unsigned short iVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << iVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strValueName 	Name of the string value. 
\param	iVal		 	The value. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, int iVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << iVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strValueName 	Name of the string value. 
\param	lVal		 	The value. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, long lVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << lVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strValueName 	Name of the string value. 
\param	fltVal		 	The flt value. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, float fltVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << fltVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strValueName 	Name of the string value. 
\param	dblVal		 	The double value. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, double dblVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << dblVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strValueName 	Name of the string value. 
\param	strVal		 	The string value. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, string strVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << strVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

/**
\brief	Standard throw error.

\author	dcofer
\date	5/3/2011

\exception	oInfo	Thrown when information. 

\param	lError		 	The error number. 
\param	strError	 	The string error message. 
\param	strSourceFile	The string source filename. 
\param	lSourceLine  	Source line number. 
\param	strText		 	The string text. 

\return	.
**/
void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strText)
{
	CStdErrorInfo oInfo;

	oInfo.m_lError = lError;
	oInfo.m_strError = strError + strText;
	oInfo.m_strSourceFile = strSourceFile;
	oInfo.m_lSourceLine = lSourceLine;

	AddToErrorCallChain(oInfo, strSourceFile, lSourceLine);
	
	Std_TraceMsg(StdLogError, oInfo.m_strError, oInfo.m_strSourceFile, oInfo.m_lSourceLine, true, true);

	throw oInfo;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(string strVal)
{return strVal;}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(const char *strVal)
{
	string strV = strVal;
	return strV;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	iVal	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(unsigned char iVal)
{
	std::ostringstream buf; 
	buf << iVal ;
	std::string str = buf.str() ;
	return str;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	iVal	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(unsigned short iVal)
{
	std::ostringstream buf; 
	buf << iVal ;
	std::string str = buf.str() ;
	return str;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	iVal	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(int iVal)
{
	std::ostringstream buf; 
	buf << iVal ;
	std::string str = buf.str() ;
	return str;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	lVal	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(long lVal)
{
	std::ostringstream buf; 
	buf << lVal ;
	std::string str = buf.str() ;
	return str;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	fltVal	The flt value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(float fltVal)
{
	std::ostringstream buf; 
	buf << fltVal ;
	std::string str = buf.str() ;
	return str;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	dblVal	The double value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(double dblVal)
{
	std::ostringstream buf; 
	buf << dblVal ;
	std::string str = buf.str() ;
	return str;
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	bVal	true to value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(bool bVal)
{	
	if(bVal)
		return "True";
	else
		return "False";
}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strFormat	The string format. 
\param	iVal	 	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(string strFormat, unsigned char iVal)
{return Std_Format(strFormat.c_str(), iVal);}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strFormat	The string format. 
\param	iVal	 	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(string strFormat, unsigned short iVal)
{return Std_Format(strFormat.c_str(), iVal);}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strFormat	The string format. 
\param	iVal	 	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(string strFormat, int iVal)
{return Std_Format(strFormat.c_str(), iVal);}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strFormat	The string format. 
\param	lVal	 	The value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(string strFormat, long lVal)
{return Std_Format(strFormat.c_str(), lVal);}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strFormat	The string format. 
\param	fltVal   	The flt value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(string strFormat, float fltVal)
{return Std_Format(strFormat.c_str(), fltVal);}

/**
\brief	Converts a value to a string

\author	dcofer
\date	5/3/2011

\param	strFormat	The string format. 
\param	dblVal   	The double value. 

\return	.
**/
string STD_UTILS_PORT Std_ToStr(string strFormat, double dblVal)
{return Std_Format(strFormat.c_str(), dblVal);}

/**
\brief	Converts a value toa  bool.

\author	dcofer
\date	5/3/2011

\param	iVal	The value. 

\return	.
**/
bool STD_UTILS_PORT Std_ToBool(int iVal)
{
	if(iVal)
		return true;
	else
		return false;
}

/**
\brief	Converts a value toa  bool.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
bool STD_UTILS_PORT Std_ToBool(string strVal)
{
	string strV = Std_ToUpper(Std_Trim(strVal));

	if(strV == "TRUE" || strV == "1")
		return true;
	else
		return false;
}

/**
\brief	If string is blank it uses NULL instead.

\author	dcofer
\date	5/3/2011

\param	strFormat	The string format. 

\return	.
**/
string STD_UTILS_PORT Std_NullStr(string strFormat)
{
	if(strFormat.length())
		return ("'" + strFormat + "'");
	else
		return "NULL";
}

/**
\brief	converts LPCWSTR to ansi

\author	dcofer
\date	5/3/2011

\param	strData	Information describing the string. 

\return	.
**/
string STD_UTILS_PORT Std_ConvertToANSI(LPCWSTR strData)
{
	// len will hold the required length of converted string
	int len = WideCharToMultiByte(CP_ACP, 0, strData, -1, 0, 0, 0, 0);
	// allocate the buffer to hold the converted string
	LPSTR result = new char[len];
	// do the conversion
	WideCharToMultiByte(CP_ACP, 0, strData, -1, result, len, 0, 0);

	string strVal = result;
	if(result) 
		delete[] result;

	return strVal;
}

/**
\brief	converts ansi string to LPWSTR.

\author	dcofer
\date	5/3/2011

\param	strData	Information describing the string. 

\return	.
**/
LPWSTR STD_UTILS_PORT Std_ConvertFromANSI(string strData)
{
	// len will hold the required length of converted string
	int len = MultiByteToWideChar(CP_ACP, 0, strData.c_str(), -1, 0, 0);
	// allocate the buffer to hold the converted string
	wchar_t *result = new wchar_t[len];
	// do the conversion
	MultiByteToWideChar(CP_ACP, 0, strData.c_str(), -1, result, len);

	return result;
}

/**
\brief	Standard variant type to constant.

\author	dcofer
\date	5/3/2011

\param	strType	Type of the string. 

\return	.
**/
int STD_UTILS_PORT Std_VariantTypeToConst(string strType)
{

	strType = Std_Trim(Std_ToUpper(strType));

	if(strType == "EMPTY")
		return StdVtEmpty;
	else if(strType == "SHORT")
		return StdVtShort;
	else if(strType == "LONG")
		return StdVtLong;
	else if(strType == "FLOAT")
		return StdVtFloat;
	else if(strType == "DOUBLE")
		return StdVtDouble;
	else if(strType == "BOOL")
		return StdVtBool;
	else if(strType == "CHAR")
		return StdVtChar;
	else if(strType == "UCHAR")
		return StdVtUChar;
	else if(strType == "USHORT")
		return StdVtUShort;
	else if(strType == "ULONG")
		return StdVtULong;
	else if(strType == "INT")
		return StdVtInt;
	else if(strType == "UINT")
		return StdVtUInt;
	else if(strType == "STRING")
		return StdVtString;
	else
		THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", strType);

	return -1;
}

/**
\brief	Standard constant to variant type.

\author	dcofer
\date	5/3/2011

\param	iType	The type. 

\return	.
**/
string STD_UTILS_PORT Std_ConstToVariantType(int iType)
{
	switch(iType)
	{
		case StdVtEmpty:
			return "Empty";
		case StdVtShort:
			return "Short";
		case StdVtLong:
			return "Long";
		case StdVtFloat:
			return "Float";
		case StdVtBool:
			return "Bool";
		case StdVtChar:
			return "Char";
		case StdVtUChar:
			return "UChar";
		case StdVtUShort:
			return "UShort";
		case StdVtULong:
			return "ULong";
		case StdVtInt:
			return "Int";
		case StdVtUInt:
			return "UInt";
		case StdVtString:
			return "String";

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", iType);
	}
	
	return "";
}

/**
\brief	Splits a string into an array of subparts based on a delimiter.

\author	dcofer
\date	5/3/2011

\param	input		   	The input. 
\param	delimiter	   	The delimiter. 
\param [in,out]	results	The results. 

\return	.
**/
int STD_UTILS_PORT  Std_Split(const string& input, const string& delimiter, CStdArray<string> &results)
{
	int iPos = 0;
	int newPos = -1;
	int sizeS2 = delimiter.size();
	int isize = input.size();

	vector<int> positions;
	results.RemoveAll();

	newPos = input.find (delimiter, 0);

	if( newPos < 0 ) 
	{ 
		string strInput = input;

		if(!Std_IsBlank(input))
		{
			strInput = Std_Trim(strInput);
			results.push_back(strInput);
			return 1;
		}
		else
			return 0; 
	}

	int numFound = 0;

	while( newPos > iPos )
	{
		numFound++;
		positions.push_back(newPos);
		iPos = newPos;
		newPos = input.find (delimiter, iPos+sizeS2+1);
	}

	for( int i=0; i <= positions.size(); i++ )
	{
		string s;
		if( i == 0 ) 
			{s = input.substr( i, positions[i] ); }
		else
		{
			int offset = positions[i-1] + sizeS2;
			if( offset < isize )
			{
				if( i == positions.size() )
				{
					s = input.substr(offset);
				}
				else if( i > 0 )
				{
					s = input.substr( positions[i-1] + sizeS2, positions[i] - positions[i-1] - sizeS2 );
				}
			}
		}

		if( s.size() > 0 )
		{
			results.push_back(s);
		}
	}
	return results.GetSize();
}

/**
\brief	Combines an array of strings into a single string.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryParts	The array of string parts. 
\param	strDelimiter		The string delimiter. 

\return	.
**/
string STD_UTILS_PORT Std_Combine(CStdArray<string> &aryParts, string strDelimiter)
{
	string strCombined;
	int iCount = aryParts.GetSize();
	for(int iPart=0; iPart<iCount; iPart++)
	{
		strCombined += aryParts[iPart];
		if(iPart<iCount-1) strCombined += strDelimiter;
	}

	return strCombined;
}

/**
\brief	Trims a string

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
string STD_UTILS_PORT Std_Trim(string strVal)
{
	return Std_TrimLeft(Std_TrimRight(strVal));
}

/**
\brief	Test if this is a space.

\author	dcofer
\date	5/3/2011

\param	c	The character. 

\return	true if it succeeds, false if it fails.
**/
bool NotSpace(char c)
{return !std::isspace(c);}

/**
\brief	Standard trim left.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
string STD_UTILS_PORT Std_TrimLeft(string strVal)
{
	strVal.erase(strVal.begin(), std::find_if(strVal.begin(), strVal.end(), NotSpace));
	return strVal;
}

/**
\brief	Standard trim right.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
string STD_UTILS_PORT Std_TrimRight(string strVal)
{
	// NOTE:  When comparing reverse_iterators here (MYRITER), I avoid using
	// operator!=.  This is because namespace rel_ops also has a template
	// operator!= which conflicts with the global operator!= already defined
	// for reverse_iterator in the header <utility>.
	// Thanks to John James for alerting me to this.

	string::reverse_iterator it = std::find_if(strVal.rbegin(), strVal.rend(), NotSpace);
	if ( !(strVal.rend() == it) )
		strVal.erase(strVal.rend() - it);

	strVal.erase(!(it == strVal.rend()) ? strVal.find_last_of(*it) + 1 : 0);
	return strVal;
}

/**
\brief	Tests if this string is a number.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
BOOL STD_UTILS_PORT Std_IsNumeric(string strVal)
{
	int iPos = strVal.find_first_not_of("0123456789.+-eE");
	if(iPos == -1)
		return true;
	else
		return false;
}

/**
\brief	Tests if this string is an integer.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
BOOL STD_UTILS_PORT Std_IsIntegerType(string strVal)
{
	int iPos = strVal.find_first_not_of("0123456789+-");
	if(iPos == -1)
		return true;
	else
		return false;
}

/**
\brief	Gets the left portion of a substring.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 
\param	iCount	Number of characters to the left to get. 

\return	.
**/
string STD_UTILS_PORT Std_Left(string strVal, int iCount)
{
	iCount = SSMAX(0, SSMIN(iCount, static_cast<int>(strVal.length())));
	return strVal.substr(0, iCount); 
}

/**
\brief	Gets the right portion of a substring.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 
\param	iCount	Number of characters to the right to get. 

\return	.
**/
string STD_UTILS_PORT Std_Right(string strVal, int iCount)
{
	iCount = SSMAX(0, SSMIN(iCount, static_cast<int>(strVal.length())));
	return strVal.substr(strVal.length()-iCount);
}

/**
\brief	Converts a string to upper case.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
string STD_UTILS_PORT Std_ToUpper(string strVal)
{
	std::transform(strVal.begin(), strVal.end(), strVal.begin(), (int(*)(int))std::toupper);
	return strVal;
}

/**
\brief	Converts a string to lower case.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	.
**/
string STD_UTILS_PORT Std_ToLower(string strVal)
{
	std::transform(strVal.begin(), strVal.end(), strVal.begin(), (int(*)(int))std::tolower);
	return strVal;
}

/**
\brief	Replaces a substring with another string.

\author	dcofer
\date	5/3/2011

\param	strVal	  	The string to test. 
\param	strFind   	The string to find. 
\param	strReplace	The string to replace it with. 

\return	new string.
**/
string STD_UTILS_PORT Std_Replace(string strVal, string strFind, string strReplace)
{
	int i = strVal.find(strFind, 0);
	while(i != string::npos)
	{
		strVal.replace(i, strFind.size(), strReplace);
		i = strVal.find(strFind, 0);
	}
	return strVal;
}

/**
\brief	Formats a string

\author	dcofer
\date	5/3/2011

\param	szFormat	The size format. 

\return	.
**/
string STD_UTILS_PORT Std_Format(const char* szFormat,...)
{
	std::vector<CHAR> _buffer(8096);
	va_list argList;
	va_start(argList,szFormat);
	int ret = _vsnprintf(&_buffer[0],8096,szFormat,argList);

	va_end(argList);
	string strVal;

	strVal.assign(&_buffer[0],ret);
	return strVal;
}

/**
\brief	Generates a long representation of a color

\author	dcofer
\date	5/3/2011

\param	iRed  	The red. 
\param	iGreen	The green. 
\param	iBlue 	The blue. 

\return	.
**/
long STD_UTILS_PORT Std_RGB(unsigned char iRed, unsigned char iGreen, unsigned char iBlue)
{
	return ((iBlue << 16) | (iGreen << 8) | (iRed));
}

/**
\brief	Loads a color

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml	The xml to load. 
\param	strParamName	Name of the xml parameter. 
\param	bThrowError 	true to throw error if there is a problem. 
\param	lDefault		The default color value. 

\return	.
**/
long STD_UTILS_PORT Std_LoadRGB(CStdXml &oXml, string strParamName, bool bThrowError, long lDefault)
{
	long lVal=0;

	if(oXml.FindChildElement(strParamName, bThrowError))
	{
		int iRed = oXml.GetChildAttribInt("Red");
		Std_InValidRange((int) 0, (int) 255, iRed, true, (strParamName + "::Red"));

		int iGreen = oXml.GetChildAttribInt("Green");
		Std_InValidRange((int) 0, (int) 255, iGreen, true, (strParamName + "::Green"));

		int iBlue = oXml.GetChildAttribInt("Blue");
		Std_InValidRange((int) 0, (int) 255, iBlue, true, (strParamName + "::Blue"));
		
		lVal = Std_RGB(iRed, iGreen, iBlue);
	}
	else
		lVal = lDefault;

	return lVal;
}

/**
\brief	Loads a color

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml		The xml to load. 
\param	strParamName		Name of the xml parameter. 
\param [in,out]	aryColor	Color array. 
\param	bThrowError 	true to throw error if there is a problem. 

**/
void STD_UTILS_PORT Std_LoadColor(CStdXml &oXml, string strParamName, float *aryColor, bool bThrowError)
{
	if(oXml.FindChildElement(strParamName, bThrowError))
	{
		oXml.IntoChildElement(strParamName);
		aryColor[0] = oXml.GetAttribFloat("Red");
		aryColor[1] = oXml.GetAttribFloat("Green");
		aryColor[2] = oXml.GetAttribFloat("Blue");
		aryColor[3] = oXml.GetAttribFloat("Alpha");
		oXml.OutOfElem();
	}
	else if(bThrowError)
		THROW_TEXT_ERROR(Std_Err_lParamNotFound, Std_Err_strParamNotFound, strParamName);
	else
	{
		aryColor[0] = 1;
		aryColor[1] = 1;
		aryColor[2] = 1;
		aryColor[3] = 1;
	}

	Std_InValidRange((float) 0, (float) 1, aryColor[0], bThrowError, strParamName);
	Std_InValidRange((float) 0, (float) 1, aryColor[1], bThrowError, strParamName);
	Std_InValidRange((float) 0, (float) 1, aryColor[2], bThrowError, strParamName);
	Std_InValidRange((float) 0, (float) 1, aryColor[3], bThrowError, strParamName);
}

/**
\brief	Loads a color

\author	dcofer
\date	5/3/2011

\param	strXml				The string xml. 
\param	strParamName		Name of the string parameter. 
\param [in,out]	aryColor	If non-null, the ary color. 
\param	bThrowError			true to throw error. 

**/
void STD_UTILS_PORT Std_LoadColor(string strXml, string strParamName, float *aryColor, bool bThrowError)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement(strParamName);
	
	Std_LoadColor(oXml, strParamName, aryColor, bThrowError);
}

/**
\brief	Determines the sign of a number.

\author	dcofer
\date	5/4/2011

\param	fltVal	The value. 

\return	-1 if number is negative, 0 if it is 0, and 1 if it is positive.
**/
int STD_UTILS_PORT Std_Sign(float fltVal)
{
	if(fltVal<0) 
		return -1; 
	else if(fltVal==0)
		return 0;
	else
		return 1;
}

void init_genrand(unsigned long s);
unsigned long genrand_int32(void);
long genrand_int31(void);
double genrand_real1(void);
double genrand_real2(void);
double genrand_real3(void);
double genrand_res53(void);

/**
\brief	Sets the seed for generating a random number

\author	dcofer
\date	5/3/2011

\param	lSeed	The seed. 
**/
void STD_UTILS_PORT Std_SRand(unsigned long lSeed)
{init_genrand(lSeed);}

/**
\brief	Generates a random number between two values.

\author	dcofer
\date	5/3/2011

\param	low 	The low value for the random number. 
\param	high	The high value for the random number. 

\return	random number.
**/
int STD_UTILS_PORT Std_IRand( int low, int high )
{
	high++;
	return (int) (( abs(high-low) * genrand_real2() ) + low);
}


/**
\brief	Generates a random number between two values.

\author	dcofer
\date	5/3/2011

\param	low 	The low value for the random number. 
\param	high	The high value for the random number. 

\return	random number.
**/
long STD_UTILS_PORT Std_LRand( long low, long high )
{
	high++;
	return (long) (( abs(high-low) * genrand_real2() ) + low);
}


/**
\brief	Generates a random number between two values.

\author	dcofer
\date	5/3/2011

\param	low 	The low value for the random number. 
\param	high	The high value for the random number. 

\return	random number.
**/
float STD_UTILS_PORT Std_FRand( float low, float high )
{return (float) (( fabs(high-low) * genrand_real1() ) + low);}


/**
\brief	Generates a random number between two values.

\author	dcofer
\date	5/3/2011

\param	low 	The low value for the random number. 
\param	high	The high value for the random number. 

\return	random number.
**/
double STD_UTILS_PORT Std_DRand( double low, double high )
{return (( fabs(high-low) * genrand_real1() ) + low);}

/**
\brief	Tests whether a number is within a valid range.

\author	dcofer
\date	5/3/2011

\param	iMinVal			The minimum value. 
\param	iMaxVal			The maximum value. 
\param	iVal			The test value. 
\param	bThrowError 	true to throw error if outside of range. 
\param	strParamName	Name of the parameter. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_InValidRange(int iMinVal, int iMaxVal, int iVal, bool bThrowError, string strParamName)
{

	if( (iVal>=iMinVal) && (iVal<=iMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << iVal << "  Range [";
			oss << iMinVal << ", " << iMaxVal << "]";

			THROW_TEXT_ERROR(Std_Err_lValueOutOfRange, Std_Err_strValueOutOfRange, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Standard in valid range.

\author	dcofer
\date	5/3/2011

\param	lMinVal			The minimum value. 
\param	lMaxVal			The maximum value. 
\param	lVal			The test value. 
\param	bThrowError 	true to throw error if outside of range. 
\param	strParamName	Name of the parameter. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_InValidRange(long lMinVal, long lMaxVal, long lVal, bool bThrowError, string strParamName)
{

	if( (lVal>=lMinVal) && (lVal<=lMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << lVal << "  Range [";
			oss << lMinVal << ", " << lMaxVal << "]";

			THROW_TEXT_ERROR(Std_Err_lValueOutOfRange, Std_Err_strValueOutOfRange, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Standard in valid range.

\author	dcofer
\date	5/3/2011

\param	fltMinVal   	The minimum value. 
\param	fltMaxVal   	The maximum value. 
\param	fltVal			The test value. 
\param	bThrowError 	true to throw error if outside of range. 
\param	strParamName	Name of the parameter. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_InValidRange(float fltMinVal, float fltMaxVal, float fltVal, bool bThrowError, string strParamName)
{

	if( (fltVal>=fltMinVal) && (fltVal<=fltMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << fltVal << "  Range [";
			oss << fltMinVal << ", " << fltMaxVal << "]";

			THROW_TEXT_ERROR(Std_Err_lValueOutOfRange, Std_Err_strValueOutOfRange, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Standard in valid range.

\author	dcofer
\date	5/3/2011

\param	dblMinVal   	The double minimum value. 
\param	dblMaxVal   	The double maximum value. 
\param	dblVal			The test value. 
\param	bThrowError 	true to throw error if outside of range. 
\param	strParamName	Name of the parameter. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_InValidRange(double dblMinVal, double dblMaxVal, double dblVal, bool bThrowError, string strParamName)
{

	if( (dblVal>=dblMinVal) && (dblVal<=dblMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << dblVal << "  Range [";
			oss << dblMinVal << ", " << dblMaxVal << "]";

			THROW_TEXT_ERROR(Std_Err_lValueOutOfRange, Std_Err_strValueOutOfRange, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is below a maximum value.

\author	dcofer
\date	5/3/2011

\param	iMaxVal		   	The maximum value. 
\param	iVal		   	The test value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsBelowMax(int iMaxVal, int iVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && iVal<iMaxVal) || (bInclusiveLimit && iVal<=iMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << iVal << "  Maximum: " << iMaxVal;

			THROW_TEXT_ERROR(Std_Err_lAboveMaxValue, Std_Err_strAboveMaxValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is below a maximum value.

\author	dcofer
\date	5/3/2011

\param	lMaxVal		   	The maximum value. 
\param	lVal		   	The value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsBelowMax(long lMaxVal, long lVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && lVal<lMaxVal) || (bInclusiveLimit && lVal<=lMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << lVal << "  Maximum: " << lMaxVal;

			THROW_TEXT_ERROR(Std_Err_lAboveMaxValue, Std_Err_strAboveMaxValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is below a maximum value.

\author	dcofer
\date	5/3/2011

\param	fltMaxVal	   	The flt maximum value. 
\param	fltVal		   	The flt value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsBelowMax(float fltMaxVal, float fltVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && fltVal<fltMaxVal) || (bInclusiveLimit && fltVal<=fltMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << fltVal << "  Maximum: " << fltMaxVal;

			THROW_TEXT_ERROR(Std_Err_lAboveMaxValue, Std_Err_strAboveMaxValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is below a maximum value.

\author	dcofer
\date	5/3/2011

\param	dblMaxVal	   	The double maximum value. 
\param	dblVal		   	The double value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsBelowMax(double dblMaxVal, double dblVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && dblVal<dblMaxVal) || (bInclusiveLimit && dblVal<=dblMaxVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << dblVal << "  Maximum: " << dblMaxVal;

			THROW_TEXT_ERROR(Std_Err_lAboveMaxValue, Std_Err_strAboveMaxValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is above a minimum value.

\author	dcofer
\date	5/3/2011

\param	iMinVal		   	The minimum value. 
\param	iVal		   	The test value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsAboveMin(int iMinVal, int iVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && iVal>iMinVal) || (bInclusiveLimit && iVal>=iMinVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << iVal << "  Minimum: " << iMinVal;

			THROW_TEXT_ERROR(Std_Err_lBelowMinValue, Std_Err_strBelowMinValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is above a minimum value.

\author	dcofer
\date	5/3/2011

\param	lMinVal		   	The minimum value. 
\param	lVal		   	The test value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsAboveMin(long lMinVal, long lVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && lVal>lMinVal) || (bInclusiveLimit && lVal>=lMinVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << lVal << "  Minimum: " << lMinVal;

			THROW_TEXT_ERROR(Std_Err_lBelowMinValue, Std_Err_strBelowMinValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is above a minimum value.

\author	dcofer
\date	5/3/2011

\param	fltMinVal	   	The flt minimum value. 
\param	fltVal		   	The test value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsAboveMin(float fltMinVal, float fltVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && fltVal>fltMinVal) || (bInclusiveLimit && fltVal>=fltMinVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << fltVal << "  Minimum: " << fltMinVal;

			THROW_TEXT_ERROR(Std_Err_lBelowMinValue, Std_Err_strBelowMinValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Tests if a number is above a minimum value.

\author	dcofer
\date	5/3/2011

\param	dblMinVal	   	The double minimum value. 
\param	dblVal		   	The test value. 
\param	bThrowError	   	true to throw error if outside of range. 
\param	strParamName   	Name of the parameter. 
\param	bInclusiveLimit	true for inclusive limit. 

\return	true if valid, false else.
**/
bool STD_UTILS_PORT Std_IsAboveMin(double dblMinVal, double dblVal, bool bThrowError, string strParamName, bool bInclusiveLimit)
{

	if( (!bInclusiveLimit && dblVal>dblMinVal) || (bInclusiveLimit && dblVal>=dblMinVal) )
		return true;
	else
	{
		if(bThrowError)
		{
			std::ostringstream oss;
			
			if(!strParamName.empty())
				oss << "Paramater: " << strParamName;

			oss << "  Value: " << dblVal << "  Minimum: " << dblMinVal;

			THROW_TEXT_ERROR(Std_Err_lBelowMinValue, Std_Err_strBelowMinValue, oss.str().c_str());
		}

		return false;
	}

}

/**
\brief	Converts a string to upper case and trims it.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	new string.
**/
string STD_UTILS_PORT Std_CheckString(string strVal)
{
	string strTemp = strVal;
	strTemp = Std_ToUpper(Std_Trim(strTemp));
	return strTemp;
}

/**
\brief	Trims a string and tests if a string is blank.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	true if blank, false else.
**/
bool STD_UTILS_PORT Std_IsBlank(string strVal)
{
	string strTemp = Std_Trim(strVal);
	if(!strTemp.length())
		return true;
	else
		return false;
}


string STD_UTILS_PORT Std_RetrieveParam(int argc, const char **argv, string strParamName, bool bThrowError)
{
	int iParam=0;
	bool bRetrieved=false, bFound = false;
	string strParam="", strFound ="";
	while(!bRetrieved && iParam<argc)
	{
		strParam = Std_ToUpper(Std_Trim(argv[iParam]));

		if(bFound)
		{
			strFound = strParam;
			bRetrieved = true;
		}

		if(strParam == strParamName)
			bFound = true;

		iParam++;
	}

	if(bThrowError && !bRetrieved)
		THROW_PARAM_ERROR(Std_Err_lNoProjectParamOnCommandLine, Std_Err_strNoProjectParamOnCommandLine, "Param", strParamName);

	return strFound;
}

//***************************************************************************************************************
// Byte Array Functions

//  bit_get (x,i) is the value of bit i of x

/**
\brief	Gets a bit of x.

\author	dcofer
\date	5/3/2011

\param	x	The x coordinate. 
\param	i	The bit index. 

\return	.
**/
unsigned long bit_get (unsigned long x, int i) 
{return ( x >> i) & 1;}


//  bit_put (x, i, v) returns the same as x except that bit i is 
//                    set to the same value as bit 0 of v.

/**
\brief	puts a bit in x.

\author	dcofer
\date	5/3/2011

\param	x	The x coordinate. 
\param	i	The bit index. 
\param	v	The new bit. 

\return	.
**/
unsigned long bit_put (unsigned long x, int i, unsigned long v) 
{
	if ( v & 1 ) {
		return x | (1 << i);
	} else {
		return x & ~(1 << i);
	}
}

/**
\brief	Converts a byte array to hexadecimal string.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryBytes	The array in bytes. 

\return	hex string.
**/
string STD_UTILS_PORT Std_ByteArrayToHexString(CStdArray<unsigned char> &aryBytes) 
{
	string strHex;
	Std_ByteArrayToHexString(aryBytes, strHex);
	return strHex;
}

/**
\brief	Converts a byte array to hexadecimal string.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryBytes	The array in bytes. 
\param [in,out]	strHex  	The string hexadecimal. 

**/
void STD_UTILS_PORT Std_ByteArrayToHexString(CStdArray<unsigned char> &aryBytes, string &strHex) 
{
	string strByte;
	int iByte, iByteLength;

	strHex = "";
	iByteLength = aryBytes.GetSize();

	for(iByte=0; iByte<iByteLength; iByte++)
	{
		strByte = Std_Format("%02X", aryBytes[iByte]);
		strHex += strByte;
	}

}

/**
\brief	Converts a hex string to a byte array

\author	dcofer
\date	5/3/2011

\param	strHex				The hexadecimal string. 
\param [in,out]	aryBytes	The array of bytes. 
**/
void STD_UTILS_PORT Std_HexStringToByteArray(string strHex, CStdArray<unsigned char> &aryBytes) 
{
	int iLength, iByteSize, iNibble0, iNibble1, iByte;
	unsigned char cVal;

	aryBytes.RemoveAll();

	iLength = strHex.length();
	iByteSize = iLength / 2;
	aryBytes.SetSize(iByteSize);

	for(iByte=0; iByte<iByteSize; iByte++)
	{
		iNibble0 = iByte*2;
		iNibble1 = (iByte*2) + 1;

		cVal = (Std_HexCharToByte(strHex[iNibble0]) << 4) + Std_HexCharToByte(strHex[iNibble1]);
		aryBytes[iByte] = cVal;
	}
}

/**
\brief	Converts a hex string to a byte array

\author	dcofer
\date	5/3/2011

\param	strHex			  	The hexadecimal string. 
\param [in,out]	lArraySize	Size of the array. 

\return	Byte array
**/
unsigned char STD_UTILS_PORT *Std_HexStringToByteArray(string strHex, long &lArraySize) 
{
	int iLength, iByteSize, iNibble0, iNibble1, iByte;
	unsigned char cVal;
	unsigned char *lpBytes = NULL;

	iLength = strHex.length();
	iByteSize = iLength / 2;

	lpBytes = new unsigned char[iByteSize];

	for(iByte=0; iByte<iByteSize; iByte++)
	{
		iNibble0 = iByte*2;
		iNibble1 = (iByte*2) + 1;

		cVal = (Std_HexCharToByte(strHex[iNibble0]) << 4) + Std_HexCharToByte(strHex[iNibble1]);
		lpBytes[iByte] = cVal;
	}

	return lpBytes;
}

/**
\brief	Converts a hexidecimal number to a byte.

\author	dcofer
\date	5/3/2011

\param	strVal	The string value. 

\return	byte.
**/
unsigned char STD_UTILS_PORT Std_HexToByte(string strVal) 
{
	unsigned char cVal;

	if(!(strVal.length() == 2 || (strVal.length() == 4 && Std_ToUpper(Std_Left(strVal, 2)) == "0X")))
		THROW_PARAM_ERROR(Std_Err_lInvalidHexChar, Std_Err_strInvalidHexChar, "HexValue", strVal);

	if(strVal.length() == 4 && Std_ToUpper(Std_Left(strVal, 2)) == "0X")
		strVal = Std_Right(strVal, 2);

	cVal = (Std_HexCharToByte(strVal[1]) << 4) + Std_HexCharToByte(strVal[0]);

	return cVal;
}

/**
\brief	Converts a hexidecimal number to a byte.

\author	dcofer
\date	5/3/2011

\param	cVal	The value. 

\return	byte.
**/
unsigned char STD_UTILS_PORT Std_HexCharToByte(char cVal) 
{
	if (cVal >= '0' && cVal <= '9')
		return (cVal - '0');
	
	if (cVal >= 'A' && cVal <= 'F')
		return ((cVal - 'A') + 0x0A);

	//if we get here then it is an invalid character sequence.
	string strVal;
	strVal = cVal;
	THROW_PARAM_ERROR(Std_Err_lInvalidHexChar, Std_Err_strInvalidHexChar, "HexValue", strVal);
	return 0;
}




/**
\brief	Gets the number of bits in a byte array.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryRawData	Raw byte array. 

\return	number of bits.
**/
long STD_UTILS_PORT Std_GetBitSize(CStdArray<unsigned char> &aryRawData)
{return (8*aryRawData.GetSize());}

/**
\brief	Gets a section of bits from a byte array.

\details GetBinarySection Documentation
Assume we have a byte array of ByteArray = "5A05AA58607AEA3A9F69F97A1B17139A8AC0735C" of hex values.
The GetBinarySection functions retrieve sections of that byte array on BIT boundaries and not on byte
boundaries. In other words you can tell it to get 10 bits starting at bit position 19 and return it
to me as a 2 byte short. It works on the basic assumption that the right hand side of the binary data
is the least significant bit. However, when you tell it the bit position where you want to start 
reading from the most signfigant bit is numbered 0. So lets setup some examples to show what it will
return. The binray data shown below is the 5AA58 section, the first bit shown is number 12 and the
last bit is 31.
        12      19             31
        |       |              |
  ....  0101 1010 1010 0101 1000 ....
        5    A    A    5    8
GetBinarySection(m_aryBinaryData, 12, 16, iOut);  iOut = 0x5AA5
GetBinarySection(m_aryBinaryData, 19, 6, cOut);   cOut = 0x14
GetBinarySection(m_aryBinaryData, 19, 10, iOut);  iOut = 0x014B   0001 0100 1011
GetBinarySection(m_aryBinaryData, 19, 11, iOut);  iOut = 0x0296   0010 1001 0110
when it starts filling in the returned value it starts at the end of the section it is retrieving 
and fills it in backwards so that the least signifigant value in the section is the least significant
bit in the value. If the section is too small to completely fill the bytes of the variable then the 
remaining bits are set to zero.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lStartBit		  	The start bit of the section we will retrieve. 
\param	lBitLength		  	Bit length of the section to retrieve. 
\param [in,out]	cOut	  	The output section. 

**/
void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned char &cOut)
{
	CStdArray<unsigned char> aryOutData;

	Std_InValidRange((long) 1, (long) 8, lBitLength, true, "Length");

	Std_GetBinarySection(aryRawData, lStartBit, lBitLength, aryOutData);

	cOut = aryOutData[aryOutData.GetSize()-1];
}

/**
\brief	Gets a section of bits from a byte array.

\details GetBinarySection Documentation
Assume we have a byte array of ByteArray = "5A05AA58607AEA3A9F69F97A1B17139A8AC0735C" of hex values.
The GetBinarySection functions retrieve sections of that byte array on BIT boundaries and not on byte
boundaries. In other words you can tell it to get 10 bits starting at bit position 19 and return it
to me as a 2 byte short. It works on the basic assumption that the right hand side of the binary data
is the least significant bit. However, when you tell it the bit position where you want to start 
reading from the most signfigant bit is numbered 0. So lets setup some examples to show what it will
return. The binray data shown below is the 5AA58 section, the first bit shown is number 12 and the
last bit is 31.
        12      19             31
        |       |              |
  ....  0101 1010 1010 0101 1000 ....
        5    A    A    5    8
GetBinarySection(m_aryBinaryData, 12, 16, iOut);  iOut = 0x5AA5
GetBinarySection(m_aryBinaryData, 19, 6, cOut);   cOut = 0x14
GetBinarySection(m_aryBinaryData, 19, 10, iOut);  iOut = 0x014B   0001 0100 1011
GetBinarySection(m_aryBinaryData, 19, 11, iOut);  iOut = 0x0296   0010 1001 0110
when it starts filling in the returned value it starts at the end of the section it is retrieving 
and fills it in backwards so that the least signifigant value in the section is the least significant
bit in the value. If the section is too small to completely fill the bytes of the variable then the 
remaining bits are set to zero.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lStartBit		  	The start bit of the section we will retrieve. 
\param	lBitLength		  	Bit length of the section to retrieve. 
\param [in,out]	iOut	  	The output section. 

**/
void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned short &iOut)
{
	CStdArray<unsigned char> aryOutData;
	short iByte, iByteCount, iArraySize;

	Std_InValidRange((long) 1, (long) 16, lBitLength, true, "Length");

	iOut = 0;
	Std_GetBinarySection(aryRawData, lStartBit, lBitLength, aryOutData);
	iArraySize = aryOutData.GetSize()-1;

	iByteCount=0;
	for(iByte=iArraySize; iByte>=0 && iByteCount<2; iByte--)
	{
		iOut += (aryOutData[iByte] << (iByteCount*8));
		iByteCount++;		
	}
}

/**
\brief	Gets a section of bits from a byte array.

\details GetBinarySection Documentation
Assume we have a byte array of ByteArray = "5A05AA58607AEA3A9F69F97A1B17139A8AC0735C" of hex values.
The GetBinarySection functions retrieve sections of that byte array on BIT boundaries and not on byte
boundaries. In other words you can tell it to get 10 bits starting at bit position 19 and return it
to me as a 2 byte short. It works on the basic assumption that the right hand side of the binary data
is the least significant bit. However, when you tell it the bit position where you want to start 
reading from the most signfigant bit is numbered 0. So lets setup some examples to show what it will
return. The binray data shown below is the 5AA58 section, the first bit shown is number 12 and the
last bit is 31.
        12      19             31
        |       |              |
  ....  0101 1010 1010 0101 1000 ....
        5    A    A    5    8
GetBinarySection(m_aryBinaryData, 12, 16, iOut);  iOut = 0x5AA5
GetBinarySection(m_aryBinaryData, 19, 6, cOut);   cOut = 0x14
GetBinarySection(m_aryBinaryData, 19, 10, iOut);  iOut = 0x014B   0001 0100 1011
GetBinarySection(m_aryBinaryData, 19, 11, iOut);  iOut = 0x0296   0010 1001 0110
when it starts filling in the returned value it starts at the end of the section it is retrieving 
and fills it in backwards so that the least signifigant value in the section is the least significant
bit in the value. If the section is too small to completely fill the bytes of the variable then the 
remaining bits are set to zero.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lStartBit		  	The start bit of the section we will retrieve. 
\param	lBitLength		  	Bit length of the section to retrieve. 
\param [in,out]	lOut	  	The output section. 

**/
void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned long &lOut)
{
	CStdArray<unsigned char> aryOutData;
	short iByte, iByteCount, iArraySize;

	Std_InValidRange((long) 1, (long) 32, lBitLength, true, "Length");

	lOut = 0;
	Std_GetBinarySection(aryRawData, lStartBit, lBitLength, aryOutData);
	iArraySize = aryOutData.GetSize()-1;

	iByteCount=0;
	for(iByte=iArraySize; iByte>=0 && iByteCount<4; iByte--)
	{
		lOut += (aryOutData[iByte] << (iByteCount*8));
		iByteCount++;		
	}
}

/**
\brief	Gets a section of bits from a byte array.

\details GetBinarySection Documentation
Assume we have a byte array of ByteArray = "5A05AA58607AEA3A9F69F97A1B17139A8AC0735C" of hex values.
The GetBinarySection functions retrieve sections of that byte array on BIT boundaries and not on byte
boundaries. In other words you can tell it to get 10 bits starting at bit position 19 and return it
to me as a 2 byte short. It works on the basic assumption that the right hand side of the binary data
is the least significant bit. However, when you tell it the bit position where you want to start 
reading from the most signfigant bit is numbered 0. So lets setup some examples to show what it will
return. The binray data shown below is the 5AA58 section, the first bit shown is number 12 and the
last bit is 31.
        12      19             31
        |       |              |
  ....  0101 1010 1010 0101 1000 ....
        5    A    A    5    8
GetBinarySection(m_aryBinaryData, 12, 16, iOut);  iOut = 0x5AA5
GetBinarySection(m_aryBinaryData, 19, 6, cOut);   cOut = 0x14
GetBinarySection(m_aryBinaryData, 19, 10, iOut);  iOut = 0x014B   0001 0100 1011
GetBinarySection(m_aryBinaryData, 19, 11, iOut);  iOut = 0x0296   0010 1001 0110
when it starts filling in the returned value it starts at the end of the section it is retrieving 
and fills it in backwards so that the least signifigant value in the section is the least significant
bit in the value. If the section is too small to completely fill the bytes of the variable then the 
remaining bits are set to zero.

\author	dcofer
\date	5/3/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lStartBit		  	The start bit of the section we will retrieve. 
\param	lBitLength		  	Bit length of the section to retrieve. 
\param [in,out]	aryOutData	  	The output section. 

**/
void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, 
																							CStdArray<unsigned char> &aryOutData)
{
	long lEndBit, lByteLength, lBit, lBitsNeeded, lByte;

	lEndBit = lStartBit + lBitLength - 1;

	Std_InValidRange((long) 0, (long) ((aryRawData.GetSize() * 8) - 1), lStartBit, true, "Starting Bit Position");
	Std_InValidRange((long) 0, (long) ((aryRawData.GetSize() * 8) - 1), lEndBit, true, "Ending Bit Position");

	if( lBitLength%8 )
		lByteLength = ((long) ceil((float) (lBitLength / 8))) + 1;
	else
		lByteLength = lBitLength / 8;
	
	aryOutData.RemoveAll();
	aryOutData.SetSize(lByteLength);

	lBit = lEndBit;
	for(lByte=(lByteLength-1); lByte>=0; lByte--)
	{
		lBitsNeeded = lBitLength - (lEndBit-lBit);
		if(lBitsNeeded>8) lBitsNeeded = 8;

		aryOutData[lByte] = Std_GetByteFromArray(aryRawData, lBit, lBitsNeeded);
		lBit-=lBitsNeeded;
	}

}

/**
\brief	Gets a single byte from a byte array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	The raw byte array. 
\param	lEndBit			  	The end bit. 
\param	lBitsNeeded		  	The number of bits to retrieve. 

\return	output data.
**/
unsigned char STD_UTILS_PORT Std_GetByteFromArray(CStdArray<unsigned char> &aryRawData, long lEndBit, long lBitsNeeded)
{
	unsigned char cVal=0;
	long lBitIndex, lBit;

	Std_InValidRange((long) 1, (long) 8, lBitsNeeded, true, "Bits Needed");

	for(lBitIndex=0; lBitIndex<lBitsNeeded; lBitIndex++)
	{
		lBit = lEndBit - lBitIndex;
		
		if(Std_GetBitFromArray(aryRawData, lBit))
			cVal += (1 << lBitIndex);
	}

	return cVal;
}

/**
\brief	Gets a single bit from a byte array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	The raw byte array. 
\param	lBit			  	The bit to retrieve. 

\return	True if it succeeds.
**/
bool STD_UTILS_PORT Std_GetBitFromArray(CStdArray<unsigned char> &aryRawData, long lBit)
{
	long lByte;
	int iShift;
	unsigned char cVal;

	lByte = (long) (lBit/8);
	iShift = lBit - (lByte*8);
	cVal = aryRawData[lByte];

	if(cVal & (0x80 >> iShift))
		return true;
	else
		return false;

}

/**
\brief	Replaces a section of a byte array with a new byte array

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	The raw byte array. 
\param	lStartBit		  	The start bit. 
\param	lBitLength		  	Bit length of the section to replace. 
\param	cIn				  	The new data. 

**/
void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned char cIn)
{
	long lEndBit;

	Std_InValidRange((long) 1, (long) 8, lBitLength, true, "Length");

	lEndBit = lStartBit + lBitLength-1;
	Std_SetByteInArray(aryRawData, cIn, lEndBit, lBitLength);
}


/**
\brief	Replaces a section of a byte array with a new byte array

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	The raw byte array. 
\param	lStartBit		  	The start bit. 
\param	lBitLength		  	Bit length of the section to replace. 
\param	iIn				  	The new data. 

**/
void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned short iIn)
{
	CStdArray<unsigned char> aryIn;
	unsigned char cVal;

	Std_InValidRange((long) 1, (long) 16, lBitLength, true, "Length");

	cVal = (unsigned char) ((iIn & 0xFF00) >> 8);
	aryIn.Add(cVal);

	cVal = (unsigned char) (iIn & 0x00FF);
	aryIn.Add(cVal);

	Std_SetBinarySection(aryRawData, lStartBit, lBitLength, aryIn);
}

/**
\brief	Replaces a section of a byte array with a new byte array

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	The raw byte array. 
\param	lStartBit		  	The start bit. 
\param	lBitLength		  	Bit length of the section to replace. 
\param	lIn				  	The new data. 

**/
void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned long lIn)
{
	CStdArray<unsigned char> aryIn;
	unsigned char cVal;

	Std_InValidRange((long) 1, (long) 32, lBitLength, true, "Length");

	cVal = (unsigned char) ((lIn & 0xFF000000) >> 24);
	aryIn.Add(cVal);

	cVal = (unsigned char) ((lIn & 0x00FF0000) >> 16);
	aryIn.Add(cVal);

	cVal = (unsigned char) ((lIn & 0x0000FF00) >> 8);
	aryIn.Add(cVal);

	cVal = (unsigned char) (lIn & 0x000000FF);
	aryIn.Add(cVal);

	Std_SetBinarySection(aryRawData, lStartBit, lBitLength, aryIn);
}

/**
\brief	Replaces a section of a byte array with a new byte array

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	The raw byte array. 
\param	lStartBit		  	The start bit. 
\param	lBitLength		  	Bit length of the section to replace. 
\param	aryInData				  	The new data. 

**/
void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, 
																		 CStdArray<unsigned char> &aryInData)
{
	long lEndBit, lByteLength, lBit, lBitsNeeded, lByte, lNumBytes, lLastByte;

	lEndBit = lStartBit + lBitLength - 1;

	Std_InValidRange((long) 1, (long) (aryInData.GetSize() * 8), lBitLength, true, "BitLength");

	if( lBitLength%8 )
		lByteLength = ((long) ceil((float) (lBitLength / 8))) + 1;
	else
		lByteLength = lBitLength / 8;
	
	lNumBytes = aryInData.GetSize();
	lLastByte = lNumBytes - lByteLength;

	lBit = lEndBit;
	for(lByte=(lNumBytes-1); lByte>=lLastByte; lByte--)
	{
		lBitsNeeded = lBitLength - (lEndBit-lBit);
		if(lBitsNeeded>8) lBitsNeeded = 8;

		Std_SetByteInArray(aryRawData, aryInData[lByte], lBit, lBitsNeeded);
		lBit-=lBitsNeeded;
	}

}

/**
\brief	Copies a section of a byte array to a new position within the array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryInData 	Input raw byte array. 
\param	lStartInBit		  	The start bit to begin the copy. 
\param [in,out]	aryOutData	Output raw byte array. 
\param	lStartOutBit	  	The start bit of where to place the copy. 
\param	lBitLength		  	Bit length of the copy. 

**/
void STD_UTILS_PORT Std_CopyBinarySection(CStdArray<unsigned char> &aryInData, long lStartInBit,  
 																		  CStdArray<unsigned char> &aryOutData, long lStartOutBit,
																			long lBitLength)
{
	CStdArray<unsigned char> aryTempData;
	
	//Get the part that needs to be copied.
	Std_GetBinarySection(aryInData, lStartInBit, lBitLength, aryTempData);
	Std_SetBinarySection(aryOutData, lStartOutBit, lBitLength, aryTempData);
}

/**
\brief	Sets a byte within an array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	Raw input byte array. 
\param	cIn				  	The input byte. 
\param	lEndBit			  	The end bit. 
\param	lBitsNeeded		  	The number of bits to copy. 

**/
void STD_UTILS_PORT Std_SetByteInArray(CStdArray<unsigned char> &aryRawData, unsigned char cIn, long lEndBit, long lBitsNeeded)
{
	long lBitIndex, lBit;

	Std_InValidRange((long) 1, (long) 8, lBitsNeeded, true, "Bits Needed");

	for(lBitIndex=0; lBitIndex<lBitsNeeded; lBitIndex++)
	{
		lBit = lEndBit - lBitIndex;

		if(bit_get(cIn, lBitIndex))
			Std_SetBitInArray(aryRawData, lBit, true);
		else
			Std_SetBitInArray(aryRawData, lBit, false);
	}

}

/**
\brief	Sets a bit within an array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lBit			  	The bit to set in the array. 
\param	bVal			  	The bit value. 

**/
void STD_UTILS_PORT Std_SetBitInArray(CStdArray<unsigned char> &aryRawData, long lBit, bool bVal)
{
	long lByte;
	int iShift;
	unsigned char cVal;

	lByte = (long) (lBit/8);
	iShift = lBit - (lByte*8);

	if(lByte>=aryRawData.GetSize())
		aryRawData.SetSize(lByte+1);

	cVal = aryRawData[lByte];

	if(bVal)
		cVal = cVal | (0x80 >> iShift);
	else
		cVal = cVal & (0xFF7F >> iShift);

	aryRawData[lByte] = cVal;
}

/**
\brief	Sets an entire section within a byte array with a bit value.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lStartBit		  	The start bit of the section. 
\param	lBitLength		  	Bit length of the section. 
\param	bVal			  	Bit value to set. 
**/
void STD_UTILS_PORT Std_SetBitRangeInArray(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, bool bVal)
{
	Std_IsAboveMin((long) -1, lBitLength, true, "SetBitRangeInArray::BitLength");
	Std_IsAboveMin((long) -1, lStartBit, true, "SetBitRangeInArray::StartBit");

	long lEndBit=lStartBit + lBitLength;
	for(long lBit=lStartBit; lBit<lEndBit; lBit++)
		Std_SetBitInArray(aryRawData, lBit, bVal);
}

/**
\brief	Flips a bit within a byte array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lBit			  	The bit to flip. 
**/
void STD_UTILS_PORT Std_FlipBitInArray(CStdArray<unsigned char> &aryRawData, long lBit)
{
	bool bVal = Std_GetBitFromArray(aryRawData, lBit);
	bVal = !bVal;
	Std_SetBitInArray(aryRawData, lBit, bVal);
}

/**
\brief	Flips all the bits within a section of a byte array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryRawData	Raw byte array. 
\param	lStartBit		  	The start bit of the section to flip. 
\param	lBitLength		  	Bit length of the section. 

**/
void STD_UTILS_PORT Std_FlipBitRangeInArray(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength)
{
	Std_IsAboveMin((long) -1, lBitLength, true, "FlipBitRangeInArray::BitLength");
	Std_IsAboveMin((long) -1, lStartBit, true, "FlipBitRangeInArray::StartBit");

	long lEndBit=lStartBit + lBitLength;
	for(long lBit=lStartBit; lBit<lEndBit; lBit++)
		Std_FlipBitInArray(aryRawData, lBit);
}

/**
\brief	Reads binary data from a byte array

\author	dcofer
\date	5/4/2011

\param [in,out]	aryBinaryData	Raw byte array. 
\param	lLastBit			 	The last bit. 
\param [in,out]	lStartBit	 	The start bit. 
\param	lBitLength			 	Bit length of the section to read. 
\param [in,out]	cOut		 	The output. 

\return	True if successful.
**/
bool STD_UTILS_PORT Std_ReadBinaryData(CStdArray<unsigned char> &aryBinaryData, long lLastBit, long &lStartBit, long lBitLength, unsigned char &cOut)
{
	long lEndBit;

	//Verify that is still enough left in the array for the read.
	lEndBit = lStartBit + lBitLength-1;

	if(!Std_InValidRange((long) 0, lLastBit, lStartBit, false)) return false;
	if(!Std_InValidRange((long) 0, lLastBit, lEndBit, false)) return false;

	Std_GetBinarySection(aryBinaryData, lStartBit, lBitLength, cOut);

	TRACE_DETAIL_NS("[ReadBinaryData(uchar)] Value: " + STR(cOut) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;

	return true;
}

/**
\brief	Reads binary data from a byte array

\author	dcofer
\date	5/4/2011

\param [in,out]	aryBinaryData	Raw byte array. 
\param	lLastBit			 	The last bit. 
\param [in,out]	lStartBit	 	The start bit. 
\param	lBitLength			 	Bit length of the section to read. 
\param [in,out]	iOut		 	The output. 

\return	True if successful.
**/
bool STD_UTILS_PORT Std_ReadBinaryData(CStdArray<unsigned char> &aryBinaryData, long lLastBit, long &lStartBit, long lBitLength, unsigned short &iOut)
{
	long lEndBit;

	//Verify that is still enough left in the array for the read.
	lEndBit = lStartBit + lBitLength-1;

	if(!Std_InValidRange((long) 0, lLastBit, lStartBit, false)) return false;
	if(!Std_InValidRange((long) 0, lLastBit, lEndBit, false)) return false;

	Std_GetBinarySection(aryBinaryData, lStartBit, lBitLength, iOut);

	TRACE_DETAIL_NS("[ReadBinaryData(ushort)] Value: " + STR(iOut) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;

	return true;
}

/**
\brief	Reads binary data from a byte array

\author	dcofer
\date	5/4/2011

\param [in,out]	aryBinaryData	Raw byte array. 
\param	lLastBit			 	The last bit. 
\param [in,out]	lStartBit	 	The start bit. 
\param	lBitLength			 	Bit length of the section to read. 
\param [in,out]	lOut		 	The output. 

\return	True if successful.
**/
bool STD_UTILS_PORT Std_ReadBinaryData(CStdArray<unsigned char> &aryBinaryData, long lLastBit, long &lStartBit, long lBitLength, unsigned long &lOut)
{
	long lEndBit;

	//Verify that is still enough left in the array for the read.
	lEndBit = lStartBit + lBitLength-1;

	if(!Std_InValidRange((long) 0, lLastBit, lStartBit, false)) return false;
	if(!Std_InValidRange((long) 0, lLastBit, lEndBit, false)) return false;

	Std_GetBinarySection(aryBinaryData, lStartBit, lBitLength, lOut);

	TRACE_DETAIL_NS("[ReadBinaryData(ulong)] Value: " + STR((long) lOut) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;

	return true;
}

/**
\brief	Writes data to a byte array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryBinaryData	Raw byte array. 
\param [in,out]	lStartBit	 	The start bit. 
\param	lBitLength			 	Bit length of the section. 
\param	cIn					 	The input. 

**/
void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned char cIn)
{
	TRACE_DETAIL_NS("[WriteBinaryData(uchar)] Value: " + STR(cIn) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	Std_SetBinarySection(aryBinaryData, lStartBit, lBitLength, cIn);

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;
}


/**
\brief	Writes data to a byte array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryBinaryData	Raw byte array. 
\param [in,out]	lStartBit	 	The start bit. 
\param	lBitLength			 	Bit length of the section. 
\param	iIn					 	The input. 

**/
void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned short iIn)
{
	TRACE_DETAIL_NS("[WriteBinaryData(ushort)] Value: " + STR(iIn) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	Std_SetBinarySection(aryBinaryData, lStartBit, lBitLength, iIn);

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;
}


/**
\brief	Writes data to a byte array.

\author	dcofer
\date	5/4/2011

\param [in,out]	aryBinaryData	Raw byte array. 
\param [in,out]	lStartBit	 	The start bit. 
\param	lBitLength			 	Bit length of the section. 
\param	lIn					 	The input. 

**/
void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned long lIn)
{
	TRACE_DETAIL_NS("[WriteBinaryData(ulong)] Value: " + STR((long) lIn) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	Std_SetBinarySection(aryBinaryData, lStartBit, lBitLength, lIn);

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;
}

/**
\brief	Converts a binary value to grey code.

\author	dcofer
\date	5/4/2011

\param	cVal	The binary value. 

\return	grey code value.
**/
unsigned char STD_UTILS_PORT Std_BinaryToGreyCode(unsigned char cVal)
{
	short iBit;
	unsigned char cOut=0;

	for(iBit=0; iBit< 7; iBit++) 
	{
		cOut = (unsigned char) bit_put(cOut, iBit, bit_get(cVal, iBit) ^ bit_get(cVal, iBit+1) );
	}
	cOut = (unsigned char) bit_put(cOut, 7, bit_get(cVal, 7) );

	TRACE_DETAIL_NS("[BinaryToGreyCode(uchar)] Input: " + STR(cVal) + "  Output: " + STR(cOut));

	return cOut;
}


/**
\brief	Converts a binary value to grey code.

\author	dcofer
\date	5/4/2011

\param	iVal	The binary value. 

\return	grey code value.
**/
unsigned short STD_UTILS_PORT Std_BinaryToGreyCode(unsigned short iVal)
{
	short iBit;
	unsigned short iOut=0;

	for(iBit=0; iBit< 15; iBit++) 
	{
		iOut = (unsigned short) bit_put(iOut, iBit, bit_get(iVal, iBit) ^ bit_get(iVal, iBit+1) );
	}
	iOut = (unsigned short) bit_put(iOut, 15, bit_get(iVal, 15) );

	TRACE_DETAIL_NS("[BinaryToGreyCode(ushort)] Input: " + STR(iVal) + "  Output: " + STR(iOut));

	return iOut;
}

/**
\brief	Converts a binary value to grey code.

\author	dcofer
\date	5/4/2011

\param	lVal	The binary value. 

\return	grey code value.
**/
unsigned long STD_UTILS_PORT Std_BinaryToGreyCode(unsigned long lVal)
{
	short iBit;
	unsigned long lOut=0;

	for(iBit=0; iBit< 31; iBit++) 
	{
		lOut = bit_put(lOut, iBit, bit_get(lVal, iBit) ^ bit_get(lVal, iBit+1) );
	}
	lOut = bit_put(lOut, 31, bit_get(lVal, 31) );

	TRACE_DETAIL_NS("[BinaryToGreyCode(ulong)] Input: " + STR((long) lVal) + "  Output: " + STR((long) lOut));

	return lOut;
}


/**
\brief	Converts a grey code to a binary value.

\author	dcofer
\date	5/4/2011

\param	cVal	The grey code value. 

\return	binary code value.
**/
unsigned char STD_UTILS_PORT Std_GreyCodeToBinary(unsigned char cVal)
{
	short iBit, iTempBit;
	unsigned char cOut=0, cTemp;

	cOut = 0;
	for (iBit = 0; iBit <= 7; iBit++ ) 
	{
		cTemp = 0;
		for (iTempBit = iBit; iTempBit <= 7; iTempBit++ ) 
			cTemp ^= bit_get(cVal, iTempBit);

		cOut = (unsigned char) bit_put (cOut, iBit, cTemp);
	}

	TRACE_DETAIL_NS("[GreyCodeToBinary(uchar)] Input: " + STR(cVal) + "  Output: " + STR(cOut));

	return cOut;
}

/**
\brief	Converts a grey code to a binary value.

\author	dcofer
\date	5/4/2011

\param	iVal	The grey code value. 

\return	binary code value.
**/
unsigned short STD_UTILS_PORT Std_GreyCodeToBinary(unsigned short iVal)
{
	short iBit, iTempBit;
	unsigned short iOut=0, iTemp;

	iOut = 0;
	for (iBit = 0; iBit <= 15; iBit++ ) 
	{
		iTemp = 0;
		for (iTempBit = iBit; iTempBit <= 15; iTempBit++ ) 
			iTemp ^= bit_get(iVal, iTempBit);

		iOut = (unsigned short) bit_put (iOut, iBit, iTemp);
	}

	TRACE_DETAIL_NS("[BinaryToGreyCode(ushort)] Input: " + STR(iVal) + "  Output: " + STR(iOut));

	return iOut;
}


/**
\brief	Converts a grey code to a binary value.

\author	dcofer
\date	5/4/2011

\param	lVal	The grey code value. 

\return	binary code value.
**/
unsigned long STD_UTILS_PORT Std_GreyCodeToBinary(unsigned long lVal)
{
	short iBit, iTempBit;
	unsigned long lOut=0, lTemp;

	lOut = 0;
	for (iBit = 0; iBit <= 31; iBit++ ) 
	{
		lTemp = 0;
		for (iTempBit = iBit; iTempBit <= 31; iTempBit++ ) 
			lTemp ^= bit_get(lVal, iTempBit);

		lOut = bit_put (lOut, iBit, lTemp);
	}

	TRACE_DETAIL_NS("[BinaryToGreyCode(ulong)] Input: " + STR((long) lVal) + "  Output: " + STR((long) lOut));

	return lOut;
}

// Byte Array Functions
//***************************************************************************************************************


//***************************************************************************************************************
// Logging Functions

#ifdef _WIN32_WCE
	int g_iTraceLevel = 0;

	int STD_UTILS_PORT Std_GetTraceLevel()
	{return g_iTraceLevel;}

	void STD_UTILS_PORT Std_SetTraceLevel(const int iLevel)
	{g_iTraceLevel = iLevel;}

	void STD_UTILS_PORT Std_SetLogLevel(const int iLevel)
	{g_iTraceLevel = iLevel;}

	void STD_UTILS_PORT Std_LogMsg(const int iLevel, string strMessage, string strSourceFile, int iSourceLine, bool bPrintHeader)
	{
		//if(g_iTraceLevel==0 || iLevel>g_iTraceLevel)
		if(iLevel>g_iTraceLevel)
			return;

		string strFinalMessage;

		if(iSourceLine>=0)
			strFinalMessage = STR(strSourceFile) + " (" + STR(iSourceLine) + ") \r\n" + STR(strMessage) + "\r\n";
		else
			strFinalMessage = strMessage;

		STLOG_WRITE(strFinalMessage.c_str()); 
	}

	void STD_UTILS_PORT Std_TraceMsg(const int iLevel, string strMessage, string strSourceFile, int iSourceLine, bool bLogToFile, bool bPrintHeader)
	{
		string strTemp = strMessage + "\r\n";

	#ifdef STD_TRACE_TO_DEBUGGER
		wchar_t *sMessage = Std_ConvertFromANSI(strTemp);
		if(sMessage)
		{
			OutputDebugString(sMessage);
			delete[] sMessage;
			sMessage = NULL;
		}
	#endif

		if(bLogToFile) 
			Std_LogMsg(iLevel, strMessage, strSourceFile, iSourceLine, bPrintHeader);
	}

	void STD_UTILS_PORT Std_ResetLog()
	{
		CSTLogFile::GetLogFile()->Stop();
	}
#else

	/**
	\brief	Gets the trace level.
	
	\author	dcofer
	\date	5/4/2011
	
	\return	trace level.
	**/
	int STD_UTILS_PORT Std_GetTraceLevel()
	{return GetTraceLevel();}

	/**
	\brief	Sets teh trace level.
	
	\author	dcofer
	\date	5/4/2011
	
	\param	iVal	The value. 

	**/
	void STD_UTILS_PORT Std_SetTraceLevel(const int iVal)
	{SetTraceLevel(iVal);}

	/**
	\brief	Sets the log file prefix.
	
	\author	dcofer
	\date	5/4/2011
	
	\param	strFilePrefix	The string file prefix. 

	**/
	void STD_UTILS_PORT Std_SetLogFilePrefix(string strFilePrefix)
	{
		if(GetTraceFilePrefix() != strFilePrefix)
			SetTraceFilePrefix(strFilePrefix.c_str());
	}

	/**
	\brief	Gets the log file prefix.
	
	\author	dcofer
	\date	5/4/2011
	
	\return	prefix.
	**/
	string STD_UTILS_PORT Std_GetLogFilePrefix()
	{return GetTraceFilePrefix();}

	/**
	\brief	Sets the log level.
	
	\author	dcofer
	\date	5/4/2011
	
	\param	iLevel	The level. 

	**/
	void STD_UTILS_PORT Std_SetLogLevel(const int iLevel)
	{SetTraceLevel(iLevel);}

	/**
	\brief	Logs a message,
	
	\author	dcofer
	\date	5/4/2011
	
	\param	iLevel		 	The log level. 
	\param	strMessage   	Log message. 
	\param	strSourceFile	The string source file. 
	\param	iSourceLine  	Source line number. 
	\param	bPrintHeader 	true to print header. 

	**/
	void STD_UTILS_PORT Std_LogMsg(const int iLevel, string strMessage, string strSourceFile, int iSourceLine, bool bPrintHeader)
	{
		if(GetTraceLevel()==0 || iLevel>GetTraceLevel())
			return;

		string strFinalMessage;

		if(iSourceLine>=0)
			strFinalMessage = STR(strSourceFile) + " (" + STR(iSourceLine) + ") \r\n" + STR(strMessage) + "\r\n";
		else
			strFinalMessage = strMessage;

		Std_Log(iLevel, bPrintHeader, strFinalMessage.c_str());
	}

	/**
	\brief	Traces a message to the debugger window.
	
	\author	dcofer
	\date	5/4/2011
	
	\param	iLevel		 	The log level. 
	\param	strMessage   	Log message. 
	\param	strSourceFile	The string source file. 
	\param	iSourceLine  	Source line number. 
	\param	bLogToFile   	true to log to file. 
	\param	bPrintHeader 	true to print header. 

	**/
	void STD_UTILS_PORT Std_TraceMsg(const int iLevel, string strMessage, string strSourceFile, int iSourceLine, bool bLogToFile, bool bPrintHeader)
	{
		int iLogLevel = GetTraceLevel();
		if(iLogLevel==0||iLevel>iLogLevel) return;

		string strTemp = strMessage + "\r\n";

		#ifdef STD_TRACE_TO_DEBUGGER
			OutputDebugString(strTemp.c_str());
		#endif

		if(bLogToFile) 
		{
			if(iSourceLine>=0)
				strMessage = STR(strSourceFile) + " (" + STR(iSourceLine) + ") \r\n" + STR(strMessage) + "\r\n";

			Std_Log(iLevel, bPrintHeader, strMessage.c_str());
		}
	}
#endif


// Logging Functions
//***************************************************************************************************************


//***************************************************************************************************************
// Timing Functions


#ifndef _WIN32_WCE

/**
\brief	Gets the time tick.

\author	dcofer
\date	5/4/2011

\return	time tick.
**/
unsigned long STD_UTILS_PORT Std_GetTick()
	{
		/*
	  struct _timeb stTime;
		_ftime( &stTime );
		return ((stTime.time*1000)+ stTime.millitm);
		*/
		return  GetTickCount64();
	}

#endif 

  
// Timing Functions
//***************************************************************************************************************

//***************************************************************************************************************
// File Functions

/**
\brief	determines if this is a full path name.

\details It does this by checking to see if the : is present in the string.

\author	dcofer
\date	5/4/2011

\param	strPath	filename. 

\return	True if full path name.
**/
bool STD_UTILS_PORT Std_IsFullPath(string strPath)
{
	CStdArray<string> aryParts;
	int iCount = Std_Split(strPath, ":", aryParts);
	if(iCount>1)
		return true;
	else
		return false;
}

/**
\brief	Splits the path from the actual filename.

\author	dcofer
\date	5/4/2011

\param	strFullPath	   	Full pathname of the string full file. 
\param [in,out]	strPath	Full path without the filename. 
\param [in,out]	strFile	Filename. 

**/
void STD_UTILS_PORT Std_SplitPathAndFile(string strFullPath, string &strPath, string &strFile)
{
	CStdArray<string> aryParts;
	int iCount = Std_Split(strFullPath, "\\", aryParts);
	if(iCount<=0)
	{
		strPath = "";
		strFile = strFullPath;
	}
	else
	{
		strFile = aryParts[iCount-1];
		aryParts.RemoveAt(iCount-1);

		strPath = Std_Combine(aryParts, "\\");
		if(!Std_IsBlank(strPath)) strPath += "\\";
	}
}

/**
\brief	Gets the file extension.

\author	dcofer
\date	5/4/2011

\param [in,out]	strFile	Filename. 

\return	file extension.
**/
string STD_UTILS_PORT Std_FileExtension(string &strFile)
{
	CStdArray<string> aryParts;
	int iCount = Std_Split(strFile, ".", aryParts);
	string strExt = "";

	if(iCount >0)
	{
		strExt = "." + aryParts[iCount-1];
	}

	return strExt;
}

/**
\brief	Queries if a given directory exists.

\author	dcofer
\date	5/4/2011

\param	strPath	Full pathname of the string file. 

\return	.
**/
BOOL STD_UTILS_PORT Std_DirectoryExists(string strPath)
{
#ifdef _WIN32_WCE
	wchar_t *sPath = Std_ConvertFromANSI(strPath);
	DWORD dwAttr = GetFileAttributes(sPath);
	if(sPath) delete sPath;
#else
	DWORD dwAttr = GetFileAttributes(strPath.c_str());
#endif

	if(dwAttr == 0xffffffff)
		return FALSE;
	else if(dwAttr & FILE_ATTRIBUTE_DIRECTORY)
		return TRUE;

	return FALSE;
}

void STD_UTILS_PORT Std_SetFileTime(string strFilename)
{
	// Create a systemtime struct
	SYSTEMTIME thesystemtime;
	 
	// Get current system time and then change the day to the 3rd
	// You can also change year, month, day of week etc
	GetSystemTime(&thesystemtime);

	Std_SetFileTime(strFilename, thesystemtime);
}

void STD_UTILS_PORT Std_SetFileTime(string strFilename, SYSTEMTIME newTime)
{
	// Create a FILETIME struct and convert our new SYSTEMTIME
	// over to the FILETIME struct for use in SetFileTime below
	FILETIME thefiletime;
	SystemTimeToFileTime(&newTime,&thefiletime);
	 
	// Get a handle to our file and with file_write_attributes access
	HANDLE filename = CreateFile(strFilename.c_str(), FILE_WRITE_ATTRIBUTES, FILE_SHARE_READ|FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	    
	// Set the file time on the file
	SetFileTime(filename,(LPFILETIME) NULL,(LPFILETIME) NULL,&thefiletime);
	    
	// Close our handle.
	CloseHandle(filename);
}


// File Functions
//***************************************************************************************************************

//class StdFixed;
//
//void TestTree()
//
//{
//	tree<CStdFixed *> tr;
//	tree<CStdFixed *>::iterator top, one;
//
//	CStdFixed *lp1 = new CStdFixed();
//	top=tr.begin();
//	one=tr.insert(top, lp1);
//	
//	tr.clear();
//
//	ostringstream str;
//	kptree::ptr_print_tree_bracketed(tr, str);
//	OutputDebugString(str.str().c_str());
//}

}				//StdUtils

