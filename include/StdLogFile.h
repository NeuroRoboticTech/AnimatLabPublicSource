/////////////////////////////////////////////////////////////////////////////
// File name:      STLogFile.h
// Author:         Nicholas Tsipanov (nicholas@spbteam.com)
// Created:        March 2001
// Last changed:   November, 15, 2002
// Version:        3.75
// Description:    Debug tracing to file 
//
//
/////////////////////////////////////////////////////////////////////////////
// LICENSE
// 
// This code may be used in compiled form in any way you desire. This
// file may be redistributed unmodified by any means PROVIDING it is 
// not sold for profit without the authors written consent, and 
// providing that this notice and the authors name is included.
//
// This code can be compiled, modified and distributed freely, providing
// that this copyright information remains intact in the distribution.
//
// This code may be compiled in original or modified form in any private 
// or commercial application.
//
// THIS CODE IS DISTRIBUTED "AS IS".  NO WARRANTY OF ANY
// KIND IS EXPRESSED OR IMPLIED.  YOU USE AT YOUR OWN RISK.
// THE AUTHOR WILL NOT BE LIABLE FOR DATA LOSS, DAMAGES, LOSS
// OF PROFITS OR ANY OTHER KIND OF LOSS WHILE USING OR MISUSING
// THIS SOFTWARE.

#ifndef ___LOGFILE_H__INCLUDED___
#define ___LOGFILE_H__INCLUDED___


/*
History:

3.7
what's new:
--new feature: per thread indent in the log files
--new options:  STLOG_MULTITHREADING (turns on/off multithreading features), STLOG_USE_PERFORMANCE_CONTER (you can turn off usage of GetPerformanceCounter() function)
--fixed bug with stack overflow in GetLogFileName() function (thanks to Alexander Shargin)
--fixed bug with unclosed handle when used from DLL (thanks to Rene Heijndijk).
--fixed bug with ___DoNothing() function for the cases when the STLOG_DEBUG macro is undefined
--new functions: STLOG_WRITE_IID, STLOG_WRITE_GUID, STLOG_WRITE_CLSID

*/

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <wtypes.h>
//  Comment the line below if you don't want logging. 
//You can add this define to the project defines in case you include this file from many projects
#define STLOG_DEBUG 1

//  If STLOG_CREATE_NEW is defined then every time your program starts it will create 
//  a new log file named <Executable>_log00.txt, <Executable>_log01.txt, etc. Otherwise it will 
//  overwrite the old <Executable>_log.txt file.
#define STLOG_CREATE_NEW 1

//Every time the program starts the program will delete the previous file if it uses the 
// single log (sometime it is not needed, for example you want to have all logs of all program runs)
//#define STLOG_DELETE_OLD

//  By default the logfile is created in the root folder, but you 
// may want to create a log in the directory where the executable is located
#define STLOG_CREATE_FILE_IN_THE_SAME_DIRECTORY

//  Undef this if you don't want compile time info (file name and line number 
//  where the logging has been called) included in the log
//#define STLOG_PRINT_COMPILE_INFO 1

//override standard TRACE macro behaviour
#define STLOG_USE_FOR_TRACE

//You can specify your log file name if you want to. In most cases 
// it can be done automatically : Logfile determines where the executable is located
// and creates an <executable>_log.txt file, but if you want a custom file, then 
//unocomment the #define below and somewhere in you .cpp files (stdafx.cpp most appropriate)
// add the line  LPCTSTR cszLogFileName = _T("C:\\LogFile");
//#define STLOG_FILENAME


#define STLOG_MAXCOUNTERS 30



#define STLOG_USE_PERFORMANCE_COUNTER

#define STLOG_MULTITHREADING

