#include "StdAfx.h"
#include <stdarg.h>
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsOrganism.h"
#include "VsStructure.h"
#include "VsClassFactory.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"

//#include "VsSimulationRecorder.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsBody::VsBody()
{
	m_eControlType = VxEntity::kControlDynamic; //Default to dynamic control of the rigid body.
	m_bCullBackfaces = FALSE; //No backface culling by default.
	m_eTextureMode = GL_TEXTURE_2D;
}



VsBody::~VsBody()
{

try
{
	DeleteGraphics();
	DeletePhysics();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsBody\r\n", "", -1, FALSE, TRUE);}
}

string VsBody::Physics_ID()
{
	if(m_lpThis)
		return m_lpThis->ID();
	else
		return "";
}

#pragma region Selection-Code

void VsBody::Physics_Selected(BOOL bValue, BOOL bSelectMultiple)  
{
	//If selecting and not already selected then select it
	bool bNodeFound = m_osgNodeGroup->containsNode(m_osgSelectedGroup.get());
	if(bValue && !bNodeFound)
	{
		m_osgNodeGroup->addChild(m_osgSelectedGroup.get());
		m_osgDragger->AddToScene();
	}
	//if de-selecting and selected then de-select the node
	else if(!bValue && bNodeFound)
	{
		m_osgNodeGroup->removeChild(m_osgSelectedGroup.get());
		m_osgDragger->RemoveFromScene();
	}
}

void VsBody::CreateSelectedGraphics(string strName)
{
	m_osgSelectedGroup = new osg::Group();
	m_osgSelectedGroup->setName(strName + "_SelectedGroup");
    m_osgSelectedGroup->addChild(m_osgNode.get());  

    // set up the state so that the underlying color is not seen through
    // and that the drawing mode is changed to wireframe, and a polygon offset
    // is added to ensure that we see the wireframe itself, and turn off 
    // so texturing too.
    osg::StateSet* stateset = new osg::StateSet;
    osg::PolygonOffset* polyoffset = new osg::PolygonOffset;
    polyoffset->setFactor(-1.0f);
    polyoffset->setUnits(-1.0f);
    osg::PolygonMode* polymode = new osg::PolygonMode;
    polymode->setMode(osg::PolygonMode::FRONT_AND_BACK,osg::PolygonMode::LINE);
    stateset->setAttributeAndModes(polyoffset,osg::StateAttribute::OVERRIDE|osg::StateAttribute::ON);
    stateset->setAttributeAndModes(polymode,osg::StateAttribute::OVERRIDE|osg::StateAttribute::ON);

    osg::Material* material = new osg::Material;
    stateset->setAttributeAndModes(material,osg::StateAttribute::OVERRIDE|osg::StateAttribute::ON);
    stateset->setMode(GL_LIGHTING,osg::StateAttribute::OVERRIDE|osg::StateAttribute::OFF);

    stateset->setTextureMode(0,GL_TEXTURE_2D,osg::StateAttribute::OVERRIDE|osg::StateAttribute::OFF);
    
    m_osgSelectedGroup->setStateSet(stateset);

	CreateDragger(strName);
}

void VsBody::CreateDragger(string strName)
{
	string strVers = osgGetSOVersion();  

	if(m_lpThis->GetSimulator())
	{
		VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThis->GetSimulator());
		if(!lpVsSim)
			THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

		if(lpVsSim->OsgCmdMgr())
		{
			m_osgDragger = new VsDragger(this);
			m_osgDragger->setName(strName + "_Dragger");

			m_osgDragger->setupDefaultGeometry();

			lpVsSim->OsgCmdMgr()->connect(*m_osgDragger, *m_osgMT);

			//Add pointers to this object to the grip so it will no which body part to
			//call the EndGripDrag method on when the drag is finished.
			m_osgDragger->setUserData(new VsOsgUserData(this));
		}
	}
}

#pragma endregion

void VsBody::LocalMatrix(osg::Matrix osgLocalMT)
{
	m_osgLocalMatrix = osgLocalMT;
	m_osgFinalMatrix = osgLocalMT;
}

void VsBody::AttachedPartMovedOrRotated(string strID)
{
	ResetGraphicsAndPhysics();
}

void VsBody::DeleteGraphics()
{
	if(m_osgParent.valid() && m_osgRoot.valid())
	{
		if(m_osgParent->containsNode(m_osgRoot.get()))
			m_osgParent->removeChild(m_osgRoot.get());
	}

	if(m_osgCull.valid()) m_osgCull.release();
	if(m_osgTexture.valid()) m_osgTexture.release();
	if(m_osgStateSet.valid()) m_osgStateSet.release();
	if(m_osgMaterial.valid()) m_osgMaterial.release();

	if(m_osgGeometry.valid()) m_osgGeometry.release();
	if(m_osgNode.valid()) m_osgNode.release();
	if(m_osgSelectedGroup.valid()) m_osgSelectedGroup.release();
	if(m_osgNodeGroup.valid()) m_osgNodeGroup.release();
	if(m_osgMT.valid()) m_osgMT.release();
	if(m_osgRoot.valid()) m_osgRoot.release();
	if(m_osgParent.valid()) m_osgParent.release();
}

CStdFPoint VsBody::GetOSGWorldCoords()
{
	return GetOSGWorldCoords(m_osgMT.get());
}

CStdFPoint VsBody::GetOSGWorldCoords(osg::MatrixTransform *osgMT)
{
	WorldCoordinateNodeVisitor ncv;
	osgMT->accept(ncv);
	osg::Vec3 vCoord = ncv.MatrixTransform().getTrans();
	CStdFPoint vPoint(vCoord[0], vCoord[1], vCoord[2]);
	return vPoint;
}

osg::Matrix VsBody::GetOSGWorldMatrix()
{
	return GetOSGWorldMatrix(m_osgMT.get());
}

osg::Matrix VsBody::GetOSGWorldMatrix(osg::MatrixTransform *osgMT)
{
	WorldCoordinateNodeVisitor ncv;
	osgMT->accept(ncv);
	return ncv.MatrixTransform();
}

void VsBody::WorldToBodyCoords(VxReal3 vWorld, StdVector3 &vLocalPos)
{
	osg::Vec3f vWorldPos;
	osg::Vec3f vLocal;

	vLocalPos[0] = vWorld[0]; vLocalPos[1] = vWorld[1]; vLocalPos[2] = vWorld[2];
	vWorldPos[0] = vWorld[0]; vWorldPos[1] = vWorld[1]; vWorldPos[2] = vWorld[2];

	if(m_osgNode.valid())
	{
	  osg::NodePathList paths = m_osgNode->getParentalNodePaths(); 
	  osg::Matrix worldToLocal = osg::computeWorldToLocal(paths.at(0)); 
	  vLocal = worldToLocal * vWorldPos;
	}

	vLocalPos[0] = vLocal[0]; vLocalPos[1] = vLocal[1]; vLocalPos[2] = vLocal[2];
} 

osg::MatrixTransform* VsBody::GetMatrixTransform()
{
	return m_osgMT.get();
}

