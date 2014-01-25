// BlTerrain.cpp: implementation of the BlTerrain class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlOsgGeometry.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlMeshBase.h"
#include "BlTerrain.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlTerrain::BlTerrain()
{
	SetThisPointers();
	m_bCullBackfaces = true; //we want back face culling on by default for Terrains.
	m_osgHeightField = NULL;
}

BlTerrain::~BlTerrain()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlTerrain/\r\n", "", -1, false, true);}
}

void BlTerrain::CreateGraphicsGeometry()
{
	m_osgGeometry = CreatePlaneGeometry(-1, -1, 2, 2, 2, 2, false);
}

void BlTerrain::SetTexture(std::string strTexture)
{
	if(m_osgMeshNode.valid())
	{
		if(!Std_IsBlank(strTexture))
		{
			std::string strFile = AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), strTexture);
			osg::ref_ptr<osg::Image> image = osgDB::readImageFile(strFile);
			if(!image)
				THROW_PARAM_ERROR(Bl_Err_lTextureLoad, Bl_Err_strTextureLoad, "Image File", strFile);

			osg::StateSet* state = m_osgMeshNode->getOrCreateStateSet();
			m_osgTexture = new osg::Texture2D(image.get());
		    m_osgTexture->setDataVariance(osg::Object::DYNAMIC); // protect from being optimized away as static state.

			m_osgTexture->setFilter(osg::Texture2D::MIN_FILTER,osg::Texture2D::LINEAR_MIPMAP_LINEAR);
			m_osgTexture->setFilter(osg::Texture2D::MAG_FILTER,osg::Texture2D::LINEAR);
			m_osgTexture->setWrap(osg::Texture2D::WRAP_S, osg::Texture2D::REPEAT);
			m_osgTexture->setWrap(osg::Texture2D::WRAP_T, osg::Texture2D::REPEAT);

			osg::Matrixd matrix;
			matrix.makeScale(osg::Vec3(m_iTextureLengthSegments, m_iTextureWidthSegments, 1.0)); 

			osg::ref_ptr<osg::TexMat> matTexture = new osg::TexMat;
			matTexture->setMatrix(matrix); 

			state->setTextureAttributeAndModes(0, m_osgTexture.get());
			state->setTextureAttributeAndModes(0, matTexture.get(), osg::StateAttribute::ON); 

			state->setTextureMode(0, m_eTextureMode, osg::StateAttribute::ON);
			state->setMode(GL_BLEND,osg::StateAttribute::ON);
			
			//state->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
		}
		else if(m_osgTexture.valid()) //If we have already set it and we are clearing it then reset the state
		{
			m_osgTexture.release();
			osg::StateSet* state = m_osgMeshNode->getOrCreateStateSet();
			state->setTextureAttributeAndModes(0, NULL); 
			state->setTextureMode(0, m_eTextureMode, osg::StateAttribute::OFF);
		}
	}
}

//Terrains can never have fluid interactions/dynamics.
void BlTerrain::Physics_FluidDataChanged()
{}

void BlTerrain::LoadMeshNode()
{
	std::string strPath = m_lpThisAB->GetSimulator()->ProjectPath();
	std::string strMeshFile = m_lpThisMesh->MeshFile();
	std::string strFile = AnimatSim::GetFilePath(strPath, strMeshFile);

	//Get the terrain node loaded in.
	m_osgBaseMeshNode = CreateHeightField(strFile, m_fltSegmentWidth, m_fltSegmentLength, m_fltMaxHeight, &m_osgHeightField);

	osg::Matrix osgScaleMatrix = osg::Matrix::identity();
	m_osgMeshNode = new osg::MatrixTransform(osgScaleMatrix);

	m_osgMeshNode->addChild(m_osgBaseMeshNode.get());
	m_osgMeshNode->setName(m_lpThisAB->Name() + "_MeshNode");

	SetTexture(m_lpThisRB->Texture());
}

void BlTerrain::CreatePhysicsGeometry()
{
	if(m_osgHeightField)
	{
        DeleteCollisionGeometry();

        //Mass of a terrain is always zero because it is always static.
        m_fltMass = 0;
		m_btHeightField = CreateBtHeightField(m_osgHeightField, m_fltSegmentWidth, m_fltSegmentLength, 0, 0, 0);
        m_eBodyType = TERRAIN_SHAPE_PROXYTYPE;
		m_btCollisionShape = m_btHeightField;
	}

	m_eControlType = DynamicsControlType::ControlNode;  //This is not a dynamic part.

	if(!m_btHeightField)
		THROW_TEXT_ERROR(Bl_Err_lCreatingGeometry, Bl_Err_strCreatingGeometry, "Body: " + m_lpThisAB->Name() + " Mesh: " + AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), m_lpThisMesh->MeshFile()));
}

