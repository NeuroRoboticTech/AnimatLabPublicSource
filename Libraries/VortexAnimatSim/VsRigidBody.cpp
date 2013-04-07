// VsRigidBody.cpp: implementation of the VsRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsRigidBody::VsRigidBody()
{
	m_bCollectExtraData = FALSE;
	m_vxSensor = NULL;
	m_vxPart = NULL;
	m_vxGeometry = NULL;
	m_vxCollisionGeometry = NULL;

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

VsRigidBody::~VsRigidBody()
{

try
{
	//int i= 5;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsRigidBody\r\n", "", -1, FALSE, TRUE);}
}

void VsRigidBody::SetThisPointers()
{
	VsBody::SetThisPointers();
	m_lpThisRB = dynamic_cast<RigidBody *>(this);
	if(!m_lpThisRB)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisRB, " + m_lpThisAB->Name());

	m_lpThisRB->PhysicsBody(this);
}

Vx::VxCollisionSensor* VsRigidBody::Sensor()
{
	return m_vxSensor;
}

Vx::VxPart* VsRigidBody::Part()
{
	return m_vxPart;
}

Vx::VxNode VsRigidBody::GraphicsNode()
{
	return m_vxGraphicNode;
}

Vx::VxCollisionGeometry *VsRigidBody::CollisionGeometry()
{
	return m_vxCollisionGeometry;
}

void VsRigidBody::CollisionGeometry(Vx::VxCollisionGeometry *vxGeometry)
{
	m_vxCollisionGeometry = vxGeometry;
	Physics_FluidDataChanged();
}

CStdFPoint VsRigidBody::Physics_GetCurrentPosition()
{
	if(m_vxSensor && m_lpThisMI)
	{
		Vx::VxReal3Ptr vPos = NULL;
		m_vxSensor->getPosition(vPos);
		m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
		return m_lpThisMI->AbsolutePosition();
	}
	else
	{
		CStdFPoint vPos;
		return vPos;
	}
}

void VsRigidBody::Physics_UpdateMatrix()
{
	VsBody::Physics_UpdateMatrix();

	if(m_lpThisRB)
		m_lpThisRB->UpdatePhysicsPosFromGraphics();
}
 
void VsRigidBody::UpdatePositionAndRotationFromMatrix()
{
	VsBody::UpdatePositionAndRotationFromMatrix();

	if(m_lpThisRB)
		m_lpThisRB->UpdatePhysicsPosFromGraphics();
}

void VsRigidBody::Physics_UpdateNode()
{
	if(m_lpThisRB)
	{
		if(m_lpThisRB->IsContactSensor())
			ResetSensorCollisionGeom();
		else if(m_lpThisRB->HasStaticJoint())
			ResetStaticCollisionGeom(); //If this body uses a static joint then we need to reset the offest matrix for its collision geometry.
		else if(m_vxSensor)
			m_vxSensor->updateFromNode();
	}

	Physics_UpdateAbsolutePosition();
}

void VsRigidBody::Physics_SetColor()
{
	if(m_lpThisRB)
		SetColor(*m_lpThisRB->Ambient(), *m_lpThisRB->Diffuse(), *m_lpThisRB->Specular(), m_lpThisRB->Shininess()); 
}

void VsRigidBody::Physics_TextureChanged()
{
	if(m_lpThisRB)
		SetTexture(m_lpThisRB->Texture());
}

void VsRigidBody::Physics_SetFreeze(BOOL bVal)
{
	if(m_vxSensor)
		m_vxSensor->freeze(bVal);
}

void VsRigidBody::Physics_SetDensity(float fltVal)
{
	if(m_vxSensor)
		m_vxCollisionGeometry->setRelativeDensity(fltVal);
}

void VsRigidBody::Physics_SetMaterialID(string strID)
{
	if(m_vxCollisionGeometry && m_lpThisAB)
	{
		int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(strID);
	
		if(iMaterialID >= 0)
			m_vxCollisionGeometry->setMaterialID(iMaterialID);
	}
}

void VsRigidBody::Physics_SetVelocityDamping(float fltLinear, float fltAngular)
{
	if(m_vxPart && fltLinear > 0)
		m_vxPart->setLinearVelocityDamping(fltLinear);

	if(m_vxPart && fltAngular > 0)
		m_vxPart->setAngularVelocityDamping(fltAngular);
}

void VsRigidBody::Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz)
{
	if(m_vxPart)
	{
		if(fltTx != 0 || fltTy != 0 || fltTz != 0)
			m_vxPart->setCOMOffset(fltTx, fltTy, fltTz);
	}
}

void VsRigidBody::Physics_Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = NULL;
		if(m_osgGeometryRotationMT.valid())
			osgGroup = dynamic_cast<osg::Geode *>(m_osgGeometryRotationMT->getChild(0));
		else
			osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());

		if(!osgGroup)
			THROW_TEXT_ERROR(Vs_Err_lNodeNotGeode, Vs_Err_strNodeNotGeode, m_lpThisAB->Name());

		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
			osgGroup->removeDrawable(m_osgGeometry.get());

		m_osgGeometry.release();

		//Create a new box geometry with the new sizes.
		CreateGraphicsGeometry();
		if(m_lpThisAB)
			m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

		//Add it to the geode.
		osgGroup->addDrawable(m_osgGeometry.get());

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		if(m_osgNodeGroup.valid())
		{
			osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
			osgVisitor->traverse(*m_osgNodeGroup);
		}
	}

	if(m_vxGeometry)
	{
		ResizePhysicsGeometry();

		//We need to reset the density in order for it to recompute the mass and volume.
		if(m_lpThisRB)
			Physics_SetDensity(m_lpThisRB->Density());

		//Now get base values, including mass and volume
		GetBaseValues();
	}
}

