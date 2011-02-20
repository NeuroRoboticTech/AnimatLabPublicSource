#pragma once

class STD_UTILS_PORT CStdFixed
{
protected:
	int m_iM;
	int m_iN;

	//The number of digits to shift when doing a multiplication.
	//For this we assume we are using some standard M.N format, but
	//sometimes we may have to multiply be a different format. When that
	//multiplication occurs we must always output things in the M.N format though.
	int m_iMultiplyM;
	int m_iMultiplyN;

	long m_lFixed;
	double m_dblVal;  //Actual value in float variable
	float m_fltVal;   //Actual value in double variable.

	//This is the maximum integer value attainable by the fixed number.
	unsigned long m_lMaxInt;
	//This is the maximum real value attainable by the fixed number.
	unsigned long m_lMaxReal;

	double m_dblConvertReal;
	double m_dblConvertInt;

	//This is used to make certain that the number cannot go above the maximum number of bits.
	long m_lAddPosMask;
	long m_lAddNegMask;
	long m_lAddTestMask;

	long Convert(double dblVal)
	{	
		return (long) ((dblVal)*m_dblConvertInt);
	}

	double Convert(long lVal)
	{	
		return (lVal*m_dblConvertReal);
	}

public:
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
	
	int M() {return m_iM;};
	int N() {return m_iN;};
	int MultM() {return m_iMultiplyM;};
	int MultN() {return m_iMultiplyN;};

	long FixedVal() {return m_lFixed;};

	void Fixed(long lVal)
	{
		m_lFixed = lVal;
		m_dblVal = (float) *this;
		m_fltVal = m_dblVal;
	}

	float GetFloat() {return m_fltVal;};
	float *GetFloatPtr() {return &m_fltVal;};

	double GetDouble() {return m_dblVal;};
	double *GetDoublePtr() {return &m_dblVal;};

	long *GetLongPtr() {return &m_lFixed;};

	virtual void Configure(int iM, int iN, int iMultM = -1, int iMultN = -1);

	//Set operators
	CStdFixed operator=(float fltVal)
	{
		Fixed( (long) Convert((double) fltVal));
		return *this;
	}

	CStdFixed operator=(double dblVal)
	{
		Fixed( (long) Convert(dblVal));
		return *this;
	}

	CStdFixed operator=(long lVal)
	{
		Fixed( (long) lVal);
		return *this;
	}

	CStdFixed operator=(int iVal)
	{
		Fixed( (long) iVal);
		return *this;
	}

	//Addition operators
	CStdFixed operator+(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator+(fxB);		
	}

	CStdFixed operator+(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator+(fxB);		
	}

	CStdFixed operator+(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator+(fxB);		
	}

