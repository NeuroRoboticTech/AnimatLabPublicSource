#pragma once

using namespace System;

namespace ManagedAnimatSim
{   
	/// <summary> 
	/// Summary for StdXml
	/// </summary>
	public ref class StdXml : public ManagedAnimatInterfaces::IStdXml
	{
	protected: 
		CStdXml *m_lpXml; 
	
		ManagedAnimatInterfaces::ILogger ^m_lpLogger;
		

		virtual void LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel, System::String ^sMessage);

	public: 
		StdXml(void);
		~StdXml(void);

		virtual void SetLogger(ManagedAnimatInterfaces::ILogger ^lpLog);

		virtual System::String ^Serialize();
		virtual void Deserialize(String ^strXml);

		virtual bool IntoElem();
		virtual bool OutOfElem();
		virtual String ^FullTagPath();
		virtual String ^FullTagPath(bool bAddChildName);
		virtual String ^TagName();
		virtual String ^ChildTagName();

		virtual int NumberOfChildren();
		virtual bool FindElement(String ^strElementName);
		virtual bool FindElement(String ^strElementName, bool bThrowError);
		virtual bool FindChildByIndex(int iIndex);
		virtual bool FindChildByIndex(int iIndex, bool bThrowError);
		virtual bool FindChildElement(String ^strElementName);
		virtual bool FindChildElement(String ^strElementName, bool bThrowError);

		virtual bool IntoChildElement(String ^strElementName);
		virtual bool IntoChildElement(String ^strElementName, bool bThrowError);

		virtual String ^GetChildString(String ^strElementName);
		virtual String ^GetChildString(String ^strElementName, String ^strDefault);
		virtual String ^GetChildString();
		virtual System::Int64 GetChildLong(String ^strElementName);
		virtual System::Int64 GetChildLong(String ^strElementName, System::Int64 lDefault);
		virtual System::Int64 GetChildLong();
		virtual System::Int32 GetChildInt(String ^strElementName);
		virtual System::Int32 GetChildInt(String ^strElementName, System::Int32 iDefault);
		virtual System::Int32 GetChildInt();
		virtual double GetChildDouble(String ^strElementName);
		virtual double GetChildDouble(String ^strElementName, double dblDefault);
		virtual double GetChildDouble();
		virtual float GetChildFloat(String ^strElementName);
		virtual float GetChildFloat(String ^strElementName, float fltDefault);
		virtual float GetChildFloat();
		virtual bool GetChildBool(String ^strElementName);
		virtual bool GetChildBool(String ^strElementName, bool bDefault);
		virtual bool GetChildBool();
		virtual array<System::Byte>^ GetChildByteArray(String ^strElementName);

		virtual void AddElement(String ^strElementName);
		virtual void AddElement(String ^strElementName, String ^strData);

		virtual void AddChildElement(String ^strElementName);
		virtual void AddChildElement(String ^strElementName, String ^strVal);
		virtual void AddChildElement(String ^strElementName, char cVal);
		virtual void AddChildElement(String ^strElementName, unsigned char cVal);
		virtual void AddChildElement(String ^strElementName, System::Int64 lVal);
		virtual void AddChildElement(String ^strElementName, System::Int32 iVal);
		virtual void AddChildElement(String ^strElementName, double dblVal);
		virtual void AddChildElement(String ^strElementName, float fltVal);
		virtual void AddChildElement(String ^strElementName, bool bVal);
		virtual void AddChildElement(String ^strElementName, array<System::Byte>^ aryData);

		virtual void AddChildCData(String ^strElementName, String ^strCData);

