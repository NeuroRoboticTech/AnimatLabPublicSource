#include "stdafx.h"
#include <boost/test/unit_test.hpp> 

BOOST_AUTO_TEST_SUITE( ClassFactory_Suite )

bool is_critical( CStdErrorInfo const& ex ) { return ex.m_lError < 0; }

BOOST_AUTO_TEST_CASE( CreateClassFactory )
{
	IStdClassFactory *lpFactory=NULL;
	lpFactory = IStdClassFactory::LoadModule("StdClassFactoryTester.dll");
	
	BOOST_CHECK( lpFactory != NULL );

	CStdSerialize *lpTest = NULL;
	BOOST_CHECK_EXCEPTION( lpTest = lpFactory->CreateObject("test1", "TestObject"), CStdErrorInfo, is_critical );
	BOOST_CHECK_EXCEPTION( lpTest = lpFactory->CreateObject("test", "TestObject1"), CStdErrorInfo, is_critical );

	lpTest = lpFactory->CreateObject("test", "TestObject");
	BOOST_CHECK( lpTest != NULL );

	CStdSerialize *lpClone = lpTest->Clone();
	BOOST_CHECK( lpTest == NULL );

	if(lpFactory) 
		delete lpFactory;
}

BOOST_AUTO_TEST_SUITE_END()