/**
\file	LinearHillMuscle.cpp

\brief	Implements the linear hill muscle class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <math.h>
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
#include "Sensor.h"
#include "Attachment.h"
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

#include "ExternalStimulus.h"

#include "LineBase.h"
#include "Gain.h"
#include "SigmoidGain.h"
#include "LengthTensionGain.h"
#include "MuscleBase.h"
#include "LinearHillMuscle.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/10/2011
**/
LinearHillMuscle::LinearHillMuscle()
{
	m_fltKse = 0;
	m_fltKpe = 0;
	m_fltB = 0;
	m_fltKseByB = 0;
	m_fltKpeByKse = 0;
	m_fltMaxTension = 0;
	m_fltDisplacement = 0;
	m_fltDisplacementRatio = 0;
	m_fltVmuscle = 0;
	m_fltVm = (float) -0.15;
	m_fltTL = 0;
	m_fltTLPerc = 0;
	m_fltAct = 0;
	m_fltA = 0;
	m_fltPrevA = 0;
	m_fltTdot = 0;
	m_fltTension = 0;
	m_fltPrevTension = 0;
	m_fltInternalTension = 0;
	m_fltLength = 0;
	m_fltPrevLength = 0;
	m_fltIbDischargeConstant = 100;
	m_fltIbRate = 0;
	m_fltSeDisplacement = 0;

	m_fltSeLPrev = 0;
	m_fltPeLPrev = 0;
	m_fltVse = 0;
	m_fltVpe = 0;

	m_aryMuscleVelocities = NULL;
	m_iMuscleVelAvgCount = 4;
	m_iVelAvgIndex = 0;
	m_fltAvgMuscleVel = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
LinearHillMuscle::~LinearHillMuscle()
{
	try
	{
		if(m_aryMuscleVelocities) 
		{delete[] m_aryMuscleVelocities; m_aryMuscleVelocities = NULL;}

	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of LinearHillMuscle\r\n", "", -1, false, true);}
}

/**
\brief	Gets the serial elastic spring constant.

\author	dcofer
\date	5/20/2011

\return	SE spring constant.
**/
float LinearHillMuscle::Kse() {return m_fltKse;}

/**
\brief	Sets the serial elastic spring constant.

\author	dcofer
\date	5/20/2011

\param	fltVal	The new value.
**/
void LinearHillMuscle::Kse(float fltVal)
{
	Std_InValidRange((float) 0.00001, (float) 1e11, fltVal, true, "Kse");
	m_fltKse = fltVal;
	m_fltKseByB = m_fltKse/m_fltB;
	m_fltKpeByKse = (1 + (m_fltKpe/m_fltKse));
}

/**
\brief	Gets the parallel elastic spring constant.

\author	dcofer
\date	5/20/2011

\return	PE spring constant.
**/
float LinearHillMuscle::Kpe() {return m_fltKpe;}

/**
\brief	Sets the parallel elastic spring constant.

\author	dcofer
\date	5/20/2011

\param	fltVal	The new value.
**/
void LinearHillMuscle::Kpe(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1e11, fltVal, true, "Kpe");
	m_fltKpe = fltVal;
	m_fltKpeByKse = (1 + (m_fltKpe/m_fltKse));
}

/**
\brief	Gets the damping value. 

\author	dcofer
\date	5/20/2011

\return	damping.
**/
float LinearHillMuscle::B() {return m_fltB;}

/**
\brief	Sets the damping value. 

\author	dcofer
\date	5/20/2011

\param	fltVal	The new value.
**/
void LinearHillMuscle::B(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1e11, fltVal, true, "B");
	m_fltB = fltVal;
	m_fltKseByB = m_fltKse/m_fltB;
}

/**
\brief	Gets the resting length.

\author	dcofer
\date	5/20/2011

\return	length.
**/
float LinearHillMuscle::RestingLength() {return m_gainLengthTension.RestingLength();}

/**
\brief	Sets the resting length.

\author	dcofer
\date	5/20/2011

\param	fltVal	The new value.
**/
void LinearHillMuscle::RestingLength(float fltVal) {m_gainLengthTension.RestingLength(fltVal);}

/**
\brief	Gets the ib discharge constant.

\author	dcofer
\date	5/20/2011

\return	discharge constant.
**/
float LinearHillMuscle::IbDischargeConstant() {return m_fltIbDischargeConstant;}

