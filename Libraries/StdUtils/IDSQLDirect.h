// IDSQLDirect.h: interface for the CIDSQLDirect class.
//
// The main use of this class is to connect to an ODBC data source
// and issue SQL commands.  This class uses STL mechanisms
// instead of the CPtrArray MFC class.
//
// Author: Ian Duff - 30 December 2001
// Email : ianduff@bold.net.au
//
// Many thanks to Dave Merner for his article on direct SQL calls with ODBC
//////////////////////////////////////////////////////////////////////

#if !defined(   __IDSQLDIRECT_H__ )
#define         __IDSQLDIRECT_H__ 

// ---------- Module includes

#include "IDSQLColumn.h"
#include <vector>       // We'll store a vector of columns
#include <algorithm>    // find, etc
#include <functional>   // STL function access
#include <fstream>		// ifstream, etc
#include <ctype.h>      // toupper macros, etc

// ---------- Module namespaces 

using namespace std;

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/******************************************************************************
* Class CIDSQLDirect
*
* This class encompasses most of the required functionality 
* for issuing SQL commands through an ODBC connection.
*
* The CIDSQLColumn operators will now allow you to use other STL
* mechanisms like sort:
* sort(m_vColArray.begin(), m_vColArray.end() );
******************************************************************************/

class STD_UTILS_PORT CIDSQLDirect  
{
public:
	CIDSQLDirect();
	virtual ~CIDSQLDirect();

	void AddColumn( int, string );
	CIDSQLDirect& Close();

	bool Commit();
	int Connect( LPCSTR );
	int ConnectDSN( LPCSTR );
  int ConnectDSN( string s ) { return( ConnectDSN( s.c_str() ) ); }
	void DisplayError();
	int ExecuteSQL( LPCSTR);
	int ExecuteQueryFile( string );
	int Fetch();
	int FindColumn( int );
	string GetCol( int );
	string GetCol( string, string = "" );
	CIDSQLColumn *GetColumn( int );
	int GetColumnID( string, string );
	string GetColumnName( int );
	int GetColumnType( int );
	string GetError();
	int GetNumColumns();
  CIDSQLDirect& PrintColumnNames();
	bool Rollback();
  CIDSQLDirect &SetDSN( LPCSTR );
	CIDSQLDirect &SetPassword( LPCSTR );
	CIDSQLDirect &SetUserName( LPCSTR );

	inline void CIDSQLDirect::SupressErrors( bool bSupress = true ) 
    {  m_bSupressErrors = bSupress; }
	string TrimRight( LPCSTR );

protected:
	void Init();

	SQLRETURN       m_nReturn;                  // Internal SQL error code
  string          m_strSQL;                   // SQL string
  string          m_strErrorMsg;	            // SQL error message
	HENV            m_hEnv;                     // Handle to environment
	HDBC            m_hDBC;                     // Handle to database connection
	HSTMT           m_hStmt;                    // Handle to Statement

  vector< CIDSQLColumn > m_vColArray;         // vector of columns
  vector< CIDSQLColumn >::iterator m_colIter; // column iterator

	bool m_bSupressErrors;           // Ignore errors if set to true
  string m_strPassword;              // User password
  string m_strUserName;              // User name
  string m_strDsn;                   // DSN connection string
};

#endif // !defined( __IDSQLDIRECT_H__ )
