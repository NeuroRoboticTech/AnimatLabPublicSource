// VsOrganism.h: interface for the VsOrganism class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSORGANISM_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_)
#define AFX_VSORGANISM_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		class VORTEX_PORT VsOrganism : public AnimatSim::Environment::Organism   
		{

		protected:
			Vx::VxAssembly *m_lpAssembly;

		public:
			VsOrganism();
			virtual ~VsOrganism();

			virtual void *Assembly() {return (void *)m_lpAssembly;};

			virtual void ResetSimulation();
		};

	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSORGANISM_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_)
