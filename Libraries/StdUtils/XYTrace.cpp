
#include "StdAfx.h"

namespace StdUtils
{

#ifdef WIN32

// private helper class
class XYTraceHelper
{
	// friend functions of this class
	friend void SetTraceFilePrefix(LPCTSTR strFilePrefix);
	friend string GetTraceFilePrefix();
	friend void SetTraceLevel(const int nLevel);
	friend int GetTraceLevel();
	friend void Std_Log(const int nLevel, bool bPrintHeader, LPCTSTR strFormat, ...);
	friend void Std_ResetLog();

	// internal data members
	string m_strFilename;
	HANDLE	m_hFile;
	int m_nLevel;
	long m_nThreadId;
	string m_strTraceFilePrefix;
	SYSTEMTIME m_timeStart;

	// close the current trace file
	void CloseTraceFile()
	{
		if(m_hFile) ::CloseHandle(m_hFile);
		m_hFile = NULL;
	}
	// open a new trace file
	HANDLE OpenTraceFile()
	{
		// construct the new trace file path
		TCHAR strFilePath[1001];
		string strPrefix;
		SYSTEMTIME sysTime;
		::GetLocalTime(&sysTime);

		if(m_strTraceFilePrefix.length())
			strPrefix = m_strTraceFilePrefix;
		else
			strPrefix = "Trace";

		_stprintf
		(
			strFilePath,
			_T("%s_%04d%02d%02d_%02d%02d%02d_%X.txt"),
			strPrefix.c_str(),
			sysTime.wYear,
			sysTime.wMonth,
			sysTime.wDay,
			sysTime.wHour,
			sysTime.wMinute,
			sysTime.wSecond,
			::GetCurrentProcessId()
		);

		m_strFilename = strFilePath;

		// create the new trace file
		m_hFile = CreateFile
		(
			strFilePath,
			GENERIC_WRITE,
			FILE_SHARE_READ,
			NULL,
			CREATE_ALWAYS,
			FILE_ATTRIBUTE_NORMAL,
			NULL
		);
		// if successful, save the start time variable
		if(m_hFile) m_timeStart = sysTime;
		// return the file handle
		return m_hFile;
	}
	// set lock to gain exclusive access to trace
	// functions
	void Lock()
	{
		long nThreadId = ::GetCurrentThreadId();
		while(m_nThreadId!=nThreadId)
		{
			// keep trying until successfully completed the operation
			#if _MSC_VER > 1300  // VC 7
				::InterlockedCompareExchangePointer((void**)&m_nThreadId, (void*)nThreadId, 0);  
			#else  // VC 6
				::InterlockedCompareExchange((void**)&m_nThreadId, (void*)nThreadId, 0);
			#endif

			if(m_nThreadId==nThreadId) break;
			::Sleep(25);
		}
	}
	// release lock so that other threads can access 
	// trace functions
	void Unlock()
	{
		// only the thread that set the lock can release it
			#if _MSC_VER > 1300  // VC 7
				::InterlockedCompareExchangePointer((void**)&m_nThreadId, 0, (void*)::GetCurrentThreadId());
			#else  // VC 6
				::InterlockedCompareExchange((void**)&m_nThreadId, 0, (void*)::GetCurrentThreadId());
			#endif
	}
	// set the current trace level
	void SetTraceLevel(const int nLevel) { m_nLevel = nLevel>0?nLevel:0; }
	int GetTraceLevel() {return m_nLevel; }

