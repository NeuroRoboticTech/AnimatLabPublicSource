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
	m_ptGrid.Set(5, 5, 0);
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Terrain::~Terrain()
{

}

void Terrain::Grid(CStdIPoint ptPoint)
{
	if(ptPoint.x <= 0) ptPoint.x = 1;
	if(ptPoint.y <= 0) ptPoint.y = 1;
	m_ptGrid = ptPoint;
}

BOOL Terrain::AllowMouseManipulation() {return FALSE;}

void Terrain::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	CStdIPoint ptGrid;
	Std_LoadPoint(oXml, "Grid", ptGrid);
	Grid(ptGrid);

	oXml.OutOfElem(); //OutOf RigidBody Element

	//Reset the rotation to 0
	m_oRotation.Set(0, 0, 0);

	//Density is always zero
	m_fltDensity = 0;

	//This part type is always frozen
	m_bFreeze = TRUE;
}


		}		//Bodies
	}			//Environment
}				//AnimatSim