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
#include "StdCriticalSection.h"
#include "StdCriticalSectionInternal.h"

namespace StdUtils
{

/**
\brief	Constructor.

\details  For internal use by CStdCriticalSection
This locks access to the internal variables of
an instance of CStdCriticalSection from other threads
 
\author	dcofer
\date	5/3/2011

\param	plBusy	The busy flag. 
**/
CStdCriticalSectionInternal::InternalLocker::InternalLocker(boost::atomic<LockState> &iBusy) :
   m_iBusy(iBusy)
{
   while (m_iBusy.exchange(Locked, boost::memory_order_acquire) == Locked)
   {
	   #ifdef WIN32
	   		Sleep(0);
	   #else
	   		sleep(0);
	   #endif   
   }
}

/**
\brief	Destructor.

\details For internal use by CStdCriticalSection
This unlocks the lock the constructor of this
class gained.

\author	dcofer
\date	5/3/2011
**/
CStdCriticalSectionInternal::InternalLocker::~InternalLocker()
{
   m_iBusy.exchange(Unlocked, boost::memory_order_acquire);
}

/**
\brief	Default constructor.

\author	dcofer
\date	5/3/2011
**/
CStdCriticalSectionInternal::CStdCriticalSectionInternal() :
   m_iBusy(Unlocked), 
   m_bOwned(false),
   m_ulRefCnt(0)
{
}

/**
\brief	Destructor.

\details This releases the critical section lock if one was gained
with a TryEnter call

\author	dcofer
\date	5/3/2011
**/
CStdCriticalSectionInternal::~CStdCriticalSectionInternal()
{
   Leave(); //lint !e534
}

//TryEnter


/**
\brief	Gets the try enter.

\details This locks the critical section for the current thread if
no other thread already owns the critical section.  If the
current thread already owns the critical section and this
is reentry, the current thread is allowed to pass

\author	dcofer
\date	5/3/2011

\return	true if it succeeds, false if it fails.
**/
bool CStdCriticalSectionInternal::TryEnter()
{
   bool bRet(false);
   InternalLocker locker(m_iBusy);
   
   if (!m_bOwned)
   {
      //Nobody owns this cs, so the current will gain ownership
      //ATLASSERT(m_ulRefCnt == 0);
      m_dwOwner = boost::this_thread::get_id();
      m_bOwned = true;
      m_ulRefCnt = 1;
      bRet = true;
   }
   else if (m_dwOwner == boost::this_thread::get_id())
   {
      //The current thread already owns this cs
      //ATLASSERT(m_ulRefCnt > 0);
      m_ulRefCnt++;
      bRet = true;
   }

   //If we return false, some other thread already owns this cs, so
   // we will not increment the recursive ownership count (m_ulRefCnt)
   return bRet;
}

/**
\brief	Try's to enter the critical section. Waits until it can get in, or until timeout.

\author	dcofer
\date	5/3/2011

\param	lMilliTimeout	The milli timeout. 

\return	true if it succeeds, false if it fails.
**/
bool CStdCriticalSectionInternal::Enter(long lMilliTimeout)
{
	bool bDone = false;
	long lTotal = 0;

	while(!bDone)
	{
		bDone = TryEnter();

		if(!bDone)
		{
	   #ifdef WIN32
	   		Sleep(10);
	   #else
	   		sleep(10);
	   #endif   
			lTotal+=10;

			if(lMilliTimeout > 0 && lTotal >= lMilliTimeout)
				return false;
		}
	}

	return true;
}


/**
\brief	Leaves this critical section.

\details This unlocks the critical section for the current thread if
the current thread already owns the critical section and it only
has one "lock" on the critical section.  If the lock count (the
number of times the same thread has it locked) is greater than one,
then the count is simply decremented.

\author	dcofer
\date	5/3/2011

\return	true if it succeeds, false if it fails.
**/
bool CStdCriticalSectionInternal::Leave()
{
   InternalLocker locker(m_iBusy);
   //If the current thread owns this cs
   if (m_dwOwner == boost::this_thread::get_id())
   {
      //and if decrementing the recursive ownership count results in
      // a recursive ownership count of zero, then the current thread
      // should no longer own this cs
      //ATLASSERT(m_ulRefCnt > 0);
      if (--m_ulRefCnt == 0)
      {
         //By setting m_dwOwner to zero, we're stating that no thread owns
         // this cs
         m_bOwned = false;
      }
      return true;
   }
   return false;
}

}				//StdUtils

