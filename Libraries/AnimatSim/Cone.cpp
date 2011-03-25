/**
\file	Cone.cpp

\brief	Implements the cone class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Cone.h"
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
\date	3/4/2011
**/
Cone::Cone()
{
	m_fltLowerRadius = 1;
	m_fltUpperRadius = 1;
	m_fltHeight = 1;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/4/2011
**/
Cone::~Cone()
{

}

float Cone::LowerRadius() {return m_fltLowerRadius;}

void Cone::LowerRadius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Cone.LowerRadius", TRUE);
	if(m_fltLowerRadius == 0 && m_fltUpperRadius == 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidConeRadius, Al_Err_strInvalidConeRadius, "Body", m_strName);

	if(bUseScaling)
		m_fltLowerRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLowerRadius = fltVal;

	Resize();
}

float Cone::UpperRadius() {return m_fltUpperRadius;}

void Cone::UpperRadius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Cone.UpperRadius", TRUE);
	if(m_fltLowerRadius == 0 && m_fltUpperRadius == 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidConeRadius, Al_Err_strInvalidConeRadius, "Body", m_strName);

	if(bUseScaling)
		m_fltUpperRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltUpperRadius = fltVal;

	Resize();
}


float Cone::Height() {return m_fltHeight;}

void Cone::Height(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Cone.Height");
	if(bUseScaling)
		m_fltHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltHeight = fltVal;

	Resize();
}

void Cone::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	LowerRadius(oXml.GetChildFloat("LowerRadius", m_fltLowerRadius));
	UpperRadius(oXml.GetChildFloat("UpperRadius", m_fltUpperRadius));
	Height(oXml.GetChildFloat("Height"), m_fltHeight);
	oXml.OutOfElem(); //OutOf RigidBody Element

	m_lpSim->HasConvexMesh(TRUE);
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
