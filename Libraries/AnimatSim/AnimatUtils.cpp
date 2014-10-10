
#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"


namespace AnimatSim
{

	//float ANIMAT_PORT EvalGraph(unsigned char iGraphType, float fltA, float fltB, float fltC, float fltD, float fltQty)
	//{
	//	float fltVal=0;
	//	

	//	switch(iGraphType)
	//	{
	//	case LINEAR_GRAPH:
	//		if(fltA) fltVal = ((fltB/fltA)*fltQty + fltC);
	//		break;

	//	case BELL_GRAPH:
	//		if(fltB) 
	//		{
	//			fltVal = pow((float) (fltQty-fltA), (float) 2.0);
	//			fltVal = exp(-fltC * fltVal);
	//			fltVal = (fltB * fltVal) + fltD;	
	//		}
	//		//fltVal = (fltB * exp(-fltC * pow((fltQty-fltA), 2.0))) + fltD

	//		//fltVal = (fltB * exp(-fltC * pow(((fltQty-fltA)/fltB), 2.0))) + fltD;
	//		break;

	//	case SIGMOID_GRAPH:
	//		fltVal = (fltB/(1+exp(fltC*(fltA-fltQty)))) + fltD;
	//		break;

	//	case INVERSE_GRAPH:
	//		fltVal = pow((fltQty-fltC), fltB);
	//		if(fltVal)
	//			fltVal = (fltA / fltVal) + fltD;
	//		else
	//			fltVal = 0;
	//		break;
	//		//fltVal = (fltA / ((fltQty-fltC)^fltB)) + fltD;

	//	case QUADRATIC_GRAPH:
	//		fltVal = (fltA * pow((fltQty-fltC), fltB)) + fltD;
	//		break;

	//	}
	//	
	//	return fltVal;
	//}


	std::string ANIMAT_PORT GetFilePath(std::string strProjectPath, std::string strFilename)
	{
		//If they specify a full path name here then we will use that.
		//but if it is only a file name then we assume that the file
		//is located in the project path.
		std::string strPath;
		if(!Std_IsFullPath(strFilename))
		{
#ifndef WIN32
			//If we are not on a windows system then replace // slashes with \
			strFilename = Std_Replace(strFilename, "\\", "/"); 
#endif			
			strPath = strProjectPath + strFilename;
		}		
		else
			strPath = strFilename;

		return strPath;
	}

	//float ANIMAT_PORT LoadScaledNumber(CStdXml oXml, std::string strName, bool bThrowError = true, float fltDefault = 0)
	//{
	//	float fltValue = fltDefault;

	//	if(oXml.FindChildElement(strName, bThrowError))
	//	{
	//		oXml.IntoChildElement(strName);
	//		fltValue = oXml.GetAttribFloat("Actual", bThrowError, fltDefault);
	//		oXml.OutOfElem();
	//	}

	//	return fltValue;
	//}

}			//AnimatSim

