// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__F4A31B04_AEE2_4819_B678_83F6177B0C54__INCLUDED_)
#define AFX_STDAFX_H__F4A31B04_AEE2_4819_B678_83F6177B0C54__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


// Insert your headers here

#ifdef _WINDOWS
	#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

	#include <windows.h>
	
	#pragma comment(lib, "Vfw32.lib")
#endif

#include "StdIncludes.h"


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__F4A31B04_AEE2_4819_B678_83F6177B0C54__INCLUDED_)
