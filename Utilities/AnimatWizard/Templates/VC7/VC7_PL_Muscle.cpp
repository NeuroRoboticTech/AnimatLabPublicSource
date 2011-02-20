// [*MUSCLE_NAME*].cpp: implementation of the [*MUSCLE_NAME*] class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "[*MUSCLE_NAME*].h"

namespace Test
{
	namespace Bodies
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


[*MUSCLE_NAME*]::[*MUSCLE_NAME*]()
{
}

[*MUSCLE_NAME*]::~[*MUSCLE_NAME*]()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of [*MUSCLE_NAME*]\r\n", "", -1, FALSE, TRUE);}
}

void [*MUSCLE_NAME*]::CalculateTension(Simulator *lpSim)
{
	//Store the previous muscle length
	m_fltPrevMuscleLength = m_fltMuscleLength;

	//Calculate the current muscle length.
	m_fltMuscleLength = CalculateMuscleLength(lpSim);

	m_fltPrevTension = m_fltTension;
}

//Calculates the membrane voltage needed for the inverse dynamics of the muscle.
void [*MUSCLE_NAME*]::CalculateInverseDynamics(Simulator *lpSim, float fltLength, float fltVelocity, float fltT, float &fltVm, float &fltA)
{
}

float *[*MUSCLE_NAME*]::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "TENSION")
		lpData = &m_fltTension;
	else if(strType == "TDOT")
		lpData = &m_fltTdot;
	else if(strType == "MUSCLELENGTH")
		lpData = &m_fltMuscleLength;
	else if(strType == "MEMBRANEVOLTAGE")
		lpData = &m_fltVm;
	else if(strType == "ENABLE")
		lpData = &m_fltEnabled;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
}

void [*MUSCLE_NAME*]::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	VsMuscleBase::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into RigidBody Element

	oXml.OutOfElem(); //OutOf RigidBody Element
}

	}		//Bodies
}				//Test
