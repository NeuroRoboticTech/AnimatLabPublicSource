/**
\file	AnimatBase.cpp

\brief	Implements the animat base class. 

\details Cpp file that implements the base class for almost all 
objects in the AnimatLab simulation environment.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include "Gain.h"
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

/**
\brief	Constructs an AnimatBase object.

\details Sets the ID to a default GUID value on creation. and selected is set to false by
default. 

\author	dcofer
\date	2/22/2011
**/
AnimatBase::AnimatBase()
{
	m_lpSim = NULL;
	m_lpStructure = NULL;
	m_lpModule = NULL;
	m_lpNode = NULL;
	m_strID = Std_CreateAppID();
	m_bSelected = FALSE;
}

/**
\brief	Destroys the AnimatBase object..

\details When an AnimatBase object is destroyed a call is made to remove that object from the
object list kept in the simulator. All objects are added to that list in their Load methods. The
list allows you to easily find objects based on ID or name, and to call some of the base methods
like AddItem and SetData. 

\author	dcofer
\date	2/22/2011
**/
AnimatBase::~AnimatBase()
{

try
{
	//This will remove this object from the object list of the simulation.
	if(m_lpSim)
		m_lpSim->RemoveFromObjectList(this);
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of AnimatBase\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the simulator pointer. 

\author	dcofer
\date	2/24/2011

\return	pointer the Simulator object for this simulation. 
**/
Simulator *AnimatBase::GetSimulator() {return m_lpSim;}

/**
\brief	Gets the structure for this node. 

\author	dcofer
\date	2/24/2011

\return	returns the Structure pointer for this node. 
**/
Structure *AnimatBase::GetStructure() {return m_lpStructure;}

/**
\brief	Gets the neural module. 

\author	dcofer
\date	2/24/2011

\return	Returns the NeuralModule pointer associated with this node. This only applies to neural
network nodes. All others will return NULL. 
**/
NeuralModule *AnimatBase::GetNeuralModule() {return m_lpModule;}

/**
\brief	Gets the node. 

\author	dcofer
\date	3/2/2011

\return	Parent node pointer. If none exists it returns NULL. 
**/
Node *AnimatBase::GetNode() {return m_lpNode;}

/**

\fn	virtual string AnimatBase::ID()
		
\brief	Gets the unique GUID ID of this object. 

\author	dcofer
\date	2/22/2011
		
\return	string ID GUID. 
**/
string AnimatBase::ID() {return m_strID;}

/**
\brief	Sets the unique GUID ID of the object.

\details ID value must not be blank or exception Al_Err_lIDBlank will be thrown. 

\author	dcofer
\date	2/22/2011

\param	strID	new ID string value. 
**/
void AnimatBase::ID(string strID) 
{
	if(Std_IsBlank(strID))
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strID = Std_CheckString(strID);
}

/**
\brief	Gets the name of this object. 

\author	dcofer
\date	2/22/2011

\return	string name. 
**/
string AnimatBase::Name() {return m_strName;}

/**
\brief	Sets the name of the object. Blank is acceptable. 

\author	dcofer
\date	2/22/2011

\param	strValue	The name value. 
**/
void AnimatBase::Name(string strValue) {m_strName = strValue;}

/**
\brief	returns the string type name of this object.

\details Gets the string type description of this object. This is basically just another type of
tag that can be applied by the developer of a class to differentiate it. An example of where this
is used is in the synaptic types class. I use it to specify the synapse type as gated, modulatory,
etc.. They still have different classes, but this gives a string description of that type. It is
not mandatory and can be used how the class developer requires. 

\author	dcofer
\date	2/22/2011

\return	string of assigned class type. 
**/
string AnimatBase::Type() {return m_strType;}

/**
\brief	Sets the class type for this object. 

\author	dcofer
\date	2/22/2011

\param	strValue	The string value. 
**/
void AnimatBase::Type(string strValue) {m_strType = strValue;}

/**
\brief	Tells if this items is selected or not.

\details Body part items can be selected in the simulation by clicking on them while the
simulation is in the correct mode. When selected the draggers are shown for manipulating the
part. In the GUI parts can be selected by clicking on them in teh project workspace. Whenever
this happens the selected variable is set so we know that the part is selected. Also, if we
selected it in the simulation, then when applicable events are raised back up to the GUI to let
it know that a part has been selected. 

\author	dcofer
\date	2/22/2011

\return	true if it selected, false if not. 
**/
BOOL AnimatBase::Selected() {return m_bSelected;}

/**
\brief	Selects this object.

\details Selects or deselects this object. If selectmultiple is true and bValue is true then it
will try and add this item to a group of selected items. If selectmultiple is false then the
currently selected item will be deselected and this one will be selected. All of this selection
logic is actually taking place in the GUI. All the simulation does is have this variable that
keeps track of whether THIS item is selected or not. Determining if it should be part of a group
or not is done in the GUI project workspace. 

\author	dcofer
\date	2/22/2011

\param	bValue			true to select, false to deselect. 
\param	bSelectMultiple	If true then this items is added as part of a group. If false then it is
						removed from a group. 
**/
void AnimatBase::Selected(BOOL bValue, BOOL bSelectMultiple) {m_bSelected = bValue;}


#pragma region DataAccesMethods


//Don't know why, but the documentation for this has to be in the .h file. When I try and put it here
//it is not processed by doxygen. ????
void AnimatBase::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	m_lpSim = lpSim;
	m_lpStructure = lpStructure;
	m_lpModule = lpModule;
	m_lpNode = lpNode;

	if(bVerify) VerifySystemPointers();
}

/**
\brief	Verify that system pointers have been set correctly.

\details The system pointers should be set just after an object is created. However, if for some
reason it was not then this could cause read/write memory access errors later on because we would
be using NULL pointers. This method is called during SetSystemPointers, Load, and Initialize to
verify that the pointers for that type of object have been set correctly. We are calling three
different places to ensure that it is checked before use. For example, if you are doing a Load
call then you want to check it before attempting the load, but there may be an object that does
not Load, but only does Initialize. So we need to check it there as well. 

\author	dcofer
\date	3/4/2011
**/
void AnimatBase::VerifySystemPointers()
{
	if(!m_lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);
}

/**
\brief	Returns a float pointer to a data item of interest in this object.

\details This is a generic method used to get a pointer to data variable of interest. It is used
by a variety of systems in the simulation. The most prominent are the data charting and stimulus
classes. Within this method we associate a variable with a string name. By passing in the name of
the data type we are interested in we can recieve back a float pointer to that data type. We can
use that to read or set the data item in other classes. For example, the data charting system
gets the pointer and then each time it needs to log a data point it reads the value into an
array. 

\author	dcofer
\date	2/22/2011

\param	strDataType	name of the data item for which we are looking. 

\return	float pointer of the data item. If not found then it throws an exception. 
\exception	If	DataType is not found. 
**/
float *AnimatBase::GetDataPointer(string strDataType)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, ("ID: " + m_strID + " Name: " + m_strName));
	return NULL;
}