//DWC: When you do this you are going to have to call UpdateNode on all child objects as well.
void VsBody::UpdatePositionAndRotationFromMatrix()
{
	LocalMatrix(m_osgMT->getMatrix());

	//Lets get the current world coordinates for this body part and then recalculate the 
	//new local position for the part and then finally reset its new local position.
	osg::Vec3 vL = m_osgLocalMatrix.getTrans();
	CStdFPoint vLocal(vL.x(), vL.y(), vL.z());
	vLocal.ClearNearZero();
	m_lpThis->LocalPosition(vLocal, FALSE, TRUE, FALSE);
	
	CStdFPoint vWorldPos = GetOSGWorldCoords();
	vWorldPos.ClearNearZero();
	m_lpThis->AbsolutePosition(vWorldPos.x, vWorldPos.y, vWorldPos.z);
	m_lpThis->ReportWorldPosition(m_lpThis->AbsolutePosition() * m_lpThis->GetSimulator()->DistanceUnits());
	
	//Now lets get the euler angle rotation
	Vx::VxReal44 vxTM;
	VxOSG::copyOsgMatrix_to_VxReal44(m_osgLocalMatrix, vxTM);
	Vx::VxTransform vTrans(vxTM);
	Vx::VxReal3 vEuler;
	vTrans.getRotationEulerAngles(vEuler);
	CStdFPoint vRot(vEuler[0], vEuler[1] ,vEuler[2]);
	vRot.ClearNearZero();
	m_lpThis->Rotation(vRot, TRUE, FALSE);

	m_osgDragger->SetupMatrix();

	//Test the matrix to make sure they match. I will probably get rid of this code after full testing.
	osg::Matrix osgTest = SetupMatrix(vLocal, vRot);
	if(!OsgMatricesEqual(osgTest, m_osgLocalMatrix))
		THROW_ERROR(Vs_Err_lUpdateMatricesDoNotMatch, Vs_Err_strUpdateMatricesDoNotMatch);
}

void VsBody::Physics_UpdateMatrix()
{
	LocalMatrix(SetupMatrix(m_lpThis->LocalPosition(), m_lpThis->Rotation()));
	m_osgMT->setMatrix(m_osgLocalMatrix);
	m_osgDragger->SetupMatrix();

	//If we are here then we did not have a physics component, just and OSG one.
	CStdFPoint vPos = VsBody::GetOSGWorldCoords();
	vPos.ClearNearZero();
	m_lpThis->AbsolutePosition(vPos.x, vPos.y, vPos.z);
	m_lpThis->ReportWorldPosition(m_lpThis->AbsolutePosition() * m_lpThis->GetSimulator()->DistanceUnits());
}

void VsBody::BuildLocalMatrix()
{
	//build the local matrix
	BuildLocalMatrix(m_lpThis->LocalPosition(), m_lpThis->Rotation(), m_lpThis->Name());
}

void VsBody::BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName)
{
	if(!m_osgMT.valid())
	{
		m_osgMT = new osgManipulator::Selection;
		m_osgMT->setName(strName + "_MT");
	}

	if(!m_osgRoot.valid())
	{
		m_osgRoot = new osg::Group;
		m_osgRoot->setName(strName + "_Root");
	}

	if(!m_osgRoot->containsNode(m_osgMT.get()))
		m_osgRoot->addChild(m_osgMT.get());

	LocalMatrix(SetupMatrix(localPos, localRot));

	//set the matrix to the matrix transform node
	m_osgMT->setMatrix(m_osgLocalMatrix);	

	//First create the node group. The reason for this is so that we can add other decorated groups on to this node.
	//This is used to add the selected overlays.
	if(!m_osgNodeGroup.valid())
	{
		m_osgNodeGroup = new osg::Group();
		m_osgNodeGroup->addChild(m_osgNode.get());		
		m_osgNodeGroup->setName(strName + "_NodeGroup");
		
		m_osgMT->addChild(m_osgNodeGroup.get());
	
		CreateSelectedGraphics(strName);
	}
}

BoundingBox VsBody::Physics_GetBoundingBox()
{
 	BoundingBox abb;

	osg::BoundingBox bb;
	osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
	if(osgGroup)
	{
		bb = osgGroup->getBoundingBox();
		abb.Set(bb.xMin(), bb.yMin(), bb.zMin(), bb.xMax(), bb.yMax(), bb.zMax());
	}
	else
		abb.Set(-0.5, -0.5, -0.5, 0.5, 0.5, 0.5); 

	return abb;
}

float VsBody::Physics_GetBoundingRadius()
{
	BoundingBox bb = Physics_GetBoundingBox();
	return bb.MaxDimension();
}

void VsBody::SetTexture(string strTexture)
{
	if(m_osgNode.valid())
	{
		if(!Std_IsBlank(strTexture))
		{
			string strFile = AnimatSim::GetFilePath(m_lpThis->GetSimulator()->ProjectPath(), strTexture);
			osg::ref_ptr<osg::Image> image = osgDB::readImageFile(strFile);
			if(!image)
				THROW_PARAM_ERROR(Vs_Err_lTextureLoad, Vs_Err_strTextureLoad, "Image File", strFile);

			osg::StateSet* state = m_osgNode->getOrCreateStateSet();
			m_osgTexture = new osg::Texture2D(image.get());
		    m_osgTexture->setDataVariance(osg::Object::DYNAMIC); // protect from being optimized away as static state.

			m_osgTexture->setWrap(osg::Texture2D::WRAP_S, osg::Texture2D::REPEAT);
			m_osgTexture->setWrap(osg::Texture2D::WRAP_T, osg::Texture2D::REPEAT);
			
			state->setTextureAttributeAndModes(0, m_osgTexture.get());
			state->setTextureMode(0, m_eTextureMode, osg::StateAttribute::ON);
			state->setMode(GL_BLEND,osg::StateAttribute::ON);

			//state->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
		}
		else if(m_osgTexture.valid()) //If we have already set it and we are clearing it then reset the state
		{
			m_osgTexture.release();
			osg::StateSet* state = m_osgNode->getOrCreateStateSet();
			state->setTextureAttributeAndModes(0, NULL); 
			state->setTextureMode(0, m_eTextureMode, osg::StateAttribute::OFF);
		}
	}
}

void VsBody::SetCulling()
{
	if(m_osgMT.valid())
	{
		if(m_bCullBackfaces)
		{
			if(!m_osgCull.valid())
			{
				m_osgCull = new osg::CullFace(); 
				m_osgCull->setMode(osg::CullFace::BACK); 
			}
			osg::StateSet* ss = m_osgMT->getOrCreateStateSet();
			ss->setAttributeAndModes(m_osgCull.get(), osg::StateAttribute::ON); 
		}
		else if(m_osgCull.valid())
		{
			osg::StateSet* ss = m_osgMT->getOrCreateStateSet();
			ss->setAttributeAndModes(m_osgCull.get(), osg::StateAttribute::OFF); 
		}
	}
}

