/**
\file	BodyPart.cpp

\brief	Implements the body part class. 
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
\fn	BodyPart::BodyPart(void)

\brief	Default constructor. 

\author	dcofer
\date	3/2/2011
**/
BodyPart::BodyPart(void)
{
	m_bIsVisible = TRUE;
	m_lpCallback = NULL;
	m_lpStructure = NULL;
	m_lpParent = NULL;
	m_lpPhysicsBody = NULL;

	m_fltGraphicsAlpha = 1;
	m_fltCollisionsAlpha = 1;
	m_fltJointsAlpha = 1;
	m_fltReceptiveFieldsAlpha = 1;
	m_fltSimulationAlpha = 1;
	m_fltAlpha = 1;
	m_fltGripScale = 1;
}

/**
\fn	BodyPart::~BodyPart(void)

\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
BodyPart::~BodyPart(void)
{
}

#pragma region AccessorMutators

/**
\fn	RigidBody *BodyPart::Parent()

\brief	Gets the parent RigidBody of this part. 

\author	dcofer
\date	3/2/2011

\return	Pointer to the parent RigidBody of this part, NULL if not set. 
**/
RigidBody *BodyPart::Parent() {return m_lpParent;}

/**
\fn	void BodyPart::Parent(RigidBody *lpValue)

\brief	Sets the parent RigidBody of this part. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpValue	The pointer to the parent RigidBody. 
**/
void BodyPart::Parent(RigidBody *lpValue) {m_lpParent = lpValue;}

/**
\fn	int BodyPart::VisualSelectionType()

\brief	Gets the visual selection type for this part. 

\details In the GUI the user can select several different types of visual selection modes
When a user selects an object while in a given selection mode we need to be able to check 
each object that is in the path of the click to see if it should be selected. So we check
the current selection mode with this VisualSelectionType property to see if they match. So
if the current VisualSelectionMode is Joints, and this body part is a joint then it will
have a matching VisualSelectionType property and we will no it should be selected, but if
a rigid body was selected its VisualSelectionType will be different, so we will not select it.

\author	dcofer
\date	3/2/2011

\return	. 
**/
int BodyPart::VisualSelectionType() {return 0;}

/**
\fn	BOOL BodyPart::AllowMouseManipulation()

\brief	Tells if a given part can be manipulated using the mouse and draggers. 

\author	dcofer
\date	3/2/2011

\return	true if it can be manipulated, false if not.
**/
BOOL BodyPart::AllowMouseManipulation() {return TRUE;}

/**
\fn	CStdFPoint BodyPart::LocalPosition()

\brief	Gets the local position. (m_oLocalPosition) 

\author	dcofer
\date	3/2/2011

\return	returns m_oLocalPosition. 
**/
CStdFPoint BodyPart::LocalPosition() {return m_oLocalPosition;}

/**
\fn	void BodyPart::LocalPosition(CStdFPoint &oPoint, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix)

\brief	Sets the local position. (m_oLocalPosition) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint		The new point to use to set the local position. 
\param	bUseScaling			If true then the position values that are passed in 
will be scaled by the unit scaling values. 
\param	bFireChangeEvent	If true then this will call the IBodyPartCallback->PositionChanged callback 
method to inform the GUI that the part has moved. If false then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsBody->Physics_UpdateMatrix method will be called
so that the osg graphics will be updated. If false then this will be skipped.
**/
void BodyPart::LocalPosition(CStdFPoint &oPoint, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	if(bUseScaling)
		m_oLocalPosition = oPoint * m_lpSim->InverseDistanceUnits();
	else
		m_oLocalPosition = oPoint;
	m_oReportLocalPosition = m_oLocalPosition * m_lpSim->DistanceUnits();
	

	if(m_lpPhysicsBody && bUpdateMatrix)
		m_lpPhysicsBody->Physics_UpdateMatrix();

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->PositionChanged();
}

