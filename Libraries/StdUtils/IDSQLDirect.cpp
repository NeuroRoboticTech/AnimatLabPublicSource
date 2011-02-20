// IDSQLDirect.cpp: implementation of the CIDSQLDirect class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIDSQLDirect::CIDSQLDirect()
{
    TRACE_DEBUG("CIDSQLDirect constructor\n");
    Init();
}

/********************************************************************
* Function      : Class destructor
* Description   : Frees resources back to the system on completion.
* Parameters    : None
* Returns       : 0 (zero)
* Notes         :
* The calls to SQLFreeStatement, SQLFreeConnect and SQLFreeEnv are deprecated
* and are now replaced with SQLFreeHandle as appropriate.
* SQLFreeHandle returns SQL_SUCCESS, SQL_ERROR, or SQL_INVALID_HANDLE.
* SQLDisconnect returns SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_ERROR, or SQL_INVALID_HANDLE.
* See: http://msdn.microsoft.com/library/default.asp?url=/library/en-us/odbc/htm/odbcsqlfreehandle.asp
* The call to KillCols is no longer required, as it is catered for in the STL classes.
*********************************************************************/
CIDSQLDirect::~CIDSQLDirect()
{
    TRACE_DEBUG("CIDSQLDirect destructor\n");

    if( m_hStmt != NULL )
        m_nReturn = ::SQLFreeHandle( SQL_HANDLE_STMT, m_hStmt );

	if( m_hDBC != NULL ) {
        m_nReturn = ::SQLDisconnect( m_hDBC );
        m_nReturn = ::SQLFreeHandle( SQL_HANDLE_DBC,  m_hDBC );
	}

    if( m_hEnv!=NULL )
        m_nReturn = ::SQLFreeHandle( SQL_HANDLE_ENV, m_hEnv );
}

/********************************************************************
* Function      : Init
* Description   : Initialises member variables on instantiation.
* Parameters    : None
* Returns       : N/A
* Notes         : None
*********************************************************************/
void CIDSQLDirect::Init()
{
	m_strErrorMsg       = "";
	m_strSQL            = "";
	m_hDBC              = NULL;
	m_hEnv              = NULL;
	m_hStmt             = NULL;
	m_bSupressErrors    = false;
    m_strPassword       = "";
    m_strUserName       = "";
    m_strDsn            = "";  
    m_nReturn           = SQL_SUCCESS;
    m_vColArray.clear();
    m_colIter = m_vColArray.begin();
}

/********************************************************************
* Function      : SetPassword
* Description   : Sets internal class password string
* Parameters    : A NULL terminated password string
* Returns       : this
* Notes         : None
*********************************************************************/
CIDSQLDirect& CIDSQLDirect::SetPassword( LPCSTR strPassword )
{
    m_strPassword = strPassword;
    return( *this );
}

/********************************************************************
* Function      : SetUserName
* Description   : Sets internal class user name string
* Parameters    : A NULL terminated user name string
* Returns       : this
* Notes         : None
*********************************************************************/
CIDSQLDirect& CIDSQLDirect::SetUserName( LPCSTR strUserName )
{
    m_strUserName = strUserName;
    return( *this );
}

/********************************************************************
* Function      : SetDSN
* Description   : Sets internal DSN string
* Parameters    : A NULL terminated DSN string
* Returns       : this
* Notes         : The DSN may/may not contain user name and password
*********************************************************************/
CIDSQLDirect& CIDSQLDirect::SetDSN( LPCSTR strDsn)
{
    m_strDsn = strDsn;
    return( *this );
}

/********************************************************************
* Function      : AddColumn
* Description   : Adds a CIDSQLColumn class to the column vector
* Parameters    : 
* 1. nCol - The number of the column
* 2. svData - The data to insert into the column
* Returns       : void
* Notes         : None
*********************************************************************/
void CIDSQLDirect::AddColumn(int nCol, string svData)
{
    CIDSQLColumn column;
  
    column.m_nCol = nCol;
    column.m_strValue = svData;
    m_vColArray.push_back( column );    
#if defined ( _DEBUG )
    int nCols = m_vColArray.size();
    TRACE_DEBUG("Columns vector contains (" + STR(nCols) + ") elements\r\n");
#endif
}

