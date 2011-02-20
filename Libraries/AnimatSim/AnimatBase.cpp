// AnimatBase.cpp: implementation of the AnimatBase class.
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

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

AnimatBase::AnimatBase()
{
	m_strID = Std_CreateAppID();
	m_bSelected = FALSE;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

AnimatBase::~AnimatBase()
{

try
{
	//This will remove this object from the object list of the simulation.
	Simulator *lpSim = GetSimulator();
	if(lpSim)
		lpSim->RemoveFromObjectList(this);
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of AnimatBase\r\n", "", -1, FALSE, TRUE);}
}

void AnimatBase::ID(string strID) 
{
	if(Std_IsBlank(strID))
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strID = Std_CheckString(strID);
}

#pragma region DataAccesMethods

float *AnimatBase::GetDataPointer(string strDataType)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, ("ID: " + m_strID + " Name: " + m_strName));
	return NULL;
}

BOOL AnimatBase::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(strDataType == "NAME")
		Name(strValue);

	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, ("ID: " + m_strID + " Name: " + m_strName));
	return FALSE;
}

BOOL AnimatBase::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lItemTypeInvalid, Al_Err_strItemTypeInvalid, ("ID: " + m_strID + " ItemType: " + strItemType));
	return FALSE;
}

BOOL AnimatBase::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lItemNotFound, Al_Err_strItemNotFound, ("ID: " + m_strID));
	return FALSE;
}

#pragma endregion

void AnimatBase::Load(CStdXml &oXml)
{
	oXml.IntoElem();
	m_strType = oXml.GetChildString("Type", m_strType);
	m_strID = Std_CheckString(oXml.GetChildString("ID", m_strID));
	m_strName = oXml.GetChildString("Name", m_strName);
	oXml.OutOfElem();

	if(Std_IsBlank(m_strID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	if(Std_IsBlank(m_strName)) 
		m_strName = m_strID;

	//This will add this object to the object list of the simulation.
	Simulator *lpSim = GetSimulator();
	if(lpSim)
		lpSim->AddToObjectList(this);
}


}			//AnimatSim
