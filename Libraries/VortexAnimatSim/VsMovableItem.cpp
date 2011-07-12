#include "StdAfx.h"
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
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"

//#include "VsSimulationRecorder.h"
#include "VsMouseSpring.h"
#include "VsLight.h"
#include "VsCameraManipulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsMovableItem::VsMovableItem()
{
	m_bCullBackfaces = FALSE; //No backface culling by default.
	m_eTextureMode = GL_TEXTURE_2D;

	m_lpThisAB = NULL;
	m_lpThisMI = NULL;
	m_lpThisVsMI = NULL;
}


VsMovableItem::~VsMovableItem()
{
//
//try
//{
//	DeleteGraphics();
//	DeletePhysics();
//}
//catch(...)
//{Std_TraceMsg(0, "Caught Error in desctructor of VsMovableItem\r\n", "", -1, FALSE, TRUE);}
}

VsSimulator *VsMovableItem::GetVsSimulator()
{
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());
	//if(!lpVsSim)
	//	THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);
	return lpVsSim;
}

void VsMovableItem::SetThisPointers()
{
	m_lpThisAB = dynamic_cast<AnimatBase *>(this);
	if(!m_lpThisAB)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisAB");

	m_lpThisMI = dynamic_cast<MovableItem *>(this);
	if(!m_lpThisMI)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisMI, " + m_lpThisAB->Name());

	m_lpThisVsMI = dynamic_cast<VsMovableItem *>(this);
	if(!m_lpThisVsMI)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisVsMI, " + m_lpThisAB->Name());

	m_lpThisMI->PhysicsMovableItem(this);
}

string VsMovableItem::Physics_ID()
{
	if(m_lpThisAB)
		return m_lpThisAB->ID();
	else
		return "";
}

#pragma region Selection-Code

void VsMovableItem::Physics_Selected(BOOL bValue, BOOL bSelectMultiple)  
{
	if(m_osgNodeGroup.valid() && m_osgDragger.valid() && m_osgSelectedGroup.valid())
	{
		BOOL bIsReceptiveFieldMode = (m_lpThisAB->GetSimulator()->VisualSelectionMode() & RECEPTIVE_FIELD_SELECTION_MODE);

		//If selecting and not already selected then select it
		BOOL bNodeFound = m_osgNodeGroup->containsNode(m_osgSelectedGroup.get());
		if(bValue && !bNodeFound)
		{
			m_osgNodeGroup->addChild(m_osgSelectedGroup.get());
			if(!bIsReceptiveFieldMode)
				m_osgDragger->AddToScene();
			else
				ShowSelectedVertex();
		}
		//if de-selecting and selected then de-select the node
		else if(!bValue && bNodeFound)
		{
			m_osgNodeGroup->removeChild(m_osgSelectedGroup.get());
			m_osgDragger->RemoveFromScene();
			HideSelectedVertex();
		}
	}
}

void VsMovableItem::CreateSelectedGraphics(string strName)
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
	CreateSelectedVertex(strName);
}

void VsMovableItem::CreateDragger(string strName)
{
	string strVers = osgGetSOVersion();  

	if(m_lpThisAB->GetSimulator())
	{
		if(GetVsSimulator()->OsgCmdMgr())
		{
			m_osgDragger = new VsDragger(this, m_lpThisMI->AllowTranslateDragX(), m_lpThisMI->AllowTranslateDragY(), m_lpThisMI->AllowTranslateDragX(),
										 m_lpThisMI->AllowRotateDragX(), m_lpThisMI->AllowRotateDragY(), m_lpThisMI->AllowRotateDragZ());
			m_osgDragger->setName(strName + "_Dragger");

			m_osgDragger->setupDefaultGeometry();

			GetVsSimulator()->OsgCmdMgr()->connect(*m_osgDragger, *m_osgMT);

			//Add pointers to this object to the grip so it will no which body part to
			//call the EndGripDrag method on when the drag is finished.
			m_osgDragger->setUserData(new VsOsgUserData(this));
		}
	}
}

