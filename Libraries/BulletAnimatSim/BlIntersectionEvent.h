// BlIntersectionEvent.h: interface for the BlIntersectionEvent class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Visualization
	{

		class BULLET_PORT BlIntersectionEvent : public VxUniverse::IntersectSubscriber 
		{
		protected:
			Simulator *m_lpSim;

		public:
			BlIntersectionEvent();
			virtual ~BlIntersectionEvent();

		    void notifyIntersect(VxUniverse::eIntersectEventType type, VxIntersectResult* inResult, VxDynamicsResponseInput* inResp);
		    void notifyDisjoint(VxUniverse::eIntersectEventType type, VxIntersectResult* ires);
		};

	}			// Visualization
}				//BulletAnimatSim
