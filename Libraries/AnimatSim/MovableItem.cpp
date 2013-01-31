/**
\file	MovableItem.cpp

\brief	Implements the body part class. 
**/

#include "stdafx.h"
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
\date	3/2/2011
**/
MovableItem::MovableItem(void)
{
	m_bIsVisible = TRUE;
	m_lpCallback = NULL;
	m_lpParent = NULL;
	m_lpPhysicsMovableItem = NULL;
	m_lpMovableSim = NULL;

	m_fltGraphicsAlpha = 1;
	m_fltCollisionsAlpha = 1;
	m_fltJointsAlpha = 1;
	m_fltReceptiveFieldsAlpha = 1;
	m_fltSimulationAlpha = 1;
	m_fltAlpha = 1;

	m_vAmbient.Set(0.1f, 0.1f, 0.1f, 1);
	m_vDiffuse.Set(1, 0, 0, 1);
	m_vSpecular.Set(0.25f, 0.25f, 0.25f, 1);
	m_fltShininess = 64;
	m_fltUserDefinedDraggerRadius = -1;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
MovableItem::~MovableItem(void)
{
}

#pragma region AccessorMutators

/**
\brief	Gets the parent RigidBody of this part. 

\author	dcofer
\date	3/2/2011

\return	Pointer to the parent RigidBody of this part, NULL if not set. 
**/
RigidBody *MovableItem::Parent() {return m_lpParent;}

/**
\brief	Sets the parent RigidBody of this part. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpValue	The pointer to the parent RigidBody. 
**/
void MovableItem::Parent(RigidBody *lpValue) 
{
	m_lpParent = lpValue;
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_SetParent(lpValue);
}

/**
\brief	Gets the visual selection type for this part.

\details In the GUI the user can select several different types of visual selection modes When a
user selects an object while in a given selection mode we need to be able to check each object
that is in the path of the click to see if it should be selected. So we check the current
selection mode with this VisualSelectionType property to see if they match. So if the current
VisualSelectionMode is Joints, and this body part is a joint then it will have a matching
VisualSelectionType property and we will no it should be selected, but if a rigid body was
selected its VisualSelectionType will be different, so we will not select it. 

\author	dcofer
\date	3/2/2011

\return	. 
**/
int MovableItem::VisualSelectionType() {return 0;}

/**
\brief	Tells if a given part can be manipulated using the mouse and draggers. 

\author	dcofer
\date	3/2/2011

\return	true if it can be manipulated, false if not. 
**/
BOOL MovableItem::AllowMouseManipulation() {return TRUE;}

/**
\brief	Gets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
CStdFPoint MovableItem::Position() {return m_oPosition;}

/**
\brief	Sets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint		The new point to use to set the local position. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->PositionChanged
							callback method to inform the GUI that the part has moved. If false
							then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void MovableItem::Position(CStdFPoint &oPoint, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	if(bUseScaling)
		m_oPosition = oPoint * m_lpMovableSim->InverseDistanceUnits();
	else
		m_oPosition = oPoint;
	m_oReportPosition = m_oPosition * m_lpMovableSim->DistanceUnits();

	if(m_lpPhysicsMovableItem && bUpdateMatrix)
		m_lpPhysicsMovableItem->Physics_PositionChanged();

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->PositionChanged();

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_UpdateAbsolutePosition();
}

/**
\brief	Sets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->PositionChanged
							callback method to inform the GUI that the part has moved. If false
							then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void MovableItem::Position(float fltX, float fltY, float fltZ, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	Position(vPos, bUseScaling, bFireChangeEvent);
}

/**
\brief	Sets the local position. (m_oPosition). This method is primarily used by the GUI to
reset the local position using an xml data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the position. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->PositionChanged
							callback method to inform the GUI that the part has moved. If false
							then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void MovableItem::Position(string strXml, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Position");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Position", vPos);
	Position(vPos, bUseScaling, bFireChangeEvent);
}

/**
\brief	Gets the absolute position of this body part. 

\author	dcofer
\date	3/2/2011

\return	return m_oAbsPosition. 
**/
CStdFPoint MovableItem::AbsolutePosition() 
{
	if(!m_lpPhysicsMovableItem)
		THROW_ERROR(Al_Err_lAttempedToReadAbsPosBeforeDefined, Al_Err_strAttempedToReadAbsPosBeforeDefined);

	return m_oAbsPosition;
}

/**
\brief	Sets the absolute position of this body part. (m_oAbsPosition) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new position. 
**/
void MovableItem::AbsolutePosition(CStdFPoint &oPoint) 
{
	m_oAbsPosition = oPoint;
	m_oReportWorldPosition = m_oAbsPosition * m_lpMovableSim->DistanceUnits();
}

/**
\brief	Sets the absolute position of this body part. (m_oAbsPosition) 

\author	dcofer
\date	3/2/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void MovableItem::AbsolutePosition(float fltX, float fltY, float fltZ) 
{
	m_oAbsPosition.Set(fltX, fltY, fltZ);
	m_oReportWorldPosition = m_oAbsPosition * m_lpMovableSim->DistanceUnits();
}

/**
\brief	Gets the current position of this part. 

\author	dcofer
\date	3/2/2011

\return	The current position. 
**/
CStdFPoint MovableItem::GetCurrentPosition() {return m_oAbsPosition;}

CStdFPoint  MovableItem::UpdateAbsolutePosition()
{
	if(!m_lpPhysicsMovableItem)
		THROW_ERROR(Al_Err_lAttempedToReadAbsPosBeforeDefined, Al_Err_strAttempedToReadAbsPosBeforeDefined);

	m_lpPhysicsMovableItem->Physics_UpdateAbsolutePosition();

	return AbsolutePosition();
}

/**
\brief	Gets the reported local position. (m_oReportPosition). 

\author	dcofer
\date	3/2/2011

\return	Returns m_oReportPosition. 
**/
CStdFPoint MovableItem::ReportPosition() {return m_oReportPosition;}

/**
\brief	Sets the reported local position. (m_oReportPosition). 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new position. 
**/
void MovableItem::ReportPosition(CStdFPoint &oPoint) {m_oReportPosition = oPoint;}

/**
\brief	Sets the reported local position. (m_oReportPosition). 

\author	dcofer
\date	3/2/2011

\param	fltX	The  coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void MovableItem::ReportPosition(float fltX, float fltY, float fltZ) {m_oReportPosition.Set(fltX, fltY, fltZ);}

/**
\brief	Gets the reported world position. (m_oReportWorldPosition) 

\author	dcofer
\date	3/2/2011

\return	Returns the reported world position. (m_oReportWorldPosition) 
**/
CStdFPoint MovableItem::ReportWorldPosition() {return m_oReportWorldPosition;}

/**
\brief	Sets the reported world position (m_oReportWorldPosition) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new position. 
**/
void MovableItem::ReportWorldPosition(CStdFPoint &oPoint) {m_oReportWorldPosition = oPoint;}

/**
\brief	Sets the reported world position (m_oReportWorldPosition) 

\author	dcofer
\date	3/2/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void MovableItem::ReportWorldPosition(float fltX, float fltY, float fltZ) {m_oReportWorldPosition.Set(fltX, fltY, fltZ);}

/**
\brief	Gets the rotation of this body in radians. 

\author	dcofer
\date	3/2/2011

\return	rotation of body. (m_oRotation) 
**/
CStdFPoint MovableItem::Rotation()	{return m_oRotation;}

/**
\brief	Sets the rotation of this body in radians. (m_oRotation) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint		The new rotation values. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->RotationChanged
							callback method to inform the GUI that the part has moved. If false
							then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void MovableItem::Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	m_oRotation = oPoint;
	m_oReportRotation = m_oRotation;

	if(m_lpPhysicsMovableItem && bUpdateMatrix)
		m_lpPhysicsMovableItem->Physics_RotationChanged();

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->RotationChanged();
}

/**
\brief	Sets the rotation of this body in radians. (m_oRotation) 

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->RotationChanged
							callback method to inform the GUI that the part has moved. If false
							then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void MovableItem::Rotation(float fltX, float fltY, float fltZ, BOOL bFireChangeEvent, BOOL bUpdateMatrix) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	Rotation(vPos, bFireChangeEvent);
}

/**
\brief	Sets the rotation of this body in radians. (m_oRotation). This method is primarily used
by the GUI to reset the rotation using an xml data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string used to load the rotation. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->RotationChanged
							callback method to inform the GUI that the part has moved. If false
							then this callback will be skipped. 
\param	bUpdateMatrix		If true then the IPhysicsMovableItem->Physics_UpdateMatrix method will be
							called so that the osg graphics will be updated. If false then this
							will be skipped. 
**/
void MovableItem::Rotation(string strXml, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
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
\brief	Gets the reported rotation of this part. (m_oReportRotation) 

\author	dcofer
\date	3/2/2011

\return	Returns the reported rotation. 
**/
CStdFPoint MovableItem::ReportRotation() {return m_oReportRotation;}

/**
\brief	Sets the reported rotation of this part. (m_oReportRotation) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint	The new reported rotation. 
**/
void MovableItem::ReportRotation(CStdFPoint &oPoint) {m_oReportRotation = oPoint;}

/**
\brief	Sets the reported rotation of this part. (m_oReportRotation) 

\author	dcofer
\date	3/2/2011

\param	fltX	The x coordinate. 
\param	fltY	The y coordinate. 
\param	fltZ	The z coordinate. 
**/
void MovableItem::ReportRotation(float fltX, float fltY, float fltZ) {m_oReportRotation.Set(fltX, fltY, fltZ);}

/**
\brief	Query if this object is visible. 

\author	dcofer
\date	3/2/2011

\return	true if visible, false if not. 
**/
BOOL MovableItem::IsVisible() {return m_bIsVisible;}

/**
\brief	Sets whether this part is visible or not. 

\author	dcofer
\date	3/2/2011

\param	bVal	true to make visible, false to make invisible. 
**/
void MovableItem::IsVisible(BOOL bVal) 
{
	m_bIsVisible = bVal;

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->SetVisible(m_bIsVisible);
}

/**
\brief	Gets the graphics alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float MovableItem::GraphicsAlpha() {return m_fltGraphicsAlpha;}

/**
\brief	Sets the graphics alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new GraphicsAlpha value between 0 and 1. 
\exception	If	value not between 0 and 1. 
**/
void MovableItem::GraphicsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "GraphicsAlpha");

	m_fltGraphicsAlpha = fltVal;

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->SetAlpha();
}

