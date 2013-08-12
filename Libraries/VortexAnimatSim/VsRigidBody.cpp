// VsRigidBody.cpp: implementation of the VsRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsRigidBody::VsRigidBody()
{
	m_vxSensor = NULL;
	m_vxPart = NULL;
	m_vxGeometry = NULL;
	m_vxCollisionGeometry = NULL;
    m_lpVsSim = NULL;
}

VsRigidBody::~VsRigidBody()
{

try
{
	//int i= 5;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsRigidBody\r\n", "", -1, false, true);}
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

bool VsRigidBody::Physics_IsDefined()
{
    if(m_vxSensor && m_vxGeometry)
        return true;
    else
        return false;
}

bool VsRigidBody::Physics_IsGeometryDefined()
{
    if(m_vxGeometry)
        return true;
    else
        return false;
}

Vx::VxEntity::EntityControlTypeEnum VsRigidBody::ConvertControlType()
{
    if(m_eControlType == ControlAnimated)
        return Vx::VxEntity::EntityControlTypeEnum::kControlAnimated;
    else if(m_eControlType == ControlNode)
        return Vx::VxEntity::EntityControlTypeEnum::kControlNode;
    else if(m_eControlType == ControlDynamic)
        return Vx::VxEntity::EntityControlTypeEnum::kControlDynamic;
    else
        return Vx::VxEntity::EntityControlTypeEnum::kControlStatic;
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

VsSimulator *VsRigidBody::GetVsSimulator()
{
    if(!m_lpVsSim)
    {
    	m_lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisVsMI)
		    THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpVsSim, " + m_lpThisAB->Name());
    }
	return m_lpVsSim;
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

void VsRigidBody::Physics_SetFreeze(bool bVal)
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
		bool bEnabled = m_lpThisRB->EnableFluids();

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

void VsRigidBody::SetFollowEntity(OsgRigidBody *lpEntity)
{
	if(m_vxSensor)
	{
        VsRigidBody *lpVsEntity = dynamic_cast<VsRigidBody *>(lpEntity);

        if(lpVsEntity)
        {
		    m_vxSensor->setFastMoving(true);
		    Vx::VxReal44 vOffset;
		    VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);
		    Vx::VxTransform vTM(vOffset);

		    Vx::VxCollisionSensor *vxSensor = lpVsEntity->Sensor();
		    if(vxSensor)
			    m_vxSensor->followEntity(vxSensor, true, &vTM);
        }
	}
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
		m_vxSensor->setControl(ConvertControlType());  // Set it to dynamic.
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
		VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());

		osg::MatrixTransform *lpMT = dynamic_cast<osg::MatrixTransform *>(m_osgMT.get());
		m_vxSensor->setNode(lpMT);               // Connect to the node.

		// Add the part to the universe.
		if(lpVsSim && lpVsSim->Universe())
			lpVsSim->Universe()->addEntity(m_vxSensor);
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
	ContactSensor *lpSensor = m_lpThisRB->GetContactSensor();
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

void VsRigidBody::WorldToBodyCoords(VxReal3 vWorld, StdVector3 &vLocalPos)
{
	osg::Vec3f vWorldPos;
	osg::Vec3f vLocal;

	vLocalPos[0] = vWorld[0]; vLocalPos[1] = vWorld[1]; vLocalPos[2] = vWorld[2];
	vWorldPos[0] = vWorld[0]; vWorldPos[1] = vWorld[1]; vWorldPos[2] = vWorld[2];

	if(m_osgNode.valid())
	{
	  osg::NodePathList paths = m_osgNode->getParentalNodePaths(); 
	  osg::Matrix worldToLocal = osg::computeWorldToLocal(paths.at(0)); 
	  vLocal = vWorldPos * worldToLocal;
	}

	vLocalPos[0] = vLocal[0]; vLocalPos[1] = vLocal[1]; vLocalPos[2] = vLocal[2];
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
        float fltRatio = fMassUnits * fDisUnits;
		m_vForce[0] = vData[0] * fltRatio;
		m_vForce[1] = vData[1] * fltRatio;
		m_vForce[2] = vData[2] * fltRatio;

        fltRatio = fMassUnits * fDisUnits * fDisUnits;
		m_vTorque[0] = vData2[0] * fltRatio;
		m_vTorque[1] = vData2[1] * fltRatio;
		m_vTorque[2] = vData2[2] * fltRatio;

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

	if(m_lpThisRB->GetContactSensor()) 
		ProcessContacts();
}

void VsRigidBody::Physics_ResetSimulation()
{
	OsgRigidBody::Physics_ResetSimulation();

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

void VsRigidBody::Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits)
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

void VsRigidBody::Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits)
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
    if(m_lpThisRB && m_vxSensor && m_lpThisRB->Freeze())
        m_vxSensor->freeze(false);

	if(m_vxPart)
		fltMass = m_vxPart->getMass();

    if(m_lpThisRB && m_vxSensor && m_lpThisRB->Freeze())
        m_vxSensor->freeze(true);

	return fltMass;
}

bool VsRigidBody::Physics_HasCollisionGeometry()
{
	if(m_vxSensor)
		return true;
	else
		return false;
}

	}			// Environment
}				//VortexAnimatSim
