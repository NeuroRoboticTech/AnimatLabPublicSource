#include "StdAfx.h"
#include "Util.h"
#include "MStdXml.h"

#using <mscorlib.dll>
using namespace System;

namespace AnimatGUI
{
	namespace Interfaces
	{
		StdXml::StdXml(void)
		{
			m_lpXml = new CStdXml;
		}

		StdXml::~StdXml(void)
		{
			if(m_lpXml)
				delete m_lpXml;
		}

		System::String ^StdXml::Serialize()
		{
			try
			{
				return gcnew String(m_lpXml->Serialize().c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return nullptr;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while attempting to Serialize.";
				throw gcnew System::Exception(strError);
				return nullptr;
			}
		}

		void StdXml::Deserialize(System::String ^strXml)
		{
			try
			{
				string strData = Util::StringToStd(strXml);
				m_lpXml->Deserialize(strData);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while attempting to Deserialize: " + strXml;
				throw gcnew System::Exception(strError);
			}
		}

		bool StdXml::IntoElem()
		{
			try
			{
				return m_lpXml->IntoElem();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling IntoElem.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::OutOfElem()
		{
			try
			{
				return m_lpXml->OutOfElem();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling OutOfElem.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		System::String ^StdXml::FullTagPath()
		{return FullTagPath(true);}

		System::String ^StdXml::FullTagPath(bool bAddChildName)
		{
			try
			{
				return gcnew String(m_lpXml->FullTagPath(bAddChildName).c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return nullptr;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling FullTagPath.";
				throw gcnew System::Exception(strError);
				return nullptr;
			}
		}

		System::String ^StdXml::TagName()
		{
			try
			{
				string strVal = m_lpXml->GetTagName();
				return gcnew String(strVal.c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling TagName.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		System::String ^StdXml::ChildTagName()
		{
			try
			{
				string strVal = m_lpXml->GetChildTagName();
				return gcnew String(strVal.c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling ChildTagName.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		int StdXml::NumberOfChildren()
		{
			try
			{
				return m_lpXml->NumberOfChildren();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling NumberOfChildren.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::FindElement(System::String ^strElementName)
		{return FindElement(strElementName, true);}

		bool StdXml::FindElement(System::String ^strElementName, bool bThrowError)
		{
			try
			{
				return m_lpXml->FindElement(Util::StringToStd(strElementName), bThrowError);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling FindElement.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::FindChildByIndex(int iIndex)
		{return FindChildByIndex(iIndex, true);}

		bool StdXml::FindChildByIndex(int iIndex, bool bThrowError)
		{
			try
			{
				return m_lpXml->FindChildByIndex(iIndex, bThrowError);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling FindChildByIndex.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::FindChildElement(System::String ^strElementName)
		{return FindChildElement(strElementName, true);}

		bool StdXml::FindChildElement(System::String ^strElementName, bool bThrowError)
		{
			try
			{
				return m_lpXml->FindChildElement(Util::StringToStd(strElementName), bThrowError);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling FindChildElement.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::IntoChildElement(System::String ^strElementName)
		{return IntoChildElement(strElementName, true);}
		
		bool StdXml::IntoChildElement(System::String ^strElementName, bool bThrowError)
		{
			try
			{
				return m_lpXml->IntoChildElement(Util::StringToStd(strElementName), bThrowError);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling IntoChildElement.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		System::String ^StdXml::GetChildString(System::String ^strElementName)
		{
			try
			{
				return gcnew String(m_lpXml->GetChildString(Util::StringToStd(strElementName)).c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildString.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		System::String ^StdXml::GetChildString(System::String ^strElementName, System::String ^strDefault)
		{
			try
			{
				return gcnew String(m_lpXml->GetChildString(Util::StringToStd(strElementName), Util::StringToStd(strDefault)).c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildString.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		System::String ^StdXml::GetChildString()
		{
			try
			{
				return gcnew String(m_lpXml->GetChildString().c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildString.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		long StdXml::GetChildLong(System::String ^strElementName)
		{
			try
			{
				return m_lpXml->GetChildLong(Util::StringToStd(strElementName));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildLong.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}
		
		long StdXml::GetChildLong(System::String ^strElementName, long lDefault)
		{
			try
			{
				return m_lpXml->GetChildLong(Util::StringToStd(strElementName), lDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildLong.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		long StdXml::GetChildLong()
		{
			try
			{
				return m_lpXml->GetChildLong();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildLong.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		int StdXml::GetChildInt(System::String ^strElementName)
		{
			try
			{
				return m_lpXml->GetChildInt(Util::StringToStd(strElementName));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildInt.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		int StdXml::GetChildInt(System::String ^strElementName, int iDefault)
		{
			try
			{
				return m_lpXml->GetChildInt(Util::StringToStd(strElementName), iDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildInt.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		int StdXml::GetChildInt()
		{
			try
			{
				return m_lpXml->GetChildInt();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildInt.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		double StdXml::GetChildDouble(System::String ^strElementName)
		{
			try
			{
				return m_lpXml->GetChildDouble(Util::StringToStd(strElementName));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildDouble.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		double StdXml::GetChildDouble(System::String ^strElementName, double dblDefault)
		{
			try
			{
				return m_lpXml->GetChildDouble(Util::StringToStd(strElementName), dblDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildDouble.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		double StdXml::GetChildDouble()
		{
			try
			{
				return m_lpXml->GetChildDouble();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildDouble.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		float StdXml::GetChildFloat(System::String ^strElementName)
		{
			try
			{
				return m_lpXml->GetChildFloat(Util::StringToStd(strElementName));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildFloat.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		float StdXml::GetChildFloat(System::String ^strElementName, float fltDefault)
		{
			try
			{
				return m_lpXml->GetChildFloat(Util::StringToStd(strElementName), fltDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildFloat.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		float StdXml::GetChildFloat()
		{
			try
			{
				return m_lpXml->GetChildFloat();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildFloat.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::GetChildBool(System::String ^strElementName)
		{
			try
			{
				return m_lpXml->GetChildBool(Util::StringToStd(strElementName));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildBool.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::GetChildBool(System::String ^strElementName, bool bDefault)
		{
			try
			{
				return m_lpXml->GetChildBool(Util::StringToStd(strElementName), bDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildBool.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::GetChildBool()
		{
			try
			{
				return m_lpXml->GetChildBool();
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildBool.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		array<System::Byte>^ StdXml::GetChildByteArray(System::String ^strElementName)
		{

			try
			{
				CStdArray<unsigned char> aryBytes;
				string strHex = m_lpXml->GetChildString(Util::StringToStd(strElementName));
				long lByteCount = m_lpXml->GetChildAttribLong("ByteCount");

				if(strHex.length() == 0)
					return gcnew array<System::Byte>(0);
				else
				{
					Std_HexStringToByteArray(strHex, aryBytes); 
					if(aryBytes.GetSize() != lByteCount)
						THROW_TEXT_ERROR(Std_Err_lByteCountMismatch, Std_Err_strByteCountMismatch, 
						                ("Xml ByteCount: " + STR(lByteCount) + " Length: " + STR(aryBytes.GetSize())));

					array<System::Byte> ^aryData = gcnew array<System::Byte>(lByteCount);
					long lIndex;
					for(lIndex=0; lIndex<lByteCount; lIndex++)
						aryData[lIndex] = (System::Byte) aryBytes[lIndex];

					return aryData;
				}
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return nullptr;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildByteArray.";
				throw gcnew System::Exception(strError);
				return nullptr;
			}
		}

		void StdXml::AddElement(System::String ^strElementName)
		{AddElement(strElementName, "");}

		void StdXml::AddElement(System::String ^strElementName, System::String ^strData)
		{
			try
			{
				m_lpXml->AddElement(Util::StringToStd(strElementName), Util::StringToStd(strData));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, System::String ^strVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), Util::StringToStd(strVal));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, char cVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), cVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, unsigned char cVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), cVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, Int32 lVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), lVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, Int16 iVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), iVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, double dblVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), dblVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, float fltVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), fltVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, bool bVal)
		{
			try
			{
				m_lpXml->AddChildElement(Util::StringToStd(strElementName), bVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildElement(System::String ^strElementName, array<System::Byte>^ aryData)
		{

			try
			{
				CStdArray<unsigned char> aryBytes;

				long lIndex;
				long lLength = aryData->Length;
				for(lIndex=0; lIndex<lLength; lIndex++)
				{
					unsigned char iVal = (unsigned char) aryData[lIndex];
					aryBytes.Add(iVal);
				}

				string strHex = Std_ByteArrayToHexString(aryBytes);

				m_lpXml->AddChildElement(Util::StringToStd(strElementName), strHex);
				m_lpXml->SetChildAttrib("ByteCount", lLength);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildElement.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildCData(System::String ^strElementName, System::String ^strCData)
		{
			try
			{
				m_lpXml->AddChildCData(Util::StringToStd(strElementName), Util::StringToStd(strCData));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildCData.";
				throw gcnew System::Exception(strError);
			}
		}

		System::String ^StdXml::GetAttribString(System::String ^strAttribName)
		{return GetAttribString(strAttribName, false, true, "");}

		System::String ^StdXml::GetAttribString(System::String ^strAttribName, bool bCanBeBlank)
		{return GetAttribString(strAttribName, bCanBeBlank, true, "");}

		System::String ^StdXml::GetAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError)
		{return GetAttribString(strAttribName, bCanBeBlank, bThrowError, "");}

		System::String ^StdXml::GetAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError, System::String ^strDefault)
		{
			try
			{
				return gcnew String(m_lpXml->GetAttribString(Util::StringToStd(strAttribName), bCanBeBlank, bThrowError, Util::StringToStd(strDefault)).c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetAttribString.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		long StdXml::GetAttribLong(System::String ^strAttribName)
		{return GetAttribLong(strAttribName, true, 0);}

		long StdXml::GetAttribLong(System::String ^strAttribName, bool bThrowError)
		{return GetAttribLong(strAttribName, bThrowError, 0);}

		long StdXml::GetAttribLong(System::String ^strAttribName, bool bThrowError, long lDefault)
		{
			try
			{
				return m_lpXml->GetAttribLong(Util::StringToStd(strAttribName), bThrowError, lDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetAttribLong.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		int StdXml::GetAttribInt(System::String ^strAttribName)
		{return GetAttribInt(strAttribName, true, 0);}

		int StdXml::GetAttribInt(System::String ^strAttribName, bool bThrowError)
		{return GetAttribInt(strAttribName, bThrowError, 0);}

		int StdXml::GetAttribInt(System::String ^strAttribName, bool bThrowError, int iDefault)
		{
			try
			{
				return m_lpXml->GetAttribInt(Util::StringToStd(strAttribName), bThrowError, iDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetAttribInt.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		double StdXml::GetAttribDouble(System::String ^strAttribName)
		{return GetAttribDouble(strAttribName, true, 0);}

		double StdXml::GetAttribDouble(System::String ^strAttribName, bool bThrowError)
		{return GetAttribDouble(strAttribName, bThrowError, 0);}

		double StdXml::GetAttribDouble(System::String ^strAttribName, bool bThrowError, double dblDefault)
		{
			try
			{
				return m_lpXml->GetAttribDouble(Util::StringToStd(strAttribName), bThrowError, dblDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetAttribDouble.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		float StdXml::GetAttribFloat(System::String ^strAttribName)
		{return GetAttribFloat(strAttribName, true, 0);}

		float StdXml::GetAttribFloat(System::String ^strAttribName, bool bThrowError)
		{return GetAttribFloat(strAttribName, bThrowError, 0);}

		float StdXml::GetAttribFloat(System::String ^strAttribName, bool bThrowError, float fltDefault)
		{
			try
			{
				return m_lpXml->GetAttribFloat(Util::StringToStd(strAttribName), bThrowError, fltDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetAttribFloat.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::GetAttribBool(System::String ^strAttribName)
		{return GetAttribBool(strAttribName, true, 0);}

		bool StdXml::GetAttribBool(System::String ^strAttribName, bool bThrowError)
		{return GetAttribBool(strAttribName, bThrowError, 0);}

		bool StdXml::GetAttribBool(System::String ^strAttribName, bool bThrowError, bool bDefault)
		{
			try
			{
				return m_lpXml->GetAttribBool(Util::StringToStd(strAttribName), bThrowError, bDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetAttribBool.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, System::String ^strVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), Util::StringToStd(strVal));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, char cVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), cVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, unsigned char cVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), cVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, Int32 lVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), lVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, Int16 iVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), iVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, double dblVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), dblVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, float fltVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), fltVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetAttrib(System::String ^strAttribName, bool bVal)
		{
			try
			{
				m_lpXml->SetAttrib(Util::StringToStd(strAttribName), bVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		System::String ^StdXml::GetChildAttribString(System::String ^strAttribName)
		{return GetChildAttribString(strAttribName, false, true, "");}

		System::String ^StdXml::GetChildAttribString(System::String ^strAttribName, bool bCanBeBlank)
		{return GetChildAttribString(strAttribName, bCanBeBlank, true, "");}

		System::String ^StdXml::GetChildAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError)
		{return GetChildAttribString(strAttribName, bCanBeBlank, bThrowError, "");}

		System::String ^StdXml::GetChildAttribString(System::String ^strAttribName, bool bCanBeBlank, bool bThrowError, System::String ^strDefault)
		{
			try
			{
				return gcnew String(m_lpXml->GetChildAttribString(Util::StringToStd(strAttribName), bCanBeBlank, bThrowError, Util::StringToStd(strDefault)).c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildAttribString.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		long StdXml::GetChildAttribLong(System::String ^strAttribName)
		{return GetChildAttribLong(strAttribName, true, 0);}

		long StdXml::GetChildAttribLong(System::String ^strAttribName, bool bThrowError)
		{return GetChildAttribLong(strAttribName, bThrowError, 0);}

		long StdXml::GetChildAttribLong(System::String ^strAttribName, bool bThrowError, long lDefault)
		{
			try
			{
				return m_lpXml->GetChildAttribLong(Util::StringToStd(strAttribName), bThrowError, lDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildAttribLong.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		int StdXml::GetChildAttribInt(System::String ^strAttribName)
		{return GetChildAttribInt(strAttribName, true, 0);}

		int StdXml::GetChildAttribInt(System::String ^strAttribName, bool bThrowError)
		{return GetChildAttribInt(strAttribName, bThrowError, 0);}

		int StdXml::GetChildAttribInt(System::String ^strAttribName, bool bThrowError, int iDefault)
		{
			try
			{
				return m_lpXml->GetChildAttribInt(Util::StringToStd(strAttribName), bThrowError, iDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildAttribInt.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		double StdXml::GetChildAttribDouble(System::String ^strAttribName)
		{return GetChildAttribDouble(strAttribName, true, 0);}

		double StdXml::GetChildAttribDouble(System::String ^strAttribName, bool bThrowError)
		{return GetChildAttribDouble(strAttribName, bThrowError, 0);}

		double StdXml::GetChildAttribDouble(System::String ^strAttribName, bool bThrowError, double dblDefault)
		{
			try
			{
				return m_lpXml->GetChildAttribDouble(Util::StringToStd(strAttribName), bThrowError, dblDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildAttribDouble.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		float StdXml::GetChildAttribFloat(System::String ^strAttribName)
		{return GetChildAttribFloat(strAttribName, true, 0);}

		float StdXml::GetChildAttribFloat(System::String ^strAttribName, bool bThrowError)
		{return GetChildAttribFloat(strAttribName, bThrowError, 0);}

		float StdXml::GetChildAttribFloat(System::String ^strAttribName, bool bThrowError, float fltDefault)
		{
			try
			{
				return m_lpXml->GetChildAttribFloat(Util::StringToStd(strAttribName), bThrowError, fltDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildAttribFloat.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		bool StdXml::GetChildAttribBool(System::String ^strAttribName)
		{return GetChildAttribBool(strAttribName, true, 0);}

		bool StdXml::GetChildAttribBool(System::String ^strAttribName, bool bThrowError)
		{return GetChildAttribBool(strAttribName, bThrowError, 0);}

		bool StdXml::GetChildAttribBool(System::String ^strAttribName, bool bThrowError, bool bDefault)
		{
			try
			{
				return m_lpXml->GetChildAttribBool(Util::StringToStd(strAttribName), bThrowError, bDefault);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return NULL;
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildAttribBool.";
				throw gcnew System::Exception(strError);
				return NULL;
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, System::String ^strVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), Util::StringToStd(strVal));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, char cVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), cVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, unsigned char cVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), cVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, Int32 lVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), lVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, Int16 iVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), iVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, double dblVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), dblVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, float fltVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), fltVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::SetChildAttrib(System::String ^strAttribName, bool bVal)
		{
			try
			{
				m_lpXml->SetChildAttrib(Util::StringToStd(strAttribName), bVal);
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling SetChildAttrib.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::AddChildDoc(System::String ^Doc)
		{
			try
			{
				string strDoc = Util::StringToStd(Doc);
				m_lpXml->AddChildDoc(strDoc);
				Doc = gcnew String(strDoc.c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling AddChildDoc.";
				throw gcnew System::Exception(strError);
			}
		}

		System::String ^StdXml::GetChildDoc()
		{
			try
			{
				string strDoc = m_lpXml->GetChildDoc();
				System::String ^strVal = gcnew String(strDoc.c_str());
				return strVal;
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetChildDoc.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		System::String ^StdXml::GetParentTagName()
		{
			try
			{
				return gcnew String(m_lpXml->GetParentTagName().c_str());
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
				return "";
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling GetParentTagName.";
				throw gcnew System::Exception(strError);
				return "";
			}
		}

		void StdXml::Load(System::String ^strFilename)
		{
			try
			{
				m_lpXml->Load(Util::StringToStd(strFilename));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling Load.";
				throw gcnew System::Exception(strError);
			}
		}

		void StdXml::Save(System::String ^strFilename)
		{
			try
			{
				m_lpXml->Save(Util::StringToStd(strFilename));
			}
			catch(CStdErrorInfo oError)
			{
				throw gcnew Exception(gcnew String(oError.m_strError.c_str()));
			}
			catch(...)
			{
				System::String ^strError = "An unknown error occurred while calling Save.";
				throw gcnew System::Exception(strError);
			}
		}
	}
}

