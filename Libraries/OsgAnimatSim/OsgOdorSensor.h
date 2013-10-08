/**
\file	OsgOdorSensor.h

\brief	Declares the vortex odor sensor class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class ANIMAT_OSG_PORT OsgOdorSensor : public AnimatSim::Environment::Bodies::OdorSensor, public OsgRigidBody
{
protected:
    bool m_bPhsyicsDefined;

	virtual void CreateGraphicsGeometry();
	virtual void CreatePhysicsGeometry();
	virtual void ResizePhysicsGeometry();
    
    virtual void ProcessContacts() {};
	virtual void DeletePhysics(bool bIncludeChildren) {};
	virtual void CreateSensorPart() {};
	virtual void CreateStaticPart() {};
	virtual void CreateDynamicPart() {};
	virtual void RemoveStaticPart() {};
    virtual void GetBaseValues() {};

public:
	OsgOdorSensor();
	virtual ~OsgOdorSensor();
    
    virtual bool Physics_IsGeometryDefined() {return false;};
    virtual void Physics_EnableCollision(RigidBody *lpBody) {};
    virtual void Physics_DisableCollision(RigidBody *lpBody) {};
    virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) {};
	virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits) {};
    virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) {CStdFPoint vVel; return vVel;};
    virtual float Physics_GetMass() {return 0;};
    virtual bool Physics_IsDefined() {return m_bPhsyicsDefined;};

	virtual void CreateParts();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
