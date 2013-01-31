/**
\file	Terrrain.cpp

\brief	Implements the terrrain class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Mesh.h"
#include "Terrain.h"
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
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
/**
\brief	Default constructor. 

\details The density of this part is defaulted to zero, and it is setup to always be frozen.

\author	dcofer
\date	3/10/2011
**/
Terrain::Terrain()
{
	m_fltDensity = 0;
	m_bFreeze = TRUE;
	m_strCollisionMeshType = "TERRAIN";
	m_fltSegmentWidth = 1;
	m_fltSegmentLength = 1;
	m_fltMaxHeight = 5;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Terrain::~Terrain()
{

}

float Terrain::SegmentWidth() {return m_fltSegmentWidth;}

void Terrain::SegmentWidth(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Terrain.SegmentWidth");
	if(bUseScaling)
		m_fltSegmentWidth = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSegmentWidth = fltVal;

	Resize();
}

float Terrain::SegmentLength() {return m_fltSegmentLength;}

void Terrain::SegmentLength(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Terrain.SegmentLength");
	if(bUseScaling)
		m_fltSegmentLength = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSegmentLength = fltVal;

	Resize();
}

float Terrain::MaxHeight() {return m_fltMaxHeight;}

void Terrain::MaxHeight(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Terrain.MaxHeight");
	if(bUseScaling)
		m_fltMaxHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMaxHeight = fltVal;

	Resize();
}

BOOL Terrain::AllowRotateDragX() {return FALSE;}

BOOL Terrain::AllowRotateDragY() {return FALSE;}

BOOL Terrain::AllowRotateDragZ() {return FALSE;}

BOOL Terrain::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Mesh::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "SEGMENTWIDTH")
	{
		SegmentWidth(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SEGMENTLENGTH")
	{
		SegmentLength(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "MAXHEIGHT")
	{
		MaxHeight(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Terrain::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Mesh::QueryProperties(aryNames, aryTypes);

	aryNames.Add("SegmentWidth");
	aryTypes.Add("Float");

	aryNames.Add("SegmentLength");
	aryTypes.Add("Float");

	aryNames.Add("MaxHeight");
	aryTypes.Add("Float");
}

void Terrain::Load(CStdXml &oXml)
{
	Mesh::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	SegmentWidth(oXml.GetChildFloat("SegmentWidth", m_fltSegmentWidth));
	SegmentLength(oXml.GetChildFloat("SegmentLength", m_fltSegmentLength));
	MaxHeight(oXml.GetChildFloat("MaxHeight", m_fltMaxHeight));

	oXml.OutOfElem(); //OutOf RigidBody Element

	//Density is always zero
	m_fltDensity = 0;

	//This part type is always frozen
	m_bFreeze = TRUE;
}


		}		//Bodies
	}			//Environment
}				//AnimatSim