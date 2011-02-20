// Plane.cpp: implementation of the Plane class.
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
#include "Plane.h"
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
   Constructs a plane object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a plane. 
*/

Plane::Plane()
{
	m_fltHeight = 0;
	m_fltDensity = 0;
	m_bFreeze = TRUE;
	m_bAllowMouseManipulation = FALSE;

	m_ptCorner.Set(-100, -100, 0);
	m_ptSize.Set(200, 200, 0);
	m_ptGrid.Set(5, 5, 0);
}

/*! \brief 
   Destroys the plane object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the plane object..	 
*/

Plane::~Plane()
{

}

float Plane::Height()
{return m_fltHeight;}

void Plane::Height(float fltVal)
{m_fltHeight = fltVal;}

void Plane::Size(CStdFPoint ptPoint)
{
	if(ptPoint.x <= 0)
		THROW_PARAM_ERROR(Al_Err_lInavlidPlaneSize, Al_Err_strInavlidPlaneSize, "X Size", ptPoint.x);
	if(ptPoint.y <= 0)
		THROW_PARAM_ERROR(Al_Err_lInavlidPlaneSize, Al_Err_strInavlidPlaneSize, "Y Size", ptPoint.y);
	m_ptSize = ptPoint;
}


void Plane::Grid(CStdIPoint ptPoint)
{
	if(ptPoint.x <= 0) ptPoint.x = 1;
	if(ptPoint.y <= 0) ptPoint.y = 1;
	m_ptGrid = ptPoint;
}

void Plane::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_strTexture = oXml.GetChildString("Texture", "");
	m_fltHeight = oXml.GetChildFloat("Height");

	m_vDiffuse.Load(oXml, "Diffuse", false);
	m_vAmbient.Load(oXml, "Ambient", false);
	m_vSpecular.Load(oXml, "Specular", false);
	m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess);

	/////////////////////////////////////////////////////////////////////////////////////
	//TODO:: Need to change these variables in the asim file to match the new variables.
	CStdFPoint ptSize; CStdIPoint ptGrid;
	Std_LoadPoint(oXml, "MapLocation", m_ptCorner);
	Std_LoadPoint(oXml, "MapSize", ptSize);
	int iGrids = oXml.GetChildInt("MapScale");
	ptGrid.Set(iGrids, iGrids, 0);
	//TODO:: Need to change these variables in the asim file to match the new variables.
	/////////////////////////////////////////////////////////////////////////////////////
	
	this->Grid(ptGrid);
	this->Size(ptSize);

	oXml.OutOfElem(); //OutOf RigidBody Element

	m_oLocalPosition.Set(0, m_fltHeight, 0);
	m_oAbsPosition.Set(0, m_fltHeight, 0);
	m_oRotation.Set(0, 0, 0);
}


/*! \fn float Plane::Height()
   \brief
   Height property.
      
   \remarks
	 The height of the plane on the Y axis.
	 This is the accessor function for the m_fltHeight element.
*/
/*! \fn void Plane::Height(float fltVal)
   \brief
   Height property.
      
   \remarks
	 The height of the plane on the Y axis.
	 This is the mutator function for the m_fltHeight element.
*/


		}		//Bodies
	}			//Environment
}				//AnimatSim