void VsRigidBody::Physics_SelectedVertex(float fltXPos, float fltYPos, float fltZPos)
{
	if(m_lpThisRB->IsCollisionObject() && m_osgSelVertexMT.valid())
	{
		osg::Matrix osgMT;
		osgMT.makeIdentity();
		osgMT = osgMT.translate(fltXPos, fltYPos, fltZPos);

		m_osgSelVertexMT->setMatrix(osgMT);
	}
}


void VsRigidBody::ShowSelectedVertex()
{
	if(m_lpThisRB && m_lpThisAB && m_lpThisRB->IsCollisionObject() && m_lpThisAB->Selected() && m_osgMT.valid() && m_osgSelVertexMT.valid())
	{
		if(!m_osgMT->containsNode(m_osgSelVertexMT.get()))
			m_osgMT->addChild(m_osgSelVertexMT.get());
	}
}

void VsRigidBody::HideSelectedVertex()
{
	if(m_lpThisRB->IsCollisionObject() && m_osgMT.valid() && m_osgSelVertexMT.valid())
	{
		if(m_osgMT->containsNode(m_osgSelVertexMT.get()))
			m_osgMT->removeChild(m_osgSelVertexMT.get());
	}
}

void VsRigidBody::Physics_ResizeSelectedReceptiveFieldVertex()
{
	DeleteSelectedVertex();
	CreateSelectedVertex(m_lpThisAB->Name());
}

void  VsRigidBody::Physics_FluidDataChanged()
{
	if(m_vxCollisionGeometry && m_lpThisRB)
	{
		CStdFPoint vpCenter = m_lpThisRB->BuoyancyCenter();
		Vx::VxReal3 vCenter = {vpCenter.x, vpCenter.y, vpCenter.z};

		CStdFPoint vpDrag = m_lpThisRB->Drag();
		Vx::VxReal3 vDrag = {vpDrag.x, vpDrag.y, vpDrag.z};

		float fltScale = m_lpThisRB->BuoyancyScale();
		float fltMagnus = m_lpThisRB->Magnus();
		BOOL bEnabled = m_lpThisRB->EnableFluids();

		m_vxCollisionGeometry->setFluidInteractionData(vCenter, vDrag, fltScale, fltMagnus);

		if(bEnabled)
			m_vxCollisionGeometry->setDefaultFluidInteractionForceFn();
		else
			m_vxCollisionGeometry->setFluidInteractionForceFn(NULL);
	}
}