/**
\fn	void BodyPart::LocalPosition(float fltX, float fltY, float fltZ, BOOL bUseScaling,
BOOL bFireChangeEvent, BOOL bUpdateMatrix)

\brief	Sets the local position. (m_oLocalPosition) 

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bUseScaling			If true then the position values that are passed in 
will be scaled by the unit scaling values. 
\param	bFireChangeEvent	If true then this will call the IBodyPartCallback->PositionChanged callback 
method to inform the GUI that the part has moved. If false then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsBody->Physics_UpdateMatrix method will be called
so that the osg graphics will be updated. If false then this will be skipped.
**/
void BodyPart::LocalPosition(float fltX, float fltY, float fltZ, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	LocalPosition(vPos, bUseScaling, bFireChangeEvent);
}

/**
\fn	void BodyPart::LocalPosition(string strXml, BOOL bUseScaling, BOOL bFireChangeEvent,
BOOL bUpdateMatrix)

\brief	Sets the local position. (m_oLocalPosition). This method is primarily used by the GUI
to reset the local position using an xml data packet.

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the position. 
\param	bUseScaling			If true then the position values that are passed in 
will be scaled by the unit scaling values. 
\param	bFireChangeEvent	If true then this will call the IBodyPartCallback->PositionChanged callback 
method to inform the GUI that the part has moved. If false then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsBody->Physics_UpdateMatrix method will be called
so that the osg graphics will be updated. If false then this will be skipped.
**/
void BodyPart::LocalPosition(string strXml, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("LocalPosition");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "LocalPosition", vPos);
	LocalPosition(vPos, bUseScaling, bFireChangeEvent);
}

/**
\fn	CStdFPoint BodyPart::AbsolutePosition()

\brief	Gets the absolute position of this body part. 

\author	dcofer
\date	3/2/2011

\return	return m_oAbsPosition. 
**/
CStdFPoint BodyPart::AbsolutePosition() {return m_oAbsPosition;}

/**
\fn	void BodyPart::AbsolutePosition(CStdFPoint &oPoint)

\brief	Sets the absolute position of this body part. (m_oAbsPosition)

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new position. 
**/
void BodyPart::AbsolutePosition(CStdFPoint &oPoint) {m_oAbsPosition = oPoint;}

/**
\fn	void BodyPart::AbsolutePosition(float fltX, float fltY, float fltZ)

\brief	Sets the absolute position of this body part. (m_oAbsPosition)

\author	dcofer
\date	3/2/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void BodyPart::AbsolutePosition(float fltX, float fltY, float fltZ) {m_oAbsPosition.Set(fltX, fltY, fltZ);}

/**
\fn	CStdFPoint BodyPart::GetCurrentPosition()

\brief	Gets the current position of this part. 

\author	dcofer
\date	3/2/2011

\return	The current position. 
**/

CStdFPoint BodyPart::GetCurrentPosition() {return m_oAbsPosition;}

/**
\fn	CStdFPoint BodyPart::ReportLocalPosition()

\brief	Gets the reported local position. (m_oReportLocalPosition).

\author	dcofer
\date	3/2/2011

\return	Returns m_oReportLocalPosition. 
**/
CStdFPoint BodyPart::ReportLocalPosition() {return m_oReportLocalPosition;}

/**
\fn	void BodyPart::ReportLocalPosition(CStdFPoint &oPoint)

\brief	Sets the reported local position. (m_oReportLocalPosition).

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new position. 
**/
void BodyPart::ReportLocalPosition(CStdFPoint &oPoint) {m_oReportLocalPosition = oPoint;}

/**
\fn	void BodyPart::ReportLocalPosition(float fltX, float fltY, float fltZ)

\brief	Sets the reported local position. (m_oReportLocalPosition).

\author	dcofer
\date	3/2/2011

\param	fltX	The  coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void BodyPart::ReportLocalPosition(float fltX, float fltY, float fltZ) {m_oReportLocalPosition.Set(fltX, fltY, fltZ);}

/**
\fn	CStdFPoint BodyPart::ReportWorldPosition()

\brief	Gets the reported world position. (m_oReportWorldPosition)

\author	dcofer
\date	3/2/2011

\return	Returns the reported world position. (m_oReportWorldPosition) 
**/
CStdFPoint BodyPart::ReportWorldPosition() {return m_oReportWorldPosition;}

/**
\fn	void BodyPart::ReportWorldPosition(CStdFPoint &oPoint)

\brief	Sets the reported world position (m_oReportWorldPosition)

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new position. 
**/
void BodyPart::ReportWorldPosition(CStdFPoint &oPoint) {m_oReportWorldPosition = oPoint;}

