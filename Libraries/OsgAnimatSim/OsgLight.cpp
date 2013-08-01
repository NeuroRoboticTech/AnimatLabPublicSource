/**
\file	OsgLight.cpp

\brief	Implements the vortex Light class.
**/

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgLight.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
OsgLight::OsgLight()
{
	m_lpThisLI = NULL;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
OsgLight::~OsgLight()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsBox\r\n", "", -1, false, true);}
}

void OsgLight::SetThisPointers()
{
	OsgMovableItem::SetThisPointers();

	m_lpThisLI = dynamic_cast<Light *>(this);
	if(!m_lpThisLI)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisLI, " + m_lpThisAB->Name());
}

osg::Group *OsgLight::ParentOSG()
{
	return GetOsgSimulator()->OSGRoot();
}

void OsgLight::Enabled(bool bVal)
{
	 AnimatSim::Environment::Light::Enabled(bVal);

	 if(GetOsgSimulator() &&  GetOsgSimulator()->OSGRoot())
	 {
		osg::StateSet *rootStateSet = GetOsgSimulator()->OSGRoot()->getOrCreateStateSet();

		if(bVal)
			rootStateSet->setMode( GetGlLight(), osg::StateAttribute::ON );
		else
			rootStateSet->setMode( GetGlLight(), osg::StateAttribute::OFF );
	 }
}

void OsgLight::Position(CStdFPoint &oPoint, bool bUseScaling, bool bFireChangeEvent, bool bUpdateMatrix)
{
	Light::Position(oPoint, bUseScaling, bFireChangeEvent, bUpdateMatrix);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		osg::Vec4 position(m_oPosition.x, m_oPosition.y, m_oPosition.z, 1);
		m_osgLight->setPosition(position);
	}
}

void OsgLight::Ambient(CStdColor &aryColor)
{
	Light::Ambient(aryColor);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		osg::Vec4 color(m_vAmbient.r(), m_vAmbient.g(), m_vAmbient.b(), 1.0);
		m_osgLight->setAmbient(color);
	}
}

void OsgLight::Diffuse(CStdColor &aryColor)
{
	Light::Diffuse(aryColor);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		osg::Vec4 color(m_vDiffuse.r(), m_vDiffuse.g(), m_vDiffuse.b(), 1.0);
		m_osgLight->setDiffuse(color);
	}
}

void OsgLight::Specular(CStdColor &aryColor)
{
	Light::Specular(aryColor);

	//Reposition the light.
	if(m_osgLight.valid())
	{
		osg::Vec4 specular(m_vSpecular.r(), m_vSpecular.g(), m_vSpecular.b(), 1.0);
		m_osgLight->setSpecular(specular);
	}
}

void OsgLight::SetAttenuation()
{
	if(m_osgLight.valid())
	{
		m_osgLight->setConstantAttenuation(m_fltConstantAttenRatio);
		
		if(m_fltLinearAttenDistance > 0)
		{
			float fltAtten = 1/m_fltLinearAttenDistance;
			m_osgLight->setLinearAttenuation(fltAtten);
		}
		else
			m_osgLight->setLinearAttenuation(0);

		if(m_fltQuadraticAttenDistance > 0)
		{
			float fltAtten = 1/m_fltQuadraticAttenDistance;
			m_osgLight->setQuadraticAttenuation(fltAtten);
		}
		else
			m_osgLight->setQuadraticAttenuation(0);
	}
}

int OsgLight::GetGlLight()
{
	switch (m_iLightNum)
	{
	case 0:
		return GL_LIGHT0;
	case 1:
		return GL_LIGHT1;
	case 2:
		return GL_LIGHT2;
	case 3:
		return GL_LIGHT3;
	case 4:
		return GL_LIGHT4;
	case 5:
		return GL_LIGHT5;
	case 6:
		return GL_LIGHT6;
	case 7:
		return GL_LIGHT7;
	default:
		return GL_LIGHT0;
	}
}

void OsgLight::SetupLighting()
{
    // Set up lighting.
    osg::Vec4 ambient(m_vAmbient.r(), m_vAmbient.g(), m_vAmbient.b(), 1.0);
    osg::Vec4 diffuse(m_vDiffuse.r(), m_vDiffuse.g(), m_vDiffuse.b(), 1.0);
    osg::Vec4 specular(m_vSpecular.r(), m_vSpecular.g(), m_vSpecular.b(), 1.0);
    osg::Vec4 position(m_oPosition.x, m_oPosition.y, m_oPosition.z, 1);

	SetAttenuation();
    m_osgLight = new osg::Light(m_iLightNum);
    m_osgLight->setAmbient(ambient);
	m_osgLight->setDiffuse(diffuse);
    m_osgLight->setSpecular(specular);
    m_osgLight->setPosition(position);

    m_osgLightSource = new osg::LightSource;
    m_osgLightSource->setLight(m_osgLight.get());
	GetOsgSimulator()->OSGRoot()->addChild(m_osgLightSource.get());

	Enabled(m_bEnabled);
}


void OsgLight::SetupGraphics()
{
	OsgMovableItem::SetupGraphics();

	SetupLighting();
}

void OsgLight::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), Radius());
}

void OsgLight::DeleteGraphics()
{
	OsgMovableItem::DeleteGraphics();

	if(m_osgLightSource.valid() && GetOsgSimulator() && GetOsgSimulator()->OSGRoot())
	{
		if(GetOsgSimulator()->OSGRoot()->containsNode(m_osgLightSource.get()))
			GetOsgSimulator()->OSGRoot()->removeChild(m_osgLightSource.get());
		
		osg::StateSet *rootStateSet = GetOsgSimulator()->OSGRoot()->getOrCreateStateSet();
		rootStateSet->setMode( GetGlLight(), osg::StateAttribute::OFF );
	}

	m_osgLight.release();
	m_osgLightSource.release();
}

void OsgLight::Create()
{
	CreateGeometry();
	CreateItem();

	Light::Create();
}

void OsgLight::ResetSimulation()
{
	OsgMovableItem::Physics_ResetSimulation();

	Light::ResetSimulation();
}

void OsgLight::Physics_Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
		if(!osgGroup)
			THROW_TEXT_ERROR(Osg_Err_lNodeNotGeode, Osg_Err_strNodeNotGeode, m_lpThisAB->Name());

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
			osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
			osgVisitor->traverse(*m_osgNodeGroup);
		}
	}

	SetAttenuation();
}

void OsgLight::Physics_SetColor()
{
	SetColor(*m_lpThisMI->Ambient(), *m_lpThisMI->Diffuse(), *m_lpThisMI->Specular(), m_lpThisMI->Shininess()); 
}

	}			// Environment
}				//OsgAnimatSim
