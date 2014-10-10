// RbRigidBody.cpp: implementation of the RbRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbRigidBody::RbRigidBody()
{
	m_lpMaterial = NULL;
    m_lpRbSim = NULL;
}

RbRigidBody::~RbRigidBody()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RbRigidBody\r\n", "", -1, false, true);}
}

bool RbRigidBody::Physics_IsDefined()
{
    return true;
}

bool RbRigidBody::Physics_IsGeometryDefined()
{
    return true;
}

CStdFPoint RbRigidBody::Physics_GetCurrentPosition()
{
	CStdFPoint vPos;
	return vPos;
}

RbSimulator *RbRigidBody::GetRbSimulator()
{
    if(!m_lpRbSim)
    {
    	m_lpRbSim = dynamic_cast<RbSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisRbMI)
		    THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpRbSim, " + m_lpThisAB->Name());
    }
	return m_lpRbSim;
}

void RbRigidBody::Physics_UpdateNode()
{
}

void RbRigidBody::Physics_SetFreeze(bool bVal)
{
}

void RbRigidBody::Physics_SetMass(float fltVal)
{
}

float RbRigidBody::Physics_GetMass()
{
	return 0;
}

float RbRigidBody::Physics_GetDensity()
{
    return 0;
}

void RbRigidBody::Physics_SetMaterialID(std::string strID)
{
}

void RbRigidBody::Physics_SetVelocityDamping(float fltLinear, float fltAngular)
{
}

void RbRigidBody::Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz)
{
}

void  RbRigidBody::Physics_FluidDataChanged()
{
}

void RbRigidBody::Physics_WakeDynamics()
{
}

void RbRigidBody::Physics_ContactSensorAdded(ContactSensor *lpSensor)
{
}

void RbRigidBody::Physics_ContactSensorRemoved()
{
}

void RbRigidBody::Physics_ChildBodyAdded(RigidBody *lpChild)
{
}

void RbRigidBody::Physics_ChildBodyRemoved(bool bHasStaticJoint)
{
}

void RbRigidBody::Physics_CollectData()
{
}

void RbRigidBody::Physics_CollectExtraData()
{
}

void RbRigidBody::Physics_ResetSimulation()
{
}

void RbRigidBody::Physics_EnableCollision(RigidBody *lpBody)
{
}

void RbRigidBody::Physics_DisableCollision(RigidBody *lpBody)
{
}

void RbRigidBody::Physics_AddBodyForceAtLocalPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits)
{
}

void RbRigidBody::Physics_AddBodyForceAtWorldPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits)
{
}

void RbRigidBody::Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits)
{
}

CStdFPoint RbRigidBody::Physics_GetVelocityAtPoint(float x, float y, float z)
{
	CStdFPoint linVel(0,0,0);
	return linVel;
}	

bool RbRigidBody::Physics_HasCollisionGeometry()
{
	return true;
}

void RbRigidBody::Physics_StepHydrodynamicSimulation()
{
}

float *RbRigidBody::Physics_GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);
	RigidBody *lpBody = dynamic_cast<RigidBody *>(this);

	if(strType == "BODYBUOYANCY")
		return (&m_fltReportNull);

	if(strType == "BODYDRAGFORCEX")
		return (&m_fltReportNull);

	if(strType == "BODYDRAGFORCEY")
		return (&m_fltReportNull);

    if(strType == "BODYDRAGFORCEZ")
		return (&m_fltReportNull);

	if(strType == "BODYDRAGTORQUEX")
		return (&m_fltReportNull);

	if(strType == "BODYDRAGTORQUEY")
		return (&m_fltReportNull);

    if(strType == "BODYDRAGTORQUEZ")
		return (&m_fltReportNull);

	if(strType == "BODYTORQUEX")
		return (&m_fltReportNull);

	if(strType == "BODYTORQUEY")
		return (&m_fltReportNull);

	if(strType == "BODYTORQUEZ")
		return (&m_fltReportNull);

	if(strType == "BODYFORCEX")
		return (&m_fltReportNull);

	if(strType == "BODYFORCEY")
		return (&m_fltReportNull);

	if(strType == "BODYFORCEZ")
		return (&m_fltReportNull);

	if(strType == "BODYLINEARVELOCITYX")
		return (&m_fltReportNull);

	if(strType == "BODYLINEARVELOCITYY")
		return (&m_fltReportNull);

	if(strType == "BODYLINEARVELOCITYZ")
		return (&m_fltReportNull);

	if(strType == "BODYANGULARVELOCITYX")
		return (&m_fltReportNull);

	if(strType == "BODYANGULARVELOCITYY")
		return (&m_fltReportNull);

	if(strType == "BODYANGULARVELOCITYZ")
		return (&m_fltReportNull);

	if(strType == "BODYLINEARACCELERATIONX")
		return (&m_fltReportNull);

	if(strType == "BODYLINEARACCELERATIONY")
		return (&m_fltReportNull);

	if(strType == "BODYLINEARACCELERATIONZ")
		return (&m_fltReportNull);

	if(strType == "BODYANGULARACCELERATIONX")
		return (&m_fltReportNull);

	if(strType == "BODYANGULARACCELERATIONY")
		return (&m_fltReportNull);

	if(strType == "BODYANGULARACCELERATIONZ")
		return (&m_fltReportNull);

	return NULL;
}

	}			// Environment
}				//RoboticsAnimatSim