void VsRigidBody::GetBaseValues()
{
	if(m_vxPart && m_lpThisRB)
	{
		//Fluid Density is in the units being used. So if the user set 1 g/cm^3 
		//and the units were grams and decimeters then density would be 1000 g/dm^3

		//Recalculate the mass and volume
		m_lpThisRB->GetMass();
		m_lpThisRB->GetVolume();

		//m_fltBuoyancy = -(m_lpThisAB->GetSimulator()->FluidDensity() * fltVolume * m_lpThisAB->GetSimulator()->Gravity());
		//m_fltReportBuoyancy = m_fltBuoyancy * m_lpThisAB->GetSimulator()->MassUnits() * m_lpThisAB->GetSimulator()->DistanceUnits();
	}
}

void VsRigidBody::Initialize()
{
	//GetBaseValues();
}

/**
\brief	Builds the local matrix.

\details If this is the root object then use the world coordinates of the structure instead of the
local coordinates of the rigid body.

\author	dcofer
\date	5/11/2011
**/
void  VsRigidBody::BuildLocalMatrix()
{
	//build the local matrix
	if(m_lpThisRB && m_lpThisMI && m_lpThisAB)
	{
		if(m_lpThisRB->IsRoot())
			VsBody::BuildLocalMatrix(m_lpThisAB->GetStructure()->AbsolutePosition(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
		else
			VsBody::BuildLocalMatrix(m_lpThisMI->Position(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
	}
}

/**
\brief	Gets the parent osg node.

\details If this is the root object then attach it directly to the OSG root node because we are building its local matrix
using the structures absolute position. This is so the vortex stuff works correctly, and so that you can move the structure
by moving the root body. 

\author	dcofer
\date	5/11/2011

\return	Pointer to the OSG group node of the parent.
**/
osg::Group *VsRigidBody::ParentOSG()
{
	VsMovableItem *lpItem = NULL;

	if(!m_lpThisRB->IsRoot() && m_lpParentVsMI)
		return m_lpParentVsMI->GetMatrixTransform();
	else
		return GetVsSimulator()->OSGRoot();
}

void VsRigidBody::SetupPhysics()
{
	//If no geometry is defined then this part does not have a physics representation.
	//it is purely an osg node attached to other parts. An example of this is an attachment point or a sensor.
	if(m_vxGeometry && m_lpThisRB)
	{
		//If the parent is not null and the joint is null then that means we need to statically link this part to 
		//its parent. So we do not create a physics part, we just get a link to its parents part.
		if(m_lpThisRB->IsContactSensor())
			CreateSensorPart();
		else if(m_lpThisRB->HasStaticJoint())
			CreateStaticPart();
		else
			CreateDynamicPart();
	}
}

void VsRigidBody::CreateSensorPart()
{
	if(m_lpThisRB && m_lpThisAB)
	{
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThisRB->Parent());

		m_vxSensor = new VxCollisionSensor;
		m_vxPart = NULL;
		m_vxSensor->setUserData((void*) m_lpThisRB);

		m_vxSensor->setName(m_lpThisAB->ID().c_str());               // Give it a name.
		CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, 0));
		string strName = m_lpThisAB->ID() + "_CollisionGeometry";
		m_vxCollisionGeometry->setName(strName.c_str());

		m_vxSensor->setControl(VxEntity::kControlNode);

		//For some reason attempting to disable collisions between a sensor and its parent part does not work.
		//I put some code in the checking to prevent this from being counted.

		if(lpVsParent)
			SetFollowEntity(lpVsParent);
		else
			m_vxSensor->freeze(m_lpThisRB->Freeze());
	}
}

void VsRigidBody::SetFollowEntity(VsRigidBody *lpEntity)
{
	if(m_vxSensor)
	{
		m_vxSensor->setFastMoving(true);
		Vx::VxReal44 vOffset;
		VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);
		Vx::VxTransform vTM(vOffset);

		Vx::VxCollisionSensor *vxSensor = lpEntity->Sensor();
		if(vxSensor)
			m_vxSensor->followEntity(vxSensor, true, &vTM);
	}
}