void VsBody::SetAlpha()
{
	switch (m_lpThis->GetSimulator()->VisualSelectionMode())
	{
		case GRAPHICS_SELECTION_MODE:
			m_lpThis->Alpha(m_lpThis->GraphicsAlpha());			
			break;

		case COLLISION_SELECTION_MODE:
			m_lpThis->Alpha(m_lpThis->CollisionsAlpha());				
			break;

		case JOINT_SELECTION_MODE:
			m_lpThis->Alpha(m_lpThis->JointsAlpha());					
			break;

		case RECEPTIVE_FIELD_SELECTION_MODE:
			m_lpThis->Alpha(m_lpThis->ReceptiveFieldsAlpha());					
			break;

		case SIMULATION_SELECTION_MODE:
			m_lpThis->Alpha(m_lpThis->SimulationAlpha());					
			break;

		default:
			m_lpThis->Alpha(m_lpThis->GraphicsAlpha());					
			break;
	}

	if(m_osgMaterial.valid() && m_osgStateSet.valid())
		SetMaterialAlpha(m_osgMaterial.get(), m_osgStateSet.get(), m_lpThis->Alpha());
}

void VsBody::SetMaterialAlpha(osg::Material *osgMat, osg::StateSet *ss, float fltAlpha)
{
	osgMat->setAlpha(osg::Material::FRONT_AND_BACK, fltAlpha);

	if(fltAlpha < 1)
		ss->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
	else
		ss->setRenderingHint(osg::StateSet::OPAQUE_BIN);
}

void VsBody::SetColor(CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess)
{
	//create a material to use with this node
	if(!m_osgMaterial)
		m_osgMaterial = new osg::Material();		

	//create a stateset for this node
	m_osgStateSet = m_osgNode->getOrCreateStateSet();

	//set the diffuse property of this node to the color of this body	
	m_osgMaterial->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(vAmbient[0], vAmbient[1], vAmbient[2], 1));
	m_osgMaterial->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(vDiffuse[0], vDiffuse[1], vDiffuse[2], vDiffuse[3]));
	m_osgMaterial->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(vSpecular[0], vSpecular[1], vSpecular[2], 1));
	m_osgMaterial->setShininess(osg::Material::FRONT_AND_BACK, fltShininess);
	m_osgStateSet->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 
	SetAlpha();

	//if(vDiffuse[3] < 1)
	//{
	//	m_osgStateSet->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 
	//	m_osgStateSet->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
	//}

	//apply the material
	m_osgStateSet->setAttribute(m_osgMaterial.get(), osg::StateAttribute::ON);
}

void VsBody::SetVisible(osg::Node *osgNode, BOOL bVisible)
{
	if(osgNode)
	{
		if(bVisible)
			osgNode->setNodeMask(0x1);
		else
			osgNode->setNodeMask(0x0);
	}
}

void VsBody::SetVisible(BOOL bVisible)
{
	SetVisible(m_osgNode.get(), bVisible);
}

void VsBody::CreateBody()
{
	Initialize();
	SetupGraphics();
	SetupPhysics();
}

void VsBody::EndGripDrag()
{
	this->UpdatePositionAndRotationFromMatrix();
/*
	//First check to see if the body was moved.
	if(m_lpThis && m_lpThis->Parent())
	{
		//Lets get the current world coordinates for this body part and then recalculate the 
		//new local position for the part and then finally reset its new local position.
		CStdFPoint vWorldPos = GetOSGWorldCoords();
	
		CStdFPoint vParent = m_lpThis->Parent()->AbsolutePosition();

		CStdFPoint vLocal = vWorldPos - vParent;

		if(vLocal != m_lpThis->LocalPosition())
		{
			m_lpThis->LocalPosition(vLocal, FALSE, TRUE);
			return; //Only one operation can be done at a time. If it is translate then cannot be rotate.
		}
	}


	//Then check to see if it was rotated.
	osg::Matrix osgMT = m_osgMT->getMatrix();
	osg::Quat qRot = osgMT.getRotate();
	CStdFPoint vRot = QuaterionToEuler(qRot);
	
	osg::Quat qRot2 = EulerToQuaternion(vRot.x, vRot.y, vRot.z);

	if(qRot != qRot2)
		vRot.x = vRot.x;

	if(vRot != m_lpThis->Rotation())
		m_lpThis->Rotation(vRot, TRUE);
		*/
}

#pragma region CreateGeometry_Code


