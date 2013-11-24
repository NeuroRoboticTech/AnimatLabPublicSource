// BlAnimatCollisionDispatcher.cpp: implementation of the BlAnimatCollisionDispatcher class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlRigidBody.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlAnimatCollisionDispatcher::BlAnimatCollisionDispatcher(btCollisionConfiguration* collisionConfiguration, BlSimulator *lpSim) : 
        btCollisionDispatcher(collisionConfiguration),
        m_lpSim(lpSim)
{
    //setNearCallback(AnimatNearCallback);
}

BlAnimatCollisionDispatcher::~BlAnimatCollisionDispatcher()
{

try
{
    m_lpSim = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BlAnimatCollisionDispatcher\r\n", "", -1, false, true);}
}

bool	BlAnimatCollisionDispatcher::needsCollision(const btCollisionObject* body0, const btCollisionObject* body1)
{
	btAssert(body0);
	btAssert(body1);

	bool needsCollision = true;

#ifdef BT_DEBUG
	if (!(m_dispatcherFlags & btCollisionDispatcher::CD_STATIC_STATIC_REPORTED))
	{
		//broadphase filtering already deals with this
		if (body0->isStaticOrKinematicObject() && body1->isStaticOrKinematicObject())
		{
			m_dispatcherFlags |= btCollisionDispatcher::CD_STATIC_STATIC_REPORTED;
			printf("warning btCollisionDispatcher::needsCollision: static-static collision!\n");
		}
	}
#endif //BT_DEBUG

	if ((!body0->isActive()) && (!body1->isActive()))
		needsCollision = false;
	else if (!body0->checkCollideWith(body1))
		needsCollision = false;
	
    BlBulletData *lpData1 = (BlBulletData *) body0->getUserPointer();
    BlBulletData *lpData2 = (BlBulletData *) body1->getUserPointer();

    if(lpData1 && lpData2)
    {
        if(lpData1->m_bExclusionProcessing && lpData1->m_lpBody && lpData2->m_lpBody && !lpData1->m_lpBody->NeedCollision(lpData2->m_lpBody))
           return false;

        if(lpData2->m_bExclusionProcessing && lpData2->m_lpBody && lpData1->m_lpBody && !lpData2->m_lpBody->NeedCollision(lpData1->m_lpBody))
           return false;
    }

	return needsCollision ;

}
//
//bool BlAnimatCollisionDispatcher::IsContactObject(btCollisionObject* body0, btCollisionObject* body1)
//{
//    if( ((body0->getCollisionFlags() & AnimatCollisionTypes::CONTACT_SENSOR) || (body1->getCollisionFlags() & AnimatCollisionTypes::CONTACT_SENSOR)) ||
//        ((body0->getCollisionFlags() & AnimatCollisionTypes::RECEPTIVE_FIELD_SENSOR) || (body1->getCollisionFlags() & AnimatCollisionTypes::RECEPTIVE_FIELD_SENSOR)) )
//        return true;
//
//    return false;
//}
//
//
////by default, Bullet will use this near callback
//void BlAnimatCollisionDispatcher::AnimatNearCallback(btBroadphasePair& collisionPair, btCollisionDispatcher& dispatcher, const btDispatcherInfo& dispatchInfo)
//{
//		btCollisionObject* colObj0 = (btCollisionObject*)collisionPair.m_pProxy0->m_clientObject;
//		btCollisionObject* colObj1 = (btCollisionObject*)collisionPair.m_pProxy1->m_clientObject;
//
//		if (dispatcher.needsCollision(colObj0,colObj1))
//		{
//			//dispatcher will keep algorithms persistent in the collision pair
//			if (!collisionPair.m_algorithm)
//			{
//				collisionPair.m_algorithm = dispatcher.findAlgorithm(colObj0,colObj1);
//			}
//
//			if (collisionPair.m_algorithm)
//			{
//				btManifoldResult contactPointResult(colObj0,colObj1);
//				
//				if (dispatchInfo.m_dispatchFunc == 		btDispatcherInfo::DISPATCH_DISCRETE)
//				{
//					//discrete collision detection query
//					collisionPair.m_algorithm->processCollision(colObj0,colObj1,dispatchInfo,&contactPointResult);
//				} else
//				{
//					//continuous collision detection query, time of impact (toi)
//					btScalar toi = collisionPair.m_algorithm->calculateTimeOfImpact(colObj0,colObj1,dispatchInfo,&contactPointResult);
//					if (dispatchInfo.m_timeOfImpact > toi)
//						dispatchInfo.m_timeOfImpact = toi;
//
//				}
//			}
//		}
//
//}

}				//BulletAnimatSim
