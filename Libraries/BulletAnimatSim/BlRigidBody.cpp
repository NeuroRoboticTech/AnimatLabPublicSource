// BlRigidBody.cpp: implementation of the BlRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlRigidBody.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlRigidBody::BlRigidBody()
{
    m_btCompoundShape = NULL;
    m_btCollisionShape = NULL;
    m_btCollisionObject = NULL;
    m_btPart = NULL;
    m_osgbMotion = NULL;
    m_lpBulletData = NULL;
    m_fltStaticMasses = 0;

    m_lpVsSim = NULL;
}

BlRigidBody::~BlRigidBody()
{

try
{
    m_aryContactPoints.Clear();
    //Cleanup of all BlRigidBody objects is taken care of in the DeletePhysics call
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BlRigidBody\r\n", "", -1, false, true);}
}

bool BlRigidBody::Physics_IsDefined()
{
    if(m_btPart && m_btCollisionShape)
        return true;
    else
        return false;
}

bool BlRigidBody::Physics_IsGeometryDefined()
{
    if(m_btCollisionShape)
        return true;
    else
        return false;
}

CStdFPoint BlRigidBody::Physics_GetCurrentPosition()
{
	if(m_osgbMotion && m_lpThisMI)
	{
        btTransform trans;
        m_osgbMotion->getWorldTransform(trans);
 
        btVector3 vPos = trans.getOrigin();
        m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
		return m_lpThisMI->AbsolutePosition();
	}
	else
	{
		CStdFPoint vPos;
		return vPos;
	}
}

BlSimulator *BlRigidBody::GetBlSimulator()
{
    if(!m_lpVsSim)
    {
    	m_lpVsSim = dynamic_cast<BlSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisVsMI)
		    THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpVsSim, " + m_lpThisAB->Name());
    }
	return m_lpVsSim;
}

void BlRigidBody::Physics_UpdateNode()
{
	if(m_lpThisRB)
	{
		if(m_lpThisRB->IsContactSensor())
			ResetSensorCollisionGeom();
		else if(m_lpThisRB->HasStaticJoint())
			ResetStaticCollisionGeom(); //If this body uses a static joint then we need to reset the offest matrix for its collision geometry.
        else if(m_btPart)
			ResetDynamicCollisionGeom();
	}

	Physics_UpdateAbsolutePosition();
}

void BlRigidBody::Physics_SetFreeze(bool bVal)
{
    if(m_lpThisRB && m_btPart)
    {
        if(m_lpThisRB->Freeze())
            m_btPart->setActivationState(0);
        else
            m_btPart->setActivationState(ACTIVE_TAG);
    }
}

void BlRigidBody::Physics_SetDensity(float fltVal)
{
    //FIX PHYSICS
	//if(m_vxSensor)
	//	m_vxCollisionGeometry->setRelativeDensity(fltVal);
}

void BlRigidBody::Physics_SetMaterialID(string strID)
{
    //FIX PHYSICS
	//if(m_vxCollisionGeometry && m_lpThisAB)
	//{
	//	int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(strID);
	//
	//	if(iMaterialID >= 0)
	//		m_vxCollisionGeometry->setMaterialID(iMaterialID);
	//}
}

void BlRigidBody::Physics_SetVelocityDamping(float fltLinear, float fltAngular)
{
    //FIX PHYSICS
	//if(m_vxPart && fltLinear > 0)
	//	m_vxPart->setLinearVelocityDamping(fltLinear);

	//if(m_vxPart && fltAngular > 0)
	//	m_vxPart->setAngularVelocityDamping(fltAngular);
}

void BlRigidBody::Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz)
{
    //FIX PHYSICS
	//if(m_vxPart)
	//{
	//	if(fltTx != 0 || fltTy != 0 || fltTz != 0)
	//		m_vxPart->setCOMOffset(fltTx, fltTy, fltTz);
	//}
}

void  BlRigidBody::Physics_FluidDataChanged()
{
    //FIX PHYSICS
	//if(m_vxCollisionGeometry && m_lpThisRB)
	//{
	//	CStdFPoint vpCenter = m_lpThisRB->BuoyancyCenter();
	//	Vx::VxReal3 vCenter = {vpCenter.x, vpCenter.y, vpCenter.z};

	//	CStdFPoint vpDrag = m_lpThisRB->Drag();
	//	Vx::VxReal3 vDrag = {vpDrag.x, vpDrag.y, vpDrag.z};

	//	float fltScale = m_lpThisRB->BuoyancyScale();
	//	float fltMagnus = m_lpThisRB->Magnus();
	//	bool bEnabled = m_lpThisRB->EnableFluids();

	//	m_vxCollisionGeometry->setFluidInteractionData(vCenter, vDrag, fltScale, fltMagnus);

	//	if(bEnabled)
	//		m_vxCollisionGeometry->setDefaultFluidInteractionForceFn();
	//	else
	//		m_vxCollisionGeometry->setFluidInteractionForceFn(NULL);
	//}
}

