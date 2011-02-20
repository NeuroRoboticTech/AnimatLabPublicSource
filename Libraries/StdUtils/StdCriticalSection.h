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

#ifndef CTRYENTERCS_H
#define CTRYENTERCS_H

class STD_UTILS_PORT CStdCriticalSection
{
protected:
   //Do not allow copy constructor or copy operator
   CStdCriticalSection(const CStdCriticalSection& src);
   const CStdCriticalSection& operator=(const CStdCriticalSection& src);

   LONG  m_lBusy;
   DWORD m_dwOwner;
   ULONG m_ulRefCnt;

   class InternalLocker
   {
   protected:
      //Do not allow copy constructor or copy operator
      InternalLocker();
      InternalLocker(const InternalLocker& src);
      const InternalLocker& operator=(const InternalLocker& src);
      LPLONG m_plBusy;
   public:
      explicit InternalLocker(LPLONG plBusy);
      ~InternalLocker();
   };
   friend InternalLocker;

public:
   CStdCriticalSection();
   ~CStdCriticalSection();

   bool TryEnter();
   bool Enter(long lMilliTimeout = -1);
   bool Leave();
};

#endif CTRYENTERCS_H