void VsMovableItem::CreateSelectedVertex(string strName)
{
	if(!m_osgSelVertexNode.valid())
	{
		m_osgSelVertexNode = new osg::Geode();
		m_osgSelVertexNode->setName(strName + "SelVertex");
		float fltRadius = m_lpThisAB->GetSimulator()->RecFieldSelRadius();
		osg::ShapeDrawable *osgDraw = new osg::ShapeDrawable(new osg::Sphere(osg::Vec3(0, 0, 0), fltRadius));
		osgDraw->setColor(osg::Vec4(0, 1, 0, 0));
		m_osgSelVertexNode->addDrawable(osgDraw);
	}

	if(!m_osgSelVertexMT.valid())
	{
		m_osgSelVertexMT = new osg::MatrixTransform();

		//Initially have it at the center. It will get moved as vertices are picked.
		osg::Matrix osgMT;
		osgMT.makeIdentity();
		m_osgSelVertexMT->setMatrix(osgMT);

		m_osgSelVertexMT->addChild(m_osgSelVertexNode.get());
	}
}

void VsMovableItem::DeleteSelectedVertex()
{
	HideSelectedVertex();

	if(m_osgSelVertexNode.valid()) m_osgSelVertexNode.release();
	if(m_osgSelVertexMT.valid()) m_osgSelVertexMT.release();
}

#pragma endregion

float *VsMovableItem::Physics_GetDataPointer(string strDataType) {return NULL;}

void VsMovableItem::LocalMatrix(osg::Matrix osgLocalMT)
{
	m_osgLocalMatrix = osgLocalMT;
	m_osgFinalMatrix = osgLocalMT;
	UpdateWorldMatrix();
}

void VsMovableItem::GeometryRotationMatrix(osg::Matrix osgGeometryMT)
{
	if(!m_osgGeometryRotationMT.valid())
	{
		m_osgGeometryRotationMT = new osg::MatrixTransform;
		m_osgGeometryRotationMT->setName(m_lpThisAB->Name() + "_GeometryMT");
	}
	m_osgGeometryRotationMT->setMatrix(osgGeometryMT);	
}

void VsMovableItem::AttachedPartMovedOrRotated(string strID)
{
	Physics_ResetGraphicsAndPhysics();
}

void VsMovableItem::CreateGraphicsGeometry() {}

void VsMovableItem::CreatePhysicsGeometry() {}

void VsMovableItem::ResizePhysicsGeometry() {}

void VsMovableItem::CreateGeometry() 
{
	CreateGraphicsGeometry();
	m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	osgGroup->setName(m_lpThisAB->Name() + "_Node");

	//If ther is a geometry rotation then apply it first, otherwise
	//just use the node straight out.
	if(m_osgGeometryRotationMT.valid())
	{
		m_osgGeometryRotationMT->addChild(osgGroup);
		m_osgNode = m_osgGeometryRotationMT.get();
	}
	else
		m_osgNode = osgGroup;

	CreatePhysicsGeometry();
}

