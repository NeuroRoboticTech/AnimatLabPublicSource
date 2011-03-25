/**
\file	SimulationRecorder.cpp

\brief	Implements the simulation recorder class.
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sensor.h"
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
	namespace Recording
	{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
SimulationRecorder::SimulationRecorder()
{}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
SimulationRecorder::~SimulationRecorder()
{}


void SimulationRecorder::Add(ActivatedItem *lpItem)
{
	//Lets make sure that there are no other frames that already exist that overlap with
	//this frame that are the same type.
	int iCount = m_aryItems.GetSize();
	ActivatedItem *lpTest = NULL;
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpTest = m_aryItems[iIndex];

		if(lpTest->Overlaps(lpItem))
			THROW_PARAM_ERROR(Al_Err_lKeyFrameOverlap, Al_Err_strKeyFrameOverlap, "Overlap Frame ID", lpTest->ID()); 
	}

	ActivatedItemMgr::Add(lpItem);
}

/**
\brief	Adds a new keyframe type.

\author	dcofer
\date	3/24/2011

\param	strType	the keyframe Type to add. 
\param	lStart 	The start time of the keyframe. 
\param	lEnd   	The end time of the keyframe. 

\return	Pointer to the new keyframe.
**/
KeyFrame *SimulationRecorder::Add(string strType, long lStart, long lEnd)
{
	KeyFrame *lpFrame = NULL;

try
{
	lpFrame = dynamic_cast<KeyFrame *>(m_lpSim->CreateObject("AnimatLab", "KeyFrame", strType));
	if(!lpFrame)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "KeyFrame");

	lpFrame->EndSlice(lEnd);
	lpFrame->StartSlice(lStart);
	lpFrame->GenerateID();

	Add(lpFrame);

	lpFrame->Initialize();

	return lpFrame;
}
catch(CStdErrorInfo oError)
{
	if(lpFrame) delete lpFrame;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpFrame) delete lpFrame;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

void SimulationRecorder::Load(CStdXml &oXml)
{
	VerifySystemPointers();

	Reset();

	if(oXml.FindChildElement("KeyFrames", FALSE))
	{
		oXml.IntoElem(); //Into KeyFrames Element

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadKeyFrame(oXml);
		}

		oXml.OutOfElem(); //OutOf KeyFrames Element
	}
}

/**
\brief	Loads a new key frame.

\author	dcofer
\date	3/24/2011

\param [in,out]	oXml	The xml of the keyframe to load. 

\return	Pointer to the new keyframe.
**/
KeyFrame *SimulationRecorder::LoadKeyFrame(CStdXml &oXml)
{
	KeyFrame *lpFrame = NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem(); //Into KeyFrame Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf KeyFrame Element

	lpFrame = dynamic_cast<KeyFrame *>(m_lpSim->CreateObject(strModuleName, "KeyFrame", strType));
	if(!lpFrame)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "KeyFrame");

	lpFrame->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
	lpFrame->Load(oXml);

	Add(lpFrame);
	return lpFrame;
}
catch(CStdErrorInfo oError)
{
	if(lpFrame) delete lpFrame;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpFrame) delete lpFrame;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			//Recording
}				//AnimatSim
