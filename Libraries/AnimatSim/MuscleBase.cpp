// MuscleBase.cpp: implementation of the MuscleBase class.
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
#include "MuscleBase.h"

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

MuscleBase::MuscleBase()
{
	m_fltMaxTension = 0;
	m_fltVm = (float) -0.15;
	m_fltTdot = 0;
	m_fltTension = 0;
	m_fltPrevTension = 0;
}

/*! \brief 
   Destroys the muscle object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the muscle object..	 
*/

MuscleBase::~MuscleBase()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of MuscleBase\r\n", "", -1, FALSE, TRUE);}
}

void MuscleBase::Enabled(BOOL bVal)
{
	m_bEnabled = bVal;
	m_fltEnabled = (float) bVal;

	if(!bVal)
	{
		m_fltTdot = 0;
		m_fltTension = 0;
	}
}

void MuscleBase::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput)
{
	//We are changing this. It is now really driven by the membrane voltage of the non-spiking neuron. Integration from 
	//different motor neurons takes place in the non-spiking neuron and we get that here instead of frequency and use that
	//to calculate the max isometric tension from the stim-tension curve.
	m_fltVm=fltInput;
}

void MuscleBase::Load(CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	LineBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_fltMaxTension = oXml.GetChildFloat("MaximumTension", m_fltMaxTension);
	Std_IsAboveMin((float) 0, m_fltMaxTension, TRUE, "Max Tension");

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
