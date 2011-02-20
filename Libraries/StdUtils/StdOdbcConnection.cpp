// StdOdbcConnection.cpp: implementation of the CStdOdbcConnection class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CStdOdbcConnection::CStdOdbcConnection()
{

}

CStdOdbcConnection::~CStdOdbcConnection()
{

}

bool CStdOdbcConnection::IsConnected()
{
	if(m_hDBC)
		return true;
	else
		return false;
}

void CStdOdbcConnection::Connect(string strDsn, string strUserName, string strPassword)
{

try
{
	SetUserName(strUserName.c_str());
	SetPassword(strPassword.c_str());
	CIDSQLDirect::Connect(strDsn.c_str());
}
catch(CStdErrorInfo oError)
{
	if(oError.m_strError.length())
		oError.m_strError = STR(Std_Err_ODBC_strConnection) + "  Error: " +  oError.m_strError;
	else
		oError.m_strError = Std_Err_ODBC_strConnection;

	oError.m_strError += "  DSN: " + strDsn + "  UserName: " + strUserName + "  Password: " + strPassword;
	RELAY_ERROR(oError);
}
catch(...)
{THROW_ERROR(Std_Err_ODBC_lConnection, (STR(Std_Err_ODBC_strConnection) + "  DSN: " + STR(strDsn) + "  UserName: " + STR(strUserName) + "  Password: " + STR(strPassword)));}
}


int CStdOdbcConnection::ExecuteSQL(string strSQL)
{
	if(!IsConnected())
		THROW_ERROR(Std_Err_ODBC_lNotConnected, Std_Err_ODBC_strNotConnected);

	CIDSQLDirect::ExecuteSQL(strSQL.c_str());
	
	SQLINTEGER iRowsAffected=0;
	
	if(SQLRowCount(m_hStmt, &iRowsAffected) == SQL_SUCCESS)
		return iRowsAffected;

	return -1;
}


long CStdOdbcConnection::GetNextID(string strTable, string strIdField)
{
	string strSQL;

	strSQL = "Select count(*) from " + strTable + " With (NOLOCK)";
	CIDSQLDirect::ExecuteSQL(strSQL.c_str());
	Fetch();

	long lCount = atol(GetCol(1).c_str());	
	
	if(!lCount)
		return 0;

	strSQL = "Select Max(" + strIdField + ") From " + strTable + " With (NOLOCK)";

	CIDSQLDirect::ExecuteSQL(strSQL.c_str());
	Fetch();

	lCount = atol(GetCol(1).c_str());	
	lCount++;
	return lCount;
}





