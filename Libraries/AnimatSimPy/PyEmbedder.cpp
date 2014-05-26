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
	if(!Py_IsInitialized())
	{
		Py_SetProgramName("AnimatLab");
		Py_Initialize();
		PyRun_SimpleString("import os\n"
						   "import sys\n"
						   "sys.path.append(\"C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin\")\n"
						   "os.chdir(\"C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin\")\n"
						   "import AnimatSimPy\n");
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

}