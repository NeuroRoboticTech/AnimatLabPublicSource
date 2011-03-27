// VsRigidBody.cpp: implementation of the VsRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsRigidBody.h"
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

	m_fltBuoyancy = 0;
	m_fltReportBuoyancy = 0;
	m_fltMass = 0;
	m_fltReportMass = 0;
	m_fltReportVolume = 0;

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

CStdFPoint VsRigidBody::Physics_GetCurrentPosition()
{
	Vx::VxReal3Ptr vPos = NULL;
	m_vxSensor->getPosition(vPos);
	m_lpThis->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
	return m_lpThis->AbsolutePosition();
}

void VsRigidBody::Physics_UpdateMatrix()
{
	VsBody::Physics_UpdateMatrix();

	m_lpThis->UpdatePhysicsPosFromGraphics();
}
 
void VsRigidBody::UpdatePositionAndRotationFromMatrix()
{
	VsBody::UpdatePositionAndRotationFromMatrix();

	m_lpThis->UpdatePhysicsPosFromGraphics();
}

void VsRigidBody::Physics_UpdateNode()
{
	if(m_vxSensor)
		m_vxSensor->updateFromNode();
}

void VsRigidBody::Physics_SetColor()
{
	SetColor(*m_lpThisBody->Ambient(), *m_lpThisBody->Diffuse(), *m_lpThisBody->Specular(), m_lpThisBody->Shininess()); 
}

void VsRigidBody::Physics_TextureChanged()
{
	SetTexture(m_lpThisBody->Texture());
}

void VsRigidBody::SetFreeze(BOOL bVal)
{
	if(m_vxSensor)
		m_vxSensor->freeze(bVal);
}

void VsRigidBody::SetDensity(float fltVal)
{
	if(m_vxSensor)
		m_vxCollisionGeometry->setRelativeDensity(fltVal);
}

void VsRigidBody::GetBaseValues()
{
	if(m_vxPart)
	{
		//Fluid Density is in the units being used. So if the user set 1 g/cm^3 
		//and the units were grams and decimeters then density would be 1000 g/dm^3

		//Simulate buoyancy
		//Fb = Pl * (Ms/Ps) * G
		m_fltMass = m_vxPart->getMass();  //mass is in unit scale values. (ie. grams)
		float fltVolume = m_fltMass/m_lpThisBody->Density();   //volume is in unit scale values. (ie. decimeters^3)

		//We need to convert the mass to grams and the volume to cubic meters for reporting purposes.
		m_fltReportMass = m_fltMass*m_lpThis->GetSimulator()->MassUnits();
		m_fltReportVolume = fltVolume*(pow(m_lpThis->GetSimulator()->DistanceUnits(), 3));
		m_fltBuoyancy = -(m_lpThis->GetSimulator()->FluidDensity() * fltVolume * m_lpThis->GetSimulator()->Gravity());
		m_fltReportBuoyancy = m_fltBuoyancy * m_lpThis->GetSimulator()->MassUnits() * m_lpThis->GetSimulator()->DistanceUnits();
	}
}

void VsRigidBody::Initialize()
{
	GetBaseValues();
}

osg::Group *VsRigidBody::ParentOSG()
{
	RigidBody *lpParent = NULL;

	if(m_lpThis) lpParent = m_lpThis->Parent();

	if(lpParent)
	{
		VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(lpParent);
		if(lpVsParent) return lpVsParent->GetMatrixTransform();
	}
	else if(m_lpThis->GetStructure())
	{
		void *lpGroup = m_lpThis->GetStructure()->GetMatrixPointer();
		osg::Group *osgGroup = (osg::Group*) lpGroup;
		return osgGroup;
	}
	return NULL;
}

