// Simulator.cpp: implementation of the Simulator class.
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

namespace AnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Simulator *g_lpSimulator = NULL;
Simulator ANIMAT_PORT *GetSimulator() 
{return g_lpSimulator;};

/*! \brief 
   Constructs an simulator object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a simulator. 
*/

Simulator::Simulator()
{
	m_strID = "SIMULATOR";
	m_strName = m_strID;
	m_fltTime = 0;
	m_fltTimeStep = (float) 0.0025;
	m_iPhysicsStepInterval = 4;
	m_fltPhysicsTimeStep = (float) 0.01;
	m_lTimeSlice = 0;
	m_fltEndSimTime = -1;
	m_lEndSimTimeSlice = -1;
	m_iStartSimTick = 0;
	m_bStopSimulation = FALSE;
	m_bForceSimulationStop = FALSE;
    m_bBlockSimulation = FALSE;
	m_bSimBlockConfirm = FALSE;
	m_lPhysicsSliceCount = 0;
	m_lVideoSliceCount = 0;
	m_iPhysicsStepCount = 0;
	m_iFrameRate = 30;
 	m_fltFrameStep = (1/ (float) m_iFrameRate);
	m_iVideoLoops = 0;
	m_lpAnimatClassFactory = NULL;
	m_bSimulateHydrodynamics = FALSE;
	m_fltFluidDensity = 1.0;
	m_fltGravity = (float) -9.81;
	m_fltDistanceUnits = (float) 0.01;  //use centimeters
	m_fltInverseDistanceUnits = 1/m_fltDistanceUnits;
	m_fltDenominatorDistanceUnits = 1;
	m_fltMassUnits = (float) 0.001;    //use grams
	m_fltInverseMassUnits = 1/m_fltMassUnits;
	m_fltDensityMassUnits = 0.01f;
	m_fltMouseSpringStiffness = 25;
	m_ftlMouseSpringDamping = 2.8f;
	m_fltLinearCompliance = 1e-6f;
	m_fltAngularCompliance = 1e-6f;
	m_fltLinearDamping = 1e6f;
	m_fltAngularDamping = 1e6f;
	m_fltLinearKineticLoss = 1e6f;
	m_fltAngularKineticLoss = 1e6f;
	m_bForceFastMoving = TRUE;
	m_iSelectionMode = GRAPHICS_SELECTION_MODE;
	m_bAddBodiesMode = FALSE;

	m_bPaused = TRUE;
	m_bInitialized = FALSE;
	m_bSimRunning = FALSE;

	m_lpSelOrganism = NULL;
	m_lpSelStructure = NULL;
	m_bManualStepSimulation = FALSE;
	m_lpVideoRecorder = NULL;
	m_lpVideoPlayback = NULL;
	m_lpSimRecorder = NULL;
	m_lpSimStopPoint = NULL;
	m_bEnableSimRecording = FALSE;
	m_lSnapshotByteSize = 0;

	m_bHasConvexMesh = FALSE;
	m_bHasTriangleMesh = FALSE;
	m_bHasHeightField = FALSE;

	m_lpManagedInstance = NULL;
	m_lpUpdateDataCallback = NULL;
	m_lpStartSimulationCallback = NULL;
	m_lpPauseSimulationCallback = NULL;
	m_lpEndingSimulationCallback = NULL;

	m_bAutoGenerateRandomSeed = TRUE;
	m_iManualRandomSeed = 12345;

	m_bRecordVideo = FALSE;
	m_strVideoFilename = "Video.avi";
	m_fltVideoRecordFrameTime = 1e-3f;
	m_fltVideoPlaybackFrameTime = 100e-3f;
	m_fltVideoStartTime = 0;
	m_fltVideoEndTime = 1;
	m_iVideoStepSize = -1;
	m_iVideoStepSize = 0;
	m_lVideoStartSlice = -1;
	m_lVideoEndSlice = -1;
	m_lVideoFrame = 0;
	m_lpAvi = NULL;

	m_aviOpts.cbFormat = 0;
	m_aviOpts.cbParms = 4;
	m_aviOpts.dwBytesPerSecond = 0;
	m_aviOpts.dwFlags = 8;
	m_aviOpts.dwInterleaveEvery = 0;
	m_aviOpts.dwKeyFrameEvery = 0;
	m_aviOpts.dwQuality = 7500;
	m_aviOpts.fccHandler = 1668707181;
	m_aviOpts.fccType = 0;
	m_aviOpts.lpFormat = 0;
	m_aviOpts.lpParms = 0;

	m_vBackgroundColor[0] = 0.2f;
	m_vBackgroundColor[1] = 0.2f;
	m_vBackgroundColor[2] = 0.6f;
	m_vBackgroundColor[3] = 1;

	m_lpWinMgr = NULL;
}


/*! \brief 
   Destroys the simulator object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the simulator object..	 
*/

Simulator::~Simulator()
{

try
{
	g_lpSimulator = NULL;
	if(m_lpAnimatClassFactory) {delete m_lpAnimatClassFactory; m_lpAnimatClassFactory = NULL;}
	m_aryOrganisms.RemoveAll();
	m_aryStructures.RemoveAll();
	m_aryAllStructures.RemoveAll();
	if(m_lpSimRecorder)
	{
		delete m_lpSimRecorder;
		m_lpSimRecorder = NULL;
	}

	if(m_lpSimStopPoint)
	{
		delete m_lpSimStopPoint;
		m_lpSimStopPoint = NULL;
	}

	if(m_lpAvi)
	{
		delete m_lpAvi;
		m_lpAvi = NULL;
	}

	if(m_lpWinMgr)
	{
		m_lpWinMgr->Close();
		delete m_lpWinMgr;
		m_lpWinMgr = NULL;
	}

	m_aryNeuralModuleFactories.RemoveAll();

	m_arySourcePhysicsAdapters.RemoveAll();
	m_aryTargetPhysicsAdapters.RemoveAll();
	m_iTargetAdapterCount = 0;

	m_aryOdorTypes.RemoveAll();
	m_aryFoodSources.RemoveAll();	
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Simulator\r\n", "", -1, FALSE, TRUE);}
}

float Simulator::TimeStep() 
{return m_fltTimeStep;}

void Simulator::TimeStep(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "TimeStep");
	m_fltTimeStep = fltVal;

	//Find the number of timeslices that need to occur before the physics system is updated
	m_iPhysicsStepInterval = m_fltPhysicsTimeStep / m_fltTimeStep;

	//Now recaculate the physics time step using the minimum time step as the base.
	m_fltPhysicsTimeStep = m_fltTimeStep * m_iPhysicsStepInterval;
}

void Simulator::DistanceUnits(string strUnits)
{
	m_fltDistanceUnits = ConvertDistanceUnits(strUnits);
	m_fltInverseDistanceUnits = 1/m_fltDistanceUnits;
	m_fltDenominatorDistanceUnits = ConvertDenominatorDistanceUnits(strUnits);
}

void Simulator::MassUnits(string strUnits)
{
	m_fltMassUnits = ConvertMassUnits(strUnits);
	m_fltInverseMassUnits = 1/m_fltMassUnits;
	m_fltDensityMassUnits = ConvertDensityMassUnits(strUnits);
}

void Simulator::SimulateHydrodynamics(BOOL bVal)
{
	m_bSimulateHydrodynamics = bVal;
}

void Simulator::FluidDensity(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "FluidDensity");

	if(bUseScaling)
		fltVal *= pow(m_fltDenominatorDistanceUnits, 3);  //Perform a conversion if necessary because we may be using different units in the denominator.

	m_fltFluidDensity = fltVal;
}

void Simulator::Gravity(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Gravity");

	//We must convert the gravity to use the correct scale.
	if(bUseScaling)
		fltVal /= m_fltDistanceUnits;

	m_fltGravity = fltVal;
}

void Simulator::PhysicsTimeStep(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "PhysicsTimeStep");
	m_fltPhysicsTimeStep = fltVal;

	//If no timestep has been set then start the time step with the physics system and then later on we will find the 
	//real minimum value while looking at all of the neural modules.
	if(m_fltTimeStep < 0)
		m_fltTimeStep = fltVal;

	//Find the number of timeslices that need to occur before the physics system is updated
	m_iPhysicsStepInterval = fltVal / m_fltTimeStep;

	//Now recaculate the physics time step using the minimum time step as the base.
	m_fltPhysicsTimeStep = m_fltTimeStep * m_iPhysicsStepInterval;
}

void Simulator::FrameRate(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, TRUE, "FrameRate");

	m_iFrameRate = iVal;
 	m_fltFrameStep = (1/ (float) m_iFrameRate);
}

//CNlClassFactory *Simulator::NeuralClassFactory()
//{
//	if(!m_lpClassFactory)
//		THROW_ERROR(Al_Err_lClassFactoryNotDefined, Al_Err_strClassFactoryNotDefined);
//
//	return m_lpClassFactory;
//}


