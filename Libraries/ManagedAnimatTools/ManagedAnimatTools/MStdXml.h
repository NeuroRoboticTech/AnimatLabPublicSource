#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;

namespace AnimatGUI
{   
	namespace Interfaces
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
			virtual void Deserialize(System::String ^strXml);

			virtual bool IntoElem();
			virtual bool OutOfElem();
			virtual System::String ^FullTagPath();
			virtual System::String ^FullTagPath(bool bAddChildName);
			virtual System::String ^TagName();
			virtual System::String ^ChildTagName();

			virtual int NumberOfChildren();
			virtual bool FindElement(System::String ^strElementName);
			virtual bool FindElement(System::String ^strElementName, bool bThrowError);
			virtual bool FindChildByIndex(int iIndex);
			virtual bool FindChildByIndex(int iIndex, bool bThrowError);
			virtual bool FindChildElement(System::String ^strElementName);
			virtual bool FindChildElement(System::String ^strElementName, bool bThrowError);

			virtual bool IntoChildElement(System::String ^strElementName);
			virtual bool IntoChildElement(System::String ^strElementName, bool bThrowError);

			virtual System::String ^GetChildString(System::String ^strElementName);
			virtual System::String ^GetChildString(System::String ^strElementName, System::String ^strDefault);
			virtual System::String ^GetChildString();
			virtual System::Int64 GetChildLong(System::String ^strElementName);
			virtual System::Int64 GetChildLong(System::String ^strElementName, System::Int64 lDefault);
			virtual System::Int64 GetChildLong();
			virtual System::Int32 GetChildInt(System::String ^strElementName);
			virtual System::Int32 GetChildInt(System::String ^strElementName, System::Int32 iDefault);
			virtual System::Int32 GetChildInt();
			virtual double GetChildDouble(System::String ^strElementName);
			virtual double GetChildDouble(System::String ^strElementName, double dblDefault);
			virtual double GetChildDouble();
			virtual float GetChildFloat(System::String ^strElementName);
			virtual float GetChildFloat(System::String ^strElementName, float fltDefault);
			virtual float GetChildFloat();
			virtual bool GetChildBool(System::String ^strElementName);
			virtual bool GetChildBool(System::String ^strElementName, bool bDefault);
			virtual bool GetChildBool();
			virtual array<System::Byte>^ GetChildByteArray(System::String ^strElementName);

			virtual void AddElement(System::String ^strElementName);
			virtual void AddElement(System::String ^strElementName, System::String ^strData);

			virtual void AddChildElement(System::String ^strElementName);
			virtual void AddChildElement(System::String ^strElementName, System::String ^strVal);
			virtual void AddChildElement(System::String ^strElementName, char cVal);
			virtual void AddChildElement(System::String ^strElementName, unsigned char cVal);
			virtual void AddChildElement(System::String ^strElementName, System::Int64 lVal);
			virtual void AddChildElement(System::String ^strElementName, System::Int32 iVal);
			virtual void AddChildElement(System::String ^strElementName, double dblVal);
			virtual void AddChildElement(System::String ^strElementName, float fltVal);
			virtual void AddChildElement(System::String ^strElementName, bool bVal);
			virtual void AddChildElement(System::String ^strElementName, array<System::Byte>^ aryData);

			virtual void AddChildCData(System::String ^strElementName, System::String ^strCData);

