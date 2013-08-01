/**
\file	StdXml.cpp

\brief	Implements the standard xml class.
**/

#include "StdAfx.h"

namespace StdUtils
{

/**
\brief	Default constructor.

\author	dcofer
\date	5/4/2011
**/
CStdXml::CStdXml()
{

}

/**
\brief	Destructor.

\author	dcofer
\date	5/4/2011
**/
CStdXml::~CStdXml()
{

}

/**
\brief	Goes into the next element where the cursor is located.

\author	dcofer
\date	5/4/2011

\return	true if it succeeds, false if it fails.
**/
bool CStdXml::IntoElem()
{
	m_aryTagStack.push(GetChildTagName());
	return CMarkupSTL::IntoElem();
}

/**
\brief	Goes out of the element where the cursor is located.

\author	dcofer
\date	5/4/2011

\return	true if it succeeds, false if it fails.
**/
bool CStdXml::OutOfElem()
{
	bool bVal = CMarkupSTL::OutOfElem();
	if(bVal) m_aryTagStack.pop();
	return bVal;
}

/**
\brief	Full tag path.

\author	dcofer
\date	5/4/2011

\param	bAddChildName	true to add child name. 

\return	.
**/
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

/**
\brief	Generates a value error string.

\author	dcofer
\date	5/4/2011

\param	strValue	The string value. 

\return	.
**/
string CStdXml::ValueErrorString(string strValue)
{
	string strError;
	strError = "  Element: " + FullTagPath() + "  Value: " + strValue;
	return strError;
}

/**
\brief	Clears the tag stack.

\author	dcofer
\date	5/4/2011
**/
void CStdXml::ClearTagStack()
{
	int iSize = m_aryTagStack.size(), iIndex;

	for(iIndex=0; iIndex<iSize; iIndex++)
		m_aryTagStack.pop();
}

/**
\brief	Serializes the document to a string.

\author	dcofer
\date	5/4/2011

\return	xml document string.
**/
string CStdXml::Serialize()
{return GetDoc();}

/**
\brief	Deserializes a string into an xml document.

\author	dcofer
\date	5/4/2011

\param [in,out]	strXml	The string xml. 
**/
void CStdXml::Deserialize(string &strXml)
{
	if(!SetDoc(strXml.c_str()))
		THROW_ERROR(Std_Err_lDeserializingXml, Std_Err_strDeserializingXml);
}

/**
\brief	Finds an element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	bThrowError   	true to throw error. 

\return	true if it succeeds, false if it fails.
**/
bool CStdXml::FindElement(string strElementName, bool bThrowError)
{
	if(CMarkupSTL::FindElem(strElementName.c_str()))
	{
		ClearTagStack();
		m_aryTagStack.push(strElementName);

		return true;
	}
	else if(bThrowError) 
		THROW_PARAM_ERROR(Std_Err_lElementNotFound, Std_Err_strElementNotFound, "Element", (FullTagPath(false) + "\\" + strElementName));

	return false;
}

/**
\brief	Gets the number of children of the current element.

\author	dcofer
\date	5/4/2011

\return	The total number of children.
**/
int CStdXml::NumberOfChildren()
{
	int iTotal = 0;

	ResetChildPos();
	while(FindChildElem())
		iTotal++;
	ResetChildPos();

	return iTotal;
}

/**
\brief	Finds a child element by index.

\author	dcofer
\date	5/4/2011

\param	iIndex	   	Zero-based index of the element. 
\param	bThrowError	true to throw error. 

\return	true if it succeeds, false if it fails.
**/
bool CStdXml::FindChildByIndex(int iIndex, bool bThrowError)
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
				return false;
		}
	}

	return true;
}

/**
\brief	Finds a child element by name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	bThrowError   	true to throw error. 

\return	true if it succeeds, false if it fails.
**/
bool CStdXml::FindChildElement(string strElementName, bool bThrowError)
{
	ResetChildPos();
	if(CMarkupSTL::FindChildElem(strElementName.c_str()))
		return true;
	else if(bThrowError) 
		THROW_PARAM_ERROR(Std_Err_lElementNotFound, Std_Err_strElementNotFound, "Element", (FullTagPath(false) + "\\" + strElementName));

	return false;
}

