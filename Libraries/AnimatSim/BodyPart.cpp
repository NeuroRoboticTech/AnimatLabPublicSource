#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
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
	namespace Environment
	{

BodyPart::BodyPart(void)
{
	m_bIsVisible = TRUE;
	m_bAllowMouseManipulation = TRUE;
	m_lpCallback = NULL;
	m_lpStructure = NULL;
	m_lpParent = NULL;
	m_lpPhysicsBody = NULL;

	m_fltGraphicsAlpha = 1;
	m_fltCollisionsAlpha = 1;
	m_fltJointsAlpha = 1;
	m_fltReceptiveFieldsAlpha = 1;
	m_fltSimulationAlpha = 1;
	m_fltAlpha = 1;
	m_fltGripScale = 1;
}

BodyPart::~BodyPart(void)
{
}


void BodyPart::Selected(BOOL bValue, BOOL bSelectMultiple)
{
	Node::Selected(bValue, bSelectMultiple);

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_Selected(bValue, bSelectMultiple);

	if(m_lpCallback)
		m_lpCallback->SelectionChanged(bValue, bSelectMultiple);
}

void BodyPart::VisualSelectionModeChanged(int iNewMode)
{
	Node::VisualSelectionModeChanged(iNewMode);

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

void BodyPart::IsVisible(BOOL bVal) 
{
	m_bIsVisible = bVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetVisible(m_bIsVisible);
}

void BodyPart::GraphicsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "GraphicsAlpha");

	m_fltGraphicsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

void BodyPart::CollisionsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "CollisionsAlpha");

	m_fltCollisionsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

void BodyPart::JointsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "JointsAlpha");

	m_fltJointsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

void BodyPart::ReceptiveFieldsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "ReceptiveFieldsAlpha");

	m_fltReceptiveFieldsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

void BodyPart::SimulationAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "SimulationAlpha");

	m_fltSimulationAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

float BodyPart::GetBoundingRadius()
{
	if(m_lpPhysicsBody)
		return m_lpPhysicsBody->Physics_GetBoundingRadius();
	else
		return 1;
}

void BodyPart::LocalPosition(CStdFPoint &oPoint, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	if(bUseScaling)
		m_oLocalPosition = oPoint * m_lpSim->InverseDistanceUnits();
	else
		m_oLocalPosition = oPoint;
	m_oReportLocalPosition = m_oLocalPosition * m_lpSim->DistanceUnits();
	

	if(m_lpPhysicsBody && bUpdateMatrix)
		m_lpPhysicsBody->Physics_UpdateMatrix();

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->PositionChanged();
}

void BodyPart::LocalPosition(float fltX, float fltY, float fltZ, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	LocalPosition(vPos, bUseScaling, bFireChangeEvent);
}

void BodyPart::LocalPosition(string strXml, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("LocalPosition");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "LocalPosition", vPos);
	LocalPosition(vPos, bUseScaling, bFireChangeEvent);
}

void BodyPart::Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	m_oRotation = oPoint;
	m_oReportRotation = m_oRotation;

	if(m_lpPhysicsBody && bUpdateMatrix)
		m_lpPhysicsBody->Physics_UpdateMatrix();

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->RotationChanged();
}

void BodyPart::Rotation(float fltX, float fltY, float fltZ, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	Rotation(vPos, bFireChangeEvent);
}

void BodyPart::Rotation(string strXml, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Rotation");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Rotation", vPos);
	Rotation(vPos, bFireChangeEvent);
}


void BodyPart::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)
{
	if(m_lpCallback)
		m_lpCallback->AddBodyClicked(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ);
}

void BodyPart::UpdateData(Simulator *lpSim, Structure *lpStructure)
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_CollectBodyData(lpSim);
}


#pragma region DataAccesMethods

float *BodyPart::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "BODYPOSITIONX" || strType == "POSITIONX"|| strType == "WORLDPOSITIONX")
		return &m_oReportWorldPosition.x;

	if(strType == "BODYPOSITIONY" || strType == "POSITIONY"|| strType == "WORLDPOSITIONY")
		return &m_oReportWorldPosition.y;

	if(strType == "BODYPOSITIONZ" || strType == "POSITIONZ"|| strType == "WORLDPOSITIONZ")
		return &m_oReportWorldPosition.z;

	if(strType == "LOCALPOSITIONX")
		return &m_oReportLocalPosition.x;

	if(strType == "LOCALPOSITIONY")
		return &m_oReportLocalPosition.y;

	if(strType == "LOCALPOSITIONZ")
		return &m_oReportLocalPosition.z;

	if(strType == "BODYROTATIONX" || strType == "ROTATIONX")
		return &m_oReportRotation.x;

	if(strType == "BODYROTATIONY" || strType == "ROTATIONY")
		return &m_oReportRotation.y;

	if(strType == "BODYROTATIONZ" || strType == "ROTATIONZ")
		return &m_oReportRotation.z;

	return 0;
}

BOOL BodyPart::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(Node::SetData(strDataType, strValue, FALSE))
		return true;

	if(strDataType == "LOCALPOSITION")
	{
		LocalPosition(strValue);
		return true;
	}

	if(strDataType == "ROTATION")
	{
		Rotation(strValue);
		return true;
	}

	if(strDataType == "VISIBLE")
	{
		IsVisible(Std_ToBool(strValue));
		return true;
	}

	if(strDataType == "GRAPHICSALPHA")
	{
		GraphicsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "COLLISIONALPHA")
	{
		CollisionsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "JOINTSALPHA")
	{
		JointsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "RECEPTIVEFIELDSALPHA")
	{
		ReceptiveFieldsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SIMULATIONALPHA")
	{
		SimulationAlpha(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void BodyPart::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Node::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element

	if(oXml.FindChildElement("LocalPosition", FALSE))
		Std_LoadPoint(oXml, "LocalPosition", m_oLocalPosition);
	else
		m_oLocalPosition.Set(0, 0, 0);
	m_oLocalPosition *= lpSim->InverseDistanceUnits();

	if(!m_lpParent)
		m_oAbsPosition += m_oLocalPosition;
	else
		m_oAbsPosition = m_lpParent->AbsolutePosition() + m_oLocalPosition;

	m_oReportLocalPosition = m_oLocalPosition * lpSim->DistanceUnits();
	m_oReportWorldPosition = m_oAbsPosition * lpSim->DistanceUnits();

	if(oXml.FindChildElement("Rotation", FALSE))
		Std_LoadPoint(oXml, "Rotation", m_oRotation);
	else
		m_oRotation.Set(0, 0, 0);
	m_oReportRotation = m_oRotation;

	m_bIsVisible = oXml.GetChildBool("IsVisible", m_bIsVisible);
	m_fltGraphicsAlpha = oXml.GetChildFloat("GraphicsAlpha", m_fltGraphicsAlpha);
	m_fltCollisionsAlpha = oXml.GetChildFloat("CollisionsAlpha", m_fltCollisionsAlpha);
	m_fltJointsAlpha = oXml.GetChildFloat("JointsAlpha", m_fltJointsAlpha);
	m_fltReceptiveFieldsAlpha = oXml.GetChildFloat("ReceptiveFieldsAlpha", m_fltReceptiveFieldsAlpha);
	m_fltSimulationAlpha = oXml.GetChildFloat("SimulationAlpha", m_fltSimulationAlpha);

	oXml.OutOfElem(); //OutOf RigidBody Element
}

	}			//Environment
}				//AnimatSim