/**
\brief	Gets the collisions alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float MovableItem::CollisionsAlpha() {return m_fltCollisionsAlpha;}

/**
\brief	Sets the collisions alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new CollisionsAlpha value between 0 and 1. 
\exception	If	value not between 0 and 1. 
**/
void MovableItem::CollisionsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "CollisionsAlpha");

	m_fltCollisionsAlpha = fltVal;

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->SetAlpha();
}

/**
\brief	Gets the joints alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float MovableItem::JointsAlpha() {return m_fltJointsAlpha;}

/**
\brief	Sets the joints alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new JointsAlpha value between 0 and 1. 
\exception	If	value not between 0 and 1. 
**/
void MovableItem::JointsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "JointsAlpha");

	m_fltJointsAlpha = fltVal;

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->SetAlpha();
}

/**
\brief	Gets the receptive fields alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float MovableItem::ReceptiveFieldsAlpha() {return m_fltReceptiveFieldsAlpha;}

/**
\brief	Sets the receptive fields alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new ReceptiveFieldsAlpha value between 0 and 1. 
\exception	If	value not between 0 and 1. 
**/
void MovableItem::ReceptiveFieldsAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "ReceptiveFieldsAlpha");

	m_fltReceptiveFieldsAlpha = fltVal;

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->SetAlpha();
}

