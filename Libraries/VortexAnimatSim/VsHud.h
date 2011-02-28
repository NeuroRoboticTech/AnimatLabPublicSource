// VsHud.h: interface for the VsHud class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsHud : public AnimatSim::AnimatBase 
		{
		protected:
			CStdPtrArray<VsHudItem> m_aryHudItems;
			osg::ref_ptr<osg::Projection> m_osgProjection;
			osg::ref_ptr<osg::MatrixTransform> m_osgMT;

			virtual VsHudItem *LoadHudItem(Simulator *lpSim, CStdXml &oXml);

		public:
			VsHud();
			virtual ~VsHud();

			virtual void Reset();
			virtual void Initialize(Simulator *lpSim);
			virtual void Update(Simulator *lpSim);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
