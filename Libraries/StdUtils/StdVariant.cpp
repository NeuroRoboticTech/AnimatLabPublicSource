// StdVariant.cpp: implementation of the CStdVariant class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CStdVariant::CStdVariant()
{
	m_bCreatedVar = false;
	m_iVariantType = StdVtEmpty;

	m_lpShort = NULL;
	m_lpLong = NULL;
	m_lpFloat = NULL;
	m_lpDouble = NULL;
	m_lpBool = NULL;
	m_lpChar = NULL;
	m_lpUChar = NULL;
	m_lpUShort = NULL;
	m_lpULong = NULL;
	m_lpInt = NULL;
	m_lpUInt = NULL;
	m_lpString = NULL;
}

CStdVariant::~CStdVariant()
{

try
{
	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CStdVariant\r\n", "", -1, FALSE, TRUE);}
}

int CStdVariant::VariantType()
{return m_iVariantType;}

string CStdVariant::VariantTypeName()
{return Std_ConstToVariantType(m_iVariantType);}

void CStdVariant::Reset()
{

	if(m_bCreatedVar)
	{
		if(m_lpShort) delete m_lpShort;
		if(m_lpLong) delete m_lpLong;
		if(m_lpFloat) delete m_lpFloat;
		if(m_lpDouble) delete m_lpDouble;
		if(m_lpBool) delete m_lpBool;
		if(m_lpChar) delete m_lpChar;
		if(m_lpUChar) delete m_lpUChar;
		if(m_lpUShort) delete m_lpUShort;
		if(m_lpULong) delete m_lpULong;
		if(m_lpInt) delete m_lpInt;
		if(m_lpUInt) delete m_lpUInt;
		if(m_lpString) delete m_lpString;
	}

	m_bCreatedVar = false;
	m_iVariantType = StdVtEmpty;

	m_lpShort = NULL;
	m_lpLong = NULL;
	m_lpFloat = NULL;
	m_lpDouble = NULL;
	m_lpBool = NULL;
	m_lpChar = NULL;
	m_lpUChar = NULL;
	m_lpUShort = NULL;
	m_lpULong = NULL;
	m_lpInt = NULL;
	m_lpUInt = NULL;
	m_lpString = NULL;
}

short CStdVariant::GetShort(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtShort) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Short");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (short) *m_lpShort;
		case StdVtLong:
			return (short) *m_lpLong;
		case StdVtFloat:
			return (short) *m_lpFloat;
		case StdVtDouble:
			return (short) *m_lpDouble;
		case StdVtBool:
			return (short) *m_lpBool;
		case StdVtChar:
			return (short) *m_lpChar;
		case StdVtUChar:
			return (short) *m_lpUChar;
		case StdVtUShort:
			return (short) *m_lpUShort;
		case StdVtULong:
			return (short) *m_lpULong;
		case StdVtInt:
			return (short) *m_lpInt;
		case StdVtUInt:
			return (short) *m_lpUInt;
		case StdVtString:
			return (short) atol(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

short *CStdVariant::GetShortPtr()
{
	if(!m_lpShort || (m_iVariantType!=StdVtShort) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Short");
	return m_lpShort;
}

void CStdVariant::SetValue(short Val)
{
	Reset();
	m_iVariantType = StdVtShort;
	m_lpShort = new short;
	m_bCreatedVar = true;
	*m_lpShort = Val;
}

void CStdVariant::SetPtr(short *lpVal)
{
	Reset();
	m_iVariantType = StdVtShort;
	m_lpShort = lpVal;
	m_bCreatedVar = false;
}



long CStdVariant::GetLong(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtLong) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Long");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (long) *m_lpShort;
		case StdVtLong:
			return (long) *m_lpLong;
		case StdVtFloat:
			return (long) *m_lpFloat;
		case StdVtDouble:
			return (long) *m_lpDouble;
		case StdVtBool:
			return (long) *m_lpBool;
		case StdVtChar:
			return (long) *m_lpChar;
		case StdVtUChar:
			return (long) *m_lpUChar;
		case StdVtUShort:
			return (long) *m_lpUShort;
		case StdVtULong:
			return (long) *m_lpULong;
		case StdVtInt:
			return (long) *m_lpInt;
		case StdVtUInt:
			return (long) *m_lpUInt;
		case StdVtString:
			return (long) atol(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

long *CStdVariant::GetLongPtr()
{
	if(!m_lpLong || (m_iVariantType!=StdVtLong) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Long");
	return m_lpLong;
}

void CStdVariant::SetValue(long Val)
{
	Reset();
	m_iVariantType = StdVtLong;
	m_lpLong = new long;
	m_bCreatedVar = true;
	*m_lpLong = Val;
}

void CStdVariant::SetPtr(long *lpVal)
{
	Reset();
	m_iVariantType = StdVtLong;
	m_lpLong = lpVal;
	m_bCreatedVar = false;
}



float CStdVariant::GetFloat(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtFloat) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Float");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (float) *m_lpShort;
		case StdVtLong:
			return (float) *m_lpLong;
		case StdVtFloat:
			return (float) *m_lpFloat;
		case StdVtDouble:
			return (float) *m_lpDouble;
		case StdVtBool:
			return (float) *m_lpBool;
		case StdVtChar:
			return (float) *m_lpChar;
		case StdVtUChar:
			return (float) *m_lpUChar;
		case StdVtUShort:
			return (float) *m_lpUShort;
		case StdVtULong:
			return (float) *m_lpULong;
		case StdVtInt:
			return (float) *m_lpInt;
		case StdVtUInt:
			return (float) *m_lpUInt;
		case StdVtString:
			return (float) atof(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

float *CStdVariant::GetFloatPtr()
{
	if(!m_lpFloat || (m_iVariantType!=StdVtFloat) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Float");
	return m_lpFloat;
}

void CStdVariant::SetValue(float Val)
{
	Reset();
	m_iVariantType = StdVtFloat;
	m_lpFloat = new float;
	m_bCreatedVar = true;
	*m_lpFloat = Val;
}

void CStdVariant::SetPtr(float *lpVal)
{
	Reset();
	m_iVariantType = StdVtFloat;
	m_lpFloat = lpVal;
	m_bCreatedVar = false;
}



double CStdVariant::GetDouble(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtDouble) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Double");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (double) *m_lpShort;
		case StdVtLong:
			return (double) *m_lpLong;
		case StdVtFloat:
			return (double) *m_lpFloat;
		case StdVtDouble:
			return (double) *m_lpDouble;
		case StdVtBool:
			return (double) *m_lpBool;
		case StdVtChar:
			return (double) *m_lpChar;
		case StdVtUChar:
			return (double) *m_lpUChar;
		case StdVtUShort:
			return (double) *m_lpUShort;
		case StdVtULong:
			return (double) *m_lpULong;
		case StdVtInt:
			return (double) *m_lpInt;
		case StdVtUInt:
			return (double) *m_lpUInt;
		case StdVtString:
			return (double) atof(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

double *CStdVariant::GetDoublePtr()
{
	if(!m_lpDouble || (m_iVariantType!=StdVtDouble) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Double");
	return m_lpDouble;
}

void CStdVariant::SetValue(double Val)
{
	Reset();
	m_iVariantType = StdVtDouble;
	m_lpDouble = new double;
	m_bCreatedVar = true;
	*m_lpDouble = Val;
}

void CStdVariant::SetPtr(double *lpVal)
{
	Reset();
	m_iVariantType = StdVtDouble;
	m_lpDouble = lpVal;
	m_bCreatedVar = false;
}



bool CStdVariant::GetBool(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtBool) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Bool");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			if(*m_lpShort == 0) return false; else return true;
		case StdVtLong:
			if(*m_lpLong == 0) return false; else return true;
		case StdVtFloat:
			if(*m_lpFloat == 0) return false; else return true;
		case StdVtDouble:
			if(*m_lpDouble == 0) return false; else return true;
		case StdVtBool:
			return *m_lpBool;
		case StdVtChar:
			if(*m_lpChar == 0) return false; else return true;
		case StdVtUChar:
			if(*m_lpUChar == 0) return false; else return true;
		case StdVtUShort:
			if(*m_lpUShort == 0) return false; else return true;
		case StdVtULong:
			if(*m_lpULong == 0) return false; else return true;
		case StdVtInt:
			if(*m_lpInt == 0) return false; else return true;
		case StdVtUInt:
			if(*m_lpUInt == 0) return false; else return true;
		case StdVtString:
			if(Std_CheckString(*m_lpString) == "TRUE") return true; else return false;

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return false;
}

bool *CStdVariant::GetBoolPtr()
{
	if(!m_lpBool || (m_iVariantType!=StdVtBool) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Bool");
	return m_lpBool;
}

void CStdVariant::SetValue(bool Val)
{
	Reset();
	m_iVariantType = StdVtBool;
	m_lpBool = new bool;
	m_bCreatedVar = true;
	*m_lpBool = Val;
}

void CStdVariant::SetPtr(bool *lpVal)
{
	Reset();
	m_iVariantType = StdVtBool;
	m_lpBool = lpVal;
	m_bCreatedVar = false;
}



char CStdVariant::GetChar(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtDouble) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Double");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (char) *m_lpShort;
		case StdVtLong:
			return (char) *m_lpLong;
		case StdVtFloat:
			return (char) *m_lpFloat;
		case StdVtDouble:
			return (char) *m_lpDouble;
		case StdVtBool:
			return (char) *m_lpBool;
		case StdVtChar:
			return (char) *m_lpChar;
		case StdVtUChar:
			return (char) *m_lpUChar;
		case StdVtUShort:
			return (char) *m_lpUShort;
		case StdVtULong:
			return (char) *m_lpULong;
		case StdVtInt:
			return (char) *m_lpInt;
		case StdVtUInt:
			return (char) *m_lpUInt;
		case StdVtString:
			if(m_lpString->length())
				return (char) (*m_lpString)[0];
			else
				return 0;

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

char *CStdVariant::GetCharPtr()
{
	if(!m_lpChar || (m_iVariantType!=StdVtChar) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Char");
	return m_lpChar;
}

void CStdVariant::SetValue(char Val)
{
	Reset();
	m_iVariantType = StdVtChar;
	m_lpChar = new char;
	m_bCreatedVar = true;
	*m_lpChar = Val;
}

void CStdVariant::SetPtr(char *lpVal)
{
	Reset();
	m_iVariantType = StdVtChar;
	m_lpChar = lpVal;
	m_bCreatedVar = false;
}



unsigned char CStdVariant::GetUChar(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtUChar) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "UChar");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (unsigned char) *m_lpShort;
		case StdVtLong:
			return (unsigned char) *m_lpLong;
		case StdVtFloat:
			return (unsigned char) *m_lpFloat;
		case StdVtDouble:
			return (unsigned char) *m_lpDouble;
		case StdVtBool:
			return (unsigned char) *m_lpBool;
		case StdVtChar:
			return (unsigned char) *m_lpChar;
		case StdVtUChar:
			return (unsigned char) *m_lpUChar;
		case StdVtUShort:
			return (unsigned char) *m_lpUShort;
		case StdVtULong:
			return (unsigned char) *m_lpULong;
		case StdVtInt:
			return (unsigned char) *m_lpInt;
		case StdVtUInt:
			return (unsigned char) *m_lpUInt;
		case StdVtString:
			return (unsigned char) atol(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

unsigned char *CStdVariant::GetUCharPtr()
{
	if(!m_lpUChar || (m_iVariantType!=StdVtUChar) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "UChar");
	return m_lpUChar;
}

void CStdVariant::SetValue(unsigned char Val)
{
	Reset();
	m_iVariantType = StdVtUChar;
	m_lpUChar = new unsigned char;
	m_bCreatedVar = true;
	*m_lpUChar = Val;
}

void CStdVariant::SetPtr(unsigned char *lpVal)
{
	Reset();
	m_iVariantType = StdVtUChar;
	m_lpUChar = lpVal;
	m_bCreatedVar = false;
}



unsigned short CStdVariant::GetUShort(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtUShort) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "UShort");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (unsigned short) *m_lpShort;
		case StdVtLong:
			return (unsigned short) *m_lpLong;
		case StdVtFloat:
			return (unsigned short) *m_lpFloat;
		case StdVtDouble:
			return (unsigned short) *m_lpDouble;
		case StdVtBool:
			return (unsigned short) *m_lpBool;
		case StdVtChar:
			return (unsigned short) *m_lpChar;
		case StdVtUChar:
			return (unsigned short) *m_lpUChar;
		case StdVtUShort:
			return (unsigned short) *m_lpUShort;
		case StdVtULong:
			return (unsigned short) *m_lpULong;
		case StdVtInt:
			return (unsigned short) *m_lpInt;
		case StdVtUInt:
			return (unsigned short) *m_lpUInt;
		case StdVtString:
			return (unsigned short) atol(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

unsigned short *CStdVariant::GetUShortPtr()
{
	if(!m_lpUShort || (m_iVariantType!=StdVtUShort) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "UShort");
	return m_lpUShort;
}

void CStdVariant::SetValue(unsigned short Val)
{
	Reset();
	m_iVariantType = StdVtUShort;
	m_lpUShort = new unsigned short;
	m_bCreatedVar = true;
	*m_lpUShort = Val;
}

void CStdVariant::SetPtr(unsigned short *lpVal)
{
	Reset();
	m_iVariantType = StdVtUShort;
	m_lpUShort = lpVal;
	m_bCreatedVar = false;
}



unsigned long CStdVariant::GetULong(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtULong) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "ULong");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (unsigned long) *m_lpShort;
		case StdVtLong:
			return (unsigned long) *m_lpLong;
		case StdVtFloat:
			return (unsigned long) *m_lpFloat;
		case StdVtDouble:
			return (unsigned long) *m_lpDouble;
		case StdVtBool:
			return (unsigned long) *m_lpBool;
		case StdVtChar:
			return (unsigned long) *m_lpChar;
		case StdVtUChar:
			return (unsigned long) *m_lpUChar;
		case StdVtUShort:
			return (unsigned long) *m_lpUShort;
		case StdVtULong:
			return (unsigned long) *m_lpULong;
		case StdVtInt:
			return (unsigned long) *m_lpInt;
		case StdVtUInt:
			return (unsigned long) *m_lpUInt;
		case StdVtString:
			return (unsigned long) atol(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

unsigned long *CStdVariant::GetULongPtr()
{
	if(!m_lpULong || (m_iVariantType!=StdVtULong) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "ULong");
	return m_lpULong;
}

void CStdVariant::SetValue(unsigned long Val)
{
	Reset();
	m_iVariantType = StdVtULong;
	m_lpULong = new unsigned long;
	m_bCreatedVar = true;
	*m_lpULong = Val;
}

void CStdVariant::SetPtr(unsigned long *lpVal)
{
	Reset();
	m_iVariantType = StdVtULong;
	m_lpULong = lpVal;
	m_bCreatedVar = false;
}



int CStdVariant::GetInt(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtInt) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Int");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (int) *m_lpShort;
		case StdVtLong:
			return (int) *m_lpLong;
		case StdVtFloat:
			return (int) *m_lpFloat;
		case StdVtDouble:
			return (int) *m_lpDouble;
		case StdVtBool:
			return (int) *m_lpBool;
		case StdVtChar:
			return (int) *m_lpChar;
		case StdVtUChar:
			return (int) *m_lpUChar;
		case StdVtUShort:
			return (int) *m_lpUShort;
		case StdVtULong:
			return (int) *m_lpULong;
		case StdVtInt:
			return (int) *m_lpInt;
		case StdVtUInt:
			return (int) *m_lpUInt;
		case StdVtString:
			return (int) atof(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

int *CStdVariant::GetIntPtr()
{
	if(!m_lpInt || (m_iVariantType!=StdVtInt) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "Int");
	return m_lpInt;
}

void CStdVariant::SetValue(int Val)
{
	Reset();
	m_iVariantType = StdVtInt;
	m_lpInt = new int;
	m_bCreatedVar = true;
	*m_lpInt = Val;
}

void CStdVariant::SetPtr(int *lpVal)
{
	Reset();
	m_iVariantType = StdVtInt;
	m_lpInt = lpVal;
	m_bCreatedVar = false;
}



unsigned int CStdVariant::GetUInt(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtUInt) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "UInt");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return (unsigned int) *m_lpShort;
		case StdVtLong:
			return (unsigned int) *m_lpLong;
		case StdVtFloat:
			return (unsigned int) *m_lpFloat;
		case StdVtDouble:
			return (unsigned int) *m_lpDouble;
		case StdVtBool:
			return (unsigned int) *m_lpBool;
		case StdVtChar:
			return (unsigned int) *m_lpChar;
		case StdVtUChar:
			return (unsigned int) *m_lpUChar;
		case StdVtUShort:
			return (unsigned int) *m_lpUShort;
		case StdVtULong:
			return (unsigned int) *m_lpULong;
		case StdVtInt:
			return (unsigned int) *m_lpInt;
		case StdVtUInt:
			return (unsigned int) *m_lpUInt;
		case StdVtString:
			return (unsigned int) atol(m_lpString->c_str());

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return -1;
}

unsigned int *CStdVariant::GetUIntPtr()
{
	if(!m_lpUInt || (m_iVariantType!=StdVtUInt) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "UInt");
	return m_lpUInt;
}

void CStdVariant::SetValue(unsigned int Val)
{
	Reset();
	m_iVariantType = StdVtUInt;
	m_lpUInt = new unsigned int;
	m_bCreatedVar = true;
	*m_lpUInt = Val;
}

void CStdVariant::SetPtr(unsigned int *lpVal)
{
	Reset();
	m_iVariantType = StdVtUInt;
	m_lpUInt = lpVal;
	m_bCreatedVar = false;
}


string CStdVariant::GetNumericString()
{
	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return STR(*m_lpShort);
		case StdVtLong:
			return STR(*m_lpLong);
		case StdVtFloat:
			return STR(*m_lpFloat);
		case StdVtDouble:
			return STR(*m_lpDouble);
		case StdVtBool:
			return STR((int) *m_lpBool);
		case StdVtChar:
			return STR(*m_lpChar);
		case StdVtUChar:
			return STR(*m_lpUChar);
		case StdVtUShort:
			return STR(*m_lpUShort);
		case StdVtULong:
			return STR((long) *m_lpULong);
		case StdVtInt:
			return STR(*m_lpInt);
		case StdVtUInt:
			return STR((long) *m_lpUInt);

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return "";
}


string CStdVariant::GetString(bool bConvert)
{
	if(!bConvert && (m_iVariantType!=StdVtString) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "String");

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			THROW_ERROR(Std_Err_lVariantNotSet, Std_Err_strVariantNotSet);
			break;

		case StdVtShort:
			return STR(*m_lpShort);
		case StdVtLong:
			return STR(*m_lpLong);
		case StdVtFloat:
			return STR(*m_lpFloat);
		case StdVtDouble:
			return STR(*m_lpDouble);
		case StdVtBool:
			return STR(*m_lpBool);
		case StdVtChar:
			return STR(*m_lpChar);
		case StdVtUChar:
			return STR(*m_lpUChar);
		case StdVtUShort:
			return STR(*m_lpUShort);
		case StdVtULong:
			return STR((long) *m_lpULong);
		case StdVtInt:
			return STR(*m_lpInt);
		case StdVtUInt:
			return STR((long) *m_lpUInt);
		case StdVtString:
			return *m_lpString;

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
	
	return "";
}

string *CStdVariant::GetStringPtr()
{
	if(!m_lpString || (m_iVariantType!=StdVtString) )
		THROW_PARAM_ERROR(Std_Err_lVariantParamUndefined, Std_Err_strVariantParamUndefined, "Param", "String");
	return m_lpString;
}

void CStdVariant::SetValue(string Val)
{
	Reset();
	m_iVariantType = StdVtShort;
	m_lpString = new string;
	m_bCreatedVar = true;
	*m_lpString = Val;
}

void CStdVariant::SetPtr(string *lpVal)
{
	Reset();
	m_iVariantType = StdVtShort;
	m_lpString = lpVal;
	m_bCreatedVar = false;
}


void CStdVariant::Copy(CStdVariant &oVar)
{
	switch (oVar.m_iVariantType)
	{
		case StdVtEmpty:
			break;
		case StdVtShort:
			SetValue(*oVar.m_lpShort);
			break;
		case StdVtLong:
			SetValue(*oVar.m_lpLong);
			break;
		case StdVtFloat:
			SetValue(*oVar.m_lpFloat);
			break;
		case StdVtDouble:
			SetValue(*oVar.m_lpDouble);
			break;
		case StdVtBool:
			SetValue(*oVar.m_lpBool);
			break;
		case StdVtChar:
			SetValue(*oVar.m_lpChar);
			break;
		case StdVtUChar:
			SetValue(*oVar.m_lpUChar);
			break;
		case StdVtUShort:
			SetValue(*oVar.m_lpUShort);
			break;
		case StdVtULong:
			SetValue(*oVar.m_lpULong);
			break;
		case StdVtInt:
			SetValue(*oVar.m_lpInt);
			break;
		case StdVtUInt:
			SetValue(*oVar.m_lpUInt);
			break;
		case StdVtString:
			SetValue(*oVar.m_lpString);
			break;

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}
}

void CStdVariant::operator=(CStdVariant &oVar)
{
	if(oVar.m_bCreatedVar)
	{
		switch (oVar.m_iVariantType)
		{
			case StdVtEmpty:
				break;
			case StdVtShort:
				SetValue(*oVar.m_lpShort);
				break;
			case StdVtLong:
				SetValue(*oVar.m_lpLong);
				break;
			case StdVtFloat:
				SetValue(*oVar.m_lpFloat);
				break;
			case StdVtDouble:
				SetValue(*oVar.m_lpDouble);
				break;
			case StdVtBool:
				SetValue(*oVar.m_lpBool);
				break;
			case StdVtChar:
				SetValue(*oVar.m_lpChar);
				break;
			case StdVtUChar:
				SetValue(*oVar.m_lpUChar);
				break;
			case StdVtUShort:
				SetValue(*oVar.m_lpUShort);
				break;
			case StdVtULong:
				SetValue(*oVar.m_lpULong);
				break;
			case StdVtInt:
				SetValue(*oVar.m_lpInt);
				break;
			case StdVtUInt:
				SetValue(*oVar.m_lpUInt);
				break;
			case StdVtString:
				SetValue(*oVar.m_lpString);
				break;

			default:
				THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
				break;
		}
	}
	else
	{
		Reset();
		m_lpShort = oVar.m_lpShort;
		m_lpLong = oVar.m_lpLong;
		m_lpFloat = oVar.m_lpFloat;
		m_lpDouble = oVar.m_lpDouble;
		m_lpBool = oVar.m_lpBool;
		m_lpChar = oVar.m_lpChar;
		m_lpUChar = oVar.m_lpUChar;
		m_lpUShort = oVar.m_lpUShort;
		m_lpULong = oVar.m_lpULong;
		m_lpInt = oVar.m_lpInt;
		m_lpUInt = oVar.m_lpUInt;
		m_lpString = oVar.m_lpString;
		m_iVariantType = oVar.m_iVariantType;
	}

}


CStdSerialize *CStdVariant::Clone()
{
	CStdVariant *lpVar=NULL;

try
{
	lpVar = new CStdVariant;
	lpVar->operator=(*this);
	return lpVar;
}
catch(CStdErrorInfo oError)
{
	if(lpVar) delete lpVar;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpVar) delete lpVar;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


void CStdVariant::Trace(ostream &oOs)
{
	oOs << "(VT: " << Std_ConstToVariantType(m_iVariantType) << "\tVal: ";

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			break;
		case StdVtShort:
			oOs << *m_lpShort;
			break;
		case StdVtLong:
			oOs << *m_lpLong;
			break;
		case StdVtFloat:
			oOs << *m_lpFloat;
			break;
		case StdVtDouble:
			oOs << *m_lpDouble;
			break;
		case StdVtBool:
			oOs << STR(*m_lpBool);
			break;
		case StdVtChar:
			oOs << *m_lpChar;
			break;
		case StdVtUChar:
			oOs << *m_lpUChar;
			break;
		case StdVtUShort:
			oOs << *m_lpUShort;
			break;
		case StdVtULong:
			oOs << *m_lpULong;
			break;
		case StdVtInt:
			oOs << *m_lpInt;
			break;
		case StdVtUInt:
			oOs << *m_lpUInt;
			break;
		case StdVtString:
			oOs << *m_lpString;
			break;

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}

	oOs << ")";
}

//CStdSerialize overloads
void CStdVariant::Load(CStdXml &oXml)
{
	short sVal;
	long lVal;
	float fltVal;
	double dblVal;
	bool bVal;
	char cVal;
	unsigned char ucVal;
	unsigned short usVal;
	unsigned long ulVal;
	int iVal;
	unsigned int uiVal;
	string strVal;

	oXml.IntoElem();  //Into Variant Element

	string strVariantType = oXml.GetChildString("VariantType");
	m_iVariantType = Std_VariantTypeToConst(strVariantType);
	
	switch (m_iVariantType)
	{
		case StdVtEmpty:
			break;

		case StdVtShort:
			sVal = (short) oXml.GetChildInt("Value");
			SetValue(sVal);
			break;

		case StdVtLong:
			lVal = oXml.GetChildLong("Value");
			SetValue(lVal);
			break;

		case StdVtFloat:
			fltVal = oXml.GetChildFloat("Value");
			SetValue(fltVal);
			break;

		case StdVtDouble:
			dblVal = oXml.GetChildDouble("Value");
			SetValue(dblVal);
			break;

		case StdVtBool:
			bVal = oXml.GetChildBool("Value");
			SetValue(bVal);
			break;

		case StdVtChar:
			cVal = (char) oXml.GetChildInt("Value");
			SetValue(cVal);
			break;

		case StdVtUChar:
			ucVal = (unsigned char) oXml.GetChildInt("Value");
			SetValue(ucVal);
			break;

		case StdVtUShort:
			usVal = (unsigned short) oXml.GetChildLong("Value");
			SetValue(usVal);
			break;

		case StdVtULong:
			ulVal = (unsigned long) oXml.GetChildLong("Value");
			SetValue(ulVal);
			break;

		case StdVtInt:
			iVal = oXml.GetChildInt("Value");
			SetValue(iVal);
			break;

		case StdVtUInt:
			uiVal = (unsigned int) oXml.GetChildLong("Value");
			SetValue(uiVal);
			break;

		case StdVtString:
			strVal = oXml.GetChildString("Value");
			SetValue(strVal);
			break;

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}

	oXml.OutOfElem(); //OutOf Variant Element
}

void CStdVariant::Save(CStdXml &oXml)
{
	string strEmpty;

	oXml.AddChildElement("Variant");
	oXml.IntoElem();  //Into Variant Element

	string strVariantType = Std_ConstToVariantType(m_iVariantType);
	oXml.AddChildElement("VariantType", strVariantType);

	switch (m_iVariantType)
	{
		case StdVtEmpty:
			oXml.AddChildElement("Value", strEmpty);
			break;

		case StdVtShort:
			oXml.AddChildElement("Value", (long) *m_lpShort);
			break;

		case StdVtLong:
			oXml.AddChildElement("Value", *m_lpLong);
			break;

		case StdVtFloat:
			oXml.AddChildElement("Value", *m_lpFloat);
			break;

		case StdVtDouble:
			oXml.AddChildElement("Value", *m_lpDouble);
			break;

		case StdVtBool:
			oXml.AddChildElement("Value", STR(*m_lpBool));
			break;

		case StdVtChar:
			oXml.AddChildElement("Value", (int) *m_lpChar);
			break;

		case StdVtUChar:
			oXml.AddChildElement("Value", (int) *m_lpUChar);
			break;

		case StdVtUShort:
			oXml.AddChildElement("Value", (long) *m_lpUShort);
			break;

		case StdVtULong:
			oXml.AddChildElement("Value", (long) *m_lpULong);
			break;

		case StdVtInt:
			oXml.AddChildElement("Value", *m_lpInt);
			break;

		case StdVtUInt:
			oXml.AddChildElement("Value", (long) *m_lpUInt);
			break;

		case StdVtString:
			oXml.AddChildElement("Value", *m_lpString);
			break;

		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidVariantType, Std_Err_strInvalidVariantType, "VariantType", m_iVariantType);
			break;
	}

	oXml.OutOfElem(); //OutOf Variant Element
}