//CNlNeuron *Simulator::GetNeuron(string strOrganismID, short iXPos, 
//																	 short iYPos, short iZPos)
//{
//	Organism *lpOrganism = FindOrganism(strOrganismID);
//
//	//return lpOrganism->GetNeuron(iXPos, iYPos, iZPos);
//	return NULL;
//}

BOOL Simulator::IsPhysicsBeingUpdated()
{
	if(m_iPhysicsStepCount == m_iPhysicsStepInterval)
		return TRUE;
	else
		return FALSE;
}

void Simulator::VisualSelectionMode(int iVal)
{
	if(iVal <0 || iVal > SIMULATION_SELECTION_MODE)
		THROW_PARAM_ERROR(Al_Err_lInvalidSelMode, Al_Err_strInvalidSelMode, "Selection Mode", iVal);

	m_iSelectionMode = iVal;
	
	//Go through and call VisualSelectionModeChanged for all objects.
	CStdMap<string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase && lpBase != this)
			lpBase->VisualSelectionModeChanged(m_iSelectionMode);
	}
};

void Simulator::MouseSpringStiffness(float fltVal, BOOL bUseScaling) 
{
	if(bUseScaling)
		fltVal *= this->InverseMassUnits();

	m_fltMouseSpringStiffness = fltVal;
}

void Simulator::MouseSpringDamping(float fltVal, BOOL bUseScaling) 
{
	if(bUseScaling)
		fltVal = fltVal/this->DensityMassUnits();

	m_ftlMouseSpringDamping = fltVal;
}

/*! \brief 
   Resets all objects of the simulation.
      
	 \return
	 No return value.

	 \remarks
	 Use this function to completely reset a simulation to its initial default settings.
	 This will destroy all organisms and strcutres defined within the simulation

	 \sa
	 ~Simulator, LoadSimulation
*/

void Simulator::Reset()
{
	m_fltTime = 0;
	m_fltTimeStep = (float) 0.0025;
	m_iPhysicsStepInterval = 4;
	m_fltPhysicsTimeStep = (float) 0.01;
	m_iPhysicsStepCount = 0;
	m_lTimeSlice = 0;
	m_fltEndSimTime = -1;
	m_lPhysicsSliceCount = 0;
	m_iFrameRate = 30;
 	m_fltFrameStep = (1/ (float) m_iFrameRate);
	m_lVideoSliceCount = 0;
	m_iVideoLoops = 0;
	m_bSimulateHydrodynamics = FALSE;
	m_fltFluidDensity = 1.0;
	m_fltGravity = (float) -9.8;
	m_fltDistanceUnits = (float) 0.01;  //use centimeters
	m_fltInverseDistanceUnits = 1/m_fltDistanceUnits;
	m_fltDenominatorDistanceUnits = 1;
	m_fltMassUnits = (float) 0.001;    //use grams
	m_fltInverseMassUnits = 1/m_fltMassUnits;
	m_fltDensityMassUnits = 0.01f;
	m_fltMouseSpringStiffness = 25;
	m_ftlMouseSpringDamping = 2.8f;
	m_fltLinearCompliance = 1e-6f;
	m_fltAngularCompliance = 1e-6f;
	m_fltLinearDamping = 1e6f;
	m_fltAngularDamping = 1e6f;
	m_fltLinearKineticLoss = 1e6f;
	m_fltAngularKineticLoss = 1e6f;
	m_bForceFastMoving = TRUE;

	if(m_lpAnimatClassFactory) {delete m_lpAnimatClassFactory; m_lpAnimatClassFactory = NULL;}
	m_aryOrganisms.RemoveAll();
	m_aryStructures.RemoveAll();
	m_aryAllStructures.RemoveAll();
	m_lpVideoRecorder = NULL; //Do not delete this object. It is in the list of Keyframes.
	m_lpVideoPlayback = NULL; //Do not delete this object. It is in the list of Keyframes.
	m_bEnableSimRecording = FALSE;
	m_lSnapshotByteSize = 0;

	m_bHasConvexMesh = FALSE;
	m_bHasTriangleMesh = FALSE;
	m_bHasHeightField = FALSE;

	m_bPaused = TRUE;
	m_bInitialized = FALSE;
	m_bSimRunning = FALSE;

	m_bRecordVideo = FALSE;
	m_strVideoFilename = "Video.avi";
	m_fltVideoRecordFrameTime = 1e-3f;
	m_fltVideoPlaybackFrameTime = 100e-3f;
	m_fltVideoStartTime = 0;
	m_fltVideoEndTime = 1;
	m_iVideoStepSize = -1;
	m_iVideoStepSize = 0;
	m_lVideoStartSlice = -1;
	m_lVideoEndSlice = -1;
	m_lVideoFrame = 0;

	m_aviOpts.cbFormat = 0;
	m_aviOpts.cbParms = 4;
	m_aviOpts.dwBytesPerSecond = 0;
	m_aviOpts.dwFlags = 8;
	m_aviOpts.dwInterleaveEvery = 0;
	m_aviOpts.dwKeyFrameEvery = 0;
	m_aviOpts.dwQuality = 7500;
	m_aviOpts.fccHandler = 1668707181;
	m_aviOpts.fccType = 0;
	m_aviOpts.lpFormat = 0;
	m_aviOpts.lpParms = 0;

	m_oDataChartMgr.Reset();
	m_oExternalStimuliMgr.Reset();
	m_oMaterialMgr.Reset();
	if(m_lpSimRecorder)
	{
		delete m_lpSimRecorder;
		m_lpSimRecorder = NULL;
	}

	if(m_lpSimStopPoint)
	{
		delete m_lpSimStopPoint;
		m_lpSimStopPoint = NULL;
	}

	if(m_lpAvi)
	{
		delete m_lpAvi;
		m_lpAvi = NULL;
	}

	if(m_lpWinMgr)
		m_lpWinMgr->Close();

	m_aryNeuralModuleFactories.RemoveAll();

	m_arySourcePhysicsAdapters.RemoveAll();
	m_aryTargetPhysicsAdapters.RemoveAll();
	m_iTargetAdapterCount = 0;

	m_aryOdorTypes.RemoveAll();
	m_aryFoodSources.RemoveAll();

	//Reference pointers only
	m_lpSelOrganism = NULL;
	m_lpSelStructure = NULL;
}

//This resets the the simulation to its orginal settings at time 0
void Simulator::ResetSimulation()
{
	m_fltTime = 0;
	m_lTimeSlice = 0;
	m_lPhysicsSliceCount = 0;
	m_lVideoSliceCount = 0;
	m_iPhysicsStepCount = 0;
	m_bPaused = TRUE;
	m_bSimRunning = FALSE;

	CStdMap<string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->ResetSimulation(this);
	}

	int iSize = m_arySourcePhysicsAdapters.GetSize();
	for(int iIndex=0; iIndex<iSize; iIndex++)
		m_arySourcePhysicsAdapters[iIndex]->ResetSimulation(this, NULL);

	iSize = m_aryTargetPhysicsAdapters.GetSize();
	for(int iIndex=0; iIndex<iSize; iIndex++)
		m_aryTargetPhysicsAdapters[iIndex]->ResetSimulation(this, NULL);

	m_oDataChartMgr.ResetSimulation(this);
	m_oExternalStimuliMgr.ResetSimulation(this);
	
	if(m_lpSimRecorder)
		m_lpSimRecorder->ResetSimulation(this);
}

void Simulator::GenerateAutoSeed()
{
	SYSTEMTIME st;
	GetLocalTime(&st);

	m_iManualRandomSeed = (unsigned) (st.wSecond + st.wMilliseconds + Std_IRand(0, 1000));
	Std_SRand(m_iManualRandomSeed);
	srand(m_iManualRandomSeed);
}

#pragma region DataAccesMethods

/*! \brief 
   Finds an organism with the specified ID.
      
   \param strOrganismID The ID of the organism to find. This is not case sensitive.
	 \param bThrowError If this is TRUE and the ID is not found then an exception is
	               thrown. If this is FALSE and the ID is not found then NULL
								 is returned.

	 \return
	 Returns a pointer to the organism with the specified ID if one is found. If one
	 is not found then it will either throw an exception or return NULL depending on 
	 the value of bThrowError.

	 \remarks
   Finds an organism with the specified ID.

	 \sa
	 FindOrganism, FindStructure, FindStructureFromAll
*/

Organism *Simulator::FindOrganism(string strOrganismID, BOOL bThrowError)
{
	Organism *lpOrganism = NULL;
	CStdPtrMap<string, Organism>::iterator oPos;
	oPos = m_aryOrganisms.find(Std_CheckString(strOrganismID));

	if(oPos != m_aryOrganisms.end())
		lpOrganism =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lOrganismIDNotFound, Al_Err_strOrganismIDNotFound, "OrganismID", strOrganismID);

	return lpOrganism;
}