void BlRigidBody::Physics_WakeDynamics()
{
    //FIX PHYSICS
    //if(m_vxPart)
    //    m_vxPart->wakeDynamics();
}

void BlRigidBody::GetBaseValues()
{
    //FIX PHYSICS
	//if(m_vxPart && m_lpThisRB)
	//{
	//	//Fluid Density is in the units being used. So if the user set 1 g/cm^3 
	//	//and the units were grams and decimeters then density would be 1000 g/dm^3

	//	//Recalculate the mass and volume
	//	m_lpThisRB->GetMass();
	//	m_lpThisRB->GetVolume();

	//	//m_fltBuoyancy = -(m_lpThisAB->GetSimulator()->FluidDensity() * fltVolume * m_lpThisAB->GetSimulator()->Gravity());
	//	//m_fltReportBuoyancy = m_fltBuoyancy * m_lpThisAB->GetSimulator()->MassUnits() * m_lpThisAB->GetSimulator()->DistanceUnits();
	//}
}

void BlRigidBody::CreateSensorPart()
{
    BlSimulator *lpSim = GetBlSimulator();

	if(m_lpThisRB && m_lpThisAB && m_btCollisionShape)
	{
		BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpThisRB->Parent());

        m_btCollisionObject = new btCollisionObject;
        m_btCollisionObject->setCollisionShape( m_btCollisionShape );
        m_btCollisionObject->setCollisionFlags( btCollisionObject::CF_KINEMATIC_OBJECT );
        m_btCollisionObject->setWorldTransform( osgbCollision::asBtTransform( GetOSGWorldMatrix() ) );

        if(!m_lpBulletData)
            m_lpBulletData = new BlBulletData(this, false);

        m_btCollisionObject->setUserPointer((void *) m_lpBulletData);

        lpSim->DynamicsWorld()->addCollisionObject( m_btCollisionObject, AnimatCollisionTypes::CONTACT_SENSOR, ALL_COLLISIONS );

        int iFlags = m_btCollisionObject->getCollisionFlags() | btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK;
        m_btCollisionObject->setCollisionFlags(iFlags);
    }
}

