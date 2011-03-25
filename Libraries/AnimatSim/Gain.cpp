/**
\file	Gain.cpp

\brief	Implements the gain base class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include "Gain.h"
#include "Node.h"
#include "IPhysicsBody.h"
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
	m_bUseLimits = FALSE;
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
{Std_TraceMsg(0, "Caught Error in desctructor of Gain\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Tells whether limits should be used. 

\author	dcofer
\date	3/16/2011

\return	true if using limits, false otherwise. 
**/
BOOL Gain::UseLimits() {return m_bUseLimits;}

/**
\brief	Sets if limits should be used. 

\author	dcofer
\date	3/16/2011

\param	bVal	true to use limits. 
**/
void Gain::UseLimits(BOOL bVal) {m_bUseLimits = bVal;}

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
void Gain::LowerLimit(float fltVal) {m_fltLowerLimit = fltVal;}

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
void Gain::UpperLimit(float fltVal) {m_fltUpperLimit = fltVal;}

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
		LowerLimit(oXml.GetChildFloat("LowerLimit"));
		LowerOutput(oXml.GetChildFloat("LowerOutput"));
		UpperLimit(oXml.GetChildFloat("UpperLimit"));
		UpperOutput(oXml.GetChildFloat("UpperOutput"));
		
		Std_IsAboveMin(m_fltLowerLimit, m_fltUpperLimit, TRUE, "UpperLimit");
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
Gain ANIMAT_PORT *LoadGain(Simulator *lpSim, string strName, CStdXml &oXml)
{
	Gain *lpGain = NULL;

	try
	{
		//Now lets load this Current Graph object.
		oXml.IntoChildElement(strName);
		string strModuleName = oXml.GetChildString("ModuleName", "");
		string strType = oXml.GetChildString("Type");
		oXml.OutOfElem(); //OutOf Gain Element

		Gain *lpGain = dynamic_cast<AnimatSim::Gains::Gain *>(lpSim->CreateObject(strModuleName, "Gain", strType));
		if(!lpGain)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "CurrentGraph");

		lpGain->SetSystemPointers(lpSim, NULL, NULL, NULL, TRUE);
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
