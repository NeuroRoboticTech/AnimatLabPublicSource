// OdorType.cpp: implementation of the OdorType class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatSim
{
	namespace Environment
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

OdorType::OdorType()
{
	m_fltDiffusionConstant = 1;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

OdorType::~OdorType()
{

try
{
	m_aryOdorSources.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Odor\r\n", "", -1, FALSE, TRUE);}
}

Odor *OdorType::FindOdorSource(string strOdorID, BOOL bThrowError)
{
	Odor *lpOdor = NULL;
	CStdMap<string, Odor *>::iterator oPos;
	oPos = m_aryOdorSources.find(Std_CheckString(strOdorID));

	if(oPos != m_aryOdorSources.end())
		lpOdor =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lOdorIDNotFound, Al_Err_strOdorIDNotFound, "OdorID", strOdorID);

	return lpOdor;
}

void OdorType::AddOdorSource(Odor *lpOdor)
{
	if(!FindOdorSource(lpOdor->ID(), FALSE))
	{
		m_aryOdorSources.Add(lpOdor->ID(), lpOdor);
	}
}

float OdorType::CalculateOdorValue(CStdFPoint &oSensorPos)
{
	//Loop through each of the associated odors and calculate the value for each one.
	CStdMap<string, Odor *>::iterator oIterator;
	Odor *lpOdor = NULL;
	float fltVal = 0;

	for(oIterator=m_aryOdorSources.begin(); 
	    oIterator!=m_aryOdorSources.end(); 
			++oIterator)
	{
		lpOdor = oIterator->second;
		fltVal += lpOdor->CalculateOdorValue(this, oSensorPos);
	}

	return fltVal;
}

/*! \brief 
   Loads a joint from an xml configuration file.
      
   \param lpSim This is a pointer to the simulator.
   \param lpStructure This is a pointer to the structure/Organism that
                      this rigid body is a part of.
   \param oXml This is an xml object.

	 \return
	 No return value.

	 \remarks
	 This method is responsible for loading the joint from a XMl
	 configuration file. You should call this method even in your 
	 overriden function becuase it loads all of the base properties
	 for the Joint. 
*/

void OdorType::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Joint Element

	m_fltDiffusionConstant = oXml.GetChildFloat("DiffusionConstant", m_fltDiffusionConstant);
	m_fltDiffusionConstant = m_fltDiffusionConstant/(m_lpSim->DistanceUnits()*m_lpSim->DistanceUnits()); //Our diffusion constant is in m^2/s. We need to convert its distance units appropriately.
	Std_IsAboveMin((float) 0, m_fltDiffusionConstant, TRUE, "Diffusion Constant");

	oXml.OutOfElem(); //OutOf Joint Element
}

	}			// Environment
}			//AnimatSim