/**
\brief	Sets the ib discharge constant.

\author	dcofer
\date	5/20/2011

\param	fltVal	The new value.
**/
void LinearHillMuscle::IbDischargeConstant(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1e11, fltVal, true, "IbDischargeConstant");
	m_fltIbDischargeConstant = fltVal;
}

/**
\brief	Gets the se length.

\author	dcofer
\date	5/20/2011

\return	length.
**/
float LinearHillMuscle::SeLength() {return m_fltSeLength;}

/**
\brief	Gets the pe length.

\author	dcofer
\date	5/20/2011

\return	length.
**/
float LinearHillMuscle::PeLength() {return m_fltPeLength;}

/**
\brief	Gets the displacement.

\author	dcofer
\date	5/20/2011

\return	length.
**/
float LinearHillMuscle::Displacement() {return m_fltDisplacement;}

/**
\brief	Gets the displacement ratio.

\author	dcofer
\date	5/20/2011

\return	length ratio.
**/
float LinearHillMuscle::DisplacementRatio() {return m_fltDisplacementRatio;}

/**
\brief	Gets the tl.

\author	dcofer
\date	5/20/2011

\return	.
**/
float LinearHillMuscle::TL() {return m_fltTL;}

/**
\brief	Gets the muscle activation value.

\author	dcofer
\date	5/20/2011

\return	activation.
**/
float LinearHillMuscle::Act() {return m_fltAct;}

/**
\brief	Gets activation.

\author	dcofer
\date	5/20/2011

\return	activation.
**/
float LinearHillMuscle::A() {return m_fltA;}

/**
\brief	Gets the internal tension.

\author	dcofer
\date	5/20/2011

\return	tension.
**/
float LinearHillMuscle::InternalTension() {return m_fltInternalTension;}

/**
\brief	Gets the muscle membrane voltage.

\author	dcofer
\date	5/20/2011

\return	voltage.
**/
float LinearHillMuscle::Vmuscle() {return m_fltVmuscle;}

void LinearHillMuscle::Enabled(bool bVal)
{
	MuscleBase::Enabled(bVal);

	if(!bVal)
	{
		m_fltInternalTension = 0;
		m_fltTdot = 0;
		m_fltTension = 0;
	}
}

/**
\brief	Calculates the length tension relationship..

\author	dcofer
\date	5/20/2011

\param	fltL	The length tension ratio.

\return	ratio.
**/
inline float LinearHillMuscle::Ftl(float fltL)
{
	float fltTl = m_gainLengthTension.CalculateGain(fltL);
	if(fltTl<0) fltTl = 0;
	return fltTl;
}

/**
\brief	Calculates the muscle activation.

\author	dcofer
\date	5/20/2011

\param	fltStim	The stimulus level.

\return	activation.
**/
inline float LinearHillMuscle::Fact(float fltStim)
{	
	float fltAct=m_gainStimTension.CalculateGain(fltStim);

	if(fltAct <0)
		fltAct = 0;

	return fltAct;
}

void LinearHillMuscle::ResetSimulation()
{
	MuscleBase::ResetSimulation();

	m_fltDisplacement = 0;
	m_fltDisplacementRatio = 0;
	m_fltVmuscle = 0;
	m_fltPrevTension = 0;
	m_fltTL = 0;
	m_fltAct = 0;
	m_fltA = 0;
	m_fltPrevA = 0;
	m_fltTLPerc = 0;
	m_fltTdot = 0;  
	m_fltInternalTension = 0;
	m_fltTension = 0;

	m_fltSeLPrev = m_fltSeLength;
	m_fltPeLPrev = m_fltPeLength;

	m_fltSeLength = m_gainLengthTension.SeRestLength();
	m_fltPeLength = m_fltLength - m_fltSeLength;
	if(m_fltPeLength<= 0)
		m_fltPeLength = m_gainLengthTension.MinPeLength();

	m_fltSeDisplacement = 0;

	m_fltVse = 0;
	m_fltVpe = 0;

	m_fltVm = (float) -0.15;

	m_fltIbRate = 0;

	m_fltAvgMuscleVel = 0;
	for(int i=0; i<m_iMuscleVelAvgCount; i++)
		m_aryMuscleVelocities[i] = 0;
}