/**
\fn	void BodyPart::ReportWorldPosition(float fltX, float fltY, float fltZ)

\brief	Sets the reported world position (m_oReportWorldPosition)

\author	dcofer
\date	3/2/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void BodyPart::ReportWorldPosition(float fltX, float fltY, float fltZ) {m_oReportWorldPosition.Set(fltX, fltY, fltZ);}

/**
\fn	CStdFPoint BodyPart::Rotation()

\brief	Gets the rotation of this body in radians. 

\author	dcofer
\date	3/2/2011

\return	rotation of body. (m_oRotation)
**/
CStdFPoint BodyPart::Rotation()	{return m_oRotation;}

/**
\fn	void BodyPart::Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent, BOOL bUpdateMatrix)

\brief	Sets the rotation of this body in radians. (m_oRotation)

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint		The new rotation values. 
\param	bFireChangeEvent	If true then this will call the IBodyPartCallback->RotationChanged callback 
method to inform the GUI that the part has moved. If false then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsBody->Physics_UpdateMatrix method will be called
so that the osg graphics will be updated. If false then this will be skipped.
**/
void BodyPart::Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	m_oRotation = oPoint;
	m_oReportRotation = m_oRotation;

	if(m_lpPhysicsBody && bUpdateMatrix)
		m_lpPhysicsBody->Physics_UpdateMatrix();

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->RotationChanged();
}

/**
\fn	void BodyPart::Rotation(float fltX, float fltY, float fltZ, BOOL bFireChangeEvent, BOOL bUpdateMatrix)

\brief	Sets the rotation of this body in radians. (m_oRotation)

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bFireChangeEvent	If true then this will call the IBodyPartCallback->RotationChanged callback 
method to inform the GUI that the part has moved. If false then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsBody->Physics_UpdateMatrix method will be called
so that the osg graphics will be updated. If false then this will be skipped.
**/
void BodyPart::Rotation(float fltX, float fltY, float fltZ, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	Rotation(vPos, bFireChangeEvent);
}

/**
\fn	void BodyPart::Rotation(string strXml, BOOL bFireChangeEvent, BOOL bUpdateMatrix)

\brief	Sets the rotation of this body in radians. (m_oRotation). This method is primarily used by the GUI
to reset the rotation using an xml data packet

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string used to load the rotation. 
\param	bFireChangeEvent	If true then this will call the IBodyPartCallback->RotationChanged callback 
method to inform the GUI that the part has moved. If false then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsBody->Physics_UpdateMatrix method will be called
so that the osg graphics will be updated. If false then this will be skipped.
**/
void BodyPart::Rotation(string strXml, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Rotation");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Rotation", vPos);
	Rotation(vPos, bFireChangeEvent);
}

/**
\fn	CStdFPoint BodyPart::ReportRotation()

\brief	Gets the reported rotation of this part. (m_oReportRotation)

\author	dcofer
\date	3/2/2011

\return	Returns the reported rotation. 
**/
CStdFPoint BodyPart::ReportRotation() {return m_oReportRotation;}

/**
\fn	void BodyPart::ReportRotation(CStdFPoint &oPoint)

\brief	Sets the reported rotation of this part. (m_oReportRotation)

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new reported rotation. 
**/
void BodyPart::ReportRotation(CStdFPoint &oPoint) {m_oReportRotation = oPoint;}

/**
\fn	void BodyPart::ReportRotation(float fltX, float fltY, float fltZ)

\brief	Sets the reported rotation of this part. (m_oReportRotation)

\author	dcofer
\date	3/2/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void BodyPart::ReportRotation(float fltX, float fltY, float fltZ) {m_oReportRotation.Set(fltX, fltY, fltZ);}

/**
\fn	BOOL BodyPart::IsVisible()

\brief	Query if this object is visible. 

\author	dcofer
\date	3/2/2011

\return	true if visible, false if not. 
**/
BOOL BodyPart::IsVisible() {return m_bIsVisible;}