void VsRigidBody::SetupGraphics()
{
	m_osgParent = ParentOSG();

	if(m_osgParent.valid())
	{
		BuildLocalMatrix();

		SetColor(*m_lpThisBody->Ambient(), *m_lpThisBody->Diffuse(), *m_lpThisBody->Specular(), m_lpThisBody->Shininess());
		SetTexture(m_lpThisBody->Texture());
		SetCulling();
		SetVisible(m_lpThis->IsVisible());

		//Add it to the scene graph.
		m_osgParent->addChild(m_osgRoot.get());

		//Set the position with the world coordinates.
		m_lpThis->AbsolutePosition(VsBody::GetOSGWorldCoords());

		//We need to set the UserData on the OSG side so we can do picking.
		//We need to use a node visitor to set the user data for all drawable nodes in all geodes for the group.
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void VsRigidBody::SetupPhysics()
{
	//If no geometry is defined then this part does not have a physics representation.
	//it is purely an osg node attached to other parts. An example of this is an attachment point or a sensor.
	if(m_vxGeometry)
	{
		//If the parent is not null and the joint is null then that means we need to statically link this part to 
		//its parent. So we do not create a physics part, we just get a link to its parents part.
		if(m_lpThisBody->IsContactSensor())
			CreateSensorPart();
		else if(m_lpThis->Parent() && !m_lpThisBody->JointToParent())
			CreateStaticPart();
		else
			CreateDynamicPart();

		SetFluidInteractions();
	}
}

void VsRigidBody::CreateSensorPart()
{
	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThis->Parent());

	m_vxSensor = new VxCollisionSensor;
	m_vxPart = NULL;
	m_vxSensor->setUserData((void*) m_lpThis);

	m_vxSensor->setName(m_lpThis->ID().c_str());               // Give it a name.
    m_vxSensor->addGeometry(m_vxGeometry, 0);
	m_vxSensor->setControl(VxEntity::kControlNode);

	if(lpVsParent)
	{
		m_vxSensor->setFastMoving(true);
		Vx::VxReal44 vOffset;
		VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);
		Vx::VxTransform vTM(vOffset);

		Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
		if(vxSensor)
			m_vxSensor->followEntity(vxSensor, false, &vTM);
	}
	else
	{
		m_vxSensor->freeze(m_lpThisBody->Freeze());
	}
}

void VsRigidBody::CreateDynamicPart()
{
	// Create the physics object.
	m_vxPart = new VxPart;
	m_vxSensor = m_vxPart;
	m_vxSensor->setUserData((void*) m_lpThis);
	int iMaterialID = m_lpThis->GetSimulator()->GetMaterialID(m_lpThisBody->MaterialID());

	m_vxSensor->setName(m_lpThis->ID().c_str());               // Give it a name.
    m_vxSensor->setControl(m_eControlType);  // Set it to dynamic.
    m_vxCollisionGeometry = m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisBody->Density());
	m_vxSensor->setFastMoving(true);

	//if this body is frozen; freeze it
	m_vxSensor->freeze(m_lpThisBody->Freeze());

	//if the center of mass isn't the graphical center then set the offset relative to position and orientation
	if(m_vxPart)
	{
		CStdFPoint vCOM = m_lpThisBody->CenterOfMass();
		if(vCOM.x != 0 || vCOM.y != 0 || vCOM.z != 0)
			m_vxPart->setCOMOffset(vCOM.x, vCOM.y, vCOM.z);

		if(m_lpThisBody->LinearVelocityDamping() > 0)
			m_vxPart->setLinearVelocityDamping(m_lpThisBody->LinearVelocityDamping());

		if(m_lpThisBody->AngularVelocityDamping() > 0)
			m_vxPart->setAngularVelocityDamping(m_lpThisBody->AngularVelocityDamping());
	}
}

void VsRigidBody::CreateStaticPart()
{
	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpThis->Parent());

	Vx::VxReal44 vOffset;
	VxOSG::copyOsgMatrix_to_VxReal44(m_osgMT->getMatrix(), vOffset);
	int iMaterialID = m_lpThis->GetSimulator()->GetMaterialID(m_lpThisBody->MaterialID());

	Vx::VxCollisionSensor *vxSensor = lpVsParent->Sensor();
	if(vxSensor)
	    m_vxCollisionGeometry = vxSensor->addGeometry(m_vxGeometry, iMaterialID, vOffset, m_lpThisBody->Density());
}

void VsRigidBody::DeletePhysics()
{
	if(m_vxSensor)
	{
		VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThis->GetSimulator());
		if(!lpVsSim)
			THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

		lpVsSim->Universe()->removeEntity(m_vxSensor);
		delete m_vxSensor;

		m_vxSensor = NULL;
		m_vxPart = NULL;
	}
}

void VsRigidBody::SetBody()
{
	if(m_vxSensor)
	{
		//VxAssembly *lpAssem = (VxAssembly *) lpStructure->Assembly();
		VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThis->GetSimulator());

		osg::MatrixTransform *lpMT = dynamic_cast<osg::MatrixTransform *>(m_osgMT.get());
		m_vxSensor->setNode(lpMT);               // Connect to the node.

		// Add the part to the universe.
		lpVsSim->Universe()->addEntity(m_vxSensor);
		//lpAssem->addEntity(m_vxSensor);
	}
}