void LinearHillMuscle::AfterResetSimulation() 
{
	MuscleBase::AfterResetSimulation();
	m_fltPeLength = m_fltLength - m_fltSeLength;
}

void LinearHillMuscle::CalculateTension()
{
	int i=0;
    if(m_lpSim->Time() >= 1.5)
		i=6;

	//Store the previous muscle length
	m_fltPrevLength = m_fltLength;

	//Calculate the current muscle length.
	m_fltLength = CalculateLength();

	//Calculate the displacement of this muscle d = (x-x*)
	m_fltDisplacement = m_fltLength-m_gainLengthTension.RestingLength();
	m_fltDisplacementRatio = m_fltLength/m_gainLengthTension.RestingLength();

	//Calculate the instantaneous velocity of change of the muscle length.
	m_fltVmuscle = (m_fltLength-m_fltPrevLength)/m_lpSim->PhysicsTimeStep();

	//Calculate averaged velocity
	m_aryMuscleVelocities[m_iVelAvgIndex] = m_fltVmuscle;
	m_iVelAvgIndex++;
	if(m_iVelAvgIndex >= m_iMuscleVelAvgCount) m_iVelAvgIndex = 0;

	m_fltAvgMuscleVel = 0;
	for(int i=0; i<m_iMuscleVelAvgCount; i++)
		m_fltAvgMuscleVel+= m_aryMuscleVelocities[i];
	m_fltAvgMuscleVel = m_fltAvgMuscleVel/m_iMuscleVelAvgCount;


	//Calculate the active force that is generated by neural stimulation
	m_fltTL = Ftl(m_fltLength);
	m_fltAct = Fact(m_fltVm);
	m_fltA = m_fltTL * m_fltAct;

	m_fltTLPerc = m_fltTL*100;

	//Calculation of the derivitave of the tension
	m_fltTdot = m_fltKseByB*(m_fltKpe*m_fltDisplacement + m_fltB*m_fltVmuscle - m_fltKpeByKse*m_fltInternalTension + m_fltA);  

	//The new tension
	m_fltInternalTension = m_fltInternalTension + m_fltTdot*m_lpSim->PhysicsTimeStep();

	//tension can never be negative, but we want to maintain the "internal" calculations so that the
	//time constants are correct. If you shorten the muscle rapidly it will take it some time to 
	//rebuild its tension. This time will be determined by the internal tension, new length,  and the params like viscosity.
	//But the force seen on the muscle itself will still be 0 N because it is slack.
	//If we did not do this then if your muscle is at rest and is pulled, stays steady, and then relaxed, then
	//it would end up generating a negative tension. (See fig 4 of shadmehr web doc on muscle model).
	//There is a problem in this figure in that you can see that the muscle produced negative tension and this is not 
	//possible in a real muscle.
	if(m_fltInternalTension >= 0)
		m_fltTension = m_fltInternalTension;
	else
		m_fltTension = 0;

	//Make certain that the tension never exceed the absolute maximum set by the user.
	if(m_fltTension > m_fltMaxTension)
		m_fltTension = m_fltMaxTension;

	m_fltSeLPrev = m_fltSeLength;
	m_fltPeLPrev = m_fltPeLength;

	m_fltSeLength = m_gainLengthTension.SeRestLength() + (m_fltTension/m_fltKse);
	m_fltSeDisplacement = m_fltSeLength - m_gainLengthTension.SeRestLength();
	if(m_fltSeDisplacement < 0) m_fltSeDisplacement = 0;

	m_fltPeLength = m_fltLength - m_fltSeLength;
	if(m_fltPeLength < m_gainLengthTension.MinPeLength())
	{
		m_fltSeLength = m_fltLength  - m_gainLengthTension.MinPeLength();
		m_fltPeLength = m_gainLengthTension.MinPeLength();
	}

	m_fltVse = (m_fltSeLength-m_fltSeLPrev)/m_lpSim->PhysicsTimeStep();
	m_fltVpe = (m_fltPeLength-m_fltPeLPrev)/m_lpSim->PhysicsTimeStep();

	m_fltIbRate = m_fltIbDischargeConstant*m_fltTension;

	if(!m_bEnabled)
	{
		m_fltTension = 0;
		m_fltInternalTension = 0;
	}
}