/*! \brief 
   Finds a structure with the specified ID.
      
   \param strStructureID The ID of the structure to find. This is not case sensitive.
	 \param bThrowError If this is TRUE and the ID is not found then an exception is
	               thrown. If this is FALSE and the ID is not found then NULL
								 is returned.

	 \return
	 Returns a pointer to the structure with the specified ID if one is found. If one
	 is not found then it will either throw an exception or return NULL depending on 
	 the value of bThrowError. This will only search the "static" structures. It will
	 not search the organisms also. If you want to find a structure regardless if it 
	 is an organism then you need to use FindStructureFromAll.

	 \remarks
   Finds a structure with the specified ID.

	 \sa
	 FindOrganism, FindStructure, FindStructureFromAll
*/

Structure *Simulator::FindStructure(string strStructureID, BOOL bThrowError)
{
	Structure *lpStructure = NULL;
	CStdPtrMap<string, Structure>::iterator oPos;
	oPos = m_aryStructures.find(Std_CheckString(strStructureID));

	if(oPos != m_aryStructures.end())
		lpStructure =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lStructureIDNotFound, Al_Err_strStructureIDNotFound, "StructureID", strStructureID);

	return lpStructure;
}

OdorType *Simulator::FindOdorType(string strOdorID, BOOL bThrowError)
{
	OdorType *lpOdorType = NULL;
	CStdPtrMap<string, OdorType>::iterator oPos;
	oPos = m_aryOdorTypes.find(Std_CheckString(strOdorID));

	if(oPos != m_aryOdorTypes.end())
		lpOdorType =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lOdorIDNotFound, Al_Err_strOdorIDNotFound, "OdorID", strOdorID);

	return lpOdorType;
}

/*! \brief 
   Finds a structure or organism with the specified ID.
      
   \param strStructureID The ID of the structure to find. This is not case sensitive.
	 \param bThrowError If this is TRUE and the ID is not found then an exception is
	               thrown. If this is FALSE and the ID is not found then NULL
								 is returned.

	 \return
	 Returns a pointer to the structure with the specified ID if one is found. If one
	 is not found then it will either throw an exception or return NULL depending on 
	 the value of bThrowError. This will search both the "static" structures and the
	 organisms for the specified ID.

	 \remarks
   Finds a structure with the specified ID.

	 \sa
	 FindOrganism, FindStructure, FindStructureFromAll
*/

Structure *Simulator::FindStructureFromAll(string strStructureID, BOOL bThrowError)
{
	Structure *lpStructure = NULL;
	CStdPtrMap<string, Structure>::iterator oPos;
	oPos = m_aryAllStructures.find(Std_CheckString(strStructureID));

	if(oPos != m_aryAllStructures.end())
		lpStructure =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lStructureIDNotFound, Al_Err_strStructureIDNotFound, "StructureID", strStructureID);

	return lpStructure;
}


/*! \brief 
   Finds a joint with the specified ID in the specified structure.
      
   \param strStructureID The ID of the structure to find. This is not case sensitive.
   \param strJointID The ID of the joint within that structure to find. This is not case sensitive.
	 \param bThrowError If this is TRUE and the ID's are not found then an exception is
	               thrown. If this is FALSE and the ID's are not found then NULL
								 is returned.

	 \return
	 Returns a pointer to the joint with the specified ID that is inside the specified 
	 structure if one is found. If either the structure or joint are not found
	 then it will either throw an exception or return NULL depending on 
	 the value of bThrowError. This uses the FindStructureFromAll method to
	 search both the "static" structures and the organisms for the specified 
	 StructureID.

	 \remarks
   Finds a joint within a structure with the specified ID.

	 \sa
	 FindOrganism, FindStructure, FindStructureFromAll, FindRigidBody, FindJoint
*/

Joint *Simulator::FindJoint(string strStructureID, string strJointID, BOOL bThrowError)
{
	Structure *lpStructure = FindStructureFromAll(strStructureID, bThrowError);

	if(lpStructure)
		return lpStructure->FindJoint(strJointID, bThrowError);
	else
		return NULL;
}


/*! \brief 
   Finds a rigid body with the specified ID in the specified structure.
      
   \param strStructureID The ID of the structure to find. This is not case sensitive.
   \param strBodyID The ID of the rigid body within that structure to find. This is not case sensitive.
	 \param bThrowError If this is TRUE and the ID's are not found then an exception is
	               thrown. If this is FALSE and the ID's are not found then NULL
								 is returned.

	 \return
	 Returns a pointer to the body with the specified ID that is inside the specified 
	 structure if one is found. If either the structure or body are not found
	 then it will either throw an exception or return NULL depending on 
	 the value of bThrowError. This uses the FindStructureFromAll method to
	 search both the "static" structures and the organisms for the specified 
	 StructureID.

	 \remarks
   Finds a rigid body within a structure with the specified ID.

	 \sa
	 FindOrganism, FindStructure, FindStructureFromAll, FindRigidBody, FindJoint
*/

RigidBody *Simulator::FindRigidBody(string strStructureID, string strBodyID, BOOL bThrowError)
{
	Structure *lpStructure = FindStructureFromAll(strStructureID, bThrowError);

	if(lpStructure)
		return lpStructure->FindRigidBody(strBodyID, bThrowError);
	else
		return NULL;
}


//ClassFactory methods.

IStdClassFactory *Simulator::FindNeuralModuleFactory(string strModuleName, BOOL bThrowError)
{
	IStdClassFactory *lpFactory = NULL;
	CStdMap<string, IStdClassFactory *>::iterator oPos;
	oPos = m_aryNeuralModuleFactories.find(Std_CheckString(strModuleName));

	if(oPos != m_aryNeuralModuleFactories.end())
		lpFactory =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lModuleNameNotFound, Al_Err_strModuleNameNotFound, "ModuleName", strModuleName);

	return lpFactory;
}

AnimatBase *Simulator::FindByID(string strID, BOOL bThrowError)
{
	AnimatBase *lpFind = NULL;
	CStdMap<string, AnimatBase *>::iterator oPos;
	oPos = m_aryObjectList.find(Std_CheckString(strID));

	if(oPos != m_aryObjectList.end())
		lpFind =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lIDNotFound, Al_Err_strIDNotFound, "ID", strID);

	return lpFind;
}

void Simulator::AddToObjectList(AnimatBase *lpItem)
{
	if(!FindByID(lpItem->ID(), FALSE))
		m_aryObjectList.Add(lpItem->ID(), lpItem);
}

void Simulator::RemoveFromObjectList(AnimatBase *lpItem)
{
	if(FindByID(lpItem->ID(), FALSE))
		m_aryObjectList.Remove(lpItem->ID());
}

float *Simulator::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "TIME")
		lpData = &m_fltTime;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);

	return lpData;
}

BOOL Simulator::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "VISUALSELECTIONMODE")
	{
		VisualSelectionMode(atoi(strValue.c_str()));
		return TRUE;
	}
	else if(strType == "ADDBODIESMODE")
	{
		AddBodiesMode(Std_ToBool(strValue));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

BOOL Simulator::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "STIMULUS")
		return m_oExternalStimuliMgr.AddStimulus(this, strXml);
	else if(strType == "DATACHART")
		return m_oDataChartMgr.AddDataChart(this, strXml);
	else if(strType == "STRUCTURE")
	{
		AddStructure(strXml);
		return TRUE;
	}
	else if(strType == "ORGANISM")
	{
		AddOrganism(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL Simulator::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "STIMULUS")
		return m_oExternalStimuliMgr.RemoveStimulus(this, strID);
	else if(strType == "DATACHART")
		return m_oDataChartMgr.RemoveDataChart(this, strID);
	else if(strType == "STRUCTURE")
	{
		RemoveStructure(strID);
		return TRUE;
	}
	else if(strType == "ORGANISM")
	{
		RemoveOrganism(strID);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

void Simulator::AddNeuralModuleFactory(string strModuleName, NeuralModule *lpModule)
{
	if(!lpModule->ClassFactory())
		THROW_PARAM_ERROR(Al_Err_lModuleClassFactoryNotDefined, Al_Err_strModuleClassFactoryNotDefined, "ModuleName", strModuleName);

	if(!FindNeuralModuleFactory(strModuleName, FALSE))
		m_aryNeuralModuleFactories.Add(Std_CheckString(strModuleName), lpModule->ClassFactory());
}

void Simulator::AttachSourceAdapter(Structure *lpStructure, Adapter *lpAdapter)
{
	string strModuleName = Std_CheckString(lpAdapter->SourceModule());

	//If no neural module name is specified then this must be getting attached to the physics engine.
	//Otherwise it gets attached to the specified neural module in an organism
	if(strModuleName == "" || strModuleName == "ANIMATLAB")
		m_arySourcePhysicsAdapters.Add(lpAdapter);
	else
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
		if(!lpOrganism)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

		NeuralModule *lpModule = lpOrganism->NervousSystem()->FindNeuralModule(strModuleName);
		lpModule->AttachSourceAdapter(lpAdapter);
	}
}

void Simulator::AttachTargetAdapter(Structure *lpStructure, Adapter *lpAdapter)
{
	string strModuleName = Std_CheckString(lpAdapter->TargetModule());

	//If no neural module name is specified then this must be getting attached to the physics engine.
	//Otherwise it gets attached to the specified neural module in an organism
	if(strModuleName == "" || strModuleName == "ANIMATLAB")
	{
		m_aryTargetPhysicsAdapters.Add(lpAdapter);
		m_iTargetAdapterCount = m_aryTargetPhysicsAdapters.GetSize();
	}
	else
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
		if(!lpOrganism)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

		NeuralModule *lpModule = lpOrganism->NervousSystem()->FindNeuralModule(strModuleName);
		lpModule->AttachTargetAdapter(lpAdapter);
	}
}

