// Terrain.cpp: implementation of the Terrain class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a Terrain object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a Terrain. 
*/

Terrain::Terrain()
{
	m_fltHeight = 0;
	m_fltDensity = 0;
	m_bFreeze = TRUE;
	m_ptGrid.Set(5, 5, 0);
}

/*! \brief 
   Destroys the Terrain object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Terrain object..	 
*/

Terrain::~Terrain()
{

}

float Terrain::Height()
{return m_fltHeight;}

void Terrain::Height(float fltVal)
{m_fltHeight = fltVal;}

void Terrain::Grid(CStdIPoint ptPoint)
{
	if(ptPoint.x <= 0) ptPoint.x = 1;
	if(ptPoint.y <= 0) ptPoint.y = 1;
	m_ptGrid = ptPoint;
}

BOOL Terrain::AllowMouseManipulation() {return FALSE;}

void Terrain::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	m_strTexture = oXml.GetChildString("Texture", "");
	m_fltHeight = oXml.GetChildFloat("Height");

	m_vDiffuse.Load(oXml, "Diffuse", false);
	m_vAmbient.Load(oXml, "Ambient", false);
	m_vSpecular.Load(oXml, "Specular", false);
	m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess);

	m_strTerrainFile = oXml.GetChildString("TerrainFile", "");
	if(Std_IsBlank(m_strTerrainFile))
		THROW_PARAM_ERROR(Al_Err_lTerrainFileNotDefined, Al_Err_strTerrainFileNotDefined, "Body", m_strName);

	CStdIPoint ptGrid;
	Std_LoadPoint(oXml, "Grid", ptGrid);
	this->Grid(ptGrid);

	oXml.OutOfElem(); //OutOf RigidBody Element

	m_oLocalPosition.Set(m_oLocalPosition.x, m_fltHeight, m_oLocalPosition.z);
	m_oAbsPosition.Set(m_oAbsPosition.x, m_fltHeight, m_oAbsPosition.z);
	m_oRotation.Set(0, 0, 0);
}


/*! \fn float Terrain::Height()
   \brief
   Height property.
      
   \remarks
	 The height of the Terrain on the Y axis.
	 This is the accessor function for the m_fltHeight element.
*/
/*! \fn void Terrain::Height(float fltVal)
   \brief
   Height property.
      
   \remarks
	 The height of the Terrain on the Y axis.
	 This is the mutator function for the m_fltHeight element.
*/


		}		//Bodies
	}			//Environment
}				//AnimatSim