/**
\brief	Set a variable based on a string data type name.

\details This is a generic method that can be used to set any variable in an AnimatBase object by
specifying the name of the variable and a string representation of that data. The GUI uses this
method to set data into variables in the simulation when the user changes them in the UI. The
value string can be as simple as a float or int, or as complex as an xml packet. It is the
developers responsibilty to know what type of data is needed and to process it accordingly. 

\author	dcofer
\date	2/22/2011

\param	strDataType	string name of the data type to set. 
\param	strValue	The string value of the data. It is up to the developer to determine what
					this should be. For example, in most cases it is simply a float and you just
					have to convert it to a float and make the appropriate mutator method call.
					However, it can be any type of string, including an entire xml packet. It is
					the developers responsibility to know how to set and process the data as
					required. 
\param	bThrowError	true to throw error if there is a problem. If false then it will not return
					an error, just return false. 

\return	true if it succeeds, false if it fails. 
**/
BOOL AnimatBase::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(strDataType == "NAME")
	{
		Name(strValue);
		return TRUE;
	}

	if(strDataType == "CALL_INIT")
	{
		this->Initialize();
		return TRUE;
	}

	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, ("ID: " + m_strID + " Name: " + m_strName));
	return FALSE;
}

/**
\brief	Queries this object for a list of properties that can be changed using SetData.

\details The method provides a list of the properties that can be set using the SetData method.
It gives the property name and the type of data that should be passed to it. Valid date types
are Boolean, Float, Integer, and Xml.

\author	dcofer
\date	2/22/2011

\param	aryNames	Array list of the property names. 
\param	strTypes	Array list of the type that is associated with the cooresponding name in the list. 

\return	Nothing. 
**/
void AnimatBase::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	aryNames.Add("Name");
	aryTypes.Add("String");
}

/**
\brief	Queries this object if it has a property with the given name.

\details This method checks whether it has a property with the specified name.

\author	dcofer
\date	2/6/2013

\param	strName		Property name we are checking for. 

\return	True if a property with that exact name is found. Capilalization is not important. 
**/
BOOL AnimatBase::HasProperty(string strName)
{
	CStdArray<string> aryNames, aryTypes;
	QueryProperties(aryNames, aryTypes);

	string strCheck = Std_CheckString(strName);
	int iCount = aryNames.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(Std_CheckString(aryNames[iIdx]) == strCheck)
			return TRUE;

	return FALSE;
}


