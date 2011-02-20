#include "stdafx.h"

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

string STD_UTILS_PORT Std_CreateAppID()
{
	//Need to make this threadsafe at somet point.
	
	g_iAppID++;
	return STR(g_iAppID);
}

void AddToErrorCallChain(CStdErrorInfo &oInfo, string strSourceFile, long lSourceLine)
{
	if(lSourceLine > 0)
	{
		ostringstream oStream;
		oStream << strSourceFile << " (" << lSourceLine << ")"; 
		oInfo.m_aryCallChain.Add(oStream.str());
	}
}

void STD_UTILS_PORT Std_RelayError(CStdErrorInfo oInfo, string strSourceFile, long lSourceLine)
{
	AddToErrorCallChain(oInfo, strSourceFile, lSourceLine);
	throw oInfo;
}

void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, unsigned char iVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << (int) iVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, unsigned short iVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << iVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, int iVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << iVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, long lVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << lVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, float fltVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << fltVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, double dblVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << dblVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

void STD_UTILS_PORT Std_ThrowError(long lError, string strError, string strSourceFile, long lSourceLine, 
																	 string strValueName, string strVal)
{
	ostringstream oStream;
	oStream << "   (" << strValueName << ": " << strVal << ")"; 
	Std_ThrowError(lError, strError, strSourceFile, lSourceLine, oStream.str());
}

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

string STD_UTILS_PORT Std_ToStr(string strVal)
{return strVal;}

string STD_UTILS_PORT Std_ToStr(const char *strVal)
{
	string strV = strVal;
	return strV;
}

string STD_UTILS_PORT Std_ToStr(unsigned char iVal)
{
	std::ostringstream buf; 
	buf << iVal ;
	std::string str = buf.str() ;
	return str;
}

string STD_UTILS_PORT Std_ToStr(unsigned short iVal)
{
	std::ostringstream buf; 
	buf << iVal ;
	std::string str = buf.str() ;
	return str;
}

string STD_UTILS_PORT Std_ToStr(int iVal)
{
	std::ostringstream buf; 
	buf << iVal ;
	std::string str = buf.str() ;
	return str;
}

string STD_UTILS_PORT Std_ToStr(long lVal)
{
	std::ostringstream buf; 
	buf << lVal ;
	std::string str = buf.str() ;
	return str;
}

string STD_UTILS_PORT Std_ToStr(float fltVal)
{
	std::ostringstream buf; 
	buf << fltVal ;
	std::string str = buf.str() ;
	return str;
}

string STD_UTILS_PORT Std_ToStr(double dblVal)
{
	std::ostringstream buf; 
	buf << dblVal ;
	std::string str = buf.str() ;
	return str;
}

string STD_UTILS_PORT Std_ToStr(bool bVal)
{	
	if(bVal)
		return "True";
	else
		return "False";
}

string STD_UTILS_PORT Std_ToStr(string strFormat, unsigned char iVal)
{return Std_Format(strFormat.c_str(), iVal);}

string STD_UTILS_PORT Std_ToStr(string strFormat, unsigned short iVal)
{return Std_Format(strFormat.c_str(), iVal);}

string STD_UTILS_PORT Std_ToStr(string strFormat, int iVal)
{return Std_Format(strFormat.c_str(), iVal);}

string STD_UTILS_PORT Std_ToStr(string strFormat, long lVal)
{return Std_Format(strFormat.c_str(), lVal);}

string STD_UTILS_PORT Std_ToStr(string strFormat, float fltVal)
{return Std_Format(strFormat.c_str(), fltVal);}

string STD_UTILS_PORT Std_ToStr(string strFormat, double dblVal)
{return Std_Format(strFormat.c_str(), dblVal);}

bool STD_UTILS_PORT Std_ToBool(int iVal)
{
	if(iVal)
		return true;
	else
		return false;
}

bool STD_UTILS_PORT Std_ToBool(string strVal)
{
	string strV = Std_ToUpper(Std_Trim(strVal));

	if(strV == "TRUE" || strV == "1")
		return true;
	else
		return false;
}

