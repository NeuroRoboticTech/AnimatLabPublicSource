/**
\file	AnimatBase.cpp

\brief	Implements the animat base class. 

\details Cpp file that implements the base class for almost all 
objects in the AnimatLab simulation environment.
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include "Gain.h"
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


/**

\fn	AnimatBase::AnimatBase()

\brief	Constructs an AnimatBase object.

\details Sets the ID to a default GUID value on creation.
and selected is set to false by default.

\author	dcofer
\date	2/22/2011
**/

AnimatBase::AnimatBase()
{
	m_strID = Std_CreateAppID();
	m_bSelected = FALSE;
}


/**

\fn	AnimatBase::~AnimatBase()

\brief	Destroys the AnimatBase object..

\details When an AnimatBase object is destroyed a call is made to
   remove that object from the object list kept in the simulator. All 
   objects are added to that list in their Load methods. The list allows
   you to easily find objects based on ID or name, and to call some of the 
   base methods like AddItem and SetData.

\author	dcofer
\date	2/22/2011
**/
AnimatBase::~AnimatBase()
{

try
{
	//This will remove this object from the object list of the simulation.
	Simulator *lpSim = GetSimulator();
	if(lpSim)
		lpSim->RemoveFromObjectList(this);
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of AnimatBase\r\n", "", -1, FALSE, TRUE);}
}

/**

\fn	virtual string AnimatBase::ID()
		
\brief	Gets the unique GUID ID of this object. 

\author	dcofer
\date	2/22/2011
		
\return	string ID GUID. 
**/
string AnimatBase::ID() {return m_strID;}

/**

\fn	virtual void AnimatBase::ID(string strValue);
		
\brief	Sets the unique GUID ID of the object.

\details ID value must not be blank or exception Al_Err_lIDBlank will be thrown.

\author	dcofer
\date	2/22/2011
		
\param	strValue	new ID string value. 
**/
void AnimatBase::ID(string strID) 
{
	if(Std_IsBlank(strID))
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strID = Std_CheckString(strID);
}

/**

\fn	string AnimatBase::Name()

\brief	Gets the name of this object. 

\author	dcofer
\date	2/22/2011

\return	string name. 
**/
string AnimatBase::Name() {return m_strName;}

/**

\fn	void AnimatBase::Name(string strValue)

\brief	Sets the name of the object. Blank is acceptable. 

\author	dcofer
\date	2/22/2011

\param	strValue	The name value. 
**/
void AnimatBase::Name(string strValue) {m_strName = strValue;}

/**

\fn	string AnimatBase::Type()

\brief	returns the string type name of this object.

\details Gets the string type description of this object. This is basically just another
type of tag that can be applied by the developer of a class to differentiate it. An example
of where this is used is in the synaptic types class. I use it to specify the synapse type
as gated, modulatory, etc.. They still have different classes, but this gives a string description
of that type. It is not mandatory and can be used how the class developer requires.

\author	dcofer
\date	2/22/2011

\return	string of assigned class type. 
**/
string AnimatBase::Type() {return m_strType;}

/**

\fn	void AnimatBase::Type(string strValue)

\brief	Sets the class type for this object. 

\author	dcofer
\date	2/22/2011

\param	strValue	The string value. 
**/
void AnimatBase::Type(string strValue) {m_strType = strValue;}

/**

\fn	BOOL AnimatBase::Selected()

\brief	Tells if this items is selected or not. 

\details Body part items can be selected in the simulation by clicking on them while the simulation
is in the correct mode. When selected the draggers are shown for manipulating the part. In the GUI parts
can be selected by clicking on them in teh project workspace. Whenever this happens the selected variable
is set so we know that the part is selected. Also, if we selected it in the simulation, then when applicable
events are raised back up to the GUI to let it know that a part has been selected.
\author	dcofer
\date	2/22/2011

\return	true if it selected, false if not. 
**/
BOOL AnimatBase::Selected() {return m_bSelected;}

/**

\fn	void AnimatBase::Selected(BOOL bValue, BOOL bSelectMultiple)

\brief	Selects this object.

\details Selects or deselects this object. If selectmultiple is true and bValue 
is true then it will try and add this item to a group of selected items. If selectmultiple
is false then the currently selected item will be deselected and this one will be selected.
All of this selection logic is actually taking place in the GUI. All the simulation does is
have this variable that keeps track of whether THIS item is selected or not. Determining if
it should be part of a group or not is done in the GUI project workspace.

\author	dcofer
\date	2/22/2011

\param	bValue			true to select, false to deselect. 
\param	bSelectMultiple	If true then this items is added as part of a group. 
If false then it is removed from a group. 
**/
void AnimatBase::Selected(BOOL bValue, BOOL bSelectMultiple) {m_bSelected = bValue;}


#pragma region DataAccesMethods

/**

\fn	float *AnimatBase::GetDataPointer(string strDataType)

\brief	Returns a float pointer to a data item of interest in this object. 

\details This is a generic method used to get a pointer to data variable of interest. It is used by a variety
of systems in the simulation. The most prominent are the data charting and stimulus classes.
Within this method we associate a variable with a string name. By passing in the name of the 
data type we are interested in we can recieve back a float pointer to that data type. We can use
that to read or set the data item in other classes. For example, the data charting system gets the 
pointer and then each time it needs to log a data point it reads the value into an array. 

\author	dcofer
\date	2/22/2011

\param	string name of the data item for which we are looking. 

\return	float pointer of the data item. If not found then it throws an exception.
**/
float *AnimatBase::GetDataPointer(string strDataType)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, ("ID: " + m_strID + " Name: " + m_strName));
	return NULL;
}