/*USAGE::
#include this file into any header that will be included in all files.
For VC projects using precompiled headers the ideal place would be stdafx.h
You can either insert this file into the project or not, in the latter case 
the log classes won't litter your project workspace. This won't affect
usage of logs.

The primary usage of this code is to have a simple way to output debugging
information into the log file, which could be easiliy turned off or on during 
the compile time.
Example :

STLOG_WRITE(_T("saving %d transactions"), nTransactions); 

Another STLogFile feature, marker: 

void MyFunction()
{
	STLOG_MARKER(_T("MyFunction"));
	if (somethingbad())
		return FALSE;
	if (somethinggood())
		return TRUE;

	return FALSE; 
}

If you want to output some binary data (buffer contents) you should use a STLOG_WRITE_DATA macro:

  {
  ...
  char buffer[125];
  // .. fill the buffer with data 

  STLOG_WRITE_DATA(buffer, 125);
  }

This code will write a >>[MyFunction] line on entering the function and <<[MyFunction] 
on exit wherever the program leaves your function.

To find out the problem of the wrong function execution simply add STLOG_LASTERROR:

  HANDLE hFile = CreateFile(....);
  if (INVALID_HANDLE == hFile)
  {
	STLOG_WRITE("Cannot open file");
	STLOG_LASTERROR;
  }
This will print the last textual description of error returned by GetLastError() funciton.

And .. profiling. Before describing log file profiling features it would be appropriate 
to say that all measured timing includes the time for file operation, which slightly 
increases the execution time, therefore this method could only help to *estimate* and 
compare execution times to find bottlenecks of your code.


  ....
  STLOG_PROFILE_FUNCTION(Calculate());
  ..
This code will estimate time needed for function execution. Simple. When the 
logging is turned off this will be transformed to straight call to Calculate().


  {
	STLOG_PROFILE_BLOCK;
	.. code here ...

  }
  This sample of code will print to log file the time when the execution entered the
  profiled block, when the execution leaves it and the time interval between the two points.
  When your program finishes, this macro will print profiling statistics, like how many 
  times this code has been executed, how much time did it take in total, in average, 
  maximium and minimum timings.


  Sometime you want to have intermediate timings and STLogFile has a solution for this case:
  {
	STLOG_PROFILE_BLOCK;
	Phase0();
	STLOG_PROFILE_CHECKPOINT
	Phase1();
	STLOG_PROFILE_CHECKPOINT
	Phase2();
  }
  This will print timings between the check points and the time from beginning of the block 
  every time execution reaches any check point.

There are some options, which can be used for fine tuning like where to create log file,
how to create it and others. 




*/

//---------------  Don't touch anything below this line. 
//---------------  Of all changes and improvements please notify authors via email. 

