/**
\file	Structure.cpp

\brief	Implements the structure class. 
**/

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

/**
\fn	Structure::Structure()

\brief	Default constructor. 

\author	dcofer
\date	2/25/2011
**/
Structure::Structure()
{
	m_lpSim = NULL;
	m_lpBody = NULL;
	m_lpCallback = NULL;
}

/**
\fn	Structure::~Structure()

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
\fn	void Structure::Sim(Simulator *lpSim)

\brief	Sets the simulator object. 

\author	dcofer
\date	2/25/2011

\param [in,out]	lpSim	The pointer to a simulation. 
**/
void Structure::Sim(Simulator *lpSim) 
{m_lpSim = lpSim;}

/**
\fn	RigidBody *Structure::Body()

\brief	Gets the root body. 

\details Gets the root body of the structure. 

\author	dcofer
\date	2/25/2011

\return	null if it fails, else. 
**/
RigidBody *Structure::Body() 
{return m_lpBody;}

/**
\fn	CStdFPoint Structure::Position()

\brief	Gets the current position of the structure. 

\author	dcofer
\date	2/25/2011

\return	CStdFPoint position. 
**/
CStdFPoint Structure::Position() 
{return m_oPosition;}

/**
\fn	void Structure::Position(CStdFPoint &oPoint)

\brief	Sets the position of the structure. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oPoint	The new position. 
**/
void Structure::Position(CStdFPoint &oPoint) 
{m_oPosition = oPoint;}

/**
\fn	CStdFPoint Structure::ReportPosition()

\brief	Gets the position that is reported back to the GUI.

\details Internally the simulation represents both space and mass abstractly. It scales the number based
on the users setting of MassUnits and DistanceUnits. So this means that for information that is reported back
to the user it must be rescaled appropriately to appear correct for them. This variable

\author	dcofer
\date	2/25/2011

\return	. 
**/
CStdFPoint Structure::ReportPosition() 
{return m_oReportPosition;}

/**
\fn	void Structure::ReportPosition(CStdFPoint &oPoint)

\brief	sets the reported position. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oPoint	The reported position. 
**/
void Structure::ReportPosition(CStdFPoint &oPoint) 
{m_oReportPosition = oPoint;}

/**
\fn	void Structure::ReportPosition(float fltX, float fltY, float fltZ)

\brief	sets the reported position. 

\author	dcofer
\date	2/25/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void Structure::ReportPosition(float fltX, float fltY, float fltZ)
{m_oReportPosition.Set(fltX, fltY, fltZ);}

/**
\fn	CStdFPoint Structure::ReportRotation()

\brief	Gets the rotation that is reported back to the GUI.

\author	dcofer
\date	2/25/2011

\return	CStdFPoint rotation in radians. 
**/

CStdFPoint Structure::ReportRotation() 
{return m_oReportRotation;}

/**
\fn	void Structure::ReportRotation(CStdFPoint &oPoint)

\brief	Sets the report rotation in radians. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oPoint	The new rotation value. 
**/
void Structure::ReportRotation(CStdFPoint &oPoint) 
{m_oReportRotation = oPoint;}

/**
\fn	void Structure::ReportRotation(float fltX, float fltY, float fltZ)

\brief	Sets the report rotation in radians. 

\author	dcofer
\date	2/25/2011

\param	fltX	The x rotation. 
\param	fltY	The y rotation. 
\param	fltZ	The z rotation. 
**/
void Structure::ReportRotation(float fltX, float fltY, float fltZ) 
{m_oReportRotation.Set(fltX, fltY, fltZ);}

/**
\fn	CStdPtrArray<CollisionPair> Structure::ExclusionList()

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
\fn	IBodyPartCallback *Structure::Callback()

\brief	Gets the IBodyPartCallback pointer for this structure. 

\author	dcofer
\date	2/25/2011

\return	null if it fails, else. 
**/
IBodyPartCallback *Structure::Callback() 
{return m_lpCallback;}

/**
\fn	void Structure::Callback(IBodyPartCallback *lpCallback)

\brief	Sets the IBodyPartCallback pointer for this structure. 

\details This is used by the GUI interface to pass in a pointer to a 
callback object that will communicate events back up to the GUI.

\author	dcofer
\date	2/25/2011

\param [in,out]	lpCallback	The pointer to a callback. 
**/
void Structure::Callback(IBodyPartCallback *lpCallback) 
{m_lpCallback = lpCallback;}
/**
\fn	void Structure::CollectStructureData()

\brief	Collects reporting data for the structure at each time step.

\details This is called during StepSimulation of the structure so that we can collect or setup
any data that needs to be reported back to other systems. An example of this is setting the
ReportPosition and ReportRotation values. 

\author	dcofer
\date	2/25/2011
**/
void Structure::CollectStructureData()
{
	if(m_lpSim && m_lpBody)
	{
		m_oReportPosition = m_lpBody->ReportWorldPosition();
		m_oReportRotation = m_lpBody->ReportRotation();
	}
}