void BlRigidBody::CreateDynamicPart()
{
	if(m_lpThisRB && m_lpThisAB && m_btCollisionShape)
    {
        BlSimulator *lpSim = GetBlSimulator();

        float fltMass = 0;
        if(!m_lpThisRB->Freeze())
            fltMass = m_lpThisRB->GetMassValue();

        btScalar mass(fltMass);
	    btVector3 localInertia( 0, 0, 0 );
        const bool isDynamic = ( mass != 0.f );
	    if( isDynamic )
		    m_btCollisionShape->calculateLocalInertia( mass, localInertia );

        //Keep a copy of the matrix transform for osgMT so I can reset it back later
        osg::Matrix osgMTmat = m_osgMT->getMatrix();

        // Create MotionState to control OSG subgraph visual reprentation transform
        // from a Bullet world transform. To do this, the MotionState need the address
        // of the Transform node (must be either AbsoluteModelTransform or
        // MatrixTransform), center of mass, scale vector, and the parent (or initial)
        // transform (usually the non-scaled OSG local-to-world matrix obtained from
        // the parent node path).
        m_osgbMotion = new osgbDynamics::MotionState();
        m_osgbMotion->setTransform( m_osgMT.get() );

        osg::Vec3 com;
	    CStdFPoint vCOM = m_lpThisRB->CenterOfMass();
	    if(vCOM.x != 0 || vCOM.y != 0 || vCOM.z != 0)
		    com = osg::Vec3(vCOM.x, vCOM.y, vCOM.z);
        else
            com = osg::Vec3(0, 0, 0);
        m_osgbMotion->setCenterOfMass( com );

        m_osgbMotion->setScale( osg::Vec3( 1., 1., 1. ) );
        m_osgbMotion->setParentTransform( osg::Matrix::identity() );

        // Finally, create rigid body.
        btRigidBody::btRigidBodyConstructionInfo rbInfo( mass, m_osgbMotion, m_btCollisionShape, localInertia );
        rbInfo.m_friction = btScalar( 1 );
        rbInfo.m_restitution = btScalar( 1 );
        rbInfo.m_linearDamping = m_lpThisRB->LinearVelocityDamping();
        rbInfo.m_angularDamping = m_lpThisRB->AngularVelocityDamping();

        m_btPart = new btRigidBody( rbInfo );

        if(!m_lpBulletData)
            m_lpBulletData = new BlBulletData(this, false);

        m_btPart->setUserPointer((void *) m_lpBulletData);

        // Last thing to do: Position the rigid body in the world coordinate system. The
        // MotionState has the initial (parent) transform, and also knows how to account
        // for center of mass and scaling. Get the world transform from the MotionState,
        // then set it on the rigid body, which in turn sets the world transform on the
        // MotionState, which in turn transforms the OSG subgraph visual representation.
        btTransform wt = osgbCollision::asBtTransform(osgMTmat);
        m_osgbMotion->setWorldTransform( wt );
        m_btPart->setWorldTransform( wt );

	    //if this body is frozen; freeze it
        if(m_lpThisRB->Freeze())
            m_btPart->setActivationState(0);
        else
            m_btPart->setActivationState(ACTIVE_TAG);

        //If this part has a receptive field then turn on the custom callbacks.
        if(m_lpThisRB->GetContactSensor())
        {
            int iFlags = m_btPart->getCollisionFlags() | btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK;
            m_btPart->setCollisionFlags(iFlags);
        }

        lpSim->DynamicsWorld()->addRigidBody( m_btPart, AnimatCollisionTypes::RIGID_BODY, ALL_COLLISIONS );

        //FIX PHYSICS
	    // Create the physics object.
	    //m_vxPart = new VxPart;
	    //m_vxSensor = m_vxPart;
	    //m_vxSensor->setUserData((void*) m_lpThisRB);
	    //int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(m_lpThisRB->MaterialID());

	    //m_vxSensor->setName(m_lpThisAB->ID().c_str());               // Give it a name.
	    //m_vxSensor->setControl(ConvertControlType());  // Set it to dynamic.
	    //CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisRB->Density()));
           
        if(m_lpThisRB->DisplayDebugCollisionGraphic())
        {
            m_osgDebugNode = osgbCollision::osgNodeFromBtCollisionShape( m_btCollisionShape );
            m_osgDebugNode->setName(m_lpThisRB->Name() + "_Debug");
	        m_osgNodeGroup->addChild(m_osgDebugNode.get());	
        }

        GetBaseValues();
    }
}

void BlRigidBody::AddStaticGeometry(BlRigidBody *lpChild)
{
    if(m_btPart && lpChild && lpChild->m_btCollisionShape)
    {
 /*       if(!m_bt

        float fltChildMass = 0;
        if(!m_lpThisRB->Freeze())
            fltChildMass = lpChild->m_lpThisRB->GetMassValue();

        m_fltStaticMasses += fltChildMass;
        btVector3 localInertia( 0, 0, 0 );
        
        

        btParentPart->setMassProps(0, localInertia);
	    btParentPart->updateInertiaTensor();

        
        lpVsParent->DeleteDynamicPart();
        lpVsParent->AddStaticPartShape(this);
        lpVsParent->*/
    }

}

void BlRigidBody::RemoveStaticGeometry(BlRigidBody *lpChild)
{
}

void BlRigidBody::CreateStaticPart()
{
	BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpThisRB->Parent());

    if(m_lpThisRB && m_lpThisAB && lpVsParent && lpVsParent->Part())
        lpVsParent->AddStaticGeometry(this);
	//if(m_lpThisRB && m_lpThisAB)
	//{
	//	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThisRB->Parent());

	//	Vx::VxReal44 vOffset;
	//	VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);
	//	int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(m_lpThisRB->MaterialID());

	//	if(lpVsParent)
	//	{
	//		Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
	//		if(vxSensor)
	//		{
	//			CollisionGeometry(vxSensor->addGeometry(m_vxGeometry, iMaterialID, vOffset, m_lpThisRB->Density()));
	//			string strName = m_lpThisAB->ID() + "_CollisionGeometry";
	//			m_vxCollisionGeometry->setName(strName.c_str());
	//		}
	//	}
	//}
}

void BlRigidBody::RemoveStaticPart()
{
	BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpThisRB->Parent());

	if(lpVsParent)
	{
        //FIX PHYSICS
		//Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
		//if(vxSensor && m_vxCollisionGeometry)
		//	vxSensor->removeCollisionGeometry(m_vxCollisionGeometry);
	}
}

