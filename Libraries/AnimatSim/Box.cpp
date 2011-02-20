// Box.cpp: implementation of the Box class.
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
#include "Box.h"
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
   Constructs a box object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a box. 
*/

Box::Box()
{
	m_fltLength = 1;
	m_fltWidth = 1;
	m_fltHeight = 1;
}

/*! \brief 
   Destroys the box object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the box object..	 
*/

Box::~Box()
{
}

void Box::Length(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BoxSize.Length");
	if(bUseScaling)
		m_fltLength = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltLength = fltVal;

	Resize();
}

void Box::Width(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BoxSize.Width");
	if(bUseScaling)
		m_fltWidth = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltWidth = fltVal;

	Resize();
}

void Box::Height(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BoxSize.Height");
	if(bUseScaling)
		m_fltHeight = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltHeight = fltVal;

	Resize();
}

BOOL Box::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "LENGTH")
	{
		Length(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "WIDTH")
	{
		Width(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "HEIGHT")
	{
		Height(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Box::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	RigidBody::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element

	Length(oXml.GetChildFloat("Length", m_fltLength));
	Width(oXml.GetChildFloat("Width", m_fltWidth));
	Height(oXml.GetChildFloat("Height", m_fltHeight));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

/*! \fn CStdFPoint CAlBox::CollisionBoxSize()
   \brief
   CollisionBoxSize property.
      
   \remarks
   The dimension vector for the collision box.
	 This is the accessor function for the m_oCollisionBoxSize element.
*/
/*! \fn void CAlBox::CollisionBoxSize(CStdFPoint &oPoint)
   \brief
   CollisionBoxSize property.
      
   \remarks
   The dimension vector for the collision box.
	 This is the mutator function for the m_oCollisionBoxSize element.
*/


/*! \fn CStdFPoint CAlBox::GraphicsBoxSize()
   \brief
   GraphicsBoxSize property.
      
   \remarks
	 The dimension vector for the graphic box.
	 This is the accessor function for the m_oGraphicsBoxSize element.
*/
/*! \fn void CAlBox::GraphicsBoxSize(CStdFPoint &oPoint)
   \brief
   GraphicsBoxSize property.
      
   \remarks
	 The dimension vector for the graphic box.
	 This is the mutator function for the m_oGraphicsBoxSize element.
*/


		}		//Bodies
	}			//Environment
}				//AnimatSim