void VsRigidBody::ResetSensorCollisionGeom()
{
	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThisRB->Parent());

	if(lpVsParent)
		SetFollowEntity(lpVsParent);
}


void VsRigidBody::CreateDynamicPart()
{
	if(m_lpThisRB && m_lpThisAB)
	{
		// Create the physics object.
		m_vxPart = new VxPart;
		m_vxSensor = m_vxPart;
		m_vxSensor->setUserData((void*) m_lpThisRB);
		int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(m_lpThisRB->MaterialID());

		m_vxSensor->setName(m_lpThisAB->ID().c_str());               // Give it a name.
		m_vxSensor->setControl(m_eControlType);  // Set it to dynamic.
		CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisRB->Density()));

        GetBaseValues();

		string strName = m_lpThisAB->ID() + "_CollisionGeometry";
		m_vxCollisionGeometry->setName(strName.c_str());
		m_vxSensor->setFastMoving(true);

		//if this body is frozen; freeze it
		m_vxSensor->freeze(m_lpThisRB->Freeze());

		//if the center of mass isn't the graphical center then set the offset relative to position and orientation
		if(m_vxPart)
		{
			CStdFPoint vCOM = m_lpThisRB->CenterOfMass();
			if(vCOM.x != 0 || vCOM.y != 0 || vCOM.z != 0)
				m_vxPart->setCOMOffset(vCOM.x, vCOM.y, vCOM.z);

			if(m_lpThisRB->LinearVelocityDamping() > 0)
				m_vxPart->setLinearVelocityDamping(m_lpThisRB->LinearVelocityDamping());

			if(m_lpThisRB->AngularVelocityDamping() > 0)
				m_vxPart->setAngularVelocityDamping(m_lpThisRB->AngularVelocityDamping());
		}
	}
}

void VsRigidBody::CreateStaticPart()
{
	if(m_lpThisRB && m_lpThisAB)
	{
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThisRB->Parent());

		Vx::VxReal44 vOffset;
		VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);
		int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(m_lpThisRB->MaterialID());

		if(lpVsParent)
		{
			Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
			if(vxSensor)
			{
				CollisionGeometry(vxSensor->addGeometry(m_vxGeometry, iMaterialID, vOffset, m_lpThisRB->Density()));
				string strName = m_lpThisAB->ID() + "_CollisionGeometry";
				m_vxCollisionGeometry->setName(strName.c_str());
			}
		}
	}
}

void VsRigidBody::RemoveStaticPart()
{
	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThisRB->Parent());

	if(lpVsParent)
	{
		Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
		if(vxSensor && m_vxCollisionGeometry)
			vxSensor->removeCollisionGeometry(m_vxCollisionGeometry);
	}
}

void VsRigidBody::ResetStaticCollisionGeom()
{
	if(m_osgMT.valid() && m_lpThisRB && m_lpThisRB->Parent())
	{
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThisRB->Parent());

		if(lpVsParent)
		{
			Vx::VxReal44 vOffset;
			VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);

			Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
			if(vxSensor && m_vxCollisionGeometry)
				m_vxCollisionGeometry->setRelativeTransform(vOffset);
		}
	}
}

void VsRigidBody::DeletePhysics()
{
	if(m_vxSensor)
	{
		if(GetVsSimulator() && GetVsSimulator()->Universe())
		{
			GetVsSimulator()->Universe()->removeEntity(m_vxSensor);
			delete m_vxSensor;
		}

		m_vxSensor = NULL;
		m_vxPart = NULL;
		m_vxGeometry = NULL;
	}
	else if(m_lpThisRB->HasStaticJoint())
		RemoveStaticPart();
}

