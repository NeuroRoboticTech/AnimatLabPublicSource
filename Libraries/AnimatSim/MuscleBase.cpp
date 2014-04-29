/**
\file	MuscleBase.cpp

\brief	Implements the muscle base class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <math.h>
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
#include "Sensor.h"
#include "Attachment.h"
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

#include "ExternalStimulus.h"

#include "LineBase.h"
#include "Gain.h"
#include "SigmoidGain.h"
#include "LengthTensionGain.h"
#include "MuscleBase.h"

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
MuscleBase::MuscleBase()
{
	m_fltMaxTension = 0;
	m_fltVm = (float) -0.15;
	m_fltTdot = 0;
	m_fltTension = 0;
	m_fltPrevTension = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
MuscleBase::~MuscleBase()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of MuscleBase\r\n", "", -1, false, true);}
}

/**
\brief	Gets the tension of the muscle. 

\author	dcofer
\date	3/10/2011

\return	Tension. 
**/
float MuscleBase::Tension() {return m_fltTension;}

/**
\brief	Sets the tension of the muscle. 

\author	dcofer
\date	3/10/2011

\param	fltVal	The new value. 
\exception Tension cannot be less than zero.
**/
void MuscleBase::Tension(float fltVal)
{
	if(fltVal < 0)
		THROW_PARAM_ERROR(Al_Err_lForceLessThanZero, Al_Err_strForceLessThanZero, "MuscleID", m_strName);

	m_fltTension = fltVal;
}

/**
\brief	Gets the maximum tension. 

\author	dcofer
\date	3/10/2011

\return	Maximum tension allowed. 
**/
float MuscleBase::MaxTension() {return m_fltMaxTension;}

/**
\brief	Sets the Maximum tension. 

\author	dcofer
\date	3/10/2011

\param	fltVal	The new value. 
\exception Max tension must be greater than zero.
**/
void MuscleBase::MaxTension(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Max Tension");
	m_fltMaxTension = fltVal;
}

/**
\brief	Gets the total stimulation applied to the muscle. 

\author	dcofer
\date	3/10/2011

\return	Total Stim. 
**/
float MuscleBase::Vm() {return m_fltVm;}

/**
\brief	Gets the derivative of the tension. 

\author	dcofer
\date	3/10/2011

\return	Derivated of tension. 
**/
float MuscleBase::Tdot() {return m_fltTdot;}

/**
\brief	Gets the previous tension. 

\author	dcofer
\date	3/10/2011

\return	Previous tension. 
**/
float MuscleBase::PrevTension() {return m_fltPrevTension;}

/**
\brief	Gets whether the muscle is enabled. 

\author	dcofer
\date	3/10/2011

\return	true if it enabled, false otherwise. 
**/
bool MuscleBase::Enabled() {return m_bEnabled;};

/**
\brief	Sets whether this muscle is Enabled. 

\author	dcofer
\date	3/10/2011

\param	bVal	true to enable. 
**/
void MuscleBase::Enabled(bool bVal)
{
	LineBase::Enabled(bVal);

	if(!bVal)
	{
		m_fltTdot = 0;
		m_fltTension = 0;
	}
}

SigmoidGain *MuscleBase::StimTension() {return &m_gainStimTension;}



/**
\brief	Sets the stim-tension gain using an xml packet.

\author	dcofer

\param	strXml	The xml packet defining the gain. 
**/
void MuscleBase::StimTension(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");
	m_gainStimTension.Load(oXml);
}

LengthTensionGain *MuscleBase::LengthTension() {return &m_gainLengthTension;}

/**
\brief	Sets the stim-tension gain using an xml packet.

\author	dcofer

\param	strXml	The xml packet defining the gain. 
**/
void MuscleBase::LengthTension(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");
	m_gainLengthTension.Load(oXml);
}

void MuscleBase::AddExternalNodeInput(float fltInput)
{
	//We are changing this. It is now really driven by the membrane voltage of the non-spiking neuron. Integration from 
	//different motor neurons takes place in the non-spiking neuron and we get that here instead of frequency and use that
	//to calculate the max isometric tension from the stim-tension curve.
	m_fltVm=fltInput;
}

void MuscleBase::ResetSimulation()
{
	LineBase::ResetSimulation();

	m_fltVm = 0;
	m_fltTdot = 0;
	m_fltTension = 0;
	m_fltPrevTension = 0;
}

#pragma region DataAccesMethods

void MuscleBase::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	m_gainStimTension.SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	m_gainLengthTension.SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	LineBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
}

void MuscleBase::VerifySystemPointers()
{
	LineBase::VerifySystemPointers();
	m_gainStimTension.VerifySystemPointers();
	m_gainLengthTension.VerifySystemPointers();
}

float *MuscleBase::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "TENSION")
		lpData = &m_fltTension;
	else if(strType == "TDOT")
		lpData = &m_fltTdot;
	else if(strType == "MEMBRANEVOLTAGE")
		lpData = &m_fltVm;
	else
		lpData = LineBase::GetDataPointer(strDataType);

	return lpData;
}

bool MuscleBase::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(LineBase::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "MAXTENSION")
	{
		MaxTension((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "STIMULUSTENSION")
	{
		StimTension(strValue);
		return true;
	}

	if(strDataType == "LENGTHTENSION")
	{
		LengthTension(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void MuscleBase::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	LineBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("MaxTension", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StimTension", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LengthTension", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
}

#pragma endregion

void MuscleBase::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	LineBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_fltMaxTension = oXml.GetChildFloat("MaximumTension", m_fltMaxTension);

	if(oXml.FindChildElement("StimulusTension"))
		m_gainStimTension.Load(oXml);

	if(oXml.FindChildElement("LengthTension"))
		m_gainLengthTension.Load(oXml);

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