void Simulator::AddFoodSource(RigidBody *lpFood)
{
	m_aryFoodSources.Add(lpFood);
}

RigidBody *Simulator::FindClosestFoodSource(CStdFPoint &oMouthPos, float fltMinRadius)
{
	RigidBody *lpFood = NULL, *lpMinFood = NULL;
	float fltDist=0, fltMinDist=0;
	int iCount = m_aryFoodSources.GetSize();

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpFood = m_aryFoodSources[iIndex];
		fltDist = Std_CalculateDistance(oMouthPos, lpFood->GetCurrentPosition());

		if( (fltDist <= fltMinRadius) && ((fltDist < fltMinDist) || !lpMinFood))
		{
			fltMinDist = fltDist;
			lpMinFood = lpFood;
		}
	}

	return lpMinFood;
}

CStdSerialize *Simulator::CreateObject(string strModule, string strClassName, string strType, BOOL bThrowError)
{
	strModule = Std_CheckString(strModule);
	
	if(strModule == "" || strModule == "ANIMATLAB")
	{
		if(!m_lpAnimatClassFactory)
			THROW_ERROR(Al_Err_lClassFactoryNotDefined, Al_Err_strClassFactoryNotDefined);

		return m_lpAnimatClassFactory->CreateObject(strClassName, strType, bThrowError);
	}
	else
	{
		IStdClassFactory *lpFactory = FindNeuralModuleFactory(strModule, FALSE);

		if(lpFactory)
			return lpFactory->CreateObject(strClassName, strType, bThrowError);
		else
		{
			//Lets load the dynamic library and get a pointer to the class factory.
			lpFactory = LoadClassFactory(strModule);

			//Now create an instance of a neural module. There is only one type of 
			return lpFactory->CreateObject(strClassName, strType, bThrowError);
		}
	}

	return NULL;
}


/*! \brief 
   Enables collision between the past-in object and all rigid bodies of the simulator.
      
   \param lpBody  This is a pointer to the body to enable collisions on.

	 \return
	 No return value.

	 \remarks
	 This method enables collision responses between the rigid body being past
	 in and all rigid bodies in the simulator.

   \sa
   EnableCollision, DisableCollision	
*/

void Simulator::EnableCollisions(Structure *lpStruct, CStdPtrArray<CollisionPair> &m_aryCollisionList)
{
	//Now lets disable any collisions that have been added to the exclusion list.
	int iCount = m_aryCollisionList.GetSize();
	CollisionPair *lpPair = NULL;
	RigidBody *lpPart1=NULL, *lpPart2=NULL;

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair =  m_aryCollisionList[iIndex];
		lpPart1 = lpStruct->FindRigidBody(lpPair->m_strPart1ID);
		lpPart2 = lpStruct->FindRigidBody(lpPair->m_strPart2ID);
		
		lpPart1->EnableCollision(this, lpPart2);
	}	
}

void Simulator::EnableCollision(RigidBody *lpBody)
{
	CStdMap<string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->EnableCollision(this, lpBody);
	}
}

void Simulator::DisableCollisions(Structure *lpStruct, CStdPtrArray<CollisionPair> &m_aryCollisionList)
{
	//Now lets disable any collisions that have been added to the exclusion list.
	int iCount = m_aryCollisionList.GetSize();
	CollisionPair *lpPair = NULL;
	RigidBody *lpPart1=NULL, *lpPart2=NULL;

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair =  m_aryCollisionList[iIndex];
		lpPart1 = lpStruct->FindRigidBody(lpPair->m_strPart1ID);
		lpPart2 = lpStruct->FindRigidBody(lpPair->m_strPart2ID);
		
		lpPart1->DisableCollision(this, lpPart2);
	}	
}

/*! \brief 
   Disables collision between the past-in object and all rigid bodies of the simulator.
      
   \param lpBody This is a pointer to the body to disable collisions on.

	 \return
	 No return value.

	 \remarks
	 This method disables collision responses between the rigid body being past
	 in and all rigid bodies in the simulator.

   \sa
   EnableCollision, DisableCollision	
*/

void Simulator::DisableCollision(RigidBody *lpBody)
{
	CStdMap<string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->DisableCollision(this, lpBody);
	}
}


/*! \fn virtual void Simulator::Initialize(int argc, const char **argv)
   \brief
   Initializes all aspects of the simulation to prepare it to be run.
      
	 \param argc Command line parameter count.
	 \param argv Command line paramenters.

	 \return
	 No return value.

	 \remarks
	 This is a pure virtual method that must be implemented in the simulator application.
	 It is where a lot of the nitty gritty details are done with initializing and 
	 setting up the physics engine so that it can run. It is also where we initialize
	 each structure to tell them to create their parts and joints.

	 \sa
	 Structure::Initialize, CreateParts, CreateJoints
*/


/*! \brief 
   Initializes all of the structures of this simulation.
      
	 \return
	 No return value.

	 \remarks
	 This method runs through all of the "static" structures and organisms
	 and calls their Initialize method.

	 \sa
	 Initialize, InitializeStructures
*/

void Simulator::InitializeStructures()
{
	m_oMaterialMgr.Initialize(this);

	CStdMap<string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->Initialize(this);
	}

	if(m_bEnableSimRecording)
		m_lSnapshotByteSize = CalculateSnapshotByteSize();

	if(m_bRecordVideo)
	{
		m_iVideoStepSize = (int) (m_fltVideoRecordFrameTime/m_fltPhysicsTimeStep);
		m_lVideoStartSlice = (long) (m_fltVideoStartTime/m_fltTimeStep);
		m_lVideoEndSlice = (long) (m_fltVideoEndTime/m_fltTimeStep);
		m_iVideoStep = 0;

		//First lets check if there is already an existing video file with that same
		//name. If there is then get rid of it.
		string strVideoFile = m_strProjectPath + m_strVideoFilename;
		struct stat f__stat;
		BOOL bFileExists = (stat(strVideoFile.c_str(), &f__stat) != 0);
		if(bFileExists)
			remove(strVideoFile.c_str( ));

		m_lpAvi = new CStdAvi(strVideoFile, (int) (m_fltVideoPlaybackFrameTime*1000), NULL); 
		m_lpAvi->m_aviOpts.cbFormat = m_aviOpts.cbFormat;
		m_lpAvi->m_aviOpts.cbParms = m_aviOpts.cbParms;
		m_lpAvi->m_aviOpts.dwBytesPerSecond = m_aviOpts.dwBytesPerSecond;
		m_lpAvi->m_aviOpts.dwFlags = m_aviOpts.dwFlags;
		m_lpAvi->m_aviOpts.dwInterleaveEvery = m_aviOpts.dwInterleaveEvery;
		m_lpAvi->m_aviOpts.dwKeyFrameEvery = m_aviOpts.dwKeyFrameEvery;
		m_lpAvi->m_aviOpts.dwQuality = m_aviOpts.dwQuality;
		m_lpAvi->m_aviOpts.fccHandler = m_aviOpts.fccHandler;
		m_lpAvi->m_aviOpts.fccType = m_aviOpts.fccType;
		m_lpAvi->m_aviOpts.lpFormat = m_aviOpts.lpFormat;
		m_lpAvi->m_aviOpts.lpParms = m_aviOpts.lpParms;
	}

}

inline void Simulator::StepNeuralEngine()
{
	for(m_oOrganismIterator=m_aryOrganisms.begin(); 
	    m_oOrganismIterator!=m_aryOrganisms.end(); 
			++m_oOrganismIterator)
	{
		m_lpSelOrganism = m_oOrganismIterator->second;
		m_lpSelOrganism->StepNeuralEngine(this);
	}
}

inline void Simulator::StepPhysicsEngine()
{
	for(m_oStructureIterator=m_aryAllStructures.begin(); 
	    m_oStructureIterator!=m_aryAllStructures.end(); 
			++m_oStructureIterator)
	{
		m_lpSelStructure = m_oStructureIterator->second;
		m_lpSelStructure->StepPhysicsEngine(this);
	}

	//Now lets step all Target adapters. This will be all items outputing
	//to the physics engine. Examples are motorized joints and muscles.
	for(int iIndex=0; iIndex<m_iTargetAdapterCount; iIndex++)
		m_aryTargetPhysicsAdapters[iIndex]->StepSimulation(this, NULL);

	if(m_bRecordVideo)
		RecordVideoFrame();

	m_lPhysicsSliceCount++;
}