string STD_UTILS_PORT Std_NullStr(string strFormat)
{
	if(strFormat.length())
		return ("'" + strFormat + "'");
	else
		return "NULL";
}

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

string STD_UTILS_PORT Std_Trim(string strVal)
{
	return Std_TrimLeft(Std_TrimRight(strVal));
}

bool NotSpace(char c)
{return !std::isspace(c);}

string STD_UTILS_PORT Std_TrimLeft(string strVal)
{
	strVal.erase(strVal.begin(), std::find_if(strVal.begin(), strVal.end(), NotSpace));
	return strVal;
}

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

BOOL STD_UTILS_PORT Std_IsNumeric(string strVal)
{
	int iPos = strVal.find_first_not_of("0123456789.+-eE");
	if(iPos == -1)
		return true;
	else
		return false;
}

BOOL STD_UTILS_PORT Std_IsIntegerType(string strVal)
{
	int iPos = strVal.find_first_not_of("0123456789+-");
	if(iPos == -1)
		return true;
	else
		return false;
}

string STD_UTILS_PORT Std_Left(string strVal, int iCount)
{
	iCount = SSMAX(0, SSMIN(iCount, static_cast<int>(strVal.length())));
	return strVal.substr(0, iCount); 
}

string STD_UTILS_PORT Std_Right(string strVal, int iCount)
{
	iCount = SSMAX(0, SSMIN(iCount, static_cast<int>(strVal.length())));
	return strVal.substr(strVal.length()-iCount);
}

string STD_UTILS_PORT Std_ToUpper(string strVal)
{
	std::transform(strVal.begin(), strVal.end(), strVal.begin(), (int(*)(int))std::toupper);
	return strVal;
}

string STD_UTILS_PORT Std_ToLower(string strVal)
{
	std::transform(strVal.begin(), strVal.end(), strVal.begin(), (int(*)(int))std::tolower);
	return strVal;
}

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

long STD_UTILS_PORT Std_RGB(unsigned char iRed, unsigned char iGreen, unsigned char iBlue)
{
	return ((iBlue << 16) | (iGreen << 8) | (iRed));
}

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

void STD_UTILS_PORT Std_LoadColor(string strXml, string strParamName, float *aryColor, bool bThrowError)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement(strParamName);
	
	Std_LoadColor(oXml, strParamName, aryColor, bThrowError);
}

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


void STD_UTILS_PORT Std_SRand(unsigned long lSeed)
{init_genrand(lSeed);}

int STD_UTILS_PORT Std_IRand( int low, int high )
{
	high++;
	return (int) (( abs(high-low) * genrand_real2() ) + low);
}


long STD_UTILS_PORT Std_LRand( long low, long high )
{
	high++;
	return (long) (( abs(high-low) * genrand_real2() ) + low);
}


float STD_UTILS_PORT Std_FRand( float low, float high )
{return (float) (( fabs(high-low) * genrand_real1() ) + low);}


double STD_UTILS_PORT Std_DRand( double low, double high )
{return (( fabs(high-low) * genrand_real1() ) + low);}



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


string STD_UTILS_PORT Std_CheckString(string strVal)
{
	string strTemp = strVal;
	strTemp = Std_ToUpper(Std_Trim(strTemp));
	return strTemp;
}


bool STD_UTILS_PORT Std_IsBlank(string strVal)
{
	string strTemp = Std_Trim(strVal);
	if(!strTemp.length())
		return true;
	else
		return false;
}


//***************************************************************************************************************
// Byte Array Functions

//  bit_get (x,i) is the value of bit i of x
unsigned long bit_get (unsigned long x, int i) 
{return ( x >> i) & 1;}


//  bit_put (x, i, v) returns the same as x except that bit i is 
//                    set to the same value as bit 0 of v.
unsigned long bit_put (unsigned long x, int i, unsigned long v) 
{
	if ( v & 1 ) {
		return x | (1 << i);
	} else {
		return x & ~(1 << i);
	}
}


