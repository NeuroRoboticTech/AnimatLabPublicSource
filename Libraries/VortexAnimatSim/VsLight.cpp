/**
\file	VsLight.cpp

\brief	Implements the vortex Light class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsLight.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
VsLight::VsLight()
{
	m_lpThisLI = NULL;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
VsLight::~VsLight()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsBox\r\n", "", -1, FALSE, TRUE);}
}

void VsLight::SetThisPointers()
{
	VsMovableItem::SetThisPointers();

	m_lpThisLI = dynamic_cast<Light *>(this);
	if(!m_lpThisLI)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisLI, " + m_lpThisAB->Name());
}

osg::Group *VsLight::ParentOSG()
{
	return GetVsSimulator()->OSGLightGroup();
}

void VsLight::Position(CStdFPoint &oPoint, BOOL bUseScaling, BOOL bFireChangeEvent, BOOL bUpdateMatrix)
{
	Light::Position(oPoint, bUseScaling, bFireChangeEvent, bUpdateMatrix);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		osg::Vec4 position(m_oPosition.x, m_oPosition.y, m_oPosition.z, 1);
		m_osgLight->setPosition(position);
	}
}

void VsLight::Ambient(CStdColor &aryColor)
{
	Light::Diffuse(aryColor);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		//We swapped the ambient and diffuse for the sphere. We need to swap it back when setting the color for the light object.
		osg::Vec4 color(m_vDiffuse.r(), m_vDiffuse.g(), m_vDiffuse.b(), 1.0);
		m_osgLight->setAmbient(color);
	}
}

void VsLight::Diffuse(CStdColor &aryColor)
{
	Light::Ambient(aryColor);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		//We swapped the ambient and diffuse for the sphere. We need to swap it back when setting the color for the light object.
		osg::Vec4 color(m_vAmbient.r(), m_vAmbient.g(), m_vAmbient.b(), 1.0);
		m_osgLight->setDiffuse(color);
	}
}

void VsLight::Specular(CStdColor &aryColor)
{
	Light::Specular(aryColor);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		osg::Vec4 specular(m_vSpecular.r(), m_vSpecular.g(), m_vSpecular.b(), 1.0);
		m_osgLight->setSpecular(specular);
	}
}

void VsLight::SetupLighting()
{
    // Set up lighting.
	//For a light object we want to swap the ambient and diffuse settings for the 
	//sphere that is shown. When we create the actuall light we will swap these back.
    osg::Vec4 ambient(m_vDiffuse.r(), m_vDiffuse.g(), m_vDiffuse.b(), 1.0);
    osg::Vec4 diffuse(m_vAmbient.r(), m_vAmbient.g(), m_vAmbient.b(), 1.0);
    osg::Vec4 specular(m_vSpecular.r(), m_vSpecular.g(), m_vSpecular.b(), 1.0);
    osg::Vec4 position(m_oPosition.x, m_oPosition.y, m_oPosition.z, 1);
    //osg::Vec3 direction(0, -1, 0);
    //direction.normalize();

    m_osgLight = new osg::Light(m_iLightNum);
    m_osgLight->setAmbient(ambient);
	m_osgLight->setDiffuse(diffuse);
    m_osgLight->setSpecular(specular);
    m_osgLight->setPosition(position);
    //m_osgLight->setConstantAttenuation(0.05);
    m_osgLight->setQuadraticAttenuation(0.002);
	//m_osgLight->setLightNum(m_iLightNum);
    //light->setDirection(direction);

    m_osgLightSource = new osg::LightSource;
    m_osgLightSource->setLight(m_osgLight.get());
	GetVsSimulator()->OSGLightGroup()->addChild(m_osgLightSource.get());

	osg::StateSet *groupStateSet = GetVsSimulator()->OSGLightGroup()->getOrCreateStateSet();
	m_osgLightSource->setLocalStateSetModes(osg::StateAttribute::ON); 
	m_osgLightSource->setStateSetModes(*groupStateSet, osg::StateAttribute::ON); 
}


void VsLight::SetupGraphics()
{
	VsMovableItem::SetupGraphics();

	SetupLighting();
}

void VsLight::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), Radius());
}

void VsLight::DeleteGraphics()
{
	VsMovableItem::DeleteGraphics();

	if(m_osgLightSource.valid() && GetVsSimulator() && GetVsSimulator()->OSGLightGroup() && GetVsSimulator()->OSGLightGroup()->containsNode(m_osgLightSource.get()))
		GetVsSimulator()->OSGLightGroup()->removeChild(m_osgLightSource.get());

	m_osgLight.release();
	m_osgLightSource.release();
}

void VsLight::Create()
{
	CreateGeometry();
	CreateItem();

	Light::Create();
}

void VsLight::ResetSimulation()
{
	VsMovableItem::Physics_ResetSimulation();

	Light::ResetSimulation();
}

void VsLight::Physics_Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
		if(!osgGroup)
			THROW_TEXT_ERROR(Vs_Err_lNodeNotGeode, Vs_Err_strNodeNotGeode, m_lpThisAB->Name());

		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
			osgGroup->removeDrawable(m_osgGeometry.get());

		m_osgGeometry.release();

		//Create a new box geometry with the new sizes.
		CreateGraphicsGeometry();
		m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

		//Add it to the geode.
		osgGroup->addDrawable(m_osgGeometry.get());

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		if(m_osgNodeGroup.valid())
		{
			osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
			osgVisitor->traverse(*m_osgNodeGroup);
		}
	}

}

void VsLight::Physics_SetColor()
{
	SetColor(*m_lpThisMI->Ambient(), *m_lpThisMI->Diffuse(), *m_lpThisMI->Specular(), m_lpThisMI->Shininess()); 
}

	}			// Environment
}				//VortexAnimatSim
