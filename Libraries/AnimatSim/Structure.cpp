/**
\file	Structure.cpp

\brief	Implements the structure class. 
**/

#include "stdafx.h"
#include "IMotorizedJoint.h"
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
#include "MotorizedJoint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
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
\date	2/25/2011
**/
Structure::Structure()
{
	m_lpSim = NULL;
	m_lpBody = NULL;
	m_lpPhysicsMovableItem = NULL;
	m_fltSize = 0.02f;
}

/**
\brief	Destructor. 

\author	dcofer
\date	2/25/2011
**/
Structure::~Structure()
{

try
{
	m_aryRigidBodies.RemoveAll();
	m_aryJoints.RemoveAll();
	m_aryExcludeCollisionList.RemoveAll();
	if(m_lpBody) delete m_lpBody;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Structure\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Sets the simulator object. 

\author	dcofer
\date	2/25/2011

\param [in,out]	lpSim	The pointer to a simulation. 
**/
void Structure::Sim(Simulator *lpSim) 
{m_lpSim = lpSim;}

/**
\brief	Sets the root body.

\author	dcofer
\date	5/10/2011

\param [in,out]	lpBody	Pointer to the root body.

**/
void Structure::Body(RigidBody *lpBody) {m_lpBody = lpBody;}

/**
\brief	Gets the root body. 

\details Gets the root body of the structure. 

\author	dcofer
\date	2/25/2011

\return	null if it fails, else. 
**/
RigidBody *Structure::Body() 
{return m_lpBody;}

CStdFPoint Structure::Position() {return MovableItem::Position();}

void Structure::Position(CStdFPoint &oPoint, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
	MovableItem::Position(oPoint, bUseScaling, bFireChangeEvent, bUpdateMatrix);

	///When we change the position of the structure we need to let the rigid body know that.
	if(bUpdateMatrix && m_lpBody && m_lpBody->PhysicsMovableItem())
		m_lpBody->PhysicsMovableItem()->Physics_PositionChanged();
}

/**
\brief	Gets the size of the graphical representation of this joint.

\author	dcofer
\date	3/22/2011

\return	Size for the graphics object.
**/
float Structure::Size() {return m_fltSize;};

/**
\brief	Sets the size of the graphical representation of this joint.

\author	dcofer
\date	3/22/2011

\param	fltVal	   	The new size value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Structure::Size(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Structure.Size");
	if(bUseScaling)
		m_fltSize = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltSize = fltVal;

	//TODO
	//Resize();
}

void Structure::Selected(BOOL bValue, BOOL bSelectMultiple)
{
	AnimatBase::Selected(bValue, bSelectMultiple);
	MovableItem::Selected(bValue, bSelectMultiple);
}

//The structure is not manipulated directly through the editor window. Instead, the root rigid body is moved/rotated.
BOOL Structure::AllowTranslateDragX() {return FALSE;}

BOOL Structure::AllowTranslateDragY() {return FALSE;}

BOOL Structure::AllowTranslateDragZ() {return FALSE;}

BOOL Structure::AllowRotateDragX() {return FALSE;}

BOOL Structure::AllowRotateDragY() {return FALSE;}

BOOL Structure::AllowRotateDragZ() {return FALSE;}


/**
\brief	Gets the collision exclusion list as an array.

\details The collision exclusion list in an array of CollisionPair objects.
Collisions between the two objects in the pair are disabled during the simulation.
This allows the user to manually configure instances where rigid bodies should 
not have collisions between them. This is often important in making a stable and fast 
simulation. For instance, if you have items that may hit each other often and bind 
within a given structure you should disable the collisions between them to prevent this
from occuring.

\author	dcofer
\date	2/25/2011

\return	Array of CollisionPair objects that will be excluded from collisions. 
**/
CStdPtrArray<CollisionPair> Structure::ExclusionList() 
{return m_aryExcludeCollisionList;}

/**
\brief	Collects reporting data for the structure at each time step.

\details This is called during StepSimulation of the structure so that we can collect or setup
any data that needs to be reported back to other systems. An example of this is setting the
ReportPosition and ReportRotation values. 

\author	dcofer
\date	2/25/2011
**/
void Structure::UpdateData()
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_CollectData();
}

void Structure::UpdatePhysicsPosFromGraphics()
{
	if(m_lpBody)
		m_lpBody->UpdatePhysicsPosFromGraphics();
}

void Structure::Create()
{
	if(m_lpBody)
	{
		//First create all of the model objects.
		m_lpBody->CreateParts();

		//Then create all of the joints between the models.
		m_lpBody->CreateJoints();
	}

	m_lpSim->DisableCollisions(this, m_aryExcludeCollisionList);
}

void Structure::ResetSimulation()
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_ResetSimulation();

	if(m_lpBody)
	{
		m_lpBody->ResetSimulation();
		
		UpdateData();

		//We have to call this after method because some objects (ie: muscles and spindles, etc.) depend on other items
		//already being reset to their original positions. So they must be done first and then these items get reset.
		m_lpBody->AfterResetSimulation();
	}
}

