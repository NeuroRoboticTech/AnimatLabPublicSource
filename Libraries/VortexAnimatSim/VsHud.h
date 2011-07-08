/**
\file	VsHud.h

\brief	Declares the vortex heads-up display class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsHud : public AnimatSim::Hud 
		{
		protected:
			osg::ref_ptr<osg::Projection> m_osgProjection;
			osg::ref_ptr<osg::MatrixTransform> m_osgMT;

		public:
			VsHud();
			virtual ~VsHud();

			virtual void Initialize();
			virtual void Update();
		};

	}			// Visualization
}				//VortexAnimatSim
