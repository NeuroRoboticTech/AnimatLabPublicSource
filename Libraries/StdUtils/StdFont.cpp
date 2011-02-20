// StdFont.cpp: implementation of the CStdFont class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CStdFont::CStdFont()
{
	m_strName = "Arial";
	m_fltSize = 8.25;
	m_fltWeight = 400;
	m_bBold = false;
	m_bItalic = false;
	m_bStrikethrough = false;
	m_bUnderline = false;
}

CStdFont::~CStdFont()
{

}

void CStdFont::Load(CStdXml &oXml, string strParamName, bool bThrowError)
{
	if(oXml.FindChildElement(strParamName, bThrowError))
	{
		m_strName = oXml.GetChildAttribString("Name", false, false, m_strName);
		m_fltSize = oXml.GetChildAttribFloat("Size", false, m_fltSize);
		Std_InValidRange((float) 1, (float) 100, m_fltSize, true, (strParamName + "::Size"));

		m_fltWeight = oXml.GetChildAttribFloat("Weight", false, m_fltWeight);
		Std_InValidRange((float) 0, (float) 1000, m_fltWeight, true, (strParamName + "::Weight"));

		m_bBold = oXml.GetChildAttribBool("Bold", false, m_bBold);
		m_bItalic = oXml.GetChildAttribBool("Italic", false, m_bItalic);
		m_bStrikethrough = oXml.GetChildAttribBool("Strikethrough", false, m_bStrikethrough);
		m_bUnderline = oXml.GetChildAttribBool("Underline", false, m_bUnderline);
	}
}


void CStdFont::Save(CStdXml &oXml, string strParamName)
{
	oXml.AddChildElement(strParamName);
	oXml.SetChildAttrib("Name", m_strName);
	oXml.SetChildAttrib("Size", m_fltSize);
	oXml.SetChildAttrib("Weight", m_fltWeight);
	oXml.SetChildAttrib("Bold", m_bBold);
	oXml.SetChildAttrib("Italic", m_bItalic);
	oXml.SetChildAttrib("Strikethrough", m_bStrikethrough);
	oXml.SetChildAttrib("Underline", m_bUnderline);
}