void Structure::MinTimeStep(float &fltMin) {}


/**
\brief	Allows the organism and its parts to update at each time slice.

\details This function is called for each structure/organism at every time slice. It gives the
structures a chance to update any values. For instance, the hydrodynamic simulation code needs to
manually add drag and buoyancy forces to each rigid body every time slice. You need to be VERY
careful to keep all code within the StepSimulation methods short, sweet, and very fast. They are
in the main processing loop and even a small increase in the amount of processing time that
occurrs within this loop will lead to major impacts on the ultimate performance of the system. 

\author	dcofer
\date	2/25/2011
**/
void Structure::StepPhysicsEngine()
{
	if(m_lpBody)
	{
		m_lpBody->StepSimulation();
		UpdateData();
	}
}

/**
\brief	Adds a new joint to the list of all joints for this structure.

\details There are two reasons for this method. The first is to get a list of references to all
joints in this structure that is mapped to their ID value. This allows us to use the STL find
funtions to find joints. This is more efficeient that using a loop and recursion. The second
reason is that this also allows us to ensure that each joint that is being added has a unique ID
value. If you attempt to add a joint that has a ID that is already in the list then an exception
will be thrown. Note that this method is NOT creating the object itself, that is done elsewhere.
It is simply adding a reference to that created object to the list. Also, items in that list are
not destroyed when the list is destroyed. It is a list of references only. 

\author	dcofer
\date	2/25/2011

\param [in,out]	lpJoint	pointer to a joint. 
**/

void Structure::AddJoint(Joint *lpJoint)
{
	if(!lpJoint)
		THROW_PARAM_ERROR(Al_Err_lJointNotDefined, Al_Err_strJointNotDefined, "StructureID", m_strName);

	try
	{
		m_aryJoints.Add(lpJoint->ID(), lpJoint);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate joint Key: " + lpJoint->Name() + " in Structure: " + m_strName; 
		THROW_ERROR(oError.m_lError, oError.m_strError);
	}
}

/**
\brief	Removes the joint with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the joint to remove
\exception If ID is not found.
**/
void Structure::RemoveJoint(string strID)
{
	try
	{
		m_aryJoints.Remove(strID);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Joint not found Key: " + strID + " in Structure: " + m_strName;
		THROW_ERROR(oError.m_lError, oError.m_strError);
	}
}

/**
\brief	Adds a new rigid body to the list of all bodies for this structure.

\details There are two reasons for this method. The first is to get a list of references to all
rigid bodies in this structure that is mapped to their ID value. This allows us to use the STL
find funtions to find bodies. This is more efficeient that using a loop and recursion. The second
reason is that this also allows us to ensure that each body that is being added has a unique ID
value. If you attempt to add a  body that has a ID that is already in the list then an exception
will be thrown. Note that this method is NOT creating the object itself, that is done elsewhere.
It is simply adding a reference to that created object to the list. Also, items in that list are
not destroyed when the list is destroyed. It is a list of references only. 

\author	dcofer
\date	2/25/2011

\param [in,out]	lpBody	The pointer to a body. 
**/

void Structure::AddRigidBody(RigidBody *lpBody)
{
	if(!lpBody)
		THROW_PARAM_ERROR(Al_Err_lBodyNotDefined, Al_Err_strBodyNotDefined, "StructureID", m_strName);

	try
	{
		m_aryRigidBodies.Add(lpBody->ID(), lpBody);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate rigid body Key: " + lpBody->Name() + " in Structure: " + m_strName; 
		THROW_ERROR(oError.m_lError, oError.m_strError);
	}
}