AnimatBase::AnimatPropertyType AnimatBase::PropertyType(string strName)
{
	CStdArray<string> aryNames, aryTypes;
	QueryProperties(aryNames, aryTypes);

	string strCheck = Std_CheckString(strName);
	int iCount = aryNames.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(Std_CheckString(aryNames[iIdx]) == strCheck)
		{
			string strType = Std_CheckString(aryTypes[iIdx]);
			if(strType == "BOOLEAN")
				return AnimatPropertyType::Boolean;
			else if(strType == "INTEGER")
				return AnimatPropertyType::Integer;
			else if(strType == "FLOAT")
				return AnimatPropertyType::Float;
			else if(strType == "STRING")
				return AnimatPropertyType::String;
			else if(strType == "XML")
				return AnimatPropertyType::Xml;
			else
				return AnimatPropertyType::Invalid;
		}

	return AnimatPropertyType::Invalid;
}

/**
\brief	Adds a new object to this parent.

\details Generic method to add a new child item to this parent by specifying a string item type
descriptor and an xml packet that can be used to load in the new object. The GUI uses this method
to create new items that were added with the user interface. The item type lets the method
determine what type of item is being created, like synapse, neuron, body part, etc.. It then gets
the modulename, classname, and type from the xml and calls CreateObject to create the appropriate
type of object. Then it passes in the xml packet to the new objects load method and does any
needed initialization and adds it to the parent. 

\author	dcofer
\date	2/22/2011

\param	strItemType	String descriptor of the type of item that is being created. 
\param	strXml		XML packet that is used to create and load the new item. 
\param	bThrowError	If true then throw an error if there is a problem, otherwise return false. 

\return	true if it succeeds, false if it fails. 
**/
BOOL AnimatBase::AddItem(string strItemType, string strXml, BOOL bThrowError, BOOL bDoNotInit)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lItemTypeInvalid, Al_Err_strItemTypeInvalid, ("ID: " + m_strID + " ItemType: " + strItemType));
	return FALSE;
}

/**
\brief	Removes a child item from this parent.

\details This is a generic method that is used to delete a child object from this parent. The GUI
uses this method to remove objects from the simulation that have been deleted in the UI. The item
type lets the method determine what type of item is being deleted, like synapse, neuron, body
part, etc.. The ID is then used to delete that specific item. 

\author	dcofer
\date	2/22/2011

\param	strItemType	String descriptor of the type of item that is being created. 
\param	strID		Unique ID of the item that will be removed. 
\param	bThrowError	If true then throw an error if there is a problem, otherwise return false. 

\return	true if it succeeds, false if it fails. 
**/
BOOL AnimatBase::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lItemNotFound, Al_Err_strItemNotFound, ("ID: " + m_strID));
	return FALSE;
}

#pragma endregion


#pragma region SimulationMethods

/**
\brief	Resets this object.

\deatils Call this method to reset all data for this object back to its pre-loaded state. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::Reset() {};

/**
\brief	Initializes this object.

\details After an object is loaded in it must be initialized before it can be used. This allows
the object to retrieve any pointers or setup memory that it will need during execution. Each
object is responsible for initializing any of its child objects, so you simply need to call this
method on the containing class to init all child objects. It also calls VerifySystemPointers to
make sure that the system pointers have been setup correctly. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::Initialize()
{
	VerifySystemPointers();
}

/**
\brief	Resets the simulation back to time 0.

\details This method calls the ResetSimulation method on all subitems in order to reset the
simulation back to the beginning. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::ResetSimulation() {};

/**
\brief	Called after a simulation reset for some objects.

\details This method is only used by some objects because they need to do some processing after
the simulation has been reset. 

\author	dcofer
\date	3/4/2011
**/
void AnimatBase::AfterResetSimulation() {};

/**
\brief	Re-initialize this object.

\details Some objects like data charts need to be re-initialized some times. An example of when
that would be needed is when the simulation is re-started. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::ReInitialize() {};

/**
\brief	Kills.

\details Called to kill the organism, nervous system, neurons, and body parts. All neural items
are disabled to prevent any further neural activity, and all joints are disabled to allow free
rotation, and all biomechancical components are disabled so they can no longer produce forces.
This method is only relevant to these types of objects, but I am putting the definition in the
base class because a variety of different types of classes all need this method and I want it
consolidated. Those classes that do not need it do not have to call it or do anything when it is
called. 

\author	dcofer
\date	3/3/2011

\param	bState	true to state. 
**/
void AnimatBase::Kill(BOOL bState) {};

