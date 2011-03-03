// Sensor.cpp: implementation of the Sensor class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sensor.h"
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Sensor::Sensor()
{
	m_fltDensity = 0;
	m_bUsesJoint = FALSE;
	m_lpJointToParent = NULL;
	m_fltRadius = 1;
}

Sensor::~Sensor()
{

}

// There are no parts or joints to create for muscle attachment points.
void Sensor::CreateParts(Simulator *lpSim, Structure *lpStructure)
{}

void Sensor::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{}

void Sensor::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	AnimatBase::Load(oXml);

	//We do NOT want to call the rigid body load here. The reason is that
	//a sensor is a leaf node. It can not have children. So the load is 
	//a lot simpler.
	oXml.IntoElem();  //Into RigidBody Element

	Std_LoadPoint(oXml, "LocalPosition", m_oLocalPosition);

	m_fltRadius = oXml.GetChildFloat("Radius", m_fltRadius);
	Std_IsAboveMin((float) 0,m_fltRadius, TRUE, "Radius");
	
	m_bIsVisible = oXml.GetChildBool("IsVisible", m_bIsVisible);

	m_vDiffuse.Load(oXml, "Diffuse", false);
	m_vAmbient.Load(oXml, "Ambient", false);
	m_vSpecular.Load(oXml, "Specular", false);
	m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess);

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
