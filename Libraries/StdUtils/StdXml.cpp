// StdXml.cpp: implementation of the CStdXml class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CStdXml::CStdXml()
{

}

CStdXml::~CStdXml()
{

}

bool CStdXml::IntoElem()
{
	m_aryTagStack.push(GetChildTagName());
	return CMarkupSTL::IntoElem();
}

bool CStdXml::OutOfElem()
{
	bool bVal = CMarkupSTL::OutOfElem();
	if(bVal) m_aryTagStack.pop();
	return bVal;
}

string CStdXml::FullTagPath(bool bAddChildName)
{
	int iSize = m_aryTagStack.size(), iIndex;
	string strTagPath, strVal;
	deque<string> aryTemp;

try
{
	for(iIndex=0; iIndex<iSize; iIndex++)
	{
		strVal = m_aryTagStack.top();

		if(iIndex<(iSize-1))
			strTagPath = "\\" + strVal + strTagPath;
		else
			strTagPath = strVal + strTagPath;

		aryTemp.push_front(strVal);
		m_aryTagStack.pop();
	}

	if(bAddChildName)
	{
		strVal = GetChildTagName();
		if( !strVal.empty() ) strTagPath += ("\\" + strVal);
	}

	for(iIndex=0; iIndex<iSize; iIndex++)
	{
		strVal = aryTemp[iIndex];
		m_aryTagStack.push(strVal);
	}

	return strTagPath;
}
catch(...)
{return "";}
}


string CStdXml::ValueErrorString(string strValue)
{
	string strError;
	strError = "  Element: " + FullTagPath() + "  Value: " + strValue;
	return strError;
}


void CStdXml::ClearTagStack()
{
	int iSize = m_aryTagStack.size(), iIndex;

	for(iIndex=0; iIndex<iSize; iIndex++)
		m_aryTagStack.pop();
}

string CStdXml::Serialize()
{return GetDoc();}

void CStdXml::Deserialize(string &strXml)
{
	if(!SetDoc(strXml.c_str()))
		THROW_ERROR(Std_Err_lDeserializingXml, Std_Err_strDeserializingXml);
}

BOOL CStdXml::FindElement(string strElementName, bool bThrowError)
{
	if(CMarkupSTL::FindElem(strElementName.c_str()))
	{
		ClearTagStack();
		m_aryTagStack.push(strElementName);

		return TRUE;
	}
	else if(bThrowError) 
		THROW_PARAM_ERROR(Std_Err_lElementNotFound, Std_Err_strElementNotFound, "Element", (FullTagPath(false) + "\\" + strElementName));

	return FALSE;
}


int CStdXml::NumberOfChildren()
{
	int iTotal = 0;

	ResetChildPos();
	while(FindChildElem())
		iTotal++;
	ResetChildPos();

	return iTotal;
}


BOOL CStdXml::FindChildByIndex(int iIndex, bool bThrowError)
{
	int iPos;

	ResetChildPos();

	for(iPos=0; iPos<=iIndex; iPos++)
	{
		if(!FindChildElem())
		{
			if(bThrowError)
				THROW_PARAM_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex, "Index", iIndex);
			else
				return FALSE;
		}
	}

	return TRUE;
}

BOOL CStdXml::FindChildElement(string strElementName, bool bThrowError)
{
	ResetChildPos();
	if(CMarkupSTL::FindChildElem(strElementName.c_str()))
		return TRUE;
	else if(bThrowError) 
		THROW_PARAM_ERROR(Std_Err_lElementNotFound, Std_Err_strElementNotFound, "Element", (FullTagPath(false) + "\\" + strElementName));

	return FALSE;
}


bool CStdXml::IntoChildElement(string strElementName, bool bThrowError)
{
	if(FindChildElement(strElementName, bThrowError))
		return IntoElem();

	return false;
}


string CStdXml::GetChildString()
{return GetChildData();}


string CStdXml::GetChildString(string strElementName)
{
	FindChildElement(strElementName);
	return GetChildData();
}

