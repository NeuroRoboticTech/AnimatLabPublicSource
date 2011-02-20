// VsSimulationRecorder.h: interface for the VsSimulationRecorder class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSSIMULATIONRECORDER_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_)
#define AFX_VSSIMULATIONRECORDER_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace VortexAnimatLibrary
{
	namespace Recording
	{

		class VORTEX_PORT VsSimulationRecorder : public SimulationRecorder  
		{
		protected:

		public:
			VsSimulationRecorder();
			virtual ~VsSimulationRecorder();

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			//Recording
}				//VortexAnimatLibrary

#endif // !defined(AFX_VSSIMULATIONRECORDER_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_)
