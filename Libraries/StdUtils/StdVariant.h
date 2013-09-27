/**
\file	StdVariant.h

\brief	Declares the standard variant class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Values that represent StdVariantType. 
**/
enum StdVariantType
{
	StdVtEmpty = -1,
	StdVtShort = 0,
	StdVtLong = 1,
	StdVtFloat = 2,
	StdVtDouble = 3,
	StdVtBool = 4,
	StdVtChar = 5,
	StdVtUChar = 6,
	StdVtUShort = 7,
	StdVtULong = 8,
	StdVtInt = 9,
	StdVtUInt = 10,
	StdVtString = 11
};

/**
\brief	Standard variant type. 

\author	dcofer
\date	5/4/2011
**/
class STD_UTILS_PORT CStdVariant : public CStdSerialize  
{
protected:
	/// true if created variable
	bool m_bCreatedVar;

	/// Variant type
	int m_iVariantType;

	/// StdVtShort	2-byte signed int.
	short *m_lpShort;							

	/// StdVtLong	4-byte signed int.
	long *m_lpLong;	

	/// StdVtFloat	4-byte real.
	float *m_lpFloat;	

	/// StdVtDouble	8-byte real.
	double *m_lpDouble;	

	/// StdVtBool	Boolean.				
	bool *m_lpBool;

	/// StdVtChar	Char.
	char *m_lpChar;		

	/// StdVtUChar	Unsigned char.
	unsigned char *m_lpUChar;

	/// StdVtUShort	2 byte unsigned int.
	unsigned short *m_lpUShort;

	/// StdVtULong	4 byte unsigned int.
	unsigned long *m_lpULong;			

	/// StdVtInt	2 byte signed int.
	int *m_lpInt;

	/// StdVtUInt	2 byte unsigned int.
	unsigned int *m_lpUInt;				

	/// StdVtString	string.
	std::string *m_lpString;						

public:
	CStdVariant();
	virtual ~CStdVariant();

	void Reset();
	int VariantType();
	std::string VariantTypeName();

	short GetShort(bool bConvert = true);
	short *GetShortPtr();
	void SetValue(short Val);
	void SetPtr(short *lpVal);

	long GetLong(bool bConvert = true);
	long *GetLongPtr();
	void SetValue(long Val);
	void SetPtr(long *lpVal);

	float GetFloat(bool bConvert = true);
	float *GetFloatPtr();
	void SetValue(float Val);
	void SetPtr(float *lpVal);

	double GetDouble(bool bConvert = true);
	double *GetDoublePtr();
	void SetValue(double Val);
	void SetPtr(double *lpVal);

	bool GetBool(bool bConvert = true);
	bool *GetBoolPtr();
	void SetValue(bool Val);
	void SetPtr(bool *lpVal);

	char GetChar(bool bConvert = true);
	char *GetCharPtr();
	void SetValue(char Val);
	void SetPtr(char *lpVal);

	unsigned char GetUChar(bool bConvert = true);
	unsigned char *GetUCharPtr();
	void SetValue(unsigned char Val);
	void SetPtr(unsigned char *lpVal);

	unsigned short GetUShort(bool bConvert = true);
	unsigned short *GetUShortPtr();
	void SetValue(unsigned short Val);
	void SetPtr(unsigned short *lpVal);

	unsigned long GetULong(bool bConvert = true);
	unsigned long *GetULongPtr();
	void SetValue(unsigned long Val);
	void SetPtr(unsigned long *lpVal);

	int GetInt(bool bConvert = true);
	int *GetIntPtr();
	void SetValue(int Val);
	void SetPtr(int *lpVal);

	unsigned int GetUInt(bool bConvert = true);
	unsigned int *GetUIntPtr();
	void SetValue(unsigned int Val);
	void SetPtr(unsigned int *lpVal);

	std::string GetString(bool bConvert = true);
	std::string GetNumericString();
	std::string *GetStringPtr();
	void SetValue(std::string Val);
	void SetPtr(std::string *lpVal);

	void Copy(CStdVariant &oVar);
	void operator=(CStdVariant &oVar);

	//CStdSerialize overloads
	virtual CStdSerialize *Clone();
	virtual void Trace(std::ostream &oOs);
	virtual void Load(CStdXml &oXml);
	virtual void Save(CStdXml &oXml);
};

}				//StdUtils
