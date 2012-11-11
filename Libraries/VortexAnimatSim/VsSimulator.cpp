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
#include "VsLight.h"
#include "VsCameraManipulator.h"
#include "VsDragger.h"
#include "MeshMinVertexDistanceVisitor.h"


namespace VortexAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


VsSimulator::VsSimulator()
{
	m_uUniverse = NULL;
	m_grpScene = NULL;	
	m_vsWinMgr = NULL;
	m_vsWinMgr = new VsSimulationWindowMgr;
	m_lpWinMgr = m_vsWinMgr;
	m_lpWinMgr->SetSystemPointers(this, NULL, NULL, NULL, TRUE);
	m_uUniverse = NULL;
	m_vxFrame = NULL;
	m_dblTotalStepTime = 0;
	m_lStepTimeCount = 0;
	m_dblTotalStepTime= 0;
	m_lStepTimeCount = 0;
	m_dblTotalVortexStepTime = 0;
	m_lStepVortexTimeCount = 0;
}

VsSimulator::~VsSimulator()
{

try
{
	m_bShuttingDown = TRUE;

	//Set this to NULL so all of the DeletePhysics calls will not try and remove
	//entities from the universe when shutting down.
	m_uUniverse = NULL;

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

void VsSimulator::CalcCriticalSimParams(BOOL bVal)
{
	Simulator::CalcCriticalSimParams(bVal);
	SetSimulationStabilityParams();
}

void VsSimulator::LinearCompliance(float fltVal, BOOL bUseScaling)
{
	Simulator::LinearCompliance(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::AngularCompliance(float fltVal, BOOL bUseScaling)
{
	Simulator::AngularCompliance(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::LinearDamping(float fltVal, BOOL bUseScaling)
{
	Simulator::LinearDamping(fltVal, bUseScaling);
	if(!m_bCalcCriticalSimParams)
		SetSimulationStabilityParams();
}

void VsSimulator::AngularDamping(float fltVal, BOOL bUseScaling)
{
	Simulator::AngularDamping(fltVal, bUseScaling);
	if(!m_bCalcCriticalSimParams)
		SetSimulationStabilityParams();
}

void VsSimulator::LinearKineticLoss(float fltVal, BOOL bUseScaling)
{
	Simulator::LinearKineticLoss(fltVal, bUseScaling);
	if(!m_bCalcCriticalSimParams)
		SetSimulationStabilityParams();
}

void VsSimulator::AngularKineticLoss(float fltVal, BOOL bUseScaling)
{
	Simulator::AngularKineticLoss(fltVal, bUseScaling);
	if(!m_bCalcCriticalSimParams)
		SetSimulationStabilityParams();
}

void VsSimulator::PhysicsTimeStep(float fltVal)
{
	Simulator::PhysicsTimeStep(fltVal);

	if(m_vxFrame)
	{
		m_vxFrame->setTimeStep(m_fltPhysicsTimeStep);		
		SetSimulationStabilityParams();
	}
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

void VsSimulator::InitializeVortexViewer(int argc, const char **argv)
{
    osg::ArgumentParser arguments(&argc, (char **) argv);

	osg::setNotifyLevel(osg::NotifySeverity::NOTICE);  //ConvertTraceLevelToOSG());

	//osg::notify(osg::NOTICE) << "Setting OSG notice level to '" << Std_GetTraceLevel() << std::endl

    // set up the usage document, in case we need to print out how to use this program.
    arguments.getApplicationUsage()->setApplicationName(arguments.getApplicationName());
    arguments.getApplicationUsage()->setDescription(arguments.getApplicationName()+" is the standard OpenSceneGraph example which loads and visualises 3d models.");
    arguments.getApplicationUsage()->setCommandLineUsage(arguments.getApplicationName()+" [options] filename ...");
    arguments.getApplicationUsage()->addCommandLineOption("-h or --help", "Display this information");

	//Ensure that our exe path is the only place it will attempt to find library files.
	osgDB::FilePathList aryList = osgDB::getLibraryFilePathList();
	string strPath = m_strExecutablePath.substr(0, m_strExecutablePath.length()-1);
	aryList.clear();
	aryList.push_front(strPath);
	osgDB::setLibraryFilePathList(aryList);

    // add resource search paths
    osgDB::getDataFilePathList().push_front("./Resources");
    osgDB::getDataFilePathList().push_front("../Resources");
    osgDB::getDataFilePathList().push_front("../../Resources");

	string strFile = osgDB::findLibraryFile("osgdb_freetype.dll");

	//This is the root of the scenegraph.  Which will corrospond
	//to the root of the simulation
	m_grpScene = new osg::Group;	
    m_grpScene->setName("World");

	//Add the mouse spring lines to the scene
	m_grpScene->addChild(VsMouseSpring::GetInstance()->GetNode());

	//Create the windows, cameras, and Huds
	m_vsWinMgr->Initialize();

	//Create the command manager if needed.
	if(!m_osgCmdMgr.valid())
	    m_osgCmdMgr = new osgManipulator::CommandManager;

    osg::StateSet* rootStateSet = m_grpScene->getOrCreateStateSet();
	rootStateSet->setMode( GL_LIGHTING, osg::StateAttribute::ON );
	rootStateSet->setMode( GL_LIGHT0, osg::StateAttribute::ON );
	//rootStateSet->setMode( GL_LIGHT1, osg::StateAttribute::ON );

	m_oLightMgr.Initialize();
}

void VsSimulator::SetSimulationStabilityParams()
{
	if(m_uUniverse)
	{
	/*	if(m_bCalcCriticalSimParams)
		{*/
			//VxReal halflife = 5;
			//m_uUniverse->getSolverParameters(0)->setConstraintLinearCompliance(m_fltLinearCompliance);
			//m_uUniverse->getSolverParameters(0)->setConstraintAngularCompliance(m_fltAngularCompliance);
			//m_uUniverse->setCriticalConstraintParameters(0, halflife);
		/*}
		else
		{
			m_uUniverse->getSolverParameters(0)->setConstraintLinearCompliance(m_fltLinearCompliance);
			m_uUniverse->getSolverParameters(0)->setConstraintLinearDamping(m_fltLinearDamping);
			m_uUniverse->getSolverParameters(0)->setConstraintLinearKineticLoss(m_fltLinearKineticLoss);
			m_uUniverse->getSolverParameters(0)->setConstraintAngularCompliance(m_fltAngularCompliance);
			m_uUniverse->getSolverParameters(0)->setConstraintAngularDamping(m_fltAngularDamping);
			m_uUniverse->getSolverParameters(0)->setConstraintAngularKineticLoss(m_fltAngularKineticLoss);
		}*/

		TRACE_DETAIL("Reset simulation stability params\r\n");
		TRACE_DETAIL("Angular Compliance: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintAngularCompliance()) + "\r\n");
		TRACE_DETAIL("Angular Damping: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintAngularDamping()) + "\r\n");
		TRACE_DETAIL("Angular Kinetic Loss: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintAngularKineticLoss()) + "\r\n");
		TRACE_DETAIL("Linear Compliance: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintLinearCompliance()) + "\r\n");
		TRACE_DETAIL("Linear Damping: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintLinearDamping()) + "\r\n");
		TRACE_DETAIL("Linear Kinetic Loss: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintLinearKineticLoss()) + "\r\n");
	}
}

//This function initializes the Vortex related
//classes and the vortex viewer.
void VsSimulator::InitializeVortex(int argc, const char **argv)
{
	InitializeVortexViewer(argc, argv);

	int iObjectCount = 100 + m_iPhysicsBodyCount;
	int iCollisionCount = iObjectCount*40;

	//create the frame
	m_vxFrame = VxFrame::instance();
	m_uUniverse = new Vx::VxUniverse(iObjectCount, iCollisionCount);
	m_uUniverse->setGravity(0, m_fltGravity, 0);
	//m_uUniverse->setGravity(0, 0, m_fltGravity);
	m_vxFrame->addUniverse(m_uUniverse);

	//set the frame timestep
	m_vxFrame->setTimeStep(m_fltPhysicsTimeStep);		

	VxFrameRegisterAllInteractions(m_vxFrame);

	SetSimulationStabilityParams();

    // Register the simple callback to be notified at beginning and end of the interaction between parts and sensors.
    m_uUniverse->addIntersectSubscriber(VxUniverse::kResponseSensor, VxUniverse::kResponsePart, VxUniverse::kEventFirst, &m_vsIntersect, 0);
    m_uUniverse->addIntersectSubscriber(VxUniverse::kResponseSensor, VxUniverse::kResponsePart, VxUniverse::kEventDisjoint, &m_vsIntersect, 0);
}

Vx::VxTriangleMesh *VsSimulator::CreatTriangleMeshFromOsg(osg::Node *osgNode)
{
	Vx::VxTriangleMesh *vxMesh = NULL;
	vxMesh = VxTriangleMesh::createFromNode(osgNode);

	return vxMesh;
}

osg::NotifySeverity VsSimulator::ConvertTraceLevelToOSG()
{
	int iLevel = Std_GetTraceLevel();

	switch (iLevel)
	{
	case 0:
		return osg::NotifySeverity::FATAL;
	case 10:
		return osg::NotifySeverity::WARN;
	case 20:
		return osg::NotifySeverity::INFO;
	case 30:
		return osg::NotifySeverity::DEBUG_INFO;
	case 40:
		return osg::NotifySeverity::DEBUG_FP;
	default:
		return osg::NotifySeverity::WARN;
	}
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

void VsSimulator::ConvertV1MeshFile(string strOriginalMeshFile, string strNewMeshFile, string strTexture)
{
	//First load the original mesh in.
	string strPath = this->ProjectPath();
	string strOrigFile = AnimatSim::GetFilePath(strPath, strOriginalMeshFile);
	string strNewFile = AnimatSim::GetFilePath(strPath, strNewMeshFile);
	string strTextFile = "";
	
	if(!Std_IsBlank(strTexture))
		strTextFile = AnimatSim::GetFilePath(strPath, strTexture);

	osg::ref_ptr<osg::Node> osgNode = osgDB::readNodeFile(strOrigFile.c_str());

	//Make sure the mesh loaded is valid.
	if(!osgNode.valid())
		THROW_PARAM_ERROR(Vs_Err_lErrorLoadingMesh, Vs_Err_strErrorLoadingMesh, "Original Mesh file", strOriginalMeshFile);

	CStdFPoint vPos(0, 0, 0), vRot( -(osg::PI/2), 0, 0);
	ApplyVertexTransform(osgNode, SetupMatrix(vPos, vRot));

	////Now add a matrix tranform to rotate about the x axis by -90 degrees.
	//osg::ref_ptr<osg::MatrixTransform> m_osgRotateMT = new osg::MatrixTransform;
	////CStdFPoint vPos(0, 0, 0), vRot( -(osg::PI/2), 0, 0);
	//CStdFPoint vPos(0, 0, 0), vRot( 0, 0, 0);
	//m_osgRotateMT->setMatrix(SetupMatrix(vPos, vRot));

	//m_osgRotateMT->addChild(osgNode.get());

	AddNodeTexture(osgNode, strTextFile, GL_TEXTURE_2D);

	//Now save out the new collision mesh.
	osgDB::writeNodeFile(*osgNode, strNewFile.c_str());
}


//
//float VsSimulator::FindMinMeshVertexDistance(osg::Node *osgNode)
//{
//	MeshMinVertexDistanceVisitor ncv;
//	osgNode->accept(ncv);
//	return ncv.MinVertexDistance();
//}

void VsSimulator::GetPositionAndRotationFromD3DMatrix(float (&aryTransform)[4][4], CStdFPoint &vPos, CStdFPoint &vRot)
{
	osg::Matrix osgMT(aryTransform[0][0], aryTransform[0][1], aryTransform[0][2], aryTransform[0][3], 
					  aryTransform[1][0], aryTransform[1][1], aryTransform[1][2], aryTransform[1][3], 
					  aryTransform[2][0], aryTransform[2][1], aryTransform[2][2], aryTransform[2][3], 
					  aryTransform[3][0], aryTransform[3][1], aryTransform[3][2], aryTransform[3][3]);

	osg::Matrix osgFinal;
	
	osgFinal = osgMT;

	//Lets get the current world coordinates for this body part and then recalculate the 
	//new local position for the part and then finally reset its new local position.
	osg::Vec3 vL = osgFinal.getTrans();
	vPos.Set(vL.x(), vL.y(), vL.z());
	vPos.ClearNearZero();
		
	//Now lets get the euler angle rotation
	Vx::VxReal44 vxTM;
	VxOSG::copyOsgMatrix_to_VxReal44(osgFinal, vxTM);
	Vx::VxTransform vTrans(vxTM);
	Vx::VxReal3 vEuler;
	vTrans.getRotationEulerAngles(vEuler);
	vRot.Set(vEuler[0], vEuler[1] ,vEuler[2]);
	vRot.ClearNearZero();
}

//Timer Methods
unsigned long long VsSimulator::GetTimerTick()
{
	m_lLastTickTaken = osg::Timer::instance()->tick();
	return m_lLastTickTaken;
}

double VsSimulator::TimerDiff_n(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_n(lStart, lEnd);}

double VsSimulator::TimerDiff_u(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_u(lStart, lEnd);}

double VsSimulator::TimerDiff_m(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_m(lStart, lEnd);}

double VsSimulator::TimerDiff_s(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_s(lStart, lEnd);}

void VsSimulator::MicroSleep(unsigned int iMicroTime)
{OpenThreads::Thread::microSleep(iMicroTime);}

void VsSimulator::WriteToConsole(string strMessage)
{
	osg::notify(osg::NOTICE) << strMessage << std::endl;
}

void VsSimulator::Initialize(int argc, const char **argv)
{
	InitializeVortex(argc, argv);

	InitializeStructures();

	m_oDataChartMgr.Initialize();
	m_oExternalStimuliMgr.Initialize();
	if(m_lpSimRecorder) m_lpSimRecorder->Initialize();

	//realize the osg viewer
	m_vsWinMgr->Realize();

	m_bInitialized = TRUE;
}

void VsSimulator::StepSimulation()
{
	if(m_lTimeSlice > 10 && m_lTimeSlice < 5000 && !m_timePeriod.TimerStarted())
		m_timePeriod.StartTimer();


	try
	{ 
		//step the frame and update the windows
		if (!m_bPaused)	
		{
			Simulator::StepSimulation();

			unsigned long long lStart = GetTimerTick();
			m_vxFrame->step();
			double dblVal = TimerDiff_s(lStart, GetTimerTick());
			m_fltPhysicsStepTime += dblVal;

			if(m_lTimeSlice > 10 && m_lTimeSlice < 5000)
			{
				m_dblTotalVortexStepTime += dblVal;
				m_lStepVortexTimeCount++;
			}
			else if(m_lTimeSlice == 5000)
			{
				double dblAvgStepTime = m_dblTotalVortexStepTime/m_lStepVortexTimeCount;
				osg::notify(osg::NOTICE) << "Average step time: " << dblAvgStepTime << std::endl;
				osg::notify(osg::NOTICE) << "Total vortex step time: " << m_dblTotalVortexStepTime << std::endl;
				osg::notify(osg::NOTICE) << "Slice Time: " << m_lTimeSlice << std::endl;
				osg::notify(osg::NOTICE) << "Sim Time: " << Time() << std::endl;
			}
		}

	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while step the simulation.\nError: " + oError.m_strError;
		HandleNonCriticalError(strError);
	}


	//double dblVal2 = m_timeSimulationStep.StopTimer();
	//if(m_lTimeSlice > 10 && m_lTimeSlice < 5000)
	//{
	//	m_dblTotalStepTime += dblVal2;
	//	m_lStepTimeCount++;
	//}
	//else if(m_lTimeSlice == 5000)
	//{
	//	double dblAvgStepTime = m_dblTotalStepTime/m_lStepTimeCount;
	//	cout << "Average step time: " << dblAvgStepTime << std::endl;
	//	cout << "Total step time: " << m_dblTotalStepTime << ", " << m_lStepTimeCount << std::endl;
	//	cout << "Period time: " << m_timePeriod.StopTimer() << std::endl;
	//	cout << "Slice Time: " << m_lTimeSlice << std::endl;
	//	cout << "Sim Time: " << Time() << std::endl;
	//}

}

void VsSimulator::UpdateSimulationWindows()
{
	m_bStopSimulation = !m_vsWinMgr->Update();
}


void VsSimulator::SimulateEnd()
{
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
	m_lStartSimTick = GetTimerTick();

	SimStarting();
	m_bSimRunning = TRUE;
	m_bPaused = FALSE;
	return TRUE;
}

float *VsSimulator::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	//if(strType == "FRAMEDT")
	//	lpData = &m_fltFrameDt;
	//else
	//{
		lpData = Simulator::GetDataPointer(strDataType);
		if(!lpData)
			THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);
	//}

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
	string strOsgFile = strFile + ".osg";
	string strVxFile = strFile + ".vxf";
	std::string strUcon = strFile + ".ucon";

	//Temp code. Lets save it out and make sure the collision stuff is actually correct.
	try
	{
		VxPersistence::saveFrame(strVxFile.c_str(), VxPersistence::kStandard);
	}
	catch(std::exception ex)
	{
		int i= 5;
	}
	catch(...)
	{
		int i= 5;
	}

	m_uUniverse->printContent(strUcon.c_str());

	osgDB::writeNodeFile(*OSGRoot(), strOsgFile.c_str());
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
