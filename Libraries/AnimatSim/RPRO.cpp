/**
\file	RPRO.cpp

\brief	Implements the relative position, relative orientation class.
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
#include "RPRO.h"
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
		namespace Joints
		{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
RPRO::RPRO()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
RPRO::~RPRO()
{

}

/**
\brief	Gets the radius of the cylinder that extends out from the ball for this joint.

\author	dcofer
\date	4/15/2011

\return	cylinder radius.
**/
float RPRO::CylinderRadius()
{
	return m_fltSize/2;
}

/**
\brief	Gets the height of the cylinder that extends out from the ball for this joint.

\author	dcofer
\date	4/15/2011

\return	cylinder height.
**/
float RPRO::CylinderHeight()
{
	return m_fltSize;
}

/**
\brief	Gets the radius of the ball that is siutated at the center of this joint.

\author	dcofer
\date	4/15/2011

\return	radius of the ball.
**/
float RPRO::BallRadius()
{
	return m_fltSize;
}

void RPRO::Load(CStdXml &oXml)
{
	Joint::Load(oXml);

	oXml.IntoElem();  //Into Joint Element
	
	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
