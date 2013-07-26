
#include "StdAfx.h"

int main(int argc, const char **argv)
{

try
{ 
	return BootStrap_RunLibrary(argc, argv);
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
