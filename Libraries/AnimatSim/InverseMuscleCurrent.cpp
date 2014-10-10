// InverseMuscleCurrent.cpp: implementation of the InverseMuscleCurrent class.
//
//////////////////////////////////////////////////////////////////////
#include "StdAfx.h"
#include <iostream>
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
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
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

#include "LineBase.h"
#include "Gain.h"
#include "SigmoidGain.h"
#include "LengthTensionGain.h"
#include "MuscleBase.h"
#include "LinearHillMuscle.h"

#include "InverseMuscleCurrent.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

InverseMuscleCurrent::InverseMuscleCurrent()
{
	m_lpTargetNode = NULL;
	m_lpExternalCurrent = NULL;
	m_lpMuscle = NULL;
	m_fltCurrent = 0;
	m_fltPrevCurrent = 0;
	m_fltOffset = 0;
	m_fltT = 0;
	m_iIndex = 0;
	m_fltLength = 0;
	m_fltVelocity = 0;
	m_fltVm = 0;
	m_fltA = 0;
	m_fltConductance = 100e-9f;
	m_fltRestPotential = -100e-3f;
}

InverseMuscleCurrent::~InverseMuscleCurrent()
{

try
{
	m_lpExternalCurrent = NULL;
	m_lpMuscle = NULL;
	m_aryTime.RemoveAll();
	m_aryLength.RemoveAll();
	m_aryVelocity.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of InverseMuscleCurrent\r\n", "", -1, false, true);}
}


void InverseMuscleCurrent::RestPotential(float fltV)
{
	Std_IsAboveMin((float) 0, fltV, false, "RestPotential");

	m_fltRestPotential = fltV;
}

float InverseMuscleCurrent::RestPotential() {return m_fltRestPotential;}

void InverseMuscleCurrent::Conductance(float fltG)
{
	Std_IsAboveMin((float) 0, fltG, false, "Conductance");

	m_fltConductance = fltG;
}

float InverseMuscleCurrent::Conductance() {return m_fltConductance;}

void InverseMuscleCurrent::MuscleID(std::string strID)
{
	m_strMuscleID = strID;

	if(Std_IsBlank(strID))
		m_lpMuscle = NULL;
	else
		m_lpMuscle = dynamic_cast<LinearHillMuscle *>( m_lpSim->FindByID(m_strMuscleID));
}

std::string InverseMuscleCurrent::MuscleID() {return m_strMuscleID;}

LinearHillMuscle *InverseMuscleCurrent::Muscle() {return m_lpMuscle;}

void InverseMuscleCurrent::MuscleLengthData(std::string strFilename)
{
	m_strMuscleLengthData = strFilename;

	m_aryTime.RemoveAll();
	m_aryLength.RemoveAll();
	m_aryVelocity.RemoveAll();

	if(!Std_IsBlank(m_strMuscleLengthData))
	{
		//Now load the muscle data
		m_strMuscleLengthData = AnimatSim::GetFilePath(m_lpSim->ProjectPath(), m_strMuscleLengthData);
		LoadMuscleData(m_strMuscleLengthData);

		if(m_aryTime.GetSize() < 2)
			THROW_PARAM_ERROR(Al_Err_lMuscleLengthDataEmpty, Al_Err_strMuscleLengthDataEmpty, "File", m_strMuscleLengthData);

		//Get the time step used in the data
		float fltStep = m_aryTime[1] - m_aryTime[0];

		if(fabs(fltStep - m_lpSim->PhysicsTimeStep()) > 1e-3)
			THROW_TEXT_ERROR(Al_Err_lMuscleLengthTimeStep, Al_Err_strMuscleLengthTimeStep, " File Time Step: " + STR(fltStep) + " Physics Time Step: " + STR(m_lpSim->PhysicsTimeStep()) );

		//Set the start and end times using the data file
		m_bLoadedTime = true;
		m_fltStartTime = m_aryTime[0];
		m_fltEndTime = m_aryTime[m_aryTime.GetSize()-1];

		m_lStartSlice = (long) (m_fltStartTime / m_lpSim->TimeStep() + 0.5);
		m_lEndSlice = (long) (m_fltEndTime / m_lpSim->TimeStep() + 0.5);
	}
}

std::string InverseMuscleCurrent::MuscleLengthData() {return m_strMuscleLengthData;}

void InverseMuscleCurrent::TargetNodeID(std::string strID)
{
	if(Std_IsBlank(strID))
		THROW_TEXT_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank, "Muscle ID is missing.");

	m_strTargetNodeID = strID;
}

std::string InverseMuscleCurrent::TargetNodeID() {return m_strTargetNodeID;}

Node *InverseMuscleCurrent::TargetNode() {return m_lpTargetNode;}

void InverseMuscleCurrent::Initialize()
{
	ExternalStimulus::Initialize();

	//Lets try and get the node we will dealing with.
	m_lpTargetNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpTargetNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpExternalCurrent = m_lpTargetNode->GetDataPointer("ExternalCurrent");

	if(!m_lpExternalCurrent)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Stimulus: " + m_strID  + "Node: " + m_strTargetNodeID + " DataType: ExternalCurrent"));


	//Tells how many time steps it takes before we do this stimulus.
	//The neural stuff can have faster time steps than the physics engine.
	//We only need to update this guy when the physics engine steps though.
	m_iStepInterval = (int) (m_lpSim->PhysicsTimeStep()/m_lpSim->TimeStep() + 0.5);

	if(!Std_IsBlank(m_strMuscleID))
		m_lpMuscle = dynamic_cast<LinearHillMuscle *>( m_lpSim->FindByID(m_strMuscleID));

	m_lStartSlice = (long) (m_fltStartTime / m_lpSim->TimeStep() + 0.5);
	m_lEndSlice = (long) (m_fltEndTime / m_lpSim->TimeStep() + 0.5);
}

