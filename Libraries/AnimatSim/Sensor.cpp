/**
\file	Sensor.cpp

\brief	Implements the sensor class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
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
/**
\brief	Default constructor. 

\author	dcofer
\date	3/10/2011
**/
Sensor::Sensor()
{
	m_fltDensity = 0;
	m_bUsesJoint = FALSE;
	m_lpJointToParent = NULL;
	m_fltRadius = 1;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Sensor::~Sensor()
{

}

float Sensor::Radius() {return m_fltRadius;}

void Sensor::Radius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Sensor.Radius");
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;

	Resize();
}

// There are no parts or joints to create for muscle attachment points.
void Sensor::CreateParts()
{}

void Sensor::CreateJoints()
{}

void Sensor::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	Radius(oXml.GetChildFloat("Radius", m_fltRadius));
	oXml.OutOfElem(); //OutOf RigidBody Element

	//Reset the rotation to 0 for sensors
	m_oRotation.Set(0, 0, 0);
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