/**
\brief	Gets the simulation alpha. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float MovableItem::SimulationAlpha() {return m_fltSimulationAlpha;}

/**
\brief	Sets the simulation alpha. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new SimulationAlpha value between 0 and 1. 
\exception	If	value not between 0 and 1. 
**/
void MovableItem::SimulationAlpha(float fltVal) 
{
	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "SimulationAlpha");

	m_fltSimulationAlpha = fltVal;

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->SetAlpha();
}

/**
\brief	Gets the current alpha.

\details The current alpha is on of the various selection mode alphas like JointsAlpha. This is
just the one that is currently selected. 

\author	dcofer
\date	3/2/2011

\return	float alpha between 0 and 1. 
**/
float MovableItem::Alpha() {return m_fltAlpha;}

/**
\brief	Sets the current alpha. 

\author	dcofer
\date	3/2/2011

\param	fltAlpha	The new Alpha value between 0 and 1. 
\exception	If	value not between 0 and 1. 
**/
void MovableItem::Alpha(float fltAlpha) 
{
	Std_InValidRange((float) 0, (float) 1, fltAlpha, TRUE, "Alpha");

	m_fltAlpha = fltAlpha;
}

/**
\brief	Gets the ambient color value. 

\author	dcofer
\date	3/2/2011

\return	Pointer to color data
**/
CStdColor *MovableItem::Ambient() {return &m_vAmbient;}

/**
\brief	Sets the Ambient color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void MovableItem::Ambient(CStdColor &aryColor)
{
	m_vAmbient = aryColor;
	if(m_lpPhysicsMovableItem) m_lpPhysicsMovableItem->Physics_SetColor();
}

/**
\brief	Sets the Ambient color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void MovableItem::Ambient(float *aryColor)
{
	CStdColor vColor(aryColor[0], aryColor[1], aryColor[2], aryColor[3], 1);
	Ambient(vColor);
}

/**
\brief	Loads the Ambient color from an XML data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/

void MovableItem::Ambient(string strXml)
{
	CStdColor vColor(1);
	vColor.Load(strXml, "Color");
	Ambient(vColor);
}

/**
\brief	Gets the diffuse color. 

\author	dcofer
\date	3/2/2011

\return	Pointer to color data
**/
CStdColor *MovableItem::Diffuse() {return &m_vDiffuse;}

