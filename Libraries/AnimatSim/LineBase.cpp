/**
\file	LineBase.cpp

\brief	Implements the line base class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <math.h>
#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
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
#include "Simulator.h"

#include "ExternalStimulus.h"

#include "LineBase.h"

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
LineBase::LineBase()
{
	m_bEnabled = TRUE;

	m_fltDensity = 0;
	m_lpJointToParent = NULL;

	m_fltLength = 0;
	m_fltPrevLength = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
LineBase::~LineBase()
{
	try
	{
		m_aryAttachmentPointIDs.Clear();
		m_aryAttachmentPoints.Clear();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of LineBase\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the length of the line. 

\author	dcofer
\date	3/10/2011

\return	Length of the line. 
**/
float LineBase::Length() {return m_fltLength;}

/**
\brief	Gets the previous length of the line. 

\author	dcofer
\date	3/10/2011

\return	Previous length of the line. 
**/
float LineBase::PrevLength() {return m_fltPrevLength;}

BOOL LineBase::AllowMouseManipulation() {return FALSE;}

/**
\brief	Gets the attachment points. 

\author	dcofer
\date	3/10/2011

\return	Array of Attachment points. 
**/
CStdArray<Attachment *> *LineBase::AttachmentPoints() {return &m_aryAttachmentPoints;}

/**
\brief	Calculates the length of the line. 

\author	dcofer
\date	3/10/2011

\return	The calculated length. 
**/
float LineBase::CalculateLength()
{
	float fltLength=0;

	int iCount = m_aryAttachmentPoints.GetSize();

	if(iCount<2)
		return 0;

	Attachment *lpAttach1 = m_aryAttachmentPoints[0], *lpAttach2 = NULL;
	for(int iIndex=1; iIndex<iCount; iIndex++)
	{
		lpAttach2 = m_aryAttachmentPoints[iIndex];
		fltLength += Std_CalculateDistance(lpAttach1->GetCurrentPosition(), lpAttach2->GetCurrentPosition());
		lpAttach1 = lpAttach2;
	}

	return (fltLength * m_lpSim->DistanceUnits());
}


#pragma region DataAccesMethods

float *LineBase::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	float *lpData = RigidBody::GetDataPointer(strDataType);
	if(lpData) return lpData;

	if(strType == "MUSCLELENGTH" || strType == "LENGTH")
		lpData = &m_fltLength;
	else if(strType == "ENABLE")
		lpData = &m_fltEnabled;

	return lpData;
}

BOOL LineBase::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(strDataType == "POSITION")
	{
		//Position(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

// There are no parts or joints to create for muscle attachment points.
void LineBase::CreateParts()
{
}

void LineBase::CreateJoints()
{
	m_aryAttachmentPoints.Clear();

	string strID;
	Attachment *lpAttachment=NULL;
	int iCount = m_aryAttachmentPointIDs.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		strID = m_aryAttachmentPointIDs[iIndex];
		lpAttachment = dynamic_cast<Attachment *>(m_lpStructure->FindRigidBody(strID));
		m_aryAttachmentPoints.Add(lpAttachment);
	}

	if(m_aryAttachmentPoints.GetSize() < 2)
		m_bEnabled = FALSE;

	//Get the current length of the muscle.
	m_fltLength = CalculateLength();
	m_fltPrevLength = m_fltLength;
}

void LineBase::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	Enabled(oXml.GetChildBool("Enabled", m_bEnabled));
	IsVisible(oXml.GetChildBool("IsVisible", m_bIsVisible));

	m_aryAttachmentPointIDs.Clear();
	if(oXml.FindChildElement("Attachments", FALSE))
	{
		oXml.IntoElem();
		int iCount = oXml.NumberOfChildren();
		string strID;
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			strID = oXml.GetChildString();
			m_aryAttachmentPointIDs.Add(strID);
		}
		oXml.OutOfElem(); //OutOf NonSpikingSynapses Element
	}

	if(m_aryAttachmentPointIDs.GetSize() < 2)
		m_bEnabled = FALSE;

	//Load the colors
	m_vDiffuse.Load(oXml, "Diffuse", false);
	m_vAmbient.Load(oXml, "Ambient", false);
	m_vSpecular.Load(oXml, "Specular", false);
	m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess);

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