string STD_UTILS_PORT Std_ByteArrayToHexString(CStdArray<unsigned char> &aryBytes) 
{
	string strHex;
	Std_ByteArrayToHexString(aryBytes, strHex);
	return strHex;
}

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


//GetBinarySection Documentation
//  Assume we have a byte array of ByteArray = "5A05AA58607AEA3A9F69F97A1B17139A8AC0735C" of hex values.
//  The GetBinarySection functions retrieve sections of that byte array on BIT boundaries and not on byte
//  boundaries. In other words you can tell it to get 10 bits starting at bit position 19 and return it
//  to me as a 2 byte short. It works on the basic assumption that the right hand side of the binary data
//  is the least significant bit. However, when you tell it the bit position where you want to start 
//  reading from the most signfigant bit is numbered 0. So lets setup some examples to show what it will
//  return. The binray data shown below is the 5AA58 section, the first bit shown is number 12 and the
//  last bit is 31.
//
//        12      19             31
//        |       |              |
//  ....  0101 1010 1010 0101 1000 ....
//        5    A    A    5    8
//
//  GetBinarySection(m_aryBinaryData, 12, 16, iOut);  iOut = 0x5AA5
//  GetBinarySection(m_aryBinaryData, 19, 6, cOut);   cOut = 0x14
//  GetBinarySection(m_aryBinaryData, 19, 10, iOut);  iOut = 0x014B   0001 0100 1011
//  GetBinarySection(m_aryBinaryData, 19, 11, iOut);  iOut = 0x0296   0010 1001 0110
//  
//  when it starts filling in the returned value it starts at the end of the section it is retrieving 
//  and fills it in backwards so that the least signifigant value in the section is the least significant
//  bit in the value. If the section is too small to completely fill the bytes of the variable then the 
//  remaining bits are set to zero.

long STD_UTILS_PORT Std_GetBitSize(CStdArray<unsigned char> &aryRawData)
{return (8*aryRawData.GetSize());}

void STD_UTILS_PORT Std_GetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned char &cOut)
{
	CStdArray<unsigned char> aryOutData;

	Std_InValidRange((long) 1, (long) 8, lBitLength, true, "Length");

	Std_GetBinarySection(aryRawData, lStartBit, lBitLength, aryOutData);

	cOut = aryOutData[aryOutData.GetSize()-1];
}

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


void STD_UTILS_PORT Std_SetBinarySection(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, unsigned char cIn)
{
	long lEndBit;

	Std_InValidRange((long) 1, (long) 8, lBitLength, true, "Length");

	lEndBit = lStartBit + lBitLength-1;
	Std_SetByteInArray(aryRawData, cIn, lEndBit, lBitLength);
}



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


void STD_UTILS_PORT Std_CopyBinarySection(CStdArray<unsigned char> &aryInData, long lStartInBit,  
 																		  CStdArray<unsigned char> &aryOutData, long lStartOutBit,
																			long lBitLength)
{
	CStdArray<unsigned char> aryTempData;
	
	//Get the part that needs to be copied.
	Std_GetBinarySection(aryInData, lStartInBit, lBitLength, aryTempData);
	Std_SetBinarySection(aryOutData, lStartOutBit, lBitLength, aryTempData);
}


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


void STD_UTILS_PORT Std_SetBitRangeInArray(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength, bool bVal)
{
	Std_IsAboveMin((long) -1, lBitLength, true, "SetBitRangeInArray::BitLength");
	Std_IsAboveMin((long) -1, lStartBit, true, "SetBitRangeInArray::StartBit");

	long lEndBit=lStartBit + lBitLength;
	for(long lBit=lStartBit; lBit<lEndBit; lBit++)
		Std_SetBitInArray(aryRawData, lBit, bVal);
}


void STD_UTILS_PORT Std_FlipBitInArray(CStdArray<unsigned char> &aryRawData, long lBit)
{
	bool bVal = Std_GetBitFromArray(aryRawData, lBit);
	bVal = !bVal;
	Std_SetBitInArray(aryRawData, lBit, bVal);
}


