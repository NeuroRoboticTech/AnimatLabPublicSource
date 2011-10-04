/**
\file	ContactSensor.cpp

\brief	Implements the contact sensor class.
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
#include "Gain.h"
#include "BellGain.h"   //Need to remove later.
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
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/22/2011
**/
ContactSensor::ContactSensor()
{
	m_lpFieldGain = NULL;
	m_lpCurrentGain = NULL;
	m_fltMaxForce = 100;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/22/2011
**/
ContactSensor::~ContactSensor()
{

try
{
	if(m_lpFieldGain)
		delete m_lpFieldGain;

	if(m_lpCurrentGain)
		delete m_lpCurrentGain;

	m_aryFields.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ContactSensor\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the field Gain that modulates the contact force based on its distance from the center of the receptive field.

\author	dcofer
\date	3/22/2011

\return	Gain pointer.
**/
Gain *ContactSensor::FieldGain() {return m_lpFieldGain;}

/**
\brief	Gets the Gain that calculates the current to apply to associated neurons using the modulated force of contact.

\author	dcofer
\date	3/22/2011

\return	Gain pointer.
**/
Gain *ContactSensor::CurrentGain() {return m_lpCurrentGain;}

/**
\brief	Gets a receptive field based on its index in the array.

\author	dcofer
\date	3/22/2011

\param	iIndex	Zero-based index of the ReceptiveField array. 

\return	Pointer to the receptive field.
\exception Exception is thrown if index is out of bounds.
**/
ReceptiveField *ContactSensor::GetReceptiveField(int iIndex)
{
	Std_InValidRange((int) 0, (m_aryFields.GetSize()-1), iIndex, TRUE, "Receptive Field Index");
	return m_aryFields[iIndex];
}

/**
\brief	Searches for the first receptive field that is closes to the specified location.

\author	dcofer
\date	3/22/2011

\param [in,out]	aryFields	The array of ReceptiveFields to search. 
\param	fltX			 	The x coordinate. 
\param	fltY			 	The y coordinate. 
\param	fltZ			 	The z coordinate. 
\param [in,out]	iIndex   	Zero-based index of the found ReceptiveField. 

\return	true if a ReceptiveField is found, false otherwise.
**/
BOOL ContactSensor::FindReceptiveField(CStdPtrArray<ReceptiveField> &aryFields, float fltX, float fltY, float fltZ, int &iIndex)
{
	int high = aryFields.GetSize(), low = -1, probe=0;

	while (high - low > 1)
	{
		probe = (high + low) / 2;
		if (aryFields[probe]->GreaterThanThan(fltX, fltY, fltZ))
			high = probe;
		else
			low = probe;
	}

	if(low==-1)
		iIndex = 0;
	else
		iIndex = high;

	if((probe < aryFields.GetSize() && aryFields[probe]->Equals(fltX, fltY, fltZ))
		|| (low > -1 && low < aryFields.GetSize() && aryFields[low]->Equals(fltX, fltY, fltZ))
		|| (high < aryFields.GetSize() && aryFields[high]->Equals(fltX, fltY, fltZ)))
		return TRUE;
	else
		return FALSE;
}

/**
\brief	Searches for the first closest receptive field.

\author	dcofer
\date	3/22/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 

\return	The found closest receptive field.
**/
int ContactSensor::FindClosestReceptiveField(float fltX, float fltY, float fltZ)
{
	int iSize = m_aryFields.GetSize(), iMinIndex = -1;
	float fltDist = 0, fltMinDist = 0;
	float fltXMin, fltYMin, fltZMin;

	ReceptiveField *lpField = NULL;
	for(int iIndex=0; iIndex<iSize; iIndex++)
	{
		lpField = m_aryFields[iIndex];

		fltDist = sqrt( pow((fltX-lpField->m_vVertex[0]), 2) + pow((fltY-lpField->m_vVertex[1]), 2) + pow((fltZ-lpField->m_vVertex[2]), 2) );
		
		if(iMinIndex<0 || fltDist < fltMinDist)
		{
			fltMinDist = fltDist;
			iMinIndex = iIndex;
			fltXMin = lpField->m_vVertex[0]; fltYMin = lpField->m_vVertex[1]; fltZMin = lpField->m_vVertex[2]; 
		}
	}

	return iMinIndex;
}

/**
\brief	Adds a new ReceptiveField vertex.

\author	dcofer
\date	3/22/2011

\param [in,out]	aryFields	The array of ReceptiveFields we will add to. 
\param	fltX			 	The x coordinate. 
\param	fltY			 	The y coordinate. 
\param	fltZ			 	The z coordinate. 
**/
void ContactSensor::AddVertex(CStdPtrArray<ReceptiveField> &aryFields, float fltX, float fltY, float fltZ)
{
	int iIndex=-1, iVerifyIndex=-1;

	if(!FindReceptiveField(aryFields, fltX, fltY, fltZ, iIndex))
	{
		ReceptiveField *lpField = new ReceptiveField(fltX, fltY, fltZ, 0);

		if(iIndex == -1)
			aryFields.Add(lpField);
		else
			aryFields.InsertAt(iIndex, lpField);

		//TRACE_STL_CONTAINER(aryFields);
	}
}

/**
\brief	Searches for the first receptive field at the specified location.

\author	dcofer
\date	3/22/2011

\param	fltX		  	The x coordinate. 
\param	fltY		  	The y coordinate. 
\param	fltZ		  	The  z coordinate. 
\param [in,out]	iIndex	Zero-based index of the found receptive field. 

\return	true if it succeeds, false if it fails.
**/
BOOL ContactSensor::FindReceptiveField(float fltX, float fltY, float fltZ, int &iIndex)
{
	return FindReceptiveField(m_aryFields, fltX, fltY, fltZ, iIndex);
}

/**
\brief	Adds a ReceptiveField vertex.

\author	dcofer
\date	3/22/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void ContactSensor::AddVertex(float fltX, float fltY, float fltZ)
{
	AddVertex(m_aryFields, fltX, fltY, fltZ);
}

/**
\brief	Called when we are finished adding vertices. This is used for debugging purposes.

\author	dcofer
\date	3/22/2011
**/
void ContactSensor::FinishedAddingVertices()
{
	//DumpVertices(m_aryFields);
}

/**
\brief	Dumps the vertices to console.

\author	dcofer
\date	3/22/2011

\param [in,out]	aryFields	The array of ReceptiveFields. 
**/
void ContactSensor::DumpVertices(CStdPtrArray<ReceptiveField> &aryFields)
{
	int iSize = aryFields.GetSize();
	for(int iV=0; iV<iSize; iV++)
	{
		std::ostringstream oss;
		aryFields[iV]->Trace(oss);
		//oss << "\r\n";
		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);
	}
}

/**
\brief	Clears the currents of ReceptiveFields.

\author	dcofer
\date	3/22/2011
**/
void ContactSensor::ClearCurrents()
{
	int iSize = m_aryFields.GetSize();
	for(int iField=0; iField<iSize; iField++)
		m_aryFields[iField]->m_fltCurrent = 0;
}

/**
\brief	Process a contact for all ReceptiveFields.

\details Process contact of the ContactSensor detects the collisions and calculates
the amount of current that needs to be added to a given receptive field. It is the 
contact adapter that actually 

\author	dcofer
\date	3/22/2011

\param	vPos			 	The position of the contact in global cooridnates. 
\param	fltForceMagnitude	The force magnitude. 
**/
void ContactSensor::ProcessContact(StdVector3 vPos, float fltForceMagnitude)
{
	if(fltForceMagnitude > m_fltMaxForce)
		fltForceMagnitude = m_fltMaxForce;

	float fltDist, fltFieldGain, fltForce, fltCurrent;
	int iSize = m_aryFields.GetSize();
	ReceptiveField *lpField = NULL;

	for(int iField=0; iField<iSize; iField++)
	{
		lpField = m_aryFields[iField];
		fltDist = V3_DIST(lpField->m_vVertex, vPos) * m_lpSim->DistanceUnits();
		fltFieldGain = m_lpFieldGain->CalculateGain(fltDist);
		fltForce = fltForceMagnitude*fltFieldGain;
		fltCurrent = m_lpCurrentGain->CalculateGain(fltForce);

		lpField->m_fltCurrent += fltCurrent;

		//if(fltCurrent > 1e-9)
		//	fltCurrent = fltCurrent;
	}
}

void ContactSensor::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	oXml.IntoChildElement("FieldGain");
	string strModuleName = oXml.GetChildString("ModuleName", "");
	string strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpFieldGain = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpFieldGain)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "FieldGain");

	m_lpFieldGain->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, TRUE);
	m_lpFieldGain->Load(oXml);

	oXml.IntoChildElement("CurrentGain");
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpCurrentGain = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpCurrentGain)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "FieldGain");

	m_lpCurrentGain->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, TRUE);
	m_lpCurrentGain->Load(oXml);

	if(m_lpCurrentGain->UseLimits())
		m_fltMaxForce = m_lpCurrentGain->UpperLimit();

	oXml.FindChildElement("FieldPairs");
	oXml.IntoElem(); //Into FieldPairs Element
	int iCount = oXml.NumberOfChildren();

	m_aryFields.RemoveAll();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		oXml.FindChildByIndex(iIndex);
		LoadReceptiveField(oXml);
	}
	oXml.OutOfElem(); //OutOf FieldPairs Element

	//DumpVertices(m_aryPairs);

	oXml.OutOfElem(); //OutOf Adapter Element
}

/**
\brief	Loads a receptive field.

\author	dcofer
\date	3/22/2011

\param [in,out]	oXml	The xml packet to load. 
**/
void ContactSensor::LoadReceptiveField(CStdXml &oXml)
{
	CStdFPoint vPoint;

	oXml.IntoElem();
	Std_LoadPoint(oXml, "Vertex", vPoint);
	oXml.OutOfElem(); //OutOf FieldPairs Element

	AddVertex(m_aryFields, vPoint.x, vPoint.y, vPoint.z); 
}

	}			//Environment
}				//AnimatSim