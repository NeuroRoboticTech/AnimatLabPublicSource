// Adapter.cpp: implementation of the Adapter class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "NervousSystem.h"
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
	namespace Adapters
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

Adapter::Adapter()
{
	m_lpGain = NULL;
	m_lpSourceNode = NULL;
	m_lpSourceData = NULL;
	m_lpTargetNode = NULL;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

Adapter::~Adapter()
{

try
{
	m_lpSourceNode = NULL;
	m_lpSourceData = NULL;
	m_lpTargetNode = NULL;
	if(m_lpGain) delete m_lpGain;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Adapter\r\n", "", -1, FALSE, TRUE);}
}


//Node Overrides
void Adapter::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput)
{
	THROW_TEXT_ERROR(Al_Err_lOpNotDefinedForAdapter, Al_Err_strOpNotDefinedForAdapter, "AddExternalNodeInput");
}

void Adapter::AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Node *lpNode)
{
	THROW_TEXT_ERROR(Al_Err_lOpNotDefinedForAdapter, Al_Err_strOpNotDefinedForAdapter, "AttachSourceAdapter");
}

void Adapter::AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Node *lpNode)
{
	THROW_TEXT_ERROR(Al_Err_lOpNotDefinedForAdapter, Al_Err_strOpNotDefinedForAdapter, "AttachTargetAdapter");
}

float *Adapter::GetDataPointer(string strDataType)
{
	THROW_TEXT_ERROR(Al_Err_lOpNotDefinedForAdapter, Al_Err_strOpNotDefinedForAdapter, "GetDataPointer");
	return 0;
}

void Adapter::Initialize(Simulator *lpSim, Structure *lpStructure)
{
	m_lpSourceNode = dynamic_cast<Node *>(lpSim->FindByID(m_strSourceID));
	if(!m_lpSourceNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strSourceID);

	m_lpSourceData = m_lpSourceNode->GetDataPointer(m_strSourceDataType);

	if(!m_lpSourceData)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Adapter: " + m_strID + " StructureID: " + lpStructure->ID() + "SourceID: " + m_strSourceID + " DataType: " + m_strSourceDataType));

	m_lpTargetNode = dynamic_cast<Node *>(lpSim->FindByID(m_strTargetID));
	if(!m_lpTargetNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetID);

	m_lpSourceNode->AttachSourceAdapter(lpSim, lpStructure, this);
	m_lpTargetNode->AttachTargetAdapter(lpSim, lpStructure, this);
}

void Adapter::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	m_lpTargetNode->AddExternalNodeInput(lpSim, lpStructure, m_lpGain->CalculateGain(*m_lpSourceData));
}

void Adapter::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Node::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into Adapter Element

	//Load Source Data
	m_strSourceModule = oXml.GetChildString("SourceModule");
	if(Std_IsBlank(m_strSourceModule)) 
		THROW_TEXT_ERROR(Al_Err_lModuleNameBlank, Al_Err_strModuleNameBlank, ". Source Module. ID: " + m_strID);

	m_strSourceID = oXml.GetChildString("SourceID");
	if(Std_IsBlank(m_strSourceID)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Source ID");

	m_strSourceDataType = oXml.GetChildString("SourceDataType");
	if(Std_IsBlank(m_strSourceDataType)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Source DataType");

	m_strTargetModule = oXml.GetChildString("TargetModule");
	if(Std_IsBlank(m_strTargetModule)) 
		THROW_TEXT_ERROR(Al_Err_lModuleNameBlank, Al_Err_strModuleNameBlank, ". Target Module. ID: " + m_strID);

	m_strTargetID = oXml.GetChildString("TargetID");
	if(Std_IsBlank(m_strTargetID)) 
		THROW_TEXT_ERROR(Al_Err_lDataTypeBlank, Al_Err_strDataTypeBlank, " Target ID");

	//Now lets load this gain object.
	oXml.IntoChildElement("Gain");
	string strModuleName = oXml.GetChildString("ModuleName", "");
	string strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Gain Element

	m_lpGain = dynamic_cast<AnimatSim::Gains::Gain *>(lpSim->CreateObject(strModuleName, "Gain", strType));
	if(!m_lpGain)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Gain");

	m_lpGain->Load(oXml);

	oXml.OutOfElem(); //OutOf Adapter Element
}

void Adapter::Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
}

	}			//Adapters
}			//AnimatSim
