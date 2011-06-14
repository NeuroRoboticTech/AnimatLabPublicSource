// VsSimulator.cpp: implementation of the VsSimulator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include <stdarg.h>
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsOrganism.h"
#include "VsStructure.h"
#include "VsClassFactory.h"
#include "VsSimulator.h"

//#include "VsSimulationRecorder.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


VsSimulator::VsSimulator()
{
	m_uUniverse = NULL;
	m_grpScene = NULL;	
	m_lTimer = 0;
	m_vsWinMgr = NULL;
	m_iLastFrame = osg::Timer::instance()->tick();
	m_iCurrentFrame = osg::Timer::instance()->tick();
	m_fltFrameDt = 0;
	m_vsWinMgr = new VsSimulationWindowMgr;
	m_lpWinMgr = m_vsWinMgr;
	m_lpWinMgr->SetSystemPointers(this, NULL, NULL, NULL, TRUE);
	m_uUniverse = NULL;
	m_vxFrame = NULL;
	m_vsIntersect.SetSystemPointers(this, NULL, NULL, NULL, TRUE);
}

VsSimulator::~VsSimulator()
{

try
{
	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Simulator\r\n", "", -1, FALSE, TRUE);}
}

VxUniverse *VsSimulator::Universe()
{return m_uUniverse;}

Vx::VxFrame* VsSimulator::Frame()
{return m_vxFrame;}

#pragma region MutatorOverrides

void VsSimulator::LinearCompliance(float fltVal, BOOL bUseScaling)
{
	Simulator::LinearCompliance(fltVal, bUseScaling);
	//TODO Add to sim
}

void VsSimulator::AngularCompliance(float fltVal, BOOL bUseScaling)
{
	Simulator::AngularCompliance(fltVal, bUseScaling);
	//TODO Add to sim
}

void VsSimulator::LinearDamping(float fltVal, BOOL bUseScaling)
{
	Simulator::LinearDamping(fltVal, bUseScaling);
	//TODO Add to sim
}

void VsSimulator::AngularDamping(float fltVal, BOOL bUseScaling)
{
	Simulator::AngularDamping(fltVal, bUseScaling);
	//TODO Add to sim
}

void VsSimulator::LinearKineticLoss(float fltVal)
{
	Simulator::LinearKineticLoss(fltVal);
	//TODO Add to sim
}

void VsSimulator::AngularKineticLoss(float fltVal)
{
	Simulator::AngularKineticLoss(fltVal);
	//TODO Add to sim
}

void VsSimulator::PhysicsTimeStep(float fltVal)
{
	Simulator::PhysicsTimeStep(fltVal);

	if(m_vxFrame)
		m_vxFrame->setTimeStep(m_fltPhysicsTimeStep);		
}

void VsSimulator::Gravity(float fltVal, BOOL bUseScaling)
{
	Simulator::Gravity(fltVal, bUseScaling);

	if(m_uUniverse)
		m_uUniverse->setGravity(0, m_fltGravity, 0);
}

int VsSimulator::GetMaterialID(string strID)
{
	if(m_vxFrame)
	{
		VxMaterialTable *lpMat = m_vxFrame->getMaterialTable();
		
		if(lpMat)
			return lpMat->getMaterialID(strID.c_str());
		return -1;
	}

	return -1;
}

#pragma endregion


SimulationRecorder *VsSimulator::CreateSimulationRecorder()
{
//NEED_TO_FIX
	return NULL; //new VsSimulationRecorder;
}

void VsSimulator::Reset()
{
	Simulator::Reset();

	if(m_vxFrame)
		m_vxFrame->release(); 

	if(!m_lpAnimatClassFactory) 
		m_lpAnimatClassFactory = new VsClassFactory;

	if(m_osgCmdMgr.valid())
		m_osgCmdMgr.release();
}

void VsSimulator::ResetSimulation()
{
	Simulator::ResetSimulation();

	if(m_uUniverse)
	{
		m_uUniverse->resetDynamics();
		m_uUniverse->resetContacts();
	}
	m_bSimRunning = FALSE;
}

void VsSimulator::ToggleSimulation()
{
	if(m_bPaused)
		SimStarting();
	else
		SimPausing();

	m_bPaused = !m_bPaused;
}

void VsSimulator::StopSimulation()
{
	SimStopping();
	if(!m_bPaused)
		ToggleSimulation();
	m_bSimRunning = FALSE;
}

void VsSimulator::CreateLights()
{
    osg::StateSet* rootStateSet = m_grpScene->getOrCreateStateSet();
	//rootStateSet->setMode( GL_LIGHTING, osg::StateAttribute::ON );

// create sun light
	osg::ref_ptr<osg::Group> osgLightGroup = new osg::Group;

    // Set up lighting.
    osg::Vec4 ambient(1.0, 1.0, 1.0, 1.0);
    osg::Vec4 diffuse(1.0, 1.0, 1.0, 1.0);
    osg::Vec4 specular(1.0, 1.0, 1.0, 1.0);
    osg::Vec4 position(10, 30, 0, 1);
    //osg::Vec3 direction(0, -1, 0);
    //direction.normalize();

    osg::ref_ptr<osg::Light> light = new osg::Light;
    light->setAmbient(ambient);
    light->setDiffuse(diffuse);
    light->setSpecular(specular);
    light->setPosition(position);
    //light->setDirection(direction);

    osg::ref_ptr<osg::LightSource> lightSource = new osg::LightSource;
    lightSource->setLight(light.get());
	osgLightGroup->addChild(lightSource.get());
    osg::StateSet *groupStateSet = osgLightGroup->getOrCreateStateSet();
	lightSource->setStateSetModes(*groupStateSet, osg::StateAttribute::ON); 

	m_grpScene->addChild(osgLightGroup.get());
}


void VsSimulator::InitializeVortexViewer(int argc, const char **argv)
{
    osg::ArgumentParser arguments(&argc, (char **) argv);

    // set up the usage document, in case we need to print out how to use this program.
    arguments.getApplicationUsage()->setApplicationName(arguments.getApplicationName());
    arguments.getApplicationUsage()->setDescription(arguments.getApplicationName()+" is the standard OpenSceneGraph example which loads and visualises 3d models.");
    arguments.getApplicationUsage()->setCommandLineUsage(arguments.getApplicationName()+" [options] filename ...");
    arguments.getApplicationUsage()->addCommandLineOption("-h or --help", "Display this information");
    
    // add resource search paths
    osgDB::getDataFilePathList().push_front("./Resources");
    osgDB::getDataFilePathList().push_front("../Resources");
    osgDB::getDataFilePathList().push_front("../../Resources");

	//This is the root of the scenegraph.  Which will corrospond
	//to the root of the simulation
	m_grpScene = new osg::Group;	
    m_grpScene->setName("World");
    CreateLights();

	//Add the mouse spring lines to the scene
	m_grpScene->addChild(VsMouseSpring::GetInstance()->GetNode());

	//Create the windows, cameras, and Huds
	m_vsWinMgr->Initialize();

	//Create the command manager if needed.
	if(!m_osgCmdMgr.valid())
	    m_osgCmdMgr = new osgManipulator::CommandManager;
}

	//TODO Add to sim
    //VxSolverParameters* sp = universe->getSolverParameters();
    //sp->setConstraintAngularCompliance(c);
    //sp->setConstraintAngularDamping(b);
    //sp->setConstraintAngularKineticLoss(1e-8);
    //sp->setConstraintLinearCompliance(c);
    //sp->setConstraintLinearDamping(b);
    //sp->setConstraintLinearKineticLoss(1e-8);
    //sp->setLcpParam(20, 10, 1.e-4);
//This function initializes the Vortex related
//classes and the vortex viewer.
void VsSimulator::InitializeVortex(int argc, const char **argv)
{
	#define NOBJECTS 100

	InitializeVortexViewer(argc, argv);

	//create the frame
	m_vxFrame = VxFrame::instance();
	m_uUniverse = new Vx::VxUniverse(NOBJECTS, NOBJECTS*40);
	m_uUniverse->setGravity(0, m_fltGravity, 0);
	//m_uUniverse->setGravity(0, 0, m_fltGravity);
	m_vxFrame->addUniverse(m_uUniverse);

	//set the frame timestep
	m_vxFrame->setTimeStep(m_fltPhysicsTimeStep);		

	VxFrameRegisterAllInteractions(m_vxFrame);
	VxOSG::VxSceneGraphInterfaceRegister(m_vxFrame);

    //VxContactProperties *pM;
    //pM = materialTable->getContactProperties("groundMaterial", "boxMaterial");
    //pM->setFrictionType(VxContactProperties::kFrictionTypeTwoDirection);
    //pM->setFrictionModel(VxContactProperties::kFrictionModelScaledBox);
    //pM->setFrictionCoefficientPrimary(1.0f);
    //pM->setFrictionCoefficientSecondary(1.3f);
    //pM->setCompliance(5.0e-7f);
    //pM->setDamping(2.0e5f);
    //pM->setSlipPrimary(0.00001f);   // some slip is necessary to allow the vehicle to turn
    //pM->setSlipSecondary(0.00001f); // some slip is necessary to allow the vehicle to turn

    // Register the simple callback to be notified at beginning and end of the interaction between parts and sensors.
    m_uUniverse->addIntersectSubscriber(VxUniverse::kResponsePart, VxUniverse::kResponseSensor, VxUniverse::kEventFirst, &m_vsIntersect, 0);
    m_uUniverse->addIntersectSubscriber(VxUniverse::kResponsePart, VxUniverse::kResponseSensor, VxUniverse::kEventDisjoint, &m_vsIntersect, 0);
}

Vx::VxTriangleMesh *VsSimulator::CreatTriangleMeshFromOsg(osg::Node *osgNode)
{
	Vx::VxTriangleMesh *vxMesh = NULL;
	vxMesh = VxTriangleMesh::createFromNode(osgNode);

	return vxMesh;
}


Vx::VxConvexMesh *VsSimulator::CreateConvexMeshFromOsg(osg::Node *osgNode)
{
	Vx::VxConvexMesh *vxMesh = NULL;
	vxMesh = VxConvexMesh::createFromNode(osgNode); 

	return vxMesh;
}

/**
\brief	Generates a collision mesh file.

\details When we want to use a collision mesh then we need to create a new .osg file using the 
mesh created by the physics engine instead of the graphics one. The UI will call this method
when the user sets a new mesh file or type so we can create the new file. It will load the graphics
file, convert it usign the physics engine, and then save it back out. When loading the convex mesh
we will use this new file instead of the original one.

\author	dcofer
\date	6/10/2011

\param	strOriginalMeshFile 	The original mesh file. 
\param	strCollisionMeshFile	The new collision mesh file. 
**/
void VsSimulator::GenerateCollisionMeshFile(string strOriginalMeshFile, string strCollisionMeshFile)
{
	//First load the original mesh in.
	string strPath = this->ProjectPath();
	string strOrigFile = AnimatSim::GetFilePath(strPath, strOriginalMeshFile);
	string strNewFile = AnimatSim::GetFilePath(strPath, strCollisionMeshFile);

	osg::ref_ptr<osg::Node> osgNode = osgDB::readNodeFile(strOrigFile.c_str());

	//Make sure the mesh loaded is valid.
	if(!osgNode.valid())
		THROW_PARAM_ERROR(Vs_Err_lErrorLoadingMesh, Vs_Err_strErrorLoadingMesh, "Original Mesh file", strOriginalMeshFile);

	//Now create a convex mesh with the physics engine using the loaded mesh.
	Vx::VxConvexMesh *vxMesh = VxConvexMesh::createFromNode(osgNode.get()); 

	//Now use that convexmesh geometry to create a new osg node.
	osg::ref_ptr<osg::Geometry> osgGeom = CreateOsgFromVxConvexMesh(vxMesh);
	osg::ref_ptr<osg::Geode> osgNewNode = new osg::Geode;
	osgNewNode->addDrawable(osgGeom.get());

	//Now save out the new collision mesh.
	osgDB::writeNodeFile(*osgNewNode, strNewFile.c_str());
}

void VsSimulator::Initialize(int argc, const char **argv)
{
	InitializeVortex(argc, argv);

	//Set the random number generator seed
	if(m_bAutoGenerateRandomSeed)
		GenerateAutoSeed();
	else
	{
		Std_SRand(m_iManualRandomSeed);
		srand(m_iManualRandomSeed);
	}

	InitializeStructures();

	m_oDataChartMgr.Initialize();
	m_oExternalStimuliMgr.Initialize();
	if(m_lpSimRecorder) m_lpSimRecorder->Initialize();

	m_vsIntersect.Initialize();

	//realize the osg viewer
	m_vsWinMgr->Realize();

	m_bInitialized = TRUE;
}

void VsSimulator::StepSimulation()
{
	//If we are blocking the simulation stepping code for multi-threaded access then exit here.
	if(CheckSimulationBlock())
		return;

	//step the frame and update the windows
	if (!m_bPaused)	
	{
		Simulator::StepSimulation();
		m_vxFrame->step();
	}

	//Get the current frame time
	//To control the frame rate we need to only update the graphics after a given period of time.
	//until that time we need to be free to run physics/neural simulation as fast as possible between frames.
	m_iCurrentFrame = osg::Timer::instance()->tick();
	m_fltFrameDt = osg::Timer::instance()->delta_s(m_iLastFrame, m_iCurrentFrame);
	if( m_fltFrameDt > m_fltFrameStep)
	{
		m_bStopSimulation = !m_vsWinMgr->Update();
		m_iLastFrame = m_iCurrentFrame;
	}
	else if(m_bPaused)
	{
		//However, if we are paused then we want to sleep for the time in between instead of processing.
		float fltRemains = m_fltFrameStep - m_fltFrameDt;
		OpenThreads::Thread::microSleep((unsigned long)(fltRemains*1000000.0));
	}

	CheckEndSimulationTime();
}

void VsSimulator::Simulate()
{
	m_lTimer = Std_GetTick();

	do 
    {
        StepSimulation();
    } 
    while (!m_bStopSimulation && !m_bForceSimulationStop);
	
	m_vxFrame->release();
	m_vxFrame = NULL;
}

void VsSimulator::ShutdownSimulation()
{
	SimStopping();
	m_bForceSimulationStop = TRUE;
}

BOOL VsSimulator::PauseSimulation()
{
	SimPausing();
	m_bPaused = TRUE;
	return TRUE;
}

BOOL VsSimulator::StartSimulation()
{
	SimStarting();
	m_bSimRunning = TRUE;
	m_bPaused = FALSE;
	return TRUE;
}

float *VsSimulator::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "FRAMEDT")
		lpData = &m_fltFrameDt;
	else
	{
		lpData = Simulator::GetDataPointer(strDataType);
		if(!lpData)
			THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);
	}

	return lpData;
}

