// ModulatedSynapse.h: interface for the ModulatedSynapse class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_MODULATEDSYNAPSE_H__08C962A1_0861_455D_9EDD_F51E9E8AE94B__INCLUDED_)
#define AFX_MODULATEDSYNAPSE_H__08C962A1_0861_455D_9EDD_F51E9E8AE94B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace FiringRateSim
{
	namespace Synapses
	{

		class FAST_NET_PORT ModulatedSynapse : public Synapse    
		{
		public:
			ModulatedSynapse();
			virtual ~ModulatedSynapse();

#pragma region DataAccesMethods
			virtual float *GetDataPointer(string strDataType);
#pragma endregion

			virtual float CalculateModulation(FiringRateModule *lpModule);
		};

	}			//Synapses
}				//FiringRateSim

#endif // !defined(AFX_MODULATEDSYNAPSE_H__08C962A1_0861_455D_9EDD_F51E9E8AE94B__INCLUDED_)