#ifdef STLOG_DEBUG

	#define STLOG_MAXINDENT 100

	#ifdef STLOG_PRINT_COMPILE_INFO
		static CHAR *CURRENT_FILE;
		static DWORD CURRENT_LINE;
	#endif

	#ifdef STLOG_FILENAME
		STLOG_FILENAME;
	#endif
	static int ___g_nCounterIndex___ = 0;
	class CSTLogFile  
	{
	public:
		static CSTLogFile *GetLogFile() 
		{
			static CSTLogFile LogFile;
			if (!LogFile.m_bIsStarted) 
				LogFile.Start();
			return &LogFile;
		}

		inline void Write0(LPCSTR szEntry)
		{
			::EnterCriticalSection(&m_crit);
			BOOL bOk = TRUE;
			char szTimeString[400];
			bOk = GetTimeString(szTimeString, sizeof(szTimeString));
			char buffer[1024];
			char szIndent[STLOG_MAXINDENT];
			memset(szIndent, ' ', sizeof(szIndent))	;

			int nIndent = (int) TlsGetValue(m_dwTLSIndex);
			*(szIndent + nIndent) = 0;
#ifdef STLOG_MULTITHREADING
#define	STLOG_MULTITHREADING_STUB1 "thr 0x%08X"
#define	STLOG_MULTITHREADING_STUB2 GetCurrentThreadId(),
#else
#define	STLOG_MULTITHREADING_STUB1 
#define	STLOG_MULTITHREADING_STUB2 

#endif
#ifdef STLOG_PRINT_COMPILE_INFO
			int nBytes = _snprintf(buffer, sizeof(buffer), "%s : %s%s\t(from %s,%d)\r\n", szTimeString, szIndent, szEntry, CURRENT_FILE, CURRENT_LINE);
			//int nBytes = _snprintf(buffer, sizeof(buffer), "%s "STLOG_MULTITHREADING_STUB1" : %s%s\t(from %s,%d)\r\n", szTimeString, STLOG_MULTITHREADING_STUB2 szIndent, szEntry, CURRENT_FILE, CURRENT_LINE);
#else
			//int nBytes = _snprintf(buffer, sizeof(buffer), "%s,"STLOG_MULTITHREADING_STUB1" : %s%s\r\n", szTimeString, STLOG_MULTITHREADING_STUB2 szIndent, szEntry);
			int nBytes = _snprintf(buffer, sizeof(buffer), "%s, %s%s\r\n", szTimeString, szIndent, szEntry);
#endif

			bOk = (nBytes > 0);
			if (!bOk) goto exit_function;
			DWORD dwWrittenBytes;
			::SetFilePointer(m_hFile, 0, 0, FILE_END);
			::WriteFile(m_hFile, buffer, nBytes, &dwWrittenBytes, NULL);
			bOk = (dwWrittenBytes != (DWORD)nBytes);
			if (!bOk) goto exit_function;
			::FlushFileBuffers(m_hFile);
		exit_function:
			::LeaveCriticalSection(&m_crit);
		}

		void WriteData(LPCVOID pBytes, DWORD dwSize) 
		{
			Write("***Binary data (%d bytes)",  dwSize);
			DWORD dwWrittenBytes;
			::SetFilePointer(m_hFile, 0, 0, FILE_END);
			::WriteFile(m_hFile, pBytes, dwSize, &dwWrittenBytes, NULL);
			::WriteFile(m_hFile, "\r\n", 2, &dwWrittenBytes, NULL);
			Write("***End binary data (written %d bytes)", dwWrittenBytes);
		}
		inline void Write(LPCSTR szEntry, ...)
		{
			BOOL bOk = TRUE;
			if (m_bIsStarted) 
			{
				char buffer[1024];
				va_list args;
				va_start(args, szEntry);
				bOk = (-1 != _vsnprintf(buffer, sizeof(buffer), szEntry, args));
				va_end(args);
				if (!bOk) return;
				Write0(buffer);
			}
		}

		inline void Write(LPCWSTR szEntry, ...) 
		{
			BOOL bOk = TRUE;
			if (m_bIsStarted) 
			{
				WCHAR buffer[1024];
				va_list args;
				va_start(args, szEntry);
				int nSize = _vsnwprintf(buffer, sizeof(buffer) / sizeof(WCHAR), szEntry, args);
				bOk = (-1 != nSize);
				va_end(args);
				if (!bOk) return;
				char *pAnsiString = new char[nSize+1];
				wcstombs(pAnsiString, buffer, nSize+1);
				Write0(pAnsiString);
				delete pAnsiString;
			}
		}
/*
		void WriteGUID(REFGUID rguid)
		{
			OLECHAR szBuffer[40];
			StringFromGUID2(rguid, szBuffer, 40);
			Write(_T("GUID: %s"), szBuffer);

		}

		void WriteCLSID(REFCLSID rclsid)
		{
			LPOLESTR lpszBuffer;
			StringFromCLSID(rclsid, &lpszBuffer);
			Write(_T("CLSID: %s"), lpszBuffer);
			CoTaskMemFree(lpszBuffer);
		}

		void WriteIID(REFIID  rclsid)
		{
			LPOLESTR lpszBuffer;
			StringFromIID(rclsid, &lpszBuffer);
			Write(_T("CLSID: %s"), lpszBuffer);
			CoTaskMemFree(lpszBuffer);
		}
*/
		void WriteLastError() 
		{ 
			DWORD dwError = GetLastError(); 
			LPVOID lpMsgBuf;                 
			if (!FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL, dwError, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpMsgBuf, 0, NULL ))
			{
				Write(_T("GetLastError returned : %d (no further information)"), dwError);
			} else {
				TCHAR *ret;
				while (ret = _tcsrchr((LPTSTR)lpMsgBuf, _T('\n'))) *ret = _T(' ');
				while (ret = _tcsrchr((LPTSTR)lpMsgBuf, _T('\r'))) *ret = _T(' ');
				CSTLogFile::GetLogFile()->Write(_T("GetLastError returned : 0x%08x: %s)"), dwError, lpMsgBuf);
				LocalFree( lpMsgBuf ); 
			}
		}


		void Start() 
		{
			TlsSetValue(m_dwTLSIndex, 0);

#ifdef STLOG_FILENAME
			_totchar c1(Std_GetLogFilePrefix());
			Start(c1);

			//Start((LPTCSTR) Std_GetLogFilePrefix().c_str());
#else
			TCHAR wszFileName[MAX_PATH];
			GetLogFileName(wszFileName);
			Start(wszFileName);
#endif
		}

		void Start(LPCTSTR szFilePath) 
		{
			if (m_bIsStarted) return;
			::EnterCriticalSection(&m_crit);
#ifdef STLOG_DELETE_OLD
			::DeleteFile(szFilePath);
#endif
			m_hFile = ::CreateFile(szFilePath, GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, 
							OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
			HRESULT hr = GetLastError();
			if (m_hFile != INVALID_HANDLE_VALUE)
			{
				::SetFilePointer(m_hFile, 0, 0, FILE_END);
				m_bIsStarted = TRUE;
				TCHAR szExecutable[MAX_PATH];
				GetModuleFileName(NULL, szExecutable, MAX_PATH);
				DWORD dwProcID = GetCurrentProcessId();
				SYSTEMTIME st;
				GetLocalTime(&st);
				Write(TEXT("============================================="));
				Write(TEXT("Log is started on %02u.%02u.%04u, at %02u:%02u:%02u:%03u, executable: %s (ProcID: 0x%08x), compile time : %s %s"), st.wDay, st.wMonth, st.wYear, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds, szExecutable, dwProcID, TEXT(__DATE__) , TEXT(__TIME__));
			}
			::LeaveCriticalSection(&m_crit);
		}

		void Stop() 
		{
			::EnterCriticalSection(&m_crit);
			DeleteStaticCounters();
			if (m_bIsStarted) {
				Write(TEXT("Log finished"));
				::CloseHandle(m_hFile);
				m_bIsStarted = FALSE;
			}
			::LeaveCriticalSection(&m_crit);
		}

	protected:
		CSTLogFile() 
			: m_bIsStarted(FALSE), nLastCounter(0)
		{
			::InitializeCriticalSection(&m_crit);
			m_dwTLSIndex = TlsAlloc ();
#ifdef STLOG_USE_PERFORMANCE_COUNTER
			::QueryPerformanceFrequency(&m_nPerfFreq);
			::QueryPerformanceCounter(&m_nStartTime);
#endif 
		}
	public:
		virtual ~CSTLogFile() 
		{
			if (m_bIsStarted)
				Stop();
			::DeleteCriticalSection(&m_crit);
			TlsFree(m_dwTLSIndex);
		}
	private:
		HANDLE m_hFile;
		CRITICAL_SECTION m_crit;
		BOOL m_bIsStarted;


		DWORD m_dwTLSIndex;

	#ifdef STLOG_USE_PERFORMANCE_COUNTER
		LARGE_INTEGER m_nPerfFreq;
		LARGE_INTEGER m_nStartTime;
	#endif

		BOOL GetTimeString(LPSTR szString, int nStringSize) 
		{
			SYSTEMTIME st;
			GetLocalTime(&st);
#ifdef STLOG_USE_PERFORMANCE_COUNTER
			LARGE_INTEGER counter;
			QueryPerformanceCounter(&counter);
			return (0 < _snprintf(szString, nStringSize, "%02u:%02u:%02u(%I64d)", st.wHour, st.wMinute, st.wSecond, counter.QuadPart));
#else
			return (0 < _snprintf(szString, nStringSize, "%02u:%02u:%02u:%03u", st.wHour, st.wMinute, st.wSecond, st.wMilliseconds));
#endif



		}
	public:
		inline DWORD GetIndent()
		{
			return (DWORD)TlsGetValue(m_dwTLSIndex);
		}
		inline void SetIndent(DWORD dwIndent)
		{
			TlsSetValue(m_dwTLSIndex, (LPVOID) dwIndent);
		}
		inline void IncrIndent()
		{
			DWORD dwIndent = (DWORD)TlsGetValue(m_dwTLSIndex);
			dwIndent++;
			TlsSetValue(m_dwTLSIndex, (LPVOID) dwIndent);

		}
		inline void DecrIndent()
		{
			DWORD dwIndent = (DWORD)TlsGetValue(m_dwTLSIndex);
			dwIndent--;
			TlsSetValue(m_dwTLSIndex, (LPVOID) dwIndent);

		}
	private:
		void GetLogFileName(LPTSTR szFileName) {
			TCHAR wszExecutableFilePath[MAX_PATH];
			::GetModuleFileName(NULL, wszExecutableFilePath, MAX_PATH);
#ifdef STLOG_CREATE_FILE_IN_THE_SAME_DIRECTORY
			TCHAR *wszExecutableFileName = wszExecutableFilePath;
#else
	
#ifdef _WIN32_WCE
			TCHAR *wszExecutableFileName = ::_tcsrchr(wszExecutableFilePath, _T('\\'));
#else
//For desktop systems we need to create a log on the system's windows drive
			TCHAR wszExecutableFileName[MAX_PATH+1];
			::GetWindowsDirectory(wszExecutableFileName, MAX_PATH+1); //This will create a string with start "c:\"
			TCHAR *pcFirstSlash = _tcschr(wszExecutableFileName, _T('\\'));
			TCHAR *pcLastSlash = ::_tcsrchr(wszExecutableFilePath, _T('\\'));
			//copy the name of the executable to the string containing 
			_tcscpy(pcFirstSlash, pcLastSlash); 
#endif

#endif

			TCHAR *wszLastDot = ::_tcsrchr(wszExecutableFileName, _T('.'));

#ifdef STLOG_CREATE_NEW
			BOOL bFound = FALSE;
			int nFreeNumber = 0;
			TCHAR wszTemp[MAX_PATH];

			while (!bFound) {

				_stprintf(wszTemp, _T("_log%02d.txt"), nFreeNumber);
				::_tcscpy(wszLastDot, wszTemp);
				if (0xFFFFFFFF == ::GetFileAttributes(wszExecutableFileName))
				{
					bFound = TRUE;
				} else {
					nFreeNumber++;
				}
			}

#else 
			::_tcscpy(wszLastDot, _T("_log.txt"));
#endif 

			::_tcscpy(szFileName, wszExecutableFileName);
		}
	public:
		
		class Counter{
		public:
			Counter(char *szFile, int nLine) : m_nCounter(0), m_szFile(szFile), m_nLine(nLine)
			{
				m_nCounterIndex = ___g_nCounterIndex___++;
				::InitializeCriticalSection(&m_crit);
				CSTLogFile::GetLogFile()->Write("***Counter %d at %s,%d initialized", m_nCounterIndex, szFile, nLine);
				m_TotalTime.QuadPart = 0;
			}
			~Counter()	
			{
				CSTLogFile::GetLogFile()->Write("Counter %d statistics\r\n\tCounts: %d,\r\n\tMinTime: %I64d ms,\r\n\tMaxTime: %I64d ms,\r\n\tTotalTime: %I64d ms,\r\n\tAverageTime: %I64d ms", 
					m_nCounterIndex, m_nCounter, m_MinTime.QuadPart, m_MaxTime.QuadPart, m_TotalTime.QuadPart, m_TotalTime.QuadPart / m_nCounter); 
				::DeleteCriticalSection(&m_crit);
			}
			inline void StartSection() 
			{	
				::EnterCriticalSection(&m_crit);
				m_nCounter++;
				CSTLogFile::GetLogFile()->Write("Counter %d started", m_nCounterIndex); 
				::QueryPerformanceCounter(&m_StartTime);
				m_LastCheckPoint.QuadPart = m_StartTime.QuadPart;
				::LeaveCriticalSection(&m_crit);
			}
			inline void StopSection() 
			{
				::EnterCriticalSection(&m_crit);
				LARGE_INTEGER liCurrentTime;
				::QueryPerformanceCounter(&liCurrentTime);
				CSTLogFile::GetLogFile()->Write("Counter %d stopped %I64dms from start, %I64dms from last checkpoint", m_nCounterIndex,  liCurrentTime.QuadPart - m_StartTime.QuadPart, liCurrentTime.QuadPart - m_LastCheckPoint.QuadPart); 
				liCurrentTime.QuadPart -= m_StartTime.QuadPart;
				if (m_nCounter == 1 || m_MinTime.QuadPart > liCurrentTime.QuadPart) m_MinTime.QuadPart = liCurrentTime.QuadPart;
				if (m_nCounter == 1 || m_MaxTime.QuadPart < liCurrentTime.QuadPart) m_MaxTime.QuadPart = liCurrentTime.QuadPart;
				m_TotalTime.QuadPart += liCurrentTime.QuadPart;


				::LeaveCriticalSection(&m_crit);
			}
			inline void CheckPoint(int nLine) 
			{
				::EnterCriticalSection(&m_crit);
				LARGE_INTEGER liCheckPoint; ::QueryPerformanceCounter(&liCheckPoint);
				CSTLogFile::GetLogFile()->Write("Counter %d check point at line %d: %I64d ms from start, %I64dms from last checkpoint", m_nCounterIndex,  nLine, liCheckPoint.QuadPart - m_StartTime.QuadPart, liCheckPoint.QuadPart - m_LastCheckPoint.QuadPart); 
				m_LastCheckPoint.QuadPart = liCheckPoint.QuadPart;
				::LeaveCriticalSection(&m_crit);
			}
			inline LARGE_INTEGER GetTimeFromStart() 
			{
				LARGE_INTEGER liCurrentTime;
				::QueryPerformanceCounter(&liCurrentTime);
				liCurrentTime.QuadPart -= m_StartTime.QuadPart;
				return liCurrentTime;
			}

		private:
			LARGE_INTEGER m_StartTime;
			LARGE_INTEGER m_MaxTime;
			LARGE_INTEGER m_MinTime;
			LARGE_INTEGER m_TotalTime;
			LARGE_INTEGER m_LastCheckPoint;
			int m_nCounter;
			CRITICAL_SECTION m_crit;
			friend class CounterAux;
			char *m_szFile;
			int m_nLine;
			int m_nCounterIndex ;
		};
		struct CounterAux
		{
			CounterAux(Counter *pCounter) : m_pCounter(pCounter) { if (pCounter) pCounter->StartSection();}
			~CounterAux() { if (m_pCounter) m_pCounter->StopSection();}
			Counter *m_pCounter;
		};

		Counter *m_counters[STLOG_MAXCOUNTERS];
		int nLastCounter;
		static Counter *GetStaticCounter(char *szFile, int nLine) 
		{
			CSTLogFile* pLogFile = GetLogFile();
			if (pLogFile->nLastCounter == STLOG_MAXCOUNTERS)
			{
				pLogFile->Write("*****Max counters (%d) reached, the counter at %s, %d will not be created", STLOG_MAXCOUNTERS, szFile, nLine);
				return 0;
			}


			Counter *p = new Counter(szFile, nLine);
			pLogFile->m_counters[pLogFile->nLastCounter++] = p;
			return p;
		}

		void DeleteStaticCounters()
		{
			while (nLastCounter)
			{
				delete m_counters[--nLastCounter];
			}
		}
		struct Marker 
		{
			Marker(LPCSTR szEntry)
			{
				m_bWide = FALSE;
				m_szEntry = szEntry;
				CSTLogFile::GetLogFile()->Write(">>[%s]", m_szEntry);	
				if (CSTLogFile::GetLogFile()->GetIndent() < STLOG_MAXINDENT)
					CSTLogFile::GetLogFile()->IncrIndent();
			}
			Marker(LPCWSTR szEntry)
			{
				m_bWide = TRUE;
				m_szEntry = (LPCSTR) szEntry;
				CSTLogFile::GetLogFile()->Write(L">>[%s]", m_szEntry);
				if (CSTLogFile::GetLogFile()->GetIndent() < STLOG_MAXINDENT)
					CSTLogFile::GetLogFile()->IncrIndent();
			}
			~Marker()
			{
				if (CSTLogFile::GetLogFile()->GetIndent() > 0)
					CSTLogFile::GetLogFile()->DecrIndent();

				if (m_bWide)
					CSTLogFile::GetLogFile()->Write(_T("<<[%s]"), (LPCWSTR)m_szEntry);
				else
					CSTLogFile::GetLogFile()->Write(_T("<<[%s]"), m_szEntry);


			}
		private:
			LPCSTR m_szEntry;
			BOOL m_bWide;
		};

	};

