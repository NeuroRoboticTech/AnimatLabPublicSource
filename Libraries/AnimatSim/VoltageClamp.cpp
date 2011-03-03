// VoltageClamp.cpp: implementation of the VoltageClamp class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
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
#include "ExternalStimulus.h"
#include "ExternalInputStimulus.h"
#include "VoltageClamp.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VoltageClamp::VoltageClamp()
{
	m_lpOrganism = NULL;
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	m_lpTotalCurrent = NULL;
	m_lpVrest = NULL;
	m_lpGm = NULL;
	m_fltVtarget = -0.70f;
	m_fltActiveCurrent = 0;
	m_fltTargetCurrent = 0;
}

VoltageClamp::~VoltageClamp()
{

try
{
	m_lpOrganism = NULL;
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	m_lpTotalCurrent = NULL;
	m_lpVrest = NULL;
	m_lpGm = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VoltageClamp\r\n", "", -1, FALSE, TRUE);}
}

void VoltageClamp::Vtarget(float fltVal)
{
	m_fltVtarget = fltVal;
	m_fltTargetCurrent = (m_fltVtarget - *m_lpVrest)*(*m_lpGm);
}

void VoltageClamp::Initialize(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	ExternalStimulus::Initialize(lpSim);

	//Lets try and get the node we will dealing with.
	m_lpOrganism = lpSim->FindOrganism(m_strOrganismID);
	m_lpNode = dynamic_cast<Node *>(lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpExternalCurrent = m_lpNode->GetDataPointer("ExternalCurrent");
	m_lpTotalCurrent = m_lpNode->GetDataPointer("TotalCurrent");
	m_lpVrest = m_lpNode->GetDataPointer("Vrest");
	m_lpGm = m_lpNode->GetDataPointer("Gm");

	//Calculate the target current to keep the voltage at the target level.
	m_fltTargetCurrent = (m_fltVtarget - *m_lpVrest)*(*m_lpGm);

	if(!m_lpExternalCurrent)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Stimulus: " + m_strID + " OrganismID: " + m_strOrganismID + " NeuralModule: " +  
 		 m_strNeuralModule + "Node: " + m_strTargetNodeID + " DataType: ExternalCurrent"));
}

void VoltageClamp::Activate(Simulator *lpSim)
{
	ExternalStimulus::Activate(lpSim);

	m_fltActiveCurrent = m_fltTargetCurrent - (*m_lpTotalCurrent);
	*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
}

void VoltageClamp::StepSimulation(Simulator *lpSim)
{
	m_fltActiveCurrent = m_fltTargetCurrent - (*m_lpTotalCurrent);
	*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
}

void VoltageClamp::Deactivate(Simulator *lpSim)
{		
	ExternalStimulus::Deactivate(lpSim);
	//*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
}

void VoltageClamp::ResetSimulation(Simulator *lpSim)
{
	ExternalStimulus::ResetSimulation(lpSim);
	
	m_fltActiveCurrent = 0;
}

float *VoltageClamp::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "CLAMPCURRENT")
		lpData = &m_fltActiveCurrent;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 

BOOL VoltageClamp::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "VTARGET")
	{
		Vtarget(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void VoltageClamp::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	m_strNeuralModule = oXml.GetChildString("ModuleName");
	if(Std_IsBlank(m_strNeuralModule)) 
		THROW_ERROR(Al_Err_lNeuralModuleNameBlank, Al_Err_strNeuralModuleNameBlank);

	m_strOrganismID = oXml.GetChildString("OrganismID");

	if(Std_IsBlank(m_strOrganismID))
		THROW_PARAM_ERROR(Al_Err_lOrganismIDBlank, Al_Err_strOrganismIDBlank, "ID", m_strID);

	m_strTargetNodeID = oXml.GetChildString("TargetNodeID");
	if(Std_IsBlank(m_strTargetNodeID)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Target ID");

	m_fltVtarget = oXml.GetChildFloat("Vtarget", m_fltVtarget);

	oXml.OutOfElem(); //OutOf Simulus Element
}

	}			//ExternalStimuli
}				//VortexAnimatSim