string CStdXml::GetChildString(string strElementName, string strDefault)
{
	string strVal;

	if(FindChildElement(strElementName, false))
		strVal = GetChildData();
	else
		strVal = strDefault;

	return strVal;
}

long CStdXml::GetChildLong()
{
	string strVal;

	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atol(strVal.c_str());
}


long CStdXml::GetChildLong(string strElementName)
{
	string strVal;

	FindChildElement(strElementName);
	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atol(strVal.c_str());
}

long CStdXml::GetChildLong(string strElementName, long lDefault)
{
	string strVal;
	long lVal=0;

	if(FindChildElement(strElementName, false))
	{
		strVal = GetChildData();
		strVal = Std_Trim(strVal);
		
		if(Std_IsNumeric(strVal) && Std_IsIntegerType(strVal))
			lVal = atol(strVal.c_str());
		else
			lVal = lDefault;
	}
	else
		lVal = lDefault;

	return lVal;
}

int CStdXml::GetChildInt()
{
	string strVal;

	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atoi(strVal.c_str());
}

int CStdXml::GetChildInt(string strElementName)
{
	string strVal;

	FindChildElement(strElementName);
	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atoi(strVal.c_str());
}

int CStdXml::GetChildInt(string strElementName, int iDefault)
{
	string strVal;
	int iVal=0;

	if(FindChildElement(strElementName, false))
	{
		strVal = GetChildData();
		strVal = Std_Trim(strVal);

		if(Std_IsNumeric(strVal) && Std_IsIntegerType(strVal))
			iVal = atoi(strVal.c_str());
		else
			iVal = iDefault;
	}
	else
		iVal = iDefault;

	return iVal;
}

double CStdXml::GetChildDouble()
{
	string strVal;

	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atof(strVal.c_str());
}

double CStdXml::GetChildDouble(string strElementName)
{
	string strVal;

	FindChildElement(strElementName);
	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atof(strVal.c_str());
}

double CStdXml::GetChildDouble(string strElementName, double dblDefault)
{
	string strVal;
	double dblVal=0;

	if(FindChildElement(strElementName, false))
	{
		strVal = GetChildData();
		strVal = Std_Trim(strVal);

		if(Std_IsNumeric(strVal))
			dblVal = atof(strVal.c_str());
		else
			dblVal = dblDefault;
	}
	else
		dblVal = dblDefault;

	return dblVal;
}

float CStdXml::GetChildFloat()
{
	string strVal;

	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return (float) atof(strVal.c_str());
}

float CStdXml::GetChildFloat(string strElementName)
{
	string strVal;

	FindChildElement(strElementName);
	strVal = GetChildData();
	strVal = Std_Trim(strVal);
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return (float) atof(strVal.c_str());
}

float CStdXml::GetChildFloat(string strElementName, float fltDefault)
{
	string strVal;
	float fltVal=0;

	if(FindChildElement(strElementName, false))
	{
		strVal = GetChildData();
		strVal = Std_Trim(strVal);

		if(Std_IsNumeric(strVal))
			fltVal = (float) atof(strVal.c_str());
		else
			fltVal = fltDefault;
	}
	else
		fltVal = fltDefault;

	return fltVal;
}


BOOL CStdXml::GetChildBool()
{
	string strVal;

	strVal = GetChildData();
	strVal = Std_CheckString(strVal);

	if(strVal == "TRUE" || strVal == "1")
		return TRUE;
	else if(strVal == "FALSE" || strVal == "0")
		return FALSE;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return FALSE;
}

BOOL CStdXml::GetChildBool(string strElementName)
{
	string strVal;

	FindChildElement(strElementName);
	strVal = GetChildData();
	strVal = Std_CheckString(strVal);

	if(strVal == "TRUE" || strVal == "1")
		return TRUE;
	else if(strVal == "FALSE" || strVal == "0")
		return FALSE;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return FALSE;
}

