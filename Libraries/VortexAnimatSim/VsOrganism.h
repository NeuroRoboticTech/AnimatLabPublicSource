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
			osg::ref_ptr<osg::MatrixTransform> m_osgMT;

		public:
			VsOrganism();
			virtual ~VsOrganism();

			virtual void *Assembly() {return (void *)m_lpAssembly;};
			virtual void *GetMatrixPointer() {return m_osgMT.get();};

			virtual void Initialize(Simulator *lpSim);
			virtual void ResetSimulation(Simulator *lpSim);
			void AddOSGNode(osg::ref_ptr<osg::Group> grpScene);
		};

	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSORGANISM_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_)