void VsMovableItem::SetupGraphics()
{
	m_osgParent = ParentOSG();

	if(m_osgParent.valid())
	{
		BuildLocalMatrix();

		SetColor(*m_lpThisMI->Ambient(), *m_lpThisMI->Diffuse(), *m_lpThisMI->Specular(), m_lpThisMI->Shininess());
		SetTexture(m_lpThisMI->Texture());
		SetCulling();
		SetVisible(m_lpThisMI->IsVisible());

		//Add it to the scene graph.
		m_osgParent->addChild(m_osgRoot.get());

		//Set the position with the world coordinates.
		Physics_UpdateAbsolutePosition();

		//We need to set the UserData on the OSG side so we can do picking.
		//We need to use a node visitor to set the user data for all drawable nodes in all geodes for the group.
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void VsMovableItem::DeleteGraphics()
{
	if(m_osgParent.valid() && m_osgRoot.valid())
	{
		if(m_osgParent->containsNode(m_osgRoot.get()))
			m_osgParent->removeChild(m_osgRoot.get());
	}

	if(m_osgSelVertexNode.valid()) m_osgSelVertexNode.release();
	if(m_osgSelVertexMT.valid()) m_osgSelVertexMT.release();

	if(m_osgCull.valid()) m_osgCull.release();
	if(m_osgTexture.valid()) m_osgTexture.release();
	if(m_osgStateSet.valid()) m_osgStateSet.release();
	if(m_osgMaterial.valid()) m_osgMaterial.release();

	if(m_osgGeometry.valid()) m_osgGeometry.release();
	if(m_osgNode.valid()) m_osgNode.release();
	if(m_osgSelectedGroup.valid()) m_osgSelectedGroup.release();
	if(m_osgNodeGroup.valid()) m_osgNodeGroup.release();
	if(m_osgGeometryRotationMT.valid()) m_osgGeometryRotationMT.release();
	if(m_osgMT.valid()) m_osgMT.release();
	if(m_osgRoot.valid()) m_osgRoot.release();
	if(m_osgParent.valid()) m_osgParent.release();
}

VsMovableItem *VsMovableItem::VsParent()
{
	return dynamic_cast<VsMovableItem *>(m_lpThisMI->Parent());
}

osg::Matrix VsMovableItem::GetWorldMatrix()
{
	return m_osgWorldMatrix;
}

osg::Matrix VsMovableItem::GetParentWorldMatrix()
{
	if(m_lpThisVsMI && m_lpThisVsMI->VsParent())
		return m_lpThisVsMI->VsParent()->GetWorldMatrix();
	
	osg::Matrix osgMatrix;
	osgMatrix.makeIdentity();
	return osgMatrix;
}

void VsMovableItem::UpdateWorldMatrix()
{
	osg::Matrix osgParentMatrix = GetParentWorldMatrix();

	//Multiply the two matrices together to get the new world location.
	m_osgWorldMatrix = m_osgFinalMatrix * osgParentMatrix;
}

CStdFPoint VsMovableItem::GetOSGWorldCoords()
{
	UpdateWorldMatrix();
	osg::Vec3 vCoord = m_osgWorldMatrix.getTrans();
	CStdFPoint vPoint(vCoord[0], vCoord[1], vCoord[2]);

	return vPoint;
}

osg::Matrix VsMovableItem::GetOSGWorldMatrix(BOOL bUpdate)
{
	if(bUpdate)
		UpdateWorldMatrix();

	return m_osgWorldMatrix;
}

BOOL VsMovableItem::Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos)
{
	VsMovableItem *lpParent = m_lpThisVsMI->VsParent();

	if(lpParent)
	{
		fltWorldX *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();
		fltWorldY *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();
		fltWorldZ *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();

		CStdFPoint vPos(fltWorldX, fltWorldY, fltWorldZ), vRot(0, 0, 0);
		osg::Matrix osgWorldPos = SetupMatrix(vPos, vRot);

		//Get the parent object.
		osg::Matrix osgInverse = osg::Matrix::inverse(lpParent->GetWorldMatrix());

		osg::Matrix osgCalc = osgWorldPos * osgInverse;

		osg::Vec3 vCoord = osgCalc.getTrans();
		vLocalPos.Set(vCoord[0] * m_lpThisAB->GetSimulator()->DistanceUnits(), 
				      vCoord[1] * m_lpThisAB->GetSimulator()->DistanceUnits(), 
				      vCoord[2] * m_lpThisAB->GetSimulator()->DistanceUnits());
		
		return TRUE;
	}

	return FALSE;
}

/*
CStdFPoint VsMovableItem::GetOSGWorldCoords()
{
	CStdFPoint vTemp = GetMyOSGWorldCoords();

	CStdFPoint vOld = GetOSGWorldCoords(GetMatrixTransform());

	int iDiff = 0;
	if(vOld != vTemp)
		iDiff = 1;

	return vOld;
}

CStdFPoint VsMovableItem::GetOSGWorldCoords(osg::MatrixTransform *osgMT)
{
	WorldCoordinateNodeVisitor ncv;
	osgMT->accept(ncv);
	osg::Vec3 vCoord = ncv.MatrixTransform().getTrans();
	CStdFPoint vPoint(vCoord[0], vCoord[1], vCoord[2]);
	return vPoint;
}

osg::Matrix VsMovableItem::GetOSGWorldMatrix()
{
	return GetOSGWorldMatrix(GetMatrixTransform());
}

osg::Matrix VsMovableItem::GetOSGWorldMatrix(osg::MatrixTransform *osgMT)
{
	WorldCoordinateNodeVisitor ncv;
	osgMT->accept(ncv);
	return ncv.MatrixTransform();
}
*/

void VsMovableItem::WorldToBodyCoords(VxReal3 vWorld, StdVector3 &vLocalPos)
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

osg::MatrixTransform* VsMovableItem::GetMatrixTransform()
{
	return m_osgMT.get();
}

/**
\brief	Gets the matrix transform used by the camera for the mouse spring.

\details Sometimes it is necessary to rotate the geometry that was generated to match the correct
orientation of the physics geometry. If this MT is set then this is added 
BEFORE the local matrix so we can make the graphics and physics geometries match. If
it is not set then it is not used. The mouse spring needs to have the end matrix transform to work 
correctly, but I do not want to use that for the matrix other parts use because it adds an extra 
rotation that it should not to other parts then.

\author	dcofer
\date	5/15/2011

\return	Pointer to the matrix transform used by the camera.
**/
osg::MatrixTransform* VsMovableItem::GetCameraMatrixTransform()
{
	if(m_osgGeometryRotationMT.valid())
		return m_osgGeometryRotationMT.get();
	else
		return m_osgMT.get();
}

void VsMovableItem::UpdatePositionAndRotationFromMatrix()
{
	UpdatePositionAndRotationFromMatrix(m_osgMT->getMatrix());
}

void VsMovableItem::UpdatePositionAndRotationFromMatrix(osg::Matrix osgMT)
{
	LocalMatrix(osgMT);

	//Lets get the current world coordinates for this body part and then recalculate the 
	//new local position for the part and then finally reset its new local position.
	osg::Vec3 vL = m_osgLocalMatrix.getTrans();
	CStdFPoint vLocal(vL.x(), vL.y(), vL.z());
	vLocal.ClearNearZero();
	m_lpThisMI->Position(vLocal, FALSE, TRUE, FALSE);
		
	//Now lets get the euler angle rotation
	Vx::VxReal44 vxTM;
	VxOSG::copyOsgMatrix_to_VxReal44(m_osgLocalMatrix, vxTM);
	Vx::VxTransform vTrans(vxTM);
	Vx::VxReal3 vEuler;
	vTrans.getRotationEulerAngles(vEuler);
	CStdFPoint vRot(vEuler[0], vEuler[1] ,vEuler[2]);
	vRot.ClearNearZero();
	m_lpThisMI->Rotation(vRot, TRUE, FALSE);

	m_osgDragger->SetupMatrix();

	//Test the matrix to make sure they match. I will probably get rid of this code after full testing.
	osg::Matrix osgTest = SetupMatrix(vLocal, vRot);
	if(!OsgMatricesEqual(osgTest, m_osgLocalMatrix))
		THROW_ERROR(Vs_Err_lUpdateMatricesDoNotMatch, Vs_Err_strUpdateMatricesDoNotMatch);
}

void VsMovableItem::Physics_UpdateMatrix()
{
	if(m_osgMT.valid())
	{
		LocalMatrix(SetupMatrix(m_lpThisMI->Position(), m_lpThisMI->Rotation()));
		m_osgMT->setMatrix(m_osgLocalMatrix);

		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		Physics_UpdateAbsolutePosition();
	}
}

void VsMovableItem::Physics_UpdateAbsolutePosition()
{
	//If we are here then we did not have a physics component, just and OSG one.
	CStdFPoint vPos = VsMovableItem::GetOSGWorldCoords();
	vPos.ClearNearZero();
	m_lpThisMI->AbsolutePosition(vPos.x, vPos.y, vPos.z);
}

void VsMovableItem::BuildLocalMatrix()
{
	//build the local matrix
	BuildLocalMatrix(m_lpThisMI->Position(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
}

void VsMovableItem::BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName)
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
	if(!m_osgNodeGroup.valid() && m_osgNode.valid())
	{
		m_osgNodeGroup = new osg::Group();
		m_osgNodeGroup->addChild(m_osgNode.get());		
		m_osgNodeGroup->setName(strName + "_NodeGroup");
		
		m_osgMT->addChild(m_osgNodeGroup.get());
	
		CreateSelectedGraphics(strName);
	}
}

void VsMovableItem::Physics_ResetGraphicsAndPhysics()
{
	BuildLocalMatrix();

	SetupPhysics();	
}

void VsMovableItem::Physics_PositionChanged()
{
	Physics_UpdateMatrix();
}

void VsMovableItem::Physics_RotationChanged()
{
	Physics_UpdateMatrix();
}

BoundingBox VsMovableItem::Physics_GetBoundingBox()
{
 	BoundingBox abb;

	osg::BoundingBox bb;
	osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
	if(osgGroup)
	{
		bb = osgGroup->getBoundingBox();
		abb.Set(bb.xMin(), bb.yMin(), bb.zMin(), bb.xMax(), bb.yMax(), bb.zMax());
	}
	else if(m_osgNode.valid())
	{
		osg::BoundingSphere osgBound =	m_osgNode->getBound();
		abb.Set(-osgBound.radius(), -osgBound.radius(), -osgBound.radius(), osgBound.radius(), osgBound.radius(), osgBound.radius()); 
	}
	else
	{
		abb.Set(-0.5, -0.5, -0.5, 0.5, 0.5, 0.5); 
	}

	return abb;
}

float VsMovableItem::Physics_GetBoundingRadius()
{
	BoundingBox bb = Physics_GetBoundingBox();
	return bb.MaxDimension();

	//if(m_osgNode.valid())
	//{
	//	osg::BoundingSphere osgBound =	m_osgNode->getBound();
	//	return osgBound.radius();
	//}

	//return 0.5f;
}

void VsMovableItem::SetTexture(string strTexture)
{
	if(m_osgNode.valid())
	{
		if(!Std_IsBlank(strTexture))
		{
			string strFile = AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), strTexture);
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

void VsMovableItem::Physics_CollectData()
{
	//If we are here then we did not have a physics component, just and OSG one.
	Physics_UpdateAbsolutePosition();

	//TODO: Get Rotation
	//m_lpThis->ReportRotation(QuaterionToEuler(m_osgLocalMatrix.getRotate());
}

void VsMovableItem::Physics_ResetSimulation()
{
	if(m_osgMT.valid())
	{
		BuildLocalMatrix();

		//Set the position with the world coordinates.
		Physics_UpdateAbsolutePosition();
		m_lpThisMI->ReportRotation(m_lpThisMI->Rotation());
	}
}


void VsMovableItem::SetCulling()
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

void VsMovableItem::ShowSelectedVertex() {}

void VsMovableItem::HideSelectedVertex() {}

void VsMovableItem::SetAlpha()
{
	switch (m_lpThisAB->GetSimulator()->VisualSelectionMode())
	{
		case GRAPHICS_SELECTION_MODE:
			m_lpThisMI->Alpha(m_lpThisMI->GraphicsAlpha());			
			HideSelectedVertex();
			break;

		case COLLISION_SELECTION_MODE:
			m_lpThisMI->Alpha(m_lpThisMI->CollisionsAlpha());				
			HideSelectedVertex();
			break;

		case JOINT_SELECTION_MODE:
			m_lpThisMI->Alpha(m_lpThisMI->JointsAlpha());					
			HideSelectedVertex();
			break;

		case RECEPTIVE_FIELD_SELECTION_MODE:
			m_lpThisMI->Alpha(m_lpThisMI->ReceptiveFieldsAlpha());
			ShowSelectedVertex();
			break;

		case SIMULATION_SELECTION_MODE:
			m_lpThisMI->Alpha(m_lpThisMI->SimulationAlpha());					
			HideSelectedVertex();
			break;

		default:
			m_lpThisMI->Alpha(m_lpThisMI->GraphicsAlpha());					
			HideSelectedVertex();
			break;
	}

	if(m_osgMaterial.valid() && m_osgStateSet.valid())
		SetMaterialAlpha(m_osgMaterial.get(), m_osgStateSet.get(), m_lpThisMI->Alpha());
}

void VsMovableItem::SetMaterialAlpha(osg::Material *osgMat, osg::StateSet *ss, float fltAlpha)
{
	osgMat->setAlpha(osg::Material::FRONT_AND_BACK, fltAlpha);

	if(fltAlpha < 1)
		ss->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
	else
		ss->setRenderingHint(osg::StateSet::OPAQUE_BIN);
}

void VsMovableItem::SetColor(CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess)
{
	if(m_osgNode.valid())
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
}

void VsMovableItem::SetVisible(osg::Node *osgNode, BOOL bVisible)
{
	if(osgNode)
	{
		if(bVisible)
			osgNode->setNodeMask(0x1);
		else
			osgNode->setNodeMask(0x0);
	}
}

void VsMovableItem::SetVisible(BOOL bVisible)
{
	SetVisible(m_osgNode.get(), bVisible);
}

void VsMovableItem::CreateItem()
{
	m_lpThisAB->Initialize();
	SetupGraphics();
	SetupPhysics();
}

void VsMovableItem::EndGripDrag()
{
	this->UpdatePositionAndRotationFromMatrix();
}
//
//osg::Vec3 VsMovableItem::FindPointOnSurface(osg::Vec3 vDirection)
//{
//	//If the parent object is not set then we cannot do orientation.
//	if(!m_lpThisMI)
//		return osg::Vec3(0, 0, 0);
//
//	CStdFPoint vAbsPos = m_lpThisMI->AbsolutePosition();
//	osg::Vec3 vPos(vAbsPos.x, vAbsPos.y, vAbsPos.z);
//	
//	osg::Vec3 vStart = vPos - (vDirection*10);
//	osg::Vec3 vEnd = vPos + (vDirection*10);
//
//	osg::LineSegment* osgLine = new osg::LineSegment();
//	osgLine->set(vStart, vEnd);
//
//	osgUtil::IntersectVisitor findIntersectVisitor;
//	findIntersectVisitor.addLineSegment(osgLine);
//	findIntersectVisitor.apply(*m_osgNodeGroup.get()); //
//
//	osgUtil::IntersectVisitor::HitList tankIntersectHits;
//	tankIntersectHits = findIntersectVisitor.getHitList(osgLine);
//	osgUtil::Hit heightTestResults;
//	if ( tankIntersectHits.empty() )
//		return osg::Vec3(0, 0, 0);
//
//	heightTestResults = tankIntersectHits.front();
//	osg::Vec3d vIntersect = heightTestResults.getLocalIntersectPoint();;
//
//	return vIntersect;
//}
//
//void VsMovableItem::Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm)
//{
//	//If the parent object is not set then we cannot do orientation.
//	if(!m_lpThisMI || !m_lpThisMI->Parent())
//		return;
//
//	osg::Vec3 vClickPos(fltXPos, fltYPos, fltZPos), vClickNormal(fltXNorm, fltYNorm, fltZNorm);
//	osg::Vec3 vPointOnSurf = FindPointOnSurface(vClickNormal);
//	
//	osg::Vec3 vWorldPos = vClickPos + vPointOnSurf;
//
//	CStdFPoint vParentPos = m_lpThisMI->Parent()->AbsolutePosition();
//	osg::Vec3 vParent(vParentPos.x, vParentPos.y, vParentPos.z);
//
//	osg::Vec3 vLocalPos = vWorldPos - vParent;
//
//
//	osg::Vec3 vInitDir(0, 0, 1);
//	float fltDot = vInitDir * vClickNormal;
//	float fltAngle = acos(fltDot);
//	osg::Vec3 vAxis = vInitDir ^ vClickNormal;
//
//	//Setup the new local matrix.
//	osg::Matrix osgM;
//	osgM.makeIdentity();
//	osgM.makeRotate(fltAngle, vAxis);
//	osgM.makeTranslate(vLocalPos);
//
//	UpdatePositionAndRotationFromMatrix(osgM);
//
//	//rbNewPart.DxLocation = v + rbNewPart.FindPointOnSurface(new Vector3(), -rbParent.FaceNormal);
//
//	//Vector3 v3InitDir = new Vector3(0,0,1);
//	//
//	//float fltAngle = (float)Math.Acos(Vector3.Dot(v3InitDir,Direction));
//
//	//Vector3 v3Axis = Vector3.Cross(v3InitDir, Direction);
//
//	//m_mtxOrientation.RotateAxis(v3Axis,fltAngle);
//}


void VsMovableItem::Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm)
{
	//If the parent object is not set then we cannot do orientation.
	if(!m_lpThisMI || !m_lpThisMI->Parent())
		return;

	CStdFPoint vParentPos = m_lpThisMI->Parent()->AbsolutePosition();
	osg::Vec3 vParent(vParentPos.x, vParentPos.y, vParentPos.z);

	osg::Vec3 vClickPos(fltXPos, fltYPos, fltZPos), vClickNormal(fltXNorm, fltYNorm, fltZNorm);

	//Lets get the bounding radius for this part
	float fltRadius = Physics_GetBoundingRadius();

	//Now add the part at the specified position, but a radius away.
	osg::Vec3 vWorldPos = vClickPos + (vClickNormal*fltRadius);

	//Calculate the local position relative to the parent.
	osg::Vec3 vLocalPos = vWorldPos - vParent;

	//Now reset our position
	m_lpThisMI->Position(vLocalPos[0], vLocalPos[1], vLocalPos[2], FALSE, TRUE, TRUE);
}

	}			// Environment
//}				//VortexAnimatSim

}