BOOL CStdXml::GetChildBool(string strElementName, bool bDefault)
{
	string strVal;
	BOOL bVal=FALSE;

	if(FindChildElement(strElementName, false))
	{
		strVal = GetChildData();
		strVal = Std_CheckString(strVal);

		if(strVal == "TRUE" || strVal == "1")
			return TRUE;
		else if(strVal == "FALSE" || strVal == "0")
			return FALSE;
		else
		{
			if(bDefault)
				bVal = TRUE;
			else
				bVal = FALSE;
		}
	}
	else
	{
		if(bDefault)
			bVal = TRUE;
		else
			bVal = FALSE;
	}

	return bVal;
}



void CStdXml::AddElement(string strElementName, string strData)
{
	if(!AddElem(strElementName.c_str(), strData.c_str()))
		THROW_PARAM_ERROR(Std_Err_lAddingElement, Std_Err_strAddingElement, "Element", (FullTagPath(false) + "\\" + strElementName));
}

void CStdXml::AddChildElement(string strElementName, string strVal)
{
	if(!AddChildElem(strElementName.c_str(), strVal.c_str()))
		THROW_PARAM_ERROR(Std_Err_lAddingElement, Std_Err_strAddingElement, "Element", (FullTagPath(false) + "\\" + strElementName));
}

void CStdXml::AddChildElement(string strElementName)
{
	string strVal;
	AddChildElement(strElementName, strVal);
}


void CStdXml::AddChildElement(string strElementName, char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;

	oStream << iVal; 
	AddChildElement(strElementName, oStream.str());
}

void CStdXml::AddChildElement(string strElementName, unsigned char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;
	
	oStream << iVal; 
	AddChildElement(strElementName, oStream.str());
}

void CStdXml::AddChildElement(string strElementName, long lVal)
{
	ostringstream oStream;
	
	oStream << lVal; 
	AddChildElement(strElementName, oStream.str());
}

void CStdXml::AddChildElement(string strElementName, int iVal)
{
	ostringstream oStream;
	
	oStream << iVal; 
	AddChildElement(strElementName, oStream.str());
}

void CStdXml::AddChildElement(string strElementName, double dblVal)
{
	ostringstream oStream;
	
	oStream << dblVal; 
	AddChildElement(strElementName, oStream.str());
}

void CStdXml::AddChildElement(string strElementName, float fltVal)
{
	ostringstream oStream;
	
	oStream << fltVal; 
	AddChildElement(strElementName, oStream.str());
}

void CStdXml::AddChildElement(string strElementName, bool bVal)
{
	string strVal;

	if(bVal)
		strVal = "True";
	else
		strVal = "False";

	AddChildElement(strElementName, strVal);
}

void CStdXml::AddChildCData(string strElementName, string strCData)
{
	AddChildElement(strElementName);
	IntoElem();
	SetData(strCData.c_str(), 1);
	OutOfElem();
}


string CStdXml::GetAttribString(string strAttribName, bool bCanBeBlank, bool bThrowError, string strDefault)
{
	string strVal = GetAttrib(strAttribName.c_str());

	if(Std_IsBlank(strVal) && !bCanBeBlank)
	{
		if(bThrowError)
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));

		strVal = strDefault;
	}

	return strVal;
}

long CStdXml::GetAttribLong(string strAttribName, bool bThrowError, long lDefault)
{
	string strVal;

	strVal = GetAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(Std_IsBlank(strVal))
	{
		if(bThrowError)
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));
		else
			return lDefault;
	}

	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atol(strVal.c_str());
}


int CStdXml::GetAttribInt(string strAttribName, bool bThrowError, int iDefault)
{
	string strVal;

	strVal = GetAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(Std_IsBlank(strVal))
	{
		if(bThrowError)
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));
		else
			return iDefault;
	}
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atoi(strVal.c_str());
}


double CStdXml::GetAttribDouble(string strAttribName, bool bThrowError, double dblDefault)
{
	string strVal;

	strVal = GetAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(Std_IsBlank(strVal))
	{
		if(bThrowError)
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));
		else
			return dblDefault;
	}
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atof(strVal.c_str());
}


