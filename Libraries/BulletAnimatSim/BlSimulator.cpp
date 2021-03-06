// BlSimulator.cpp: implementation of the BlSimulator class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "BlOsgGeometry.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlClassFactory.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


BlSimulator::BlSimulator()
{
    m_lpCollisionConfiguration = NULL;
    m_lpDispatcher = NULL;
    m_lpSolver = NULL;
    m_lpBroadPhase = NULL;
    m_lpDynamicsWorld = NULL;

	m_grpScene = NULL;	
	m_vsWinMgr = NULL;
	m_vsWinMgr = new OsgSimulationWindowMgr;
	m_lpWinMgr = m_vsWinMgr;
	m_lpWinMgr->SetSystemPointers(this, NULL, NULL, NULL, true);
	m_dblTotalStepTime = 0;
	m_lStepTimeCount = 0;
	m_dblTotalStepTime= 0;
	m_lStepTimeCount = 0;
	m_dblTotalVortexStepTime = 0;
	m_lStepVortexTimeCount = 0;
	m_lpMeshMgr = NULL;
	m_osgAlphafunc = NULL;
	m_iSubstepCallbackCount = 0;

    //Setup the global matrix util.
    m_lpMatrixUtil = new OsgMatrixUtil;
    //m_lpMatrixUtil = new BlMatrixUtil;
    SetMatrixUtil(m_lpMatrixUtil);

	if(!m_lpAnimatClassFactory) 
		m_lpAnimatClassFactory = new BlClassFactory;

    m_bDrawDebug = false;
}

