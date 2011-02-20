// Node.cpp: implementation of the Node class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
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

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

Node::Node()
{
	m_lpSim = NULL;
	m_lpStructure = NULL;
	m_lpModule = NULL;

	m_bEnabledMem = TRUE;
	m_bEnabled = TRUE;
	m_fltEnabled = 0;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

Node::~Node()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Node\r\n", "", -1, FALSE, TRUE);}
}

void Node::AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter)
{
	lpSim->AttachSourceAdapter(lpStructure, lpAdapter);
}

void Node::AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter)
{
	lpSim->AttachTargetAdapter(lpStructure, lpAdapter);
}

void Node::Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState)
{
	if(bState)
	{
		m_bEnabledMem = m_bEnabled;
		Enabled(FALSE);
	}
	else
		Enabled(m_bEnabledMem);

}

void *Node::GetDataItem(string strItemType, string strID, BOOL bThrowError) 
{
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return NULL;
}

void Node::UpdateData(Simulator *lpSim, Structure *lpStructure)
{}

//Sometimes it is necessary to set the system pointers before Initialize is called. (eg. in load). This allows us to do that.
void Node::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule)
{
	m_lpSim = lpSim;
	m_lpStructure = lpStructure;
	m_lpModule = lpModule;
}

void Node::Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule)
{
	m_lpSim = lpSim;
	m_lpStructure = lpStructure;
	m_lpModule = lpModule;
}

void Node::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	AnimatBase::Load(oXml);
}


}			//AnimatSim