void BlRigidBody::ResetStaticCollisionGeom()
{
	if(m_osgMT.valid() && m_lpThisRB && m_lpThisRB->Parent())
	{
		BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpThisRB->Parent());

		if(lpVsParent)
		{
            //FIX PHYSICS
			//Vx::VxReal44 vOffset;
			//VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);

			//Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
			//if(vxSensor && m_vxCollisionGeometry)
			//	m_vxCollisionGeometry->setRelativeTransform(vOffset);
		}
	}
}

void BlRigidBody::ResetSensorCollisionGeom()
{
    if(m_osgMT.valid() && m_osgbMotion && m_btCollisionObject)
    {
        osg::Matrix osgMTmat = m_osgMT->getMatrix();
        btTransform wt = osgbCollision::asBtTransform(osgMTmat);
        m_osgbMotion->setWorldTransform( wt );
        m_btCollisionObject->setWorldTransform( wt );
    }
}

void BlRigidBody::ResetDynamicCollisionGeom()
{
    if(m_osgMT.valid() && m_osgbMotion && m_btPart)
    {
        osg::Matrix osgMTmat = m_osgMT->getMatrix();
        btTransform wt = osgbCollision::asBtTransform(osgMTmat);
        m_osgbMotion->setWorldTransform( wt );
        m_btPart->setWorldTransform( wt );
    }
}

void BlRigidBody::DeleteDynamicPart()
{
    if(m_btPart)
    {
        GetBlSimulator()->DynamicsWorld()->removeRigidBody(m_btPart);
        delete m_btPart;
        m_btPart = NULL;

        if(m_osgbMotion)
            {delete m_osgbMotion; m_osgbMotion = NULL;}

        if(m_lpBulletData)
            {delete m_lpBulletData; m_lpBulletData = NULL;}
    }
}

void BlRigidBody::DeleteSensorPart()
{
    if(m_btCollisionObject)
    {
        GetBlSimulator()->DynamicsWorld()->removeCollisionObject(m_btCollisionObject);
        delete m_btCollisionObject;
        m_btCollisionObject = NULL;

        if(m_lpBulletData)
            {delete m_lpBulletData; m_lpBulletData = NULL;}
    }
}

void BlRigidBody::DeleteCollisionGeometry()
{
    if(m_btCompoundShape)
    {
        int iCount = m_btCompoundShape->getNumChildShapes();
        for(int iIdx=0; iIdx<iCount; iIdx++)
            m_btCompoundShape->removeChildShapeByIndex(0);
        delete m_btCompoundShape;
        m_btCompoundShape = NULL;
    }

    if(m_btCollisionShape)
        {delete m_btCollisionShape; m_btCollisionShape = NULL;}
}

void BlRigidBody::DeletePhysics()
{
	if(m_btPart)
	{
        DeleteDynamicPart();
        DeleteSensorPart();
        DeleteCollisionGeometry();
	}
	else if(m_lpThisRB && m_lpThisRB->HasStaticJoint())
		RemoveStaticPart();
}

bool BlRigidBody::NeedCollision(BlRigidBody *lpTest)
{
    //If we find this objects rigid body in the list of collision exclusions then we want to skip this collision.
    if(m_lpThisRB->FindCollisionExclusionBody(lpTest->m_lpThisRB, false))
        return false;
    else
        return true;
}

void BlRigidBody::Physics_ContactSensorAdded(ContactSensor *lpSensor)
{
    if(m_btPart)
    {
        int iFlags = m_btPart->getCollisionFlags() | btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK;
        m_btPart->setCollisionFlags(iFlags);
    }
}

void BlRigidBody::Physics_ContactSensorRemoved()
{
    if(m_btPart)
    {
        int iFlags = m_btPart->getCollisionFlags() & ~btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK;
        m_btPart->setCollisionFlags(iFlags);
    }
}

