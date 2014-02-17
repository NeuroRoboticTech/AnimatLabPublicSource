/**
\file	btAnimatGeneric6DofConstraint.cpp

\brief	Implements an override of the btGeneric6DofConstraint so I can access key data.
**/

#include "StdAfx.h"
#include "btAnimatGeneric6DofConstraint.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

btAnimatGeneric6DofConstraint::btAnimatGeneric6DofConstraint(btRigidBody& rbA, btRigidBody& rbB, const btTransform& frameInA, const btTransform& frameInB ,bool useLinearReferenceFrameA) : 
        btGeneric6DofSpringConstraint(rbA, rbB, frameInA, frameInB, useLinearReferenceFrameA)
{
}

btAnimatGeneric6DofConstraint::btAnimatGeneric6DofConstraint(btRigidBody& rbB, const btTransform& frameInB, bool useLinearReferenceFrameB) : 
        btGeneric6DofSpringConstraint(rbB, frameInB, useLinearReferenceFrameB)
{
}

btVector3 btAnimatGeneric6DofConstraint::GetLinearForceAxis(int iAxis)
{
	btVector3 linear_axis;

	if (m_useLinearReferenceFrameA)
	    linear_axis = m_calculatedTransformA.getBasis().getColumn(iAxis);
	else
	    linear_axis = m_calculatedTransformB.getBasis().getColumn(iAxis);

    return linear_axis;
}

btVector3 btAnimatGeneric6DofConstraint::GetAngularForceAxis(int iAxis)
{
    return m_calculatedAxis[iAxis];
}

void btAnimatGeneric6DofConstraint::ApplyMotorForces(btScalar	timeStep)
{
    // angular
    btVector3 angular_axis;
    btScalar angularJacDiagABInv;
    int i=0;
    //for (int i=0;i<3;i++)
    //{
        if (m_angularLimits[i].needApplyTorques())
        {

			// get axis
			angular_axis = getAxis(i);

			angularJacDiagABInv = btScalar(1.) / m_jacAng[i].getDiagonal();

			m_angularLimits[i].solveAngularLimits(timeStep,angular_axis,angularJacDiagABInv, &m_rbA,&m_rbB);
        }
    //}
}


		}		//Joints
	}			// Environment
}				//BulletAnimatSim
