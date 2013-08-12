/**
\file	StdFixed.h

\brief	Declares the standard fixed-point calculation class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Standard fixed-point number class.

\details This is used to calculate fixed point numbers.

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT CStdFixed
{
protected:
	/// The number of bits to the left of the decimal point of the fixed-point 
	/// number representation.
	int m_iM;

	/// The number of bits to the right of the decimal point of the fixed-point 
	/// number representation.
	int m_iN;

	///The number of digits to shift when doing a multiplication.
	///For this we assume we are using some standard M.N format, but
	///sometimes we may have to multiply be a different format. When that
	///multiplication occurs we must always output things in the M.N format though.
	int m_iMultiplyM;

	///The number of digits to shift when doing a multiplication.
	///For this we assume we are using some standard M.N format, but
	///sometimes we may have to multiply be a different format. When that
	///multiplication occurs we must always output things in the M.N format though.
	int m_iMultiplyN;

	/// The fixed-point representation of the number. 
	long m_lFixed;

	///Actual value in double variable.
	double m_dblVal;

	///Actual value in float variable
	float m_fltVal;

	///This is the maximum integer value attainable by the fixed number.
	unsigned long m_lMaxInt;

	///This is the maximum real value attainable by the fixed number.
	unsigned long m_lMaxReal;

	/// The value to convert a fixed-point number to a floating-point number.
	double m_dblConvertReal;

	/// The value to convert a floating-point number to a fixed-point number.
	double m_dblConvertInt;

	/// Mask that is used to make certain that the number cannot go above the maximum number of bits.
	long m_lAddPosMask;

	/// Mask that is used to make certain that the number cannot go above the maximum number of bits.
	long m_lAddNegMask;

	/// Mask that is used to make certain that the number cannot go above the maximum number of bits.
	long m_lAddTestMask;

	/**
	\brief	Converts floating-point number to a fixed-point number.
	
	\author	dcofer
	\date	4/1/2011
	
	\param	dblVal	The floating-point value to convert. 
	
	\return	Converted fixed-point number.
	**/
	long Convert(double dblVal)
	{	
		return (long) ((dblVal)*m_dblConvertInt);
	}

	/**
	\brief	Converts a fixed-point number to a floating-point number.
	
	\author	dcofer
	\date	4/1/2011
	
	\param	lVal	The fixed-point value to convert. 
	
	\return	Converted floating-point number.
	**/	
	double Convert(long lVal)
	{	
		return (lVal*m_dblConvertReal);
	}

