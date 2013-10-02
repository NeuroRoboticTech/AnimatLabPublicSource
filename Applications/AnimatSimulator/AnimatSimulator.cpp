
#include "StdAfx.h"

int main(int argc, const char **argv)
{

try
{ 
#ifndef ANIMAT_STATIC
	return BootStrap_RunLibrary(argc, argv);
#else
	BulletAnimatSim::RunBootstrap(argc, argv);
#endif
}
catch(char *str)
{
	printf("Error occurred: %s\n", str) ;
	return -1;
}
catch(...)
{
  printf("An Unknown Error occurred.\n") ;
	return -1;
}
}