void VsSimulator::SnapshotStopFrame()
{
	if(m_lpSimStopPoint) delete m_lpSimStopPoint;
	m_lpSimStopPoint = dynamic_cast<KeyFrame *>(CreateObject("AnimatLab", "KeyFrame", "Snapshot"));
	if(!m_lpSimStopPoint)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "KeyFrame");

	m_lpSimStopPoint->StartSlice(m_lTimeSlice);
	m_lpSimStopPoint->EndSlice(m_lTimeSlice);
	m_lpSimStopPoint->Activate();
}

VsSimulator *VsSimulator::ConvertSimulator(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);

	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	return lpVsSim;
}

void VsSimulator::Save(string strFile) 
{
	//Temp code. Lets save it out and make sure the collision stuff is actually correct.
	VxPersistence::saveFrame(strFile.c_str(), VxPersistence::kAutoGenerateGraphics);
}


/*
VsSimulator *VsSimulator::ConvertSimulator(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);

	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	return lpVsSim;
}
*/


/*
__declspec( dllexport ) void RunSimulator(void)
{
	VsSimulator oSim;
	oSim.ProjectPath("C:\\Projects\\bin\\Experiments\\Robot\\");

	oSim.Load("Robot.asim");
	oSim.Initialize(0, NULL);
	oSim.Simulate();
}
*/

}			//VortexAnimatSim