			virtual System::String ^GetAttribString(System::String ^strAttribName);
			virtual System::String ^GetAttribString(System::String ^strAttribName, bool bCanBeBlank);
			virtual System::String ^GetAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError);
			virtual System::String ^GetAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError, System::String ^strDefault);

			virtual System::Int64 GetAttribLong(System::String ^strAttribName);
			virtual System::Int64 GetAttribLong(System::String ^strAttribName, bool bThrowError);
			virtual System::Int64 GetAttribLong(System::String ^strAttribName, bool bThrowError, System::Int64 lDefault);

			virtual System::Int32 GetAttribInt(System::String ^strAttribName);
			virtual System::Int32 GetAttribInt(System::String ^strAttribName, bool bThrowError);
			virtual System::Int32 GetAttribInt(System::String ^strAttribName, bool bThrowError, System::Int32 iDefault);

			virtual double GetAttribDouble(System::String ^strAttribName);
			virtual double GetAttribDouble(System::String ^strAttribName, bool bThrowError);
			virtual double GetAttribDouble(System::String ^strAttribName, bool bThrowError, double dblDefault);

			virtual float GetAttribFloat(System::String ^strAttribName);
			virtual float GetAttribFloat(System::String ^strAttribName, bool bThrowError);
			virtual float GetAttribFloat(System::String ^strAttribName, bool bThrowError, float fltDefault);

			virtual bool GetAttribBool(System::String ^strAttribName);
			virtual bool GetAttribBool(System::String ^strAttribName, bool bThrowError);
			virtual bool GetAttribBool(System::String ^strAttribName, bool bThrowError, bool bDefault);

			virtual void SetAttrib(System::String ^strAttribName, System::String ^strVal);
			virtual void SetAttrib(System::String ^strAttribName, char cVal);
			virtual void SetAttrib(System::String ^strAttribName, unsigned char cVal);
			virtual void SetAttrib(System::String ^strAttribName, System::Int64 lVal);
			virtual void SetAttrib(System::String ^strAttribName, System::Int32 iVal);
			virtual void SetAttrib(System::String ^strAttribName, double dblVal);
			virtual void SetAttrib(System::String ^strAttribName, float fltVal);
			virtual void SetAttrib(System::String ^strAttribName, bool bVal);

			virtual System::String ^GetChildAttribString(System::String ^strAttribName);
			virtual System::String ^GetChildAttribString(System::String ^strAttribName, bool bCanBeBlank);
			virtual System::String ^GetChildAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError);
			virtual System::String ^GetChildAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError, System::String ^strDefault);

			virtual System::Int64 GetChildAttribLong(System::String ^strAttribName);
			virtual System::Int64 GetChildAttribLong(System::String ^strAttribName, bool bThrowError);
			virtual System::Int64 GetChildAttribLong(System::String ^strAttribName, bool bThrowError, System::Int64 lDefault);

			virtual System::Int32 GetChildAttribInt(System::String ^strAttribName);
			virtual System::Int32 GetChildAttribInt(System::String ^strAttribName, bool bThrowError);
			virtual System::Int32 GetChildAttribInt(System::String ^strAttribName, bool bThrowError, System::Int32 iDefault);

			virtual double GetChildAttribDouble(System::String ^strAttribName);
			virtual double GetChildAttribDouble(System::String ^strAttribName, bool bThrowError);
			virtual double GetChildAttribDouble(System::String ^strAttribName, bool bThrowError, double dblDefault);

			virtual float GetChildAttribFloat(System::String ^strAttribName);
			virtual float GetChildAttribFloat(System::String ^strAttribName, bool bThrowError);
			virtual float GetChildAttribFloat(System::String ^strAttribName, bool bThrowError, float fltDefault);

			virtual bool GetChildAttribBool(System::String ^strAttribName);
			virtual bool GetChildAttribBool(System::String ^strAttribName, bool bThrowError);
			virtual bool GetChildAttribBool(System::String ^strAttribName, bool bThrowError, bool bDefault);

			virtual void SetChildAttrib(System::String ^strAttribName, System::String ^strVal);
			virtual void SetChildAttrib(System::String ^strAttribName, char cVal);
			virtual void SetChildAttrib(System::String ^strAttribName, unsigned char cVal);
			virtual void SetChildAttrib(System::String ^strAttribName, System::Int64 lVal);
			virtual void SetChildAttrib(System::String ^strAttribName, System::Int32 iVal);
			virtual void SetChildAttrib(System::String ^strAttribName, double dblVal);
			virtual void SetChildAttrib(System::String ^strAttribName, float fltVal);
			virtual void SetChildAttrib(System::String ^strAttribName, bool bVal);

			virtual void AddChildDoc(System::String ^Doc);	
			virtual System::String ^GetChildDoc();
			virtual System::String ^GetParentTagName();

			virtual void Load(System::String ^strFilename);
			virtual void Save(System::String ^strFilename);
		};
	}
}
