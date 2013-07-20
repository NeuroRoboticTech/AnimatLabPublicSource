/**
\file	StdTimer.h

\brief	Declares the standard color class.
**/

#pragma once

/**
\namespace	StdUtils

\brief	Namespace for the standard utility objects.
**/
namespace StdUtils
{

typedef struct {
     LARGE_INTEGER start;
     LARGE_INTEGER stop;
 } stopWatch;
 

/**
\brief	Standard color class. 

\author	dcofer
\date	5/3/2011
**/
/**
\brief	Standard color class. 

\author	dcofer
\date	5/3/2011
**/
class CStdWinTimer : public IStdTimer
{
 private:
     stopWatch timer;
     LARGE_INTEGER frequency;
     double LIToSecs( LARGE_INTEGER & L) ;
	 bool m_bStarted;
 public:
     CStdWinTimer() ;
     
     virtual void StartTimer();
     virtual double StopTimer();
     virtual double ElapsedTime();
	 virtual bool TimerStarted() {return m_bStarted;}
 };

}				//StdUtils
