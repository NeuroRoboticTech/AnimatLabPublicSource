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

void ScriptProcessorPy::BeforeStepPhysicsEnginePy(std::string strVal) {m_strBeforeStepPhysicsEnginePy = strVal;}

std::string ScriptProcessorPy::BeforeStepPhysicsEnginePy() {return m_strBeforeStepPhysicsEnginePy;}

void ScriptProcessorPy::AfterStepPhysicsEnginePy(std::string strVal) {m_strAfterStepPhysicsEnginePy = strVal;}

std::string ScriptProcessorPy::AfterStepPhysicsEnginePy() {return m_strAfterStepPhysicsEnginePy;}

void ScriptProcessorPy::BeforeStepNeuralEnginePy(std::string strVal) {m_strBeforeStepNeuralEnginePy = strVal;}

std::string ScriptProcessorPy::BeforeStepNeuralEnginePy() {return m_strBeforeStepNeuralEnginePy;}

void ScriptProcessorPy::AfterStepNeuralEnginePy(std::string strVal) {m_strAfterStepNeuralEnginePy = strVal;}

std::string ScriptProcessorPy::AfterStepNeuralEnginePy() {return m_strAfterStepNeuralEnginePy;}

void ScriptProcessorPy::KillPy(std::string strVal) {m_strKillPy = strVal;}

std::string ScriptProcessorPy::KillPy() {return m_strKillPy;}

void ScriptProcessorPy::SimStartingPy(std::string strVal) {m_strSimStartingPy = strVal;}

std::string ScriptProcessorPy::SimStartingPy() {return m_strSimStartingPy;}

void ScriptProcessorPy::SimPausingPy(std::string strVal) {m_strSimPausingPy = strVal;}

std::string ScriptProcessorPy::SimPausingPy() {return m_strSimPausingPy;}

void ScriptProcessorPy::SimStoppingPy(std::string strVal) {m_strSimStoppingPy = strVal;}

std::string ScriptProcessorPy::SimStoppingPy() {return m_strSimStoppingPy;}

bool ScriptProcessorPy::ExecutePythonScript(const std::string &strPy, bool bThrowError)
{
	//Create a static python embedder to init python. Using static to ensure that this is only done once per process.
	static PyEmbedder g_pyEmbedder;
	if(m_bEnabled)
		return g_pyEmbedder.ExecutePythonScript(strPy, bThrowError);
	else
		return true;
}

void ScriptProcessorPy::Initialize()
{
	ScriptProcessor::Initialize();

	ExecutePythonScript(m_strInitPy);
}

void ScriptProcessorPy::ResetSimulation()
{
	ScriptProcessor::ResetSimulation();
	ExecutePythonScript(m_strResetSimPy);
}

void ScriptProcessorPy::BeforeStepPhysicsEngine()
{
	ScriptProcessor::BeforeStepPhysicsEngine();
	ExecutePythonScript(m_strBeforeStepPhysicsEnginePy);
}

void ScriptProcessorPy::AfterStepPhysicsEngine()
{
	ScriptProcessor::AfterStepPhysicsEngine();
	ExecutePythonScript(m_strAfterStepPhysicsEnginePy);
}

void ScriptProcessorPy::BeforeStepNeuralEngine()
{
	ScriptProcessor::BeforeStepNeuralEngine();
	ExecutePythonScript(m_strBeforeStepNeuralEnginePy);
}

void ScriptProcessorPy::AfterStepNeuralEngine()
{
	ScriptProcessor::AfterStepNeuralEngine();
	ExecutePythonScript(m_strAfterStepNeuralEnginePy);
}

void ScriptProcessorPy::Kill(bool bState)
{
	ScriptProcessor::Kill();
	ExecutePythonScript(m_strKillPy);
}

void ScriptProcessorPy::SimStarting()
{
	ScriptProcessor::SimStarting();
	ExecutePythonScript(m_strSimStartingPy);
}

void ScriptProcessorPy::SimPausing()
{
	ScriptProcessor::SimPausing();
	ExecutePythonScript(m_strSimPausingPy);
}

void ScriptProcessorPy::SimStopping()
{
	ScriptProcessor::SimStopping();
	ExecutePythonScript(m_strSimStoppingPy);
}

bool ScriptProcessorPy::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(ScriptProcessor::SetData(strDataType, strValue, false))
		return true;

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}

	if(strType == "RESETSIMPY")
	{
		ResetSimPy(strValue);
		return true;
	}

	if(strType == "BEFORESTEPPHYSICSENGINEPY")
	{
		BeforeStepPhysicsEnginePy(strValue);
		return true;
	}

	if(strType == "AFTERSTEPPHYSICSENGINEPY")
	{
		AfterStepPhysicsEnginePy(strValue);
		return true;
	}

	if(strType == "BEFORESTEPNEURALENGINEPY")
	{
		BeforeStepNeuralEnginePy(strValue);
		return true;
	}

	if(strType == "AFTERSTEPNEURALENGINEPY")
	{
		AfterStepNeuralEnginePy(strValue);
		return true;
	}

	if(strType == "KILLPY")
	{
		KillPy(strValue);
		return true;
	}

	if(strType == "SIMSTARTINGPY")
	{
		SimStartingPy(strValue);
		return true;
	}

	if(strType == "SIMPAUSINGPY")
	{
		SimPausingPy(strValue);
		return true;
	}

	if(strType == "SIMSTOPPINGPY")
	{
		SimStoppingPy(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Data Type", strDataType);

	return false;
}

void ScriptProcessorPy::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	ScriptProcessor::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Enabled", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("InitPy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("ResetSimPy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("BeforeStepPhysicsEnginePy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AfterStepPhysicsEnginePy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("BeforeStepNeuralEnginePy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AfterStepNeuralEnginePy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("KillPy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SimStartingPy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SimPausingPy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SimStoppingPy", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

void ScriptProcessorPy::Load(StdUtils::CStdXml &oXml)
{
	ScriptProcessor::Load(oXml);

	oXml.IntoElem();  //Into Script Element

	Enabled(oXml.GetChildBool("Enabled", m_bEnabled));
	InitPy(oXml.GetChildString("InitPy", m_strInitPy));
	ResetSimPy(oXml.GetChildString("ResetSimPy", m_strResetSimPy));
	BeforeStepPhysicsEnginePy(oXml.GetChildString("BeforeStepPhysicsEnginePy", m_strBeforeStepPhysicsEnginePy));
	AfterStepPhysicsEnginePy(oXml.GetChildString("AfterStepPhysicsEnginePy", m_strAfterStepPhysicsEnginePy));
	BeforeStepNeuralEnginePy(oXml.GetChildString("BeforeStepNeuralEnginePy", m_strBeforeStepNeuralEnginePy));
	AfterStepNeuralEnginePy(oXml.GetChildString("AfterStepNeuralEnginePy", m_strAfterStepNeuralEnginePy));
	KillPy(oXml.GetChildString("KillPy", m_strKillPy));
	SimStartingPy(oXml.GetChildString("SimStartingPy", m_strSimStartingPy));
	SimPausingPy(oXml.GetChildString("SimPausingPy", m_strSimPausingPy));
	SimStoppingPy(oXml.GetChildString("SimStoppingPy", m_strSimStoppingPy));

	oXml.OutOfElem(); //OutOf Script Element
}


}