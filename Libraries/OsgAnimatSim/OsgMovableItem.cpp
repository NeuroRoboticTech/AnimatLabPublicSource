#include "StdAfx.h"
#include <stdarg.h>
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgOrganism.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

OsgMovableItem::OsgMovableItem()
{
	m_bCullBackfaces = FALSE; //No backface culling by default.
	m_eTextureMode = GL_TEXTURE_2D;

    m_lpOsgSim = NULL;
	m_lpThisAB = NULL;
	m_lpThisMI = NULL;
	m_lpThisVsMI = NULL;
	m_lpParentVsMI = NULL;
}


OsgMovableItem::~OsgMovableItem()
{
}

void OsgMovableItem::SetThisPointers()
{
	m_lpThisAB = dynamic_cast<AnimatBase *>(this);
	if(!m_lpThisAB)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisAB");

	m_lpThisMI = dynamic_cast<MovableItem *>(this);
	if(!m_lpThisMI)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisMI, " + m_lpThisAB->Name());

	m_lpThisVsMI = dynamic_cast<OsgMovableItem *>(this);
	if(!m_lpThisVsMI)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisVsMI, " + m_lpThisAB->Name());

	m_lpThisMI->PhysicsMovableItem(this);
}

OsgSimulator *OsgMovableItem::GetOsgSimulator()
{
    if(!m_lpOsgSim)
    {
        m_lpOsgSim = dynamic_cast<OsgSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisVsMI)
		    THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpOsgSim, " + m_lpThisAB->Name());
    }
    
    return m_lpOsgSim;
}

string OsgMovableItem::Physics_ID()
{
	if(m_lpThisAB)
		return m_lpThisAB->ID();
	else
		return "";
}

#pragma region Selection-Code

void OsgMovableItem::Physics_Selected(BOOL bValue, BOOL bSelectMultiple)  
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

void OsgMovableItem::CreateSelectedGraphics(string strName)
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

void OsgMovableItem::CreateDragger(string strName)
{
	if(m_lpThisAB->GetSimulator())
	{
		if(GetOsgSimulator()->OsgCmdMgr())
		{
			if(m_osgDragger.valid())
				m_osgDragger.release();

			m_osgDragger = new OsgDragger(this, m_lpThisMI->AllowTranslateDragX(), m_lpThisMI->AllowTranslateDragY(), m_lpThisMI->AllowTranslateDragX(),
										 m_lpThisMI->AllowRotateDragX(), m_lpThisMI->AllowRotateDragY(), m_lpThisMI->AllowRotateDragZ(), 
										 m_lpThisMI->UserDefinedDraggerRadius());
			m_osgDragger->setName(strName + "_Dragger");

			m_osgDragger->setupDefaultGeometry();

			GetOsgSimulator()->OsgCmdMgr()->connect(*m_osgDragger, *m_osgMT);

			//Add pointers to this object to the grip so it will no which body part to
			//call the EndGripDrag method on when the drag is finished.
			m_osgDragger->setUserData(new OsgUserData(this));
		}
	}
}

void OsgMovableItem::CreateSelectedVertex(string strName)
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

void OsgMovableItem::DeleteSelectedVertex()
{
	HideSelectedVertex();

	if(m_osgSelVertexNode.valid()) m_osgSelVertexNode.release();
	if(m_osgSelVertexMT.valid()) m_osgSelVertexMT.release();
}

#pragma endregion

float *OsgMovableItem::Physics_GetDataPointer(const string &strDataType) {return NULL;}

void OsgMovableItem::LocalMatrix(osg::Matrix osgLocalMT)
{
	m_osgLocalMatrix = osgLocalMT;
	m_osgFinalMatrix = osgLocalMT;
	UpdateWorldMatrix();
}

void OsgMovableItem::GeometryRotationMatrix(osg::Matrix osgGeometryMT)
{
	if(!m_osgGeometryRotationMT.valid())
	{
		m_osgGeometryRotationMT = new osg::MatrixTransform;
		m_osgGeometryRotationMT->setName(m_lpThisAB->Name() + "_GeometryMT");
	}
	m_osgGeometryRotationMT->setMatrix(osgGeometryMT);	
}

void OsgMovableItem::AttachedPartMovedOrRotated(string strID)
{
	Physics_ResetGraphicsAndPhysics();
}

void OsgMovableItem::CreateGraphicsGeometry() {}

void OsgMovableItem::CreatePhysicsGeometry() {}

