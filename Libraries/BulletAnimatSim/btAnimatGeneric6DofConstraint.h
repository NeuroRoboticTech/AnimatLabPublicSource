/**
\file	btAnimatGeneric6DofConstraint.h

\brief	Declares the btGeneric6DofConstraint class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	BulletAnimatSim::Environment::Joints

		\brief	Implements an override of the btGeneric6DofConstraint so I can access key data.. 
		**/
		namespace Joints
		{

            class BULLET_PORT btAnimatGeneric6DofConstraint : public btGeneric6DofSpringConstraint
            {
            public:
                btAnimatGeneric6DofConstraint(btRigidBody& rbA, btRigidBody& rbB, const btTransform& frameInA, const btTransform& frameInB ,bool useLinearReferenceFrameA);
                btAnimatGeneric6DofConstraint(btRigidBody& rbB, const btTransform& frameInB, bool useLinearReferenceFrameB);

                virtual btVector3 GetLinearForceAxis(int iAxis);
                virtual btVector3 GetAngularForceAxis(int iAxis);
                virtual void ApplyMotorForces(btScalar	timeStep);
            };

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
