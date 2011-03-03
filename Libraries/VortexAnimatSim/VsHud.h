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

			virtual VsHudItem *LoadHudItem(CStdXml &oXml);

		public:
			VsHud();
			virtual ~VsHud();

			virtual void Reset();
			virtual void Initialize();
			virtual void Update();

			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
