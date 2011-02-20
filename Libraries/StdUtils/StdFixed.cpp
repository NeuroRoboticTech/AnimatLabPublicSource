#include "StdAfx.h"

CStdFixed::CStdFixed(int iM, int iN, int iMultM, int iMultN)
{
	Configure(iM, iN, iMultM, iMultN);
}

CStdFixed::CStdFixed(int iM, int iN, double dblVal, int iMultM, int iMultN)
{ 
	Configure(iM, iN, iMultM, iMultN);
	Fixed( (long) Convert(dblVal));
}

CStdFixed::CStdFixed(int iM, int iN, long lVal, int iMultM, int iMultN)
{
	Configure(iM, iN, iMultM, iMultN);
	Fixed( (long) lVal);
}

CStdFixed::~CStdFixed(void)
{
}

void CStdFixed::Configure(int iM, int iN, int iMultM, int iMultN)
{
	//if(iM < 1)
	//	THROW_PARAM_ERROR(Std_Err_lBitSizeInvalid, Std_Err_strBitSizeInvalid, " M must be positive and greater than 0", iM);

	if(iN < 0)
		THROW_PARAM_ERROR(Std_Err_lBitSizeInvalid, Std_Err_strBitSizeInvalid, " N must be positive or zero N", iN);

	if(iM+iN >= 32)
		THROW_PARAM_ERROR(Std_Err_lBitSizeToLarge, Std_Err_strBitSizeToLarge, "BitSize=M+N ", (iM+iN));

	m_iM = iM;
	m_iN = iN;

	if(iMultM <=0)
		m_iMultiplyM = m_iM;
	else
		m_iMultiplyM = iMultM;

	if(iMultN <=0)
		m_iMultiplyN = m_iN;
	else
		m_iMultiplyN = iMultN;

	m_lMaxReal = (long) pow(2.0, m_iM);
	m_lMaxInt = (long) pow(2.0, (iM+iN));
	
	m_dblConvertReal = ((double) m_lMaxReal/ (double) m_lMaxInt);
	m_dblConvertInt = ((double) m_lMaxInt/(double) m_lMaxReal);

	m_lAddPosMask = 0xFFFFFFFF >> (32 -m_iM - m_iN);
	m_lAddNegMask = 0xFFFFFFFF << (m_iM + m_iN);
	m_lAddTestMask = 1 << (m_iM + m_iN);

	m_lFixed = 0;
	m_dblVal = 0;
	m_fltVal = 0;
}


//CStdFixed operator+(float &fltA, CStdFixed fxB)
//{
//	CStdFixed fxC(fxB.M(), fxB.N(), fltA);
//
//	fxC = (fxC.FixedVal() + fxB.FixedVal());
//	return fxC;		
//}