/**
\brief	Sets the Diffuse color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void MovableItem::Diffuse(CStdColor &aryColor)
{
	m_vDiffuse = aryColor;
	if(m_lpPhysicsMovableItem) m_lpPhysicsMovableItem->Physics_SetColor();
}

/**
\brief	Sets the Diffuse color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void MovableItem::Diffuse(float *aryColor)
{
	CStdColor vColor(aryColor[0], aryColor[1], aryColor[2], aryColor[3], 1);
	Diffuse(vColor);
}

/**
\brief	Loads the Diffuse color from an XML data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/
void MovableItem::Diffuse(string strXml)
{
	CStdColor vColor(1);
	vColor.Load(strXml, "Color");
	Diffuse(vColor);
}

/**
\brief	Gets the specular color. 

\author	dcofer
\date	3/2/2011

\return	Pointer to color data
**/
CStdColor *MovableItem::Specular() {return &m_vSpecular;}

/**
\brief	Sets the Specular color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void MovableItem::Specular(CStdColor &aryColor)
{
	m_vSpecular = aryColor;
	if(m_lpPhysicsMovableItem) m_lpPhysicsMovableItem->Physics_SetColor();
}

/**
\brief	Sets the Specular color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void MovableItem::Specular(float *aryColor)
{
	CStdColor vColor(aryColor[0], aryColor[1], aryColor[2], aryColor[3], 1);
	Specular(vColor);
}

/**
\brief	Loads the Specular color from an XML data packet.  

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/
void MovableItem::Specular(string strXml)
{
	CStdColor vColor(1);
	vColor.Load(strXml, "Color");
	Specular(vColor);
}

/**
\brief	Gets the shininess. 

\author	dcofer
\date	3/2/2011

\return	Shininess value. 
**/

float MovableItem::Shininess() {return m_fltShininess;}

/**
\brief	Sets the shininess value. 

\author	dcofer
\date	3/2/2011

\param	fltVal	The new value. 
**/
void MovableItem::Shininess(float fltVal)
{
	Std_InValidRange((float) 0, (float) 128, fltVal, TRUE, "Shininess");
	m_fltShininess = fltVal;
	if(m_lpPhysicsMovableItem) m_lpPhysicsMovableItem->Physics_SetColor();
}

/**
\brief	Gets the texture filename. 

\author	dcofer
\date	3/2/2011

\return	Texture filename. 
**/
string MovableItem::Texture() {return m_strTexture;}

/**
\brief	Sets the Texture filename. 

\author	dcofer
\date	3/2/2011

\param	strValue	The texture filename. 
**/
void MovableItem::Texture(string strValue)
{
	m_strTexture = strValue;
	if(m_lpPhysicsMovableItem) m_lpPhysicsMovableItem->Physics_TextureChanged();
}

/**
\brief	Gets the relative position of the selected vertex. (m_vSelectedVertex) 

\author	dcofer
\date	3/2/2011

\return	returns m_vSelectedVertex. 
**/
CStdFPoint MovableItem::SelectedVertex() {return m_vSelectedVertex;}

/**
\brief	Sets the relative position of the selected vertex. (m_vSelectedVertex) 

\author	dcofer
\date	3/2/2011

\param [in,out]	vPoint		The new point to use to set the local position. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->SelectedVertexChanged
							callback method to inform the GUI that the selected vertex has changed. If false
							then this callback will be skipped. 
\param bUpdatePhysics		If true then the physcis object is also updated.
**/
void MovableItem::SelectedVertex(CStdFPoint &vPoint, BOOL bFireChangeEvent, BOOL bUpdatePhysics) 
{
	m_vSelectedVertex = vPoint;
	
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_SelectedVertex(vPoint.x, vPoint.y, vPoint.z);

	if(m_lpCallback && bFireChangeEvent)
		m_lpCallback->SelectedVertexChanged(vPoint.x, vPoint.y, vPoint.z);
}

/**
\brief	Sets the relative position of the selected vertex. (m_vSelectedVertex) 

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bFireChangeEvent	If true then this will call the IMovableItemCallback->SelectedVertexChanged
							callback method to inform the GUI that the selected vertex has changed. If false
							then this callback will be skipped. 
\param bUpdatePhysics		If true then the physcis object is also updated.
**/
void MovableItem::SelectedVertex(float fltX, float fltY, float fltZ, BOOL bFireChangeEvent, BOOL bUpdatePhysics) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	SelectedVertex(vPos, bFireChangeEvent, bUpdatePhysics);
}

