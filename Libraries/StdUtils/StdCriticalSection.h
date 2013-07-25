/**
\file	StdCriticalSection.h

\brief	Declares the standard critical section class.
**/

/////////////////////////////////////////////////////////////////////////////
// CTryEnterCS
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

#pragma once

#ifndef CTRYENTERCS_H
#define CTRYENTERCS_H

namespace StdUtils
{
/**
\brief	Standard critical section. 

\details This is a critical section class used to lock a section of code. This is primarily
used when multithreading to prevent multiple threads from modifying the same resources.

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT CStdCriticalSection
{
public:
   CStdCriticalSection();
   ~CStdCriticalSection();

   virtual bool TryEnter() = 0;
   virtual bool Leave() = 0;
};

CStdCriticalSection STD_UTILS_PORT *Std_GetCriticalSection();

}				//StdUtils

#endif // CTRYENTERCS_H