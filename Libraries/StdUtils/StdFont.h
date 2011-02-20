// StdFont.h: interface for the CStdFont class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDFONT_H__0FE6F490_A4DE_467F_B44A_93A4EBE63D47__INCLUDED_)
#define AFX_STDFONT_H__0FE6F490_A4DE_467F_B44A_93A4EBE63D47__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdFont  
{
public:
	string m_strName;
	float m_fltSize;
	float m_fltWeight;
	bool m_bBold;
	bool m_bItalic;
	bool m_bStrikethrough;
	bool m_bUnderline;

public:
	CStdFont();
	virtual ~CStdFont();

	virtual void Load(CStdXml &oXml, string strParamName, bool bThrowError = false);
	virtual void Save(CStdXml &oXml, string strParamName);
};

#endif // !defined(AFX_STDFONT_H__0FE6F490_A4DE_467F_B44A_93A4EBE63D47__INCLUDED_)