inline void Simulator::Step()
{
	m_oExternalStimuliMgr.StepSimulation(this);

	if(m_iPhysicsStepCount == m_iPhysicsStepInterval)
		StepPhysicsEngine();

	StepNeuralEngine();

	m_oDataChartMgr.StepSimulation(this);
	if(m_lpSimRecorder) 
		m_lpSimRecorder->StepSimulation(this);

	m_lTimeSlice++;
	m_fltTime += m_fltTimeStep;
}


/*! \brief 
   Steps the entire simulation forward by one time slice.
      
	 \return
	 No return value.

	 \remarks
	 This method steps the entire simulation forward by one time slice. It goes through
	 and calls StepSimulation for every structure/organism object, which in turn calls
	 StepSimulation for each rigid body and joint of each of those objects. So you need 
	 to be VERY careful to keep all code within the StepSimulation methods short, sweet, 
	 and very fast. They are in the main processing loop and even a small increase in the
	 amount of processing time that occurrs within this loop will lead to major impacts on
	 the ultimate performance of the system.

	 \sa
	 Structure::StepSimulation, Body::StepSimulation, Joint::StepSimulation
*/

void Simulator::StepSimulation()
{
	for(m_iPhysicsStepCount=1; m_iPhysicsStepCount<=m_iPhysicsStepInterval; m_iPhysicsStepCount++)
		Step();
}


void Simulator::RunSimulation()
{
	Load(SimulationFile());
	Initialize(0, NULL);
	Simulate();
}

void Simulator::EnableVideoPlayback(string strKeyFrameID)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback && m_lpVideoPlayback->ID() == strKeyFrameID)
		return;

	KeyFrame *lpFrame = dynamic_cast<KeyFrame *>(m_lpSimRecorder->Find(strKeyFrameID));
	lpFrame->EnableVideoPlayback(this);
}

void Simulator::DisableVideoPlayback()
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->DisableVideoPlayback(this);
}

void Simulator::StartVideoPlayback()
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->StartVideoPlayback(this);
}

void Simulator::StopVideoPlayback()
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->StopVideoPlayback(this);
}

void Simulator::StepVideoPlayback(int iFrameCount)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->StepVideoPlayback(this, iFrameCount);
}

void Simulator::SaveVideo(string strPath)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->SaveVideo(this, strPath);
}

string Simulator::AddKeyFrame(string strType, long lStart, long lEnd)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	KeyFrame *lpFrame = m_lpSimRecorder->Add(this, strType, lStart, lEnd);
	return lpFrame->ID();
}

void Simulator::RemoveKeyFrame(string strID)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	m_lpSimRecorder->Remove(this, strID);
}

string Simulator::MoveKeyFrame(string strID, long lStart, long lEnd)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	KeyFrame *lpFrame = dynamic_cast<KeyFrame *>(m_lpSimRecorder->Find(strID));

	//If neither the start or end slice is changed then jump out of here.
	if(lpFrame->StartSlice() == lStart && lpFrame->EndSlice() == lEnd)
		return lpFrame->ID();

	//unsigned char iType = lpFrame->Type();
	m_lpSimRecorder->Remove(this, strID);
	lpFrame = m_lpSimRecorder->Add(this, lpFrame->Type(), lStart, lEnd);
	return lpFrame->ID();
}

void Simulator::MoveSimulationToKeyFrame(string strKeyFrameID)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(!Std_IsBlank(strKeyFrameID))
	{
		KeyFrame *lpFrame = dynamic_cast<KeyFrame *>(m_lpSimRecorder->Find(strKeyFrameID));
		lpFrame->MakeCurrentFrame(this);
	}
	else if(m_lpSimStopPoint)
		m_lpSimStopPoint->MakeCurrentFrame(this);
}

long Simulator::CalculateSnapshotByteSize()
{
	long lByteSize = 0;
	CStdMap<string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lByteSize += lpStructure->CalculateSnapshotByteSize();
	}

	return lByteSize;
}

void Simulator::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	CStdMap<string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->SaveKeyFrameSnapshot(aryBytes, lIndex);
	}
}


void Simulator::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	CStdMap<string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->LoadKeyFrameSnapshot(aryBytes, lIndex);
	}
}


void Simulator::RecordVideoFrame()
{
	if(m_lpAvi && (m_lTimeSlice >= m_lVideoStartSlice) && (m_lTimeSlice <= m_lVideoEndSlice))
	{
		m_iVideoStep--;
		if(m_iVideoStep <= 0)
		{
			m_lVideoFrame++;
			//m_strVideoFile = "C:\\Projects\\Documentation\\Results\\Thesis\\Hi Speed Video\\Sim_Track_Error_Test\\VideoImages\\Frame_" + STR(m_lVideoFrame) + ".bmp";
			//If this is the first
			//m_lpAvi->AddWindowFrame(m_hSimulationWnd, FALSE, "");				
			m_iVideoStep = m_iVideoStepSize;
		}	
	}

	//If we have recorded the entire video then close it out.
	if(m_lpAvi && m_lTimeSlice >= m_lVideoEndSlice)
	{
		delete m_lpAvi;
		m_lpAvi = NULL;
	}
}

BOOL Simulator::WaitForSimulationBlock(long lTimeout)
{
	m_bBlockSimulation = TRUE;
	long lTime = 0;
	BOOL bDone = FALSE;
	while(!bDone)
	{
		if(!m_bSimBlockConfirm)
		{
			Sleep(10);

			lTime+=10;
			if(lTimeout > 0 && lTime >= lTimeout)
			{
				bDone = TRUE;
				m_bBlockSimulation = FALSE;
			}
		}
		else
			bDone = TRUE;
	}

	return m_bSimBlockConfirm;
}

BOOL Simulator::CheckSimulationBlock()
{
	if(m_bBlockSimulation)
	{
		m_bSimBlockConfirm = TRUE;
		Sleep(1);
	}
	else
		m_bSimBlockConfirm = FALSE;

	return m_bSimBlockConfirm;
}

void Simulator::SimStarting()
{
	CStdMap<string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase != this)
			lpBase->SimStarting();
	}
}

void Simulator::SimPausing()
{
	CStdMap<string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase != this)
			lpBase->SimPausing();
	}
}

void Simulator::SimStopping()
{
	CStdMap<string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase != this)
			lpBase->SimStopping();
	}
}



/*! \brief
   Loads the simulation from an xml configuration file.
      
	 \param strFileName the name of the configuration file to load.
	               It uses this and m_strProjectPath to get the
								 full path to the location of the file.

	 \return
	 No return value.

	 \remarks
	 This method is responsible for loading the entire simulation 
	 from a XMl configuration file. It loads all of the "static" 
	 structures and organisms. 

	 \sa
	 Load, LoadEnvironment
*/

void Simulator::Load(string strFileName)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading simulator config file.\r\nFileName: " + strFileName);

	if(Std_IsBlank(strFileName))
	{
	 if(Std_IsBlank(m_strSimulationFile))
		 THROW_ERROR(Al_Err_lSimFileBlank, Al_Err_strSimFileBlank);
	}	
	else
		m_strSimulationFile = strFileName;

	oXml.Load(AnimatSim::GetFilePath(m_strProjectPath, m_strSimulationFile));

	oXml.FindElement("Simulation");
	oXml.FindChildElement("");

	Load(oXml);

	TRACE_DEBUG("Finished loading simulator config file.");
}


/*! \brief
   Loads the simulation from an xml configuration file.
      
   \param oXml  This is an xml object.

	 \return
	 No return value.

	 \remarks
	 This method is responsible for loading the entire simulation 
	 from a XMl configuration file. It loads all of the "static" 
	 structures and organisms. 

	 \sa
	 Load, LoadEnvironment
*/

void Simulator::Load(CStdXml &oXml)
{
	TRACE_DEBUG("Loading simulator config from Xml.");

	Reset();

	AddToObjectList(this);

	if(Std_IsBlank(m_strProjectPath))
		m_strProjectPath = oXml.GetChildString("ProjectPath", "");

	//m_lUpdateDataInterval = oXml.GetChildLong("UpdateDataInterval", m_lUpdateDataInterval);
	m_bPaused = oXml.GetChildBool("StartPaused", m_bPaused);
	m_bEnableSimRecording = oXml.GetChildBool("EnableSimRecording", m_bEnableSimRecording);
	
	BOOL bSetEndSim = oXml.GetChildBool("SetSimEnd", FALSE);
	if(bSetEndSim)
		m_fltEndSimTime = oXml.GetChildFloat("SimEndTime", m_fltEndSimTime);
 
	if(m_bEnableSimRecording)
		m_lpSimRecorder = CreateSimulationRecorder();

	//Other stuff Later
	LoadEnvironment(oXml);
	m_oDataChartMgr.Load(this, oXml);
	m_oMaterialMgr.Load(this, oXml);
	if(m_lpWinMgr) 
		m_lpWinMgr->Load(this, oXml);

	if(oXml.FindChildElement("ExternalStimuli", FALSE))
	{
		string strStimuliFile = oXml.GetChildString("ExternalStimuli", "");

		if(!Std_IsBlank(strStimuliFile))
			m_oExternalStimuliMgr.Load(this, m_strProjectPath, strStimuliFile);
		else
			m_oExternalStimuliMgr.Load(this, oXml);
	}

	if(m_lpSimRecorder && oXml.FindChildElement("RecorderKeyFrames", FALSE))
	{
		string strKeyframesFile = oXml.GetChildString("RecorderKeyFrames");
		m_lpSimRecorder->Load(this, m_strProjectPath, strKeyframesFile);
	}

	TRACE_DEBUG("Finished loading simulator config from Xml.");
}


