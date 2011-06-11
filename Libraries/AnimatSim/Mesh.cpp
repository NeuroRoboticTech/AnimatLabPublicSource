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


void Mesh::SetMeshFile(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("MeshFile");

	//oXml.IntoElem();

	m_strMeshFile = oXml.GetChildString("MeshFile");
	m_strConvexMeshFile = oXml.GetChildString("ConvexMeshFile", "");
	CollisionMeshType(oXml.GetChildString("MeshType"));

	//oXml.OutOfElem();
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

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Mesh::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	MeshFile(oXml.GetChildString("MeshFile"));
	CollisionMeshType(oXml.GetChildString("MeshType"));
	ConvexMeshFile(oXml.GetChildString("ConvexMeshFile", ""));

	oXml.OutOfElem(); //OutOf RigidBody Element

}

		}		//Bodies
	}			//Environment
}				//AnimatSim