void OsgMovableItem::ResizePhysicsGeometry() {}

void OsgMovableItem::CreateGeometry() 
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

void OsgMovableItem::SetupGraphics()
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
		osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void OsgMovableItem::DeleteGraphics()
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

OsgMovableItem *OsgMovableItem::VsParent()
{
	return m_lpParentVsMI;
}

osg::Matrix OsgMovableItem::GetWorldMatrix()
{
	return m_osgWorldMatrix;
}

osg::Matrix OsgMovableItem::GetParentWorldMatrix()
{
	if(m_lpParentVsMI)
		return m_lpParentVsMI->GetWorldMatrix();
	
	osg::Matrix osgMatrix;
	osgMatrix.makeIdentity();
	return osgMatrix;
}

void OsgMovableItem::UpdateWorldMatrix()
{
	osg::Matrix osgParentMatrix = GetParentWorldMatrix();

	//Multiply the two matrices together to get the new world location.
	m_osgWorldMatrix = m_osgFinalMatrix * osgParentMatrix;
}

CStdFPoint OsgMovableItem::GetOSGWorldCoords()
{
	UpdateWorldMatrix();
	osg::Vec3 vCoord = m_osgWorldMatrix.getTrans();
	CStdFPoint vPoint(vCoord[0], vCoord[1], vCoord[2]);

	return vPoint;
}

osg::Matrix OsgMovableItem::GetOSGWorldMatrix(BOOL bUpdate)
{
	if(bUpdate)
		UpdateWorldMatrix();

	return m_osgWorldMatrix;
}

BOOL OsgMovableItem::Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos)
{
	OsgMovableItem *lpParent = m_lpThisVsMI->VsParent();

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

osg::MatrixTransform* OsgMovableItem::GetMatrixTransform()
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
osg::MatrixTransform* OsgMovableItem::GetCameraMatrixTransform()
{
	if(m_osgGeometryRotationMT.valid())
		return m_osgGeometryRotationMT.get();
	else
		return m_osgMT.get();
}

void OsgMovableItem::UpdatePositionAndRotationFromMatrix()
{
	UpdatePositionAndRotationFromMatrix(m_osgMT->getMatrix());
}

void OsgMovableItem::UpdatePositionAndRotationFromMatrix(osg::Matrix osgMT)
{
	LocalMatrix(osgMT);

	//Lets get the current world coordinates for this body part and then recalculate the 
	//new local position for the part and then finally reset its new local position.
	osg::Vec3 vL = osgMT.getTrans();
	CStdFPoint vLocal(vL.x(), vL.y(), vL.z());
	vLocal.ClearNearZero();
	m_lpThisMI->Position(vLocal, FALSE, TRUE, FALSE);
		
	//Now lets get the euler angle rotation
    CStdFPoint vRot = EulerRotationFromMatrix(osgMT);    
	m_lpThisMI->Rotation(vRot, TRUE, FALSE);

	if(m_osgDragger.valid())
		m_osgDragger->SetupMatrix();

	//Test the matrix to make sure they match. I will probably get rid of this code after full testing.
	osg::Matrix osgTest = SetupMatrix(vLocal, vRot);
	if(!OsgMatricesEqual(osgTest, m_osgLocalMatrix))
		THROW_ERROR(Osg_Err_lUpdateMatricesDoNotMatch, Osg_Err_strUpdateMatricesDoNotMatch);
}

void OsgMovableItem::Physics_UpdateMatrix()
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

void OsgMovableItem::Physics_UpdateAbsolutePosition()
{
	//If we are here then we did not have a physics component, just and OSG one.
	CStdFPoint vPos = OsgMovableItem::GetOSGWorldCoords();
	vPos.ClearNearZero();
	m_lpThisMI->AbsolutePosition(vPos.x, vPos.y, vPos.z);
}

void OsgMovableItem::BuildLocalMatrix()
{
	//build the local matrix
	BuildLocalMatrix(m_lpThisMI->Position(), m_lpThisMI->Rotation(), m_lpThisAB->Name());
}

void OsgMovableItem::BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName)
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