void InverseMuscleCurrent::ResetSimulation()
{
	m_fltCurrent = 0;
	m_fltPrevCurrent = 0;
	m_fltOffset = 0;
	m_fltT = 0;
	m_iIndex = 0;
	m_fltLength = 0;
	m_fltVelocity = 0;
	m_fltVm = 0;
	m_fltA = 0;
}

void InverseMuscleCurrent::Activate()
{
	ExternalStimulus::Activate();

	m_iIndex = 0;

	if(m_lpMuscle)
	{
		m_fltT = m_lpMuscle->Tension();
	}
}

void InverseMuscleCurrent::StepSimulation()
{
	if(m_lpMuscle && m_iIndex < m_aryTime.GetSize())
	{
		float fltTime = m_aryTime[m_iIndex];
		float fltTime1 = m_lpSim->Time();

		//if(fltTime >= 2.03)
		//	fltTime = fltTime;

		m_fltLength = m_aryLength[m_iIndex];
		m_fltVelocity = m_aryVelocity[m_iIndex];

		//First calculate the active tension required.
		m_lpMuscle->CalculateInverseDynamics(m_fltLength, m_fltVelocity, m_fltT, m_fltVm, m_fltA);
		m_fltVm = m_fltVm - m_fltRestPotential;

		m_fltPrevCurrent = m_fltCurrent;
		m_fltCurrent = m_fltVm*m_fltConductance;
		m_fltOffset = m_fltCurrent - m_fltPrevCurrent;

		*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltOffset;

		m_iIndex++;
	}

}

void InverseMuscleCurrent::Deactivate()
{		
	ExternalStimulus::Deactivate();

	if(m_lpMuscle)
		*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltCurrent;
}

float *InverseMuscleCurrent::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	if(strType == "A")
		lpData = &m_fltA;
	else if(strType == "VM")
		lpData = &m_fltVm;
	else if(strType == "CURRENT")
		lpData = &m_fltCurrent;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
}

bool InverseMuscleCurrent::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(ExternalStimulus::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "RESTPOTENTIAL")
	{
		RestPotential((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "CONDUCTANCE")
	{
		Conductance((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "MUSCLEID")
	{
		MuscleID(strValue);
		return true;
	}

	if(strDataType == "MUSCLELENGTHDATA")
	{
		MuscleLengthData(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void InverseMuscleCurrent::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	ExternalStimulus::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("A", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Vm", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Current", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("RestPotential", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Conductance", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MuscleID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MuscleLengthData", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

void InverseMuscleCurrent::Load(CStdXml &oXml)
{		
	VerifySystemPointers();

	oXml.IntoElem();  //Into Item Element

	m_strID = Std_CheckString(oXml.GetChildString("ID"));
	if(Std_IsBlank(m_strID))
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strName = oXml.GetChildString("Name", "");

	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);

	TargetNodeID(oXml.GetChildString("TargetNodeID"));
	AlwaysActive(oXml.GetChildBool("AlwaysActive", m_bAlwaysActive));
	Enabled(oXml.GetChildBool("Enabled", m_bEnabled));
	MuscleID(oXml.GetChildString("MuscleID", ""));
	MuscleLengthData(oXml.GetChildString("LengthData", ""));
	Conductance(oXml.GetChildFloat("Conductance", m_fltConductance));
	RestPotential(oXml.GetChildFloat("RestPotential", m_fltRestPotential));

	oXml.OutOfElem(); //OutOf Simulus Element
}

void InverseMuscleCurrent::LoadMuscleData(std::string strFilename)
{
	std::ifstream fsFile(strFilename.c_str());

	if(fsFile.fail())
		THROW_TEXT_ERROR(Al_Err_lOpenFile, Al_Err_strOpenFile, "File: " + strFilename);

	//Read off the top column name line
	CStdArray<std::string> aryParts;
	char sLine[300];
	fsFile.getline(sLine, 300);

	Std_Split(sLine, "\t", aryParts);
	if(aryParts.GetSize() != 3)
		THROW_PARAM_ERROR(Al_Err_lInvalidMuscleLengthCols, Al_Err_strInvalidMuscleLengthCols, "Col Size", aryParts.GetSize());

	float fltTime=0, fltLength=0, fltVelocity=0;
	while(!fsFile.eof())
	{
		fsFile.getline(sLine, 300);
		
		Std_Split(sLine, "\t", aryParts);

		if(aryParts.GetSize() == 3)
		{
			fltTime = atof(aryParts[0].c_str());
			fltLength = atof(aryParts[1].c_str());
			fltVelocity = atof(aryParts[2].c_str());

			m_aryTime.Add(fltTime);
			m_aryLength.Add(fltLength);
			m_aryVelocity.Add(fltVelocity);
		}
	}
}

	}			//ExternalStimuli
}				//VortexAnimatSim




