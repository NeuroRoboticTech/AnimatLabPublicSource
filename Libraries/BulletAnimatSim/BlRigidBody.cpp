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
    m_eBodyType = BOX_SHAPE_PROXYTYPE;

    m_fltBuoyancy = 0;
    m_fltReportBuoyancy = 0;

    for(int iIdx=0; iIdx<3; iIdx++)
    {
        m_vLinearDragForce[iIdx] = 0;
        m_vAngularDragTorque[iIdx] = 0;
    }

	m_lpMaterial = NULL;
    m_lpVsSim = NULL;

	m_btStickyLock = NULL;
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
        m_osgWorldMatrix = m_osgMT->getMatrix();

		//Then update the absolute position and rotation.
        osg::Vec3d vPos = m_osgWorldMatrix.getTrans();
        m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
		return m_lpThisMI->AbsolutePosition();
	}
	else
	{
		CStdFPoint vPos;
		return vPos;
	}
}

osg::Matrix BlRigidBody::GetPhysicsWorldMatrix()
{
    if(m_osgbMotion)
    {
        btTransform trans;
        m_osgbMotion->getWorldTransform(trans);
        osg::Matrix osgMT = osgbCollision::asOsgMatrix(trans);
        return osgMT;
    }
    else
    {
	    osg::Matrix osgMatrix;
	    osgMatrix.makeIdentity();
	    return osgMatrix;
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
	////Test code
	//int i=5;
	//if(Std_ToLower(m_lpThisAB->ID()) == "b42f968f-0639-4a69-9974-9e0f411d40d8") // && m_lpSim->Time() > 1.8
	//	i=6;

	OsgBody::UpdatePositionAndRotationFromMatrix();

	if(m_lpThisRB)
	{
		if(m_lpThisRB->IsContactSensor())
			ResetSensorCollisionGeom();
		else if(m_lpThisRB->HasStaticJoint())
			ResetStaticCollisionGeom(); //If this body uses a static joint then we need to reset the offest matrix for its collision geometry.
        else if(m_btPart)
			ResetDynamicCollisionGeom();
	}
}

void BlRigidBody::Physics_SetFreeze(bool bVal)
{
    ResizePhysicsGeometry();
}

void BlRigidBody::Physics_SetMass(float fltVal)
{
    ResizePhysicsGeometry();
}

float BlRigidBody::Physics_GetMass()
{
    if(m_lpThisRB)
        return m_lpThisRB->Mass();

	return 0;
}

float BlRigidBody::Physics_GetDensity()
{
    if(m_lpThisRB)
    {
        float fltVolume = m_lpThisRB->Volume();
        if(fltVolume > 0)
            return m_lpThisRB->Mass()/fltVolume;
    }

    return 0;
}

void BlRigidBody::Physics_SetMaterialID(std::string strID)
{
	//First remove this rigid body from any previously associated material.
	if(m_lpMaterial)
	{
		m_lpMaterial->RemoveRigidBodyAssociation(m_lpThisRB);
		m_lpMaterial = NULL;
	}

	m_lpMaterial = dynamic_cast<BlMaterialType *>(m_lpThisAB->GetSimulator()->FindByID(strID));

	if(!m_lpMaterial)
		THROW_PARAM_ERROR(Bl_Err_lConvertingMaterialType, Bl_Err_strConvertingMaterialType, "ID", strID);

	m_lpMaterial->AddRigidBodyAssociation(m_lpThisRB);

	MaterialTypeModified();
}

void BlRigidBody::MaterialTypeModified()
{
	if(m_btPart && m_lpMaterial)
	{
		m_btPart->setFriction(m_lpMaterial->FrictionLinearPrimary());
        m_btPart->setRollingFriction(m_lpMaterial->FrictionAngularPrimaryConverted());
		m_btPart->setRestitution(m_lpMaterial->Restitution());
	}
}

void BlRigidBody::Physics_SetVelocityDamping(float fltLinear, float fltAngular)
{
	if(m_btPart && fltLinear > 0 && fltAngular > 0)
        m_btPart->setDamping(fltLinear, fltAngular);
}

void BlRigidBody::Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz)
{
    ResetPhyiscsAndChildJoints();
}