/**

\fn	BOOL AnimatBase::SetData(string strDataType, string strValue, BOOL bThrowError)

\brief	Set a variable based on a string data type name. 

\details This is a generic method that can be used to set any variable in an AnimatBase object
by specifying the name of the variable and a string representation of that data. The GUI uses this 
method to set data into variables in the simulation when the user changes them in the UI. The value string
can be as simple as a float or int, or as complex as an xml packet. It is the developers responsibilty 
to know what type of data is needed and to process it accordingly.

\author	dcofer
\date	2/22/2011

\param	strDataType	string name of the data type to set. 
\param	strValue	The string value of the data. It is up to the developer to determine what this should be. 
For example, in most cases it is simply a float and you just have to convert it to a float and make the appropriate
mutator method call. However, it can be any type of string, including an entire xml packet. It is the developers
responsibility to know how to set and process the data as required.
\param	bThrowError	true to throw error if there is a problem. If false then it will not return an error, just return false. 

\return	true if it succeeds, false if it fails. 
**/
BOOL AnimatBase::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(strDataType == "NAME")
		Name(strValue);

	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, ("ID: " + m_strID + " Name: " + m_strName));
	return FALSE;
}

/**

\fn	BOOL AnimatBase::AddItem(string strItemType, string strXml, BOOL bThrowError)

\brief	Adds a new object to this parent.

\details Generic method to add a new child item to this parent by specifying a string item
type descriptor and an xml packet that can be used to load in the new object. The GUI uses this 
method to create new items that were added with the user interface. The item type
lets the method determine what type of item is being created, like synapse, neuron, body part, etc..
It then gets the modulename, classname, and type from the xml and calls CreateObject to create
the appropriate type of object. Then it passes in the xml packet to the new objects load method and
does any needed initialization and adds it to the parent.

\author	dcofer
\date	2/22/2011

\param	strItemType	String descriptor of the type of item that is being created. 
\param	strXml		XML packet that is used to create and load the new item.
\param	bThrowError	If true then throw an error if there is a problem, otherwise return false

\return	true if it succeeds, false if it fails. 
**/
BOOL AnimatBase::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	//If we are using the AnimatBase function then there are no data pointer, so throw an error.
	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lItemTypeInvalid, Al_Err_strItemTypeInvalid, ("ID: " + m_strID + " ItemType: " + strItemType));
	return FALSE;
}

/**

\fn	BOOL AnimatBase::RemoveItem(string strItemType, string strID, BOOL bThrowError)

\brief	Removes a child item from this parent. 

\details This is a generic method that is used to delete a child object from this parent. The GUI
uses this method to remove objects from the simulation that have been deleted in the UI. The item type
lets the method determine what type of item is being deleted, like synapse, neuron, body part, etc..
The ID is then used to delete that specific item.

\author	dcofer
\date	2/22/2011

\param	strItemType	String descriptor of the type of item that is being created. 
\param	strID		Unique ID of the item that will be removed. 
\param	bThrowError	If true then throw an error if there is a problem, otherwise return false

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

/**

\fn	void AnimatBase::Load(CStdXml &oXml)

\brief	Loads this object. 

\details This uses the Xml packet to load in the data for this object.

\author	dcofer
\date	2/22/2011

\param [in,out]	oXml	The CStdXml xml utility. 
**/
void AnimatBase::Load(CStdXml &oXml)
{
	oXml.IntoElem();
	m_strType = oXml.GetChildString("Type", m_strType);
	m_strID = Std_CheckString(oXml.GetChildString("ID", m_strID));
	m_strName = oXml.GetChildString("Name", m_strName);
	oXml.OutOfElem();

	if(Std_IsBlank(m_strID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	if(Std_IsBlank(m_strName)) 
		m_strName = m_strID;

	//This will add this object to the object list of the simulation.
	Simulator *lpSim = GetSimulator();
	if(lpSim)
		lpSim->AddToObjectList(this);
}


}			//AnimatSim
