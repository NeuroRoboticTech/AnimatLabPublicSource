// StdOdbcConnection.h: interface for the CStdOdbcConnection class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDODBCCONNECTION_H__5261BAF8_4346_488D_8148_802EE40AE98A__INCLUDED_)
#define AFX_STDODBCCONNECTION_H__5261BAF8_4346_488D_8148_802EE40AE98A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdOdbcConnection : public CIDSQLDirect  
{
public:
	CStdOdbcConnection();
	virtual ~CStdOdbcConnection();

	bool IsConnected();

	virtual void Connect(string strOdbc, string strUserName, string strPassword);

	virtual int ExecuteSQL(string strSQL);
	virtual long GetNextID(string strTable, string strIdField);
};

#endif // !defined(AFX_STDODBCCONNECTION_H__5261BAF8_4346_488D_8148_802EE40AE98A__INCLUDED_)
