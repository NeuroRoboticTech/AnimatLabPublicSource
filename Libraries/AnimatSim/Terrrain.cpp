/**
\file	Terrrain.cpp

\brief	Implements the terrrain class. 
**/

#include "StdAfx.h"
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
	m_bFreeze = true;
	m_strCollisionMeshType = "TERRAIN";
	m_fltSegmentWidth = 1;
	m_fltSegmentLength = 1;
	m_fltMaxHeight = 5;
	m_iTextureLengthSegments = 10;
	m_iTextureWidthSegments = 10;
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

void Terrain::SegmentWidth(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Terrain.SegmentWidth");
	if(bUseScaling)
		m_fltSegmentWidth = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSegmentWidth = fltVal;

	Resize();
}

float Terrain::SegmentLength() {return m_fltSegmentLength;}

void Terrain::SegmentLength(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Terrain.SegmentLength");
	if(bUseScaling)
		m_fltSegmentLength = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSegmentLength = fltVal;

	Resize();
}

float Terrain::MaxHeight() {return m_fltMaxHeight;}

void Terrain::MaxHeight(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Terrain.MaxHeight");
	if(bUseScaling)
		m_fltMaxHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMaxHeight = fltVal;

	Resize();
}

int Terrain::TextureLengthSegments()
{return m_iTextureLengthSegments;}

void Terrain::TextureLengthSegments(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "Terrain.TextureLengthSegments");
	m_iTextureLengthSegments = iVal;

	//Reset the texture
	Texture(m_strTexture);
}

int Terrain::TextureWidthSegments()
{return m_iTextureWidthSegments;}

void Terrain::TextureWidthSegments(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "Terrain.TextureWidthSegments");
	m_iTextureWidthSegments = iVal;

	//Reset the texture
	Texture(m_strTexture);
}

bool Terrain::AllowRotateDragX() {return false;}

bool Terrain::AllowRotateDragY() {return false;}

bool Terrain::AllowRotateDragZ() {return false;}

bool Terrain::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Mesh::SetData(strType, strValue, false))
		return true;

	if(strType == "SEGMENTWIDTH")
	{
		SegmentWidth((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "SEGMENTLENGTH")
	{
		SegmentLength((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXHEIGHT")
	{
		MaxHeight((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "TEXTURELENGTHSEGMENTS")
	{
		TextureLengthSegments(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "TEXTUREWIDTHSEGMENTS")
	{
		TextureWidthSegments(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Terrain::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	Mesh::QueryProperties(aryNames, aryTypes);

	aryNames.Add("SegmentWidth");
	aryTypes.Add("Float");

	aryNames.Add("SegmentLength");
	aryTypes.Add("Float");

	aryNames.Add("MaxHeight");
	aryTypes.Add("Float");

	aryNames.Add("TextureLengthSegments");
	aryTypes.Add("Integer");

	aryNames.Add("TextureWidthSegments");
	aryTypes.Add("Integer");
}

void Terrain::Load(CStdXml &oXml)
{
	Mesh::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	SegmentWidth(oXml.GetChildFloat("SegmentWidth", m_fltSegmentWidth));
	SegmentLength(oXml.GetChildFloat("SegmentLength", m_fltSegmentLength));
	MaxHeight(oXml.GetChildFloat("MaxHeight", m_fltMaxHeight));
	TextureLengthSegments(oXml.GetChildInt("TextureLengthSegments", m_iTextureLengthSegments));
	TextureWidthSegments(oXml.GetChildInt("TextureWidthSegments", m_iTextureWidthSegments));

	oXml.OutOfElem(); //OutOf RigidBody Element

	//Density is always zero
	m_fltDensity = 0;

	//This part type is always frozen
	m_bFreeze = true;
}


		}		//Bodies
	}			//Environment
}				//AnimatSim