osg::Geometry *CreateBoxGeometry(float xsize, float ysize, float zsize, float fltXSegWidth, float fltYSegWidth, float fltZSegWidth)
{
    //if(! hor || ! vert || ! depth)
    //{
    //    SWARNING << "makeBox: illegal parameters hor=" << hor << ", vert="
    //             << vert << ", depth=" << depth << std::endl;
    //    return NULL;
    //}

    osg::Vec3 sizeMin(-xsize/2.0f,  -ysize/2.0f,  -zsize/2.0f);
    osg::Vec3 sizeMax(xsize/2.0f,  ysize/2.0f,  zsize/2.0f);
	osg::Vec3 steps( (int) (xsize/fltXSegWidth), (int) (ysize/fltYSegWidth), (int) (zsize/fltZSegWidth) );
	osg::Vec3 SegWidths(fltXSegWidth, fltYSegWidth, fltZSegWidth);	

	osg::Geometry* boxGeom = new osg::Geometry();
	osg::ref_ptr<osg::Vec3Array> verts = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> norms = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> texts = new osg::Vec2Array(); 
	int iLen = 0;
	int iPos = 0;

#pragma region X-constant-loops

	//Side 1
	float fltY1 = sizeMin.y();
	float fltY2 = fltY1 + SegWidths.y();
	float fltZ1 = sizeMin.z();
	float fltZ2 = fltZ1 + SegWidths.z();
	for(int iy=0; iy<(int) steps.y(); iy++)
	{
		for(int iz=0; iz<(int) steps.z(); iz++)
		{
			verts->push_back(osg::Vec3(sizeMin.x(), fltY1, fltZ1)); // 0
			verts->push_back(osg::Vec3(sizeMin.x(), fltY1, fltZ2)); // 3
			verts->push_back(osg::Vec3(sizeMin.x(), fltY2, fltZ2)); // 5
			verts->push_back(osg::Vec3(sizeMin.x(), fltY2, fltZ1)); // 1

			norms->push_back(osg::Vec3(-1,  0,  0));
			norms->push_back(osg::Vec3(-1,  0,  0));
			norms->push_back(osg::Vec3(-1,  0,  0));
			norms->push_back(osg::Vec3(-1,  0,  0));

			texts->push_back(osg::Vec2( 0.f, 0.f)); // 0
			texts->push_back(osg::Vec2( 1.f, 0.f)); // 1
			texts->push_back(osg::Vec2( 1.f, 1.f)); // 4
			texts->push_back(osg::Vec2( 0.f, 1.f)); // 2
			
			fltZ1+=SegWidths.z(); fltZ2+=SegWidths.z();
		}

		fltY1+=SegWidths.y(); fltY2+=SegWidths.y();
		fltZ1 = sizeMin.z(); fltZ2 = fltZ1 + SegWidths.z();
	}

	//Side 2 opposite
	fltY1 = sizeMin.y();
	fltY2 = fltY1 + SegWidths.y();
	fltZ1 = sizeMin.z();
	fltZ2 = fltZ1 + SegWidths.z();
	for(int iy=0; iy<(int) steps.y(); iy++)
	{
		for(int iz=0; iz<(int) steps.z(); iz++)
		{
			verts->push_back(osg::Vec3(sizeMax.x(), fltY1, fltZ1)); // 0
			verts->push_back(osg::Vec3(sizeMax.x(), fltY2, fltZ1)); // 3
			verts->push_back(osg::Vec3(sizeMax.x(), fltY2, fltZ2)); // 5
			verts->push_back(osg::Vec3(sizeMax.x(), fltY1, fltZ2)); // 1

			norms->push_back(osg::Vec3(1,  0,  0));
			norms->push_back(osg::Vec3(1,  0,  0));
			norms->push_back(osg::Vec3(1,  0,  0));
			norms->push_back(osg::Vec3(1,  0,  0));

			texts->push_back(osg::Vec2( 0.f, 0.f)); // 3
			texts->push_back(osg::Vec2( 1.f, 0.f)); // 6
			texts->push_back(osg::Vec2( 1.f, 1.f)); // 7
			texts->push_back(osg::Vec2( 0.f, 1.f)); // 5
			
			fltZ1+=SegWidths.z(); fltZ2+=SegWidths.z();
		}

		fltY1+=SegWidths.y(); fltY2+=SegWidths.y();
		fltZ1 = sizeMin.z(); fltZ2 = fltZ1 + SegWidths.z();
	}

#pragma endregion


#pragma region Y-constant-loops

	//Side 1
	float fltX1 = sizeMin.x();
	float fltX2 = fltX1 + SegWidths.x();
	fltZ1 = sizeMin.z();
	fltZ2 = fltZ1 + SegWidths.z();
	for(int ix=0; ix<(int) steps.x(); ix++)
	{
		for(int iz=0; iz<(int) steps.z(); iz++)
		{
			verts->push_back(osg::Vec3(fltX1, sizeMin.y(), fltZ1)); // 0
			verts->push_back(osg::Vec3(fltX2, sizeMin.y(), fltZ1)); // 3
			verts->push_back(osg::Vec3(fltX2, sizeMin.y(), fltZ2)); // 5
			verts->push_back(osg::Vec3(fltX1, sizeMin.y(), fltZ2)); // 1

			norms->push_back(osg::Vec3( 0, -1,  0));
			norms->push_back(osg::Vec3( 0, -1,  0));
			norms->push_back(osg::Vec3( 0, -1,  0));
			norms->push_back(osg::Vec3( 0, -1,  0));

			texts->push_back(osg::Vec2( 0.f, 0.f)); // 0
			texts->push_back(osg::Vec2( 1.f, 0.f)); // 3
			texts->push_back(osg::Vec2( 1.f, 1.f)); // 5
			texts->push_back(osg::Vec2( 0.f, 1.f)); // 1
			
			fltZ1+=SegWidths.z(); fltZ2+=SegWidths.z();
		}

		fltX1+=SegWidths.x(); fltX2+=SegWidths.x();
		fltZ1 = sizeMin.z(); fltZ2 = fltZ1 + SegWidths.z();
	}

	//Side 2 opposite
	fltX1 = sizeMin.x();
	fltX2 = fltX1 + SegWidths.x();
	fltZ1 = sizeMin.z();
	fltZ2 = fltZ1 + SegWidths.z();
	for(int ix=0; ix<(int) steps.x(); ix++)
	{
		for(int iz=0; iz<(int) steps.z(); iz++)
		{
			verts->push_back(osg::Vec3(fltX2, sizeMax.y(), fltZ1)); // 0
			verts->push_back(osg::Vec3(fltX1, sizeMax.y(), fltZ1)); // 3
			verts->push_back(osg::Vec3(fltX1, sizeMax.y(), fltZ2)); // 5
			verts->push_back(osg::Vec3(fltX2, sizeMax.y(), fltZ2)); // 1

			norms->push_back(osg::Vec3( 0, 1,  0));
			norms->push_back(osg::Vec3( 0, 1,  0));
			norms->push_back(osg::Vec3( 0, 1,  0));
			norms->push_back(osg::Vec3( 0, 1,  0));

			texts->push_back(osg::Vec2( 0.f, 0.f)); // 6
			texts->push_back(osg::Vec2( 1.f, 0.f)); // 2
			texts->push_back(osg::Vec2( 1.f, 1.f)); // 4
			texts->push_back(osg::Vec2( 0.f, 1.f)); // 7
			
			fltZ1+=SegWidths.z(); fltZ2+=SegWidths.z();
		}

		fltX1+=SegWidths.x(); fltX2+=SegWidths.x();
		fltZ1 = sizeMin.z(); fltZ2 = fltZ1 + SegWidths.z();
	}

#pragma endregion


#pragma region Z-constant-loops

	//Side 1
	fltX1 = sizeMin.x();
	fltX2 = fltX1 + SegWidths.x();
	fltY1 = sizeMin.y();
	fltY2 = fltY1 + SegWidths.y();
	for(int ix=0; ix<(int) steps.x(); ix++)
	{
		for(int iy=0; iy<(int) steps.y(); iy++)
		{
			verts->push_back(osg::Vec3(fltX1, fltY1, sizeMax.z())); // 0
			verts->push_back(osg::Vec3(fltX2, fltY1, sizeMax.z())); // 3
			verts->push_back(osg::Vec3(fltX2, fltY2, sizeMax.z())); // 5
			verts->push_back(osg::Vec3(fltX1, fltY2, sizeMax.z())); // 1

			norms->push_back(osg::Vec3( 0,  0,  1));
			norms->push_back(osg::Vec3( 0,  0,  1));
			norms->push_back(osg::Vec3( 0,  0,  1));
			norms->push_back(osg::Vec3( 0,  0,  1));

			texts->push_back(osg::Vec2( 0.f, 0.f)); // 1
			texts->push_back(osg::Vec2( 1.f, 0.f)); // 5
			texts->push_back(osg::Vec2( 1.f, 1.f)); // 7
			texts->push_back(osg::Vec2( 0.f, 1.f)); // 4
			
			fltY1+=SegWidths.y(); fltY2+=SegWidths.y();
		}

		fltX1+=SegWidths.x(); fltX2+=SegWidths.x();
		fltY1 = sizeMin.y(); fltY2 = fltY1 + SegWidths.y();
	}

	//Side 2 opposite
	fltX1 = sizeMin.x();
	fltX2 = fltX1 + SegWidths.x();
	fltY1 = sizeMin.y();
	fltY2 = fltY1 + SegWidths.y();
	for(int ix=0; ix<(int) steps.x(); ix++)
	{
		for(int iy=0; iy<(int) steps.y(); iy++)
		{
			verts->push_back(osg::Vec3(fltX2, fltY1, sizeMin.z())); // 0
			verts->push_back(osg::Vec3(fltX1, fltY1, sizeMin.z())); // 3
			verts->push_back(osg::Vec3(fltX1, fltY2, sizeMin.z())); // 5
			verts->push_back(osg::Vec3(fltX2, fltY2, sizeMin.z())); // 1

			norms->push_back(osg::Vec3( 0,  0, -1));
			norms->push_back(osg::Vec3( 0,  0, -1));
			norms->push_back(osg::Vec3( 0,  0, -1));
			norms->push_back(osg::Vec3( 0,  0, -1));

			texts->push_back(osg::Vec2( 0.f, 0.f)); // 3
			texts->push_back(osg::Vec2( 1.f, 0.f)); // 0
			texts->push_back(osg::Vec2( 1.f, 1.f)); // 2
			texts->push_back(osg::Vec2( 0.f, 1.f)); // 6
			
			fltY1+=SegWidths.y(); fltY2+=SegWidths.y();
		}

		fltX1+=SegWidths.x(); fltX2+=SegWidths.x();
		fltY1 = sizeMin.y(); fltY2 = fltY1 + SegWidths.y();
	}

#pragma endregion

    // create the geometry
     boxGeom->setVertexArray(verts.get());
     boxGeom->addPrimitiveSet(new osg::DrawArrays(GL_QUADS, 0, verts->size()));

	 boxGeom->setNormalArray(norms.get());
     boxGeom->setNormalBinding(osg::Geometry::BIND_PER_VERTEX);

     boxGeom->setTexCoordArray( 0, texts.get() );

     osg::Vec4Array* colors = new osg::Vec4Array;
     colors->push_back(osg::Vec4(1,1,1,1));
     boxGeom->setColorArray(colors);
     boxGeom->setColorBinding(osg::Geometry::BIND_OVERALL);

    return boxGeom;
}

