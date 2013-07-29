#include "stdafx.h"
#include <boost/test/unit_test.hpp> 
#include <boost/shared_ptr.hpp>

BOOST_AUTO_TEST_SUITE( Rotation_Suite )

bool is_critical( CStdErrorInfo const& ex ) { return ex.m_lError < 0; }

BOOST_AUTO_TEST_CASE( CreateClassFactory )
{
    int i=5;
    i = 6;
}

BOOST_AUTO_TEST_SUITE_END()