/********************************************************************
* Function      : Close
* Description   : Releases resources back to the system and empties 
*                 the vector of column classes.
* Parameters    : None
* Returns       : this
* Notes         : None.
*********************************************************************/
CIDSQLDirect& CIDSQLDirect::Close()
{
	if( m_hStmt != NULL ) {
        m_nReturn = ::SQLFreeHandle( SQL_HANDLE_STMT, m_hStmt );
        m_vColArray.clear();
        m_nReturn = ::SQLAllocHandle( SQL_HANDLE_STMT, m_hDBC, &m_hStmt );
	}
    return( *this );
}

/********************************************************************
* Function      : Commit
* Description   : Utility function for database commit (transactional)
* Parameters    : None.
* Returns       : true
* Notes         : None
*********************************************************************/
bool CIDSQLDirect::Commit()
{
    if( ExecuteSQL( "COMMIT" ) != SQL_SUCCESS ) 	{
		m_nReturn = SQLEndTran( SQL_HANDLE_DBC, m_hDBC, SQL_COMMIT);
		return (( m_nReturn == SQL_SUCCESS ) || ( m_nReturn == SQL_SUCCESS_WITH_INFO ));
	}
	return( true );
}

/********************************************************************
* Function      : ExecuteSQL
* Description   : Executes the incoming string as an SQL query
* Parameters    : A NULL terminated SQL query string
* Returns       : 0 on success, otherwise fail.
* Notes         : None
*********************************************************************/
int CIDSQLDirect::ExecuteSQL( LPCSTR strSQL)
{
    m_strSQL = strSQL;

	if( m_hStmt != NULL )
        Close();

    int nRet=::SQLExecDirect( m_hStmt, (SQLTCHAR *)m_strSQL.c_str(),m_strSQL.length() );
	if( ( nRet != SQL_SUCCESS && nRet != SQL_SUCCESS_WITH_INFO ) && !m_bSupressErrors ) 
		DisplayError();

	return( nRet );
}

/********************************************************************
* Function      : DisplayError
* Description   : Displays an error message in a Message box
* Parameters    : A NULL terminated DSN string
* Returns       : void
* Notes         : 
* 1. Displays if the boolean m_bSupressErrors is set to true.
* 2. If you don't want the message box, send the text to cout or cerr
*********************************************************************/
void CIDSQLDirect::DisplayError()
{
    if( !m_bSupressErrors ) 
		{
        string  strError = GetError();

				if(m_strSQL.length()) 
					strError += "\r\nSQL: " + m_strSQL;

				THROW_ERROR(m_nReturn, strError);
    }
}

/********************************************************************
* Function      : GetError
* Description   : Extracts an error message from ODBC
* Parameters    : None
* Returns       : A populated string containing error text.
* Notes         : 
* SQLError is deprecated. Using SQLGetDiagRec instead.
* Handle types may be:
* SQL_HANDLE_ENV
* SQL_HANDLE_DBC
* SQL_HANDLE_STMT
* SQL_HANDLE_DESC 
*
* SQLSTATES: See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/odbc/htm/odbcodbc_error_codes.asp
* TODO: Change to SQLGetDiagRec
*********************************************************************/
string CIDSQLDirect::GetError()
{
		SQLCHAR     Msg[ SQL_MAX_MESSAGE_LENGTH ] = "";
    SQLCHAR     SqlState[ 6 ] = "";
    SQLINTEGER  NativeError = 0;
    SQLSMALLINT i = 1;
    SQLSMALLINT MsgLen = 0;
    string strRet, strTemp;
		bool bDone = false;

    while(!bDone)
    {
			m_nReturn = SQLGetDiagRec(SQL_HANDLE_STMT, m_hStmt, i, SqlState, 
																&NativeError, Msg, sizeof( Msg ), &MsgLen);
			
			if( (m_nReturn != SQL_NO_DATA) && (m_nReturn != SQL_ERROR) && (m_nReturn != SQL_INVALID_HANDLE) )
			{
				strTemp = (char *)Msg;
				strRet += strTemp + "\r\n";
				i++;
			}
			else
				bDone = true;
    }

    return( strRet );
}


