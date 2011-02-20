// Mesh.cpp: implementation of the Mesh class.
//
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a Mesh object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a Mesh. 
*/

Mesh::Mesh()
{
}

/*! \brief 
   Destroys the Mesh object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Mesh object..	 
*/

Mesh::~Mesh()
{
}


void Mesh::Trace(ostream &oOs)
{
	oOs << "{";
	RigidBody::Trace(oOs);
	oOs << "}";
}


void Mesh::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	RigidBody::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element
	Std_LoadPoint(oXml, "CollisionBoxSize", m_oCollisionBoxSize);
	Std_LoadPoint(oXml, "GraphicsBoxSize", m_oGraphicsBoxSize);
	m_strGraphicsMesh = oXml.GetChildString("MeshFile");
	m_strCollisionMesh = oXml.GetChildString("CollisionMeshFile", "");
	m_strCollisionMeshType = oXml.GetChildString("CollisionMeshType");
	m_strReceptiveFieldMesh = oXml.GetChildString("ReceptiveFieldMeshFile", "");
	oXml.OutOfElem(); //OutOf RigidBody Element

	Std_IsAboveMin((float) 0, m_oCollisionBoxSize.x, TRUE, "CollisionBoxSize.x");
	Std_IsAboveMin((float) 0, m_oCollisionBoxSize.y, TRUE, "CollisionBoxSize.y");
	Std_IsAboveMin((float) 0, m_oCollisionBoxSize.z, TRUE, "CollisionBoxSize.z");
	
	Std_IsAboveMin((float) 0, m_oGraphicsBoxSize.x, TRUE, "GraphicsBoxSize.x");
	Std_IsAboveMin((float) 0, m_oGraphicsBoxSize.y, TRUE, "GraphicsBoxSize.y");
	Std_IsAboveMin((float) 0, m_oGraphicsBoxSize.z, TRUE, "GraphicsBoxSize.z");

	if(Std_IsBlank(m_strGraphicsMesh))
		THROW_PARAM_ERROR(Al_Err_lGraphicsMeshNotDefined, Al_Err_strGraphicsMeshNotDefined, "Body", m_strName);

	if(Std_IsBlank(m_strCollisionMesh))
	{
		m_strCollisionMesh = m_strGraphicsMesh;
		m_strCollisionMeshType = "Regular";
	}

	m_strCollisionMeshType = Std_CheckString(m_strCollisionMeshType);
	if(m_strCollisionMeshType != "REGULAR" && m_strCollisionMeshType != "CONVEX")
		THROW_TEXT_ERROR(Al_Err_lInvalidCollisionMeshType, Al_Err_strInvalidCollisionMeshType, "Body: " + m_strName + "  MeshType: " + m_strCollisionMeshType);

	if(m_strCollisionMeshType == "REGULAR")
		lpSim->HasTriangleMesh(TRUE);
	else
		lpSim->HasConvexMesh(TRUE);
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
