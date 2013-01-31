/**
\file	Mesh.cpp

\brief	Implements the mesh class.
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

\author	dcofer
\date	5/26/2011
**/
Mesh::Mesh()
{
	m_vScale.Set(1, 1, 1);
}

/**
\brief	Destructor.

\author	dcofer
\date	5/26/2011
**/
Mesh::~Mesh()
{
}

/**
\brief	Gets the mesh filename.

\details If this filename is blank, or is not found then it will display a default box.

\author	dcofer
\date	5/26/2011

\return	mesh filename.
**/
string Mesh::MeshFile() {return m_strMeshFile;}

/**
\brief	Sets the mesh filename.

\details If this filename is blank, or is not found then it will display a default box.
\author	dcofer
\date	5/26/2011

\param	strFile	The filename.
**/
void Mesh::MeshFile(string strFile) 
{
	m_strMeshFile = strFile;
	Resize();
}

/**
\brief	Gets the collision mesh type.

\details This is only used if this is a collision object. It can be either convex or triangular (regular).

\author	dcofer
\date	5/26/2011

\return	collision mesh type.
**/
string Mesh::CollisionMeshType() {return m_strCollisionMeshType;}

/**
\brief	Sets the collision mesh type.

\details This is only used if this is a collision object. It can be either convex or triangular (regular).

\author	dcofer
\date	5/26/2011

\param	strType	Type of the mesh.
**/
void Mesh::CollisionMeshType(string strType)
{
	string strUpType = Std_CheckString(strType);
	if(strUpType != "TRIANGULAR" && strUpType != "CONVEX" && strUpType != "TERRAIN")
		THROW_TEXT_ERROR(Al_Err_lInvalidCollisionMeshType, Al_Err_strInvalidCollisionMeshType, "Body: " + m_strName + "  MeshType: " + m_strCollisionMeshType);

	m_strCollisionMeshType = strUpType;
	Resize();
}


/**
\brief	Gets the convex mesh filename.

\details If this filename is blank, or is not found then it will display a default box.

\author	dcofer
\date	5/26/2011

\return	mesh filename.
**/
string Mesh::ConvexMeshFile() {return m_strConvexMeshFile;}

/**
\brief	Sets the convex mesh filename.

\details If this filename is blank, or is not found then it will display a default box.
\author	dcofer
\date	5/26/2011

\param	strFile	The filename.
**/
void Mesh::ConvexMeshFile(string strFile) 
{
	m_strConvexMeshFile = strFile;
	Resize();
}


/**
\brief	Gets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
CStdFPoint Mesh::Scale() {return m_vScale;}

/**
\brief	Sets the local sacle. (m_vScale) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint		The new point to use to set the scale. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void Mesh::Scale(CStdFPoint &oPoint, BOOL bUpdateMatrix) 
{
	m_vScale = oPoint;

	if(m_lpPhysicsMovableItem && bUpdateMatrix)
		m_lpPhysicsMovableItem->Physics_Resize();
}

/**
\brief	Sets the scale. (m_vScale) 

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bUseScaling			If true then the scale values that are passed in will be scaled by
							the unit scaling values. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void Mesh::Scale(float fltX, float fltY, float fltZ, BOOL bUpdateMatrix) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	Scale(vPos);
}

/**
\brief	Sets the scale of the mesh. (m_vScale). This method is primarily used by the GUI to
scale of the mesh using an xml data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the scale. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->SizeChanged
							callback method to inform the GUI that the part has moved. If false
							then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void Mesh::Scale(string strXml, BOOL bUpdateMatrix)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Scale");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Scale", vPos);
	Scale(vPos);
}


void Mesh::SetMeshFile(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("MeshFile");

	m_strMeshFile = oXml.GetChildString("MeshFile");
	m_strConvexMeshFile = oXml.GetChildString("ConvexMeshFile", "");
	CollisionMeshType(oXml.GetChildString("MeshType"));
}

BOOL Mesh::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "MESHFILE")
	{
		MeshFile(strValue);
		return TRUE;
	}

	if(strType == "MESHTYPE")
	{
		CollisionMeshType(strValue);
		return TRUE;
	}

	if(strType == "CONVEXMESHFILE")
	{
		ConvexMeshFile(strValue);
		return TRUE;
	}

	if(strType == "SETMESHFILE")
	{
		SetMeshFile(strValue);
		return TRUE;
	}

	if(strType == "SCALE")
	{
		Scale(strValue);
		return TRUE;
	}

	if(strType == "SCALE.X")
	{
		Scale(atof(strValue.c_str()), m_vScale.y, m_vScale.z);
		return TRUE;
	}

	if(strType == "SCALE.Y")
	{
		Scale(m_vScale.x, atof(strValue.c_str()), m_vScale.z);
		return TRUE;
	}

	if(strType == "SCALE.Z")
	{
		Scale(m_vScale.x, m_vScale.y, atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Mesh::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	RigidBody::QueryProperties(aryNames, aryTypes);

	aryNames.Add("MeshFile");
	aryTypes.Add("String");

	aryNames.Add("MeshType");
	aryTypes.Add("String");

	aryNames.Add("ConvexMeshFile");
	aryTypes.Add("String");

	aryNames.Add("SetMeshFile");
	aryTypes.Add("Xml");

	aryNames.Add("Scale");
	aryTypes.Add("Xml");

	aryNames.Add("Scale.X");
	aryTypes.Add("Float");

	aryNames.Add("Scale.Y");
	aryTypes.Add("Float");

	aryNames.Add("Scale.Z");
	aryTypes.Add("Float");
}

void Mesh::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	MeshFile(oXml.GetChildString("MeshFile"));
	CollisionMeshType(oXml.GetChildString("MeshType"));
	ConvexMeshFile(oXml.GetChildString("ConvexMeshFile", ""));

	Std_LoadPoint(oXml, "Scale", m_vScale, false);

	oXml.OutOfElem(); //OutOf RigidBody Element

}

		}		//Bodies
	}			//Environment
}				//AnimatSim