float CStdXml::GetAttribFloat(string strAttribName, bool bThrowError, float fltDefault)
{
	string strVal;

	strVal = GetAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(Std_IsBlank(strVal))
	{
		if(bThrowError)
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));
		else
			return fltDefault;
	}
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return (float) atof(strVal.c_str());
}


BOOL CStdXml::GetAttribBool(string strAttribName, bool bThrowError, bool bDefault)
{
	string strVal;

	strVal = GetAttrib(strAttribName.c_str());
	strVal = Std_CheckString(strVal);

	if(Std_IsBlank(strVal))
	{
		if(bThrowError)
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));
		else
			return bDefault;
	}

	if(strVal == "TRUE" || strVal == "1")
		return TRUE;
	else if(strVal == "FALSE" || strVal == "0")
		return FALSE;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return FALSE;
}

void CStdXml::SetAttrib(string strAttribName, string strData)
{
	if(!CMarkupSTL::SetAttrib(strAttribName.c_str(), strData.c_str()))
		THROW_PARAM_ERROR(Std_Err_lSettingAttrib, Std_Err_strSettingAttrib, "Attrib", (FullTagPath(false) + "\\" + strAttribName));
}

void CStdXml::SetAttrib(string strAttribName, char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;

	oStream << iVal; 
	SetAttrib(strAttribName, oStream.str());
}

void CStdXml::SetAttrib(string strAttribName, unsigned char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;
	
	oStream << iVal; 
	SetAttrib(strAttribName, oStream.str());
}

void CStdXml::SetAttrib(string strAttribName, long lVal)
{
	ostringstream oStream;
	
	oStream << lVal; 
	SetAttrib(strAttribName, oStream.str());
}

void CStdXml::SetAttrib(string strAttribName, int iVal)
{
	ostringstream oStream;
	
	oStream << iVal; 
	SetAttrib(strAttribName, oStream.str());
}

void CStdXml::SetAttrib(string strAttribName, double dblVal)
{
	ostringstream oStream;
	
	oStream << dblVal; 
	SetAttrib(strAttribName, oStream.str());
}

void CStdXml::SetAttrib(string strAttribName, float fltVal)
{
	ostringstream oStream;
	
	oStream << fltVal; 
	SetAttrib(strAttribName, oStream.str());
}

void CStdXml::SetAttrib(string strAttribName, bool bVal)
{
	string strVal;

	if(bVal)
		strVal = "True";
	else
		strVal = "False";

	SetAttrib(strAttribName, strVal);
}



string CStdXml::GetChildAttribString(string strAttribName, bool bCanBeBlank, bool bThrowError, string strDefault)
{
	string strVal = GetChildAttrib(strAttribName.c_str());

	if(Std_IsBlank(strVal) && !bCanBeBlank)
	{
		if(bThrowError)
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));

		strVal = strDefault;
	}

	return strVal;
}


long CStdXml::GetChildAttribLong(string strAttribName, bool bThrowError, long lDefault)
{
	string strVal;

	strVal = GetChildAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(!bThrowError && Std_IsBlank(strVal))
		return lDefault;

	if(bThrowError && Std_IsBlank(strVal))
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));

	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atol(strVal.c_str());
}


int CStdXml::GetChildAttribInt(string strAttribName, bool bThrowError, int iDefault)
{
	string strVal;

	strVal = GetChildAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(!bThrowError && Std_IsBlank(strVal))
		return iDefault;
	
	if(bThrowError && Std_IsBlank(strVal))
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));

	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	if(!Std_IsIntegerType(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotIntegerType, Std_Err_strNotIntegerType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atoi(strVal.c_str());
}


double CStdXml::GetChildAttribDouble(string strAttribName, bool bThrowError, double dblDefault)
{
	string strVal;

	strVal = GetChildAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(!bThrowError && Std_IsBlank(strVal))
		return dblDefault;
	
	if(bThrowError && Std_IsBlank(strVal))
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return atof(strVal.c_str());
}


float CStdXml::GetChildAttribFloat(string strAttribName, bool bThrowError, float fltDefault)
{
	string strVal;

	strVal = GetChildAttrib(strAttribName.c_str());
	strVal = Std_Trim(strVal);

	if(!bThrowError && Std_IsBlank(strVal))
		return fltDefault;
	
	if(bThrowError && Std_IsBlank(strVal))
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));
	
	if(!Std_IsNumeric(strVal)) 
		THROW_TEXT_ERROR(Std_Err_lNotNumericType, Std_Err_strNotNumericType, ValueErrorString(strVal));

	//Need to add code here to verify it is a number.
	return (float) atof(strVal.c_str());
}


