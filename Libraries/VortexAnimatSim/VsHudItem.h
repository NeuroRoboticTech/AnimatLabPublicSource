// VsHudItem.h: interface for the VsHudItem class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsHudItem : public AnimatSim::AnimatBase
		{
		protected:

		public:
			VsHudItem();
			virtual ~VsHudItem();

			virtual void Initialize(osg::Projection *lpProjection) = 0;
			virtual void Update() = 0;
			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