void BlRigidBody::ProcessContacts()
{
    ContactSensor *lpSensor = m_lpThisRB->GetContactSensor();

    if(lpSensor)
        lpSensor->ClearCurrents();

    //If this is a contact sensor then we do not care about processing the force and position.
    //We only care about the contact number.
    if(m_lpThisRB->IsContactSensor())
        m_lpThisRB->SetSurfaceContactCount(m_aryContactPoints.size());
    else if(m_aryContactPoints.size() > 0 && m_btPart && lpSensor)
    {
		int iPartIdx=0;
		StdVector3 vBodyPos;
		float fltForceMag = 0;
	    float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	    float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();

		//if(m_lpThisRB->GetSimulator()->TimeSlice() == 550 || m_lpThisRB->GetSimulator()->TimeSlice() == 9550 || m_lpThisRB->GetSimulator()->TimeSlice() == 10550)
		//	fltForceMag = 0;

		int iCount=m_aryContactPoints.size();
		for(int iIdx=0; iIdx<iCount; iIdx++)
		{
            BlContactPoint *lpContactPoint = m_aryContactPoints[iIdx];

            if(lpContactPoint && lpContactPoint->m_lpCP)
            {
                lpContactPoint->m_lpCP->m_localPointB;

                if(lpContactPoint->m_bIsBodyA)
                {
                    vBodyPos[0] = lpContactPoint->m_lpCP->m_localPointA[0];
                    vBodyPos[1] = lpContactPoint->m_lpCP->m_localPointA[1];
                    vBodyPos[2] = lpContactPoint->m_lpCP->m_localPointA[2];
                }
                else
                {
                    vBodyPos[0] = lpContactPoint->m_lpCP->m_localPointB[0];
                    vBodyPos[1] = lpContactPoint->m_lpCP->m_localPointB[1];
                    vBodyPos[2] = lpContactPoint->m_lpCP->m_localPointB[2];
                }

			    fltForceMag = lpContactPoint->m_lpCP->m_appliedImpulse * (fMassUnits * fDisUnits);

			    if(fltForceMag > 0)
				    lpSensor->ProcessContact(vBodyPos, fltForceMag);
            }
		}

    }

    //Clear the contact points for the next simulation step.
    m_aryContactPoints.Clear();
}

void BlRigidBody::Physics_CollectData()
{
	float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();
    btVector3 vData;

	if(m_osgbMotion)
	{
		//Update the world matrix for this part
        btTransform trans;
        m_osgbMotion->getWorldTransform(trans);
        m_osgWorldMatrix = osgbCollision::asOsgMatrix(trans);

		//Then update the absolute position and rotation.
        btVector3 vPos = trans.getOrigin();
		m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);

        CStdFPoint vRot = OsgMatrixUtil::EulerRotationFromMatrix_Static(m_osgWorldMatrix);
		m_lpThisMI->ReportRotation(vRot[0], vRot[1], vRot[2]);

        OsgWorldCoordinateNodeVisitor* ncv = new OsgWorldCoordinateNodeVisitor();
        m_osgMT->accept(*ncv);
        osg::Matrix mat = ncv->MatrixTransform();
    }
	else 
    {
        if(m_btCollisionObject)
            m_btCollisionObject->setWorldTransform( osgbCollision::asBtTransform( GetOSGWorldMatrix(true) ) );

		//If we are here then we did not have a physics component, just and OSG one.
		Physics_UpdateAbsolutePosition();

		//TODO: Get Rotation
		//m_lpThis->ReportRotation(QuaterionToEuler(m_osgLocalMatrix.getRotate());
	}

	if(m_bCollectExtraData && m_btPart)
	{
		vData = m_btPart->getLinearVelocity();
		m_vLinearVelocity[0] = vData[0] * fDisUnits;
		m_vLinearVelocity[1] = vData[1] * fDisUnits;
		m_vLinearVelocity[2] = vData[2] * fDisUnits;

		vData = m_btPart->getAngularVelocity();
		m_vAngularVelocity[0] = vData[0];
		m_vAngularVelocity[1] = vData[1];
		m_vAngularVelocity[2] = vData[2];

		vData = m_btPart->getTotalForce();
        float fltRatio = fMassUnits * fDisUnits;
		m_vForce[0] = vData[0] * fltRatio;
		m_vForce[1] = vData[1] * fltRatio;
		m_vForce[2] = vData[2] * fltRatio;

		vData = m_btPart->getTotalTorque();
        fltRatio = fMassUnits * fDisUnits * fDisUnits;
		m_vTorque[0] = vData[0] * fltRatio;
		m_vTorque[1] = vData[1] * fltRatio;
		m_vTorque[2] = vData[2] * fltRatio;

        //FIX PHYSICS
		//Vx::VxReal3 vAccel;
		//m_vxPart->getLinearAcceleration(vAccel);
		//m_vLinearAcceleration[0] = vAccel[0] * fDisUnits;
		//m_vLinearAcceleration[1] = vAccel[1] * fDisUnits;
		//m_vLinearAcceleration[2] = vAccel[2] * fDisUnits;

		//m_vxPart->getAngularAcceleration(vAccel);
		//m_vAngularAcceleration[0] = vAccel[0];
		//m_vAngularAcceleration[1] = vAccel[1];
		//m_vAngularAcceleration[2] = vAccel[2];
	}

	if(m_lpThisRB->GetContactSensor() || m_lpThisRB->IsContactSensor()) 
		ProcessContacts();
}