/**
\brief	Goes into the child element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	bThrowError   	true to throw error. 

\return	true if it succeeds, false if it fails.
**/
bool CStdXml::IntoChildElement(string strElementName, bool bThrowError)
{
	if(FindChildElement(strElementName, bThrowError))
		return IntoElem();

	return false;
}

/**
\brief	Gets a string value from the currently selected element.

\author	dcofer
\date	5/4/2011

\return	The child string.
**/
string CStdXml::GetChildString()
{return GetChildData();}

/**
\brief	Gets a string value from the element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 

\return	The child string.
**/
string CStdXml::GetChildString(string strElementName)
{
	FindChildElement(strElementName);
	return GetChildData();
}

/**
\brief	Gets a string value from the element with the specified name. It uses the default if no element is found.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	strDefault	  	The default value. 

\return	The child string.
**/
string CStdXml::GetChildString(string strElementName, string strDefault)
{
	string strVal;

	if(FindChildElement(strElementName, false))
		strVal = GetChildData();
	else
		strVal = strDefault;

	return strVal;
}

/**
\brief	Gets a long value from the currently selected element.

\author	dcofer
\date	5/4/2011

\return	The child long value.
**/
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

/**
\brief	Gets a long value from the element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 

\return	The child long value.
**/
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

/**
\brief	Gets a long value from the element with the specified name. It uses the default if no element is found.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	lDefault	  	The default value. 

\return	The child long value.
**/
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

/**
\brief	Gets an integer value from the currently selected element.

\author	dcofer
\date	5/4/2011

\return	The child integer value.
**/
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

/**
\brief	Gets an integer value from the element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 

\return	The child integer value.
**/
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

/**
\brief	Gets an integer value from the element with the specified name. It uses the default if no element is found.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	iDefault	  	The default value. 

\return	The child integer value.
**/
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

/**
\brief	Gets a double value from the currently selected element.

\author	dcofer
\date	5/4/2011

\return	The child double value.
**/
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

/**
\brief	Gets a double value from the element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 

\return	The child double value.
**/
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

/**
\brief	Gets a double value from the element with the specified name. It uses the default if no element is found.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	dblDefault	  	The double default value. 

\return	The child double value.
**/
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

/**
\brief	Gets a float value from the currently selected element.

\author	dcofer
\date	5/4/2011

\return	The child float value.
**/
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

/**
\brief	Gets a float value from the element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 

\return	The child float value.
**/
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

/**
\brief	Gets a float value from the element with the specified name. It uses the default if no element is found.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	fltDefault	  	The float default value. 

\return	The child float value.
**/
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

/**
\brief	Gets a bool value from the currently selected element.

\author	dcofer
\date	5/4/2011

\return	The child bool value.
**/
bool CStdXml::GetChildBool()
{
	string strVal;

	strVal = GetChildData();
	strVal = Std_CheckString(strVal);

	if(strVal == "TRUE" || strVal == "1")
		return true;
	else if(strVal == "FALSE" || strVal == "0")
		return false;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return false;
}

/**
\brief	Gets a bool value from the element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 

\return	The child bool value.
**/
bool CStdXml::GetChildBool(string strElementName)
{
	string strVal;

	FindChildElement(strElementName);
	strVal = GetChildData();
	strVal = Std_CheckString(strVal);

	if(strVal == "TRUE" || strVal == "1")
		return true;
	else if(strVal == "FALSE" || strVal == "0")
		return false;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return false;
}

/**
\brief	Gets a float value from the element with the specified name. It uses the default if no element is found.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	bDefault	  	bool default value. 

\return	The child bool value.
**/
bool CStdXml::GetChildBool(string strElementName, bool bDefault)
{
	string strVal;
	bool bVal=false;

	if(FindChildElement(strElementName, false))
	{
		strVal = GetChildData();
		strVal = Std_CheckString(strVal);

		if(strVal == "TRUE" || strVal == "1")
			return true;
		else if(strVal == "FALSE" || strVal == "0")
			return false;
		else
		{
			if(bDefault)
				bVal = true;
			else
				bVal = false;
		}
	}
	else
	{
		if(bDefault)
			bVal = true;
		else
			bVal = false;
	}

	return bVal;
}