/**
\fn	void BodyPart::IsVisible(BOOL bVal)

\brief	Sets whether this part is visible or not. 

\author	dcofer
\date	3/2/2011

\param	bVal	true to make visible, false to make invisible. 
**/
void BodyPart::IsVisible(BOOL bVal) 
{
	m_bIsVisible = bVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetVisible(m_bIsVisible);
}

/**
\fn	float BodyPart::GraphicsAlpha()

\brief	Gets the graphics alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float BodyPart::GraphicsAlpha() {return m_fltGraphicsAlpha;}

/**
\fn	void BodyPart::GraphicsAlpha(float fltVal)

\brief	Sets the graphics alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new GraphicsAlpha value between 0 and 1.
\exception If value not between 0 and 1.
**/
void BodyPart::GraphicsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "GraphicsAlpha");

	m_fltGraphicsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

/**
\fn	float BodyPart::CollisionsAlpha()

\brief	Gets the collisions alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float BodyPart::CollisionsAlpha() {return m_fltCollisionsAlpha;}

/**
\fn	void BodyPart::CollisionsAlpha(float fltVal)

\brief	Sets the collisions alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new CollisionsAlpha value between 0 and 1.
\exception If value not between 0 and 1.
**/
void BodyPart::CollisionsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "CollisionsAlpha");

	m_fltCollisionsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

/**
\fn	float BodyPart::JointsAlpha()

\brief	Gets the joints alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float BodyPart::JointsAlpha() {return m_fltJointsAlpha;}

/**
\fn	void BodyPart::JointsAlpha(float fltVal)

\brief	Sets the joints alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new JointsAlpha value between 0 and 1.
\exception If value not between 0 and 1.
**/
void BodyPart::JointsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "JointsAlpha");

	m_fltJointsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

/**
\fn	float BodyPart::ReceptiveFieldsAlpha()

\brief	Gets the receptive fields alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float BodyPart::ReceptiveFieldsAlpha() {return m_fltReceptiveFieldsAlpha;}

/**
\fn	void BodyPart::ReceptiveFieldsAlpha(float fltVal)

\brief	Sets the receptive fields alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new ReceptiveFieldsAlpha value between 0 and 1.
\exception If value not between 0 and 1.
**/
void BodyPart::ReceptiveFieldsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "ReceptiveFieldsAlpha");

	m_fltReceptiveFieldsAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

/**
\fn	float BodyPart::SimulationAlpha()

\brief	Gets the simulation alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float BodyPart::SimulationAlpha() {return m_fltSimulationAlpha;}

/**
\fn	void BodyPart::SimulationAlpha(float fltVal)

\brief	Sets the simulation alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new SimulationAlpha value between 0 and 1.
\exception If value not between 0 and 1.
**/
void BodyPart::SimulationAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "SimulationAlpha");

	m_fltSimulationAlpha = fltVal;

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

/**
\fn	float BodyPart::Alpha()

\brief	Gets the current alpha. 

\details The current alpha is on of the various selection mode alphas like JointsAlpha.
This is just the one that is currently selected.

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float BodyPart::Alpha() {return m_fltAlpha;}

/**
\fn	void BodyPart::Alpha(float fltAlpha)

\brief	Sets the current alpha. 

\author	dcofer
\date	3/2/2011

\param	fltAlpha	The new Alpha value between 0 and 1.
\exception If value not between 0 and 1.
**/
void BodyPart::Alpha(float fltAlpha) 
{
	Std_InValidRange((float) 0, (float) 1, fltAlpha, TRUE, "Alpha");

	m_fltAlpha = fltAlpha;
}

/**
\fn	float BodyPart::GripScale()

\brief	Gets the grip scale. 

\author	dcofer
\date	3/2/2011

\return	float scale of the grip. 
**/
float BodyPart::GripScale() {return m_fltGripScale;}

/**
\fn	void BodyPart::GripScale(float fltScale)

\brief	Sets the grip scale. 

\author	dcofer
\date	3/2/2011

\param	fltScale	The new scale value. 
**/
void BodyPart::GripScale(float fltScale) {m_fltGripScale = fltScale;}

/**
\fn	IBodyPartCallback *BodyPart::Callback()

\brief	Gets the callback interface pointer. This is an interface pointer to a 
callback class that allows us to notify the GUI of events that occur within the simulation.

\author	dcofer
\date	3/2/2011

\return	Pointer to callback object if one exists, NULL else.
**/
IBodyPartCallback *BodyPart::Callback() {return m_lpCallback;}

