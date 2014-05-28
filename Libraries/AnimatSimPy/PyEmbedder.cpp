#include "StdAfx.h"
#include "PyEmbedder.h"

#if defined(_DEBUG) && defined(SWIG_PYTHON_INTERPRETER_NO_DEBUG)
/* Use debug wrappers with the Python release dll */
# undef _DEBUG
# include <Python.h>
# define _DEBUG
#else
# include <Python.h>
#endif

namespace AnimatSimPy
{

PyEmbedder::PyEmbedder(void)
{
	m_bPyInit = false;
	InitPython();
}

PyEmbedder::~PyEmbedder(void)
{
try
{
	FinalizePython();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PyEmbedder\r\n", "", -1, false, true);}
}

void PyEmbedder::InitPython()
{
	int iRet = 0;
	if(!Py_IsInitialized())
	{
		Py_SetProgramName("AnimatLab");
		Py_Initialize();

		if(!Py_IsInitialized())
			THROW_ERROR(Al_Err_lFailedToInitPython, Al_Err_strFailedToInitPython);

		Simulator *lpSim = ActiveSim();
		if(!lpSim)
			THROW_ERROR(Al_Err_lSimNotDefined, Al_Err_strSimNotDefined);

		std::string strExePath = lpSim->ExecutablePath();
		strExePath = Std_Replace(strExePath, "\\", "/");

		//Import base modules and set paths correctly.
		std::string strInit = "import os,sys,traceback,math\n"
							  "sys.path.append(\"" + strExePath + "\")\n"
							  "os.chdir(\"" + strExePath + "\")\n";
		iRet = PyRun_SimpleString(strInit.c_str());
		if(iRet != 0)
			THROW_ERROR(Al_Err_lFailedToInitPython, Al_Err_strFailedToInitPython);

		//Try to import AnimatSimPy
		iRet = PyRun_SimpleString("import AnimatSimPy\n");
		if(iRet != 0)
			THROW_ERROR(Al_Err_lFailedToImportAnimatSimPy, Al_Err_strFailedToImportAnimatSimPy);

		//Setup sys exception handler for animatsim.
		std::string strExcept = "def animat_excepthook(type, value, tb):\n"
							    "    strErr = \"\".join(traceback.format_exception(type, value, tb))\n"
							    "    AnimatSimPy.SetLastScriptError(strErr)\n"
							    "sys.excepthook = animat_excepthook\n";
		iRet = PyRun_SimpleString(strExcept.c_str());
		if(iRet != 0)
			THROW_ERROR(Al_Err_lFailedToInitPython, Al_Err_strFailedToInitPython);

		//For testing of exception handler.
		//std::string strTest = "a = \"text\"\n"
		//					  "b = 5\n"
		//					  "error = a + b\n";
		//iRet = PyRun_SimpleString(strTest.c_str());
		//if(iRet != 0)
		//{
		//	std::string strErr = GetLastScriptError();
		//}

		m_bPyInit = true;
	}
}

void PyEmbedder::FinalizePython()
{
	if(Py_IsInitialized())
		Py_Finalize();
}

void PyEmbedder::ResetPython()
{
	FinalizePython();
	InitPython();
}

bool PyEmbedder::ExecutePythonScript(const std::string strPy, bool bThrowError)
{
	if(m_bPyInit && strPy.length() > 0)
	{
		int iRet = PyRun_SimpleString(strPy.c_str());
		if(iRet != 0)
		{
			std::string strErr = GetLastScriptError();

			if(bThrowError)
				THROW_PARAM_ERROR(Al_Err_lExecutingPythonScript, Al_Err_strExecutingPythonScript, "Trace:\r\n", strErr);
			else
				Std_LogMsg(StdLogError, strErr, "", -1, true);

			return false;
		}
		else
			return true;
	}

	return true;
}


}