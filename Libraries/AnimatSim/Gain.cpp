// Gain.cpp: implementation of the Gain class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatSim
{
	namespace Gains
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

Gain::Gain()
{
	m_bUseLimits = FALSE;
	m_fltLowerLimit = 0;
	m_fltLowerOutput = 0;
	m_fltUpperLimit = 0;
	m_fltUpperOutput = 0;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

Gain::~Gain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Gain\r\n", "", -1, FALSE, TRUE);}
}

void Gain::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	m_bUseLimits = oXml.GetChildBool("UseLimits", m_bUseLimits);

	if(m_bUseLimits)
	{
		m_fltLowerLimit = oXml.GetChildFloat("LowerLimit");
		m_fltLowerOutput = oXml.GetChildFloat("LowerOutput");
		m_fltUpperLimit = oXml.GetChildFloat("UpperLimit");
		m_fltUpperOutput = oXml.GetChildFloat("UpperOutput");
		
		Std_IsAboveMin(m_fltLowerLimit, m_fltUpperLimit, TRUE, "UpperLimit");
	}

	oXml.OutOfElem(); //OutOf Adapter Element
}

Gain ANIMAT_PORT *LoadGain(string strName, CStdXml &oXml)
{
	Gain *lpGain = NULL;

	try
	{
		//Now lets load this Current Graph object.
		oXml.IntoChildElement(strName);
		string strModuleName = oXml.GetChildString("ModuleName", "");
		string strType = oXml.GetChildString("Type");
		oXml.OutOfElem(); //OutOf Gain Element

		Simulator *lpSim = GetSimulator();
		if(!lpSim)
			THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

		Gain *lpGain = dynamic_cast<AnimatSim::Gains::Gain *>(lpSim->CreateObject(strModuleName, "Gain", strType));
		if(!lpGain)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "CurrentGraph");

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
