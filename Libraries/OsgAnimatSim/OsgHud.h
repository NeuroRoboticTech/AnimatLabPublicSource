/**
\file	OsgHud.h

\brief	Declares the vortex heads-up display class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

		class ANIMAT_OSG_PORT OsgHud : public AnimatSim::Hud 
		{
		protected:
			osg::ref_ptr<osg::Projection> m_osgProjection;
			osg::ref_ptr<osg::MatrixTransform> m_osgMT;

		public:
			OsgHud();
			virtual ~OsgHud();

			virtual void Initialize();
			virtual void Update();
		};

	}			// Visualization
}				//OsgAnimatSim