void  BlRigidBody::Physics_FluidDataChanged()
{
}

void BlRigidBody::Physics_WakeDynamics()
{
    if(m_btPart)
    {
        int istate = m_btPart->getActivationState();

        if(m_lpThisRB->Freeze())
            m_btPart->setActivationState(0);
        else if(!m_btPart->isActive())
            m_btPart->activate(true);
    }
}

void BlRigidBody::GetBaseValues()
{
	if(m_lpThisRB)
	{
		//Recalculate the mass and volume
		m_lpThisRB->GetDensity();
		m_lpThisRB->GetVolume();
	}
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

        int iFlags = m_btCollisionObject->getCollisionFlags() | btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK | btCollisionObject::CF_NO_CONTACT_RESPONSE;
        m_btCollisionObject->setCollisionFlags(iFlags);

        //Disable collisions between this sensor part and its parent manually. For some reason this is not
        //done automatically by bullet like it is for two dynamic parts.
        m_lpThisRB->DisableCollision(m_lpThisRB->Parent());
    }
}

void BlRigidBody::CreateDynamicPart()
{
	if(m_lpThisRB && m_lpThisAB && m_btCollisionShape && m_lpMaterial)
    {
        BlSimulator *lpSim = GetBlSimulator();

        float fltMass = 0;
	    CStdFPoint vCom = m_lpThisRB->CenterOfMassWithStaticChildren();

		////Test Code
  //      if(Std_ToLower(m_lpThisRB->ID()) == "b42f968f-0639-4a69-9974-9e0f411d40d8")
  //          fltMass = 0;

        if(m_lpThisRB->HasStaticChildren())
            CreateStaticChildren(vCom);
        else if(vCom != CStdFPoint(0, 0, 0))
            SetupOffsetCOM(vCom);

        osg::ref_ptr< osgbDynamics::CreationRecord > cr = new osgbDynamics::CreationRecord;

        //We should always set the COM even if it is zero. This is to prevent the OsgBullet code from trying to approximate it using bounding boxes.
        //I want the COM to be where I say it is.
        cr->setCenterOfMass(osg::Vec3(vCom.x, vCom.y, vCom.z) );
        cr->_sceneGraph = m_osgMT.get();
        cr->_shapeType = m_eBodyType;
        cr->_parentTransform = m_osgMT->getMatrix();
  
        if(!m_lpThisRB->Freeze())
            cr->_mass = m_lpThisRB->GetMassValueWithStaticChildren();
        else
            cr->_mass = 0;

		cr->_friction = m_lpMaterial->FrictionLinearPrimary();
        cr->_rollingFriction = m_lpMaterial->FrictionAngularPrimaryConverted();
		cr->_restitution = m_lpMaterial->Restitution();
        cr->_linearDamping = m_lpThisRB->LinearVelocityDamping();
        cr->_angularDamping = m_lpThisRB->AngularVelocityDamping();

        m_osgMT->setMatrix(osg::Matrix::identity());

        m_btPart = osgbDynamics::createRigidBody( cr.get(), m_btCollisionShape );

        if(m_lpMaterial->FrictionAngularPrimary() > 0)
    		m_btPart->setAnisotropicFriction(m_btCollisionShape->getAnisotropicRollingFrictionDirection(),btCollisionObject::CF_ANISOTROPIC_ROLLING_FRICTION);

        m_btPart->setLinearVelocity( btVector3( 0., 0, 0. ) );
        m_btPart->setAngularVelocity( btVector3( 0., 0, 0. ) );

        m_osgbMotion = dynamic_cast<osgbDynamics::MotionState *>(m_btPart->getMotionState());
        m_btCollisionShape = m_btPart->getCollisionShape();

        if(!m_lpBulletData)
            m_lpBulletData = new BlBulletData(this, false);

        m_btPart->setUserPointer((void *) m_lpBulletData);

	    //if this body is frozen; freeze it
        Physics_WakeDynamics();

        //If this part has a receptive field then turn on the custom callbacks.
        if(m_lpThisRB->GetContactSensor())
        {
            int iFlags = m_btPart->getCollisionFlags() | btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK;
            m_btPart->setCollisionFlags(iFlags);
        }

        lpSim->DynamicsWorld()->addRigidBody( m_btPart, AnimatCollisionTypes::RIGID_BODY, ALL_COLLISIONS );
           
        if(m_lpThisRB->DisplayDebugCollisionGraphic())
        {
            m_osgDebugNode = osgbCollision::osgNodeFromBtCollisionShape( m_btCollisionShape );
            m_osgDebugNode->setName(m_lpThisRB->Name() + "_Debug");
	        m_osgNodeGroup->addChild(m_osgDebugNode.get());	
        }

        GetBaseValues();
        CalculateRotatedAreas();
    }
}

