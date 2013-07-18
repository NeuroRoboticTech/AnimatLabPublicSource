// StdUtils_UnitTests.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#ifndef _WINDOWS
	//Not sure why, but this is required on linux, but causes LNK2005 errors on windows
	#define BOOST_TEST_DYN_LINK
#endif

#define BOOST_TEST_MODULE StdUtilTests
#include <boost/test/unit_test.hpp>
