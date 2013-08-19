
#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgPrismaticLimit.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

OsgPrismaticLimit::OsgPrismaticLimit()
{
}

OsgPrismaticLimit::~OsgPrismaticLimit()
{
}

void OsgPrismaticLimit::LimitAlpha(float fltA)
{
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

osg::Geometry *OsgPrismaticLimit::BoxGeometry() {return m_osgBox.get();}

osg::MatrixTransform *OsgPrismaticLimit::BoxMT() {return m_osgBoxMT.get();}

osg::Material *OsgPrismaticLimit::BoxMat() {return m_osgBoxMat.get();}

osg::StateSet *OsgPrismaticLimit::BoxSS() {return m_osgBoxSS.get();}

osg::Geometry *OsgPrismaticLimit::CylinderGeometry() {return m_osgCylinder.get();}

osg::MatrixTransform *OsgPrismaticLimit::CylinderMT() {return m_osgCylinderMT.get();}

osg::Material *OsgPrismaticLimit::CylinderMat() {return m_osgCylinderMat.get();}

osg::StateSet *OsgPrismaticLimit::CylinderSS() {return m_osgCylinderSS.get();}

void OsgPrismaticLimit::SetLimitPos(float fltRadius, float fltLimitPos)
{
	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	//Reset the position of the Box.
	if(m_osgBoxMT.valid())
	{
		vPos.Set(0, 0, -fltLimitPos); vRot.Set(0, 0, 0); 
		m_osgBoxMT->setMatrix(SetupMatrix(vPos, vRot));
	}

	//Reset the position and size of the Cylinder.
	if(m_osgCylinderMT.valid() && m_osgCylinder.valid() && m_osgCylinderGeode.valid())
	{
		//First delete the cylinder geometry that currently exists.
		if(m_osgCylinderGeode->containsDrawable(m_osgCylinder.get()))
			m_osgCylinderGeode->removeDrawable(m_osgCylinder.get());
		m_osgCylinder.release();

		//Now recreate the cylinder using the new limit position.
		m_osgCylinder = CreateConeGeometry(fabs(fltLimitPos), fltRadius, fltRadius, 10, true, true, true);
		m_osgCylinderGeode->addDrawable(m_osgCylinder.get());		

		vPos.Set(0, 0, (-fltLimitPos/2)); vRot.Set(osg::PI/2, 0, 0); 
		m_osgCylinderMT->setMatrix(SetupMatrix(vPos, vRot));
	}
}

void OsgPrismaticLimit::DeleteLimitGraphics()
{
    m_osgBox.release();
    m_osgBoxMT.release();
    m_osgBoxMat.release();
    m_osgBoxSS.release();
    m_osgCylinder.release();
    m_osgCylinderGeode.release();
    m_osgCylinderMT.release();
    m_osgCylinderMat.release();
    m_osgCylinderSS.release();
}

void OsgPrismaticLimit::SetupLimitGraphics(float fltBoxSize, float fltRadius, float fltLimitPos, bool bIsShowPosition, CStdColor vColor)
{
	//Create the LIMIT Box
	m_osgBox = CreateBoxGeometry(fltBoxSize, fltBoxSize, 
									fltBoxSize, fltBoxSize, 
									fltBoxSize, fltBoxSize);
	osg::ref_ptr<osg::Geode> osgBox = new osg::Geode;
	osgBox->addDrawable(m_osgBox.get());

	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	//Translate box
	vPos.Set(0, 0, -fltLimitPos); 
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
	m_osgBoxMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(vColor.r(), vColor.g(), vColor.b(), vColor.a()));
	m_osgBoxMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
	m_osgBoxMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
	m_osgBoxSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

	//apply the material
	m_osgBoxSS->setAttribute(m_osgBoxMat.get(), osg::StateAttribute::ON);


	//Create the cylinder for the Prismatic
	//If this is the limit for showing the position then we should not create a cylinder. We only do that for the
	// upper and lower limits.
	if(!bIsShowPosition)
	{
		m_osgCylinder = CreateConeGeometry(fabs(fltLimitPos), fltRadius, fltRadius, 10, true, true, true);
		m_osgCylinderGeode = new osg::Geode;
		m_osgCylinderGeode->addDrawable(m_osgCylinder.get());

		CStdFPoint vPos(0, 0, (-fltLimitPos/2)), vRot(osg::PI/2, 0, 0); 
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
		m_osgCylinderMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(vColor.r(), vColor.g(), vColor.b(), vColor.a()));
		//m_osgCylinderMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(1, 0.25, 1, 1));
		m_osgCylinderMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
		m_osgCylinderMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
		m_osgCylinderSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

		//apply the material
		m_osgCylinderSS->setAttribute(m_osgCylinderMat.get(), osg::StateAttribute::ON);
	}
}

	}			// Environment
}				//OsgAnimatSim
