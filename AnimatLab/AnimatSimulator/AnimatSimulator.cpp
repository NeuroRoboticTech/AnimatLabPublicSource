
#include "StdAfx.h"

int main(int argc, const char **argv)
{
	//Simulator *lpSim = NULL;

try
{
	//Simulator *lpSim = Simulator::CreateSimulator(argc, argv);

	//lpSim->Load();
	//lpSim->Initialize(argc, argv);
	//lpSim->Simulate();

	//if(lpSim) delete lpSim;

	return BootStrap_RunLibrary(argc, argv);
}
catch(char *str)
{
	//if(lpSim) delete lpSim;
	printf("Error occurred: %s\n", str) ;
	return -1;
}
catch(...)
{
	//if(lpSim) delete lpSim;
  printf("An Unknown Error occurred.\n") ;
	return -1;
}
}