/**
 \brief Changes this body to use a btCompoundShape and adds the btCollision shape offset from -COM.

 \description The reason it does this is because by default bullet always has the center of mass at the center of the
 object. However, if the user has specified a different COM then we need to move it away from that. The way to do ths is
 create a new compound shape and add our actual collision shape offset in the negative com direction. Then the motion state
 will match up this new collision part with the graphics correctly.

 \author    David Cofer
 \date  11/17/2013

 \param vCom    The com vector that will be used for the offset. Do not add a negative to it. This will be done in the method.
 */
void BlRigidBody::SetupOffsetCOM(const CStdFPoint &vCom)
{
    //Swap the current collision shape into the compound collision shape so we can
    //make the collision shape a compound shape.
    btCollisionShape *btChildShape = m_btCollisionShape;
    m_aryCompoundChildShapes.Add(m_btCollisionShape);

    //Now create the new compound shape
    m_btCompoundShape = new btCompoundShape();

    //Set the collision shape to be the new compound.
    m_btCollisionShape = m_btCompoundShape;

    //Now add the shape for this body to the compound shape
	btTransform localTransform;
    localTransform.setIdentity();
    localTransform.setOrigin(btVector3(-vCom.x, -vCom.y, -vCom.z));
    m_btCompoundShape->addChildShape(localTransform, btChildShape);
}

void BlRigidBody::CreateStaticChildren(const CStdFPoint &vCom)
{
    //Swap the current collision shape into the compound collision shape so we can
    //make the collision shape a compound shape.
    btCollisionShape *btChildShape = m_btCollisionShape;
    m_aryCompoundChildShapes.Add(m_btCollisionShape);

    //Now create the new compound shape
    m_btCompoundShape = new btCompoundShape();

    //Set the collision shape to be the new compound.
    m_btCollisionShape = m_btCompoundShape;

    //Now add the shape for this body to the compound shape
	btTransform localTransform;
    localTransform.setIdentity();
    localTransform.setOrigin(btVector3(-vCom.x, -vCom.y, -vCom.z));
    m_btCompoundShape->addChildShape(localTransform, btChildShape);

    //Then add all static child shapes
	int iCount = m_lpThisRB->ChildParts()->GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
    {
        RigidBody *lpChild = m_lpThisRB->ChildParts()->at(iIndex);
        BlRigidBody *lpBlChild = dynamic_cast<BlRigidBody *>(lpChild);
		if(lpChild->HasStaticJoint())
            lpBlChild->AddStaticGeometry(this, m_btCompoundShape, vCom);
    }
}

void BlRigidBody::AddStaticGeometry(BlRigidBody *lpChild, btCompoundShape *btCompound, const CStdFPoint &vCom)
{
    if(m_lpThisMI && lpChild && btCompound)
    {
       //First create the physics geometry for this part.
       CreateGeometry(true);
       //CreatePhysicsGeometry(); 

		CStdFPoint vPos = m_lpThisMI->Position() - vCom;
		CStdFPoint vRot = m_lpThisMI->Rotation();
		osg::Matrix mt = SetupMatrix(vPos, vRot);

	    btTransform localTransform  = osgbCollision::asBtTransform(mt);
        btCompound->addChildShape(localTransform, m_btCollisionShape);

        GetBaseValues();
    }
}