void VsRigidBody::SetBody()
{
	if(m_vxSensor && m_lpThisAB)
	{
		//VxAssembly *lpAssem = (VxAssembly *) lpStructure->Assembly();
		VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());

		osg::MatrixTransform *lpMT = dynamic_cast<osg::MatrixTransform *>(m_osgMT.get());
		m_vxSensor->setNode(lpMT);               // Connect to the node.

		// Add the part to the universe.
		if(lpVsSim && lpVsSim->Universe())
			lpVsSim->Universe()->addEntity(m_vxSensor);
		//lpAssem->addEntity(m_vxSensor);
	}
}

int VsRigidBody::GetPartIndex(VxPart *vxP0, VxPart *vxP1)
{
	if(m_vxPart == vxP1)
		return 1;
	else 
		return 0;
}

void VsRigidBody::ProcessContacts()
{
	ContactSensor *lpSensor = m_lpThisRB->ContactSensor();
	float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();

	if(m_vxPart && lpSensor)
	{
		lpSensor->ClearCurrents();

		Vx::VxPart::DynamicsContactIterator itd = m_vxPart->dynamicsContactBegin();
        VxPart *p[2];
		int iPartIdx=0;
		VxReal3 vWorldPos;
		StdVector3 vBodyPos;
		VxReal3 vForce;
		float fltForceMag = 0;

		//if(m_lpThisRB->GetSimulator()->TimeSlice() == 550 || m_lpThisRB->GetSimulator()->TimeSlice() == 9550 || m_lpThisRB->GetSimulator()->TimeSlice() == 10550)
		//	fltForceMag = 0;

		int iCount=0;
		for(; itd != m_vxPart->dynamicsContactEnd(); ++itd, iCount++)
		{
			VxDynamicsContact *vxDyn = *itd;
			vxDyn->getPartPair(p, p+1);

			vxDyn->getPosition(vWorldPos);
			WorldToBodyCoords(vWorldPos, vBodyPos);

			iPartIdx = GetPartIndex(p[0], p[1]);
			vxDyn->getForce(iPartIdx, vForce);
			fltForceMag = V3_MAG(vForce) * (fMassUnits * fDisUnits);

			if(fltForceMag > 0)
				lpSensor->ProcessContact(vBodyPos, fltForceMag);
		}
	}
}

void VsRigidBody::Physics_CollectData()
{
	float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();
	Vx::VxReal3 vData;

	if(m_vxSensor)
	{
		UpdateWorldMatrix();

		//Update the world matrix for this part
		Vx::VxReal44 vxMT;
		m_vxSensor->getTransform(vxMT);
		VxOSG::copyVxReal44_to_OsgMatrix(m_osgWorldMatrix, vxMT);

		//Then update the absolute position and rotation.
		m_vxSensor->getPosition(vData);
		m_lpThisMI->AbsolutePosition(vData[0], vData[1], vData[2]);

		m_vxSensor->getOrientationEulerAngles(vData);
		m_lpThisMI->ReportRotation(vData[0], vData[1], vData[2]);
	}
	else
	{
		//If we are here then we did not have a physics component, just and OSG one.
		Physics_UpdateAbsolutePosition();

		//TODO: Get Rotation
		//m_lpThis->ReportRotation(QuaterionToEuler(m_osgLocalMatrix.getRotate());
	}

	if(m_bCollectExtraData && m_vxPart)
	{
		m_vxPart->getLinearVelocity(vData);
		m_vLinearVelocity[0] = vData[0] * fDisUnits;
		m_vLinearVelocity[1] = vData[1] * fDisUnits;
		m_vLinearVelocity[2] = vData[2] * fDisUnits;

		m_vxPart->getAngularVelocity(vData);
		m_vAngularVelocity[0] = vData[0];
		m_vAngularVelocity[1] = vData[1];
		m_vAngularVelocity[2] = vData[2];

		Vx::VxReal3 vData2;
		m_vxPart->getLastForceAndTorque(vData, vData2);
		m_vForce[0] = vData[0] * fMassUnits * fDisUnits;
		m_vForce[1] = vData[1] * fMassUnits * fDisUnits;
		m_vForce[2] = vData[2] * fMassUnits * fDisUnits;

		m_vTorque[0] = vData2[0] * fMassUnits * fDisUnits * fDisUnits;
		m_vTorque[1] = vData2[1] * fMassUnits * fDisUnits * fDisUnits;
		m_vTorque[2] = vData2[2] * fMassUnits * fDisUnits * fDisUnits;

		Vx::VxReal3 vAccel;
		m_vxPart->getLinearAcceleration(vAccel);
		m_vLinearAcceleration[0] = vAccel[0] * fDisUnits;
		m_vLinearAcceleration[1] = vAccel[1] * fDisUnits;
		m_vLinearAcceleration[2] = vAccel[2] * fDisUnits;

		m_vxPart->getAngularAcceleration(vAccel);
		m_vAngularAcceleration[0] = vAccel[0];
		m_vAngularAcceleration[1] = vAccel[1];
		m_vAngularAcceleration[2] = vAccel[2];
	}

	if(m_lpThisRB->ContactSensor()) 
		ProcessContacts();
}

