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
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Terrain::~Terrain()
{

}

void Terrain::CollisionMeshType(string strType)
{
	//The collision mesh type is always terrain for this object.
	m_strCollisionMeshType = "TERRAIN";
}

void Terrain::Load(CStdXml &oXml)
{
	Mesh::Load(oXml);

	//oXml.IntoElem();  //Into RigidBody Element

	//oXml.OutOfElem(); //OutOf RigidBody Element

	//Density is always zero
	m_fltDensity = 0;
	//Rotation(-1.5707963f, 0, 0);

	//This part type is always frozen
	m_bFreeze = TRUE;
}


		}		//Bodies
	}			//Environment
}				//AnimatSim