/**
\brief	Removes the rigid body with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the body to remove
\exception If ID is not found.
**/
void Structure::RemoveRigidBody(string strID)
{
	try
	{
		m_aryRigidBodies.Remove(strID);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " RigidBody not found Key: " + strID + " in Structure: " + m_strName;
		THROW_ERROR(oError.m_lError, oError.m_strError);
	}
}


/**
\brief	Finds a joint with a specified ID within this structure.

\details This function searches the list of joints associated with this structure 
to find one that matches the ID in a case-insensitive manner. If it finds
it then it returns a pointer to that joint. If it does not find it then
it will either throw an exception or return NULL based on the bThrowError parameter.

\author	dcofer
\date	2/25/2011

\param	strJointID	ID of the joint to find. This is not case sensitive.
\param	bThrowError	If this is TRUE and the ID is not found then an
exception is thrown. If this is FALSE and the ID is not found then NULL is returned.

\return	null if it fails and bThrowError is false, else the pointer to the found joint. 
**/
Joint *Structure::FindJoint(string strJointID, BOOL bThrowError)
{
	Joint *lpJoint = NULL;
	CStdMap<string, Joint *>::iterator oPos;
	oPos = m_aryJoints.find(Std_CheckString(strJointID));

	if(oPos != m_aryJoints.end())
		lpJoint =  oPos->second;
	else if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lJointIDNotFound, Al_Err_strJointIDNotFound, " StructureID: " + m_strName + " JointID: " + strJointID);

	return lpJoint;
}


/**
\brief	Finds a rigid body with a specified ID within this structure.

\details This function searches the list of rigid bodies associated with this structure 
to find one that matches the ID in a case-insensitive manner. If it finds
it then it returns a pointer to that body. If it does not find it then
it will either throw an exception or return NULL based on the bThrowError parameter.

\author	dcofer
\date	2/25/2011

\param	strBodyID	ID of the body to find. This is not case sensitive.
\param	bThrowError	If this is TRUE and the ID is not found then an
exception is thrown. If this is FALSE and the ID is not found then NULL is returned.

\return	null if it fails and bThrowError is false, else the pointer to the found rigid body. 
**/
RigidBody *Structure::FindRigidBody(string strBodyID, BOOL bThrowError)
{
	RigidBody *lpBody = NULL;
	CStdMap<string, RigidBody *>::iterator oPos;
	oPos = m_aryRigidBodies.find(Std_CheckString(strBodyID));

	if(oPos != m_aryRigidBodies.end())
		lpBody =  oPos->second;
	else if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lRigidBodyIDNotFound, Al_Err_strRigidBodyIDNotFound, " StructureID: " + m_strName + " BodyID: " + strBodyID);

	return lpBody;
}


/**
\brief	Searches for a Node with the given ID. 

\author	dcofer
\date	2/25/2011

\param	strID	GUID string ID of the Node.. 
\param	bThrowError	true to throw error if not found. 

\return	null if it fails and bThrowError=false, else the found node. 
**/
Node *Structure::FindNode(string strID, BOOL bThrowError)
{
	Node *lpNode = FindRigidBody(strID, FALSE);

	if(lpNode)
		return lpNode;

	lpNode = FindJoint(strID, FALSE);

	if(lpNode)
		return lpNode;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lRigidBodyIDNotFound, Al_Err_strRigidBodyIDNotFound, " StructureID: " + m_strName + " BodyID: " + strID);

	return NULL;
}

#pragma region DataAccesMethods

void Structure::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);
	m_lpMovableSim = lpSim;
}

float *Structure::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	lpData = MovableItem::GetDataPointer(strDataType);
	if(lpData)
		return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);

	return lpData;
}

BOOL Structure::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(MovableItem::SetData(strDataType, strValue, FALSE))
		return TRUE;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