void OsgMovableItem::Physics_LoadTransformMatrix(CStdXml &oXml) 
{
	string strMatrix = oXml.GetChildString("TransformMatrix");
	
	CStdArray<string> aryElements;
	int iCount = Std_Split(strMatrix, ",", aryElements);

	if(iCount != 16)
		THROW_PARAM_ERROR(Al_Err_lMatrixElementCountInvalid, Al_Err_strMatrixElementCountInvalid, "Matrix count", iCount);
	
	float aryMT[4][4];
	int iIndex=0;
	for(int iX=0; iX<4; iX++)
		for(int iY=0; iY<4; iY++, iIndex++)
			aryMT[iX][iY] = atof(aryElements[iIndex].c_str());
		
	osg::Matrix osgMT(aryMT[0][0], aryMT[0][1], aryMT[0][2], aryMT[0][3], 
					  aryMT[1][0], aryMT[1][1], aryMT[1][2], aryMT[1][3], 
					  aryMT[2][0], aryMT[2][1], aryMT[2][2], aryMT[2][3], 
					  aryMT[3][0], aryMT[3][1], aryMT[3][2], aryMT[3][3]);
	
	UpdatePositionAndRotationFromMatrix(osgMT);
}

void OsgMovableItem::Physics_ResizeDragHandler(float fltRadius)
{
	BOOL bInScene = FALSE;
	if(m_osgDragger.valid() && m_osgDragger->IsInScene())
	{
		m_osgDragger->RemoveFromScene();
		bInScene = TRUE;
	}

	CreateDragger(m_lpThisAB->Name());
	
	if(bInScene)
		m_osgDragger->AddToScene();
}

void OsgMovableItem::Physics_ResetGraphicsAndPhysics()
{
	BuildLocalMatrix();

	SetupPhysics();	
}

void OsgMovableItem::Physics_PositionChanged()
{
	Physics_UpdateMatrix();
}

void OsgMovableItem::Physics_RotationChanged()
{
	Physics_UpdateMatrix();
}

BoundingBox OsgMovableItem::Physics_GetBoundingBox()
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

float OsgMovableItem::Physics_GetBoundingRadius()
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

void OsgMovableItem::SetTexture(string strTexture)
{
	if(m_osgNode.valid())
	{
		if(!Std_IsBlank(strTexture))
		{
			string strFile = AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), strTexture);
			osg::ref_ptr<osg::Image> image = osgDB::readImageFile(strFile);
			if(!image)
				THROW_PARAM_ERROR(Osg_Err_lTextureLoad, Osg_Err_strTextureLoad, "Image File", strFile);

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

void OsgMovableItem::Physics_CollectData()
{
	//If we are here then we did not have a physics component, just and OSG one.
	Physics_UpdateAbsolutePosition();

	//TODO: Get Rotation
	//m_lpThis->ReportRotation(QuaterionToEuler(m_osgLocalMatrix.getRotate());
}

void OsgMovableItem::Physics_ResetSimulation()
{
	if(m_osgMT.valid())
	{
		BuildLocalMatrix();

		//Set the position with the world coordinates.
		Physics_UpdateAbsolutePosition();
		m_lpThisMI->ReportRotation(m_lpThisMI->Rotation());
	}
}


void OsgMovableItem::SetCulling()
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

void OsgMovableItem::ShowSelectedVertex() {}

void OsgMovableItem::HideSelectedVertex() {}

void OsgMovableItem::SetAlpha()
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

void OsgMovableItem::SetMaterialAlpha(osg::Material *osgMat, osg::StateSet *ss, float fltAlpha)
{
	osgMat->setAlpha(osg::Material::FRONT_AND_BACK, fltAlpha);

	if(fltAlpha < 1)
		ss->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
	else
		ss->setRenderingHint(osg::StateSet::OPAQUE_BIN);
}

void OsgMovableItem::SetColor(CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess)
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

		//apply the material
		m_osgStateSet->setAttribute(m_osgMaterial.get(), osg::StateAttribute::ON);
	}
}

void OsgMovableItem::SetVisible(osg::Node *osgNode, BOOL bVisible)
{
	if(osgNode)
	{
		if(bVisible)
			osgNode->setNodeMask(0x1);
		else
			osgNode->setNodeMask(0x0);
	}
}

void OsgMovableItem::SetVisible(BOOL bVisible)
{
	SetVisible(m_osgNode.get(), bVisible);
}

void OsgMovableItem::CreateItem()
{
	m_lpThisAB->Initialize();
	SetupGraphics();
	SetupPhysics();
}

void OsgMovableItem::EndGripDrag()
{
	this->UpdatePositionAndRotationFromMatrix();
}
//
//osg::Vec3 OsgMovableItem::FindPointOnSurface(osg::Vec3 vDirection)
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
//void OsgMovableItem::Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm)
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


void OsgMovableItem::Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm)
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
//}				//OsgAnimatSim

}