/*! \brief 
   Adds a new organism to the list of structures for this simulation.
      
   \param lpOrganism The new organism to be added.

	 \return
	 No return value.

	 \remarks
	 This method gets a list of all organisms and a list of referneces to
	 all structures in this simulation that are mapped to their
	 ID value. This allows us to use the STL find funtions to find organisms.
	 This is more efficeient that using a loop and recursion.
	 This also allows us to ensure that each organism/structure that is
	 being added has a unique ID value. If you attempt to add a organism that
	 has a ID that is already in the list then an exception will be thrown.
	 Note that this method is NOT creating the object itself, that is done
	 elsewhere. It is simply adding it to the organism list and adding 
	 a reference to that created object to m_aryAllStructures list.

	 \sa
	 AddOrganism, AddStructure, m_aryAllStructures, m_aryOrganisms, m_aryStructures, 
	 FindOrganism, FindStructure, FindStructureFromAll
*/

void Simulator::AddOrganism(Organism *lpOrganism)
{
	if(!lpOrganism)
		THROW_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined);

	try
	{
			m_aryAllStructures.Add(lpOrganism->ID(), lpOrganism);
			m_aryOrganisms.Add(lpOrganism->ID(), lpOrganism);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate Organism Key: " + lpOrganism->ID(); 
		RELAY_ERROR(oError);
	}
}

void Simulator::AddOrganism(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Organism");

	Organism *lpOrg = LoadOrganism(oXml);
	lpOrg->Initialize(this);
}

void Simulator::RemoveOrganism(string strID, BOOL bThrowError)
{
	m_aryAllStructures.Remove(strID);
	m_aryOrganisms.Remove(strID);
}


/*! \brief 
   Adds a new "static" structure to the list of structures for this simulation.
      
   \param lpStructure The new structure to be added.

	 \return
	 No return value.

	 \remarks
	 This method gets a list of all structures and a list of referneces to
	 all structures in this simulation that are mapped to their
	 ID value. This allows us to use the STL find funtions to find structures.
	 This is more efficeient that using a loop and recursion.
	 This also allows us to ensure that each organism/structure that is
	 being added has a unique ID value. If you attempt to add a structure that
	 has a ID that is already in the list then an exception will be thrown.
	 Note that this method is NOT creating the object itself, that is done
	 elsewhere. It is simply adding it to the structure list and adding 
	 a reference to that created object to m_aryAllStructures list.

	 \sa
	 AddOrganism, AddStructure, m_aryAllStructures, m_aryOrganisms, m_aryStructures, 
	 FindOrganism, FindStructure, FindStructureFromAll
*/

void Simulator::AddStructure(Structure *lpStructure)
{
	if(!lpStructure)
		THROW_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined);

	try
	{
			m_aryAllStructures.Add(lpStructure->ID(), lpStructure);
			m_aryStructures.Add(lpStructure->ID(), lpStructure);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate structure Key: " + lpStructure->ID(); 
		RELAY_ERROR(oError);
	}
}

void Simulator::AddStructure(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Structure");

	Structure *lpStruct = LoadStructure(oXml);
	lpStruct->Initialize(this);
}

void Simulator::RemoveStructure(string strID, BOOL bThrowError)
{
	m_aryAllStructures.Remove(strID);
	m_aryStructures.Remove(strID);
}

void Simulator::AddOdorType(OdorType *lpOdorType)
{
	if(!lpOdorType)
		THROW_ERROR(Al_Err_lOdorNotDefined, Al_Err_strOdorNotDefined);

	try
	{
			m_aryOdorTypes.Add(lpOdorType->ID(), lpOdorType);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate odor type Key: " + lpOdorType->ID(); 
		RELAY_ERROR(oError);
	}
}


/*! \brief 
   Loads all structures from from the configuration file for this simulation.
      
   \param oXml This is an xml object.

	 \return
	 No return value.

	 \remarks
	 This method loads all "static" structures and organisms specified in the 
	 simulation configuration file.

	 \sa
	 Load, LoadEnvironment
*/

void Simulator::LoadEnvironment(CStdXml &oXml)
{
	TRACE_DEBUG("Loading structures from Xml.");

	if(!m_lpAnimatClassFactory)
		THROW_ERROR(Al_Err_lClassFactoryNotDefined, Al_Err_strClassFactoryNotDefined);

	m_aryOrganisms.RemoveAll();
	m_aryStructures.RemoveAll();
	m_aryOdorTypes.RemoveAll();

	oXml.IntoChildElement("Environment"); //Into Environment Element

	DistanceUnits(oXml.GetChildString("DistanceUnits", "centimeter"));
	MassUnits(oXml.GetChildString("MassUnits", "gram"));
	Gravity(oXml.GetChildFloat("Gravity", m_fltGravity));
	PhysicsTimeStep(oXml.GetChildFloat("PhysicsTimeStep", m_fltPhysicsTimeStep));

	SimulateHydrodynamics(oXml.GetChildBool("SimulateHydrodynamics", m_bSimulateHydrodynamics));
	if(m_bSimulateHydrodynamics)
		FluidDensity(oXml.GetChildFloat("FluidDensity", m_fltFluidDensity));

	m_bAutoGenerateRandomSeed = oXml.GetChildBool("AutoGenerateRandomSeed", m_bAutoGenerateRandomSeed);
	m_iManualRandomSeed = oXml.GetChildInt("ManualRandomSeed", m_iManualRandomSeed);

	FrameRate(oXml.GetChildInt("FrameRate", m_iFrameRate));

	MouseSpringStiffness(oXml.GetChildFloat("MouseSpringStiffness", m_fltMouseSpringStiffness));
	MouseSpringDamping(oXml.GetChildFloat("MouseSpringDamping", m_ftlMouseSpringDamping));

	m_fltLinearCompliance = oXml.GetChildFloat("LinearCompliance", m_fltLinearCompliance);
	m_fltAngularCompliance = oXml.GetChildFloat("AngularCompliance", m_fltAngularCompliance);
	m_fltLinearDamping = oXml.GetChildFloat("LinearDamping", m_fltLinearDamping);
	m_fltAngularDamping = oXml.GetChildFloat("AngularDamping", m_fltAngularDamping);
	m_fltLinearKineticLoss = oXml.GetChildFloat("LinearKineticLoss", m_fltLinearKineticLoss);
	m_fltAngularKineticLoss = oXml.GetChildFloat("AngularKineticLoss", m_fltAngularKineticLoss);

	m_bForceFastMoving = oXml.GetChildBool("FastMoving", m_bForceFastMoving);

	//m_fltLinearCompliance = 1e-20;
	//m_fltAngularCompliance = 1e-20;
	//m_fltLinearDamping = 1e20;
	//m_fltAngularDamping = 1e20;

	//m_fltLinearCompliance *= m_fltMassUnits;
	//m_fltAngularCompliance *= m_fltMassUnits;
	//m_fltLinearDamping *= m_fltInverseMassUnits;
	//m_fltAngularDamping *= m_fltInverseMassUnits;
	//m_fltLinearKineticLoss = AnimatSim::LoadScaledNumber(oXml, "LinearKineticLoss", FALSE, m_fltLinearKineticLoss);
	//m_fltAngularKineticLoss = AnimatSim::LoadScaledNumber(oXml, "AngularKineticLoss", FALSE, m_fltAngularKineticLoss);

	if(oXml.FindChildElement("OdorTypes", FALSE))
	{
		oXml.IntoElem();  //Into Odors Element
		int iChildCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadOdorType(oXml);
		}
		oXml.OutOfElem(); //OutOf Odors Element
	}	

	if(oXml.FindChildElement("Organisms", FALSE))
	{
		oXml.IntoElem(); //Into Organisms Element
		int iCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadOrganism(oXml);		
		}
		oXml.OutOfElem(); //OutOf Organisms Element
	}

	if(oXml.FindChildElement("Structures", FALSE))
	{
		oXml.IntoElem(); //Into Structures Element
		int iCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadStructure(oXml);
		}
		oXml.OutOfElem(); //OutOf Structures Element

	}

	oXml.OutOfElem(); //OutOf Environment Element

	TRACE_DEBUG("Finished loading structures from Xml.");
}