/*! Create the Geometry Core used by OSG::makeConicalFrustum.

    \param[in] height Height of the conical frustum.
    \param[in] topradius Radius at the top of the conical frustum.
    \param[in] botradius Radius at the bottom of the conical frustum.
    \param[in] sides Number of sides the base is subdivided into.
    \param[in] doSide If true, side faces are created.
    \param[in] doTop If true, top cap faces are created.
    \param[in] doBttom If true, bottom cap faces are created.
    \return GeometryTransitPtr to a newly created Geometry core.

    \ingroup GrpSystemDrawablesGeometrySimpleGeometry
 */
osg::Geometry *CreateConeGeometry(float height,
                                  float topradius,
                                  float botradius,
                                  int sides,
                                  bool   doSide,
                                  bool   doTop,
                                  bool   doBottom)
{
    if(height <= 0 || topradius < 0 || botradius < 0 || sides < 3)
    {
        //SWARNING << "makeConicalFrustum: illegal parameters height=" << height
        //         << ", topradius=" << topradius
        //         << ", botradius=" << botradius
        //         << ", sides=" << sides
        //         << std::endl;
        return NULL;
    }

	osg::ref_ptr<osg::Vec3Array> p = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> n = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> t = new osg::Vec2Array(); 

    osg::Geometry* coneGeom = new osg::Geometry();
	osg::ref_ptr<osg::Vec3Array> verts = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> norms = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> texts = new osg::Vec2Array(); 

    int  j;
    float delta = 2.f * osg::PI / sides;
    float beta, x, z;
    float incl = (botradius - topradius) / height;
    float nlen = 1.f / sqrt(1 + incl * incl);
	int iLen = 0;
	int iPos = 0;

    if(doSide)
    {
        int baseindex = p->size();

        for(j = 0; j <= sides; j++)
        {
            beta = j * delta;
            x    =  sin(beta);
            z    = -cos(beta);

            p->push_back(osg::Vec3(x * topradius, height/2, z * topradius));
            n->push_back(osg::Vec3(x/nlen, incl/nlen, z/nlen));
            t->push_back(osg::Vec2(1.f - j / float(sides), 1));
        }

        for(j = 0; j <= sides; j++)
        {
            beta = j * delta;
            x    =  sin(beta);
            z    = -cos(beta);

            p->push_back(osg::Vec3(x * botradius, -height/2, z * botradius));
            n->push_back(osg::Vec3(x/nlen, incl/nlen, z/nlen));
            t->push_back(osg::Vec2(1.f - j / float(sides), 0));
        }

        for(j = 0; j <= sides; j++)
        {
            verts->push_back(p->at(baseindex + sides + 1 + j));
            verts->push_back(p->at(baseindex + j));

            norms->push_back(n->at(baseindex + sides + 1 + j));
            norms->push_back(n->at(baseindex + j));

            texts->push_back(t->at(baseindex + sides + 1 + j));
            texts->push_back(t->at(baseindex + j));
		}

		iLen = 2 * (sides + 1);
        coneGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::TRIANGLE_STRIP, iPos, iLen));
		iPos+=iLen;
	}

    if(doTop && topradius > 0)
    {
        int baseindex = p->getNumElements();

        // need to duplicate the points fornow, as we don't have multi-index geo yet

        for(j = sides - 1; j >= 0; j--)
        {
            beta = j * delta;
            x    =  topradius * sin(beta);
            z    = -topradius * cos(beta);

            p->push_back(osg::Vec3(x, height/2, z));
            n->push_back(osg::Vec3(0, 1, 0));
            t->push_back(osg::Vec2(x / topradius / 2 + .5f, -z / topradius / 2 + .5f));
        }

        for(j = 0; j < sides; j++)
        {
            verts->push_back(p->at(baseindex + j));
            norms->push_back(n->at(baseindex + j));
            texts->push_back(t->at(baseindex + j));
        }

		iLen = sides;
        coneGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::POLYGON, iPos, iLen));
		iPos+=iLen;
	}

    if(doBottom && botradius > 0 )
    {
        int baseindex = p->getNumElements();

        // need to duplicate the points fornow, as we don't have multi-index geo yet

        for(j = sides - 1; j >= 0; j--)
        {
            beta = j * delta;
            x    =  botradius * sin(beta);
            z    = -botradius * cos(beta);

            p->push_back(osg::Vec3(x, -height/2, z));
            n->push_back(osg::Vec3(0, -1, 0));
            t->push_back(osg::Vec2(x / botradius / 2 + .5f, z / botradius / 2 + .5f));
        }

        for(j = 0; j < sides; j++)
        {
			verts->push_back(p->at(baseindex + sides - 1 - j));
            norms->push_back(n->at(baseindex + sides - 1 - j));
            texts->push_back(t->at(baseindex + sides - 1 - j));
        }

		iLen = sides;
        coneGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::POLYGON, iPos, iLen));
		iPos+=iLen;
    }

    // create the geometry
     coneGeom->setVertexArray(verts.get());

	 coneGeom->setNormalArray(norms.get());
     coneGeom->setNormalBinding(osg::Geometry::BIND_PER_VERTEX);

     coneGeom->setTexCoordArray( 0, texts.get() );

     osg::Vec4Array* colors = new osg::Vec4Array;
     colors->push_back(osg::Vec4(1,1,1,1));
     coneGeom->setColorArray(colors);
     coneGeom->setColorBinding(osg::Geometry::BIND_OVERALL);

    return coneGeom;
}

