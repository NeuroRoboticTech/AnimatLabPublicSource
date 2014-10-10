// VsIntersectionEvent.cpp: implementation of the VsIntersectionEvent class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsIntersectionEvent::VsIntersectionEvent() : 
        VxUniverse::IntersectSubscriber()
{
	m_lpSim = NULL;
}

VsIntersectionEvent::~VsIntersectionEvent()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsIntersectionEvent\r\n", "", -1, false, true);}
}

void VsIntersectionEvent::notifyIntersect(VxUniverse::eIntersectEventType type, VxIntersectResult* inResult, VxDynamicsResponseInput* inResp)
{
	//Only respond to the iniital contact and removal of the contact.
	if ( type == VxUniverse::kEventFirst )
	{
        VxEntity *e1, *e2;
        inResult->getEntityPair(&e1, &e2);
		
		if(e1 && e2)
		{
			void *v1 = e1->getUserData();
			void *v2 = e2->getUserData();

			if(v1 && v2)
			{
				RigidBody *lpBody1 = (RigidBody *) v1;
				RigidBody *lpBody2 = (RigidBody *) v2;

				if( !((lpBody1->Parent() == lpBody2) || (lpBody2->Parent() == lpBody1)) )
				{
					lpBody1->AddSurfaceContact(lpBody2);
					lpBody2->AddSurfaceContact(lpBody1);
				}
			}
		}
	}
}

void VsIntersectionEvent::notifyDisjoint(VxUniverse::eIntersectEventType type, VxIntersectResult* inResult)
{
	if ( type == VxUniverse::kEventDisjoint )
	{
        VxEntity *e1, *e2;
        inResult->getEntityPair(&e1, &e2);
		
		if(e1 && e2)
		{
			void *v1 = e1->getUserData();
			void *v2 = e2->getUserData();

			if(v1 && v2)
			{
				RigidBody *lpBody1 = (RigidBody *) v1;
				RigidBody *lpBody2 = (RigidBody *) v2;

				if( !((lpBody1->Parent() == lpBody2) || (lpBody2->Parent() == lpBody1)) )
				{
					lpBody1->RemoveSurfaceContact(lpBody2);
					lpBody2->RemoveSurfaceContact(lpBody1);
				}
			}
		}
	}
}


	}			// Visualization
}				//VortexAnimatSim
