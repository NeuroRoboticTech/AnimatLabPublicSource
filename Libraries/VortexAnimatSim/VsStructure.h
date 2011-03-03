// VsOrganism.h: interface for the VsOrganism class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSSTRUCTURE_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_)
#define AFX_VSSTRUCTURE_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{

	/**
	\namespace	VortexAnimatSim::Environment

	\brief	Classes for the virtual world simulation that use the vortex physics engine. 
	**/
	namespace Environment
	{
		class VORTEX_PORT VsStructure : public AnimatSim::Environment::Structure   
		{
		protected:
			Vx::VxAssembly *m_lpAssembly;
			osg::ref_ptr<osg::MatrixTransform> m_osgMT;

		public:
			VsStructure();
			virtual ~VsStructure();

			virtual void *Assembly() {return (void *)m_lpAssembly;};
			virtual void *GetMatrixPointer() {return m_osgMT.get();};

			virtual void Initialize();
			virtual void ResetSimulation();
			osgSim::DOFTransform* GetOSGNode();
		};

	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSSTRUCTURE_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_)
