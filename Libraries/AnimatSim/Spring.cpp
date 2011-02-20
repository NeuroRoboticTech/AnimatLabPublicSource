// Spring.cpp: implementation of the Spring class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
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
#include "Simulator.h"

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
   Constructs a Spring joint.
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a Spring joint. 
*/

Spring::Spring()
{
	m_bInitEnabled = FALSE;
	m_lpPrimaryAttachment = NULL;
	m_lpSecondaryAttachment = NULL;
	m_fltNaturalLength = 1;
	m_fltStiffness = 5000;
	m_fltDamping = 1000;
	m_fltDisplacement = 0;
	m_fltTension = 0;
	m_fltEnergy = 0;
}


/*! \brief 
   Destroys the Spring joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Spring joint object..	 
*/

Spring::~Spring()
{
	try
	{
		//Don't delete these. They are only references
		m_lpPrimaryAttachment = NULL;
		m_lpSecondaryAttachment = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of Spring\r\n", "", -1, FALSE, TRUE);}
}

// There are no parts or joints to create for muscle attachment points.
void Spring::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
}

void Spring::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput)
{
	if(m_lpPrimaryAttachment && m_lpSecondaryAttachment)
	{
		if(fltInput > 0 && m_bEnabled != !m_bInitEnabled)
			Enabled(!m_bInitEnabled);

		if(fltInput <= 0 && m_bEnabled != m_bInitEnabled)
			Enabled(m_bInitEnabled);
	}
	else
		m_bEnabled = FALSE;
}

void Spring::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	LineBase::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element

	if(m_aryAttachmentPointIDs.GetSize() < 2)
		m_bEnabled = FALSE;

	m_fltNaturalLength = oXml.GetChildFloat("NaturalLength", m_fltNaturalLength);
	m_fltStiffness = oXml.GetChildFloat("Stiffness", m_fltStiffness);
	m_fltDamping = oXml.GetChildFloat("Damping", m_fltDamping);
	
	m_fltNaturalLength *= lpSim->InverseDistanceUnits();
	m_fltStiffness *= lpSim->InverseMassUnits();
	m_fltDamping = m_fltDamping/lpSim->DensityMassUnits();

	Std_IsAboveMin((float) 0, m_fltNaturalLength, TRUE, "NaturalLength");
	Std_IsAboveMin((float) 0, m_fltStiffness, TRUE, "Stiffness");
	Std_IsAboveMin((float) 0, m_fltDamping, TRUE, "Damping", TRUE);

	m_bInitEnabled = m_bEnabled;

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
