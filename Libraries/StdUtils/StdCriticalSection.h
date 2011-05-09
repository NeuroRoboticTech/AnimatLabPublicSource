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
protected:
   //Do not allow copy constructor or copy operator

   /**
   \brief	Copy constructor.
   
   \author	dcofer
   \date	5/4/2011
   
   \param	src	Source for the. 
   **/
   CStdCriticalSection(const CStdCriticalSection& src);

   /**
   \brief	Assignment operator.
   
   \author	dcofer
   \date	5/4/2011
   
   \param	src	Source for the. 
   
   \return	A shallow copy of this object.
   **/
   const CStdCriticalSection& operator=(const CStdCriticalSection& src);

   /// Tells if this critical section is currently being used.
   LONG  m_lBusy;

   /// The current owner of this critical section.
   DWORD m_dwOwner;

   /// The number of reference counts to this critical section.
   ULONG m_ulRefCnt;

#pragma region InternalLocker

   /**
   \brief	Internal locker. 

   \An internal locking class.
   
   \author	dcofer
   \date	5/3/2011
   **/
   class InternalLocker
   {
   protected:
      //Do not allow copy constructor or copy operator

      /**
      \brief	Default constructor.
      
      \author	dcofer
      \date	5/3/2011
      **/
      InternalLocker();

      /**
      \brief	Copy constructor.
      
      \author	dcofer
      \date	5/3/2011
      
      \param	src	Source for the. 
      **/
      InternalLocker(const InternalLocker& src);

      /**
      \brief	Assignment operator.
      
      \author	dcofer
      \date	5/3/2011
      
      \param	src	Source for the. 
      
      \return	A shallow copy of this object.
      **/
      const InternalLocker& operator=(const InternalLocker& src);

	  /// Tells if this is busy.
	  LPLONG m_plBusy;
   public:

      /**
      \brief	Constructor.
      
      \author	dcofer
      \date	5/3/2011
      
      \param	plBusy	The pl busy. 
      **/
      explicit InternalLocker(LPLONG plBusy);

	  /**
	  \brief	Finaliser.
	  
	  \author	dcofer
	  \date	5/3/2011
	  **/
	  ~InternalLocker();
   };
   friend InternalLocker;

#pragma endregion

public:
   CStdCriticalSection();
   ~CStdCriticalSection();

   bool TryEnter();
   bool Enter(long lMilliTimeout = -1);
   bool Leave();
};

}				//StdUtils

#endif CTRYENTERCS_H