void STD_UTILS_PORT Std_FlipBitRangeInArray(CStdArray<unsigned char> &aryRawData, long lStartBit, long lBitLength)
{
	Std_IsAboveMin((long) -1, lBitLength, true, "FlipBitRangeInArray::BitLength");
	Std_IsAboveMin((long) -1, lStartBit, true, "FlipBitRangeInArray::StartBit");

	long lEndBit=lStartBit + lBitLength;
	for(long lBit=lStartBit; lBit<lEndBit; lBit++)
		Std_FlipBitInArray(aryRawData, lBit);
}


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


void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned char cIn)
{
	TRACE_DETAIL_NS("[WriteBinaryData(uchar)] Value: " + STR(cIn) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	Std_SetBinarySection(aryBinaryData, lStartBit, lBitLength, cIn);

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;
}


void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned short iIn)
{
	TRACE_DETAIL_NS("[WriteBinaryData(ushort)] Value: " + STR(iIn) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	Std_SetBinarySection(aryBinaryData, lStartBit, lBitLength, iIn);

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;
}


void STD_UTILS_PORT Std_WriteBinaryData(CStdArray<unsigned char> &aryBinaryData, long &lStartBit, long lBitLength, unsigned long lIn)
{
	TRACE_DETAIL_NS("[WriteBinaryData(ulong)] Value: " + STR((long) lIn) + "  StartBit: " + STR(lStartBit) + "  BitLength: " + STR(lBitLength));

	Std_SetBinarySection(aryBinaryData, lStartBit, lBitLength, lIn);

	//Now move the bit position pointer up.
	lStartBit+=lBitLength;
}


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
	int STD_UTILS_PORT Std_GetTraceLevel()
	{return GetTraceLevel();}

	void STD_UTILS_PORT Std_SetTraceLevel(const int iVal)
	{SetTraceLevel(iVal);}

	void STD_UTILS_PORT Std_SetLogFilePrefix(string strFilePrefix)
	{
		if(GetTraceFilePrefix() != strFilePrefix)
			SetTraceFilePrefix(strFilePrefix.c_str());
	}

	string STD_UTILS_PORT Std_GetLogFilePrefix()
	{return GetTraceFilePrefix();}

	void STD_UTILS_PORT Std_SetLogLevel(const int iLevel)
	{SetTraceLevel(iLevel);}

	void STD_UTILS_PORT Std_LogMsg(const int iLevel, string strMessage, string strSourceFile, int iSourceLine, bool bPrintHeader)
	{
		string strFinalMessage;

		if(iSourceLine>=0)
			strFinalMessage = STR(strSourceFile) + " (" + STR(iSourceLine) + ") \r\n" + STR(strMessage) + "\r\n";
		else
			strFinalMessage = strMessage;

		Std_Log(iLevel, bPrintHeader, strFinalMessage.c_str());
	}

	void STD_UTILS_PORT Std_TraceMsg(const int iLevel, string strMessage, string strSourceFile, int iSourceLine, bool bLogToFile, bool bPrintHeader)
	{
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

unsigned long STD_UTILS_PORT Std_GetTick()
	{
	  struct _timeb stTime;
		_ftime( &stTime );
		return ((stTime.time*1000)+ stTime.millitm);
	}

#endif 

  
// Timing Functions
//***************************************************************************************************************

//***************************************************************************************************************
// File Functions

bool STD_UTILS_PORT Std_IsFullPath(string strPath)
{
	CStdArray<string> aryParts;
	int iCount = Std_Split(strPath, "\\", aryParts);
	if(iCount>1)
		return true;
	else
		return false;
}

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

// File Functions
//***************************************************************************************************************
class StdFixed;

void TestTree()

{
	tree<CStdFixed *> tr;
	tree<CStdFixed *>::iterator top, one;

	CStdFixed *lp1 = new CStdFixed();
	top=tr.begin();
	one=tr.insert(top, lp1);
	
	tr.clear();

	ostringstream str;
	kptree::ptr_print_tree_bracketed(tr, str);
	OutputDebugString(str.str().c_str());
}

