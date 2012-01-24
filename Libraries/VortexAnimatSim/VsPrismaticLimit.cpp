
#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPrismaticLimit.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

VsPrismaticLimit::VsPrismaticLimit()
{
	m_vxPrismatic = NULL;
}

VsPrismaticLimit::~VsPrismaticLimit()
{
}

void VsPrismaticLimit::PrismaticRef(Vx::VxPrismatic *vxPrismatic)
{
	m_vxPrismatic = vxPrismatic;
}

void VsPrismaticLimit::Alpha(float fltA)
{
	m_vColor.a(fltA);

	if(m_osgBoxMat.valid() && m_osgBoxSS.valid())
	{
		m_osgBoxMat->setAlpha(osg::Material::FRONT_AND_BACK, fltA);

		if(fltA < 1)
			m_osgBoxSS->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
		else
			m_osgBoxSS->setRenderingHint(osg::StateSet::OPAQUE_BIN);
	}

	if(m_osgCylinderMat.valid() && m_osgCylinderSS.valid())
	{
		m_osgCylinderMat->setAlpha(osg::Material::FRONT_AND_BACK, fltA);

		if(fltA < 1)
			m_osgCylinderSS->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
		else
			m_osgCylinderSS->setRenderingHint(osg::StateSet::OPAQUE_BIN);
	}
}

osg::Geometry *VsPrismaticLimit::BoxGeometry() {return m_osgBox.get();}

osg::MatrixTransform *VsPrismaticLimit::BoxMT() {return m_osgBoxMT.get();}

osg::Material *VsPrismaticLimit::BoxMat() {return m_osgBoxMat.get();}

osg::StateSet *VsPrismaticLimit::BoxSS() {return m_osgBoxSS.get();}

osg::Geometry *VsPrismaticLimit::CylinderGeometry() {return m_osgCylinder.get();}

osg::MatrixTransform *VsPrismaticLimit::CylinderMT() {return m_osgCylinderMT.get();}

osg::Material *VsPrismaticLimit::CylinderMat() {return m_osgCylinderMat.get();}

osg::StateSet *VsPrismaticLimit::CylinderSS() {return m_osgCylinderSS.get();}

void VsPrismaticLimit::SetLimitPos()
{
	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);

	//Reset the position of the Box.
	if(m_osgBoxMT.valid() && lpPrismatic)
	{
		vPos.Set(0, 0, -m_fltLimitPos); vRot.Set(0, 0, 0); 
		m_osgBoxMT->setMatrix(SetupMatrix(vPos, vRot));
	}

	//Reset the position and size of the Cylinder.
	if(m_osgCylinderMT.valid() && m_osgCylinder.valid() && m_osgCylinderGeode.valid() && lpPrismatic)
	{
		//First delete the cylinder geometry that currently exists.
		if(m_osgCylinderGeode->containsDrawable(m_osgCylinder.get()))
			m_osgCylinderGeode->removeDrawable(m_osgCylinder.get());
		m_osgCylinder.release();

		//Now recreate the cylinder using the new limit position.
		m_osgCylinder = CreateConeGeometry(fabs(m_fltLimitPos), lpPrismatic->CylinderRadius(), lpPrismatic->CylinderRadius(), 10, true, true, true);
		m_osgCylinderGeode->addDrawable(m_osgCylinder.get());		

		vPos.Set(0, 0, (-m_fltLimitPos/2)); vRot.Set(VX_PI/2, 0, 0); 
		m_osgCylinderMT->setMatrix(SetupMatrix(vPos, vRot));
	}

	//Set the limit on the physics Prismatic object.
	SetLimitValues();
}

void VsPrismaticLimit::SetLimitValues()
{
	if(m_vxPrismatic)
	{
		if(m_bIsLowerLimit)
			m_vxPrismatic->setLowerLimit(m_vxPrismatic->kLinearCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
		else
			m_vxPrismatic->setUpperLimit(m_vxPrismatic->kLinearCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	}
}

void VsPrismaticLimit::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);

	if(lpPrismatic)
	{
		//Create the LIMIT Box
		m_osgBox = CreateBoxGeometry(lpPrismatic->BoxSize(), lpPrismatic->BoxSize(), 
									 lpPrismatic->BoxSize(), lpPrismatic->BoxSize(), 
									 lpPrismatic->BoxSize(), lpPrismatic->BoxSize());
		osg::ref_ptr<osg::Geode> osgBox = new osg::Geode;
		osgBox->addDrawable(m_osgBox.get());

		CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

		//Translate box
		vPos.Set(0, 0, -m_fltLimitPos); 
		vRot.Set(0, 0, 0); 
		m_osgBoxMT = new osg::MatrixTransform();
		m_osgBoxMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgBoxMT->addChild(osgBox.get());

		//create a material to use with the pos Box
		if(!m_osgBoxMat.valid())
			m_osgBoxMat = new osg::Material();		

		//create a stateset for this node
		m_osgBoxSS = m_osgBoxMT->getOrCreateStateSet();

		//set the diffuse property of this node to the color of this body	
		m_osgBoxMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
		m_osgBoxMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(m_vColor.r(), m_vColor.g(), m_vColor.b(), m_vColor.a()));
		m_osgBoxMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
		m_osgBoxMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
		m_osgBoxSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

		//apply the material
		m_osgBoxSS->setAttribute(m_osgBoxMat.get(), osg::StateAttribute::ON);


		//Create the cylinder for the Prismatic
		//If this is the limit for showing the position then we should not create a cylinder. We only do that for the
		// upper and lower limits.
		if(!m_bIsShowPosition)
		{
			m_osgCylinder = CreateConeGeometry(fabs(m_fltLimitPos), lpPrismatic->CylinderRadius(), lpPrismatic->CylinderRadius(), 10, true, true, true);
			m_osgCylinderGeode = new osg::Geode;
			m_osgCylinderGeode->addDrawable(m_osgCylinder.get());

			CStdFPoint vPos(0, 0, (-m_fltLimitPos/2)), vRot(VX_PI/2, 0, 0); 
			m_osgCylinderMT = new osg::MatrixTransform();
			m_osgCylinderMT->setMatrix(SetupMatrix(vPos, vRot));
			m_osgCylinderMT->addChild(m_osgCylinderGeode.get());

			//create a material to use with the pos flap
			if(!m_osgCylinderMat.valid())
				m_osgCylinderMat = new osg::Material();		

			//create a stateset for this node
			m_osgCylinderSS = m_osgCylinderMT->getOrCreateStateSet();

			//set the diffuse property of this node to the color of this body	
			m_osgCylinderMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
			m_osgCylinderMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(m_vColor.r(), m_vColor.g(), m_vColor.b(), m_vColor.a()));
			//m_osgCylinderMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(1, 0.25, 1, 1));
			m_osgCylinderMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
			m_osgCylinderMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
			m_osgCylinderSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

			//apply the material
			m_osgCylinderSS->setAttribute(m_osgCylinderMat.get(), osg::StateAttribute::ON);
		}
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
