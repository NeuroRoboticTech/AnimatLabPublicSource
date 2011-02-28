// VsIntersectionEvent.h: interface for the VsIntersectionEvent class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsIntersectionEvent : public AnimatSim::AnimatBase, public VxUniverse::IntersectSubscriber 
		{
		protected:
			Simulator *m_lpSim;

		public:
			VsIntersectionEvent();
			virtual ~VsIntersectionEvent();

		    void notifyIntersect(VxUniverse::eIntersectEventType type, VxIntersectResult* inResult, VxDynamicsResponseInput* inResp);
		    void notifyDisjoint(VxUniverse::eIntersectEventType type, VxIntersectResult* ires);

			virtual void Initialize(Simulator *lpSim);
		};

	}			// Visualization
}				//VortexAnimatSim
