/**
\file	StdVariable.cpp

\brief	Implements the standard variable class.
**/

#include "StdAfx.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

namespace StdUtils
{
/**
\brief	Default constructor.

\author	dcofer
\date	5/4/2011
**/
CStdVariable::CStdVariable()
{
	m_dblValue = 0.0;
}

/**
\brief	Destructor.

\author	dcofer
\date	5/4/2011
**/
CStdVariable::~CStdVariable()
{

}

}				//StdUtils
