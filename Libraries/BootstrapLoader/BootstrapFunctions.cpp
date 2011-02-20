#include "stdafx.h"

void BOOTSTRAP_LOADER_PORT RunLibrary(int argc, const char **argv)
{

	int iParam=0;
	BOOL bRetrieved=FALSE, bFound = FALSE;
	char *strParam, *strLibrary;
	while(!bRetrieved && iParam<argc)
	{
		strParam = argv[iParam];

		if(bFound)
		{
			strLibrary = strParam;
			bRetrieved = TRUE;
		}

		if(_stricmp(strParam, "-library") == 0)
			bFound = TRUE;

		iParam++;
	}

	if(strlen(strLibrary) == 0)
		throw "No library was specified on the command line.";

}