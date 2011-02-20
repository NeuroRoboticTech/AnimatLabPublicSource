// MemoryChart.h: interface for the MemoryChart class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_MEMORYCHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
#define AFX_MEMORYCHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace Charting
	{

		class ANIMAT_PORT MemoryChart : public DataChart  
		{
		protected:
			CStdCriticalSection m_oRowCountLock;

		public:
			MemoryChart();
			virtual ~MemoryChart();

			virtual BOOL Lock();
			virtual void Unlock();

			virtual void Load(Simulator *lpSim, CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "MemoryChart";};

			virtual void Initialize(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
		};

	}			//Charting
}				//AnimatSim

#endif // !defined(AFX_MEMORYCHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
