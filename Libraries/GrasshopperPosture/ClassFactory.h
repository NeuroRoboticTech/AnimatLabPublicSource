// NlClassFactory.h: interface for the ClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_NLCLASSFACTORY_H__73275845_57BE_400D_AE40_1E548C6419D4__INCLUDED_)
#define AFX_NLCLASSFACTORY_H__73275845_57BE_400D_AE40_1E548C6419D4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace GrasshopperPosture
{

	class ClassFactory : public IStdClassFactory   
	{
	public:
		ClassFactory();
		virtual ~ClassFactory();

		virtual ExternalStimulus *CreateExternalStimulus(std::string strType, BOOL bThrowError = TRUE);

		virtual CStdSerialize *CreateObject(std::string strClassType, std::string strObjectType, BOOL bThrowError = TRUE);
	};

}				//FastNeuralNet

#endif // !defined(AFX_NLCLASSFACTORY_H__73275845_57BE_400D_AE40_1E548C6419D4__INCLUDED_)
