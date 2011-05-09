/**
\file	StdFont.h

\brief	Declares the standard font class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Standard font. 

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT CStdFont  
{
public:
	/// Name of the font
	string m_strName;

	/// Size of the font
	float m_fltSize;

	/// The weight of the font.
	float m_fltWeight;

	/// true for bold font.
	bool m_bBold;

	/// true for italic font.
	bool m_bItalic;

	/// true for Strikethrough font.
	bool m_bStrikethrough;

	/// true for Underline font.
	bool m_bUnderline;

public:
	CStdFont();
	virtual ~CStdFont();

	virtual void Load(CStdXml &oXml, string strParamName, bool bThrowError = false);
	virtual void Save(CStdXml &oXml, string strParamName);
};

}				//StdUtils

