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

	public:
		BlAnimatCollisionDispatcher(btCollisionConfiguration* collisionConfiguration, BlSimulator *lpSim);
		virtual ~BlAnimatCollisionDispatcher();

        virtual bool	needsCollision(btCollisionObject* body0,btCollisionObject* body1);
    };

}				//BulletAnimatSim
