// ReceptiveFieldPair.cpp: implementation of the ReceptiveFieldPair class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ReceptiveFieldPair.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sensor.h"
#include "Attachment.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
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

ReceptiveFieldPair::ReceptiveFieldPair()
{
	m_vVertex[0] = 0; m_vVertex[1] = 0; m_vVertex[2] = 0;
	m_lpTargetNode = NULL;
	m_lpField = NULL;
}


/*! \brief 
   Destroys the CollisionPair object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the CollisionPair object..	 
*/

ReceptiveFieldPair::~ReceptiveFieldPair()
{
}

void ReceptiveFieldPair::Initialize(string strTargetModule)
{
	AnimatBase::Initialize();

	m_lpTargetNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpTargetNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	RigidBody *lpBody = dynamic_cast<RigidBody *>(m_lpNode);

	if(!lpBody)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "SourceNode to RigidBody");

	ContactSensor *lpSensor = lpBody->ContactSensor();

	//I wanted to make this get the closest vertex that matched this one. However, we only save vertices in the list in the sensor
	//if they have been saved in the file as a pair already. So if there is a mismatch of some kind then the pair in the file might 
	//not match any of the generated vertices closely enough to be saved. So there is no real reason to try and find the closest matching
	//vertex here. In fact it would be misleading because the closest matching one may be far away from this vertex. Also, if no error 
	//is reported then we would not know there was a problem.
	int iIndex = 0;
	if(!lpSensor->FindReceptiveField(m_vVertex[0], m_vVertex[1], m_vVertex[2], iIndex))
		THROW_TEXT_ERROR(Al_Err_lReceptiveFieldVertexNotFound, Al_Err_strReceptiveFieldVertexNotFound, "BodyID: " + lpBody->Name() + "  Vertex: (" + STR(m_vVertex[0]) + ", " + STR(m_vVertex[1]) + ", " + STR(m_vVertex[2]) + ")");

	m_lpField = lpSensor->GetReceptiveField(iIndex);
}

void ReceptiveFieldPair::StepSimulation()
{
	if(m_lpField)
		m_lpTargetNode->AddExternalNodeInput(m_lpField->m_fltCurrent);
}

//Node Overrides

void ReceptiveFieldPair::Load(CStdXml &oXml)
{
	oXml.IntoElem();  //Into Adapter Element

	CStdFPoint vPoint;
	Std_LoadPoint(oXml, "Vertex", vPoint);
	m_vVertex[0] = vPoint.x; m_vVertex[1] = vPoint.y; m_vVertex[2] = vPoint.z;
	m_strTargetNodeID = oXml.GetChildString("TargetNodeID");
	if(Std_IsBlank(m_strTargetNodeID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Environment
}				//AnimatSim