void VsRigidBody::Physics_ResetSimulation()
{
	VsBody::Physics_ResetSimulation();

	if(m_vxSensor)
	{
		//Reset the dynamics of the part and make it match the new scenegraph position
		m_vxSensor->updateFromNode();

		if(m_vxPart)
		{
			m_vxPart->resetDynamics();
			m_vxPart->wakeDynamics();
		}
	}

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

void VsRigidBody::Physics_EnableCollision(RigidBody *lpBody)
{
	if(!lpBody)
		THROW_ERROR(Al_Err_lBodyNotDefined, Al_Err_strBodyNotDefined);

	VsRigidBody *lpVsBody = dynamic_cast<VsRigidBody *>(lpBody);

	if(!lpVsBody)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastBodyToDesiredType, Al_Err_strUnableToCastBodyToDesiredType, "Type", "VsRigidBody");

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());

	if(!lpVsSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	//If collisions between the two objects is enabled then disable it.
	if(lpVsSim->Universe()->getPairIntersectEnabled(m_vxCollisionGeometry, lpVsBody->CollisionGeometry()) == false)
		lpVsSim->Universe()->enablePairIntersect(m_vxPart, lpVsBody->Sensor());
}

void VsRigidBody::Physics_DisableCollision(RigidBody *lpBody)
{
	if(!lpBody)
		THROW_ERROR(Al_Err_lBodyNotDefined, Al_Err_strBodyNotDefined);

	VsRigidBody *lpVsBody = dynamic_cast<VsRigidBody *>(lpBody);

	if(!lpVsBody)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastBodyToDesiredType, Al_Err_strUnableToCastBodyToDesiredType, "Type", "VsRigidBody");

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());

	if(!lpVsSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	//If collisions between the two objects is enabled then disable it.
	Vx::VxUniverse *lpUniv = lpVsSim->Universe();
	if(!m_vxCollisionGeometry || !lpVsBody->CollisionGeometry())
		THROW_PARAM_ERROR(Vs_Err_lCollisionGeomNotDefined, Vs_Err_strCollisionGeomNotDefined, "ID", m_lpThisAB->ID());

	if(lpUniv->getPairIntersectEnabled(m_vxCollisionGeometry, lpVsBody->CollisionGeometry()) == true)
		lpUniv->disablePairIntersect(m_vxSensor, lpVsBody->Sensor());
}