	CStdFixed operator+(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator+(fxB);		
	}

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
	CStdFixed operator+=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator+=(fxB);		
	}

	CStdFixed operator+=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator+=(fxB);		
	}

	CStdFixed operator+=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator+=(fxB);		
	}

	CStdFixed operator+=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator+=(fxB);		
	}

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
	CStdFixed operator-(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator-(fxB);		
	}

	CStdFixed operator-(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator-(fxB);		
	}

	CStdFixed operator-(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator-(fxB);		
	}

	CStdFixed operator-(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator-(fxB);		
	}

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
	CStdFixed operator-=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator-=(fxB);		
	}

	CStdFixed operator-=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator-=(fxB);		
	}

	CStdFixed operator-=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator-=(fxB);		
	}

	CStdFixed operator-=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator-=(fxB);		
	}

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
	CStdFixed operator*(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*(const CStdFixed &fxB)
	{
		CStdFixed fxC(m_iMultiplyM, m_iMultiplyN);
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		__int64 lC = ((__int64) m_lFixed * (__int64) fxB.m_lFixed);
		fxC.Fixed((long) (lC >> iNShift));
		return fxC;
	}



	//*= operators
	CStdFixed operator*=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator*(fxB);		
	}

	CStdFixed operator*=(const CStdFixed &fxB)
	{
		__int64 lC = ((__int64) m_lFixed * (__int64) fxB.m_lFixed);
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		this->Fixed( (long) (lC >> iNShift));
		return *this;
	}



	//Division operators
	CStdFixed operator/(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator/(fxB);		
	}

	CStdFixed operator/(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator/(fxB);		
	}

	CStdFixed operator/(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator/(fxB);		
	}

	CStdFixed operator/(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator/(fxB);		
	}

	CStdFixed operator/(const CStdFixed &fxB)
	{
		CStdFixed fxC(m_iMultiplyM, m_iMultiplyN);
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		__int64 lC = ((__int64) m_lFixed << iNShift);
		__int64 lD = lC/((__int64)fxB.m_lFixed);
		fxC.Fixed( (long) lD);
		return fxC;
	}



	///= operators
	CStdFixed operator/=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator/=(fxB);		
	}

	CStdFixed operator/=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator/=(fxB);		
	}

	CStdFixed operator/=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator/=(fxB);		
	}

	CStdFixed operator/=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator/=(fxB);		
	}

	CStdFixed operator/=(const CStdFixed &fxB)
	{
		int iNShift = (m_iN + fxB.m_iN) - m_iMultiplyN;

		__int64 lC = ((__int64) m_lFixed << iNShift);
		__int64 lD = lC/((__int64)fxB.m_lFixed);
		this->Fixed( (long) lD);
		return *this;
	}

	//Cast operators
	operator const float()
	{return (float) Convert(m_lFixed);}

	operator const double()
	{return (double) Convert(m_lFixed);}

	operator const long()
	{return m_lFixed;}

	operator const int()
	{return (int) m_lFixed;}



	//== operators
	BOOL operator==(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator==(fxB);		
	}

	BOOL operator==(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator==(fxB);		
	}

	BOOL operator==(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator==(fxB);		
	}

	BOOL operator==(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator==(fxB);		
	}

	BOOL operator==(CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA == dblB )
			return TRUE;
		else
			return FALSE;
	}


	//< operators
	BOOL operator<(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator<(fxB);		
	}

	BOOL operator<(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator<(fxB);		
	}

	BOOL operator<(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator<(fxB);		
	}

	BOOL operator<(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator<(fxB);		
	}

	BOOL operator<( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA < dblB )
			return TRUE;
		else
			return FALSE;
	}

	//<= operators
	BOOL operator<=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator<=(fxB);		
	}

	BOOL operator<=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator<=(fxB);		
	}

	BOOL operator<=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator<=(fxB);		
	}

	BOOL operator<=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator<=(fxB);		
	}

	BOOL operator<=( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA <= dblB )
			return TRUE;
		else
			return FALSE;
	}



	//> operators
	BOOL operator>(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator>(fxB);		
	}

	BOOL operator>(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator>(fxB);		
	}

	BOOL operator>(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator>(fxB);		
	}

	BOOL operator>(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator>(fxB);		
	}

	BOOL operator>( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA > dblB )
			return TRUE;
		else
			return FALSE;
	}



	//>= operators
	BOOL operator>=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator>=(fxB);		
	}

	BOOL operator>=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator>=(fxB);		
	}

	BOOL operator>=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator>=(fxB);		
	}

	BOOL operator>=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator>=(fxB);		
	}

	BOOL operator>=( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA >= dblB )
			return TRUE;
		else
			return FALSE;
	}



	//!= operators
	BOOL operator!=(const float fltVal)
	{
		CStdFixed fxB(m_iM, m_iN, (double) fltVal);
		return this->operator!=(fxB);		
	}

	BOOL operator!=(const double dblVal)
	{
		CStdFixed fxB(m_iM, m_iN, dblVal);
		return this->operator!=(fxB);		
	}

	BOOL operator!=(const int iVal)
	{
		CStdFixed fxB(m_iM, m_iN, (long) iVal);
		return this->operator!=(fxB);		
	}

	BOOL operator!=(const long lVal)
	{
		CStdFixed fxB(m_iM, m_iN, lVal);
		return this->operator!=(fxB);		
	}

	BOOL operator!=( CStdFixed &fxB)
	{
		double dblA = (double) *this;
		double dblB = (double) fxB;

		if( dblA != dblB )
			return TRUE;
		else
			return FALSE;
	}

	ostream& operator<<(ostream& output) {return output;};

};


//CStdFixed operator+(float &fltA, CStdFixed fxB);
