// BlAnimatCollisionDispatcher.h: interface for the BlAnimatCollisionDispatcher class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{

	class BULLET_PORT BlAnimatCollisionDispatcher : public btCollisionDispatcher 
	{
	protected:
		Simulator *m_lpSim;
        //static vector<btManifoldResult> m_aryBodyContacts;

	public:
		BlAnimatCollisionDispatcher(btCollisionConfiguration* collisionConfiguration, BlSimulator *lpSim);
		virtual ~BlAnimatCollisionDispatcher();

        //static bool IsContactObject(btCollisionObject* body0,btCollisionObject* body1);

        virtual bool	needsCollision(btCollisionObject* body0,btCollisionObject* body1);
    	//static void  AnimatNearCallback(btBroadphasePair& collisionPair, btCollisionDispatcher& dispatcher, const btDispatcherInfo& dispatchInfo);
    };

}				//BulletAnimatSim