/**
\brief	Gets the callback interface pointer. This is an interface pointer to a callback class
that allows us to notify the GUI of events that occur within the simulation. 

\author	dcofer
\date	3/2/2011

\return	Pointer to callback object if one exists, NULL else. 
**/
IMovableItemCallback *MovableItem::Callback() {return m_lpCallback;}

/**
\brief	Sets the callback interface pointer. This is an interface pointer to a callback class
that allows us to notify the GUI of events that occur within the simulation. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpCallback	The pointer to a callback interface object. 
**/
void MovableItem::Callback(IMovableItemCallback *lpCallback) {m_lpCallback = lpCallback;}

/**
\brief	Gets the physics body interface pointer. This is an interface reference to the Vs version
of this object. It will allow us to call methods directly in the Vs (OSG) version of the object
directly without having to overload a bunch of methods in each box, sphere, etc.. 

\author	dcofer
\date	3/2/2011

\return	Pointer to Vs interface, NULL else. 
**/
IPhysicsMovableItem *MovableItem::PhysicsMovableItem() {return m_lpPhysicsMovableItem;}

/**
\brief	Sets the physics body interface pointer. This is an interface reference to the Vs version
of this object. It will allow us to call methods directly in the Vs (OSG) version of the object
directly without having to overload a bunch of methods in each box, sphere, etc.. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpBody	The pointer to the phsyics body interface. 
**/
void MovableItem::PhysicsMovableItem(IPhysicsMovableItem *lpBody) {m_lpPhysicsMovableItem = lpBody;}

/**
\brief	Gets the bounding radius of this part. 

\author	dcofer
\date	3/2/2011

\return	The bounding radius. 
**/
float MovableItem::GetBoundingRadius()
{
	if(m_lpPhysicsMovableItem)
		return m_lpPhysicsMovableItem->Physics_GetBoundingRadius();
	else
		return 1;
}

/**
\brief	Gets the bounding box for this part.

\author	dcofer
\date	6/18/2011

\return	The bounding box.
**/
BoundingBox MovableItem::GetBoundingBox()
{
	BoundingBox bb;

	if(m_lpPhysicsMovableItem)
		return m_lpPhysicsMovableItem->Physics_GetBoundingBox();
	else
		return bb;
}

/**
\brief	Gets whether this body part can be translated along the x-axis by the user with the drag handlers.

\author	dcofer
\date	4/21/2011

\return	true if can drag along x-axis.
**/
BOOL MovableItem::AllowTranslateDragX() {return TRUE;}


/**
\brief	Gets whether this body part can be translated along the y-axis by the user with the drag handlers.

\author	dcofer
\date	4/21/2011

\return	true if can drag along y-axis.
**/
BOOL MovableItem::AllowTranslateDragY() {return TRUE;}

/**
\brief	Gets whether this body part can be translated along the z-axis by the user with the drag handlers.

\author	dcofer
\date	4/21/2011

\return	true if can drag along z-axis.
**/
BOOL MovableItem::AllowTranslateDragZ() {return TRUE;}

/**
\brief	Gets whether this body part can be rotated along the x-axis by the user with the drag handlers.

\author	dcofer
\date	4/21/2011

\return	true if can rotate along x-axis.
**/
BOOL MovableItem::AllowRotateDragX() {return TRUE;}

/**
\brief	Gets whether this body part can be rotated along the y-axis by the user with the drag handlers.

\author	dcofer
\date	4/21/2011

\return	true if can rotate along y-axis.
**/
BOOL MovableItem::AllowRotateDragY() {return TRUE;}

/**
\brief	Gets whether this body part can be rotated along the z-axis by the user with the drag handlers.

\author	dcofer
\date	4/21/2011

\return	true if can rotate along z-axis.
**/
BOOL MovableItem::AllowRotateDragZ() {return TRUE;}


void MovableItem::UserDefinedDraggerRadius(float fltRadius)
{
	if(fltRadius <= 0)
		m_fltUserDefinedDraggerRadius = -1;
	else
		m_fltUserDefinedDraggerRadius = fltRadius;

	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_ResizeDragHandler(m_fltUserDefinedDraggerRadius);
}
		

float MovableItem::UserDefinedDraggerRadius()
{return m_fltUserDefinedDraggerRadius;}

#pragma endregion

