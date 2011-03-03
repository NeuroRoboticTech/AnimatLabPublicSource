// CollisionPair.cpp: implementation of the CollisionPair class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
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
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a CollisionPair object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a CollisionPair. 
*/

ContactSensor::ContactSensor()
{
	m_lpFieldGain = NULL;
	m_lpCurrentGain = NULL;
	m_fltReceptiveFieldDistance = 0.25f;
	m_fltMaxForce = 100;
}

/*! \brief 
   Destroys the CollisionPair object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the CollisionPair object..	 
*/

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

ReceptiveField *ContactSensor::GetReceptiveField(int iIndex)
{
	Std_InValidRange((int) 0, (m_aryFields.GetSize()-1), iIndex, TRUE, "Receptive Field Index");
	return m_aryFields[iIndex];
}

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

BOOL ContactSensor::FindReceptiveField(float fltX, float fltY, float fltZ, int &iIndex)
{
	return FindReceptiveField(m_aryFields, fltX, fltY, fltZ, iIndex);
}

void ContactSensor::AddVertex(float fltX, float fltY, float fltZ)
{
	AddVertex(m_aryFields, fltX, fltY, fltZ);
}

void ContactSensor::FinishedAddingVertices()
{
	//DumpVertices(m_aryFields);
}

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

void ContactSensor::ClearCurrents()
{
	int iSize = m_aryFields.GetSize();
	for(int iField=0; iField<iSize; iField++)
		m_aryFields[iField]->m_fltCurrent = 0;
}

void ContactSensor::ProcessContact(Simulator *lpSim, StdVector3 vPos, float fltForceMagnitude)
{
	if(fltForceMagnitude > m_fltMaxForce)
		fltForceMagnitude = m_fltMaxForce;

	float fltDist, fltFieldGain, fltForce, fltCurrent;
	int iSize = m_aryFields.GetSize();
	ReceptiveField *lpField = NULL;

	for(int iField=0; iField<iSize; iField++)
	{
		lpField = m_aryFields[iField];
		fltDist = V3_DIST(lpField->m_vVertex, vPos) * lpSim->DistanceUnits();
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

	m_lpFieldGain->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL);
	m_lpFieldGain->Load(oXml);

	oXml.IntoChildElement("CurrentGain");
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpCurrentGain = dynamic_cast<AnimatSim::Gains::Gain *>(m_lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpCurrentGain)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "FieldGain");

	m_lpCurrentGain->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL);
	m_lpCurrentGain->Load(oXml);

	if(m_lpCurrentGain->UseLimits())
		m_fltMaxForce = m_lpCurrentGain->UpperLimit();

	m_fltReceptiveFieldDistance = oXml.GetChildFloat("ReceptiveFieldDistance", m_fltReceptiveFieldDistance);
	m_fltReceptiveFieldDistance *= m_lpSim->InverseDistanceUnits(); 

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