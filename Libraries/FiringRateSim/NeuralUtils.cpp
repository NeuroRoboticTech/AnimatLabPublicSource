#include "stdafx.h"


#include "Synapse.h"
#include "GatedSynapse.h"
#include "ModulatedSynapse.h"
#include "Neuron.h"
#include "PacemakerNeuron.h"
#include "RandomNeuron.h"
#include "FiringRateModule.h"
#include "ClassFactory.h"

string Nl_NeuralModuleName()
{
	return "FiringRateSim";
	//#ifdef _DEBUG
	//	return "FastNeuralNet_vc9D.dll";
	//#else
	//	return "FastNeuralNet_vc9.dll";
	//#endif
}

float FAST_NET_PORT Nl_EvalGraph(unsigned char iGraphType, float fltA, float fltB, float fltC, float fltD, float fltQty)
{
	float fltVal=0;

	switch(iGraphType)
	{
	case LINEAR_GRAPH:
		if(fltA) fltVal = ((fltB/fltA)*fltQty + fltC);
		break;

	case BELL_GRAPH:
		if(fltB) 
		{
			fltVal = pow((float) (fltQty-fltA), (float) 2.0);
			fltVal = exp(-fltC * fltVal);
			fltVal = (fltB * fltVal) + fltD;	
		}
		//fltVal = (fltB * exp(-fltC * pow((fltQty-fltA), 2.0))) + fltD

		//fltVal = (fltB * exp(-fltC * pow(((fltQty-fltA)/fltB), 2.0))) + fltD;
		break;

	case SIGMOID_GRAPH:
		fltVal = (fltB/(1+exp(fltC*(fltA-fltQty)))) + fltD;
		break;

	case INVERSE_GRAPH:
		fltVal = pow((fltQty-fltC), fltB);
		if(fltVal)
			fltVal = (fltA / fltVal) + fltD;
		else
			fltVal = 0;
		break;
		//fltVal = (fltA / ((fltQty-fltC)^fltB)) + fltD;

	case QUADRATIC_GRAPH:
		fltVal = (fltA * pow((fltQty-fltC), fltB)) + fltD;
		break;

	}

	return fltVal;
}


string Nl_GetFilePath(string strProjectPath, string strFilename)
{
	//If they specify a full path name here then we will use that.
	//but if it is only a file name then we assume that the file
	//is located in the project path.
	string strPath;
	if(!Std_IsFullPath(strFilename))
		strPath = strProjectPath + strFilename;
	else
		strPath = strFilename;

	return strPath;
}


extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
{
	IStdClassFactory *lpFactory = new ClassFactory;
	return lpFactory;
}



