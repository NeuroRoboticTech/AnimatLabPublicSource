// ArrayChart.h: interface for the ArrayChart class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ARRAYCHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
#define AFX_ARRAYCHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace Charting
	{

		class ANIMAT_PORT ArrayChart : public DataChart  
		{
		protected:
			CStdFPoint m_vArraySize;

		public:
			ArrayChart();
			virtual ~ArrayChart();

			virtual void CurrentRow(long iVal) {}; //We need to disable the ability to reset the current row. It should always be the same size as the array.

			virtual void Load(CStdXml &oXml);
			virtual void LoadChart(CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "ArrayChart";};

			virtual void Initialize(Simulator *lpSim);
			virtual void ReInitialize(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
		};

	}			//Charting
}				//AnimatSim

#endif // !defined(AFX_ARRAYCHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
