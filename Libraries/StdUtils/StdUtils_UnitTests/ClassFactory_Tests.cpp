#include "stdafx.h"
#include <boost/test/unit_test.hpp> 
#include <boost/shared_ptr.hpp>

BOOST_AUTO_TEST_SUITE( ClassFactory_Suite )

bool is_critical( CStdErrorInfo const& ex ) { return ex.m_lError < 0; }

BOOST_AUTO_TEST_CASE( CreateClassFactory )
{
	//boost::shared_ptr<IStdClassFactory> lpFactory(IStdClassFactory::LoadModule("StdClassFactoryTester.dll"));
	boost::shared_ptr<IStdClassFactory> lpFactory(IStdClassFactory::LoadModule("BulletAnimatSim_vc10D.dll"));
	
	BOOST_CHECK( lpFactory != NULL );

	CStdSerialize *lpTest = NULL;
	BOOST_CHECK_EXCEPTION( lpTest = lpFactory->CreateObject("test1", "TestObject"), CStdErrorInfo, is_critical );
	BOOST_CHECK_EXCEPTION( lpTest = lpFactory->CreateObject("test", "TestObject1"), CStdErrorInfo, is_critical );

	boost::shared_ptr<CStdSerialize> lpTest2(lpFactory->CreateObject("test", "TestObject"));
	BOOST_CHECK( lpTest2 != NULL );

	CStdSerialize *lpClone = lpTest2->Clone();
	BOOST_CHECK( lpClone == NULL );
}

BOOST_AUTO_TEST_SUITE_END()