void MovableItem::Selected(BOOL bValue, BOOL bSelectMultiple)
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_Selected(bValue, bSelectMultiple);

	if(m_lpCallback)
		m_lpCallback->SelectionChanged(bValue, bSelectMultiple);
}

/**
\brief	Called when the visual selection mode changed in GUI.

\details In the GUI the user can select several different types of visual selection modes This
method is called any time that the user switches the selection mode in the GUI. This allows us to
change the current Alpha value of the objects so the display is correct. 

\author	dcofer
\date	3/2/2011

\param	iNewMode	The new VisualSelectionMode. 
**/
void MovableItem::VisualSelectionModeChanged(int iNewMode)
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->SetAlpha();
}
/**
\brief	Called when the user clicks on this object while the AddBody mode is active.

\details When the user selects the AddBody mode in the GUI then the simulation detects when a
part is clicked in the simulation window. (This occurs in the VsCameraManipulator class). It gets
the position of the click in global coordinates, and the normal vector for the surface that was
clicked. We then need to pass this info back up the GUI and let it know the click occurred. This
method uses the IMovableItemCallback object to send this info back up to the GUI. 

\author	dcofer
\date	3/2/2011

\param	fltPosX		The position x coordinate. 
\param	fltPosY		The position y coordinate. 
\param	fltPosZ		The position z coordinate. 
\param	fltNormX	The normal x coordinate. 
\param	fltNormY	The normal y coordinate. 
\param	fltNormZ	The normal z coordinate. 
**/
void MovableItem::AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)
{
	//Do nothing here in the base. This could be a strucutre, and we do not want to do anything for that class.
	//You need to implmenent this in the derived bodypart class.
}

/**
\brief	Orients a new part based on where the parent was clicked and the normal of the surface face.

\author	dcofer
\date	6/18/2011

\param	fltXPos 	The click x position.
\param	fltYPos 	The click y position.
\param	fltZPos 	The click z position.
\param	fltXNorm	The face normal x coordinate.
\param	fltYNorm	Th face normale y coordinat.
\param	fltZNorm	The face normal z coordinate.
**/
void MovableItem::OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm)
{
	if(m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_OrientNewPart(fltXPos, fltYPos, fltZPos, fltXNorm, fltYNorm, fltZNorm);
}

/**
\brief	Calculates the local position values for matrix transform for the part to be in a specific world position.

\author	dcofer
\date	7/3/2011

\param	fltWorldX	The world x coordinate we want.
\param	fltWorldY	The world y coordinate we want.
\param	fltWorldZ	The world z coordinate we want.

\return	The calculated local position for world position.
**/
BOOL MovableItem::CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos)
{
	if(m_lpPhysicsMovableItem)
		return m_lpPhysicsMovableItem->Physics_CalculateLocalPosForWorldPos(fltWorldX, fltWorldY, fltWorldZ, vLocalPos);
	return FALSE;
}

#pragma region DataAccesMethods

float *MovableItem::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "WORLDPOSITIONX")
		return &m_oReportWorldPosition.x;

	if(strType == "WORLDPOSITIONY")
		return &m_oReportWorldPosition.y;

	if(strType == "WORLDPOSITIONZ")
		return &m_oReportWorldPosition.z;

	if(strType == "POSITIONX")
		return &m_oReportPosition.x;

	if(strType == "POSITIONY")
		return &m_oReportPosition.y;

	if(strType == "POSITIONZ")
		return &m_oReportPosition.z;

	if(strType == "ROTATIONX")
		return &m_oReportRotation.x;

	if(strType == "ROTATIONY")
		return &m_oReportRotation.y;

	if(strType == "ROTATIONZ")
		return &m_oReportRotation.z;

	return 0;
}

