// TonicNeuron.h: interface for the TonicNeuron class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_TONICNEURON_H__1FCC8987_0FBA_4BC7_BAFC_1F480BA58768__INCLUDED_)
#define AFX_TONICNEURON_H__1FCC8987_0FBA_4BC7_BAFC_1F480BA58768__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace FiringRateSim
{
	namespace Neurons
	{

		class FAST_NET_PORT TonicNeuron : public Neuron  
		{
		protected:
			float m_fltIh;

			virtual float CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent);

		public:
			TonicNeuron();
			virtual ~TonicNeuron();

			float Ih();
			void Ih(float fltVal);

			virtual unsigned char NeuronType();

			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim


#endif // !defined(AFX_TONICNEURON_H__1FCC8987_0FBA_4BC7_BAFC_1F480BA58768__INCLUDED_)