void BlRigidBody::RemoveStaticGeometry(BlRigidBody *lpChild, btCompoundShape *btCompound)
{
    if(m_lpThisMI && lpChild && btCompound)
    {
       //First create the physics geometry for this part.
        btCompound->removeChildShape(m_btCollisionShape);
        DeleteCollisionGeometry();
    }
}

void BlRigidBody::ResetStaticCollisionGeom()
{
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
        //We must take into consideration the COM when resetting the world transform.
        osg::Vec3d vCom = m_osgbMotion->getCenterOfMass();
        osg::Matrixd osgMtCom = osg::Matrixd::translate(vCom);
        osg::Matrixd osgMTBase = m_osgMT->getMatrix();
        osg::Matrixd osgMTmat = osgMTBase * osgMtCom;

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
    //If we have a compound shape defined then collision shape = compound shape. So only delete once.
    if(m_btCompoundShape)
    {
        int iCount = m_btCompoundShape->getNumChildShapes();
        for(int iIdx=0; iIdx<iCount; iIdx++)
            m_btCompoundShape->removeChildShapeByIndex(0);

        m_aryCompoundChildShapes.RemoveAll();

        delete m_btCompoundShape;
        m_btCompoundShape = NULL;
        m_btCollisionShape = NULL;
    }
    else if(m_btCollisionShape)
        {delete m_btCollisionShape; m_btCollisionShape = NULL;}
}

void BlRigidBody::DeletePhysics(bool bIncludeChildren)
{
	if(m_btPart || m_btCollisionObject)
	{
        //First delete all physics associated with this part.
        //We need to delete all the constrains attached to this part, and then the part itself.
        DeleteAttachedJointPhysics();

        //Then delete this part
        DeleteDynamicPart();
        DeleteSensorPart();

        //Then delete collision geometry.
        DeleteCollisionGeometry();

        if(bIncludeChildren)
            DeleteChildPhysics();

		Physics_DeleteStickyLock();
	}
}

void BlRigidBody::DeleteChildPhysics()
{
    int iCount = m_lpThisRB->ChildParts()->GetSize();
    for(int iIdx=0; iIdx<iCount; iIdx++)
    {
        RigidBody *lpChild = m_lpThisRB->ChildParts()->at(iIdx);
        if(lpChild)
        {
        	OsgRigidBody *lpVsChild = dynamic_cast<OsgRigidBody *>(lpChild);
            if(lpVsChild)
                lpVsChild->DeletePhysics(true);
        }
    }
}

void BlRigidBody::Physics_DeleteStickyLock()
{
	if(m_btStickyLock)
	{
		if(GetBlSimulator() && GetBlSimulator()->DynamicsWorld())
			GetBlSimulator()->DynamicsWorld()->removeConstraint(m_btStickyLock);

		delete m_btStickyLock;
		m_btStickyLock = NULL;
	}
}

void BlRigidBody::DeleteAttachedJointPhysics()
{
	BlJoint *lpVsJoint = dynamic_cast<BlJoint *>(m_lpThisRB->JointToParent());

    if(lpVsJoint)
        lpVsJoint->DeletePhysics(false);

    int iCount = m_lpThisRB->ChildParts()->GetSize();
    for(int iIdx=0; iIdx<iCount; iIdx++)
    {
        RigidBody *lpChild = m_lpThisRB->ChildParts()->at(iIdx);
        if(lpChild)
        {
        	BlJoint *lpVsChildJoint = dynamic_cast<BlJoint *>(lpChild->JointToParent());
            if(lpVsChildJoint)
                lpVsChildJoint->DeletePhysics(false);
        }
    }
}

void BlRigidBody::RecreateAttachedJointPhysics()
{
	BlJoint *lpVsJoint = dynamic_cast<BlJoint *>(m_lpThisRB->JointToParent());

    if(lpVsJoint)
        lpVsJoint->SetupPhysics();

    int iCount = m_lpThisRB->ChildParts()->GetSize();
    for(int iIdx=0; iIdx<iCount; iIdx++)
    {
        RigidBody *lpChild = m_lpThisRB->ChildParts()->at(iIdx);
        if(lpChild)
        {
        	BlJoint *lpVsChildJoint = dynamic_cast<BlJoint *>(lpChild->JointToParent());
            if(lpVsChildJoint)
                lpVsChildJoint->SetupPhysics();
        }
    }
}