/********************************************************************
* Function      : Connect
* Description   : Connects to the data source
* Parameters    : A NULL terminated source string (eg "Northwind")
* Returns       : 0 = success, otherwise fail
* Notes         : 
* SQLAllocEnv is deprecated. Use SQLAllocHandle instead
* SQLAllocConnect is deprecated. Use SQLAllocHandle instead
* Notes:
* 1. User name may or may not be set.
* 2. Password may or may not be set.
*********************************************************************/
int CIDSQLDirect::Connect( LPCSTR svSource )
{
    int nConnect = SQLAllocHandle( SQL_HANDLE_ENV, SQL_NULL_HANDLE, &m_hEnv );

	if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
        // Set the ODBC version environment attribute
        nConnect = SQLSetEnvAttr( m_hEnv, SQL_ATTR_ODBC_VERSION, (void*)SQL_OV_ODBC3, 0 );
        if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
            // Allocate connection handle
            nConnect = SQLAllocHandle( SQL_HANDLE_DBC, m_hEnv, &m_hDBC );
            // Set login timeout to 5 seconds
            if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
			    SQLSetConnectOption( m_hDBC,SQL_LOGIN_TIMEOUT,5 );                
                // Connect to data source
                string strSource( svSource );

                nConnect=SQLConnect( m_hDBC,
                               ( SQLCHAR *)svSource,
                               SQL_NTS, 
                               ( SQLCHAR *)m_strUserName.c_str(),
                               SQL_NTS,
                               ( SQLCHAR *)m_strPassword.c_str(),
                               SQL_NTS );
                if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
                    // Allocate statement handle
                    nConnect = SQLAllocHandle(SQL_HANDLE_STMT, m_hDBC, &m_hStmt); 

                }
                else    DisplayError();
            }
            else    DisplayError();
        }
        else    DisplayError();
    }
    else    DisplayError();

	return( nConnect );
}

/********************************************************************
* Function      : Fetch
* Description   : Get the next row from a successful SQL execution
* Parameters    : None
* Returns       : 0 = success, otherwise fail.
* Notes         : None
*********************************************************************/
int CIDSQLDirect::Fetch()
{
    m_vColArray.clear();

	// Fetch the next row.
	int nRet=SQLFetch( m_hStmt );
	if( ( nRet != SQL_SUCCESS && nRet != SQL_SUCCESS_WITH_INFO ) && !m_bSupressErrors )
		DisplayError();
	return( nRet );
}

/********************************************************************
* Function      : GetCol
* Description   : Get the column indicated by the incoming number.
* Parameters    : nCol - An integer representing the column number.
* Returns       : A string populated with columnar results
* Notes         : None
*********************************************************************/
string CIDSQLDirect::GetCol(int nCol)
{
	string svValue;

	// Due to the nature of the SQL API, repeated calls to the same column will result in a NULL value passed
	// back into svData. We need to keep track of the columns that have been used already and if so, pass back
	// the data stored in our array.

	int nIndex=FindColumn( nCol );

	if( nIndex==-1 ) {
		// Get the column from the SQL cursor.
		UCHAR svData[8192];
		SDWORD cbDataLen;

		SQLGetData( m_hStmt,                // Statement handle
                    nCol,                   // Column number
                    GetColumnType( nCol ),  // TargetType
                    &svData,                // TargetValuePtr (returned data)
                    sizeof( svData ),       // BufferLength (len of TargetPtr in bytes)
                    &cbDataLen );           // StrLen_or_IndPtr
	
		svValue = TrimRight((char *)svData );
        if( cbDataLen == SQL_NULL_DATA) // <-- added
			svValue = _T("");           // <-- added

		AddColumn( nCol,svValue );
	}
	else {
		// Get the value from the column array.
		svValue = GetColumn( nCol )->m_strValue;
	}

	return svValue;
}

