
#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPrismaticLimit.h"
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
}

void VsPrismaticLimit::SetLimitPos()
{
	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);

	////Reset the position and rotation of the Box.
	//if(m_osgBoxRotateMT.valid() && m_osgBoxTranslateMT.valid() && lpPrismatic)
	//{
	//	float fltHeight = lpPrismatic->CylinderHeight();

	//	vPos.Set(0, 0, 0); vRot.Set(0, 0, -m_fltLimitPos); 
	//	m_osgBoxRotateMT->setMatrix(SetupMatrix(vPos, vRot));

	//	vPos.Set((fltHeight/2)*sin(-m_fltLimitPos), -(fltHeight/2)*cos(-m_fltLimitPos), 0); vRot.Set(0, 0, 0); 
	//	m_osgBoxTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
	//}

	//Set the limit on the physics Prismatic object.
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
		//Create the min Box
		m_osgBox = CreateBoxGeometry(lpPrismatic->BoxSize(), lpPrismatic->BoxSize(), 
									 lpPrismatic->BoxSize(), lpPrismatic->BoxSize(), 
									 lpPrismatic->BoxSize(), lpPrismatic->BoxSize());
		osg::ref_ptr<osg::Geode> osgBox = new osg::Geode;
		osgBox->addDrawable(m_osgBox.get());

		CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

		//Translate box
		vPos.Set(0, 0, m_fltLimitPos); 
		vRot.Set(0, 0, 0); 
		m_osgBoxTranslateMT = new osg::MatrixTransform();
		m_osgBoxTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgBoxTranslateMT->addChild(osgBox.get());

		//create a material to use with the pos Box
		if(!m_osgBoxMat.valid())
			m_osgBoxMat = new osg::Material();		

		//create a stateset for this node
		m_osgBoxSS = m_osgBoxTranslateMT->getOrCreateStateSet();

		//set the diffuse property of this node to the color of this body	
		m_osgBoxMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
		m_osgBoxMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(m_vColor.r(), m_vColor.g(), m_vColor.b(), m_vColor.a()));
		m_osgBoxMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
		m_osgBoxMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
		m_osgBoxSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

		//apply the material
		m_osgBoxSS->setAttribute(m_osgBoxMat.get(), osg::StateAttribute::ON);
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