void BlRigidBody::ResizePhysicsGeometry()
{
    if(m_lpThisRB && m_lpThisRB->HasStaticJoint() && m_btCollisionShape)
    {
        //If this is a static part then we actually need to call the resize on the parent so it
        //is all recalculated correctly.
        // Only attempt to do this if a collision shape is already present for it to resize.
        BlRigidBody *lpOsgParent = dynamic_cast<BlRigidBody *>(m_lpThisRB->Parent());
        if(lpOsgParent)
            lpOsgParent->ResizePhysicsGeometry();
    }
    else
    {
        //Then delete the physics for this part
        DeletePhysics(false);

        //Now recreate the collision geometry for this part.
        CreatePhysicsGeometry();

        //Now recreate the part itself.
        SetupPhysics();

        //Then recreate all the attached joints.
        RecreateAttachedJointPhysics();
    }
}

/**
 \brief Rotates the axis area values by the amount that this part is rotated by. This is used by the hydrodynamics to 
 calculate the drag. To do that we need to know what the surface area is in the direction of movement. We calculate the
 area for each axis in world coordinates, but the part can be rotated at will, so we need to do the same rotation to 
 find the actual area in world axis coordinates to use.

 \author    David Cofer
 \date  10/20/2013
 */
void BlRigidBody::CalculateRotatedAreas()
{
    //Get the world matrix for this part and then reset its translation to 0 so we only have rotations.
    osg::Matrix rotMT = GetWorldMatrix();
    rotMT.setTrans(osg::Vec3d(0, 0, 0));

    osg::Vec4d vArea(m_vArea.x, m_vArea.y, m_vArea.z, 0);
    osg::Vec4d vRotatedArea = rotMT * vArea;

    m_vRotatedArea.Set(fabs(vRotatedArea[0]), fabs(vRotatedArea[1]), fabs(vRotatedArea[2]));
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

void BlRigidBody::Physics_ChildBodyAdded(RigidBody *lpChild)
{
    if(lpChild && lpChild->HasStaticJoint())
        ResizePhysicsGeometry();
}

void BlRigidBody::Physics_ChildBodyRemoved(bool bHasStaticJoint)
{
    if(bHasStaticJoint)
    ResizePhysicsGeometry();
}

void BlRigidBody::CreateStickyLock()
{
	BlRigidBody *lpParent = dynamic_cast<BlRigidBody *>(m_lpThisRB->Parent());
	BlRigidBody *lpChild = m_aryContactPoints[0]->m_lpContacted;

	if(lpChild && lpParent)
	{
		osg::Matrix mtParent = lpParent->GetWorldMatrix();
		osg::Matrix mtChild = lpChild->GetWorldMatrix();

		osg::Matrix mtLocalRelToParent = mtChild * osg::Matrix::inverse(mtParent);

		btTransform mtJointRelToParent = osgbCollision::asBtTransform(mtLocalRelToParent);
		btTransform mtJointRelToChild = osgbCollision::asBtTransform(osg::Matrixd::identity());

		m_btStickyLock = new btAnimatGeneric6DofConstraint(*lpParent->Part(), *lpChild->Part(), mtJointRelToParent, mtJointRelToChild, false); 

		//Lock the linear and angular limits to prevent movement
		btVector3 vLowerLimit, vUpperLimit;
		vLowerLimit[0] = vLowerLimit[1] = vLowerLimit[2] = 0;
		vUpperLimit[0] = vUpperLimit[1] = vUpperLimit[2] = 0;

		m_btStickyLock->setLinearLowerLimit(vLowerLimit);
		m_btStickyLock->setLinearUpperLimit(vUpperLimit);
		m_btStickyLock->setAngularLowerLimit(vLowerLimit);
		m_btStickyLock->setAngularUpperLimit(vUpperLimit);
					
		GetBlSimulator()->DynamicsWorld()->addConstraint(m_btStickyLock, true);
	}

}

void BlRigidBody::SetSurfaceContactCount()
{
	if(m_lpThisRB)
	{
		m_lpThisRB->SetSurfaceContactCount(m_aryContactPoints.size());

		//if(m_lpThisRB->ID() == "4672039D-2716-4276-A7C2-CB83D2697673" && m_aryContactPoints.size() > 0 && 
		//	m_aryContactPoints[0]->m_lpContacted && m_aryContactPoints[0]->m_lpContacted->m_lpThisRB->ID() == "3AC540B5-F0D6-41D4-ABD6-0ECA575AC204")
		//if(m_lpThisRB->ID() == "18165913-2BF6-457C-A501-1DC40EE887F9" && m_aryContactPoints.size() > 0 && 
		//	m_aryContactPoints[0]->m_lpContacted && m_aryContactPoints[0]->m_lpContacted->m_lpThisRB->ID() == "71FE27FB-121A-4007-AAFA-2771EE816558")
		if(m_lpThisRB->IsStickyPart() &&  m_aryContactPoints.size() > 0 && m_aryContactPoints[0]->m_lpContacted && m_lpThisRB->Parent())
		{
			//If we have a lock and the sticky on value goes below 1 then disable it by removing the lock
			if(m_btStickyLock && m_lpThisRB->StickyOn() < 1)
				Physics_DeleteStickyLock();
			//If we do not have a lock and sticky on goes to 1 or above then create a new lock.
			else if(!m_btStickyLock && m_lpThisRB->StickyOn() >= 1)
				CreateStickyLock();
		}
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
        SetSurfaceContactCount(); //m_lpThisRB->SetSurfaceContactCount(m_aryContactPoints.size());
    else if(m_aryContactPoints.size() > 0 && m_btPart && lpSensor)
    {
		int iPartIdx=0;
		StdVector3 vBodyPos;
		float fltForceMag = 0;
	    float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	    float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();
        float fltPhysicsDt = m_lpThisAB->GetSimulator()->PhysicsTimeStep();
        float fltRatio = (fMassUnits * fDisUnits) / fltPhysicsDt;

		////Test code
		//if(m_lpThisRB->GetSimulator()->Time() >= 6.758)
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

			    fltForceMag = lpContactPoint->m_lpCP->m_appliedImpulse * fltRatio;

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
    OsgSimulator *lpSim = GetOsgSimulator();
    btVector3 vData;

	if(m_osgbMotion)
	{
		//Update the world matrix for this part
        btTransform trans;
        m_osgbMotion->getWorldTransform(trans);
        m_osgWorldMatrix = osgbCollision::asOsgMatrix(trans);

		//Then update the absolute position and rotation.
        btVector3 vPos = trans.getOrigin();
        osg::Vec3 vCom = m_osgbMotion->getCenterOfMass();

		float fltX = (vPos.x()-vCom.x());
		float fltY = (vPos.y()-vCom.y());
		float fltZ = (vPos.z()-vCom.z());

 		m_lpThisMI->AbsolutePosition(fltX, fltY, fltZ);

		m_osgWorldMatrix(3,0) = fltX;
		m_osgWorldMatrix(3,1) = fltY;
		m_osgWorldMatrix(3,2) = fltZ;

        CStdFPoint vRot = OsgMatrixUtil::EulerRotationFromMatrix_Static(m_osgWorldMatrix);
		m_lpThisMI->ReportRotation(vRot[0], vRot[1], vRot[2]);
    }
	else 
    {
        if(m_btCollisionObject)
            m_btCollisionObject->setWorldTransform( osgbCollision::asBtTransform( GetOSGWorldMatrix(true) ) );

		//If we are here then we did not have a physics component, just and OSG one.
		Physics_UpdateAbsolutePosition();

        CStdFPoint vRot = OsgMatrixUtil::EulerRotationFromMatrix_Static(m_osgWorldMatrix);
		m_lpThisMI->ReportRotation(vRot[0], vRot[1], vRot[2]);
	}


	if(m_lpThisRB->GetContactSensor() || m_lpThisRB->IsContactSensor()) 
		ProcessContacts();
}

void BlRigidBody::Physics_CollectExtraData()
{
	float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();
    OsgSimulator *lpSim = GetOsgSimulator();
    btVector3 vData;

	if(m_btPart && lpSim)
	{
        float m_vPrevLinearVelocity[3];
        float m_vPrevAngularVelocity[3];

        //Store off the previoius linear and angular velocities for calculation of accelerations
        m_vPrevLinearVelocity[0] = m_vLinearVelocity[0];
        m_vPrevLinearVelocity[1] = m_vLinearVelocity[1];
        m_vPrevLinearVelocity[2] = m_vLinearVelocity[2];

        m_vPrevAngularVelocity[0] = m_vAngularVelocity[0];
        m_vPrevAngularVelocity[1] = m_vAngularVelocity[1];
        m_vPrevAngularVelocity[2] = m_vAngularVelocity[2];

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

        float fltDt = lpSim->PhysicsTimeStep();

        if(fltDt > 0)
        {
            //Bullet has no methods to directly get these accelerations, so we need to calculate them ourselves
		    m_vLinearAcceleration[0] = (m_vLinearVelocity[0] - m_vPrevLinearVelocity[0])/fltDt;
		    m_vLinearAcceleration[1] = (m_vLinearVelocity[1] - m_vPrevLinearVelocity[1])/fltDt;
		    m_vLinearAcceleration[2] = (m_vLinearVelocity[2] - m_vPrevLinearVelocity[2])/fltDt;

		    m_vAngularAcceleration[0] = (m_vAngularVelocity[0] - m_vPrevAngularVelocity[0])/fltDt;
		    m_vAngularAcceleration[1] = (m_vAngularVelocity[1] - m_vPrevAngularVelocity[1])/fltDt;
		    m_vAngularAcceleration[2] = (m_vAngularVelocity[2] - m_vPrevAngularVelocity[2])/fltDt;
        }
	}
}

void BlRigidBody::Physics_ResetSimulation()
{
	OsgRigidBody::Physics_ResetSimulation();

    if(m_btPart)
    {
        m_btPart->clearForces();
        btVector3 zeroVector(0,0,0);
        m_btPart->setLinearVelocity(zeroVector);
        m_btPart->setAngularVelocity(zeroVector);
    }

    Physics_WakeDynamics();

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

void BlRigidBody::Physics_AddBodyForceAtLocalPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits)
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

        Physics_WakeDynamics();
        m_btPart->applyForce(fltF, fltP);
	}
}

void BlRigidBody::Physics_AddBodyForceAtWorldPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits)
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

        //Bullet force application position is relative to the center of mass of the part.
        btVector3 vCOM = m_btPart->getCenterOfMassPosition();

		fltP[0] = fltPx - vCOM[0];
		fltP[1] = fltPy - vCOM[1];
		fltP[2] = fltPz - vCOM[2];

        Physics_WakeDynamics();
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
	btVector3 vLinVel(0,0,0);
	btVector3 vPoint(x,y,z);

	//if this is a contact sensor then return nothing.
	if(m_btPart)
	{
		vLinVel = m_btPart->getVelocityInLocalPoint(vPoint);
		linVel.Set(vLinVel[0], vLinVel[1], vLinVel[2]);
	}
    
	return linVel;
}	