/********************************************************************
* Function      : FindColumn
* Description   : Finds a column number in the vector of columns.
* Parameters    : nCol - An integer representing the column number.
* Returns       : >= 0 = success, -1 = not found
* Notes         : None
*********************************************************************/
int CIDSQLDirect::FindColumn( int nCol )
{
    int nIndex = -1;
    CIDSQLColumn s;
    s.m_nCol = nCol;

    m_colIter = find( m_vColArray.begin(), m_vColArray.end(), s);
    if ( m_colIter != m_vColArray.end() )
            return( m_colIter->m_nCol );
    else    return( -1 );
}

/********************************************************************
* Function      : ConnectDSN
* Description   : Connects to a DSN with the incoming string
* Parameters    : svSource - The DSN name to use
* Returns       : 0 = success, otherwise fail.
* Notes         : 
* Connect with a populated DSN connection string, eg:
* "ODBC;DRIVER={SQL Server};DSN=QantaB;Description=datab;SERVER=METSRV1;UID=user;PWD=;DATABASE=qantab;Regional=No";
*********************************************************************/
int CIDSQLDirect::ConnectDSN(LPCSTR svSource)
{
    int nConnect = SQLAllocHandle( SQL_HANDLE_ENV, SQL_NULL_HANDLE, &m_hEnv );

	if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
        // Set the ODBC version environment attribute
        nConnect = SQLSetEnvAttr( m_hEnv, SQL_ATTR_ODBC_VERSION, (void*)SQL_OV_ODBC3, 0 );
        if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
            // Allocate connection handle
            nConnect = SQLAllocHandle( SQL_HANDLE_DBC, m_hEnv, &m_hDBC );
            // Set login timeout to 5 seconds
            if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
			    SQLSetConnectOption( m_hDBC,SQL_LOGIN_TIMEOUT,5 );                
                // Connect to data source
                string strSource( svSource );
                short shortResult = 0;
                SQLCHAR szOutConnectString[ 1024 ] = "";

                nConnect = SQLDriverConnect( m_hDBC,                // Connection Handle
                                NULL,                           // Window Handle
                                (SQLCHAR*)strSource.c_str(),  // InConnectionString
                                strSource.length(),             // StringLength1
                                szOutConnectString,             // OutConnectionString
                                sizeof( szOutConnectString ),   // Buffer length
                                &shortResult,                   // StringLength2Ptr
                                SQL_DRIVER_NOPROMPT );          // No User prompt
                if( nConnect == SQL_SUCCESS || nConnect == SQL_SUCCESS_WITH_INFO ) {
                    // Allocate statement handle
                    nConnect = SQLAllocHandle(SQL_HANDLE_STMT, m_hDBC, &m_hStmt); 
                }
                else    DisplayError();
            }
            else    DisplayError();
        }
        else    DisplayError();
    }
    else    DisplayError();

    return( nConnect );
}

/********************************************************************
* Function      : GetColumnType
* Description   : Returns the column type for the column indicated 
*                 by the incoming number
* Parameters    : nCol - An integer representing the column number.
* Returns       : >= 0 = success, -1 = not found
* Notes         : None
*********************************************************************/
int CIDSQLDirect::GetColumnType( int nCol )
{
	int nType=SQL_C_DEFAULT;

	// Due to the nature of the SQL API, repeated calls to the same 
	// column will result in a NULL value passed back into svData. 
	// We need to keep track of the columns that have been used 
	// already and if so, pass back the data stored in our array.
	int nIndex=FindColumn( nCol );

	if( nIndex==-1 ) {
		// Get the column from the SQL cursor.
		SQLCHAR svColName[ 256 ];
		SWORD swCol,swType,swScale,swNull;
		UDWORD pcbColDef;

		SQLDescribeCol( m_hStmt,            // Statement handle
                        nCol,               // ColumnNumber
                        svColName,          // ColumnName
                        sizeof( svColName), // BufferLength
                        &swCol,             // NameLengthPtr
                        &swType,            // DataTypePtr
                        &pcbColDef,         // ColumnSizePtr
                        &swScale,           // DecimalDigitsPtr
                        &swNull );          // NullablePtr

		nType=(int)swType;
	}
	else {
		// Get the value from the column array.
		nType=GetColumn( nCol )->m_nType;
	}

	switch( nType ) {
	case SQL_LONGVARBINARY:
		nType = SQL_C_DEFAULT;
		break;
	default:
		nType = SQL_C_CHAR;
		break;
	};
	return( nType );
}

