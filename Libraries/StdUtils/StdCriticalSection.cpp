/**
\file	StdCriticalSection.cpp

\brief	Implements the standard critical section class.
**/

/////////////////////////////////////////////////////////////////////////////
// CStdCriticalSection
//
// A ::TryEnterCriticalSection type thing that works on 9x
//
// Written by Olan Patrick Barnes (patrick@mfcfree.com)
// Copyright (c) 2001 Olan Patrick Barnes
//
// This code may be used in compiled form in any way you desire. This
// file may be redistributed by any means PROVIDING it is 
// not sold for profit without the authors written consent, and 
// providing that this notice and the authors name is included. 
//
// This file is provided "as is" with no expressed or implied warranty.
// The author accepts no liability if it causes any damage to you or your
// computer whatsoever.
//
// Description:
//
// ::TryEnterCriticalSection() is only available on NT platforms, and you
// may need to support 9x.  This is a custom critical section class that
// allows for "try-enter" logic.  It operates 100% in user mode (this
// class does not use expensive kernel objects), making use of the
// ::InterlockedExchange() and ::GetCurrentThreadId() API's.
//
/////////////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "StdCriticalSectionInternal.h"

namespace StdUtils
{

/**
\brief	Default constructor.

\author	dcofer
\date	5/3/2011
**/
CStdCriticalSection::CStdCriticalSection()
{
}

/**
\brief	Destructor.

\details This releases the critical section lock if one was gained
with a TryEnter call

\author	dcofer
\date	5/3/2011
**/
CStdCriticalSection::~CStdCriticalSection()
{
   //Leave(); //lint !e534
}

CStdCriticalSection STD_UTILS_PORT *Std_GetCriticalSection()
{
    return new CStdCriticalSectionInternal;
}


}				//StdUtils