/**
\fn	void BodyPart::Callback(IBodyPartCallback *lpCallback)

\brief	Sets the callback interface pointer. This is an interface pointer to a 
callback class that allows us to notify the GUI of events that occur within the simulation.
 
\author	dcofer
\date	3/2/2011

\param [in,out]	lpCallback	The pointer to a callback interface object. 
**/
void BodyPart::Callback(IBodyPartCallback *lpCallback) {m_lpCallback = lpCallback;}

/**
\fn	IPhysicsBody *BodyPart::PhysicsBody()

\brief	Gets the physics body interface pointer. This is an interface reference 
to the Vs version of this object. It will allow us to call methods directly in the 
Vs (OSG) version of the object directly without having to overload a bunch of methods in each box, sphere, etc..

\author	dcofer
\date	3/2/2011

\return	Pointer to Vs interface, NULL else.
**/
IPhysicsBody *BodyPart::PhysicsBody() {return m_lpPhysicsBody;}

/**
\fn	void BodyPart::PhysicsBody(IPhysicsBody *lpBody)

\brief	Sets the physics body interface pointer. This is an interface reference 
to the Vs version of this object. It will allow us to call methods directly in the 
Vs (OSG) version of the object directly without having to overload a bunch of methods in each box, sphere, etc..

\author	dcofer
\date	3/2/2011

\param [in,out]	lpBody	The pointer to the phsyics body interface. 
**/
void BodyPart::PhysicsBody(IPhysicsBody *lpBody) {m_lpPhysicsBody = lpBody;}

/**
\fn	float BodyPart::GetBoundingRadius()

\brief	Gets the bounding radius of this part. 

\author	dcofer
\date	3/2/2011

\return	The bounding radius. 
**/
float BodyPart::GetBoundingRadius()
{
	if(m_lpPhysicsBody)
		return m_lpPhysicsBody->Physics_GetBoundingRadius();
	else
		return 1;
}

/**
\fn	void BodyPart::Resize()

\brief	Called when this object has been resized.

\details This method is called when an item is resized. It is overloaded in the
derived class and allows that child class to perform any necessary graphics/physics
calls for the resize event.

\author	dcofer
\date	3/2/2011
**/
void BodyPart::Resize() {};

#pragma endregion

void BodyPart::Selected(BOOL bValue, BOOL bSelectMultiple)
{
	Node::Selected(bValue, bSelectMultiple);

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_Selected(bValue, bSelectMultiple);

	if(m_lpCallback)
		m_lpCallback->SelectionChanged(bValue, bSelectMultiple);
}

/**
\fn	void BodyPart::VisualSelectionModeChanged(int iNewMode)

\brief	Called when the visual selection mode changed in GUI. 

\details In the GUI the user can select several different types of visual selection modes
This method is called any time that the user switches the selection mode in the GUI. This 
allows us to change the current Alpha value of the objects so the display is correct.

\author	dcofer
\date	3/2/2011

\param	iNewMode	The new VisualSelectionMode. 
**/
void BodyPart::VisualSelectionModeChanged(int iNewMode)
{
	Node::VisualSelectionModeChanged(iNewMode);

	if(m_lpPhysicsBody)
		m_lpPhysicsBody->SetAlpha();
}

/**
\fn	void BodyPart::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)

\brief	Called when the user clicks on this object while the AddBody mode is active. 

\details When the user selects the AddBody mode in the GUI then the simulation detects when a part is clicked
in the simulation window. (This occurs in the VsCameraManipulator class). It gets the position of the click in
global coordinates, and the normal vector for the surface that was clicked. We then need to pass this info back 
up the GUI and let it know the click occurred. This method uses the IBodyPartCallback object to send this info
back up to the GUI.

\author	dcofer
\date	3/2/2011

\param	fltPosX		The position x coordinate. 
\param	fltPosY		The position y coordinate. 
\param	fltPosZ		The position z coordinate. 
\param	fltNormX	The normal x coordinate. 
\param	fltNormY	The normal y coordinate. 
\param	fltNormZ	The normal z coordinate. 
**/
void BodyPart::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)
{
	if(m_lpCallback)
		m_lpCallback->AddBodyClicked(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ);
}