/********************************************************************
* Function      : GetColumn
* Description   : Returns a pointer to the column  class indicated by
*                 by the incoming number
* Parameters    : nCol - An integer representing the column number.
* Returns       : 0 (NULL) = fail, pointer to column otherwise.
* Notes         : None
*********************************************************************/
CIDSQLColumn* CIDSQLDirect::GetColumn( int nCol )
{
	int nIndex=FindColumn( nCol );
	if( nIndex==-1 )
		return 0;
	
	return( &m_vColArray[ nIndex ] );
}

/********************************************************************
* Function      : GetColumnID
* Description   : Returns the column ID for the incoming field and
*                 table strings
* Parameters    : 
* 1. svField - The field name
* 2. svTable - The table name
* Returns       : int
* Notes         : None
*********************************************************************/
int CIDSQLDirect::GetColumnID(string svField, string svTable)
{
	UCHAR svColName[256];
	UCHAR ucOwner[256];
	SWORD swCol,swType,swScale,swNull;
	UDWORD pcbColDef;
	SDWORD wColLen;

	int nCol=1;

	// Get the total # of columns in this query.
	SQLColAttributes( m_hStmt,0,SQL_COLUMN_COUNT,ucOwner,256,&swScale,&wColLen );
	int nMaxCols=(int)wColLen;

	while( nCol<=nMaxCols ) {
		int nRet=SQLDescribeCol( m_hStmt,nCol,svColName,256,&swCol,&swType,&pcbColDef,&swScale,&swNull );
		SQLColAttributes( m_hStmt,nCol,SQL_COLUMN_TABLE_NAME,ucOwner,256,&swScale,&wColLen );

		// If we hit this condition, then our field is not in the dataset.
		if( nRet!=SQL_SUCCESS ) {
			nCol=0;
			break;
		}

		string svName = TrimRight( (char*)svColName );
		string svOwner = TrimRight( ( char *)ucOwner  );

		if( ( svName==svField ) ) {
			if(  !svTable.empty() ) {
				if( svOwner == svTable )
					break;
				else
					nCol++;
			}
			else
				break;
		}
		else
			nCol++;
	}

	return nCol;
}

/********************************************************************
* Function      : TrimRight
* Description   : Utility function that removes trailing spaces from
*                 the incoming string
* Parameters    : inString - The incoming string to trim
* Returns       : Right trimmed string
* Notes         : None
*********************************************************************/
string CIDSQLDirect::TrimRight(LPCSTR inString)
{
    string svValue( inString );
    // Trim spaces from right of string svValue
    std::string::reverse_iterator rev;
    rev = std::find_if( svValue.rbegin(), svValue.rend(), 
                        std::bind1st( std::not_equal_to<char>(), ' ' ) );
    if ( rev != svValue.rend())
        svValue.erase(svValue.rend() - rev);
    svValue.erase( svValue.rend() != rev ? svValue.find_last_of(*rev) + 1 : 0 );

    return( svValue );
}

/********************************************************************
* Function      : GetCol
* Description   : Returns the string containing the column name for 
*                 the incoming field and table strings
* Parameters    : 
* 1. svField - The field name
* 2. svTable - The table name
* Returns       : populated string
* Notes         : None
*********************************************************************/
string CIDSQLDirect::GetCol(string svName, string svTable)
{
	return GetCol( GetColumnID( svName, svTable ) );
}

/********************************************************************
* Function      : GetColumnName
* Description   : Returns the string containing the column name for 
*                 the incoming field and table strings
* Parameters    : nCol - The column number.
* Returns       : populated string
* Notes         : None
*********************************************************************/
string CIDSQLDirect::GetColumnName( int nCol )
{
	UCHAR svColName[256];
	SWORD swCol,swType,swScale,swNull;
	UDWORD pcbColDef;

	SQLDescribeCol( m_hStmt,nCol,svColName,256,&swCol,&swType,&pcbColDef,&swScale,&swNull );

	return TrimRight( ( char *)svColName );
}