BOOL CStdXml::GetChildAttribBool(string strAttribName, bool bThrowError, bool bDefault)
{
	string strVal;

	strVal = GetChildAttrib(strAttribName.c_str());
	strVal = Std_CheckString(strVal);

	if(!bThrowError && Std_IsBlank(strVal))
		return bDefault;
	
	if(bThrowError && Std_IsBlank(strVal))
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));

	if(strVal == "TRUE" || strVal == "1")
		return TRUE;
	else if(strVal == "FALSE" || strVal == "0")
		return FALSE;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return FALSE;
}

void CStdXml::SetChildAttrib(string strAttribName, string strVal)
{
	if(!CMarkupSTL::SetChildAttrib(strAttribName.c_str(), strVal.c_str()))
		THROW_PARAM_ERROR(Std_Err_lSettingAttrib, Std_Err_strSettingAttrib, "Attrib", (FullTagPath(false) + "\\" + strAttribName));
}

void CStdXml::SetChildAttrib(string strAttribName, char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;

	oStream << iVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

void CStdXml::SetChildAttrib(string strAttribName, unsigned char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;
	
	oStream << iVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

void CStdXml::SetChildAttrib(string strAttribName, long lVal)
{
	ostringstream oStream;
	
	oStream << lVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

void CStdXml::SetChildAttrib(string strAttribName, int iVal)
{
	ostringstream oStream;
	
	oStream << iVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

void CStdXml::SetChildAttrib(string strAttribName, double dblVal)
{
	ostringstream oStream;
	
	oStream << dblVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

void CStdXml::SetChildAttrib(string strAttribName, float fltVal)
{
	ostringstream oStream;
	
	oStream << fltVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

void CStdXml::SetChildAttrib(string strAttribName, bool bVal)
{
	string strVal;

	if(bVal)
		strVal = "True";
	else
		strVal = "False";

	SetChildAttrib(strAttribName, strVal);
}



void CStdXml::AddChildDoc(string &strDoc)
{

	if(!AddChildSubDoc(strDoc.c_str()))
		THROW_ERROR(Std_Err_lAddingChildDoc, Std_Err_strAddingChildDoc);
}


string CStdXml::GetChildDoc()
{
	return GetChildSubDoc();
}


string CStdXml::GetParentTagName()
{
	string strTagName;

	OutOfElem();
	strTagName = GetTagName();
	IntoElem();
	return strTagName;
}



void CStdXml::Load(string strFilename)
{
	if(Std_IsBlank(strFilename))
		THROW_ERROR(Std_Err_lFilenameBlank, Std_Err_strFilenameBlank);

	if(!CMarkupSTL::Load(strFilename.c_str()))
	{
		if(!Std_IsBlank(m_strError))
		{
			string strError = STR("\nFilename: ") + strFilename + STR("\nReason: ") + m_strError;
			THROW_TEXT_ERROR(Std_Err_lOpeningFile, Std_Err_strOpeningFile, strError);
		}
		else
			THROW_PARAM_ERROR(Std_Err_lOpeningFile, Std_Err_strOpeningFile, "Filename", strFilename);

	}
}

void CStdXml::Save(string strFilename)
{
	if(Std_IsBlank(strFilename))
		THROW_ERROR(Std_Err_lFilenameBlank, Std_Err_strFilenameBlank);

	if(!CMarkupSTL::Save(strFilename.c_str()))
		THROW_PARAM_ERROR(Std_Err_lOpeningFile, Std_Err_strOpeningFile, "Filename", strFilename);
}
