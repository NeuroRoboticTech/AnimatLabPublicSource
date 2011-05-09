/**
\file	StdLookupTable.h

\brief	Declares a standard lookup table class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Standard lookup table. 

\details This class allows you to specify a sequence of lines to implement a curve. You can then 
specify an x point on that curve, and it will calculate the y point from the appropriate line. 

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT CStdLookupTable 
{
protected:
	/// The initial points for the lines
	CStdArray< CStdDPoint > m_aryInitialPoints;

	/// Size of the table
	int m_iTableSize;

	/// The delta
	double m_dblDelta;

	/// The start x coordinate
	double m_dblStartX;

	/// The end x coordinate
	double m_dblEndX;

	/// The array of slopes
	double *m_aryM;

	/// The array of intercepts
	double *m_aryB;

	/// true to use low limit value
	BOOL m_bUseLowLimitValue;

	/// true to use high limit value
	BOOL m_bUseHighLimitValue;

	/// The double low limit value
	double m_dblLowLimitValue;

	/// The double high limit value
	double m_dblHighLimitValue;

public:
	CStdLookupTable();
	virtual ~CStdLookupTable();

	/**
	\brief	Gets the initial point count.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	count.
	**/
	int InitialPointCount() {return m_aryInitialPoints.GetSize();};

	/**
	\brief	Gets the initial point at the specified index.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	iIndex	Zero-based index of the initial point. 
	
	\return	.
	**/
	CStdDPoint InitialPoint(int iIndex) {return m_aryInitialPoints[iIndex];};

	/**
	\brief	Gets the table size.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	size.
	**/
	int TableSize() {return m_iTableSize;};

	/**
	\brief	Gets the delta.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	delta.
	**/
	double Delta() {return m_dblDelta;};

	/**
	\brief	Gets the start x coordinate.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	x coordinate.
	**/
	double StartX() {return m_dblStartX;};

	/**
	\brief	Gets the ends x coordinate.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	x corrdinate.
	**/
	double EndX() {return m_dblEndX;};

	/**
	\brief	Gets the use low limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	true if it succeeds, false if it fails.
	**/
	BOOL UseLowLimitValue() {return m_bUseLowLimitValue;};

	/**
	\brief	Sets the use low limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	bVal	true to value. 
	**/
	void UseLowLimitValue(BOOL bVal) {m_bUseLowLimitValue = bVal;};

	/**
	\brief	Gets the use high limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	true if it succeeds, false if it fails.
	**/
	BOOL UseHighLimitValue() {return m_bUseHighLimitValue;};

	/**
	\brief	Sets the use high limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	bVal	true to value. 
	**/
	void UseHighLimitValue(BOOL bVal) {m_bUseHighLimitValue = bVal;};

	/**
	\brief	Gets the low limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	low limit.
	**/
	double LowLimitValue() {return m_dblLowLimitValue;};

	/**
	\brief	Sets the low limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The double value. 
	**/
	void LowLimitValue(double dblVal) {m_dblLowLimitValue = dblVal;};

	/**
	\brief	Gets the high limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	high limit.
	**/
	double HighLimitValue() {return m_dblHighLimitValue;};

	/**
	\brief	Sets the high limit value.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	dblVal	The double value. 
	**/
	void HighLimitValue(double dblVal) {m_dblHighLimitValue = dblVal;};

	void AddPoint(double dblX, double dblY);
	void Initialize();
	void Clear();
	double Evaluate(double dblX);

	virtual void Load(CStdXml &oXml, string strParamName, bool bThrowError = false);
	virtual void Save(CStdXml &oXml, string strParamName);
};

}				//StdUtils
