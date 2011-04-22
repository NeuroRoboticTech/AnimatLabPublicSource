/**
\file	ExternalStimulus.cpp

\brief	Implements the external stimulus class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include "Gain.h"
#include "Node.h"
#include "IPhysicsBase.h"
#include "IPhysicsBody.h"
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
#include "ExternalStimulus.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
ExternalStimulus::ExternalStimulus()
{
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
ExternalStimulus::~ExternalStimulus()
{
}

BOOL ExternalStimulus::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(ActivatedItem::SetData(strDataType, strValue, bThrowError))
	{
		m_lpSim->ExternalStimuliMgr()->ReInitialize();
		return TRUE;
	}

	//Value type tells whether this is using an equation or constant. This is determined in the
	//derived class. Lets set this to true here just so we do not generate an exception.
	if(strType == "VALUETYPE")
		return TRUE;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

BOOL ExternalStimulus::operator<(ActivatedItem *lpItem)
{
	ExternalStimulus *lpStimulus = dynamic_cast<ExternalStimulus *>(lpItem);

	if(!lpStimulus)
		THROW_ERROR(Al_Err_lItemNotStimulusType, Al_Err_strItemNotStimulusType);

	if(m_lStartSlice < lpStimulus->m_lStartSlice)
		return TRUE;

	if( (m_lStartSlice == lpStimulus->m_lStartSlice) && (m_lEndSlice < lpStimulus->m_lEndSlice))
		return TRUE;

	return FALSE;
}

	}			//ExternalStimuli
}				//AnimatSim
