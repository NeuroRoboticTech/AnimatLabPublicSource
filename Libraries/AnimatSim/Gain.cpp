/**
\file	Gain.cpp

\brief	Implements the gain base class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include "Gain.h"
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
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
#include "NervousSystem.h"
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
	namespace Gains
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
Gain::Gain()
{
	m_bUseLimits = false;
	m_fltLowerLimit = 0;
	m_fltLowerOutput = 0;
	m_fltUpperLimit = 0;
	m_fltUpperOutput = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
Gain::~Gain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Gain\r\n", "", -1, false, true);}
}

/**
\brief	Tells whether limits should be used. 

\author	dcofer
\date	3/16/2011

\return	true if using limits, false otherwise. 
**/
bool Gain::UseLimits() {return m_bUseLimits;}

/**
\brief	Sets if limits should be used. 

\author	dcofer
\date	3/16/2011

\param	bVal	true to use limits. 
**/
void Gain::UseLimits(bool bVal) {m_bUseLimits = bVal;}

/**
\brief	Gets the lower limit. 

\author	dcofer
\date	3/16/2011

\return	Lower limit. 
**/
float Gain::LowerLimit() {return m_fltLowerLimit;}

/**
\brief	Sets the Lower limit. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void Gain::LowerLimit(float fltVal) 
{
	Std_IsAboveMin(fltVal, m_fltUpperLimit, true, "LowerLimit");
	m_fltLowerLimit = fltVal;
}

/**
\brief	Gets the upper limit. 

\author	dcofer
\date	3/16/2011

\return	Upper limit. 
**/
float Gain::UpperLimit() {return m_fltUpperLimit;}

/**
\brief	Sets the Upper limit. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void Gain::UpperLimit(float fltVal) 
{
	Std_IsAboveMin(m_fltLowerLimit, fltVal, true, "UpperLimit");
	m_fltUpperLimit = fltVal;
}

/**
\brief	Gets the lower output. 

\author	dcofer
\date	3/16/2011

\return	The lower output. 
**/
float Gain::LowerOutput() {return m_fltLowerOutput;}

/**
\brief	Sets the Lower output. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void Gain::LowerOutput(float fltVal) {m_fltLowerOutput = fltVal;}

/**
\brief	Gets the upper output. 

\author	dcofer
\date	3/16/2011

\return	Upper output. 
**/
float Gain::UpperOutput() {return m_fltUpperOutput;}

bool Gain::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "USELIMITS")
	{
		UseLimits(Std_ToBool(strValue));
		return true;
	}

	if(strDataType == "LOWERLIMIT")
	{
		LowerLimit((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "LOWEROUTPUT")
	{
		LowerOutput((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "UPPERLIMIT")
	{
		UpperLimit((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "UPPEROUTPUT")
	{
		UpperOutput((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Gain::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("AnimatBase");
	aryTypes.Add("Boolean");

	aryNames.Add("LowerLimit");
	aryTypes.Add("Float");

	aryNames.Add("LowerOutput");
	aryTypes.Add("Float");

	aryNames.Add("UpperLimit");
	aryTypes.Add("Float");

	aryNames.Add("UpperOutput");
	aryTypes.Add("Float");
}

/**
\brief	Sets the Upper output. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void Gain::UpperOutput(float fltVal) {m_fltUpperOutput = fltVal;}

void Gain::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	UseLimits(oXml.GetChildBool("UseLimits", m_bUseLimits));

	if(m_bUseLimits)
	{
		m_fltLowerLimit = oXml.GetChildFloat("LowerLimit");
		m_fltUpperLimit = oXml.GetChildFloat("UpperLimit");
		m_fltLowerOutput = oXml.GetChildFloat("LowerOutput");
		m_fltUpperOutput = oXml.GetChildFloat("UpperOutput");
		
		Std_IsAboveMin(m_fltLowerLimit, m_fltUpperLimit, true, "UpperLimit");
	}

	oXml.OutOfElem(); //OutOf Adapter Element
}

/**
\brief	Loads a gain object. 

\author	dcofer
\date	3/16/2011

\param [in,out]	lpSim	Pointer to a simulation. 
\param	strName			Name of the xml element. 
\param [in,out]	oXml	The xml being loaded. 

\return	Pointer to the loaded gain. 
\exception Throws an exception if there is a problem during the load.
**/
Gain ANIMAT_PORT *LoadGain(Simulator *lpSim, std::string strName, CStdXml &oXml)
{
	Gain *lpGain = NULL;

	try
	{
		//Now lets load this Current Graph object.
		oXml.IntoChildElement(strName);
		std::string strModuleName = oXml.GetChildString("ModuleName", "");
		std::string strType = oXml.GetChildString("Type");
		oXml.OutOfElem(); //OutOf Gain Element

		Gain *lpGain = dynamic_cast<AnimatSim::Gains::Gain *>(lpSim->CreateObject(strModuleName, "Gain", strType));
		if(!lpGain)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "CurrentGraph");

		lpGain->SetSystemPointers(lpSim, NULL, NULL, NULL, true);
		lpGain->Load(oXml);

		return lpGain;
	}
	catch(CStdErrorInfo oError)
	{
		if(lpGain)
			delete lpGain;
		THROW_ERROR(oError.m_lError, oError.m_strError);
		return NULL;
	}
}

	}			//Gains
}			//AnimatSim
