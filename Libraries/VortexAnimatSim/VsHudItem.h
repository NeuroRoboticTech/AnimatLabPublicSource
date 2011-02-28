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

			virtual void Initialize(Simulator *lpSim, osg::Projection *lpProjection) = 0;
			virtual void Update(Simulator *lpSim) = 0;
			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