	// set the trace file name prefix
	void SetTraceFilePrefix(LPCTSTR strFilePrefix)
	{
		// close existing trace file first
		CloseTraceFile();
		m_strTraceFilePrefix = strFilePrefix;
	}
public:
	// constructor and destructor
	XYTraceHelper()
	{
		m_hFile = NULL;
		m_nLevel = TraceDetail;
		m_nThreadId = 0;
	}
	~XYTraceHelper()
	{
		CloseTraceFile();
	}
};

// the one and only instance of XYTraceHelper
XYTraceHelper theHelper;

void SetTraceFilePrefix(LPCTSTR strFilePrefix)
{
	// set lock
	theHelper.Lock();
	// set trace file name prefix
	theHelper.SetTraceFilePrefix(strFilePrefix);
	// release lock
	theHelper.Unlock();
}

string GetTraceFilePrefix()
{
	return theHelper.m_strTraceFilePrefix;
}

void SetTraceLevel(const int nLevel)
{
	// set lock
	theHelper.Lock();
	// set trace level
	theHelper.SetTraceLevel(nLevel);
	// release lock
	theHelper.Unlock();
}

int GetTraceLevel()
{return theHelper.GetTraceLevel();}

string GetLevel(const int nLevel)
{
	switch ( nLevel )
	{
		case TraceNone:
			return "NONE";
		case TraceError:
			return "ERROR";
		case TraceInfo:
			return "INFO";
		case TraceDebug:
			return "DEBUG";
		case TraceDetail:
			return "DETAIL";
	}
	return "NONE";
}

/**
\brief	Resets the log.

\author	dcofer
\date	5/4/2011

**/
void STD_UTILS_PORT Std_ResetLog()
{
	theHelper.Lock();

	if(theHelper.m_hFile)
	{
		theHelper.CloseTraceFile();
		if(theHelper.m_strFilename.length())
			DeleteFile((LPCSTR) theHelper.m_strFilename.c_str());
	}

	theHelper.Unlock();
}

/**
\brief	Logs a message.

\author	dcofer
\date	5/4/2011

\param	nLevel			The log level. 
\param	bPrintHeader	true to print header. 
\param	strFormat   	The string format. 

\return	.
**/
void STD_UTILS_PORT Std_Log(const int nLevel, bool bPrintHeader, LPCTSTR strFormat, ...)
{
	// if the specified trace level is greater than
	// the current trace level, return immediately
	if(theHelper.m_nLevel==0||nLevel>theHelper.m_nLevel) return;
	// set lock
	theHelper.Lock();
	try
	{
		// get local time
		SYSTEMTIME sysTime;
		::GetLocalTime(&sysTime);
		// get trace file handle	
		HANDLE hFile = theHelper.m_hFile;
		// open the trace file if not already open
		if(hFile==NULL) hFile = theHelper.OpenTraceFile();
		// if it is already a new day, close the old
		// trace file and open a new one
		else if
		(
			sysTime.wYear!=theHelper.m_timeStart.wYear||
			sysTime.wMonth!=theHelper.m_timeStart.wMonth||
			sysTime.wDay!=theHelper.m_timeStart.wDay)
		{
			theHelper.CloseTraceFile();
			theHelper.OpenTraceFile();
		}
		
		string strLevel = GetLevel(nLevel);

		// write the trace message
		if(hFile)
		{
			// declare buffer (default max buffer size = 32k)
			const int nMaxSize = 32*1024;
			TCHAR pBuffer[nMaxSize+51];
			int nPos=0;

			if(bPrintHeader)
			{
				// print time stamp and thread id to buffer
				nPos = _stprintf
				(
					pBuffer,
					_T("[%s %02d/%02d/%04d %02d:%02d:%02d_%03d_%X] "), 
					strLevel.c_str(),
					sysTime.wDay,
					sysTime.wMonth,
					sysTime.wYear,
					sysTime.wHour,
					sysTime.wMinute,
					sysTime.wSecond,
					sysTime.wMilliseconds,
					theHelper.m_nThreadId
				);
			}

			// print the trace message to buffer
			va_list args;
			va_start(args, strFormat);
			nPos += _vsntprintf(pBuffer+nPos,nMaxSize,strFormat,args);
			va_end(args);
			// print the end of the line to buffer
			_stprintf(pBuffer+nPos,_T("\r\n"));
			// write the buffer to the trace file
			DWORD dwBytes;
			::WriteFile(hFile,pBuffer,_tcslen(pBuffer),&dwBytes,NULL);
		}
	}
	catch(...)
	{
		// add code to handle exception (if needed)
	}
	// release lock
	theHelper.Unlock();
}

#endif
}				//StdUtils
