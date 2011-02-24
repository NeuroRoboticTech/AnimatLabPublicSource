// Organism.cpp: implementation of the Organism class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
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
#include "Attachment.h"
#include "Structure.h"
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

/*! \brief 
   Constructs an organism object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a organism. 
*/

Organism::Organism()
{
	m_bDead = FALSE;
	m_lpNervousSystem = NULL;
}

/*! \brief 
   Destroys the organism object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the organism object..	 
*/

Organism::~Organism()
{

try
{
	if(m_lpNervousSystem) 
		{
			delete m_lpNervousSystem; 
			m_lpNervousSystem = NULL;
	}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Organism\r\n", "", -1, FALSE, TRUE);}
}

void Organism::Kill(Simulator *lpSim, BOOL bState)
{
	m_bDead = bState;
	m_lpBody->Kill(lpSim, this, bState);
	m_lpNervousSystem->Kill(lpSim, this, bState);
}

void Organism::Initialize(Simulator *lpSim)
{
	Structure::Initialize(lpSim);

	m_lpNervousSystem->Initialize(lpSim, this);
}

void Organism::ResetSimulation(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimNotDefined, Al_Err_strSimNotDefined);

	if(m_lpBody)
	{
		m_lpBody->ResetSimulation(lpSim, this);
		
		CollectStructureData(lpSim);
	}

	m_lpNervousSystem->ResetSimulation(lpSim, this);

	//We have to call this after method because some objects (ie: muscles and spindles, etc.) depend on other items
	//already being reset to their original positions. So they must be done first and then these items get reset.
	if(m_lpBody)
		m_lpBody->AfterResetSimulation(lpSim, this);
}

void Organism::StepNeuralEngine(Simulator *lpSim)
{
	if(!m_bDead)
		m_lpNervousSystem->StepSimulation(lpSim, this);
}

void Organism::StepPhysicsEngine(Simulator *lpSim)
{
	if(m_lpBody)
		m_lpBody->StepSimulation(lpSim, this);
}



#pragma region DataAccesMethods

float *Organism::GetDataPointer(string &strID, string &strDataType)
{
	RigidBody *lpBody = dynamic_cast<RigidBody *>(FindRigidBody(strID, FALSE));

	if(!lpBody)
	{
		Joint *lpJoint = dynamic_cast<Joint *>(FindJoint(strID, FALSE));
		
		if(!lpJoint)
			THROW_PARAM_ERROR(Al_Err_lBodyOrJointIDNotFound, Al_Err_strBodyOrJointIDNotFound, "ID", strID);

		return lpJoint->GetDataPointer(strDataType);		
	}
	else
		return lpBody->GetDataPointer(strDataType);
}

BOOL Organism::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	//if(strType == "TIMESTEP")
	//{
	//	TimeStep(atof(strValue.c_str()));
	//	return TRUE;
	//}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

BOOL Organism::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "NEURALMODULE")
	{
		try
		{
			m_lpNervousSystem->AddNeuralModule(GetSimulator(), this, strXml);
			return TRUE;
		}
		catch(CStdErrorInfo oError)
		{
			if(bThrowError)
				RELAY_ERROR(oError);
		}
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL Organism::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "NEURALMODULE")
	{
		try
		{
			m_lpNervousSystem->RemoveNeuralModule(GetSimulator(), this, strID);
			return TRUE;
		}
		catch(CStdErrorInfo oError)
		{
			if(bThrowError)
				RELAY_ERROR(oError);
		}
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

AnimatSim::Behavior::NervousSystem *Organism::NervousSystem()
{return m_lpNervousSystem;}

long Organism::CalculateSnapshotByteSize()
{return m_lpNervousSystem->CalculateSnapshotByteSize();}

void Organism::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{m_lpNervousSystem->SaveKeyFrameSnapshot(aryBytes, lIndex);}

void Organism::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{m_lpNervousSystem->LoadKeyFrameSnapshot(aryBytes, lIndex);}

void Organism::Load(Simulator *lpSim, CStdXml &oXml)
{
	Structure::Load(lpSim, oXml);

	oXml.IntoElem();  //Into Layout Element

	//dwc convert. Need to have a method to remove a nervous system. It needs to remove any added
	//modules from the list in the simulator.
	if(m_lpNervousSystem) {delete m_lpNervousSystem; m_lpNervousSystem = NULL;}
	m_lpNervousSystem = new AnimatSim::Behavior::NervousSystem;

	oXml.IntoChildElement("NervousSystem");
	m_lpNervousSystem->Load(lpSim, this, oXml);
	oXml.OutOfElem();

	oXml.OutOfElem(); //OutOf Layout Element
}