/********************************************************************
* Function      : GetNumColumns
* Description   : Returns the number of columns returned after a
*                 query is executed.
* Parameters    : None
* Returns       : The number of columns in the row returned.
* Notes         : None
*********************************************************************/
int CIDSQLDirect::GetNumColumns()
{
	UCHAR ucOwner[256] = "";
	SWORD swScale;
	SDWORD wColLen;

    // Get the total # of columns in this query.
    m_nReturn = ::SQLColAttribute(  m_hStmt,            // Statement handle
                                    0,                  // Column number
                                    SQL_DESC_COUNT,     // Field identifier
                                    ucOwner,            // CharacterAttributePtr
                                    sizeof( ucOwner ),  // BufferLength
                                    &swScale,           // StringLengthPtr
                                    &wColLen );         // NumericAttributePtr

	return( wColLen );
}

/********************************************************************
* Function      : Rollback
* Description   : Rollback function for transactional processing
* Parameters    : None
* Returns       : true
* Notes         : None
*********************************************************************/
bool CIDSQLDirect::Rollback()
{
    if( ExecuteSQL( "ROLLBACK" ) != SQL_SUCCESS ) {
		SQLRETURN retVal = SQLEndTran(SQL_HANDLE_DBC, m_hDBC, SQL_ROLLBACK );
		return (( retVal == SQL_SUCCESS ) || ( retVal == SQL_SUCCESS_WITH_INFO ));
	}
	return( true );
}

/********************************************************************
* Function      : PrintColumnNames
* Description   : Prints all column names to cout
* Parameters    : None
* Returns       : this
* Notes         : None
*********************************************************************/
CIDSQLDirect& CIDSQLDirect::PrintColumnNames()
{
    int nColumns = GetNumColumns();
    string strFetch = "";
    for (int i = 1; i <= nColumns; i++ ) {
        strFetch += GetColumnName( i );   // This retrieves the name of the column
        strFetch += " ";
    }
    // Remove any spaces from the right of the append string
    string s = TrimRight( strFetch.c_str() );
    strFetch = s;
    // Before we print, convert all spaces to commas
    replace_if( strFetch.begin(), strFetch.end(), ::isspace, (char)',');
    // Before we print, convert all heading names to upper case
    transform( strFetch.begin(), strFetch.end(), strFetch.begin(), ::toupper );
    // Send to screen
    copy( strFetch.begin(), strFetch.end(), ostream_iterator<char>( cout, "" ) );
    cout << std::endl;
    return( *this );
}

/********************************************************************
* Function      : ExecuteQueryFile
* Description   : Opens the incoming file name and executes the
*                 contents of the query file
* Parameters    : strFileName - The name of the file to read
* Returns       : 0 = success, failure otherwise.
* Notes         : None
*********************************************************************/
int CIDSQLDirect::ExecuteQueryFile( string strFileName )
{
	int nRetCode = 0;
	string strFetch;

	ifstream infile( strFileName.c_str() );
	if ( !infile.is_open() ) {
		TRACE_DEBUG("Unable to open input query file " + strFileName + "\r\n");
		return( nRetCode = -1 );
	}
	string	line;
	string	sSQL;
	
	while( getline( infile, line ) ) {
        // "line" now contains a line of text
		line.append( " " );	// getline excludes new line characters
		sSQL += line;
	}
	infile.close();
	// Ok, query has now been read in, let's execute it.
    if (( nRetCode = ExecuteSQL( sSQL.c_str() ) ) != SQL_SUCCESS )
        return( nRetCode );

    PrintColumnNames();

	int nCols = GetNumColumns();
    TRACE_DEBUG("Query returned (" + STR(nCols) + ") columns\r\n");
	while( Fetch() == SQL_SUCCESS ) {
		strFetch = "";
		for( int i = 1; i <= nCols; i++ ) {
			strFetch += "\"";
			strFetch += GetCol( i );
			if ( i == nCols )
					strFetch += "\"";
			else	strFetch += "\",";
		}
		if ( strFetch.empty() )
			continue;
		copy( strFetch.begin(), strFetch.end(), ostream_iterator<char>( cout, "" ) );
		cout << endl;
	}
	return( nRetCode );
}
