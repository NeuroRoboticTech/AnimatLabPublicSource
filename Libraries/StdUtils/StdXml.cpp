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
std::string CStdXml::FullTagPath(bool bAddChildName)
{
	int iSize = m_aryTagStack.size(), iIndex;
	std::string strTagPath, strVal;
	std::deque<std::string> aryTemp;

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
std::string CStdXml::ValueErrorString(std::string strValue)
{
	std::string strError;
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
std::string CStdXml::Serialize()
{return GetDoc();}

/**
\brief	Deserializes a string into an xml document.

\author	dcofer
\date	5/4/2011

\param [in,out]	strXml	The string xml. 
**/
void CStdXml::Deserialize(std::string &strXml)
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
bool CStdXml::FindElement(std::string strElementName, bool bThrowError)
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
bool CStdXml::FindChildElement(std::string strElementName, bool bThrowError)
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
bool CStdXml::IntoChildElement(std::string strElementName, bool bThrowError)
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
std::string CStdXml::GetChildString()
{return GetChildData();}

/**
\brief	Gets a string value from the element with the specified name.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the string element. 

\return	The child string.
**/
std::string CStdXml::GetChildString(std::string strElementName)
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
std::string CStdXml::GetChildString(std::string strElementName, std::string strDefault)
{
	std::string strVal;

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
	std::string strVal;

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
long CStdXml::GetChildLong(std::string strElementName)
{
	std::string strVal;

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
long CStdXml::GetChildLong(std::string strElementName, long lDefault)
{
	std::string strVal;
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
	std::string strVal;

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
int CStdXml::GetChildInt(std::string strElementName)
{
	std::string strVal;

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
int CStdXml::GetChildInt(std::string strElementName, int iDefault)
{
	std::string strVal;
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
	std::string strVal;

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
double CStdXml::GetChildDouble(std::string strElementName)
{
	std::string strVal;

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
double CStdXml::GetChildDouble(std::string strElementName, double dblDefault)
{
	std::string strVal;
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
	std::string strVal;

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
float CStdXml::GetChildFloat(std::string strElementName)
{
	std::string strVal;

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
float CStdXml::GetChildFloat(std::string strElementName, float fltDefault)
{
	std::string strVal;
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
	std::string strVal;

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
bool CStdXml::GetChildBool(std::string strElementName)
{
	std::string strVal;

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
bool CStdXml::GetChildBool(std::string strElementName, bool bDefault)
{
	std::string strVal;
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
void CStdXml::AddElement(std::string strElementName, std::string strData)
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
void CStdXml::AddChildElement(std::string strElementName, std::string strVal)
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
void CStdXml::AddChildElement(std::string strElementName)
{
	std::string strVal;
	AddChildElement(strElementName, strVal);
}

/**
\brief	Adds a child element to current element.

\author	dcofer
\date	5/4/2011

\param	strElementName	Name of the new element. 
\param	cVal		  	Data to add. 
**/
void CStdXml::AddChildElement(std::string strElementName, char cVal)
{
	std::ostringstream oStream;
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
void CStdXml::AddChildElement(std::string strElementName, unsigned char cVal)
{
	std::ostringstream oStream;
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
void CStdXml::AddChildElement(std::string strElementName, long lVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::AddChildElement(std::string strElementName, int iVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::AddChildElement(std::string strElementName, double dblVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::AddChildElement(std::string strElementName, float fltVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::AddChildElement(std::string strElementName, bool bVal)
{
	std::string strVal;

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
void CStdXml::AddChildCData(std::string strElementName, std::string strCData)
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
std::string CStdXml::GetAttribString(std::string strAttribName, bool bCanBeBlank, bool bThrowError, std::string strDefault)
{
	std::string strVal = GetAttrib(strAttribName.c_str());

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
long CStdXml::GetAttribLong(std::string strAttribName, bool bThrowError, long lDefault)
{
	std::string strVal;

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
int CStdXml::GetAttribInt(std::string strAttribName, bool bThrowError, int iDefault)
{
	std::string strVal;

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
double CStdXml::GetAttribDouble(std::string strAttribName, bool bThrowError, double dblDefault)
{
	std::string strVal;

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
float CStdXml::GetAttribFloat(std::string strAttribName, bool bThrowError, float fltDefault)
{
	std::string strVal;

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
bool CStdXml::GetAttribBool(std::string strAttribName, bool bThrowError, bool bDefault)
{
	std::string strVal;

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
void CStdXml::SetAttrib(std::string strAttribName, std::string strData)
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
void CStdXml::SetAttrib(std::string strAttribName, char cVal)
{
	std::ostringstream oStream;
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
void CStdXml::SetAttrib(std::string strAttribName, unsigned char cVal)
{
	std::ostringstream oStream;
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
void CStdXml::SetAttrib(std::string strAttribName, long lVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetAttrib(std::string strAttribName, int iVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetAttrib(std::string strAttribName, double dblVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetAttrib(std::string strAttribName, float fltVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetAttrib(std::string strAttribName, bool bVal)
{
	std::string strVal;

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
std::string CStdXml::GetChildAttribString(std::string strAttribName, bool bCanBeBlank, bool bThrowError, std::string strDefault)
{
	std::string strVal = GetChildAttrib(strAttribName.c_str());

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
long CStdXml::GetChildAttribLong(std::string strAttribName, bool bThrowError, long lDefault)
{
	std::string strVal;

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
int CStdXml::GetChildAttribInt(std::string strAttribName, bool bThrowError, int iDefault)
{
	std::string strVal;

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
double CStdXml::GetChildAttribDouble(std::string strAttribName, bool bThrowError, double dblDefault)
{
	std::string strVal;

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
float CStdXml::GetChildAttribFloat(std::string strAttribName, bool bThrowError, float fltDefault)
{
	std::string strVal;

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
bool CStdXml::GetChildAttribBool(std::string strAttribName, bool bThrowError, bool bDefault)
{
	std::string strVal;

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
void CStdXml::SetChildAttrib(std::string strAttribName, std::string strVal)
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
void CStdXml::SetChildAttrib(std::string strAttribName, char cVal)
{
	std::ostringstream oStream;
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
void CStdXml::SetChildAttrib(std::string strAttribName, unsigned char cVal)
{
	std::ostringstream oStream;
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
void CStdXml::SetChildAttrib(std::string strAttribName, long lVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetChildAttrib(std::string strAttribName, int iVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetChildAttrib(std::string strAttribName, double dblVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetChildAttrib(std::string strAttribName, float fltVal)
{
	std::ostringstream oStream;
	
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
void CStdXml::SetChildAttrib(std::string strAttribName, bool bVal)
{
	std::string strVal;

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
void CStdXml::AddChildDoc(std::string &strDoc)
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
std::string CStdXml::GetChildDoc()
{
	return GetChildSubDoc();
}

/**
\brief	Gets the parent tag name.

\author	dcofer
\date	5/4/2011

\return	The parent tag name.
**/
std::string CStdXml::GetParentTagName()
{
	std::string strTagName;

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
void CStdXml::Load(std::string strFilename)
{
	if(Std_IsBlank(strFilename))
		THROW_ERROR(Std_Err_lFilenameBlank, Std_Err_strFilenameBlank);

	if(!CMarkupSTL::Load(strFilename.c_str()))
	{
		if(!Std_IsBlank(m_strError))
		{
			std::string strError = STR("\nFilename: ") + strFilename + STR("\nReason: ") + m_strError;
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
void CStdXml::Save(std::string strFilename)
{
	if(Std_IsBlank(strFilename))
		THROW_ERROR(Std_Err_lFilenameBlank, Std_Err_strFilenameBlank);

	if(!CMarkupSTL::Save(strFilename.c_str()))
		THROW_PARAM_ERROR(Std_Err_lOpeningFile, Std_Err_strOpeningFile, "Filename", strFilename);
}

}				//StdUtils