		virtual String ^GetAttribString(String ^strAttribName);
		virtual String ^GetAttribString(String ^strAttribName, bool bCanBeBlank);
		virtual String ^GetAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError);
		virtual String ^GetAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError, String ^strDefault);

		virtual System::Int64 GetAttribLong(String ^strAttribName);
		virtual System::Int64 GetAttribLong(String ^strAttribName, bool bThrowError);
		virtual System::Int64 GetAttribLong(String ^strAttribName, bool bThrowError, System::Int64 lDefault);

		virtual System::Int32 GetAttribInt(String ^strAttribName);
		virtual System::Int32 GetAttribInt(String ^strAttribName, bool bThrowError);
		virtual System::Int32 GetAttribInt(String ^strAttribName, bool bThrowError, System::Int32 iDefault);

		virtual double GetAttribDouble(String ^strAttribName);
		virtual double GetAttribDouble(String ^strAttribName, bool bThrowError);
		virtual double GetAttribDouble(String ^strAttribName, bool bThrowError, double dblDefault);

		virtual float GetAttribFloat(String ^strAttribName);
		virtual float GetAttribFloat(String ^strAttribName, bool bThrowError);
		virtual float GetAttribFloat(String ^strAttribName, bool bThrowError, float fltDefault);

		virtual bool GetAttribBool(String ^strAttribName);
		virtual bool GetAttribBool(String ^strAttribName, bool bThrowError);
		virtual bool GetAttribBool(String ^strAttribName, bool bThrowError, bool bDefault);

		virtual void SetAttrib(String ^strAttribName, String ^strVal);
		virtual void SetAttrib(String ^strAttribName, char cVal);
		virtual void SetAttrib(String ^strAttribName, unsigned char cVal);
		virtual void SetAttrib(String ^strAttribName, System::Int64 lVal);
		virtual void SetAttrib(String ^strAttribName, System::Int32 iVal);
		virtual void SetAttrib(String ^strAttribName, double dblVal);
		virtual void SetAttrib(String ^strAttribName, float fltVal);
		virtual void SetAttrib(String ^strAttribName, bool bVal);

		virtual String ^GetChildAttribString(String ^strAttribName);
		virtual String ^GetChildAttribString(String ^strAttribName, bool bCanBeBlank);
		virtual String ^GetChildAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError);
		virtual String ^GetChildAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError, String ^strDefault);

		virtual System::Int64 GetChildAttribLong(String ^strAttribName);
		virtual System::Int64 GetChildAttribLong(String ^strAttribName, bool bThrowError);
		virtual System::Int64 GetChildAttribLong(String ^strAttribName, bool bThrowError, System::Int64 lDefault);

		virtual System::Int32 GetChildAttribInt(String ^strAttribName);
		virtual System::Int32 GetChildAttribInt(String ^strAttribName, bool bThrowError);
		virtual System::Int32 GetChildAttribInt(String ^strAttribName, bool bThrowError, System::Int32 iDefault);

		virtual double GetChildAttribDouble(String ^strAttribName);
		virtual double GetChildAttribDouble(String ^strAttribName, bool bThrowError);
		virtual double GetChildAttribDouble(String ^strAttribName, bool bThrowError, double dblDefault);

		virtual float GetChildAttribFloat(String ^strAttribName);
		virtual float GetChildAttribFloat(String ^strAttribName, bool bThrowError);
		virtual float GetChildAttribFloat(String ^strAttribName, bool bThrowError, float fltDefault);

		virtual bool GetChildAttribBool(String ^strAttribName);
		virtual bool GetChildAttribBool(String ^strAttribName, bool bThrowError);
		virtual bool GetChildAttribBool(String ^strAttribName, bool bThrowError, bool bDefault);

		virtual void SetChildAttrib(String ^strAttribName, String ^strVal);
		virtual void SetChildAttrib(String ^strAttribName, char cVal);
		virtual void SetChildAttrib(String ^strAttribName, unsigned char cVal);
		virtual void SetChildAttrib(String ^strAttribName, System::Int64 lVal);
		virtual void SetChildAttrib(String ^strAttribName, System::Int32 iVal);
		virtual void SetChildAttrib(String ^strAttribName, double dblVal);
		virtual void SetChildAttrib(String ^strAttribName, float fltVal);
		virtual void SetChildAttrib(String ^strAttribName, bool bVal);

		virtual void AddChildDoc(String ^Doc);	
		virtual String ^GetChildDoc();
		virtual String ^GetParentTagName();

		virtual void Load(String ^strFilename);
		virtual void Save(String ^strFilename);
	};

}
