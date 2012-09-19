/**
\file	StdTimer.cpp

\brief	Implements the standard color class.
**/

#include "StdAfx.h"

namespace StdUtils
{

 double CStdTimer::LIToSecs( LARGE_INTEGER & L) {
     return ((double)L.QuadPart /(double)frequency.QuadPart) ;
 }
 
 CStdTimer::CStdTimer(){
     timer.start.QuadPart=0;
     timer.stop.QuadPart=0; 
     QueryPerformanceFrequency( &frequency ) ;
 }
 
 void CStdTimer::StartTimer( ) {
     QueryPerformanceCounter(&timer.start) ;
 }
 
 double CStdTimer::StopTimer( ) {
     QueryPerformanceCounter(&timer.stop) ;
	 return ElapsedTime();
 }
 
 double CStdTimer::ElapsedTime() {
     LARGE_INTEGER time;
     time.QuadPart = timer.stop.QuadPart - timer.start.QuadPart;
     return LIToSecs( time) ;
 }


}				//StdUtils
