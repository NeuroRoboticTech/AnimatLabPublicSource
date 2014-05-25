#include "StdAfx.h"
#include "ScriptProcessorPy.h"
#include "PyEmbedder.h"

#if defined(_DEBUG) && defined(SWIG_PYTHON_INTERPRETER_NO_DEBUG)
/* Use debug wrappers with the Python release dll */
# undef _DEBUG
# include <Python.h>
# define _DEBUG
#else
# include <Python.h>
#endif

namespace AnimatSimPy
{

ScriptProcessorPy::ScriptProcessorPy(void)
{
}

ScriptProcessorPy::~ScriptProcessorPy(void)
{
try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ScriptProcessorPy\r\n", "", -1, false, true);}
}

void ScriptProcessorPy::InitPy(std::string strVal) {m_strInitPy = strVal;}

std::string ScriptProcessorPy::InitPy() {return m_strInitPy;}

void ScriptProcessorPy::ResetSimPy(std::string strVal) {m_strResetSimPy = strVal;}

std::string ScriptProcessorPy::ResetSimPy() {return m_strResetSimPy;}

void ScriptProcessorPy::StepPhysicsEnginePy(std::string strVal) {m_strStepPhysicsEnginePy = strVal;}

std::string ScriptProcessorPy::StepPhysicsEnginePy() {return m_strStepPhysicsEnginePy;}

void ScriptProcessorPy::StepNeuralEnginePy(std::string strVal) {m_strStepNeuralEnginePy = strVal;}

std::string ScriptProcessorPy::StepNeuralEnginePy() {return m_strStepNeuralEnginePy;}

void ScriptProcessorPy::StepSimulationPy(std::string strVal) {m_strStepSimulationPy = strVal;}

std::string ScriptProcessorPy::StepSimulationPy() {return m_strStepSimulationPy;}

void ScriptProcessorPy::KillPy(std::string strVal) {m_strKillPy = strVal;}

std::string ScriptProcessorPy::KillPy() {return m_strKillPy;}

void ScriptProcessorPy::SimStartingPy(std::string strVal) {m_strSimStartingPy = strVal;}

std::string ScriptProcessorPy::SimStartingPy() {return m_strSimStartingPy;}

void ScriptProcessorPy::SimPausingPy(std::string strVal) {m_strSimPausingPy = strVal;}

std::string ScriptProcessorPy::SimPausingPy() {return m_strSimPausingPy;}

void ScriptProcessorPy::SimStoppingPy(std::string strVal) {m_strSimStoppingPy = strVal;}

std::string ScriptProcessorPy::SimStoppingPy() {return m_strSimStoppingPy;}

void ScriptProcessorPy::ExecutePythonScript(const std::string &strPy)
{
}

void ScriptProcessorPy::Initialize()
{
	ExecutePythonScript(m_strInitPy);
}

void ScriptProcessorPy::ResetSimulation()
{
	ExecutePythonScript(m_strResetSimPy);
}

void ScriptProcessorPy::StepPhysicsEngine()
{
	ExecutePythonScript(m_strStepPhysicsEnginePy);
}

void ScriptProcessorPy::StepNeuralEngine()
{
	ExecutePythonScript(m_strStepNeuralEnginePy);
}

void ScriptProcessorPy::StepSimulation()
{
	ExecutePythonScript(m_strStepSimulationPy);
}

void ScriptProcessorPy::Kill(bool bState)
{
	ExecutePythonScript(m_strKillPy);
}

void ScriptProcessorPy::SimStarting()
{
	ExecutePythonScript(m_strSimStartingPy);
}

void ScriptProcessorPy::SimPausing()
{
	ExecutePythonScript(m_strSimPausingPy);
}

void ScriptProcessorPy::SimStopping()
{
	ExecutePythonScript(m_strSimStoppingPy);
}

bool ScriptProcessorPy::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;
	/*
	if(strType == "GAIN")
	{
		AddGain(strValue);
		return true;
	}

	if(strType == "ORIGINID")
	{
		SetOriginID(strValue);
		return true;
	}

	if(strType == "DESTINATIONID")
	{
		SetDestinationID(strValue);
		return true;
	}

	if(strType == "DELAYBUFFERMODE")
	{
		DelayBufferMode((eDelayBufferMode) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "DELAYBUFFERINTERVAL")
	{
		DelayBufferInterval(atof(strValue.c_str()));
		return true;
	}

	if(strType == "ROBOTIOSCALE")
	{
		RobotIOScale(atof(strValue.c_str()));
		return true;
	}
*/
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Data Type", strDataType);

	return false;
}

void ScriptProcessorPy::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);
/*
	aryProperties.Add(new TypeProperty("Enable", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("CalculatedVal", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("NextVal", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("DelayBufferMode", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("DelayBufferInterval", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RobotIOScale", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	*/
}

void ScriptProcessorPy::Load(StdUtils::CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	/*
	oXml.IntoElem();  //Into Adapter Element

	//Load Source Data
	SourceModule(oXml.GetChildString("SourceModule"));
	SourceID(oXml.GetChildString("SourceID"));
	SourceDataType(oXml.GetChildString("SourceDataType"));

	//Load Target Data
	TargetModule(oXml.GetChildString("TargetModule"));
	TargetID(oXml.GetChildString("TargetID"));

	SetGain(LoadGain(m_lpSim, "Gain", oXml));
	
	Enabled(oXml.GetChildBool("Enabled"));

	DelayBufferMode((eDelayBufferMode) oXml.GetChildInt("DelayBufferMode", m_eDelayBufferMode));
	DelayBufferInterval(oXml.GetChildFloat("DelayBufferInterval", m_fltDelayBufferInterval));

	RobotIOScale(oXml.GetChildFloat("RobotIOScale", m_fltRobotIOScale));

	oXml.OutOfElem(); //OutOf Adapter Element
	*/
}


}