bool BlRigidBody::Physics_HasCollisionGeometry()
{
	if(m_btCollisionShape)
		return true;
	else
		return false;
}

void BlRigidBody::Physics_StepHydrodynamicSimulation()
{
    Simulator *lpSim = GetSimulator();
    BlSimulator *lpBlSim = GetBlSimulator();
    if(m_btPart && lpSim && lpBlSim && m_lpThisRB && !m_lpThisRB->Freeze())
    {
        float fltDepth = m_lpThisRB->AbsolutePosition().y;
        FluidPlane *lpPlane = lpBlSim->FindFluidPlaneForDepth(fltDepth);

        if(lpPlane)
        {
            m_fltBuoyancy = -(lpPlane->Density() * m_lpThisRB->Volume() * lpSim->Gravity() * m_lpThisRB->BuoyancyScale());
            m_fltReportBuoyancy = m_fltBuoyancy * lpSim->MassUnits() * lpSim->DistanceUnits();

		    btVector3 vbtLinVel = m_btPart->getLinearVelocity();
		    btVector3 vbtAngVel = m_btPart->getAngularVelocity();

            CStdFPoint vLinearVel(vbtLinVel[0], vbtLinVel[1], vbtLinVel[2]);
            CStdFPoint vAngularVel(vbtAngVel[0], vbtAngVel[1], vbtAngVel[2]);
            CStdFPoint vSignLinear(Std_Sign(vbtLinVel[0], 1), Std_Sign(vbtLinVel[1], 1), Std_Sign(vbtLinVel[2], 1));
            CStdFPoint vSignAngular(Std_Sign(vAngularVel[0], 1), Std_Sign(vAngularVel[1], 1), Std_Sign(vAngularVel[2], 1));

            CalculateRotatedAreas();

            CStdFPoint vLinearDragForce = (m_lpThisRB->LinearDrag() * m_vRotatedArea * vLinearVel * vLinearVel * vSignLinear) * (lpPlane->Density() * -0.5);
            CStdFPoint vAngularDragTorque = (m_lpThisRB->AngularDrag() * m_vRotatedArea * vAngularVel * vAngularVel * vSignAngular) * (lpPlane->Density() * -0.5);
            float fltMaxForce = m_lpThisRB->MaxHydroForce();
            float fltMaxTorque = m_lpThisRB->MaxHydroTorque();
            for(int i=0; i<3; i++)
            {
                m_vLinearDragForce[i] = vLinearDragForce[i] * lpSim->MassUnits() * lpSim->DistanceUnits();
                m_vAngularDragTorque[i] = vAngularDragTorque[i] * lpSim->MassUnits() * lpSim->DistanceUnits() * lpSim->DistanceUnits();

                if(vLinearDragForce[i] > fltMaxForce)
                    vLinearDragForce[i] = fltMaxForce;

                if(vLinearDragForce[i] < -fltMaxForce)
                    vLinearDragForce[i] = -fltMaxForce;

                if(m_vAngularDragTorque[i] > fltMaxTorque)
                    vAngularDragTorque[i] = fltMaxTorque;

                if(m_vAngularDragTorque[i] < -fltMaxTorque)
                    vAngularDragTorque[i] = -fltMaxTorque;
            }

	        Physics_WakeDynamics();
			m_btPart->applyCentralForce(btVector3(vLinearDragForce.x, (vLinearDragForce.y+m_fltBuoyancy),  vLinearDragForce.z));
			m_btPart->applyTorque(btVector3(vAngularDragTorque.x, vAngularDragTorque.y, vAngularDragTorque.z));
        }
        else
        {
            m_fltBuoyancy = 0;
            m_fltReportBuoyancy = 0;
            for(int iIdx=0; iIdx<3; iIdx++)
            {
                m_vLinearDragForce[iIdx] = 0;
                m_vAngularDragTorque[iIdx] = 0;
            }
        }
    }

}

float *BlRigidBody::Physics_GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);
	RigidBody *lpBody = dynamic_cast<RigidBody *>(this);

	if(strType == "BODYBUOYANCY")
		return (&m_fltReportBuoyancy);

	if(strType == "BODYDRAGFORCEX")
		return (&m_vLinearDragForce[0]);

	if(strType == "BODYDRAGFORCEY")
		return (&m_vLinearDragForce[1]);

    if(strType == "BODYDRAGFORCEZ")
		return (&m_vLinearDragForce[2]);

	if(strType == "BODYDRAGTORQUEX")
		return (&m_vAngularDragTorque[0]);

	if(strType == "BODYDRAGTORQUEY")
		return (&m_vAngularDragTorque[1]);

    if(strType == "BODYDRAGTORQUEZ")
		return (&m_vAngularDragTorque[2]);

	return OsgRigidBody::Physics_GetDataPointer(strDataType);
}

	}			// Environment
}				//BulletAnimatSim
