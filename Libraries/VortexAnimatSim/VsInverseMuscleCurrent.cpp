// VsInverseMuscleCurrent.cpp: implementation of the VsInverseMuscleCurrent class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include <iostream>

#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"

#include "VsInverseMuscleCurrent.h"
#include "VsDragger.h"


namespace VortexAnimatSim
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsInverseMuscleCurrent::VsInverseMuscleCurrent()
{
	m_lpOrganism = NULL;
	m_lpNode = NULL;
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

VsInverseMuscleCurrent::~VsInverseMuscleCurrent()
{

try
{
	m_lpOrganism = NULL;
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	m_lpMuscle = NULL;
	m_aryTime.RemoveAll();
	m_aryLength.RemoveAll();
	m_aryVelocity.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsInverseMuscleCurrent\r\n", "", -1, FALSE, TRUE);}
}

void VsInverseMuscleCurrent::Initialize()
{
	if(!m_lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	ExternalStimulus::Initialize();

	//Lets try and get the node we will dealing with.
	m_lpOrganism = m_lpSim->FindOrganism(m_strOrganismID);
	m_lpNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpExternalCurrent = m_lpNode->GetDataPointer("ExternalCurrent");

	if(!m_lpExternalCurrent)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Stimulus: " + m_strID + " OrganismID: " + m_strOrganismID + " NeuralModule: " +  
 		 m_strNeuralModule + "Node: " + m_strTargetNodeID + " DataType: ExternalCurrent"));


	//Tells how many time steps it takes before we do this stimulus.
	//The neural stuff can have faster time steps than the physics engine.
	//We only need to update this guy when the physics engine steps though.
	m_iStepInterval = m_lpSim->PhysicsTimeStep()/m_lpSim->TimeStep();

	m_lpMuscle = dynamic_cast<LinearHillMuscle *>( m_lpOrganism->FindRigidBody(m_strMuscleID));
}

void VsInverseMuscleCurrent::Activate()
{
	ExternalStimulus::Activate();

	m_iIndex = 0;

	if(m_lpMuscle)
	{
		m_fltT = m_lpMuscle->Tension();
	}
}
 
void VsInverseMuscleCurrent::StepSimulation()
{
	if(m_iIndex < m_aryTime.GetSize())
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

void VsInverseMuscleCurrent::Deactivate()
{		
	ExternalStimulus::Deactivate();
	*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltCurrent;
}

float *VsInverseMuscleCurrent::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

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

void VsInverseMuscleCurrent::Load(CStdXml &oXml)
{		
	oXml.IntoElem();  //Into Item Element

	m_strID = Std_CheckString(oXml.GetChildString("ID"));
	if(Std_IsBlank(m_strID))
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strName = oXml.GetChildString("Name", "");

	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);

	m_bAlwaysActive = oXml.GetChildBool("AlwaysActive", m_bAlwaysActive);

	m_strNeuralModule = oXml.GetChildString("NeuralModuleName");
	if(Std_IsBlank(m_strNeuralModule)) 
		THROW_ERROR(Al_Err_lNeuralModuleNameBlank, Al_Err_strNeuralModuleNameBlank);

	m_strOrganismID = oXml.GetChildString("OrganismID");

	if(Std_IsBlank(m_strOrganismID))
		THROW_PARAM_ERROR(Al_Err_lOrganismIDBlank, Al_Err_strOrganismIDBlank, "ID", m_strID);

	m_strTargetNodeID = oXml.GetChildString("TargetNodeID");

	m_strMuscleID = oXml.GetChildString("MuscleID");

	if(Std_IsBlank(m_strMuscleID))
		THROW_TEXT_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank, "Muscle ID is missing.");

	m_aryTime.RemoveAll();
	m_aryLength.RemoveAll();
	m_aryVelocity.RemoveAll();

	m_strMuscleLengthData = oXml.GetChildString("LengthData");

	//Now load the muscle data
	m_strMuscleLengthData = AnimatSim::GetFilePath(m_lpSim->ProjectPath(), m_strMuscleLengthData);
	LoadMuscleData(m_strMuscleLengthData);

	if(m_aryTime.GetSize() < 2)
		THROW_PARAM_ERROR(Vs_Err_lMuscleLengthDataEmpty, Vs_Err_strMuscleLengthDataEmpty, "File", m_strMuscleLengthData);

	//Get the time step used in the data
	float fltStep = m_aryTime[1] - m_aryTime[0];

	if(fabs(fltStep - m_lpSim->PhysicsTimeStep()) > 1e-3)
		THROW_TEXT_ERROR(Vs_Err_lMuscleLengthTimeStep, Vs_Err_strMuscleLengthTimeStep, " File Time Step: " + STR(fltStep) + " Physics Time Step: " + STR(m_lpSim->PhysicsTimeStep()) );

	//Set the start and end times using the data file
	m_bLoadedTime = TRUE;
	m_fltStartTime = m_aryTime[0];
	m_fltEndTime = m_aryTime[m_aryTime.GetSize()-1];

	m_lStartSlice = (long) (m_fltStartTime / m_lpSim->TimeStep() + 0.5);
	m_lEndSlice = (long) (m_fltEndTime / m_lpSim->TimeStep() + 0.5);

	m_fltConductance = oXml.GetChildFloat("Conductance", m_fltConductance);
	Std_IsAboveMin((float) 0, m_fltConductance, FALSE, "Conductance");

	m_fltRestPotential = oXml.GetChildFloat("RestPotential", m_fltRestPotential);
	Std_IsAboveMin((float) 0, m_fltRestPotential, FALSE, "RestPotential");

	oXml.OutOfElem(); //OutOf Simulus Element

	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);
}

void VsInverseMuscleCurrent::LoadMuscleData(string strFilename)
{
	ifstream fsFile(strFilename.c_str());

	if(fsFile.fail())
		THROW_TEXT_ERROR(Vs_Err_lOpenFile, Vs_Err_strOpenFile, "File: " + strFilename);

	//Read off the top column name line
	CStdArray<string> aryParts;
	char sLine[300];
	fsFile.getline(sLine, 300);

	Std_Split(sLine, "\t", aryParts);
	if(aryParts.GetSize() != 3)
		THROW_PARAM_ERROR(Vs_Err_lInvalidMuscleLengthCols, Vs_Err_strInvalidMuscleLengthCols, "Col Size", aryParts.GetSize());

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