BOOL MovableItem::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(strDataType == "POSITION")
	{
		Position(strValue);
		return true;
	}

	if(strDataType == "POSITION.X")
	{
		Position(atof(strValue.c_str()), m_oReportPosition.y, m_oReportPosition.z);
		return true;
	}

	if(strDataType == "POSITION.Y")
	{
		Position(m_oReportPosition.x, atof(strValue.c_str()), m_oReportPosition.z);
		return true;
	}

	if(strDataType == "POSITION.Z")
	{
		Position(m_oReportPosition.x, m_oReportPosition.y, atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "ROTATION")
	{
		Rotation(strValue);
		return true;
	}

	if(strDataType == "ROTATION.X")
	{
		Rotation(atof(strValue.c_str()), m_oReportRotation.y, m_oReportRotation.z);
		return true;
	}

	if(strDataType == "ROTATION.Y")
	{
		Rotation(m_oReportRotation.x, atof(strValue.c_str()), m_oReportRotation.z);
		return true;
	}

	if(strDataType == "ROTATION.Z")
	{
		Rotation(m_oReportRotation.x, m_oReportRotation.y, atof(strValue.c_str()));
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

	if(strDataType == "AMBIENT")
	{
		Ambient(strValue);
		return TRUE;
	}

	if(strDataType == "AMBIENT.RED")
	{
		float aryVal[4] = {atof(strValue.c_str()), m_vAmbient.g(), m_vAmbient.b(), m_vAmbient.a()};
		Ambient(aryVal);
		return TRUE;
	}

	if(strDataType == "AMBIENT.GREEN")
	{
		float aryVal[4] = {m_vAmbient.r(), atof(strValue.c_str()), m_vAmbient.b(), m_vAmbient.a()};
		Ambient(aryVal);
		return TRUE;
	}

	if(strDataType == "AMBIENT.BLUE")
	{
		float aryVal[4] = {m_vAmbient.r(), m_vAmbient.g(), atof(strValue.c_str()), m_vAmbient.a()};
		Ambient(aryVal);
		return TRUE;
	}

	if(strDataType == "AMBIENT.ALPHA")
	{
		float aryVal[4] = {m_vAmbient.r(), m_vAmbient.g(), m_vAmbient.b(), atof(strValue.c_str())};
		Ambient(aryVal);
		return TRUE;
	}
	
	if(strDataType == "DIFFUSE")
	{
		Diffuse(strValue);
		return TRUE;
	}

	if(strDataType == "DIFFUSE.RED")
	{
		float aryVal[4] = {atof(strValue.c_str()), m_vAmbient.g(), m_vAmbient.b(), m_vAmbient.a()};
		Diffuse(aryVal);
		return TRUE;
	}

	if(strDataType == "DIFFUSE.GREEN")
	{
		float aryVal[4] = {m_vAmbient.r(), atof(strValue.c_str()), m_vAmbient.b(), m_vAmbient.a()};
		Diffuse(aryVal);
		return TRUE;
	}

	if(strDataType == "DIFFUSE.BLUE")
	{
		float aryVal[4] = {m_vAmbient.r(), m_vAmbient.g(), atof(strValue.c_str()), m_vAmbient.a()};
		Diffuse(aryVal);
		return TRUE;
	}

	if(strDataType == "DIFFUSE.ALPHA")
	{
		float aryVal[4] = {m_vAmbient.r(), m_vAmbient.g(), m_vAmbient.b(), atof(strValue.c_str())};
		Diffuse(aryVal);
		return TRUE;
	}
	
	if(strDataType == "SPECULAR")
	{
		Specular(strValue);
		return TRUE;
	}

	if(strDataType == "SPECULAR.RED")
	{
		float aryVal[4] = {atof(strValue.c_str()), m_vAmbient.g(), m_vAmbient.b(), m_vAmbient.a()};
		Specular(aryVal);
		return TRUE;
	}

	if(strDataType == "SPECULAR.GREEN")
	{
		float aryVal[4] = {m_vAmbient.r(), atof(strValue.c_str()), m_vAmbient.b(), m_vAmbient.a()};
		Specular(aryVal);
		return TRUE;
	}

	if(strDataType == "SPECULAR.BLUE")
	{
		float aryVal[4] = {m_vAmbient.r(), m_vAmbient.g(), atof(strValue.c_str()), m_vAmbient.a()};
		Specular(aryVal);
		return TRUE;
	}

	if(strDataType == "SPECULAR.ALPHA")
	{
		float aryVal[4] = {m_vAmbient.r(), m_vAmbient.g(), m_vAmbient.b(), atof(strValue.c_str())};
		Specular(aryVal);
		return TRUE;
	}
	
	if(strDataType == "SHININESS")
	{
		Shininess(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strDataType == "TEXTURE")
	{
		Texture(strValue);
		return TRUE;
	}
	
	if(strDataType == "DRAGGERRADIUS")
	{
		UserDefinedDraggerRadius(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void MovableItem::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	aryNames.Add("Position");
	aryTypes.Add("Xml");

	aryNames.Add("Position.X");
	aryTypes.Add("Float");

	aryNames.Add("Position.Y");
	aryTypes.Add("Float");

	aryNames.Add("Position.Z");
	aryTypes.Add("Float");

	aryNames.Add("Rotation");
	aryTypes.Add("Xml");

	aryNames.Add("Rotation.X");
	aryTypes.Add("Float");

	aryNames.Add("Rotation.Y");
	aryTypes.Add("Float");

	aryNames.Add("Rotation.Z");
	aryTypes.Add("Float");

	aryNames.Add("Visible");
	aryTypes.Add("Boolean");

	aryNames.Add("GraphicsAlpha");
	aryTypes.Add("Float");

	aryNames.Add("CollisionAlpha");
	aryTypes.Add("Float");

	aryNames.Add("JointsAlpha");
	aryTypes.Add("Float");

	aryNames.Add("ReceptiveFieldsAlpha");
	aryTypes.Add("Float");

	aryNames.Add("SimulationAlpha");
	aryTypes.Add("Float");

	aryNames.Add("Ambient");
	aryTypes.Add("Xml");

	aryNames.Add("Ambient.Red");
	aryTypes.Add("Float");

	aryNames.Add("Ambient.Green");
	aryTypes.Add("Float");

	aryNames.Add("Ambient.Blue");
	aryTypes.Add("Float");

	aryNames.Add("Ambient.Alpha");
	aryTypes.Add("Float");

	aryNames.Add("Diffuse");
	aryTypes.Add("Xml");

	aryNames.Add("Diffuse.Red");
	aryTypes.Add("Float");

	aryNames.Add("Diffuse.Green");
	aryTypes.Add("Float");

	aryNames.Add("Diffuse.Blue");
	aryTypes.Add("Float");

	aryNames.Add("Diffuse.Alpha");
	aryTypes.Add("Float");

	aryNames.Add("Specular");
	aryTypes.Add("Xml");

	aryNames.Add("Specular.Red");
	aryTypes.Add("Float");

	aryNames.Add("Specular.Green");
	aryTypes.Add("Float");

	aryNames.Add("Specular.Blue");
	aryTypes.Add("Float");

	aryNames.Add("Specular.Alpha");
	aryTypes.Add("Float");

	aryNames.Add("Shininess");
	aryTypes.Add("Float");

	aryNames.Add("Texture");
	aryTypes.Add("String");

	aryNames.Add("DraggerRadius");
	aryTypes.Add("Float");

}

#pragma endregion

/**
\brief	Loads the items position.

\details I have split this off into its own virtual class because some derived classes may need to override
this function. For example, RigidBody needs to override it to not set the position if it is the root body.

\author	dcofer
\date	5/14/2011

\param [in,out]	oXml	The xml.
**/
void MovableItem::LoadPosition(CStdXml &oXml)
{
	CStdFPoint vTemp;

	Std_LoadPoint(oXml, "Position", vTemp);
	Position(vTemp, TRUE, FALSE, FALSE);	

	//if(!m_lpParent)
	//	AbsolutePosition(m_oPosition);
	//else
	//	AbsolutePosition( m_lpParent->AbsolutePosition() + m_oPosition);
}

/**
\brief	Loads the items rotation.

\details I have split this off into its own virtual function to make it consistent with the LoadPosition method.

\author	dcofer
\date	5/14/2011

\param [in,out]	oXml	The xml.
**/
void MovableItem::LoadRotation(CStdXml &oXml)
{
	CStdFPoint vTemp;
	Std_LoadPoint(oXml, "Rotation", vTemp);
	Rotation(vTemp, FALSE, FALSE);	
}

void MovableItem::Load(CStdXml &oXml)
{
	oXml.IntoElem();  //Into Element
	
	if(oXml.FindChildElement("TransformMatrix", false) && m_lpPhysicsMovableItem)
		m_lpPhysicsMovableItem->Physics_LoadTransformMatrix(oXml);
	else
	{
		LoadPosition(oXml);
		LoadRotation(oXml);
	}

	IsVisible(oXml.GetChildBool("IsVisible", m_bIsVisible));
	GraphicsAlpha(oXml.GetChildFloat("GraphicsAlpha", m_fltGraphicsAlpha));
	CollisionsAlpha(oXml.GetChildFloat("CollisionsAlpha", m_fltCollisionsAlpha));
	JointsAlpha(oXml.GetChildFloat("JointsAlpha", m_fltJointsAlpha));
	ReceptiveFieldsAlpha(oXml.GetChildFloat("ReceptiveFieldsAlpha", m_fltReceptiveFieldsAlpha));
	SimulationAlpha(oXml.GetChildFloat("SimulationAlpha", m_fltSimulationAlpha));

	m_vDiffuse.Load(oXml, "Diffuse", false);
	m_vAmbient.Load(oXml, "Ambient", false);
	m_vSpecular.Load(oXml, "Specular", false);
	m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess);

	m_strTexture = oXml.GetChildString("Texture", "");

	m_fltUserDefinedDraggerRadius = oXml.GetChildFloat("DraggerSize", m_fltUserDefinedDraggerRadius);

	oXml.OutOfElem(); //OutOf Element
}

	}			//Environment
}				//AnimatSim