float *VsRigidBody::Physics_GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);
	RigidBody *lpBody = dynamic_cast<RigidBody *>(this);

	if(strType == "BODYTORQUEX")
		{m_bCollectExtraData = TRUE; return (&m_vTorque[0]);}

	if(strType == "BODYTORQUEY")
		{m_bCollectExtraData = TRUE; return (&m_vTorque[1]);}

	if(strType == "BODYTORQUEZ")
		{m_bCollectExtraData = TRUE; return (&m_vTorque[2]);}

	if(strType == "BODYFORCEX")
		{m_bCollectExtraData = TRUE; return (&m_vForce[0]);}

	if(strType == "BODYFORCEY")
		{m_bCollectExtraData = TRUE; return (&m_vForce[1]);}

	if(strType == "BODYFORCEZ")
		{m_bCollectExtraData = TRUE; return (&m_vForce[2]);}

	if(strType == "BODYLINEARVELOCITYX")
		{m_bCollectExtraData = TRUE; return (&m_vLinearVelocity[0]);}

	if(strType == "BODYLINEARVELOCITYY")
		{m_bCollectExtraData = TRUE; return (&m_vLinearVelocity[1]);}

	if(strType == "BODYLINEARVELOCITYZ")
		{m_bCollectExtraData = TRUE; return (&m_vLinearVelocity[2]);}

	if(strType == "BODYANGULARVELOCITYX")
		{m_bCollectExtraData = TRUE; return (&m_vAngularVelocity[0]);}

	if(strType == "BODYANGULARVELOCITYY")
		{m_bCollectExtraData = TRUE; return (&m_vAngularVelocity[1]);}

	if(strType == "BODYANGULARVELOCITYZ")
		{m_bCollectExtraData = TRUE; return (&m_vAngularVelocity[2]);}

	//if(strType == "BODYBUOYANCY")
	//	{m_bCollectExtraData = TRUE; return (&m_fltReportBuoyancy);}

	if(strType == "BODYLINEARACCELERATIONX")
		{m_bCollectExtraData = TRUE; return (&m_vLinearAcceleration[0]);}

	if(strType == "BODYLINEARACCELERATIONY")
		{m_bCollectExtraData = TRUE; return (&m_vLinearAcceleration[1]);}

	if(strType == "BODYLINEARACCELERATIONZ")
		return (&m_vLinearAcceleration[2]);

	if(strType == "BODYANGULARACCELERATIONX")
		{m_bCollectExtraData = TRUE; return (&m_vAngularAcceleration[0]);}

	if(strType == "BODYANGULARACCELERATIONY")
		{m_bCollectExtraData = TRUE; return (&m_vAngularAcceleration[1]);}

	if(strType == "BODYANGULARACCELERATIONZ")
		{m_bCollectExtraData = TRUE; return (&m_vAngularAcceleration[2]);}

	return NULL;
}

void VsRigidBody::Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
{
	if(m_vxPart && (fltFx || fltFy || fltFz) && !m_lpThisRB->Freeze())
	{
		VxReal3 fltF, fltP;
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

		m_vxPart->addForceAtPosition(fltF, fltP);
	}
}

void VsRigidBody::Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits)
{
	if(m_vxPart && (fltTx || fltTy || fltTz))
	{
		VxReal3 fltT;
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

		m_vxPart->addTorque(fltT);  
	}
}

CStdFPoint VsRigidBody::Physics_GetVelocityAtPoint(float x, float y, float z)
{
	CStdFPoint linVel(0,0,0);
	Vx::VxReal3 vxLinVel = {0,0,0};
	Vx::VxReal3 vxPoint = {x,y,z};

	//if this is a contact sensor then return nothing.
	if(m_vxPart)
	{
		m_vxPart->getVelocityAtPoint(vxPoint,  vxLinVel);
		linVel.Set(vxLinVel[0], vxLinVel[1], vxLinVel[2]);
	}

	return linVel;
}	

float VsRigidBody::Physics_GetMass()
{
	float fltMass = 0;

    //If thi spart is frozen then we will get mass values of 0. So we need to
    //temporarilly unfreeze it to get the mass and volume, then refreeze it.
	if(m_vxPart && m_vxSensor)
	{
		if(m_lpThisRB->Freeze())
			m_vxSensor->freeze(false);

		fltMass = m_vxPart->getMass();

		if(m_lpThisRB->Freeze())
			m_vxSensor->freeze(true);
	}

	return fltMass;
}

BOOL VsRigidBody::Physics_HasCollisionGeometry()
{
	if(m_vxSensor)
		return TRUE;
	else
		return FALSE;
}

	}			// Environment
}				//VortexAnimatSim
