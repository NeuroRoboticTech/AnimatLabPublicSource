/**
\file	StdTimer.cpp

\brief	Implements the standard color class.
**/

#include "StdAfx.h"
#include "StdTimer.h"

namespace StdUtils
{

 double CStdWinTimer::LIToSecs( LARGE_INTEGER & L) {
     return ((double)L.QuadPart /(double)frequency.QuadPart) ;
 }
 
 CStdWinTimer::CStdWinTimer(){
     timer.start.QuadPart=0;
     timer.stop.QuadPart=0; 
     QueryPerformanceFrequency( &frequency ) ;
 	 m_bStarted = false;
}
 
 void CStdWinTimer::StartTimer( ) {
	 m_bStarted = true;
     QueryPerformanceCounter(&timer.start) ;
 }
 
 double CStdWinTimer::StopTimer( ) {
     QueryPerformanceCounter(&timer.stop) ;
	 m_bStarted = false;
	 return ElapsedTime();
 }
 
 double CStdWinTimer::ElapsedTime() {
     LARGE_INTEGER time;
     time.QuadPart = timer.stop.QuadPart - timer.start.QuadPart;
     return LIToSecs( time) ;
 }


}				//StdUtils