void VsRigidBody::SetFluidInteractions()
{
	if(m_lpThis->GetSimulator()->SimulateHydrodynamics() && m_vxCollisionGeometry && m_lpThisBody->Density() > 0)
	{
		m_vxCollisionGeometry->setDefaultFluidInteractionForceFn();
		VxCollisionGeometry::FluidInteractionData data;
		//data.mMagnusCoefficient = 0;
		//data.mBuoyancyCenter = VxVector3(0, 0, 0);
		//data.mBuoyancyForceScale = 1;
		data.mDragCoefficient = VxVector3(m_lpThisBody->Cd());
		m_vxCollisionGeometry->setFluidInteractionData(data);
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
	ContactSensor *lpSensor = m_lpThisBody->ContactSensor();

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

		for(; itd != m_vxPart->dynamicsContactEnd(); ++itd)
		{
			VxDynamicsContact *vxDyn = *itd;
			vxDyn->getPartPair(p, p+1);

			vxDyn->getPosition(vWorldPos);
			WorldToBodyCoords(vWorldPos, vBodyPos);

			iPartIdx = GetPartIndex(p[0], p[1]);
			vxDyn->getForce(iPartIdx, vForce);
			fltForceMag = V3_MAG(vForce);

			lpSensor->ProcessContact(vBodyPos, fltForceMag);
		}
	}
}

void VsRigidBody::Physics_CollectBodyData()
{
	float fDisUnits = m_lpThis->GetSimulator()->DistanceUnits();
	float fMassUnits = m_lpThis->GetSimulator()->MassUnits();
	Vx::VxReal3 vData;

	if(m_vxSensor)
	{
		m_vxSensor->getPosition(vData);
		m_lpThis->AbsolutePosition(vData[0], vData[1], vData[2]);
		m_lpThis->ReportWorldPosition(m_lpThis->AbsolutePosition() * fDisUnits);

		m_vxSensor->getOrientationEulerAngles(vData);
		m_lpThis->ReportRotation(vData[0], vData[1], vData[2]);
	}
	else
	{
		//If we are here then we did not have a physics component, just and OSG one.
		CStdFPoint vPos = VsBody::GetOSGWorldCoords();
		m_lpThis->AbsolutePosition(vPos.x, vPos.y, vPos.z);
		m_lpThis->ReportWorldPosition(m_lpThis->AbsolutePosition() * fDisUnits);

		//Get Rotation
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

	if(m_lpThisBody->ContactSensor()) 
		ProcessContacts();
}

void VsRigidBody::Physics_ResetSimulation()
{
	if(m_osgMT.valid())
	{
		BuildLocalMatrix();

		//Set the position with the world coordinates.
		m_lpThis->AbsolutePosition(VsBody::GetOSGWorldCoords());
		m_lpThis->ReportWorldPosition(m_lpThis->AbsolutePosition() * m_lpThis->GetSimulator()->DistanceUnits());
		m_lpThis->ReportRotation(m_lpThis->Rotation());
	}

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

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThis->GetSimulator());

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

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThis->GetSimulator());

	if(!lpVsSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	//If collisions between the two objects is enabled then disable it.
	if(lpVsSim->Universe()->getPairIntersectEnabled(m_vxCollisionGeometry, lpVsBody->CollisionGeometry()) == true)
		lpVsSim->Universe()->disablePairIntersect(m_vxPart, lpVsBody->Sensor());
}

float *VsRigidBody::Physics_GetDataPointer(string strDataType)
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

	if(strType == "BODYBUOYANCY")
		{m_bCollectExtraData = TRUE; return (&m_fltReportBuoyancy);}

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

	if(strType == "MASS")
		return &m_fltReportMass;

	if(strType == "VOLUME")
		return &m_fltReportVolume;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(lpBody->ID()) + "  DataType: " + strDataType);

	return NULL;
}

void VsRigidBody::Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits)
{
	if(m_vxPart && (fltFx || fltFy || fltFz) && !m_lpThisBody->Freeze())
	{
		VxReal3 fltF, fltP;
		if(bScaleUnits)
		{
			fltF[0] = fltFx * (m_lpThis->GetSimulator()->InverseMassUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits());
			fltF[1] = fltFy * (m_lpThis->GetSimulator()->InverseMassUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits());
			fltF[2] = fltFz * (m_lpThis->GetSimulator()->InverseMassUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits());
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
			fltT[0] = fltTx * (m_lpThis->GetSimulator()->InverseMassUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits());
			fltT[1] = fltTy * (m_lpThis->GetSimulator()->InverseMassUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits());
			fltT[2] = fltTz * (m_lpThis->GetSimulator()->InverseMassUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits() * m_lpThis->GetSimulator()->InverseDistanceUnits());
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

	if(m_vxPart)
		fltMass = m_vxPart->getMass();

	return fltMass;
}

	}			// Environment
}				//VortexAnimatSim