void Structure::Initialize()
{
	AnimatBase::Initialize();

	if(m_lpBody)
	{
		//First create all of the model objects.
		m_lpBody->CreateParts();

		//Then create all of the joints between the models.
		m_lpBody->CreateJoints();
	}

	//Now lets disable any collisions that have been added to the exclusion list.
	int iCount = m_aryExcludeCollisionList.GetSize();
	CollisionPair *lpPair = NULL;
	RigidBody *lpPart1=NULL, *lpPart2=NULL;

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair =	m_aryExcludeCollisionList[iIndex];
		lpPart1 = this->FindRigidBody(lpPair->m_strPart1ID);
		lpPart2 = this->FindRigidBody(lpPair->m_strPart2ID);

		lpPart1->DisableCollision(lpPart2);
	}
}

void Structure::ResetSimulation()
{
	if(m_lpBody)
	{
		m_lpBody->ResetSimulation();
		
		CollectStructureData();

		//We have to call this after method because some objects (ie: muscles and spindles, etc.) depend on other items
		//already being reset to their original positions. So they must be done first and then these items get reset.
		m_lpBody->AfterResetSimulation();
	}
}

/**
\fn	void Structure::StepPhysicsEngine()

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
		CollectStructureData();
	}
}

/**
\fn	void Structure::AddJointToList(Joint *lpJoint)

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

void Structure::AddJointToList(Joint *lpJoint)
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
\fn	void Structure::AddRigidBodyToList(RigidBody *lpBody)

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

void Structure::AddRigidBodyToList(RigidBody *lpBody)
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
\fn	Joint *Structure::FindJoint(string strJointID, BOOL bThrowError)

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
\fn	RigidBody *Structure::FindRigidBody(string strBodyID, BOOL bThrowError)

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
\fn	Node *Structure::FindNode(string strID, BOOL bThrowError)

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

float *Structure::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "LOCALPOSITIONX" || strType == "LOCALPOSITIONY" || strType == "LOCALPOSITIONZ")
		return NULL;

	if(strType == "POSITIONX" || strType == "WORLDPOSITIONX")
		return &m_oReportPosition.x;

	if(strType == "POSITIONY" || strType == "WORLDPOSITIONY")
		return &m_oReportPosition.y;

	if(strType == "POSITIONZ" || strType == "WORLDPOSITIONZ")
		return &m_oReportPosition.z;

	if(strType == "ROTATIONX")
		return &m_oReportRotation.x;

	if(strType == "ROTATIONY")
		return &m_oReportRotation.y;

	if(strType == "ROTATIONZ")
		return &m_oReportRotation.z;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);

	return lpData;
}

BOOL Structure::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

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
\fn	void Structure::AddRoot(string strXml)

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
\fn	void Structure::RemoveRoot(string strID, BOOL bThrowError)

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
\fn	void Structure::EnableMotor(string strJointID, BOOL bVal)

\brief	Enables the given joints motor. 

\author	dcofer
\date	2/25/2011

\param	strJointID	GUID ID of the joint. 
\param	bVal		Enable/disable value. 
**/
void Structure::EnableMotor(string strJointID, BOOL bVal)
{
	Joint *lpJoint = FindJoint(strJointID);
	lpJoint->EnableMotor(bVal);
}


/**
\fn	void Structure::SetMotorInput(string strJointID, float fltInput)

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
	Joint *lpJoint = FindJoint(strJointID);
	lpJoint->MotorInput(fltInput);
}

/**
\fn	void Structure::EnableCollision(RigidBody *lpCollisionBody)

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
\fn	void Structure::DisableCollision(RigidBody *lpCollisionBody)

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
\fn	long Structure::CalculateSnapshotByteSize()

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
\fn	void Structure::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)

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
\fn	void Structure::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)

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

	oXml.IntoElem();  //Into Layout Element

	Std_LoadPoint(oXml, "Position", m_oPosition);

	LoadLayout(oXml);

	oXml.OutOfElem(); //OutOf Layout Element
}


/**
\fn	void Structure::AddCollisionPair(string strID1, string strID2)

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
\fn	void Structure::LoadCollisionPair(CStdXml &oXml)

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
\fn	void Structure::LoadLayout(CStdXml &oXml)

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
\fn	RigidBody *Structure::LoadRoot(CStdXml &oXml)

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

	m_lpBody = dynamic_cast<RigidBody *>(m_lpSim->CreateObject(strModule, "RigidBody", strType));
	if(!m_lpBody)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RigidBody");
	m_lpBody->Parent(NULL);

	m_lpBody->SetSystemPointers(m_lpSim, this, NULL, NULL);
	m_lpBody->Load(oXml);
	m_lpBody->CompileIDLists();

	return m_lpBody;
}
catch(CStdErrorInfo oError)
{
	if(m_lpBody) delete m_lpBody;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(m_lpBody) delete m_lpBody;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			// Environment
}				//AnimatSim
