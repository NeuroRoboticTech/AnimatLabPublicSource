// BootstrapLoader.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"

typedef int (*RunLibrary)(int argc, const char **argv); 

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    return TRUE;
}

/**
\brief	Boot strap method to run an arbitary library.

\author	dcofer
\date	4/7/2011

\exception	strError	Thrown when string error. 

\param	argc	The argc. 
\param	argv	The argv. 

\return	.
**/
int BOOTSTRAP_LOADER_PORT BootStrap_RunLibrary(int argc, const char **argv)
{
	int iParam=0;
	BOOL bRetrieved=FALSE, bFound = FALSE;
	char strParam[200], strLibrary[200], strError[200];

	//Clear out the library first.
	strcpy(strLibrary, "");

	while(!bRetrieved && iParam<argc)
	{
		strcpy(strParam, argv[iParam]);

		if(bFound)
		{
			strcpy(strLibrary, strParam);
			bRetrieved = TRUE;
		}

		if(_stricmp(strParam, "-library") == 0)
			bFound = TRUE;

		iParam++;
	}

	if(strlen(strLibrary) == 0)
		throw "No library was specified on the command line.";

	HMODULE hMod = LoadLibrary(strLibrary);
	
	if(!hMod)
	{
		sprintf(strError, "Unable to load the library module named '%s'.", strLibrary);
		throw strError;
	}

	RunLibrary lpRunLibrary = NULL;
  lpRunLibrary = (RunLibrary) GetProcAddress(hMod, "BootstrapRunLibrary"); 

	if(!lpRunLibrary)
	{
		sprintf(strError, "Unable to obtain a pointer to the run library entry function in library '%s'.", strLibrary);
		throw strError;
	}

	return lpRunLibrary(argc, argv);
}