/**
\brief	Adds an element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	strData		  	Data to add. 
**/
void CStdXml::AddElement(string strElementName, string strData)
{
	if(!AddElem(strElementName.c_str(), strData.c_str()))
		THROW_PARAM_ERROR(Std_Err_lAddingElement, Std_Err_strAddingElement, "Element", (FullTagPath(false) + "\\" + strElementName));
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	strData		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, string strVal)
{
	if(!AddChildElem(strElementName.c_str(), strVal.c_str()))
		THROW_PARAM_ERROR(Std_Err_lAddingElement, Std_Err_strAddingElement, "Element", (FullTagPath(false) + "\\" + strElementName));
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
**/
void CStdXml::AddChildElement(string strElementName)
{
	string strVal;
	AddChildElement(strElementName, strVal);
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	cVal		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;

	oStream << iVal; 
	AddChildElement(strElementName, oStream.str());
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	cVal		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, unsigned char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;
	
	oStream << iVal; 
	AddChildElement(strElementName, oStream.str());
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	lVal		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, long lVal)
{
	ostringstream oStream;
	
	oStream << lVal; 
	AddChildElement(strElementName, oStream.str());
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	iVal		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, int iVal)
{
	ostringstream oStream;
	
	oStream << iVal; 
	AddChildElement(strElementName, oStream.str());
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	dblVal		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, double dblVal)
{
	ostringstream oStream;
	
	oStream << dblVal; 
	AddChildElement(strElementName, oStream.str());
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	fltVal		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, float fltVal)
{
	ostringstream oStream;
	
	oStream << fltVal; 
	AddChildElement(strElementName, oStream.str());
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	bVal		  	Data to add. 
**/
void CStdXml::AddChildElement(string strElementName, bool bVal)
{
	string strVal;

	if(bVal)
		strVal = "True";
	else
		strVal = "False";

	AddChildElement(strElementName, strVal);
}

/**
\brief	Adds a child CDATA section

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 
\param	strCData	  	Information within the CDATA section. 
**/
void CStdXml::AddChildCData(string strElementName, string strCData)
{
	AddChildElement(strElementName);
	IntoElem();
	SetData(strCData.c_str(), 1);
	OutOfElem();
}

/**
\brief	Gets an attribute string of an element.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bCanBeBlank  	true if can be blank. 
\param	bThrowError  	true to throw error. 
\param	strDefault   	The string default. 

\return	The attribute string.
**/
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

/**
\brief	Gets an attribute long.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	lDefault	 	The default. 

\return	The attribute long.
**/
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

/**
\brief	Gets an attribute int.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	iDefault	 	The default. 

\return	The attribute int.
**/
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

/**
\brief	Gets an attribute double.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	dblDefault   	The double default. 

\return	The attribute double.
**/
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

/**
\brief	Gets an attribute float.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	fltDefault   	The flt default. 

\return	The attribute float.
**/
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

/**
\brief	Gets an attribute bool.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	bDefault	 	true to default. 

\return	The attribute bool.
**/
bool CStdXml::GetAttribBool(string strAttribName, bool bThrowError, bool bDefault)
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
		return true;
	else if(strVal == "FALSE" || strVal == "0")
		return false;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return false;
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	strData		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, string strData)
{
	if(!CMarkupSTL::SetAttrib(strAttribName.c_str(), strData.c_str()))
		THROW_PARAM_ERROR(Std_Err_lSettingAttrib, Std_Err_strSettingAttrib, "Attrib", (FullTagPath(false) + "\\" + strAttribName));
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	cVal		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;

	oStream << iVal; 
	SetAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	cVal		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, unsigned char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;
	
	oStream << iVal; 
	SetAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	lVal		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, long lVal)
{
	ostringstream oStream;
	
	oStream << lVal; 
	SetAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	iVal		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, int iVal)
{
	ostringstream oStream;
	
	oStream << iVal; 
	SetAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	dblVal		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, double dblVal)
{
	ostringstream oStream;
	
	oStream << dblVal; 
	SetAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	fltVal		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, float fltVal)
{
	ostringstream oStream;
	
	oStream << fltVal; 
	SetAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets an attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bVal		 	Data in the attribute. 
**/
void CStdXml::SetAttrib(string strAttribName, bool bVal)
{
	string strVal;

	if(bVal)
		strVal = "True";
	else
		strVal = "False";

	SetAttrib(strAttribName, strVal);
}

/**
\brief	Gets a child attribute string.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bCanBeBlank  	true if can be blank. 
\param	bThrowError  	true to throw error. 
\param	strDefault   	The string default. 

\return	The child attribute string.
**/
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

/**
\brief	Gets a child attribute long.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	lDefault	 	The default. 

\return	The child attribute long.
**/
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

/**
\brief	Gets a child attribute int.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	iDefault	 	The default. 

\return	The child attribute int.
**/
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

/**
\brief	Gets a child attribute double.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	dblDefault   	The double default. 

\return	The child attribute double.
**/
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

/**
\brief	Gets a child attribute float.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	fltDefault   	The flt default. 

\return	The child attribute float.
**/
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

/**
\brief	Gets a child attribute bool.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bThrowError  	true to throw error. 
\param	bDefault	 	true to default. 

\return	The child attribute bool.
**/
bool CStdXml::GetChildAttribBool(string strAttribName, bool bThrowError, bool bDefault)
{
	string strVal;

	strVal = GetChildAttrib(strAttribName.c_str());
	strVal = Std_CheckString(strVal);

	if(!bThrowError && Std_IsBlank(strVal))
		return bDefault;
	
	if(bThrowError && Std_IsBlank(strVal))
			THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, ValueErrorString(strVal));

	if(strVal == "TRUE" || strVal == "1")
		return true;
	else if(strVal == "FALSE" || strVal == "0")
		return false;
	else
		THROW_TEXT_ERROR(Std_Err_lNotBoolType, Std_Err_strNotBoolType, ValueErrorString(strVal));

	return false;
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	strVal		 	The string value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, string strVal)
{
	if(!CMarkupSTL::SetChildAttrib(strAttribName.c_str(), strVal.c_str()))
		THROW_PARAM_ERROR(Std_Err_lSettingAttrib, Std_Err_strSettingAttrib, "Attrib", (FullTagPath(false) + "\\" + strAttribName));
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	cVal		 	The value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;

	oStream << iVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	cVal		 	The value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, unsigned char cVal)
{
	ostringstream oStream;
	int iVal = (int) cVal;
	
	oStream << iVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	lVal		 	The value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, long lVal)
{
	ostringstream oStream;
	
	oStream << lVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	iVal		 	The value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, int iVal)
{
	ostringstream oStream;
	
	oStream << iVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	dblVal		 	The double value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, double dblVal)
{
	ostringstream oStream;
	
	oStream << dblVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	fltVal		 	The float value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, float fltVal)
{
	ostringstream oStream;
	
	oStream << fltVal; 
	SetChildAttrib(strAttribName, oStream.str());
}

/**
\brief	Sets a child attribute.

\author	dcofer
\date	5/4/2011

\param	strAttribName	Name of the string attribute. 
\param	bVal		 	The bool value. 
**/
void CStdXml::SetChildAttrib(string strAttribName, bool bVal)
{
	string strVal;

	if(bVal)
		strVal = "True";
	else
		strVal = "False";

	SetChildAttrib(strAttribName, strVal);
}

/**
\brief	Adds a child document. 

\author	dcofer
\date	5/4/2011

\param [in,out]	strDoc	The string document. 
**/
void CStdXml::AddChildDoc(string &strDoc)
{

	if(!AddChildSubDoc(strDoc.c_str()))
		THROW_ERROR(Std_Err_lAddingChildDoc, Std_Err_strAddingChildDoc);
}

/**
\brief	Gets a child document.

\author	dcofer
\date	5/4/2011

\return	The child document.
**/
string CStdXml::GetChildDoc()
{
	return GetChildSubDoc();
}

/**
\brief	Gets the parent tag name.

\author	dcofer
\date	5/4/2011

\return	The parent tag name.
**/
string CStdXml::GetParentTagName()
{
	string strTagName;

	OutOfElem();
	strTagName = GetTagName();
	IntoElem();
	return strTagName;
}

/**
\brief	Loads an xml data file.

\author	dcofer
\date	5/4/2011

\param	strFilename	The name of the file to load. 
**/
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

/**
\brief	Saves am xml data file.

\author	dcofer
\date	5/4/2011

\param	strFilename	The name of the file to save. 
**/
void CStdXml::Save(string strFilename)
{
	if(Std_IsBlank(strFilename))
		THROW_ERROR(Std_Err_lFilenameBlank, Std_Err_strFilenameBlank);

	if(!CMarkupSTL::Save(strFilename.c_str()))
		THROW_PARAM_ERROR(Std_Err_lOpeningFile, Std_Err_strOpeningFile, "Filename", strFilename);
}

}				//StdUtils
