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
    m_btCollisionShape = NULL;
    m_btPart = NULL;
    m_osgbMotion = NULL;

    m_lpVsSim = NULL;
}

BlRigidBody::~BlRigidBody()
{

try
{
    if(m_btCollisionShape)
        {delete m_btCollisionShape; m_btCollisionShape = NULL;}

    if(m_osgbMotion)
        {delete m_osgbMotion; m_osgbMotion = NULL;}

    if(m_btPart)
        {delete m_btPart; m_btPart = NULL;}
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

//FIX PHYSICS
//Vx::VxEntity::EntityControlTypeEnum BlRigidBody::ConvertControlType()
//{
//    if(m_eControlType == ControlAnimated)
//        return Vx::VxEntity::EntityControlTypeEnum::kControlAnimated;
//    else if(m_eControlType == ControlNode)
//        return Vx::VxEntity::EntityControlTypeEnum::kControlNode;
//    else if(m_eControlType == ControlDynamic)
//        return Vx::VxEntity::EntityControlTypeEnum::kControlDynamic;
//    else
//        return Vx::VxEntity::EntityControlTypeEnum::kControlStatic;
//}

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
        //FIX PHYSICS
   //     else if(m_vxSensor)
			//m_vxSensor->updateFromNode();
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
	if(m_lpThisRB && m_lpThisAB)
	{
		BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpThisRB->Parent());

        //FIX PHYSICS
		//m_vxSensor = new VxCollisionSensor;
		//m_vxPart = NULL;
		//m_vxSensor->setUserData((void*) m_lpThisRB);

		//m_vxSensor->setName(m_lpThisAB->ID().c_str());               // Give it a name.
		//CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, 0));
		//string strName = m_lpThisAB->ID() + "_CollisionGeometry";
		//m_vxCollisionGeometry->setName(strName.c_str());

		//m_vxSensor->setControl(VxEntity::kControlNode);

		////For some reason attempting to disable collisions between a sensor and its parent part does not work.
		////I put some code in the checking to prevent this from being counted.

		//if(lpVsParent)
		//	SetFollowEntity(lpVsParent);
		//else
		//	m_vxSensor->freeze(m_lpThisRB->Freeze());
	}
}

void BlRigidBody::SetFollowEntity(OsgRigidBody *lpEntity)
{
    //FIX PHYSICS
	//if(m_vxSensor)
	//{
 //       BlRigidBody *lpVsEntity = dynamic_cast<BlRigidBody *>(lpEntity);

 //       if(lpVsEntity)
 //       {
	//	    m_vxSensor->setFastMoving(true);
	//	    Vx::VxReal44 vOffset;
	//	    VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);
	//	    Vx::VxTransform vTM(vOffset);

	//	    Vx::VxCollisionSensor *vxSensor = lpVsEntity->Sensor();
	//	    if(vxSensor)
	//		    m_vxSensor->followEntity(vxSensor, true, &vTM);
 //       }
	//}
}