Structure *Simulator::LoadStructure(CStdXml &oXml)
{
	Structure *lpStructure = NULL;

try
{
	lpStructure = dynamic_cast<Structure *>(m_lpAnimatClassFactory->CreateObject("Structure", "Structure", TRUE));
	lpStructure->ProjectPath(m_strProjectPath);
	lpStructure->Load(this, oXml);

	AddStructure(lpStructure);
	return lpStructure;
}
catch(CStdErrorInfo oError)
{
	if(lpStructure) delete lpStructure;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpStructure) delete lpStructure;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


Organism *Simulator::LoadOrganism(CStdXml &oXml)
{
	Organism *lpOrganism = NULL;
	string strModule;
	string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpOrganism = dynamic_cast<Organism *>(CreateObject(strModule, "Organism", strType));
	if(!lpOrganism)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	lpOrganism->ProjectPath(m_strProjectPath);
	lpOrganism->Load(this, oXml);

	AddOrganism(lpOrganism);
	return lpOrganism;
}
catch(CStdErrorInfo oError)
{
	if(lpOrganism) delete lpOrganism;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpOrganism) delete lpOrganism;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

OdorType *Simulator::LoadOdorType(CStdXml &oXml)
{
	OdorType *lpOdorType = NULL;

try
{
	lpOdorType = new OdorType();
	lpOdorType->Load(this, oXml);

	AddOdorType(lpOdorType);
	return lpOdorType;
}
catch(CStdErrorInfo oError)
{
	if(lpOdorType) delete lpOdorType;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpOdorType) delete lpOdorType;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

void Simulator::LoadAnimatModuleName(string strFile, string &strAnimatModule)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading simulator module name file.\r\nFileName: " + strFile);

	if(Std_IsBlank(strFile))
		THROW_ERROR(Al_Err_lSimFileBlank, Al_Err_strSimFileBlank);

	oXml.Load(strFile);

	LoadAnimatModuleName(oXml, strAnimatModule);

	TRACE_DEBUG("Finished loading simulator module name.");
}

void Simulator::LoadAnimatModuleName(CStdXml &oXml, string &strAnimatModule)
{
	oXml.FindElement("Simulation");
	oXml.FindChildElement("");

	strAnimatModule = oXml.GetChildString("AnimatModule");
}

/*
CNlClassFactory *Simulator::LoadNeuralClassFactory(string strNeuralModule)
{
	CNlClassFactory *lpNeuralFactory=NULL;

try
{
	lpNeuralFactory = dynamic_cast<CNlClassFactory *>(IStdClassFactory::LoadModule(strNeuralModule));
	if(!lpNeuralFactory)
		THROW_PARAM_ERROR(Al_Err_lModuleFactoryNotNeural, Al_Err_strModuleFactoryNotNeural, "NeuralModule", strNeuralModule);
	return lpNeuralFactory;
}
catch(CStdErrorInfo oError)
{
	if(lpNeuralFactory) delete lpNeuralFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpNeuralFactory) delete lpNeuralFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}
*/

IStdClassFactory *Simulator::LoadClassFactory(string strModuleName)
{
	IStdClassFactory *lpFactory=NULL;

try
{
	lpFactory = IStdClassFactory::LoadModule(strModuleName);
	return lpFactory;
}
catch(CStdErrorInfo oError)
{
	if(lpFactory) delete lpFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpFactory) delete lpFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

Simulator *Simulator::CreateSimulator(int argc, const char **argv)
{	
	string strExecutablePath, strExeFile;

#ifdef _ANIMAT_VC8
		string strBuffer;
		wchar_t strWBuffer[2000];
#else
	#ifdef _WIN32_WCE
		string strBuffer;
		wchar_t strWBuffer[2000];
	#else
		char strBuffer[2000];
	#endif
#endif

	//Get the working directory for the exe.
  HINSTANCE hInst = GetModuleHandle(NULL);

#ifdef _ANIMAT_VC8
	GetModuleFileName(hInst, strWBuffer, 2000);
	strBuffer = Std_ConvertToANSI(strWBuffer);
#else
	#ifdef _WIN32_WCE
		GetModuleFileName(hInst, strWBuffer, 2000);
		strBuffer = Std_ConvertToANSI(strWBuffer);
	#else
		GetModuleFileName(hInst, strBuffer, 2000);
	#endif
#endif

	Std_SplitPathAndFile(strBuffer, strExecutablePath, strExeFile);

	//Set the log file prefix
#ifndef _WIN32_WCE
	if(Std_DirectoryExists(strExecutablePath + "Logs"))
		Std_SetLogFilePrefix(strExecutablePath + "Logs\\AnimatSimulator");
	else
		Std_SetLogFilePrefix(strExecutablePath + "AnimatSimulator");
#endif

	int iParam=0;
	BOOL bRetrieved=FALSE, bFound = FALSE;
	string strParam, strProject;
	while(!bRetrieved && iParam<argc)
	{
		strParam = Std_ToUpper(Std_Trim(argv[iParam]));

		if(bFound)
		{
			strProject = strParam;
			bRetrieved = TRUE;
		}

		if(strParam == "-PROJECT")
			bFound = TRUE;

		iParam++;
	}

	if(Std_IsBlank(strProject))
		THROW_ERROR(Al_Err_lNoProjectParamOnCommandLine, Al_Err_strNoProjectParamOnCommandLine);

	return CreateSimulator(strProject);
}

Simulator *Simulator::CreateSimulator(string strSimulationFile)
{
	Simulator *lpSim = NULL;
	string strAnimatModule;
	IStdClassFactory *lpAnimatFactory=NULL;

#ifdef _ANIMAT_VC8
		string strBuffer;
		wchar_t strWBuffer[2000];
#else
	#ifdef _WIN32_WCE
		string strBuffer;
		wchar_t strWBuffer[2000];
	#else
		char strBuffer[2000];
	#endif
#endif

	string strProjectPath, strProjectFile;
	string strExecutablePath, strExeFile;

try
{ 
	//Get the working directory for the exe.
  HINSTANCE hInst = GetModuleHandle(NULL);

#ifdef _ANIMAT_VC8
	GetModuleFileName(hInst, strWBuffer, 2000);
	strBuffer = Std_ConvertToANSI(strWBuffer);
#else
	#ifdef _WIN32_WCE
		GetModuleFileName(hInst, strWBuffer, 2000);
		strBuffer = Std_ConvertToANSI(strWBuffer);
	#else
		GetModuleFileName(hInst, strBuffer, 2000);
	#endif
#endif

	Std_SplitPathAndFile(strBuffer, strExecutablePath, strExeFile);

	//_getcwd( strBuffer, 2000 );
	//Std_SetLogFilePrefix(strExecutablePath + "Logs\\AnimatSimulator");
 
	Std_SplitPathAndFile(strSimulationFile, strProjectPath, strProjectFile);

	if(!Std_IsFullPath(strSimulationFile))
	{
		strProjectPath = strExecutablePath;
		strSimulationFile = strProjectPath + strSimulationFile;
	}

	LoadAnimatModuleName(strSimulationFile, strAnimatModule);

	lpAnimatFactory = LoadClassFactory(strAnimatModule);

	//Now we need to get the simulation application itself. This ALWAYS
	//comes from the animat engine because the animat engine uses the
	//neural engine it is higher up the food chain and it decides the
	//actual simulator that needs to be used.
	lpSim = dynamic_cast<Simulator *>(lpAnimatFactory->CreateObject("Simulator", ""));
	if(!lpSim)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Simulator");

	lpSim->ProjectPath(strProjectPath);
	lpSim->ExecutablePath(strExecutablePath);
	lpSim->SimulationFile(strProjectFile);

	if(lpAnimatFactory) 
		{delete lpAnimatFactory; lpAnimatFactory = NULL;}

	g_lpSimulator = lpSim;
	return lpSim;
}
catch(CStdErrorInfo oError)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


Simulator *Simulator::CreateSimulator(CStdXml &oXml)
{
	Simulator *lpSim = NULL;
	string strAnimatModule;
	IStdClassFactory *lpAnimatFactory=NULL;

#ifdef _ANIMAT_VC8
		string strBuffer;
		wchar_t strWBuffer[2000];
#else
	#ifdef _WIN32_WCE
		string strBuffer;
		wchar_t strWBuffer[2000];
	#else
		char strBuffer[2000];
	#endif
#endif

	string strProjectPath, strProjectFile;
	string strExecutablePath, strExeFile;

try
{ 
	//Get the working directory for the exe.
  HINSTANCE hInst = GetModuleHandle(NULL);

#ifdef _ANIMAT_VC8
	GetModuleFileName(hInst, strWBuffer, 2000);
	strBuffer = Std_ConvertToANSI(strWBuffer);
#else
	#ifdef _WIN32_WCE
		GetModuleFileName(hInst, strWBuffer, 2000);
		strBuffer = Std_ConvertToANSI(strWBuffer);
	#else
		GetModuleFileName(hInst, strBuffer, 2000);
	#endif
#endif

	Std_SplitPathAndFile(strBuffer, strExecutablePath, strExeFile);

	LoadAnimatModuleName(oXml, strAnimatModule);

	lpAnimatFactory = LoadClassFactory(strAnimatModule);

	//Now we need to get the simulation application itself. This ALWAYS
	//comes from the animat engine because the animat engine uses the
	//neural engine it is higher up the food chain and it decides the
	//actual simulator that needs to be used.
	lpSim = dynamic_cast<Simulator *>(lpAnimatFactory->CreateObject("Simulator", ""));
	if(!lpSim)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Simulator");

	lpSim->ProjectPath(strProjectPath);
	lpSim->ExecutablePath(strExecutablePath);

	if(lpAnimatFactory) 
		{delete lpAnimatFactory; lpAnimatFactory = NULL;}

	g_lpSimulator = lpSim;
	return lpSim;
}
catch(CStdErrorInfo oError)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

float Simulator::ConvertDistanceUnits(string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOMETERS" || strUnits == "KILOMETER")
		return (float) 1000;

	if(strUnits == "CENTAMETERS" || strUnits == "CENTAMETER")
		return (float) 100;

	if(strUnits == "DECAMETERS" || strUnits == "DECAMETER")
		return (float) 10;

	if(strUnits == "METERS" || strUnits == "METER")
		return (float) 1;

	if(strUnits == "DECIMETERS" || strUnits == "DECIMETER")
		return (float) 0.1;

	if(strUnits == "CENTIMETERS" || strUnits == "CENTIMETER")
		return (float) 0.01;

	if(strUnits == "MILLIMETERS" || strUnits == "MILLIMETER")
		return (float) 0.001;

	THROW_PARAM_ERROR(Al_Err_lInvalidDistanceUnits, Al_Err_strInvalidDistanceUnits, "DistanceUnits", strUnits);

	return (float) 1;
}

float Simulator::ConvertDenominatorDistanceUnits(string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOMETERS" || strUnits == "KILOMETER")
		return (float) 1;

	if(strUnits == "CENTAMETERS" || strUnits == "CENTAMETER")
		return (float) 1;

	if(strUnits == "DECAMETERS" || strUnits == "DECAMETER")  //1 Unit = 10 m
		return (float) 10;

	if(strUnits == "METERS" || strUnits == "METER")
		return (float) 1;

	if(strUnits == "DECIMETERS" || strUnits == "DECIMETER")  //1 Unit = 10 cm 
		return (float) 10;

	if(strUnits == "CENTIMETERS" || strUnits == "CENTIMETER")
		return (float) 1;

	if(strUnits == "MILLIMETERS" || strUnits == "MILLIMETER")
		return (float) 1;

	THROW_PARAM_ERROR(Al_Err_lInvalidDistanceUnits, Al_Err_strInvalidDistanceUnits, "DenominatorDistanceUnits", strUnits);

	return (float) 1;
}

float Simulator::ConvertMassUnits(string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOGRAMS" || strUnits == "KILOGRAM")
		return (float) 1;

	if(strUnits == "CENTAGRAMS" || strUnits == "CENTAGRAM")
		return (float) 0.1;

	if(strUnits == "DECAGRAMS" || strUnits == "DECAGRAM")
		return (float) 0.01;

	if(strUnits == "GRAMS" || strUnits == "GRAM")
		return (float) 0.001;

	if(strUnits == "DECIGRAMS" || strUnits == "DECIGRAM")
		return (float) 1e-4;

	if(strUnits == "CENTIGRAMS" || strUnits == "CENTIGRAM")
		return (float) 1e-5;

	if(strUnits == "MILLIGRAMS" || strUnits == "MILLIGRAM")
		return (float) 1e-6;

	THROW_PARAM_ERROR(Al_Err_lInvalidMassUnits, Al_Err_strInvalidMassUnits, "MassUnits", strUnits);

	return (float) 1;
}

float Simulator::ConvertDensityMassUnits(string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOGRAMS" || strUnits == "KILOGRAM")
		return (float) 1000;

	if(strUnits == "CENTAGRAMS" || strUnits == "CENTAGRAM")
		return (float) 100;

	if(strUnits == "DECAGRAMS" || strUnits == "DECAGRAM")
		return (float) 10;

	if(strUnits == "GRAMS" || strUnits == "GRAM")
		return (float) 1;

	if(strUnits == "DECIGRAMS" || strUnits == "DECIGRAM")
		return (float) 0.1;

	if(strUnits == "CENTIGRAMS" || strUnits == "CENTIGRAM")
		return (float) 0.01;

	if(strUnits == "MILLIGRAMS" || strUnits == "MILLIGRAM")
		return (float) 0.001;

	THROW_PARAM_ERROR(Al_Err_lInvalidMassUnits, Al_Err_strInvalidMassUnits, "MassUnits", strUnits);

	return (float) 1;
}




/*! \fn long Simulator::TimeSlice()
   \brief
   TimeSlice property.
      
   \remarks
	 The current time slice. This a long value.
	 This is the accessor function for the m_lTimeSlice element.
*/
/*! \fn void Simulator::TimeSlice(long lVal)
   \brief
   TimeSlice property.
      
   \remarks
	 The current time slice. This a long value.
	 This is the mutator function for the m_lTimeSlice element.
*/


/*! \fn string Simulator::ProjectPath()
   \brief
   ProjectPath property.
      
   \remarks
	 The directory path where the simulation configuration files are located.
	 This is the accessor function for the m_strProjectPath element.
*/
/*! \fn void Simulator::ProjectPath(string strPath)
   \brief
   ProjectPath property.
      
   \remarks
	 The directory path where the simulation configuration files are located.
	 This is the mutator function for the m_strProjectPath element.
*/


/*! \fn string Simulator::SimulationFile()
   \brief
   SimulationFile property.
      
   \remarks
	 The name of the Animat Simulation (ASIM) file.
	 This is the accessor function for the m_strSimulationFile element.
*/
/*! \fn void Simulator::SimulationFile(string strFile)
   \brief
   SimulationFile property.
      
   \remarks
	 The name of the Animat Simulation (ASIM) file.
	 This is the mutator function for the m_strSimulationFile element.
*/



/*! \fn float Simulator::StepIncrement()
   \brief
   StepIncrement property.
      
   \remarks
	 The time increment for each time slice in the simulation. 
	 This is the accessor function for the m_fltStepIncrement element.
*/
/*! \fn void Simulator::StepIncrement(float fltVal)
   \brief
   StepIncrement property.
      
   \remarks
	 The time increment for each time slice in the simulation. 
	 This is the mutator function for the m_fltStepIncrement element.
*/


/*! \fn ClassFactory *Simulator::ClassFactory()
   \brief
   ClassFactory property.
      
   \remarks
	 This is a copy of the class factory associated with this executable. There
	 can only be one class factory per executable and it should have NO state.
	 This is the accessor function for the m_lpClassFactory element.
*/
/*! \fn void Simulator::ClassFactory(ClassFactory *lpFactory)
   \brief
   ClassFactory property.
      
   \remarks
	 This is a copy of the class factory associated with this executable. There
	 can only be one class factory per executable and it should have NO state.
	 This is the mutator function for the m_lpClassFactory element.
*/


/*! \fn float Simulator::Gravity() 
   \brief
   Gravity property.
      
   \remarks
	 The acceleration of gravity to use in the simulation.
	 This is the accessor function for the m_fltGravity element.
*/
/*! \fn void Simulator::Gravity(float fltVal)
   \brief
   Gravity property.
      
   \remarks
	 The acceleration of gravity to use in the simulation.
	 This is the mutator function for the m_fltGravity element.
*/


/*! \fn BOOL Simulator::SimulateHydrodynamics()
   \brief
   SimulateHydrodynamics property.
      
   \remarks
	 Tells whether or not we will be doing hydrodynamic simulations.
	 If you are not doing stuff underwater then be sure this is set to
	 FALSE. The hydrodynamics adds extra overhead that can slow the
	 simulation down slightly.
	 This is the accessor function for the m_bSimulateHydrodynamics element.
*/
/*! \fn void Simulator::SimulateHydrodynamics(BOOL bVal)
   \brief
   SimulateHydrodynamics property.
      
   \remarks
	 Tells whether or not we will be doing hydrodynamic simulations.
	 If you are not doing stuff underwater then be sure this is set to
	 FALSE. The hydrodynamics adds extra overhead that can slow the
	 simulation down slightly.
	 This is the mutator function for the m_bSimulateHydrodynamics element.
*/


/*! \fn float Simulator::FluidDensity()
   \brief
   FluidDensity property.
      
   \remarks
	 Density of the fluid for hydrodynamic simulations. This is not
	 used if m_bSimulateHydrodynamics is FALSE.
	 This is the accessor function for the m_fltFluidDensity element.
*/
/*! \fn void Simulator::FluidDensity(float fltVal)
   \brief
   FluidDensity property.
      
   \remarks
	 Density of the fluid for hydrodynamic simulations. This is not
	 used if m_bSimulateHydrodynamics is FALSE.
	 This is the mutator function for the m_fltFluidDensity element.
*/

}			//AnimatSim