/**
\brief	Step the simulation for this object.

\details This is called on an object each time it is stepped in the simulation. this is where its
simulation code is processed. However, StepSimulation is not necessarily called every single time
that the simulation as a whole is stepped. A good example of this is that neural modules can have
different integration time steps. So a firing rate module may have a DT of 0.5 ms, while an
integrate and fire model may have one of 0.1 ms. So the firing rate module would only get its
StepSimulation method called every 5th time that the other module was called. This is all handed
in the StepSimulation method of the Simulator and NervousSystem. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::StepSimulation() {};

/**
\brief	Called just before the simulation starts.

\details This method is called on each AnimatBase object when the simulation starts. It allows it
to perform any intialization prior to the beginning of the simulation that is needed. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::SimStarting() {};

/**
\brief	Called just before the simulation pauses.

\details This method is called on each AnimatBase object when the simulation pauses. It allows it
to perform any intialization prior to the pause of the simulation that is needed. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::SimPausing() {};

/**
\brief	Called just before the simulation stops.

\details This method is called on each AnimatBase object when the simulation stops. It allows it
to perform any intialization prior to the stop of the simulation that is needed. 

\author	dcofer
\date	3/1/2011
**/
void AnimatBase::SimStopping() {};

/**
\brief	Notification method that the time step modified has been modified. 
Objects should recalculate any slice times as needed.

\author	dcofer
\date	1/28/2012
**/
void AnimatBase::TimeStepModified() {};

#pragma endregion

#pragma region SnapshotMethods

/**
\brief	Calculates the snapshot byte size.

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in
time, and then be able to go back to that specific point. To do this we grab a snapshot of all
the data in the system, including the neural variables. We essentially serialize the data into a
binary format for later re-use. This method calculates the number of bytes that will be required
to store the entire object. 

\author	dcofer
\date	2/24/2011

\return	The calculated snapshot byte size. 
**/
long AnimatBase::CalculateSnapshotByteSize() {return 0;}

/**
\brief	Saves a key frame snapshot.

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in
time, and then be able to go back to that specific point. To do this we grab a snapshot of all
the data in the system, including the neural variables. We essentially serialize the data into a
binary format for later re-use. This method goes through each module and saves its data into the
byte array. 

\author	dcofer
\date	2/24/2011

\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
\param [in,out]	lIndex		Current zero-based index of the write position in the array. 
**/
void AnimatBase::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex) {}

/**
\brief	Loads a key frame snapshot.

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in
time, and then be able to go back to that specific point. To do this we grab a snapshot of all
the data in the system, including the neural variables. We essentially serialize the data into a
binary format for later re-use. This method goes through each module and loads its data from the
byte array. 

\author	dcofer
\date	2/24/2011

\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
\param [in,out]	lIndex		Current zero-based index of the read position in the array. 
**/
void AnimatBase::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex) {}

#pragma endregion

/**
\brief	Visual selection mode changed.

\details This is called whenever the visual selection mode of the simulation is changed. This is
when the user switches from selecting graphics, collision objects, joints, etc.. 

\author	dcofer
\date	3/1/2011

\param	iNewMode	The new mode. 
**/
void AnimatBase::VisualSelectionModeChanged(int iNewMode) {}

/**
\brief	Loads the item using an XML data packet.

\details This method is responsible for loading the structure from a XMl configuration file. You
should call this method even in your overriden function becuase it loads all of the base
properties for this object like ID and Name. It also includes this object in the simulators
AddToObjectList so that the simulator knows about this object when you do a FindObject call. If
you do not call this base method then it is up to you to add your item to the simulators list of
objects. 

\author	dcofer
\date	3/1/2011

\param [in,out]	oXml	The CStdXml xml data packet to load. 
**/
void AnimatBase::Load(StdUtils::CStdXml &oXml)
{
	VerifySystemPointers();

	oXml.IntoElem();
	m_strType = oXml.GetChildString("Type", m_strType);
	m_strID = Std_CheckString(oXml.GetChildString("ID", m_strID));
	m_strName = oXml.GetChildString("Name", m_strName);
	oXml.OutOfElem();

	if(Std_IsBlank(m_strID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	if(Std_IsBlank(m_strName)) 
		m_strName = m_strID;

	//if(Std_ToLower(m_strID) == "d10da888-6478-4d60-b878-0e761b7e8ef2")
	//	m_strID = m_strID;

	//This will add this object to the object list of the simulation.
	if(m_lpSim)
		m_lpSim->AddToObjectList(this);
}

}			//AnimatSim