///*! Create the Geometry Core used by OSG::makeLatLongSphere.
//
//    \param[in] latres Number of subdivisions along latitudes.
//    \param[in] longres Number of subdivisions along longitudes.
//    \param[in] radius Radius of sphere.
//    \return GeometryTransitPtr to a newly created Geometry core.
//
//    \sa OSG::makeLatLongSphere
//
//    \ingroup GrpSystemDrawablesGeometrySimpleGeometry
// */
osg::Geometry *CreateSphereGeometry(int latres, 
                          int longres,
                          float radius)
{
    if(radius <= 0 || latres < 4 || longres < 4)
    {
        //SWARNING << "makeLatLongSphere: illegal parameters "
        //         << "latres=" << latres
        //         << ", longres=" << longres
        //         << ", radius=" << radius
        //         << std::endl;
        return NULL;
    }

	osg::ref_ptr<osg::Vec3Array> p = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> n = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> t = new osg::Vec2Array(); 

    int a, b;
    float theta, phi;
    float cosTheta, sinTheta;
    float latDelta, longDelta;

    // calc the vertices

    latDelta  =       osg::PI / latres;
    longDelta = 2.f * osg::PI / longres;

    for(a = 0, theta = -osg::PI / 2; a <= latres; a++, theta += latDelta)
    {
        cosTheta = cos(theta);
        sinTheta = sin(theta);

        for(b = 0, phi = -osg::PI; b <= longres; b++, phi += longDelta)
        {
            float cosPhi, sinPhi;

            cosPhi = cos(phi);
            sinPhi = sin(phi);

            n->push_back(osg::Vec3(cosTheta * sinPhi,
                               sinTheta,
                               cosTheta * cosPhi));
        
            p->push_back(osg::Vec3( cosTheta * sinPhi * radius,
                               sinTheta          * radius,
                               cosTheta * cosPhi * radius));

            t->push_back(osg::Vec2(b / float(longres),
                                a / float(latres)));
        }
    }


    osg::Geometry* sphereGeom = new osg::Geometry();

	osg::ref_ptr<osg::Vec3Array> verts = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> norms = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> texts = new osg::Vec2Array(); 

    // create the faces
	int iLen = (latres + 1) * 2;
	int iPos = 0;
    for(a = 0; a < longres; a++)
    {
        for(b = 0; b <= latres; b++)
        {
			verts->push_back(p->at(b * (longres+1) + a));
			verts->push_back(p->at(b * (longres+1) + a + 1));

			norms->push_back(n->at(b * (longres+1) + a));
			norms->push_back(n->at(b * (longres+1) + a + 1));

			texts->push_back(t->at(b * (longres+1) + a));
			texts->push_back(t->at(b * (longres+1) + a + 1));
        }

        sphereGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::TRIANGLE_STRIP, iPos, iLen));
		iPos+=iLen;
	}

    // create the geometry
     sphereGeom->setVertexArray(verts.get());

	 sphereGeom->setNormalArray(norms.get());
     sphereGeom->setNormalBinding(osg::Geometry::BIND_PER_VERTEX);

     sphereGeom->setTexCoordArray( 0, texts.get() );

     osg::Vec4Array* colors = new osg::Vec4Array;
     colors->push_back(osg::Vec4(1,1,1,1));
     sphereGeom->setColorArray(colors);
     sphereGeom->setColorBinding(osg::Geometry::BIND_OVERALL);

    return sphereGeom;
}


/*! Create the Geometry Core used by OSG::makeLatLongSphere.

    \param[in] latres Number of subdivisions along latitudes.
    \param[in] longres Number of subdivisions along longitudes.
    \param[in] radius Radius of sphere.
    \return GeometryTransitPtr to a newly created Geometry core.

    \sa OSG::makeLatLongSphere

    \ingroup GrpSystemDrawablesGeometrySimpleGeometry
 */
osg::Geometry *CreateEllipsoidGeometry(int latres, 
                             int longres,
                             float rSemiMajorAxis,
                             float rSemiMinorAxis)
{
    if(rSemiMajorAxis <= 0 || rSemiMinorAxis <= 0 || latres < 4 || longres < 4)
    {
        //SWARNING << "makeLatLongSphere: illegal parameters "
        //         << "latres=" << latres
        //         << ", longres=" << longres
        //         << ", rSemiMajorAxis=" << rSemiMajorAxis
        //         << ", rSemiMinorAxis=" << rSemiMinorAxis
        //         << std::endl;
        return NULL;
    }

	osg::ref_ptr<osg::Vec3Array> p = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> n = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> t = new osg::Vec2Array(); 

    int a, b;
    float theta, phi;
    float cosTheta, sinTheta;
    float latDelta, longDelta;

    // calc the vertices

    latDelta  =       osg::PI / latres;
    longDelta = 2.f * osg::PI / longres;

    float rSemiMajorAxisSquare = rSemiMajorAxis * rSemiMajorAxis;

    float e2 = (rSemiMajorAxisSquare - 
                rSemiMinorAxis * rSemiMinorAxis) / (rSemiMajorAxisSquare);

    for(a = 0, theta = -osg::PI / 2; a <= latres; a++, theta += latDelta)
    {
        cosTheta = cos(theta);
        sinTheta = sin(theta);

        float v = rSemiMajorAxis / sqrt(1 - (e2 * sinTheta * sinTheta));

        for(b = 0, phi = -osg::PI; b <= longres; b++, phi += longDelta)
        {
            float cosPhi, sinPhi;

            cosPhi = cos(phi);
            sinPhi = sin(phi);


            n->push_back(osg::Vec3(cosTheta * sinPhi,
                               sinTheta,
                               cosTheta * cosPhi));
        
            p->push_back(osg::Vec3(cosTheta * sinPhi * v,
                               sinTheta          * ((1 - e2) * v),
                               cosTheta * cosPhi * v));

            t->push_back(osg::Vec2(b / float(longres),
                                a / float(latres)));

        }
    }

    osg::Geometry* sphereGeom = new osg::Geometry();

	osg::ref_ptr<osg::Vec3Array> verts = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> norms = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> texts = new osg::Vec2Array(); 

    // create the faces
	int iLen = (latres + 1) * 2;
	int iPos = 0;

	for(a = 0; a < longres; a++)
    {
        for(b = 0; b <= latres; b++)
        {
            verts->push_back(p->at(b * (longres+1) + a));
            verts->push_back(p->at(b * (longres+1) + a + 1));

		    norms->push_back(n->at(b * (longres+1) + a));
            norms->push_back(n->at(b * (longres+1) + a + 1));

		    texts->push_back(t->at(b * (longres+1) + a));
            texts->push_back(t->at(b * (longres+1) + a + 1));
		}

	    sphereGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::TRIANGLE_STRIP, iPos, iLen));
		iPos+=iLen;
	}


    // create the geometry
     sphereGeom->setVertexArray(verts.get());

	 sphereGeom->setNormalArray(norms.get());
     sphereGeom->setNormalBinding(osg::Geometry::BIND_PER_VERTEX);

     sphereGeom->setTexCoordArray( 0, texts.get() );

     osg::Vec4Array* colors = new osg::Vec4Array;
     colors->push_back(osg::Vec4(1,1,1,1));
     sphereGeom->setColorArray(colors);
     sphereGeom->setColorBinding(osg::Geometry::BIND_OVERALL);

    return sphereGeom;
}