BOOL Structure::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "RIGIDBODY")
	{
		AddRoot(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL Structure::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "RIGIDBODY")
	{
		RemoveRoot(strID);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

/**
\brief	Creates and adds a root body part. 

\details This method is primarily used by the GUI to add a new root body to the structure.
It creates the root from info in the XML packet and then uses the XML to load in the new
body part. It then initializes it and calls CreateParts, and CreateJoints.

\author	dcofer
\date	2/25/2011

\param	strXml	The xml configuration data packet. 
**/
void Structure::AddRoot(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("RigidBody");

	LoadRoot(oXml);
	m_lpBody->Initialize();

	//First create all of the model objects.
	m_lpBody->CreateParts();

	//Then create all of the joints between the models.
	m_lpBody->CreateJoints();
}

/**
\brief	Removes the root based on ID. 

\details This is primarily used by the GUI to remove the root from the structure when 
the user does this in the GUI.

\author	dcofer
\date	2/25/2011

\param	strID	GUI ID of the root to remove
\param	bThrowError	If true then throw an error if there is a problem, otherwise return false

\return	true if it succeeds, false if it fails. 
**/
void Structure::RemoveRoot(string strID, BOOL bThrowError)
{
	if(m_lpBody && m_lpBody->ID() == strID)
	{
		delete m_lpBody;
		m_lpBody = NULL;
	}
	else
		THROW_PARAM_ERROR(Al_Err_lRigidBodyIDNotFound, Al_Err_strRigidBodyIDNotFound, "ID", strID);
}

#pragma endregion

/**
\brief	Enables the given joints motor. 

\author	dcofer
\date	2/25/2011

\param	strJointID	GUID ID of the joint. 
\param	bVal		Enable/disable value. 
**/
void Structure::EnableMotor(string strJointID, BOOL bVal)
{
	MotorizedJoint *lpJoint = dynamic_cast<MotorizedJoint *>(FindJoint(strJointID));
	if(lpJoint)
		lpJoint->EnableMotor(bVal);
	else
		THROW_PARAM_ERROR(Al_Err_lJointNotMotorized, Al_Err_strJointNotMotorized, "ID", strJointID);
}


/**
\brief	Sets the velocity for the specified joint.

\details This method attempts to locate a joint within a structure and set its velocity.
The velocity will only have any effect if the motor for that joint has been enabled.

\author	dcofer
\date	2/25/2011

\param	strJointID	The ID of the joint.
\param	fltInput	The velocity to set this joint to use.
**/
void Structure::SetMotorInput(string strJointID, float fltInput)
{
	MotorizedJoint *lpJoint = dynamic_cast<MotorizedJoint *>(FindJoint(strJointID));
	if(lpJoint)
		lpJoint->MotorInput(fltInput);
	else
		THROW_PARAM_ERROR(Al_Err_lJointNotMotorized, Al_Err_strJointNotMotorized, "ID", strJointID);
}

/**
\brief	Enables collision between the past-in object and all rigid bodies of this structure.

\details This method enables collision responses between the rigid body being past in and all
rigid bodies in the structure. 

\author	dcofer
\date	2/25/2011

\param [in,out]	lpCollisionBody	his is a pointer to the body to enable collisions on. 
**/
void Structure::EnableCollision(RigidBody *lpCollisionBody)
{
	CStdMap<string, RigidBody *>::iterator oPos;
	RigidBody *lpBody = NULL;
	for(oPos=m_aryRigidBodies.begin(); oPos!=m_aryRigidBodies.end(); ++oPos)
	{
		lpBody = oPos->second;
		lpBody->EnableCollision(lpCollisionBody);
	}
}

/**
\brief	Disables collision between the past-in object and all rigid bodies of this structure.

\details This method disables collision responses between the rigid body being past in and all
rigid bodies in the structure. 

\author	dcofer
\date	2/25/2011

\param [in,out]	lpCollisionBody	This is a pointer to the body to disable collisions on. 

**/
void Structure::DisableCollision(RigidBody *lpCollisionBody)
{
	CStdMap<string, RigidBody *>::iterator oPos;
	RigidBody *lpBody = NULL;
	for(oPos=m_aryRigidBodies.begin(); oPos!=m_aryRigidBodies.end(); ++oPos)
	{
		lpBody = oPos->second;
		lpBody->DisableCollision(lpCollisionBody);
	}
}


/**
\brief	Calculates the snapshot byte size. 

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in time,
and then be able to go back to that specific point. To do this we grab a snapshot of all the data in the system,
including the neural variables. We essentially serialize the data into a binary format for later re-use. This method
calculates the number of bytes that will be required to store the entire nervous system.

\author	dcofer
\date	2/24/2011

\return	The calculated snapshot byte size. 
**/
long Structure::CalculateSnapshotByteSize()
{return 0;}

/**
\brief	Saves a key frame snapshot. 

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in time,
and then be able to go back to that specific point. To do this we grab a snapshot of all the data in the system,
including the neural variables. We essentially serialize the data into a binary format for later re-use. This method
goes through each module and saves its data into the byte array.

\author	dcofer
\date	2/24/2011

\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
\param [in,out]	lIndex		Current zero-based index of the write position in the array. 
**/
void Structure::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{}

/**
\brief	Loads a key frame snapshot. 

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in time,
and then be able to go back to that specific point. To do this we grab a snapshot of all the data in the system,
including the neural variables. We essentially serialize the data into a binary format for later re-use. This method
goes through each module and loads its data from the byte array.

\author	dcofer
\date	2/24/2011

\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
\param [in,out]	lIndex		Current zero-based index of the read position in the array. 
**/
void Structure::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{}


void Structure::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	MovableItem::Load(oXml);

	oXml.IntoElem();  //Into Layout Element

	LoadLayout(oXml);

	oXml.OutOfElem(); //OutOf Layout Element
}


/**
\brief	Adds a collision pair to m_aryExcludeCollisionList

\author	dcofer
\date	2/25/2011

\param	strID1	GUID ID of the first rigid body in the pair. 
\param	strID2	GUID ID of the second rigid body in the pair. 
**/
void Structure::AddCollisionPair(string strID1, string strID2)
{
	CollisionPair *lpPair = new CollisionPair();
	lpPair->m_strPart1ID = strID1;
	lpPair->m_strPart2ID = strID2;

	m_aryExcludeCollisionList.Add(lpPair);
}

/**
\brief	Loads a collision pair. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml The xml data packet to load. 
**/
void Structure::LoadCollisionPair(CStdXml &oXml)
{
	CollisionPair *lpPair = NULL;
	string strID1, strID2;

try
{
	oXml.IntoElem();
	strID1 = oXml.GetChildString("Part1ID");
	strID2 = oXml.GetChildString("Part2ID");
	oXml.OutOfElem();

	lpPair = new CollisionPair();
	lpPair->m_strPart1ID = strID1;
	lpPair->m_strPart2ID = strID2;

	m_aryExcludeCollisionList.Add(lpPair);
}
catch(CStdErrorInfo oError)
{
	if(lpPair) delete lpPair;
	RELAY_ERROR(oError);
}
catch(...)
{
	if(lpPair) delete lpPair;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
}
}

/**
\brief	Loads the layout for this structure from an asl configuration file.

\details This method opens up the Animat Structure Layout (ASL) file that was associated with
this structure in the Animat Simulation (ASIM) file. Once it has loaded the file into a CStdXml
object it then calls the second method to actually create the structure from the configuration
file. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml	The xml data packet to load. 
**/

void Structure::LoadLayout(CStdXml &oXml)
{
	string strModule;
	string strType;

	Size(oXml.GetChildFloat("Size", m_fltSize));

	BOOL bFound = FALSE;
	if(oXml.FindChildElement("Body", FALSE))
		bFound = TRUE;
	else if(oXml.FindChildElement("RigidBody", FALSE))
		bFound = TRUE;

	if(bFound)
	{
		LoadRoot(oXml);

		if(oXml.FindChildElement("CollisionExclusionPairs", FALSE))
		{
			oXml.IntoElem();  //Into CollisionExclusionPairs Element
			int iChildCount = oXml.NumberOfChildren();
			for(int iIndex=0; iIndex<iChildCount; iIndex++)
			{
				oXml.FindChildByIndex(iIndex);
				LoadCollisionPair(oXml);
			}
			oXml.OutOfElem(); //OutOf CollisionExclusionPairs Element
		}
	}
	else
		m_lpBody = NULL;

}


/**
\brief	Loads the root rigid body. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml The xml data packet to load. 

\return	The root. 
**/
RigidBody *Structure::LoadRoot(CStdXml &oXml)
{
	string strModule;
	string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	RigidBody *lpBody = dynamic_cast<RigidBody *>(m_lpSim->CreateObject(strModule, "RigidBody", strType));
	if(!lpBody)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RigidBody");
	Body(lpBody);
	m_lpBody->Parent(NULL);

	m_lpBody->SetSystemPointers(m_lpSim, this, NULL, NULL, TRUE);
	m_lpBody->Load(oXml);
	AddRigidBody(m_lpBody);

	return m_lpBody;
}
catch(CStdErrorInfo oError)
{
	if(m_lpBody) delete m_lpBody;
	m_lpBody = NULL;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(m_lpBody) delete m_lpBody;
	m_lpBody = NULL;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			// Environment
}				//AnimatSim
