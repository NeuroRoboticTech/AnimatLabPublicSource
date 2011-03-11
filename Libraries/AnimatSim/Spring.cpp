/**
\file	Spring.cpp

\brief	Implements the spring class. 
**/

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
#include "LineBase.h"
#include "Spring.h"
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
Spring::Spring()
{
	m_bInitEnabled = FALSE;
	m_lpPrimaryAttachment = NULL;
	m_lpSecondaryAttachment = NULL;
	m_fltNaturalLength = 1;
	m_fltStiffness = 5000;
	m_fltDamping = 1000;
	m_fltDisplacement = 0;
	m_fltTension = 0;
	m_fltEnergy = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Spring::~Spring()
{
	try
	{
		//Don't delete these. They are only references
		m_lpPrimaryAttachment = NULL;
		m_lpSecondaryAttachment = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of Spring\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Tells whether the spring is enabled at startup of the simulation. 

\author	dcofer
\date	3/11/2011

\return	true if it enabled at startup, false otherwise. 
**/
BOOL Spring::InitEnabled() {return m_bInitEnabled;}

float Spring::NaturalLength() {return m_fltNaturalLength;}

void Spring::NaturalLength(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Spring.NaturalLength");

	if(bUseScaling)
		m_fltNaturalLength = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltNaturalLength = fltVal;
}

float Spring::Stiffness() {return m_fltStiffness;}

void Spring::Stiffness(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Spring.Stiffness");

	if(bUseScaling)
		m_fltStiffness = fltVal * m_lpSim->InverseMassUnits();
	else
		m_fltStiffness = fltVal;
}

float Spring::Damping() {return m_fltDamping;}

void Spring::Damping(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Spring.Damping", TRUE);

	if(bUseScaling)
		m_fltDamping = fltVal/m_lpSim->DensityMassUnits();
	else
		m_fltDamping = fltVal;
}

/**
\brief	Gets the curent displacement away from the natural length of the spring. 

\author	dcofer
\date	3/10/2011

\return	displacement of the spring. 
**/
float Spring::Displacement() {return m_fltDisplacement;}

/**
\brief	Gets the current tension of the spring. 

\author	dcofer
\date	3/10/2011

\return	Tension of the spring. 
**/
float Spring::Tension() {return m_fltTension;}

/**
\brief	Gets the current energy stored in the spring. 

\author	dcofer
\date	3/10/2011

\return	Energy in the spring. 
**/
float Spring::Energy() {return m_fltEnergy;}

// There are no parts or joints to create for muscle attachment points.
void Spring::CreateParts()
{
}

void Spring::AddExternalNodeInput(float fltInput)
{
	if(m_lpPrimaryAttachment && m_lpSecondaryAttachment)
	{
		if(fltInput > 0 && m_bEnabled != !m_bInitEnabled)
			Enabled(!m_bInitEnabled);

		if(fltInput <= 0 && m_bEnabled != m_bInitEnabled)
			Enabled(m_bInitEnabled);
	}
	else
		m_bEnabled = FALSE;
}

void Spring::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	LineBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	if(m_aryAttachmentPointIDs.GetSize() < 2)
		m_bEnabled = FALSE;

	NaturalLength(oXml.GetChildFloat("NaturalLength", m_fltNaturalLength));
	Stiffness(oXml.GetChildFloat("Stiffness", m_fltStiffness));
	Damping(oXml.GetChildFloat("Damping", m_fltDamping));

	m_bInitEnabled = m_bEnabled;

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
