/**
\file	Spring.cpp

\brief	Implements the spring class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
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
#include "Light.h"
#include "LightManager.h"
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
	m_bInitEnabled = false;
	m_fltNaturalLength = 1;
	m_fltNaturalLengthNotScaled = m_fltNaturalLength;
	m_fltStiffness = 5000;
	m_fltStiffnessNotScaled = m_fltStiffness;
	m_fltDamping = 1000;
    m_fltDampingNotScaled = m_fltDamping;
	m_fltDisplacement = 0;
	m_fltTension = 0;
	m_fltEnergy = 0;
    m_fltVelocity = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Spring::~Spring()
{
}

/**
\brief	Tells whether the spring is enabled at startup of the simulation. 

\author	dcofer
\date	3/11/2011

\return	true if it enabled at startup, false otherwise. 
**/
bool Spring::InitEnabled() {return m_bInitEnabled;}

float Spring::NaturalLength() {return m_fltNaturalLength;}

void Spring::NaturalLength(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Spring.NaturalLength");

	m_fltNaturalLengthNotScaled = fltVal;
	if(bUseScaling)
		m_fltNaturalLength = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltNaturalLength = fltVal;
}

float Spring::Stiffness() {return m_fltStiffness;}

void Spring::Stiffness(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Spring.Stiffness");

	m_fltStiffnessNotScaled = fltVal;
	if(bUseScaling)
		m_fltStiffness = fltVal * m_lpSim->InverseMassUnits();
	else
		m_fltStiffness = fltVal;
}

float Spring::Damping() {return m_fltDamping;}

void Spring::Damping(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Spring.Damping", true);

    m_fltDampingNotScaled = fltVal;
	if(bUseScaling)
		m_fltDamping = fltVal/m_lpSim->DisplayMassUnits();
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


/**
\brief	Gets the velocity of the length change of the spring. 

\author	dcofer
\date	3/10/2011

\return	velocity in the spring. 
**/
float Spring::Velocity() {return m_fltVelocity;}


// There are no parts or joints to create for muscle attachment points.
void Spring::CreateParts()
{
}

void Spring::ResetSimulation()
{
    LineBase::ResetSimulation();
    CalculateTension();
}

void Spring::CalculateTension()
{
	if(m_bEnabled)
	{
        m_fltPrevLength = m_fltLength;
		m_fltLength = CalculateLength();
		m_fltDisplacement = (m_fltLength - m_fltNaturalLengthNotScaled);

    	m_fltVelocity = (m_fltLength-m_fltPrevLength)/m_lpSim->PhysicsTimeStep();

		m_fltTension = m_fltStiffnessNotScaled * m_fltDisplacement + m_fltVelocity*m_fltDamping;
		m_fltEnergy = 0.5f*m_fltStiffnessNotScaled*m_fltDisplacement*m_fltDisplacement;
	}
    else
    {
		m_fltDisplacement = 0;
		m_fltTension = 0;
		m_fltEnergy = 0;
        m_fltVelocity = 0;
    }
}

void Spring::AddExternalNodeInput(float fltInput)
{
	if(m_aryAttachmentPoints.GetSize() == 2)
	{
		if(fltInput > 0 && m_bEnabled != !m_bInitEnabled)
			Enabled(!m_bInitEnabled);

		if(fltInput <= 0 && m_bEnabled != m_bInitEnabled)
			Enabled(m_bInitEnabled);
	}
	else
		m_bEnabled = false;
}


float *Spring::GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "SPRINGLENGTH")
		return &m_fltLength;

	if(strType == "DISPLACEMENT")
		return &m_fltDisplacement;

	if(strType == "TENSION")
		return &m_fltTension;

	if(strType == "ENERGY")
		return &m_fltEnergy;

	if(strType == "ENABLE")
		return &m_fltEnabled;

	if(strType == "VELOCITY")
		return &m_fltVelocity;

	return LineBase::GetDataPointer(strDataType);
}

bool Spring::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	if(LineBase::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "NATURALLENGTH")
	{
		NaturalLength((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "STIFFNESS")
	{
		Stiffness((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "DAMPING")
	{
		Damping((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Spring::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	LineBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("NaturalLength");
	aryTypes.Add("Float");

	aryNames.Add("Stiffness");
	aryTypes.Add("Float");

	aryNames.Add("Damping");
	aryTypes.Add("Float");
}

void Spring::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	LineBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	if(m_aryAttachmentPointIDs.GetSize() < 2)
		m_bEnabled = false;

	NaturalLength(oXml.GetChildFloat("NaturalLength", m_fltNaturalLength));
	Stiffness(oXml.GetChildFloat("Stiffness", m_fltStiffness));
	Damping(oXml.GetChildFloat("Damping", m_fltDamping));

	m_bInitEnabled = m_bEnabled;

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
