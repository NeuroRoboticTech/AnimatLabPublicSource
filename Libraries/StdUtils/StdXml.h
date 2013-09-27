/**
\file	StdXml.h

\brief	Declares the standard xml class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	A standard xml manipulation class.

\details This class is used to load, save, and manipulate xml data packets.

\author	dcofer
\date	5/4/2011
**/
class STD_UTILS_PORT CStdXml : public CMarkupSTL
{
protected:
	/// Stack of tags
	std::stack<std::string> m_aryTagStack;

	virtual void ClearTagStack();
	virtual std::string ValueErrorString(std::string strValue);

public:
	CStdXml();
	virtual ~CStdXml();

	virtual std::string Serialize();
	virtual void Deserialize(std::string &strXml);

	virtual bool IntoElem();
	virtual bool OutOfElem();
	virtual std::string FullTagPath(bool bAddChildName = true);

	virtual int NumberOfChildren();
	virtual bool FindElement(std::string strElementName, bool fThrowError = true);
	virtual bool FindChildByIndex(int iIndex, bool bThrowError = true);
	virtual bool FindChildElement(std::string strElementName, bool fThrowError = true);

	virtual bool IntoChildElement(std::string strElementName, bool bThrowError = true);

	virtual std::string GetChildString(std::string strElementName);
	virtual std::string GetChildString(std::string strElementName, std::string strDefault);
	virtual std::string GetChildString();
	virtual long GetChildLong(std::string strElementName);
	virtual long GetChildLong(std::string strElementName, long lDefault);
	virtual long GetChildLong();
	virtual int GetChildInt(std::string strElementName);
	virtual int GetChildInt(std::string strElementName, int iDefault);
	virtual int GetChildInt();
	virtual double GetChildDouble(std::string strElementName);
	virtual double GetChildDouble(std::string strElementName, double dblDefault);
	virtual double GetChildDouble();
	virtual float GetChildFloat(std::string strElementName);
	virtual float GetChildFloat(std::string strElementName, float fltDefault);
	virtual float GetChildFloat();
	virtual bool GetChildBool(std::string strElementName);
	virtual bool GetChildBool(std::string strElementName, bool bDefault);
	virtual bool GetChildBool();

	virtual void AddElement(std::string strElementName, std::string strData = "");
	//virtual bool RemoveElement(std::string strElementName, bool bThrowError = true);
	
	//Had to name the functions different here because bool was conflicting with int.
	virtual void AddChildElement(std::string strElementName);
	virtual void AddChildElement(std::string strElementName, std::string strVal);
	virtual void AddChildElement(std::string strElementName, char cVal);
	virtual void AddChildElement(std::string strElementName, unsigned char cVal);
	virtual void AddChildElement(std::string strElementName, long lVal);
	virtual void AddChildElement(std::string strElementName, int iVal);
	virtual void AddChildElement(std::string strElementName, double dblVal);
	virtual void AddChildElement(std::string strElementName, float fltVal);
	virtual void AddChildElement(std::string strElementName, bool bVal);

	virtual void AddChildCData(std::string strElementName, std::string strCData);

	virtual std::string GetAttribString(std::string strAttribName, bool bCanBeBlank = false, bool bThrowError = true, std::string strDefault = "");
	virtual long GetAttribLong(std::string strAttribName, bool bThrowError = true, long lDefault = 0);
	virtual int GetAttribInt(std::string strAttribName, bool bThrowError = true, int iDefault = 0);
	virtual double GetAttribDouble(std::string strAttribName, bool bThrowError = true, double dblDefault = 0);
	virtual float GetAttribFloat(std::string strAttribName, bool bThrowError = true, float fltDefault = 0);
	virtual bool GetAttribBool(std::string strAttribName, bool bThrowError = true, bool bDefault = false);

	virtual void SetAttrib(std::string strAttribName, std::string strVal);
	virtual void SetAttrib(std::string strAttribName, char cVal);
	virtual void SetAttrib(std::string strAttribName, unsigned char cVal);
	virtual void SetAttrib(std::string strAttribName, long lVal);
	virtual void SetAttrib(std::string strAttribName, int iVal);
	virtual void SetAttrib(std::string strAttribName, double dblVal);
	virtual void SetAttrib(std::string strAttribName, float fltVal);
	virtual void SetAttrib(std::string strAttribName, bool bVal);

	virtual std::string GetChildAttribString(std::string strAttribName, bool bCanBeBlank = false, bool bThrowError = true, std::string strDefault = "");
	virtual long GetChildAttribLong(std::string strAttribName, bool bThrowError = true, long lDefault = 0);
	virtual int GetChildAttribInt(std::string strAttribName, bool bThrowError = true, int iDefault = 0);
	virtual double GetChildAttribDouble(std::string strAttribName, bool bThrowError = true, double dblDefault = 0);
	virtual float GetChildAttribFloat(std::string strAttribName, bool bThrowError = true, float fltDefault = 0);
	virtual bool GetChildAttribBool(std::string strAttribName, bool bThrowError = true, bool bDefault = false);

	virtual void SetChildAttrib(std::string strAttribName, std::string strVal);
	virtual void SetChildAttrib(std::string strAttribName, char cVal);
	virtual void SetChildAttrib(std::string strAttribName, unsigned char cVal);
	virtual void SetChildAttrib(std::string strAttribName, long lVal);
	virtual void SetChildAttrib(std::string strAttribName, int iVal);
	virtual void SetChildAttrib(std::string strAttribName, double dblVal);
	virtual void SetChildAttrib(std::string strAttribName, float fltVal);
	virtual void SetChildAttrib(std::string strAttribName, bool bVal);

	virtual void AddChildDoc(std::string &strDoc);	
	virtual std::string GetChildDoc();
	virtual std::string GetParentTagName();

	virtual void Load(std::string strFilename);
	virtual void Save(std::string strFilename);
};

}				//StdUtils