osg::Geometry *CreatePlaneGeometry(float fltCornerX, float fltCornerY, float fltXSize, float fltYSize, float fltXGrid, float fltYGrid)
{
	float A = fltCornerX;
	float B = fltCornerY;
	float C = fltCornerX + fltXSize;
	float D = fltCornerY + fltYSize;

	osg::Geometry *geom = new osg::Geometry;

	// Create an array of four vertices.
	osg::ref_ptr<osg::Vec3Array> v = new osg::Vec3Array;
	geom->setVertexArray( v.get() );
	v->push_back( osg::Vec3( A, B, 0 ) );
	v->push_back( osg::Vec3( C, B, 0 ) );
	v->push_back( osg::Vec3( C, D, 0 ) );
	v->push_back( osg::Vec3( A, D, 0 ) );	

	// Create a Vec2Array of texture coordinates for texture unit 0
	// and attach it to the geom.
	osg::ref_ptr<osg::Vec2Array> tc = new osg::Vec2Array;
	geom->setTexCoordArray( 0, tc.get() );
	tc->push_back( osg::Vec2( 0.f, 0.f ) );
	tc->push_back( osg::Vec2( fltXGrid, 0.f ) );
	tc->push_back( osg::Vec2( fltXGrid, fltYGrid ) );
	tc->push_back( osg::Vec2( 0.f, fltYGrid ) );

	// Create an array for the single normal.
	osg::ref_ptr<osg::Vec3Array> n = new osg::Vec3Array;
	n->push_back( osg::Vec3( 0.f, 0.f, -1.f ) );
	geom->setNormalArray( n.get() );
	geom->setNormalBinding( osg::Geometry::BIND_OVERALL );

	// Draw a four-vertex quad from the stored data.
	geom->addPrimitiveSet(new osg::DrawArrays( osg::PrimitiveSet::QUADS, 0, 4 ) );

	return geom;
}

BOOL OsgMatricesEqual(osg::Matrix v1, osg::Matrix v2)
{
	for(int iRow=0; iRow<4; iRow++)
		for(int iCol=0; iCol<4; iCol++)
			if(fabs(v1(iRow,iCol)-v2(iRow,iCol)) > 1e-5)
				return FALSE;

	return TRUE;
}

osg::Quat EulerToQuaternion(float fX, float fY, float fZ)
{
	//Vx::VxTransform vTrans;
	//vTrans.createFromTranslationAndEulerAngles(vTrans, vEuler);


	//Vx::VxEulerAngles vAngle(fX, fY, fZ);
	//VxReal44 M; 
	//vAngle.toVxMatrix44(M);

	float c1 = cos(fY/2); 
	float c2 = cos(fZ/2); 
	float c3 = cos(fX/2); 
	
	float s1 = sin(fY/2);
	float s2 = sin(fZ/2);
	float s3 = sin(fX/2);

	float c1c2 = c1 * c2;
	float s1s2 = s1 * s2;
	
	float w =c1c2 * c3 - s1s2 * s3;
  	float x =c1c2 * s3 + s1s2 * c3;
	float y =s1*c2 * c3 + c1 * s2*s3;
	float z =c1*s2 * c3 - s1 * c2*s3;
	
	return osg::Quat(x, y, z, w);
}

CStdFPoint QuaterionToEuler(osg::Quat vQ)
{
	Vx::VxQuaternion vxQuat(vQ.w(), vQ.x(), vQ.y(), vQ.z());
	Vx::VxVector3 vEuler;
	vxQuat.toEulerXYZ(&vEuler);
	CStdFPoint vRot(vEuler.x(), vEuler.y(), vEuler.z());
	return vRot;
}

/*
osg::Quat EulerToQuaternion(float fX, float fY, float fZ)
{
	float cos_z_2 = cosf(0.5*fZ);
	float cos_y_2 = cosf(0.5*fY);
	float cos_x_2 = cosf(0.5*fX);

	float sin_z_2 = sinf(0.5*fZ);
	float sin_y_2 = sinf(0.5*fY);
	float sin_x_2 = sinf(0.5*fX);

	// and now compute quaternion
	float w  = cos_z_2*cos_y_2*cos_x_2 + sin_z_2*sin_y_2*sin_x_2;
	float x = cos_z_2*cos_y_2*sin_x_2 - sin_z_2*sin_y_2*cos_x_2;
	float y = cos_z_2*sin_y_2*cos_x_2 + sin_z_2*cos_y_2*sin_x_2;
	float z = sin_z_2*cos_y_2*cos_x_2 - cos_z_2*sin_y_2*sin_x_2;

	return osg::Quat(x, y, z, w);
}

CStdFPoint QuaterionToEuler(osg::Quat vQuat)
{
	osg::Vec4d v = vQuat.asVec4();
	float sqw = v.w()*v.w();    
	float sqx = v.x()*v.x();    
	float sqy = v.y()*v.y();    
	float sqz = v.z()*v.z();    

	CStdFPoint vEuler;
	vEuler.x = atan2f(2.f * (v.x()*v.y() + v.z()*v.w()), sqx - sqy - sqz + sqw);    		
	vEuler.y = asinf(-2.f * (v.x()*v.z() - v.y()*v.w()));
	vEuler.z = atan2f(2.f * (v.y()*v.z() + v.x()*v.w()), -sqx - sqy + sqz + sqw);    
	return vEuler;
}

CStdFPoint QuaterionToEuler(osg::Quat q1) 
{
    double sqw = q1.w()*q1.w();
    double sqx = q1.x()*q1.x();
    double sqy = q1.y()*q1.y();
    double sqz = q1.z()*q1.z();
	double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
	double test = q1.x()*q1.y() + q1.z()*q1.w();
	float heading, attitude, bank;

	if (test > 0.499*unit) 
	{ // singularity at north pole
		heading = 2 * atan2(q1.x(),q1.w());
		attitude = PI_HALF;
		bank = 0;
	}
	else if (test < -0.499*unit) 
	{ // singularity at south pole
		heading = -2 * atan2(q1.x(),q1.w());
		attitude = -PI_HALF;
		bank = 0;
	}
	else
	{
		heading = atan2(2*q1.y()*q1.w()-2*q1.x()*q1.z() , sqx - sqy - sqz + sqw);
		attitude = asin(2*test/unit);
		bank = atan2(2*q1.x()*q1.w()-2*q1.y()*q1.z() , -sqx + sqy - sqz + sqw);
	}

	CStdFPoint vRot(bank, heading, attitude);
	return vRot;
}
*/

osg::Matrix SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot)
{
	Vx::VxReal3 vLoc = {localPos.x, localPos.y, localPos.z};
	Vx::VxReal3 vRot = {localRot.x, localRot.y, localRot.z};
	Vx::VxTransform vTrans = Vx::VxTransform::createFromTranslationAndEulerAngles(vLoc, vRot);

	osg::Matrix osgLocalMatrix;
	VxOSG::copyVxReal44_to_OsgMatrix(osgLocalMatrix, vTrans.m);

	//osgLocalMatrix.makeIdentity();
	//
	////convert cstdpoint to osg::Vec3
	//osg::Vec3 vPos(localPos.x, localPos.y, localPos.z);
	//
	//osg::Quat qQuat = EulerToQuaternion(localRot.x, localRot.y, localRot.z);	
	//
	////build the matrix
	//osgLocalMatrix.makeRotate(qQuat);
	//osgLocalMatrix.setTrans(vPos);

	return osgLocalMatrix;
}