void BlRigidBody::CreateDynamicPart()
{
    BlSimulator *lpSim = GetBlSimulator();

	if(lpSim && m_lpThisRB && m_lpThisAB && m_btCollisionShape)
	{
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
        m_btPart->setUserPointer((void *) m_lpThisRB);

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

        lpSim->DynamicsWorld()->addRigidBody( m_btPart );

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

void BlRigidBody::CreateStaticPart()
{
    //Parts are created the same way, but the mass should be 0 in this case.
    CreateDynamicPart();
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

void BlRigidBody::SetupGraphics()
{
	m_osgParent = ParentOSG();

	if(m_osgParent.valid())
	{
		BuildLocalMatrix();

		SetColor(*m_lpThisMI->Ambient(), *m_lpThisMI->Diffuse(), *m_lpThisMI->Specular(), m_lpThisMI->Shininess());
		SetTexture(m_lpThisMI->Texture());
		SetCulling();
		SetVisible(m_lpThisMI->IsVisible());

		//Add it to the scene graph.
        GetBlSimulator()->OSGRoot()->addChild(m_osgRoot.get());
		//m_osgParent->addChild(m_osgRoot.get());

		//Set the position with the world coordinates.
		Physics_UpdateAbsolutePosition();

		//We need to set the UserData on the OSG side so we can do picking.
		//We need to use a node visitor to set the user data for all drawable nodes in all geodes for the group.
		osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void BlRigidBody::UpdateWorldMatrix()
{
    if(m_osgMT.valid())
    	m_osgWorldMatrix = m_osgMT->getMatrix();
}

void  BlRigidBody::BuildLocalMatrix()
{
	//build the local matrix
	if(m_lpThisRB && m_lpThisMI && m_lpThisAB)
	{
		if(m_lpThisRB->IsRoot())
			BlRigidBody::BuildLocalMatrix(m_lpThisAB->GetStructure()->AbsolutePosition(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
		else
			BlRigidBody::BuildLocalMatrix(m_lpThisMI->Position(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
	}
}

void BlRigidBody::BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName)
{
	if(!m_osgMT.valid())
	{
		m_osgMT = new osgManipulator::Selection;
		m_osgMT->setName(strName + "_MT");
	}

	if(!m_osgRoot.valid())
	{
		m_osgRoot = new osg::Group;
		m_osgRoot->setName(strName + "_Root");
	}

	if(!m_osgRoot->containsNode(m_osgMT.get()))
		m_osgRoot->addChild(m_osgMT.get());

	osg::Matrix mtGlobal = m_osgParent->getMatrix() * SetupMatrix(localPos, localRot);


	LocalMatrix(mtGlobal);

	//set the matrix to the matrix transform node
	m_osgMT->setMatrix(m_osgLocalMatrix);	

	//First create the node group. The reason for this is so that we can add other decorated groups on to this node.
	//This is used to add the selected overlays.
	if(!m_osgNodeGroup.valid() && m_osgNode.valid())
	{
		m_osgNodeGroup = new osg::Group();
		m_osgNodeGroup->addChild(m_osgNode.get());		
		m_osgNodeGroup->setName(strName + "_NodeGroup");
		
 		m_osgMT->addChild(m_osgNodeGroup.get());
	
		CreateSelectedGraphics(strName);
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

void BlRigidBody::DeletePhysics()
{
    //FIX PHYSICS
	//if(m_vxSensor)
	//{
	//	if(GetBlSimulator() && GetBlSimulator()->Universe())
	//	{
	//		GetBlSimulator()->Universe()->removeEntity(m_vxSensor);
	//		delete m_vxSensor;
	//	}

	//	m_vxSensor = NULL;
	//	m_vxPart = NULL;
	//	m_vxGeometry = NULL;
	//}
	//else if(m_lpThisRB && m_lpThisRB->HasStaticJoint())
	//	RemoveStaticPart();
}

void BlRigidBody::SetBody()
{
    //FIX PHYSICS
	//if(m_vxSensor && m_lpThisAB)
	//{
	//	BlSimulator *lpVsSim = dynamic_cast<BlSimulator *>(m_lpThisAB->GetSimulator());

	//	osg::MatrixTransform *lpMT = dynamic_cast<osg::MatrixTransform *>(m_osgMT.get());
	//	m_vxSensor->setNode(lpMT);               // Connect to the node.

	//	// Add the part to the universe.
	//	if(lpVsSim && lpVsSim->Universe())
	//		lpVsSim->Universe()->addEntity(m_vxSensor);
	//}
}

//FIX PHYSICS
//int BlRigidBody::GetPartIndex(VxPart *vxP0, VxPart *vxP1)
//{
//	if(m_vxPart == vxP1)
//		return 1;
//	else 
//		return 0;
//}

void BlRigidBody::ProcessContacts()
{
	ContactSensor *lpSensor = m_lpThisRB->GetContactSensor();
	float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();

    //FIX PHYSICS
	//if(m_vxPart && lpSensor)
	//{
	//	lpSensor->ClearCurrents();

	//	Vx::VxPart::DynamicsContactIterator itd = m_vxPart->dynamicsContactBegin();
 //       VxPart *p[2];
	//	int iPartIdx=0;
	//	VxReal3 vWorldPos;
	//	StdVector3 vBodyPos;
	//	VxReal3 vForce;
	//	float fltForceMag = 0;

	//	//if(m_lpThisRB->GetSimulator()->TimeSlice() == 550 || m_lpThisRB->GetSimulator()->TimeSlice() == 9550 || m_lpThisRB->GetSimulator()->TimeSlice() == 10550)
	//	//	fltForceMag = 0;

	//	int iCount=0;
	//	for(; itd != m_vxPart->dynamicsContactEnd(); ++itd, iCount++)
	//	{
	//		VxDynamicsContact *vxDyn = *itd;
	//		vxDyn->getPartPair(p, p+1);

	//		vxDyn->getPosition(vWorldPos);
	//		WorldToBodyCoords(vWorldPos, vBodyPos);

	//		iPartIdx = GetPartIndex(p[0], p[1]);
	//		vxDyn->getForce(iPartIdx, vForce);
	//		fltForceMag = V3_MAG(vForce) * (fMassUnits * fDisUnits);

	//		if(fltForceMag > 0)
	//			lpSensor->ProcessContact(vBodyPos, fltForceMag);
	//	}
	//}
}

//FIX PHYSICS
//void BlRigidBody::WorldToBodyCoords(VxReal3 vWorld, StdVector3 &vLocalPos)
//{
//	osg::Vec3f vWorldPos;
//	osg::Vec3f vLocal;
//
//	vLocalPos[0] = vWorld[0]; vLocalPos[1] = vWorld[1]; vLocalPos[2] = vWorld[2];
//	vWorldPos[0] = vWorld[0]; vWorldPos[1] = vWorld[1]; vWorldPos[2] = vWorld[2];
//
//	if(m_osgNode.valid())
//	{
//	  osg::NodePathList paths = m_osgNode->getParentalNodePaths(); 
//	  osg::Matrix worldToLocal = osg::computeWorldToLocal(paths.at(0)); 
//	  vLocal = vWorldPos * worldToLocal;
//	}
//
//	vLocalPos[0] = vLocal[0]; vLocalPos[1] = vLocal[1]; vLocalPos[2] = vLocal[2];
//} 

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

	if(m_lpThisRB->GetContactSensor()) 
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
	if(!lpBody)
		THROW_ERROR(Al_Err_lBodyNotDefined, Al_Err_strBodyNotDefined);

	BlRigidBody *lpVsBody = dynamic_cast<BlRigidBody *>(lpBody);

	if(!lpVsBody)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastBodyToDesiredType, Al_Err_strUnableToCastBodyToDesiredType, "Type", "BlRigidBody");

	BlSimulator *lpVsSim = dynamic_cast<BlSimulator *>(m_lpThisAB->GetSimulator());

	if(!lpVsSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	//If collisions between the two objects is enabled then disable it.
    //FIX PHYSICS
	//if(lpVsSim->Universe()->getPairIntersectEnabled(m_vxCollisionGeometry, lpVsBody->CollisionGeometry()) == false)
	//	lpVsSim->Universe()->enablePairIntersect(m_vxPart, lpVsBody->Sensor());
}

void BlRigidBody::Physics_DisableCollision(RigidBody *lpBody)
{
	if(!lpBody)
		THROW_ERROR(Al_Err_lBodyNotDefined, Al_Err_strBodyNotDefined);

	BlRigidBody *lpVsBody = dynamic_cast<BlRigidBody *>(lpBody);

	if(!lpVsBody)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastBodyToDesiredType, Al_Err_strUnableToCastBodyToDesiredType, "Type", "BlRigidBody");

	BlSimulator *lpVsSim = dynamic_cast<BlSimulator *>(m_lpThisAB->GetSimulator());

	if(!lpVsSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	//If collisions between the two objects is enabled then disable it.
    //FIX PHYSICS
	//Vx::VxUniverse *lpUniv = lpVsSim->Universe();
	//if(!m_vxCollisionGeometry || !lpVsBody->CollisionGeometry())
	//	THROW_PARAM_ERROR(Bl_Err_lCollisionGeomNotDefined, Bl_Err_strCollisionGeomNotDefined, "ID", m_lpThisAB->ID());

	//if(lpUniv->getPairIntersectEnabled(m_vxCollisionGeometry, lpVsBody->CollisionGeometry()) == true)
	//	lpUniv->disablePairIntersect(m_vxSensor, lpVsBody->Sensor());
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