#endif

#ifdef STLOG_DEBUG 
	extern CSTLogFile logfile;

	#ifdef STLOG_PRINT_COMPILE_INFO
	#define STLOG_EXPAND_INFO(file, line) \
		CURRENT_FILE = file;	CURRENT_LINE=line;
	#else
		inline void _______DoNothing0() {};
		#define STLOG_EXPAND_INFO(file, line) \
			1 ? void(0) : _______DoNothing0();
	#endif

	#define STLOG_WRITE \
		STLOG_EXPAND_INFO(__FILE__, __LINE__)\
		CSTLogFile::GetLogFile()->Write

	#define STLOG_WRITE_DATA	\
		STLOG_EXPAND_INFO(__FILE__, __LINE__)\
		CSTLogFile::GetLogFile()->WriteData
/*
	#define	STLOG_WRITE_GUID \
		STLOG_EXPAND_INFO(__FILE__, __LINE__) \
		CSTLogFile::GetLogFile()->WriteGUID

	#define	STLOG_WRITE_CLSID\
		STLOG_EXPAND_INFO(__FILE__, __LINE__) \
		CSTLogFile::GetLogFile()->WriteCLSID

	#define	STLOG_WRITE_IID\
		STLOG_EXPAND_INFO(__FILE__, __LINE__) \
		CSTLogFile::GetLogFile()->WriteIID
*/

	#define STLOG_PROFILE_BLOCK \
		STLOG_EXPAND_INFO(__FILE__, __LINE__)\
		static CSTLogFile::Counter *___pCounter___  = CSTLogFile::GetStaticCounter( __FILE__, __LINE__); \
		CSTLogFile::CounterAux __counter_aux__(___pCounter___);  

	#define STLOG_PROFILE_FUNCTION(function_name)	\
	STLOG_EXPAND_INFO(__FILE__, __LINE__)\
	{   CSTLogFile *pLog = CSTLogFile::GetLogFile(); \
		pLog->Write(_T("Estimating call to function %s"), _T(#function_name));  \
		static CSTLogFile::Counter *___pCounter___  = CSTLogFile::GetStaticCounter( __FILE__, __LINE__); \
		CSTLogFile::CounterAux __counter_aux__(___pCounter___);  \
		((void)(function_name)); \
		pLog->Write(_T("Function %s finished in %I64d ms"), _T(#function_name), ___pCounter___->GetTimeFromStart());  \
	}

	#define  STLOG_PROFILE_CHECKPOINT \
		if (___pCounter___) ___pCounter___->CheckPoint(__LINE__);

	#define STLOG_MARKER \
		STLOG_EXPAND_INFO(__FILE__, __LINE__)\
		CSTLogFile::Marker ____marker____

	#define STLOG_LASTERROR \
		STLOG_EXPAND_INFO(__FILE__, __LINE__)\
		CSTLogFile::GetLogFile()->WriteLastError();


	#ifdef STLOG_USE_FOR_TRACE
	#ifdef TRACE 
	#undef TRACE
	#undef TRACE0
	#undef TRACE1
	#undef TRACE2
	#undef TRACE3
	#endif
	#define TRACE STLOG_WRITE
	#define TRACE0 STLOG_WRITE
	#define TRACE1 STLOG_WRITE
	#define TRACE2 STLOG_WRITE
	#define TRACE3 STLOG_WRITE
	#endif

#else 
	inline void _______DoNothing( ...) {};
	#define STLOG_WRITE 1? void(0) : _______DoNothing
	#define STLOG_WRITE_DATA 1? void(0) : _______DoNothing
	#define STLOG_PROFILE_FUNCTION(function_name) ((void)(function_name));
	#define STLOG_PROFILE_CHECKPOINT ;
	#define STLOG_PROFILE_BLOCK ;
	#define STLOG_MARKER 1? void(0) : _______DoNothing
	#define STLOG_LASTERROR ;
	#define STLOG_WRITE_GUID 1? void(0) : _______DoNothing
	#define STLOG_WRITE_CLSID 1? void(0) : _______DoNothing
	#define STLOG_WRITE_IID 1? void(0) : _______DoNothing
#endif

//For backward compatibility
#define WRITE_LOG STLOG_WRITE

//Some like it quick
#define STLOG STLOG_WRITE


#endif // ___LOGFILE_H__INCLUDED___
