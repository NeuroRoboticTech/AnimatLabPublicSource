// LineBase.cpp: implementation of the LineBase class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include <math.h>
#include "Node.h"
#include "IPhysicsBody.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a muscle object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a muscle. 
*/

LineBase::LineBase()
{
	m_bEnabled = TRUE;

	m_fltDensity = 0;
	m_lpJointToParent = NULL;
	m_bAllowMouseManipulation = FALSE;

	m_fltLength = 0;
	m_fltPrevLength = 0;
}

/*! \brief 
   Destroys the muscle object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the muscle object..	 
*/

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

float LineBase::CalculateLength(Simulator *lpSim)
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

	return (fltLength * lpSim->DistanceUnits());
}


// There are no parts or joints to create for muscle attachment points.
void LineBase::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
}

void LineBase::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	m_aryAttachmentPoints.Clear();

	string strID;
	Attachment *lpAttachment=NULL;
	int iCount = m_aryAttachmentPointIDs.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		strID = m_aryAttachmentPointIDs[iIndex];
		lpAttachment = dynamic_cast<Attachment *>(lpStructure->FindRigidBody(strID));
		m_aryAttachmentPoints.Add(lpAttachment);
	}

	if(m_aryAttachmentPoints.GetSize() < 2)
		m_bEnabled = FALSE;

	//Get the current length of the muscle.
	m_fltLength = CalculateLength(lpSim);
	m_fltPrevLength = m_fltLength;
}

void LineBase::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled);
	m_bIsVisible = oXml.GetChildBool("IsVisible", m_bIsVisible);

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

	m_vDiffuse.Load(oXml, "Diffuse", false);
	m_vAmbient.Load(oXml, "Ambient", false);
	m_vSpecular.Load(oXml, "Specular", false);
	m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess);

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
