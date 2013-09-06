// BlSpring.cpp: implementation of the BlSpring class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlLine.h"
#include "BlSpring.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlSpring::BlSpring()
{
	SetThisPointers();
}

BlSpring::~BlSpring()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlSpring/\r\n", "", -1, false, true);}
}

//void BlSpring::DeletePhysics()
//{
//    //FIX PHYSICS
//	//if(!m_vxSpring)
//	//	return;
//
//	//if(GetBlSimulator() && GetBlSimulator()->Universe())
//	//{
//	//	GetBlSimulator()->Universe()->removeConstraint(m_vxSpring);
//	//	delete m_vxSpring;
//	//}
//
//	//m_vxSpring = NULL;
//}
//
//void BlSpring::SetupPhysics()
//{
//	if(m_aryAttachmentPoints.GetSize() == 2)
//	{
//		Attachment *lpPrimaryAttachment = m_aryAttachmentPoints[0];
//		Attachment *lpSecondaryAttachment = m_aryAttachmentPoints[1];
//
//		if(!lpPrimaryAttachment && !lpSecondaryAttachment)
//		{
//			Enabled(false);
//			return;
//		}
//
//		BlRigidBody *lpVsPrimary = dynamic_cast<BlRigidBody *>(lpPrimaryAttachment->Parent());
//		BlRigidBody *lpVsSecondary = dynamic_cast<BlRigidBody *>(lpSecondaryAttachment->Parent());
//
//		if(!lpVsPrimary && !lpVsSecondary)
//		{
//			Enabled(false);
//			return;
//		}
//
//        //FIX PHYSICS
//		//m_vxSpring = new AnimatVxSpring(lpVsPrimary->Part(), lpVsSecondary->Part(), m_fltNaturalLength, m_fltStiffness, m_fltDamping); // attached to the reference frame.
//
//		//CStdFPoint vPrimPos = lpPrimaryAttachment->AbsolutePosition();
//		//CStdFPoint vSecPos = lpSecondaryAttachment->AbsolutePosition();
//		//
//	 //   m_vxSpring->setPartAttachmentPosition(0, vPrimPos.x, vPrimPos.y, vPrimPos.z);
//	 //   m_vxSpring->setPartAttachmentPosition(1, vSecPos.x, vSecPos.y, vSecPos.z);
//
//		//m_vxSpring->enable(m_bEnabled);
//		//GetBlSimulator()->Universe()->addConstraint(m_vxSpring);
//	}
//	else
//		Enabled(false);
//}

void BlSpring::CreateJoints()
{
	Spring::CreateJoints();
	BlLine::CreateParts();

	SetupPhysics();
}

void BlSpring::ResetSimulation()
{
	Spring::ResetSimulation();
	BlLine::ResetSimulation();
}

void BlSpring::AfterResetSimulation()
{
	Spring::AfterResetSimulation();
	BlLine::AfterResetSimulation();
}

void BlSpring::StepSimulation()
{
	CalculateTension();
	BlLine::StepSimulation(m_fltTension); 
}


		}		//Joints
	}			// Environment
}				//BulletAnimatSim