osg::Matrix SetupMatrix(CStdFPoint &localPos, osg::Quat qRot)
{
	osg::Matrix osgLocalMatrix;
	osgLocalMatrix.makeIdentity();
	
	//convert cstdpoint to osg::Vec3
	osg::Vec3 vPos(localPos.x, localPos.y, localPos.z);
	
	//build the matrix
	osgLocalMatrix.makeRotate(qRot);
	osgLocalMatrix.setTrans(vPos);

	return osgLocalMatrix;
}
osg::MatrixTransform *CreateLinearAxis(float fltGripScale, CStdFPoint vRotAxis)
{
	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 
	float fltCylinderRadius = fltGripScale * 0.01f;
	float fltTipRadius = fltGripScale * 0.03f;
	float fltCylinderHeight = fltGripScale * 2.5;
	float fltTipHeight = fltGripScale * 0.05f;

	//Create the X-axis transform.
	osg::MatrixTransform *osgAxis = new osg::MatrixTransform();
	vPos.Set(0, 0, 0); vRot = (vRotAxis * -(VX_PI/2)); 
	osgAxis->setMatrix(SetupMatrix(vPos, vRot));

	//Create the cylinder for the hinge
	osg::ref_ptr<osg::Geometry> osgCylinderGeom = CreateConeGeometry(fltCylinderHeight, fltCylinderRadius, fltCylinderRadius, 30, true, true, true);
	osg::ref_ptr<osg::Geode> osgAxisCylinder = new osg::Geode;
	osgAxisCylinder->addDrawable(osgCylinderGeom.get());
	osgAxis->addChild(osgAxisCylinder.get());

	osg::ref_ptr<osg::MatrixTransform> osgAxisConeMT = new osg::MatrixTransform();
	vPos.Set(0, (fltCylinderHeight/2), 0); vRot.Set(0, 0, 0); 
	osgAxisConeMT->setMatrix(SetupMatrix(vPos, vRot));

	osg::ref_ptr<osg::Geometry> osgAxisTipGeom = CreateConeGeometry(fltTipHeight, 0, fltTipRadius, 30, true, true, true);
	osg::ref_ptr<osg::Geode> osgAxisTip = new osg::Geode;
	osgAxisTip->addDrawable(osgAxisTipGeom.get());
	osgAxisConeMT->addChild(osgAxisTip.get());
	osgAxis->addChild(osgAxisConeMT.get());

	return osgAxis;
}

osg::Vec3Array *CreateCircleVerts( int plane, int segments, float radius )
{
    const double angle( osg::PI * 2. / (double) segments );
    osg::Vec3Array* v = new osg::Vec3Array;
    int idx, count;
    double x(0.), y(0.), z(0.);
    double height;
    double original_radius = radius;

    for(count = 0; count <= segments/4; count++)
    {
        height = original_radius*sin(count*angle);
        radius = cos(count*angle)*radius;


    switch(plane)
    {
        case 0: // X
            x = height;
            break;
        case 1: //Y
            y = height;
            break;
        case 2: //Z
            z = height;
            break;
    }

    for( idx=0; idx<segments; idx++)
    {
        double cosAngle = cos(idx*angle);
        double sinAngle = sin(idx*angle);
        switch (plane) {
            case 0: // X
                y = radius*cosAngle;
                z = radius*sinAngle;
                break;
            case 1: // Y
                x = radius*cosAngle;
                z = radius*sinAngle;
                break;
            case 2: // Z
                x = radius*cosAngle;
                y = radius*sinAngle;
                break;
        }
        v->push_back( osg::Vec3( x, y, z ) );
    }
    }
    return v;
}

osg::Geode *CreateCircle( int plane, int segments, float radius, float width )
{
    osg::Geode* geode = new osg::Geode;
    osg::LineWidth* lw = new osg::LineWidth( width );
    geode->getOrCreateStateSet()->setAttributeAndModes( lw, osg::StateAttribute::ON );

    osg::Geometry* geom = new osg::Geometry;
    osg::Vec3Array* v = CreateCircleVerts( plane, segments, radius );
    geom->setVertexArray( v );

    osg::Vec4Array* c = new osg::Vec4Array;
    c->push_back( osg::Vec4( 1., 1., 1., 1. ) );
    geom->setColorArray( c );
    geom->setColorBinding( osg::Geometry::BIND_OVERALL );
    geom->addPrimitiveSet( new osg::DrawArrays( GL_LINE_LOOP, 0, segments ) );

    geode->addDrawable( geom );

    return geode;
}

osg::Geometry *CreateTorusGeometry(float innerRadius, 
                                float outerRadius, 
                                int sides,
                                int rings)
{
    if(innerRadius <= 0 || outerRadius <= 0 || sides < 3 || rings < 3)
    {
        //SWARNING << "makeTorus: illegal parameters innerRadius=" << innerRadius
        //         << ", outerRadius=" << outerRadius
        //         << ", sides=" << sides
        //         << ", rings=" << rings
        //         << std::endl;
        //return GeometryTransitPtr(NULL);
		return NULL;
    }

    int a, b;
    float theta, phi;
    float cosTheta, sinTheta;
    float ringDelta, sideDelta;

    // calc the vertices

	osg::ref_ptr<osg::Vec3Array> p = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> n = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> tx = new osg::Vec2Array(); 

    ringDelta = 2.f * osg::PI / rings;
    sideDelta = 2.f * osg::PI / sides;

    for(a = 0, theta = 0.0; a <= rings; a++, theta += ringDelta)
    {
        cosTheta = cos(theta);
        sinTheta = sin(theta);

        for(b = 0, phi = 0; b <= sides; b++, phi += sideDelta)
        {
            float cosPhi, sinPhi, dist;

            cosPhi = cos(phi);
            sinPhi = sin(phi);
            dist   = outerRadius + innerRadius * cosPhi;

            n->push_back(osg::Vec3(cosTheta * cosPhi,
                              -sinTheta * cosPhi,
                              sinPhi));
            p->push_back(osg::Vec3(cosTheta * dist,
                              -sinTheta * dist,
                              innerRadius * sinPhi));
            tx->push_back(osg::Vec2(- a / float(rings), b / float(sides)));
        }
    }

    // create the faces
    osg::Geometry* torusGeom = new osg::Geometry();

	osg::ref_ptr<osg::Vec3Array> verts = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec3Array> norms = new osg::Vec3Array(); 
	osg::ref_ptr<osg::Vec2Array> texts = new osg::Vec2Array(); 

	int iLen = (rings + 1) * 2;
	int iPos = 0;
    for(a = 0; a < sides; a++)
    {
        for(b = 0; b <= rings; b++)
        {
			verts->push_back(p->at(b * (sides+1) + a));
			verts->push_back(p->at(b * (sides+1) + a + 1));

			norms->push_back(n->at(b * (sides+1) + a));
			norms->push_back(n->at(b * (sides+1) + a + 1));

			texts->push_back(tx->at(b * (sides+1) + a));
			texts->push_back(tx->at(b * (sides+1) + a + 1));
        }

	    torusGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::TRIANGLE_STRIP, iPos, iLen));
		iPos+=iLen;
	}


    // create the geometry
     torusGeom->setVertexArray(verts.get());

	 torusGeom->setNormalArray(norms.get());
     torusGeom->setNormalBinding(osg::Geometry::BIND_PER_VERTEX);

     torusGeom->setTexCoordArray( 0, texts.get() );

     osg::Vec4Array* colors = new osg::Vec4Array;
     colors->push_back(osg::Vec4(1,1,1,1));
     torusGeom->setColorArray(colors);
     torusGeom->setColorBinding(osg::Geometry::BIND_OVERALL);

    return torusGeom;
}



#pragma endregion

	}			// Environment
//}				//VortexAnimatSim

}