void BlRigidBody::Physics_ResetSimulation()
{
	OsgRigidBody::Physics_ResetSimulation();

    //FIX PHYSICS
	//if(m_vxSensor)
	//{
	//	//Reset the dynamics of the part and make it match the new scenegraph position
	//	m_vxSensor->updateFromNode();

	//	if(m_vxPart)
	//	{
	//		m_vxPart->resetDynamics();
	//		m_vxPart->wakeDynamics();
	//	}
	//}

	for(int i=0; i<3; i++)
	{
		m_vTorque[i] = 0;
		m_vForce[i] = 0;
		m_vLinearVelocity[i] = 0;
		m_vAngularVelocity[i] = 0;
		m_vLinearAcceleration[i] = 0;
		m_vAngularAcceleration[i] = 0;
	}
}

void BlRigidBody::Physics_EnableCollision(RigidBody *lpBody)
{
    if(m_lpBulletData && m_lpThisRB->GetExclusionCollisionSet()->size() == 0)
        m_lpBulletData->m_bExclusionProcessing = false;
}

void BlRigidBody::Physics_DisableCollision(RigidBody *lpBody)
{
    if(m_lpBulletData && m_lpThisRB->GetExclusionCollisionSet()->size() > 0)
        m_lpBulletData->m_bExclusionProcessing = true;
}

void BlRigidBody::Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits)
{
	if(m_btPart && (fltFx || fltFy || fltFz) && !m_lpThisRB->Freeze())
	{
		btVector3 fltF, fltP;
		if(bScaleUnits)
		{
			fltF[0] = fltFx * (m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());
			fltF[1] = fltFy * (m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());
			fltF[2] = fltFz * (m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());
		}
		else
		{
			fltF[0] = fltFx;
			fltF[1] = fltFy;
			fltF[2] = fltFz;
		}

		fltP[0] = fltPx;
		fltP[1] = fltPy;
		fltP[2] = fltPz;

        m_btPart->applyForce(fltF, fltP);
	}
}

void BlRigidBody::Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits)
{
	if(m_btPart && (fltTx || fltTy || fltTz))
	{
		btVector3 fltT;
		if(bScaleUnits)
		{
			fltT[0] = fltTx * (m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());
			fltT[1] = fltTy * (m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());
			fltT[2] = fltTz * (m_lpThisAB->GetSimulator()->InverseMassUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits() * m_lpThisAB->GetSimulator()->InverseDistanceUnits());
		}
		else
		{
			fltT[0] = fltTx;
			fltT[1] = fltTy;
			fltT[2] = fltTz;
		}

		m_btPart->applyTorque(fltT);  
	}
}

CStdFPoint BlRigidBody::Physics_GetVelocityAtPoint(float x, float y, float z)
{
	CStdFPoint linVel(0,0,0);
    //FIX PHYSICS
	//Vx::VxReal3 vxLinVel = {0,0,0};
	//Vx::VxReal3 vxPoint = {x,y,z};

	//if this is a contact sensor then return nothing.
	//if(m_vxPart)
	//{
	//	m_vxPart->getVelocityAtPoint(vxPoint,  vxLinVel);
	//	linVel.Set(vxLinVel[0], vxLinVel[1], vxLinVel[2]);
	//}

	return linVel;
}	

float BlRigidBody::Physics_GetMass()
{
	float fltMass = 0;

    //If thi spart is frozen then we will get mass values of 0. So we need to
    //temporarilly unfreeze it to get the mass and volume, then refreeze it.
    //FIX PHYSICS
 //   if(m_lpThisRB && m_vxSensor && m_lpThisRB->Freeze())
 //       m_vxSensor->freeze(false);

	//if(m_vxPart)
	//	fltMass = m_vxPart->getMass();

 //   if(m_lpThisRB && m_vxSensor && m_lpThisRB->Freeze())
 //       m_vxSensor->freeze(true);

	return fltMass;
}

bool BlRigidBody::Physics_HasCollisionGeometry()
{
	if(m_btCollisionShape)
		return true;
	else
		return false;
}

	}			// Environment
}				//BulletAnimatSim