BlSimulator::~BlSimulator()
{

try
{
	if(m_lpMeshMgr)
	{
		delete m_lpMeshMgr;
		m_lpMeshMgr = NULL;
	}

	m_bShuttingDown = true;

	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Simulator\r\n", "", -1, false, true);}
}

#pragma region MutatorOverrides

void BlSimulator::StabilityScale(float fltVal)
{
	OsgSimulator::StabilityScale(fltVal);
	SetSimulationStabilityParams();
}

void BlSimulator::LinearCompliance(float fltVal, bool bUseScaling)
{
	OsgSimulator::LinearCompliance(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void BlSimulator::AngularCompliance(float fltVal, bool bUseScaling)
{
	OsgSimulator::AngularCompliance(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void BlSimulator::LinearDamping(float fltVal, bool bUseScaling)
{
	OsgSimulator::LinearDamping(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void BlSimulator::AngularDamping(float fltVal, bool bUseScaling)
{
	OsgSimulator::AngularDamping(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void BlSimulator::LinearKineticLoss(float fltVal, bool bUseScaling)
{
	OsgSimulator::LinearKineticLoss(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void BlSimulator::AngularKineticLoss(float fltVal, bool bUseScaling)
{
	OsgSimulator::AngularKineticLoss(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void BlSimulator::Gravity(float fltVal, bool bUseScaling)
{
	OsgSimulator::Gravity(fltVal, bUseScaling);

	if(m_lpDynamicsWorld)
        m_lpDynamicsWorld->setGravity( btVector3( 0, m_fltGravity, 0 ) );
}

#pragma endregion


SimulationRecorder *BlSimulator::CreateSimulationRecorder()
{
//NEED_TO_FIX
	return NULL; //new VsSimulationRecorder;
}

void BlSimulator::Reset()
{
	OsgSimulator::Reset();

    if(m_lpDynamicsWorld)
    {
        delete m_lpDynamicsWorld;
        m_lpDynamicsWorld = NULL;
    }
	
    if(m_lpSolver)
    {
    	delete m_lpSolver;
        m_lpSolver = NULL;
    }
	
    if(m_lpBroadPhase)
    {
    	delete m_lpBroadPhase;
        m_lpBroadPhase = NULL;
    }
	
    if(m_lpDispatcher)
    {
    	delete m_lpDispatcher;
        m_lpDispatcher = NULL;
    }

    if(m_lpCollisionConfiguration)
    {
    	delete m_lpCollisionConfiguration;
        m_lpCollisionConfiguration = NULL;
    }

	if(!m_lpAnimatClassFactory) 
		m_lpAnimatClassFactory = new BlClassFactory;

    m_aryFluidPlanes.clear();
}

void BlSimulator::ResetSimulation()
{
	OsgSimulator::ResetSimulation();

	m_bSimRunning = false;
}

/////MyContactCallback is just an example to show how to get access to the child shape that collided
//bool AnimatContactCallback (
//    btManifoldPoint& cp,
//    const btCollisionObject* colObj0,
//    int partId0,
//    int index0,
//    const btCollisionObject* colObj1,
//    int partId1,
//    int index1)
//{
//
//	//if (colObj0->getRootCollisionShape()->getShapeType()==COMPOUND_SHAPE_PROXYTYPE)
//	//{
//	//	btCompoundShape* compound = (btCompoundShape*)colObj0->getRootCollisionShape();
//	//	btCollisionShape* childShape;
//	//	childShape = compound->getChildShape(index0);
//	//}
//
//	//if (colObj1->getRootCollisionShape()->getShapeType()==COMPOUND_SHAPE_PROXYTYPE)
//	//{
//	//	btCompoundShape* compound = (btCompoundShape*)colObj1->getRootCollisionShape();
//	//	btCollisionShape* childShape;
//	//	childShape = compound->getChildShape(index1);
//	//}
//
//	return true;
//}

bool AnimatContactCallback(btManifoldPoint& cp, void* body0, void* body1)
{
    btCollisionObject *lpBtBody1 = (btCollisionObject *) body0;
    btCollisionObject *lpBtBody2 = (btCollisionObject *) body1;

    //Only process these if they both exist and one of them has custom material callback flag set.
    if( lpBtBody1 && lpBtBody2 &&
        ( (lpBtBody1->getCollisionFlags() & btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK) ||
         (lpBtBody2->getCollisionFlags() & btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK)) )
    {
        BlBulletData *lpData1 = (BlBulletData *) lpBtBody1->getUserPointer();
        BlBulletData *lpData2 = (BlBulletData *) lpBtBody2->getUserPointer();

        if(lpData1 && lpData1->m_lpBody && lpData2 && lpData2->m_lpBody)
        {
            if(lpBtBody1->getCollisionFlags() & btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK)
                lpData1->m_lpBody->m_aryContactPoints.Add(new BlContactPoint(&cp, lpData2->m_lpBody, true));

            if(lpBtBody2->getCollisionFlags() & btCollisionObject::CF_CUSTOM_MATERIAL_CALLBACK)
                lpData2->m_lpBody->m_aryContactPoints.Add(new BlContactPoint(&cp, lpData1->m_lpBody, false));
        }
    }

    return true;
}

void BlSimulator::InitializeBulletViewer(int argc, const char **argv)
{
    //osg::ArgumentParser arguments(&argc, (char **) argv);

	osg::setNotifyLevel(osg::NotifySeverity::NOTICE);  //ConvertTraceLevelToOSG());
	//osg::setNotifyLevel(osg::NotifySeverity::DEBUG_INFO);  //ConvertTraceLevelToOSG());

	//osg::notify(osg::NOTICE) << "Setting OSG notice level to '" << Std_GetTraceLevel() << std::endl

    // set up the usage document, in case we need to print out how to use this program.
    //arguments.getApplicationUsage()->setApplicationName(arguments.getApplicationName());
    //arguments.getApplicationUsage()->setDescription(arguments.getApplicationName()+" is the standard OpenSceneGraph example which loads and visualises 3d models.");
    //arguments.getApplicationUsage()->setCommandLineUsage(arguments.getApplicationName()+" [options] filename ...");
    //arguments.getApplicationUsage()->addCommandLineOption("-h or --help", "Display this information");

	//Ensure that our exe path is the only place it will attempt to find library files.
	osgDB::FilePathList aryList = osgDB::getLibraryFilePathList();
	std::string strPath = m_strExecutablePath.substr(0, m_strExecutablePath.length()-1);
	aryList.clear();
	aryList.push_front(strPath);
	osgDB::setLibraryFilePathList(aryList);

    // add resource search paths
    osgDB::getDataFilePathList().push_front("./Resources");
    osgDB::getDataFilePathList().push_front("../Resources");
    osgDB::getDataFilePathList().push_front("../../Resources");

#ifndef WIN32
    osgDB::getDataFilePathList().push_front("/usr/share/fonts/truetype");
#endif

	std::string strFile = osgDB::findLibraryFile("osgdb_freetype.dll");

	//This is the root of the scenegraph.  Which will corrospond
	//to the root of the simulation
	m_grpScene = new osg::MatrixTransform;
    m_grpScene->setMatrix(osg::Matrix::identity());
    m_grpScene->setName("World");

	//Add the mouse spring lines to the scene
	m_grpScene->addChild(m_lpMouseSpring->GetNode());

	//Create the windows, cameras, and Huds
	m_vsWinMgr->Initialize();

	//Create the command manager if needed.
	if(!m_osgCmdMgr.valid())
	    m_osgCmdMgr = new osgManipulator::CommandManager;

    osg::StateSet* rootStateSet = m_grpScene->getOrCreateStateSet();
	rootStateSet->setMode( GL_LIGHTING, osg::StateAttribute::ON );
	rootStateSet->setMode( GL_LIGHT0, osg::StateAttribute::ON );
	//rootStateSet->setMode( GL_LIGHT1, osg::StateAttribute::ON );

    // set up an alphafunc by default to speed up blending operations.
    m_osgAlphafunc = new osg::AlphaFunc;
    m_osgAlphafunc->setFunction(osg::AlphaFunc::GEQUAL, m_fltAlphaThreshold);
    rootStateSet->setAttributeAndModes(m_osgAlphafunc, osg::StateAttribute::ON);

	m_oLightMgr.Initialize();
}

void BlSimulator::SetSimulationStabilityParams()
{

}

void BlSimulator::BulletStepFinished(btScalar timeStep)
{
	m_iSubstepCallbackCount++;
	if(m_iPhysicsSubsteps == m_iSubstepCallbackCount)
	{
		AfterStepSimulation();
		m_iSubstepCallbackCount = 0;
	}
}

void ProcessTickCallback(btDynamicsWorld *world, btScalar timeStep) 
{
    BlSimulator *w = static_cast<BlSimulator *>(world->getWorldUserInfo());
    w->BulletStepFinished(timeStep);
}

//This function initializes the Vortex related
//classes and the vortex viewer.
void BlSimulator::InitializeBullet(int argc, const char **argv)
{
	InitializeBulletViewer(argc, argv);

	int iObjectCount = 100 + m_iPhysicsBodyCount;
	int iCollisionCount = iObjectCount*40;

    m_lpCollisionConfiguration = new btDefaultCollisionConfiguration();
    m_lpDispatcher = new BlAnimatCollisionDispatcher(m_lpCollisionConfiguration, this);

    m_lpSolver = new btSequentialImpulseConstraintSolver;
    //btDantzigSolver* mlcp = new btDantzigSolver();
    //m_lpSolver = new btMLCPSolver(mlcp);

    btVector3 worldAabbMin( -10000, -10000, -10000 );
    btVector3 worldAabbMax( 10000, 10000, 10000 );
    m_lpBroadPhase = new btAxisSweep3( worldAabbMin, worldAabbMax, 1000 );

    m_lpDynamicsWorld = new btDiscreteDynamicsWorld( m_lpDispatcher, m_lpBroadPhase, m_lpSolver, m_lpCollisionConfiguration );
    m_lpDynamicsWorld->setGravity( btVector3( 0, m_fltGravity, 0 ) );

	m_lpDynamicsWorld->setInternalTickCallback(ProcessTickCallback, static_cast<void *>(this));

    if(m_bDrawDebug)
    {
        this->OSGRoot()->addChild(m_dbgDraw.getSceneGraph());
        m_lpDynamicsWorld->setDebugDrawer( &m_dbgDraw );
    }

	//gContactAddedCallback = &AnimatContactCallback;
    gContactProcessedCallback = &AnimatContactCallback;
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
void BlSimulator::GenerateCollisionMeshFile(std::string strOriginalMeshFile, std::string strCollisionMeshFile, float fltScaleX, float fltScaleY, float fltScaleZ)
{
	//First load the original mesh in.
	std::string strPath = this->ProjectPath();
	std::string strOrigFile = AnimatSim::GetFilePath(strPath, strOriginalMeshFile);
	std::string strNewFile = AnimatSim::GetFilePath(strPath, strCollisionMeshFile);

	osg::ref_ptr<osg::Node> osgNode = MeshMgr()->LoadMesh(strOrigFile); //osgDB::readNodeFile(strOrigFile.c_str());

	//Make sure the mesh loaded is valid.
	if(!osgNode.valid())
		THROW_PARAM_ERROR(Bl_Err_lErrorLoadingMesh, Bl_Err_strErrorLoadingMesh, "Original Mesh file", strOriginalMeshFile);
     
	//Now create a convex mesh with the physics engine using the loaded mesh.
	btConvexHullShape *btHull = OsgConvexShrunkenHullCollisionShape(osgNode.get()); 
      
    if(!btHull)
		THROW_PARAM_ERROR(Bl_Err_lConvertingMeshToConvexHull, Bl_Err_strConvertingMeshToConvexHull, "Original Mesh file", strOriginalMeshFile);

	//Now use that convexmesh geometry to create a new osg node.
    osg::ref_ptr<osg::Node> osgNewNode = osgbCollision::osgNodeFromBtCollisionShape( btHull );

	osg::Matrix osgScale = osg::Matrix::scale(fltScaleX, fltScaleY, fltScaleZ);
	osg::ref_ptr<osg::MatrixTransform> osgScaleMT = new osg::MatrixTransform(osgScale);
	osgScaleMT->addChild(osgNewNode.get());
	osgScaleMT->setDataVariance(osg::Object::STATIC);

    delete btHull;
    btHull = NULL;

	// Now do some OSG voodoo, which should spread the transform downward
	//  through the loaded model, and delete the transform.
	osgUtil::Optimizer optimizer;
	optimizer.optimize(osgScaleMT.get(), osgUtil::Optimizer::FLATTEN_STATIC_TRANSFORMS);

	//Now save out the new collision mesh.
	osgDB::writeNodeFile(*osgNewNode, strNewFile.c_str());

	//Make sure we stamp the new file tim on the file. For some reason osgDB is not settign that correctly.
	//If we do not do this then the mesh mgr will not recognize that it has changed, and will not load it.
	Std_SetFileTime(strNewFile);
}

void BlSimulator::ConvertV1MeshFile(std::string strOriginalMeshFile, std::string strNewMeshFile, std::string strTexture)
{
	//First load the original mesh in.
	std::string strPath = this->ProjectPath();
	std::string strOrigFile = AnimatSim::GetFilePath(strPath, strOriginalMeshFile);
	std::string strNewFile = AnimatSim::GetFilePath(strPath, strNewMeshFile);
	std::string strTextFile = "";
	
	if(!Std_IsBlank(strTexture))
		strTextFile = AnimatSim::GetFilePath(strPath, strTexture);

	osg::ref_ptr<osg::Node> osgNode = osgDB::readNodeFile(strOrigFile.c_str());

	//Make sure the mesh loaded is valid.
	if(!osgNode.valid())
		THROW_PARAM_ERROR(Bl_Err_lErrorLoadingMesh, Bl_Err_strErrorLoadingMesh, "Original Mesh file", strOriginalMeshFile);

	CStdFPoint vPos(0, 0, 0), vRot( -(osg::PI/2), 0, 0);
	ApplyVertexTransform(osgNode.get(), SetupMatrix(vPos, vRot));
	AddNodeTexture(osgNode.get(), strTextFile, GL_TEXTURE_2D);

	//Now save out the new collision mesh.
	osgDB::writeNodeFile(*osgNode, strNewFile.c_str());
}


void BlSimulator::GetPositionAndRotationFromD3DMatrix(float (&aryTransform)[4][4], CStdFPoint &vPos, CStdFPoint &vRot)
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

    vRot = EulerRotationFromMatrix(osgFinal);
}

void BlSimulator::Initialize(int argc, const char **argv)
{
	InitializeBullet(argc, argv);

    OsgSimulator::Initialize(argc, argv);
}

void BlSimulator::StepSimulation()
{
	////Test Code
	//if(m_lTimeSlice > 10 && m_lTimeSlice < 5000 && !m_timePeriod.TimerStarted())
	//	m_timePeriod.StartTimer();


	try
	{ 
		//step the frame and update the windows
		if (!m_bPaused)	
		{
			OsgSimulator::StepSimulation();

			unsigned long long lStart = GetTimerTick();

            if( m_bDrawDebug )
                m_dbgDraw.BeginDraw();

            m_lpDynamicsWorld->stepSimulation(m_fltPhysicsTimeStep, m_iPhysicsSubsteps, m_fltPhysicsSubstepTime);

            if( m_bDrawDebug )
            {
                m_lpDynamicsWorld->debugDrawWorld();
                m_dbgDraw.EndDraw();
            }

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

        //PauseSimulation();

	}
	catch(CStdErrorInfo oError)
	{
		std::string strError = "An error occurred while step the simulation.\nError: " + oError.m_strError;
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


void BlSimulator::SimulateEnd()
{
    Reset();
}

bool BlSimulator::HasFluidPlane(FluidPlane *lpPlane)
{
  std::list<FluidPlane *>::iterator it;
  for (it=m_aryFluidPlanes.begin(); it!=m_aryFluidPlanes.end(); ++it)
     if(*it == lpPlane)
        return true;

  return false;
}

bool compare_fluid_planes (FluidPlane *first, FluidPlane *second)
{
  if(first && second)
      return (first->Height() < second->Height());

  return false;
}

void BlSimulator::AddFluidPlane(FluidPlane *lpPlane)
{
    if(!HasFluidPlane(lpPlane))
    {
        m_aryFluidPlanes.push_back(lpPlane);
        m_aryFluidPlanes.sort(compare_fluid_planes);
    }
}

void BlSimulator::RemoveFluidPlane(FluidPlane *lpPlane)
{
    if(HasFluidPlane(lpPlane))
    {
        m_aryFluidPlanes.remove(lpPlane);
        m_aryFluidPlanes.sort(compare_fluid_planes);
    }
}

void BlSimulator::SortFluidPlanes()
{
    m_aryFluidPlanes.sort(compare_fluid_planes);
}

/**
 \brief Searches the fluid planes starting at the lowest height and working its
 way up to find the first one that depth is under. This is the density we will use
 for hydrodynamics.

 \author    David Cofer
 \date  10/20/2013

 \param fltDepth    Depth to search for with the fluid plane.

 \return    null if it fails, else the found fluid plane for depth.
 */

FluidPlane *BlSimulator::FindFluidPlaneForDepth(float fltDepth)
{
    std::list<FluidPlane *>::iterator it;
    for (it=m_aryFluidPlanes.begin(); it!=m_aryFluidPlanes.end(); ++it)
        if(fltDepth <= (*it)->Height())
        return (*it);

    return NULL;
}

}			//BulletAnimatSim