void BlTerrain::CreateParts()
{
	CreateGeometry();

	BlMeshBase::CreateItem();
	Terrain::CreateParts();
}

void BlTerrain::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Terrain::CreateJoints();
	BlMeshBase::Initialize();
}

void BlTerrain::CreateDynamicPart()
{
    BlSimulator *lpSim = GetBlSimulator();

	if(lpSim && m_lpThisRB && m_lpThisAB && m_btCollisionShape)
	{
        btScalar mass( m_lpThisRB->GetMassValueWithStaticChildren() );
	    btVector3 localInertia( 0, 0, 0 );
        const bool isDynamic = ( mass != 0.f );
	    if( isDynamic )
		    m_btCollisionShape->calculateLocalInertia( mass, localInertia );

        //Keep a copy of the matrix transform for osgMT so I can reset it back later
        osg::Matrix osgMTmat = m_osgMT->getMatrix();

        // Create MotionState to control OSG subgraph visual reprentation transform
        // from a Bullet world transform. To do this, the MotionState need the address
        // of the Transform node (must be either AbsoluteModelTransform or
        // MatrixTransform), center of mass, scale vector, and the parent (or initial)
        // transform (usually the non-scaled OSG local-to-world matrix obtained from
        // the parent node path).
        m_osgbMotion = new osgbDynamics::MotionState();
        m_osgbMotion->setTransform( m_osgMT.get() );

        osg::Vec3 com;
		CStdFPoint vCOM = m_lpThisRB->CenterOfMass();
		if(vCOM.x != 0 || vCOM.y != 0 || vCOM.z != 0)
			com = osg::Vec3(vCOM.x, vCOM.y, vCOM.z);
        else
            com = osg::Vec3(0, 0, 0);
        m_osgbMotion->setCenterOfMass( com );

        m_osgbMotion->setScale( osg::Vec3( 1., 1., 1. ) );
        m_osgbMotion->setParentTransform( osg::Matrix::identity() );

        // Finally, create rigid body.
        btRigidBody::btRigidBodyConstructionInfo rbInfo( mass, m_osgbMotion, m_btCollisionShape, localInertia );
        rbInfo.m_friction = btScalar( 1 );
        rbInfo.m_restitution = btScalar( 1 );
        rbInfo.m_linearDamping = m_lpThisRB->LinearVelocityDamping();
        rbInfo.m_angularDamping = m_lpThisRB->AngularVelocityDamping();

        m_btPart = new btRigidBody( rbInfo );
        m_btPart->setUserPointer((void *) m_lpThisRB);
        m_btPart->setContactProcessingThreshold(10000);

        // Last thing to do: Position the rigid body in the world coordinate system. The
        // MotionState has the initial (parent) transform, and also knows how to account
        // for center of mass and scaling. Get the world transform from the MotionState,
        // then set it on the rigid body, which in turn sets the world transform on the
        // MotionState, which in turn transforms the OSG subgraph visual representation.
        btTransform wt = osgbCollision::asBtTransform(osgMTmat);
        m_osgbMotion->setWorldTransform( wt );
        m_btPart->setWorldTransform( wt );

		//if this body is frozen; freeze it
        if(m_lpThisRB->Freeze())
            m_btPart->setActivationState(0);
        else
            m_btPart->setActivationState(ACTIVE_TAG);

        lpSim->DynamicsWorld()->addRigidBody( m_btPart );

        //FIX PHYSICS
		// Create the physics object.
		//m_vxPart = new VxPart;
		//m_vxSensor = m_vxPart;
		//m_vxSensor->setUserData((void*) m_lpThisRB);
		//int iMaterialID = m_lpThisAB->GetSimulator()->GetMaterialID(m_lpThisRB->MaterialID());

		//m_vxSensor->setName(m_lpThisAB->ID().c_str());               // Give it a name.
		//m_vxSensor->setControl(ConvertControlType());  // Set it to dynamic.
		//CollisionGeometry(m_vxSensor->addGeometry(m_vxGeometry, iMaterialID, 0, m_lpThisRB->Density()));
           
        if(m_lpThisRB->DisplayDebugCollisionGraphic())
        {
            m_osgDebugNode = osgbCollision::osgNodeFromBtCollisionShape( m_btCollisionShape );
            m_osgDebugNode->setName(m_lpThisRB->Name() + "_Debug");
	        m_osgNodeGroup->addChild(m_osgDebugNode.get());	
        }

        GetBaseValues();
	}
}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