//Calculates the membrane voltage needed for the inverse dynamics of the muscle.
void LinearHillMuscle::CalculateInverseDynamics(float fltLength, float fltVelocity, float fltT, float &fltVm, float &fltA)
{
	//Calculate inverse dynamics force needed
	m_fltPrevA = fltA;
	fltA = fltT - m_fltKpe*(fltLength-m_gainLengthTension.RestingLength()) - m_fltB*fltVelocity + (m_fltKpe/m_fltKse)*fltT;

	if(fltA<0)
		fltA = m_fltPrevA;

	//Calculate tension length percentage
	float fltTl = Ftl(fltLength);

	//Increase A to take Tension-length curve into account.
	fltA = fltA/fltTl;

	//Use A to calculate voltage required.
	if(fltA > 0)
		fltVm = (float) (m_gainStimTension.A() - (1/m_gainStimTension.C())*log((m_gainStimTension.B()-fltA)/fltA));
	else
		fltVm = 0;

	//if(m_lpSim->Time() > 2.092 && m_strName == "Left Tricep Stretch Receptor")
	//	fltVm=fltVm;
}

void LinearHillMuscle::CreateJoints()
{
	MuscleBase::CreateJoints();

	m_fltSeLength = m_gainLengthTension.SeRestLength();
	m_fltPeLength = m_fltLength - m_fltSeLength;
	if(m_fltPeLength<= 0)
		m_fltPeLength = m_gainLengthTension.MinPeLength();

	//Lets create the muscle velocity averaging array.
	if(m_iMuscleVelAvgCount <= 0)
		THROW_TEXT_ERROR(Al_Err_lInvalidMusc_Vel_Avg, Al_Err_strInvalidMusc_Vel_Avg, "Muscle Velocity Average: " + STR(m_iMuscleVelAvgCount));

	m_aryMuscleVelocities = new float[m_iMuscleVelAvgCount];

	//Zero out the average array
	for(int i=0; i<m_iMuscleVelAvgCount; i++)
		m_aryMuscleVelocities[i] = 0;
}

float *LinearHillMuscle::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	float *lpVal = NULL; 

	if(strType == "VMUSCLE")
		lpData = &m_fltVmuscle;
	else if(strType == "VSE")
		lpData = &m_fltVse;
	else if(strType == "VPE")
		lpData = &m_fltVpe;
	else if(strType == "AVGVMUSCLE")
		lpData = &m_fltAvgMuscleVel;
	else if(strType == "DISPLACEMENT")
		lpData = &m_fltDisplacement;
	else if(strType == "DISPLACEMENTRATIO")
		lpData = &m_fltDisplacementRatio;
	else if(strType == "ACTIVATION")
		lpData = &m_fltAct;
	else if(strType == "A")
		lpData = &m_fltA;
	else if(strType == "SELENGTH")
		lpData = &m_fltSeLength;
	else if(strType == "PELENGTH")
		lpData = &m_fltPeLength;
	else if(strType == "SEDISPLACEMENT")
		lpData = &m_fltSeDisplacement;
	else if(strType == "IB")
		lpData = &m_fltIbRate;
	else if(strType == "TL")
		lpData = &m_fltTLPerc;
	else
		lpData = MuscleBase::GetDataPointer(strDataType);

	return lpData;
}

bool LinearHillMuscle::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(MuscleBase::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "KSE")
	{
		Kse((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "KPE")
	{
		Kpe((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "B")
	{
		B((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "IBDISCHARGE")
	{
		IbDischargeConstant((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void LinearHillMuscle::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	MuscleBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Kse");
	aryTypes.Add("Float");

	aryNames.Add("Kpe");
	aryTypes.Add("Float");

	aryNames.Add("B");
	aryTypes.Add("Float");

	aryNames.Add("IbDischarge");
	aryTypes.Add("Float");
}

void LinearHillMuscle::Load(CStdXml &oXml)
{
	MuscleBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	Kse(oXml.GetChildFloat("Kse", m_fltKse));
	Kpe(oXml.GetChildFloat("Kpe", m_fltKpe));
	B(oXml.GetChildFloat("B", m_fltB));
	IbDischargeConstant(oXml.GetChildFloat("IbDischarge", m_fltIbDischargeConstant));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
