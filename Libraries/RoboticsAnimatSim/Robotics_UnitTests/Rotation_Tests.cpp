#include "stdafx.h"
#include <boost/test/unit_test.hpp> 
#include <boost/shared_ptr.hpp>

BOOST_AUTO_TEST_SUITE( Rotation_Suite )

bool is_critical( CStdErrorInfo const& ex ) { return ex.m_lError < 0; }

BOOST_AUTO_TEST_CASE( Dynamic_Loading )
{
 //   std::string strExePath = Std_ExecutablePath();
 //   std::string strExecutablePath, strExeFile;
	//Std_SplitPathAndFile(strExePath, strExecutablePath, strExeFile);

 //   std::string strProjFile = strExecutablePath + "../Libraries/RoboticsAnimatSim/Bullet_UnitTests/TestResources/SingleJoint_StandaloneD.asim";

	//Simulator *lpSim = Simulator::CreateSimulator("", strProjFile);
	// 
	//RigidBody *lpBody = dynamic_cast<RigidBody *>(lpSim->CreateObject("", "RigidBody", "Box"));
	//if(lpBody)
	//	delete lpBody;

 //   if(lpSim)
 //       delete lpSim;
}


BOOST_AUTO_TEST_SUITE_END()