public:

	/**
	\brief	Default constructor.
	
	\author	dcofer
	\date	4/1/2011
	**/
	CStdFixed() 
	{
		m_iM = 0;
		m_iN = 0;
		m_iMultiplyM = 0;
		m_iMultiplyN = 0;
		m_lFixed = 0;
		m_lMaxInt = 0;
		m_lMaxReal = 0;
		m_dblConvertReal = 0;
		m_dblConvertInt = 0;
		m_lAddPosMask = 0;
		m_lAddNegMask = 0;
		m_lAddTestMask = 0;
		m_dblVal = 0;
		m_fltVal = 0;
	};

	CStdFixed(int iM, int iN, int iMultM = -1, int iMultN = -1);
	CStdFixed(int iM, int iN, double dblVal, int iMultM = -1, int iMultN = -1);
	CStdFixed(int iM, int iN, long lVal, int iMultM = -1, int iMultN = -1);
	~CStdFixed(void);
	
	/**
	\brief	Gets the number of bits to the left of the decimal point. 
	
	\author	dcofer
	\date	4/1/2011
	
	\return	bits.
	**/
	int M() {return m_iM;};

	/**
	\brief	Gets the number of bits to the right of the decimal point. 
	
	\author	dcofer
	\date	4/1/2011
	
	\return	bits.
	**/
	int N() {return m_iN;};

	/**
	\brief	Number of bits to the left of the decimal point during multiplication. 
	
	\author	dcofer
	\date	4/1/2011
	
	\return	bits.
	**/
	int MultM() {return m_iMultiplyM;};

	/**
	\brief	Number of bits to the right of the decimal point during multiplication. 
	
	\author	dcofer
	\date	4/1/2011
	
	\return	bits.
	**/
	int MultN() {return m_iMultiplyN;};

	/**
	\brief	Gets the fixed-point representation of the number.
	
	\author	dcofer
	\date	4/1/2011
	
	\return	fixed-point number.
	**/
	long FixedVal() {return m_lFixed;};

	/**
	\brief	Sets the fixed-point representation of the number.
	
	\author	dcofer
	\date	4/1/2011
	
	\param	lVal	The new value. 
	**/
	void Fixed(long lVal)
	{
		m_lFixed = lVal;
		m_dblVal = (float) *this;
		m_fltVal = m_dblVal;
	}


	/**
	\brief	Gets the floating-point representation of the number.
	
	\author	dcofer
	\date	4/1/2011
	
	\return	The number.
	**/
	float GetFloat() {return m_fltVal;};

	/**
	\brief	Gets the pointer to the floating-point representation of the number.
	
	\author	dcofer
	\date	4/1/2011
	
	\return	Pointer to float number.
	**/
	float *GetFloatPtr() {return &m_fltVal;};

	/**
	\brief	Gets the double representation of the number.
	
	\author	dcofer
	\date	4/1/2011
	
	\return	The double number.
	**/
	double GetDouble() {return m_dblVal;};

	/**
	\brief	Gets the pointer to the floating-point representation of the number.
	
	\author	dcofer
	\date	4/1/2011
	
	\return	Pointer to double number.
	**/
	double *GetDoublePtr() {return &m_dblVal;};

	/**
	\brief	Gets the pointer to the fixed-point representation of the number.
	
	\author	dcofer
	\date	4/1/2011
	
	\return	Pointer to the fixed-point number.
	**/
	long *GetLongPtr() {return &m_lFixed;};

	virtual void Configure(int iM, int iN, int iMultM = -1, int iMultN = -1);

	//Set operators

	/**
	\brief	Assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	A shallow copy of this object.
	**/
	CStdFixed operator=(float fltVal)
	{
		Fixed( (long) Convert((double) fltVal));
		return *this;
	}

	/**
	\brief	Assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	A shallow copy of this object.
	**/
	CStdFixed operator=(double dblVal)
	{
		Fixed( (long) Convert(dblVal));
		return *this;
	}

	/**
	\brief	Assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	A shallow copy of this object.
	**/
	CStdFixed operator=(long lVal)
	{
		Fixed( (long) lVal);
		return *this;
	}

	/**
	\brief	Assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	A shallow copy of this object.
	**/
	CStdFixed operator=(int iVal)
	{
		Fixed( (long) iVal);
		return *this;
	}

	//Addition operators

	/**
	\brief	Addition operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator+(fxB);		
	}

	/**
	\brief	Addition operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator+(fxB);		
	}

	/**
	\brief	Addition operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator+(fxB);		
	}

	/**
	\brief	Addition operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator+(fxB);		
	}

	/**
	\brief	Addition operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+(const CStdFixed &fxB)
	{
		CStdFixed fxC(m_iM, m_iN);

		long lVal = m_lFixed + fxB.m_lFixed;

		//If the MSB is 1 then it is a negative value and we need
		//to make sure we mask it so the other MSB's remain negative.
		//If the MSB is 0 then it is positive and we mask it to make sure the
		//other values are 0.
		if(lVal & m_lAddTestMask)
			fxC.Fixed(m_lAddNegMask | lVal);
		else
			fxC.Fixed(m_lAddPosMask & lVal);

		return fxC;		
	}

	//+= operators

	/**
	\brief	Addition assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator+=(fxB);		
	}

	/**
	\brief	Addition assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator+=(fxB);		
	}

	/**
	\brief	Addition assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator+=(fxB);		
	}

	/**
	\brief	Addition assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator+=(fxB);		
	}

	/**
	\brief	Addition assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator+=(const CStdFixed &fxB)
	{
		long lVal = m_lFixed + fxB.m_lFixed;

		//If the MSB is 1 then it is a negative value and we need
		//to make sure we mask it so the other MSB's remain negative.
		//If the MSB is 0 then it is positive and we mask it to make sure the
		//other values are 0.
		if(lVal & m_lAddTestMask)
			this->Fixed(m_lAddNegMask | lVal);
		else
			this->Fixed(m_lAddPosMask & lVal);

		return *this;		
	}


	//Subtraction operators

	/**
	\brief	Negation operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator-(fxB);		
	}

	/**
	\brief	Negation operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator-(fxB);		
	}

	/**
	\brief	Negation operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator-(fxB);		
	}

	/**
	\brief	Negation operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator-(fxB);		
	}

	/**
	\brief	Negation operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-(const CStdFixed &fxB)
	{
		CStdFixed fxC(m_iM, m_iN);

		long lVal = m_lFixed - fxB.m_lFixed;

		//If the MSB is 1 then it is a negative value and we need
		//to make sure we mask it so the other MSB's remain negative.
		//If the MSB is 0 then it is positive and we mask it to make sure the
		//other values are 0.
		if(lVal & m_lAddTestMask)
			fxC.Fixed(m_lAddNegMask | lVal);
		else
			fxC.Fixed(m_lAddPosMask & lVal);

		return fxC;		
	}



	//-= operators

	/**
	\brief	Subtraction assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator-=(fxB);		
	}

	/**
	\brief	Subtraction assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator-=(fxB);		
	}

	/**
	\brief	Subtraction assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator-=(fxB);		
	}

	/**
	\brief	Subtraction assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator-=(fxB);		
	}

	/**
	\brief	Subtraction assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator-=(const CStdFixed &fxB)
	{
		long lVal = m_lFixed - fxB.m_lFixed;

		//If the MSB is 1 then it is a negative value and we need
		//to make sure we mask it so the other MSB's remain negative.
		//If the MSB is 0 then it is positive and we mask it to make sure the
		//other values are 0.
		if(lVal & m_lAddTestMask)
			this->Fixed(m_lAddNegMask | lVal);
		else
			this->Fixed(m_lAddPosMask & lVal);

		return *this;		
	}


	//Multiplication operators

	/**
	\brief	Multiplication operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*(const CStdFixed &fxB)
	{
		CStdFixed fxC(m_iMultiplyM, m_iMultiplyN);
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		__int64 lC = ((__int64) m_lFixed * (__int64) fxB.m_lFixed);
		fxC.Fixed((long) (lC >> iNShift));
		return fxC;
	}



	//*= operators

	/**
	\brief	Multiplication assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator*(fxB);		
	}

	/**
	\brief	Multiplication assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator*=(const CStdFixed &fxB)
	{
		__int64 lC = ((__int64) m_lFixed * (__int64) fxB.m_lFixed);
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		this->Fixed( (long) (lC >> iNShift));
		return *this;
	}



	//Division operators

	/**
	\brief	Division operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator/(fxB);		
	}

	/**
	\brief	Division operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator/(fxB);		
	}

	/**
	\brief	Division operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator/(fxB);		
	}

	/**
	\brief	Division operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator/(fxB);		
	}

	/**
	\brief	Division operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/(const CStdFixed &fxB)
	{
		CStdFixed fxC(m_iMultiplyM, m_iMultiplyN);
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		__int64 lC = ((__int64) m_lFixed << iNShift);
		__int64 lD = lC/((__int64)fxB.m_lFixed);
		fxC.Fixed( (long) lD);
		return fxC;
	}

	/**
	\brief	Division assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator/=(fxB);		
	}

	/**
	\brief	Division assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator/=(fxB);		
	}

	/**
	\brief	Division assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator/=(fxB);		
	}

	/**
	\brief	Division assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator/=(fxB);		
	}

	/**
	\brief	Division assignment operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fxB	The fx b. 
	
	\return	The result of the operation.
	**/
	CStdFixed operator/=(const CStdFixed &fxB)
	{
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		__int64 lC = ((__int64) m_lFixed << iNShift);
		__int64 lD = lC/((__int64)fxB.m_lFixed);
		this->Fixed( (long) lD);
		return *this;
	}

	//Cast operators

	/**
	\brief	float casting operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	The result of the operation.
	**/
	operator const float()
	{return (float) Convert(m_lFixed);}

	/**
	\brief	double casting operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	The result of the operation.
	**/
	operator const double()
	{return (double) Convert(m_lFixed);}

	/**
	\brief	long casting operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	The result of the operation.
	**/
	operator const long()
	{return m_lFixed;}

	/**
	\brief	int casting operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	The result of the operation.
	**/
	operator const int()
	{return (int) m_lFixed;}



	//== operators

	/**
	\brief	Equality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	true if the parameters are considered equivalent.
	**/
	bool operator==(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator==(fxB);		
	}

	/**
	\brief	Equality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	true if the parameters are considered equivalent.
	**/
	bool operator==(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator==(fxB);		
	}

	/**
	\brief	Equality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	true if the parameters are considered equivalent.
	**/
	bool operator==(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator==(fxB);		
	}

	/**
	\brief	Equality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	true if the parameters are considered equivalent.
	**/
	bool operator==(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator==(fxB);		
	}

	/**
	\brief	Equality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param [in,out]	fxB	The fx b. 
	
	\return	true if the parameters are considered equivalent.
	**/
	bool operator==(CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA == dblB )
			return true;
		else
			return false;
	}


	//< operators

	/**
	\brief	Less-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	true if the first parameter is less than the second.
	**/
	bool operator<(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator<(fxB);		
	}

	/**
	\brief	Less-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	true if the first parameter is less than the second.
	**/
	bool operator<(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator<(fxB);		
	}

	/**
	\brief	Less-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	true if the first parameter is less than the second.
	**/
	bool operator<(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator<(fxB);		
	}

	/**
	\brief	Less-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	true if the first parameter is less than the second.
	**/
	bool operator<(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator<(fxB);		
	}

	/**
	\brief	Less-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param [in,out]	fxB	The fx b. 
	
	\return	true if the first parameter is less than the second.
	**/
	bool operator<( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA < dblB )
			return true;
		else
			return false;
	}

	//<= operators

	/**
	\brief	Less-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	true if the first parameter is less than or equal to the second.
	**/
	bool operator<=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator<=(fxB);		
	}

	/**
	\brief	Less-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	true if the first parameter is less than or equal to the second.
	**/
	bool operator<=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator<=(fxB);		
	}

	/**
	\brief	Less-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	true if the first parameter is less than or equal to the second.
	**/
	bool operator<=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator<=(fxB);		
	}

	/**
	\brief	Less-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	true if the first parameter is less than or equal to the second.
	**/
	bool operator<=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator<=(fxB);		
	}

	/**
	\brief	Less-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param [in,out]	fxB	The fx b. 
	
	\return	true if the first parameter is less than or equal to the second.
	**/
	bool operator<=( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA <= dblB )
			return true;
		else
			return false;
	}



	//> operators

	/**
	\brief	Greater-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	true if the first parameter is greater than to the second.
	**/
	bool operator>(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator>(fxB);		
	}

	/**
	\brief	Greater-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	true if the first parameter is greater than to the second.
	**/
	bool operator>(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator>(fxB);		
	}

	/**
	\brief	Greater-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	true if the first parameter is greater than to the second.
	**/
	bool operator>(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator>(fxB);		
	}

	/**
	\brief	Greater-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	true if the first parameter is greater than to the second.
	**/
	bool operator>(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator>(fxB);		
	}

	/**
	\brief	Greater-than comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param [in,out]	fxB	The fx b. 
	
	\return	true if the first parameter is greater than to the second.
	**/
	bool operator>( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA > dblB )
			return true;
		else
			return false;
	}



	//>= operators

	/**
	\brief	Greater-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	true if the first parameter is greater than or equal to the second.
	**/
	bool operator>=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator>=(fxB);		
	}

	/**
	\brief	Greater-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	true if the first parameter is greater than or equal to the second.
	**/
	bool operator>=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator>=(fxB);		
	}

	/**
	\brief	Greater-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	true if the first parameter is greater than or equal to the second.
	**/
	bool operator>=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator>=(fxB);		
	}

	/**
	\brief	Greater-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	true if the first parameter is greater than or equal to the second.
	**/
	bool operator>=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator>=(fxB);		
	}

	/**
	\brief	Greater-than-or-equal comparison operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param [in,out]	fxB	The fx b. 
	
	\return	true if the first parameter is greater than or equal to the second.
	**/
	bool operator>=( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA >= dblB )
			return true;
		else
			return false;
	}



	//!= operators

	/**
	\brief	Inequality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	fltVal	The value. 
	
	\return	true if the parameters are not considered equivalent.
	**/
	bool operator!=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator!=(fxB);		
	}

	/**
	\brief	Inequality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The value. 
	
	\return	true if the parameters are not considered equivalent.
	**/
	bool operator!=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator!=(fxB);		
	}

	/**
	\brief	Inequality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iVal	The value. 
	
	\return	true if the parameters are not considered equivalent.
	**/
	bool operator!=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator!=(fxB);		
	}

	/**
	\brief	Inequality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	lVal	The value. 
	
	\return	true if the parameters are not considered equivalent.
	**/
	bool operator!=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator!=(fxB);		
	}

	/**
	\brief	Inequality operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param [in,out]	fxB	The fx b. 
	
	\return	true if the parameters are not considered equivalent.
	**/
	bool operator!=( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA != dblB )
			return true;
		else
			return false;
	}

	/**
	\brief	stream output operator.
	
	\author	dcofer
	\date	5/3/2011
	
	\param [in,out]	output	The output. 
	
	\return	The output stream.
	**/
	ostream& operator<<(ostream& output) {return output;};

};

}				//StdUtils

//CStdFixed operator+(float &fltA, CStdFixed fxB);
