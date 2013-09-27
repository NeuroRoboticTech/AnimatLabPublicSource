#include "StdAfx.h"
//#include "ILogger.h"
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

		
		void StdXml::SetLogger(ManagedAnimatInterfaces::ILogger ^lpLog)
		{
			m_lpLogger = lpLog;
		}
		
		void StdXml::LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel, System::String ^sMessage)
		{
			if(m_lpLogger)
				m_lpLogger->LogMsg(eLevel, sMessage);
		}

		System::String ^StdXml::Serialize()
		{
			try
			{
				TRACE_DETAIL("StdXml::Serializing xml");
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
				TRACE_DETAIL("StdXml::Deserializing xml");
				std::string strData = Util::StringToStd(strXml);
				TRACE_DETAIL("StdXml::Xml: " + strData);
				m_lpXml->Deserialize(strData);
				TRACE_DETAIL("StdXml::Finished Deserializing");
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
				TRACE_DETAIL("StdXml::Into xml element");
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
				TRACE_DETAIL("StdXml::Outof xml element");
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
				TRACE_DETAIL("StdXml::FullTagPath");
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
				std::string strVal = m_lpXml->GetTagName();
				TRACE_DETAIL("StdXml::TagName: " + strVal);
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
				std::string strVal = m_lpXml->GetChildTagName();
				TRACE_DETAIL("StdXml::ChildTagName: " + strVal);
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
				int iNum = m_lpXml->NumberOfChildren();
				TRACE_DETAIL("StdXml::NumberOfChildren: " + STR(iNum));
				return iNum;
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::FindElement: " + strName);
				return m_lpXml->FindElement(strName, bThrowError);
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
				TRACE_DETAIL("StdXml::FindChildByIndex: " + STR(iIndex));
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::FindChildElement: " + strName);
				return m_lpXml->FindChildElement(strName, bThrowError);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::IntoChildElement: " + strName);
				return m_lpXml->IntoChildElement(strName, bThrowError);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildString: " + strName);
				return gcnew String(m_lpXml->GetChildString(strName).c_str());
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
				std::string strName = Util::StringToStd(strElementName);
				std::string strDef = Util::StringToStd(strDefault);
				TRACE_DETAIL("StdXml::GetChildString: " + strName = ", Default: " + strDef);
				return gcnew String(m_lpXml->GetChildString(strName, strDef).c_str());
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
				TRACE_DETAIL("StdXml::GetChildString");
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

		System::Int64 StdXml::GetChildLong(System::String ^strElementName)
		{
			try
			{
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildLong: " + strName);
				return m_lpXml->GetChildLong(strName);
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
		
		System::Int64 StdXml::GetChildLong(System::String ^strElementName, System::Int64 lDefault)
		{
			try
			{
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildLong: " + strName);
				return m_lpXml->GetChildLong(strName, lDefault);
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

		System::Int64 StdXml::GetChildLong()
		{
			try
			{
				TRACE_DETAIL("StdXml::GetChildLong");
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

		System::Int32 StdXml::GetChildInt(System::String ^strElementName)
		{
			try
			{
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildInt: " + strName);
				return m_lpXml->GetChildInt(strName);
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

		System::Int32 StdXml::GetChildInt(System::String ^strElementName, System::Int32 iDefault)
		{
			try
			{
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildInt: " + strName + ", Default: " + STR(iDefault));
				return m_lpXml->GetChildInt(strName, iDefault);
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

		System::Int32 StdXml::GetChildInt()
		{
			try
			{
				TRACE_DETAIL("StdXml::GetChildInt");
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildDouble: " + strName);
				return m_lpXml->GetChildDouble(strName);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildDouble: " + strName + ", Default: " + STR(dblDefault));
				return m_lpXml->GetChildDouble(strName, dblDefault);
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
				TRACE_DETAIL("StdXml::GetChildDouble");
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildFloat: " + strName);
				return m_lpXml->GetChildFloat(strName);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildFloat: " + strName + ", Default: " + STR(fltDefault));
				return m_lpXml->GetChildFloat(strName, fltDefault);
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
				TRACE_DETAIL("StdXml::GetChildFloat");
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildBool: " + strName);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildBool: " + strName + ", Default: " + STR(bDefault));
				return m_lpXml->GetChildBool(strName, bDefault);
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
				TRACE_DETAIL("StdXml::GetChildBool");
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::GetChildByteArray: " + strName);
				CStdArray<unsigned char> aryBytes;
				std::string strHex = m_lpXml->GetChildString();
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

					TRACE_DETAIL("StdXml::GetChildByteArray Finished");
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
				std::string strName = Util::StringToStd(strElementName);
				std::string strD = Util::StringToStd(strData);
				TRACE_DETAIL("StdXml::AddElement: " + strName + ", Data: " + strD);
				m_lpXml->AddElement(strName, strD);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName);
				m_lpXml->AddChildElement(strName);
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
				std::string strName = Util::StringToStd(strElementName);
				std::string strV = Util::StringToStd(strVal);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + strV);
				m_lpXml->AddChildElement(strName, strV);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + cVal);
				m_lpXml->AddChildElement(strName, cVal);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + STR(cVal));
				m_lpXml->AddChildElement(strName, cVal);
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

		void StdXml::AddChildElement(System::String ^strElementName, System::Int64 lVal)
		{
			try
			{
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + STR((long) lVal));
				m_lpXml->AddChildElement(strName, (long) lVal);
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

		void StdXml::AddChildElement(System::String ^strElementName, System::Int32 iVal)
		{
			try
			{
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + STR(iVal));
				m_lpXml->AddChildElement(strName, (int) iVal);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + STR(dblVal));
				m_lpXml->AddChildElement(strName, dblVal);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + STR(fltVal));
				m_lpXml->AddChildElement(strName, fltVal);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName + ", Value: " + STR(bVal));
				m_lpXml->AddChildElement(strName, bVal);
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
				std::string strName = Util::StringToStd(strElementName);
				TRACE_DETAIL("StdXml::AddChildElement: " + strName);

				CStdArray<unsigned char> aryBytes;

				long lIndex;
				long lLength = aryData->Length;
				for(lIndex=0; lIndex<lLength; lIndex++)
				{
					unsigned char iVal = (unsigned char) aryData[lIndex];
					aryBytes.Add(iVal);
				}

				std::string strHex = Std_ByteArrayToHexString(aryBytes);

				TRACE_DETAIL("StdXml::ByteArray: " + strHex);

				m_lpXml->AddChildElement(strName, strHex);
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
				std::string strName = Util::StringToStd(strElementName);
				std::string strC = Util::StringToStd(strCData);
				TRACE_DETAIL("StdXml::AddChildCData: " + strName + ", CData: " + strC);
				m_lpXml->AddChildCData(strName, strC);
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
				std::string strName = Util::StringToStd(strAttribName);
				std::string strD = Util::StringToStd(strDefault);
				TRACE_DETAIL("StdXml::GetAttribString: " + strName + ", Default: " + strD);
				return gcnew String(m_lpXml->GetAttribString(strName, bCanBeBlank, bThrowError, strD).c_str());
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

		System::Int64 StdXml::GetAttribLong(System::String ^strAttribName)
		{return GetAttribLong(strAttribName, true, 0);}

		System::Int64 StdXml::GetAttribLong(System::String ^strAttribName, bool bThrowError)
		{return GetAttribLong(strAttribName, bThrowError, 0);}

		System::Int64 StdXml::GetAttribLong(System::String ^strAttribName, bool bThrowError, System::Int64 lDefault)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetAttribLong: " + strName + ", Default: " + STR((long) lDefault));
				return m_lpXml->GetAttribLong(strName, bThrowError, lDefault);
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

		System::Int32 StdXml::GetAttribInt(System::String ^strAttribName)
		{return GetAttribInt(strAttribName, true, 0);}

		System::Int32 StdXml::GetAttribInt(System::String ^strAttribName, bool bThrowError)
		{return GetAttribInt(strAttribName, bThrowError, 0);}

		System::Int32 StdXml::GetAttribInt(System::String ^strAttribName, bool bThrowError, System::Int32 iDefault)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetAttribInt: " + strName + ", Default: " + STR(iDefault));
				return m_lpXml->GetAttribInt(strName, bThrowError, iDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetAttribDouble: " + strName + ", Default: " + STR(dblDefault));
				return m_lpXml->GetAttribDouble(strName, bThrowError, dblDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetAttribFloat: " + strName + ", Default: " + STR(fltDefault));
				return m_lpXml->GetAttribFloat(strName, bThrowError, fltDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetAttribBool: " + strName + ", Default: " + STR(bDefault));
				return m_lpXml->GetAttribBool(strName, bThrowError, bDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				std::string strV = Util::StringToStd(strVal);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + strV);
				m_lpXml->SetAttrib(strName, strV);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + cVal);
				m_lpXml->SetAttrib(strName, cVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + STR(cVal));
				m_lpXml->SetAttrib(strName, cVal);
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

		void StdXml::SetAttrib(System::String ^strAttribName, System::Int64 lVal)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + STR((long) lVal));
				m_lpXml->SetAttrib(strName, (long) lVal);
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

		void StdXml::SetAttrib(System::String ^strAttribName, System::Int32 iVal)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + STR(iVal));
				m_lpXml->SetAttrib(strName, (int) iVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + STR(dblVal));
				m_lpXml->SetAttrib(strName, dblVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + STR(fltVal));
				m_lpXml->SetAttrib(strName, fltVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetAttrib: " + strName + ", Value: " + STR(bVal));
				m_lpXml->SetAttrib(strName, bVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				std::string strD = Util::StringToStd(strDefault);
				TRACE_DETAIL("StdXml::GetChildAttribString: " + strName + ", Default: " +strD);
				return gcnew String(m_lpXml->GetChildAttribString(strName, bCanBeBlank, bThrowError, strD).c_str());
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

		System::Int64 StdXml::GetChildAttribLong(System::String ^strAttribName)
		{return GetChildAttribLong(strAttribName, true, 0);}

		System::Int64 StdXml::GetChildAttribLong(System::String ^strAttribName, bool bThrowError)
		{return GetChildAttribLong(strAttribName, bThrowError, 0);}

		System::Int64 StdXml::GetChildAttribLong(System::String ^strAttribName, bool bThrowError, System::Int64 lDefault)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetChildAttribLong: " + strName + ", Default: " +STR((long) lDefault));
				return m_lpXml->GetChildAttribLong(strName, bThrowError, lDefault);
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

		System::Int32 StdXml::GetChildAttribInt(System::String ^strAttribName)
		{return GetChildAttribInt(strAttribName, true, 0);}

		System::Int32 StdXml::GetChildAttribInt(System::String ^strAttribName, bool bThrowError)
		{return GetChildAttribInt(strAttribName, bThrowError, 0);}

		System::Int32 StdXml::GetChildAttribInt(System::String ^strAttribName, bool bThrowError, System::Int32 iDefault)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetChildAttribInt: " + strName + ", Default: " +STR(iDefault));
				return m_lpXml->GetChildAttribInt(strName, bThrowError, iDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetChildAttribDouble: " + strName + ", Default: " +STR(dblDefault));
				return m_lpXml->GetChildAttribDouble(strName, bThrowError, dblDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetChildAttribFloat: " + strName + ", Default: " +STR(fltDefault));
				return m_lpXml->GetChildAttribFloat(strName, bThrowError, fltDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::GetChildAttribBool: " + strName + ", Default: " +STR(bDefault));
				return m_lpXml->GetChildAttribBool(strName, bThrowError, bDefault);
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
				std::string strName = Util::StringToStd(strAttribName);
				std::string strV = Util::StringToStd(strVal);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + strV);
				m_lpXml->SetChildAttrib(strName, strV);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + cVal);
				m_lpXml->SetChildAttrib(strName, cVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + STR(cVal));
				m_lpXml->SetChildAttrib(strName, cVal);
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

		void StdXml::SetChildAttrib(System::String ^strAttribName, System::Int64 lVal)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + STR((long) lVal));
				m_lpXml->SetChildAttrib(strName, (long) lVal);
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

		void StdXml::SetChildAttrib(System::String ^strAttribName, System::Int32 iVal)
		{
			try
			{
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + STR(iVal));
				m_lpXml->SetChildAttrib(strName, (int) iVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + STR(dblVal));
				m_lpXml->SetChildAttrib(strName, dblVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + STR(fltVal));
				m_lpXml->SetChildAttrib(strName, fltVal);
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
				std::string strName = Util::StringToStd(strAttribName);
				TRACE_DETAIL("StdXml::SetChildAttrib: " + strName + ", Value: " + STR(bVal));
				m_lpXml->SetChildAttrib(strName, bVal);
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
				std::string strDoc = Util::StringToStd(Doc);
				TRACE_DETAIL("StdXml::AddChildDoc: " + strDoc);
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
				std::string strDoc = m_lpXml->GetChildDoc();
				TRACE_DETAIL("StdXml::GetChildDoc: " + strDoc);
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
				TRACE_DETAIL("StdXml::GetParentTagName");
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
				LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "Starting Load of " + strFilename);

				std::string strFile = Util::StringToStd(strFilename, m_lpLogger);

				TRACE_DETAIL("StdXml::Load: " + strFile);

				LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "Calling Sim::Xml::Load ");

				m_lpXml->Load(strFile);

				LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "Finished Load of " + strFilename);
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
				LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "Starting Save of " + strFilename);

				std::string strFile = Util::StringToStd(strFilename, m_lpLogger);
				TRACE_DETAIL("StdXml::Save: " + strFile);

				LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "Calling Sim::Xml::Save ");

				m_lpXml->Save(strFile);

				LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "Finished Save of " + strFilename);
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

