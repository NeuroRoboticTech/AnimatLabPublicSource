// BlAnimatCollisionDispatcher.cpp: implementation of the BlAnimatCollisionDispatcher class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
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

bool	BlAnimatCollisionDispatcher::needsCollision(btCollisionObject* body0,btCollisionObject* body1)
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
	
	return needsCollision ;

}

//void BlAnimatCollisionDispatcher::notifyIntersect(VxUniverse::eIntersectEventType type, VxIntersectResult* inResult, VxDynamicsResponseInput* inResp)
//{
//	//Only respond to the iniital contact and removal of the contact.
//	if ( type == VxUniverse::kEventFirst )
//	{
//        VxEntity *e1, *e2;
//        inResult->getEntityPair(&e1, &e2);
//		
//		if(e1 && e2)
//		{
//			void *v1 = e1->getUserData();
//			void *v2 = e2->getUserData();
//
//			if(v1 && v2)
//			{
//				RigidBody *lpBody1 = (RigidBody *) v1;
//				RigidBody *lpBody2 = (RigidBody *) v2;
//
//				if( !((lpBody1->Parent() == lpBody2) || (lpBody2->Parent() == lpBody1)) )
//				{
//					lpBody1->AddSurfaceContact(lpBody2);
//					lpBody2->AddSurfaceContact(lpBody1);
//				}
//			}
//		}
//	}
//}
//
//void BlAnimatCollisionDispatcher::notifyDisjoint(VxUniverse::eIntersectEventType type, VxIntersectResult* inResult)
//{
//	if ( type == VxUniverse::kEventDisjoint )
//	{
//        VxEntity *e1, *e2;
//        inResult->getEntityPair(&e1, &e2);
//		
//		if(e1 && e2)
//		{
//			void *v1 = e1->getUserData();
//			void *v2 = e2->getUserData();
//
//			if(v1 && v2)
//			{
//				RigidBody *lpBody1 = (RigidBody *) v1;
//				RigidBody *lpBody2 = (RigidBody *) v2;
//
//				if( !((lpBody1->Parent() == lpBody2) || (lpBody2->Parent() == lpBody1)) )
//				{
//					lpBody1->RemoveSurfaceContact(lpBody2);
//					lpBody2->RemoveSurfaceContact(lpBody1);
//				}
//			}
//		}
//	}
//}


}				//BulletAnimatSim
