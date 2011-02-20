// IDSQLColumn.h: interface for the CIDSQLColumn class.
//
// This column class has been represented in orthodox canonical form of
// class and contains operator functions for use with STL containers, etc.
//
// Author: Ian Duff - 30 December 2001
// Email : ianduff@bold.net.au
// Thanks to: 
// Dave Merner - direct SQL calls with ODBC
//
//////////////////////////////////////////////////////////////////////

#if !defined(   __IDSQLCOLUMN_H__ )
#define         __IDSQLCOLUMN_H__ 

// ---------- Module includes

#include <string>
#include <algorithm>

// ---------- Module namespaces 

using namespace std;

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/******************************************************************************
* This class has been modified to include the necessary operators
* required as canonical form of class for use with STL containers,etc
*
* As such, you may now use the STL algorithms, eg sort():
*
******************************************************************************/

class STD_UTILS_PORT CIDSQLColumn  
{
public:
	CIDSQLColumn();      
  CIDSQLColumn( const CIDSQLColumn& );

	virtual ~CIDSQLColumn();    // Default destructor

	CIDSQLColumn& operator=( const CIDSQLColumn& );
  bool operator<( const CIDSQLColumn& ); 
  bool operator==( const CIDSQLColumn& ) ;

public:
  int         m_nType;    // Column type
	int         m_nCol;     // Column number
  string      m_strValue; // Column value	
};

#endif // !defined( __IDSQLCOLUMN_H__ )