/**
\fn	void BodyPart::UpdateData()

\brief	Called to collect any body data for this part. 

\author	dcofer
\date	3/2/2011
**/
void BodyPart::UpdateData()
{
	if(m_lpPhysicsBody)
		m_lpPhysicsBody->Physics_CollectBodyData();
}


#pragma region DataAccesMethods

float *BodyPart::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "BODYPOSITIONX" || strType == "POSITIONX"|| strType == "WORLDPOSITIONX")
		return &m_oReportWorldPosition.x;

	if(strType == "BODYPOSITIONY" || strType == "POSITIONY"|| strType == "WORLDPOSITIONY")
		return &m_oReportWorldPosition.y;

	if(strType == "BODYPOSITIONZ" || strType == "POSITIONZ"|| strType == "WORLDPOSITIONZ")
		return &m_oReportWorldPosition.z;

	if(strType == "LOCALPOSITIONX")
		return &m_oReportLocalPosition.x;

	if(strType == "LOCALPOSITIONY")
		return &m_oReportLocalPosition.y;

	if(strType == "LOCALPOSITIONZ")
		return &m_oReportLocalPosition.z;

	if(strType == "BODYROTATIONX" || strType == "ROTATIONX")
		return &m_oReportRotation.x;

	if(strType == "BODYROTATIONY" || strType == "ROTATIONY")
		return &m_oReportRotation.y;

	if(strType == "BODYROTATIONZ" || strType == "ROTATIONZ")
		return &m_oReportRotation.z;

	return 0;
}

BOOL BodyPart::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(Node::SetData(strDataType, strValue, FALSE))
		return true;

	if(strDataType == "LOCALPOSITION")
	{
		LocalPosition(strValue);
		return true;
	}

	if(strDataType == "ROTATION")
	{
		Rotation(strValue);
		return true;
	}

	if(strDataType == "VISIBLE")
	{
		IsVisible(Std_ToBool(strValue));
		return true;
	}

	if(strDataType == "GRAPHICSALPHA")
	{
		GraphicsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "COLLISIONALPHA")
	{
		CollisionsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "JOINTSALPHA")
	{
		JointsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "RECEPTIVEFIELDSALPHA")
	{
		ReceptiveFieldsAlpha(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "SIMULATIONALPHA")
	{
		SimulationAlpha(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void BodyPart::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	if(oXml.FindChildElement("LocalPosition", FALSE))
		Std_LoadPoint(oXml, "LocalPosition", m_oLocalPosition);
	else
		m_oLocalPosition.Set(0, 0, 0);
	m_oLocalPosition *= m_lpSim->InverseDistanceUnits();

	if(!m_lpParent)
		m_oAbsPosition += m_oLocalPosition;
	else
		m_oAbsPosition = m_lpParent->AbsolutePosition() + m_oLocalPosition;

	m_oReportLocalPosition = m_oLocalPosition * m_lpSim->DistanceUnits();
	m_oReportWorldPosition = m_oAbsPosition * m_lpSim->DistanceUnits();

	if(oXml.FindChildElement("Rotation", FALSE))
		Std_LoadPoint(oXml, "Rotation", m_oRotation);
	else
		m_oRotation.Set(0, 0, 0);
	m_oReportRotation = m_oRotation;

	m_bIsVisible = oXml.GetChildBool("IsVisible", m_bIsVisible);
	m_fltGraphicsAlpha = oXml.GetChildFloat("GraphicsAlpha", m_fltGraphicsAlpha);
	m_fltCollisionsAlpha = oXml.GetChildFloat("CollisionsAlpha", m_fltCollisionsAlpha);
	m_fltJointsAlpha = oXml.GetChildFloat("JointsAlpha", m_fltJointsAlpha);
	m_fltReceptiveFieldsAlpha = oXml.GetChildFloat("ReceptiveFieldsAlpha", m_fltReceptiveFieldsAlpha);
	m_fltSimulationAlpha = oXml.GetChildFloat("SimulationAlpha", m_fltSimulationAlpha);

	oXml.OutOfElem(); //OutOf RigidBody Element
}

	}			//Environment
}				//AnimatSim