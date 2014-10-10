#include "stdafx.h"
#include <boost/test/unit_test.hpp> 
#include <boost/shared_ptr.hpp>

BOOST_AUTO_TEST_SUITE(Embedded_Suite )

bool is_critical( CStdErrorInfo const& ex ) { return ex.m_lError < 0; }

BOOST_AUTO_TEST_CASE( Dynamic_Loading )
{
    Py_SetProgramName("AnimatLab");
    Py_Initialize();
	PyRun_SimpleString("import os\n"
					   "import sys\n"
					   "sys.path.append(\"C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin\")\n"
					   "os.chdir(\"C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin\")\n"
					   "import AnimatSimPy\n"
					   "simthread = AnimatSimPy.SimulationMgr.Instance().CreateSimulation(\"C:\\Projects\\AnimatLabSDK\\AnimatLabPublicSource\\Tutorials\\Examples\\StandAloneSimTest\\Bullet_Single_x32_Debug.asim\")\n"
                       "simthread.Simulate(2)\n");
                       //"AnimatSimPy.SimulationMgr.Instance().ShutdownAllSimulations()\n");

	int iCount = SimulationMgr::Instance().SimThreads().GetSize();

	Py_Finalize();

	//ObjectScriptPy PyObj;
	//std::cout << "ID: " << PyObj.ID() << "\r\n";

 //   std::string strExePath = Std_ExecutablePath();
 //   std::string strExecutablePath, strExeFile;
	//Std_SplitPathAndFile(strExePath, strExecutablePath, strExeFile);

 //   std::string strProjFile = strExecutablePath + "..\\Tutorials\\Examples\\StandAloneSimTest\\Bullet_Single_x32_Debug.asim";

	//SimulationThread *lpThread = SimulationMgr::Instance().CreateSimulation(strProjFile);
	//lpThread->Simulate(2.0);
	//SimulationMgr::Instance().ShutdownAllSimulations();
}


BOOST_AUTO_TEST_SUITE_END()