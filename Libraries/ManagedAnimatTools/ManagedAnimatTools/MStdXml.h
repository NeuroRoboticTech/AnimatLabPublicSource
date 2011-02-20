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
		public ref class StdXml
		{
		protected: 
			CStdXml *m_lpXml; 

		public: 
			StdXml(void);
			~StdXml(void);

			String ^Serialize();
			void Deserialize(String ^strXml);

			bool IntoElem();
			bool OutOfElem();
			String ^FullTagPath();
			String ^FullTagPath(bool bAddChildName);
			String ^TagName();
			String ^ChildTagName();

			int NumberOfChildren();
			bool FindElement(String ^strElementName);
			bool FindElement(String ^strElementName, bool bThrowError);
			bool FindChildByIndex(int iIndex);
			bool FindChildByIndex(int iIndex, bool bThrowError);
			bool FindChildElement(String ^strElementName);
			bool FindChildElement(String ^strElementName, bool bThrowError);

			bool IntoChildElement(String ^strElementName);
			bool IntoChildElement(String ^strElementName, bool bThrowError);

			String ^GetChildString(String ^strElementName);
			String ^GetChildString(String ^strElementName, String ^strDefault);
			String ^GetChildString();
			long GetChildLong(String ^strElementName);
			long GetChildLong(String ^strElementName, long lDefault);
			long GetChildLong();
			int GetChildInt(String ^strElementName);
			int GetChildInt(String ^strElementName, int iDefault);
			int GetChildInt();
			double GetChildDouble(String ^strElementName);
			double GetChildDouble(String ^strElementName, double dblDefault);
			double GetChildDouble();
			float GetChildFloat(String ^strElementName);
			float GetChildFloat(String ^strElementName, float fltDefault);
			float GetChildFloat();
			bool GetChildBool(String ^strElementName);
			bool GetChildBool(String ^strElementName, bool bDefault);
			bool GetChildBool();
			array<System::Byte>^ StdXml::GetChildByteArray(String ^strElementName);

			void AddElement(String ^strElementName);
			void AddElement(String ^strElementName, String ^strData);

			void AddChildElement(String ^strElementName);
			void AddChildElement(String ^strElementName, String ^strVal);
			void AddChildElement(String ^strElementName, char cVal);
			void AddChildElement(String ^strElementName, unsigned char cVal);
			void AddChildElement(String ^strElementName, Int32 lVal);
			void AddChildElement(String ^strElementName, Int16 iVal);
			void AddChildElement(String ^strElementName, double dblVal);
			void AddChildElement(String ^strElementName, float fltVal);
			void AddChildElement(String ^strElementName, bool bVal);
			void AddChildElement(String ^strElementName, array<System::Byte>^ aryData);

			void AddChildCData(String ^strElementName, String ^strCData);

			String ^GetAttribString(String ^strAttribName);
			String ^GetAttribString(String ^strAttribName, bool bCanBeBlank);
			String ^GetAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError);
			String ^GetAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError, String ^strDefault);

			long GetAttribLong(String ^strAttribName);
			long GetAttribLong(String ^strAttribName, bool bThrowError);
			long GetAttribLong(String ^strAttribName, bool bThrowError, long lDefault);

			int GetAttribInt(String ^strAttribName);
			int GetAttribInt(String ^strAttribName, bool bThrowError);
			int GetAttribInt(String ^strAttribName, bool bThrowError, int iDefault);

			double GetAttribDouble(String ^strAttribName);
			double GetAttribDouble(String ^strAttribName, bool bThrowError);
			double GetAttribDouble(String ^strAttribName, bool bThrowError, double dblDefault);

			float GetAttribFloat(String ^strAttribName);
			float GetAttribFloat(String ^strAttribName, bool bThrowError);
			float GetAttribFloat(String ^strAttribName, bool bThrowError, float fltDefault);

			bool GetAttribBool(String ^strAttribName);
			bool GetAttribBool(String ^strAttribName, bool bThrowError);
			bool GetAttribBool(String ^strAttribName, bool bThrowError, bool bDefault);

			void SetAttrib(String ^strAttribName, String ^strVal);
			void SetAttrib(String ^strAttribName, char cVal);
			void SetAttrib(String ^strAttribName, unsigned char cVal);
			void SetAttrib(String ^strAttribName, Int32 lVal);
			void SetAttrib(String ^strAttribName, Int16 iVal);
			void SetAttrib(String ^strAttribName, double dblVal);
			void SetAttrib(String ^strAttribName, float fltVal);
			void SetAttrib(String ^strAttribName, bool bVal);

			String ^GetChildAttribString(String ^strAttribName);
			String ^GetChildAttribString(String ^strAttribName, bool bCanBeBlank);
			String ^GetChildAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError);
			String ^GetChildAttribString(String ^strAttribName, bool bCanBeBlank, bool bThrowError, String ^strDefault);

			long GetChildAttribLong(String ^strAttribName);
			long GetChildAttribLong(String ^strAttribName, bool bThrowError);
			long GetChildAttribLong(String ^strAttribName, bool bThrowError, long lDefault);

			int GetChildAttribInt(String ^strAttribName);
			int GetChildAttribInt(String ^strAttribName, bool bThrowError);
			int GetChildAttribInt(String ^strAttribName, bool bThrowError, int iDefault);

			double GetChildAttribDouble(String ^strAttribName);
			double GetChildAttribDouble(String ^strAttribName, bool bThrowError);
			double GetChildAttribDouble(String ^strAttribName, bool bThrowError, double dblDefault);

			float GetChildAttribFloat(String ^strAttribName);
			float GetChildAttribFloat(String ^strAttribName, bool bThrowError);
			float GetChildAttribFloat(String ^strAttribName, bool bThrowError, float fltDefault);

			bool GetChildAttribBool(String ^strAttribName);
			bool GetChildAttribBool(String ^strAttribName, bool bThrowError);
			bool GetChildAttribBool(String ^strAttribName, bool bThrowError, bool bDefault);

			void SetChildAttrib(String ^strAttribName, String ^strVal);
			void SetChildAttrib(String ^strAttribName, char cVal);
			void SetChildAttrib(String ^strAttribName, unsigned char cVal);
			void SetChildAttrib(String ^strAttribName, Int32 lVal);
			void SetChildAttrib(String ^strAttribName, Int16 iVal);
			void SetChildAttrib(String ^strAttribName, double dblVal);
			void SetChildAttrib(String ^strAttribName, float fltVal);
			void SetChildAttrib(String ^strAttribName, bool bVal);

			void AddChildDoc(String ^Doc);	
			String ^StdXml::GetChildDoc();
			String ^GetParentTagName();

			void Load(String ^strFilename);
			void Save(String ^strFilename);
		};
	}
}
