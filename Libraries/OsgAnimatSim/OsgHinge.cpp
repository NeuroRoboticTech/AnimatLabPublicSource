/**
\file	OsgHinge.cpp

\brief	Implements the vortex hinge class.
**/

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgHingeLimit.h"
#include "OsgHinge.h"
#include "OsgStructure.h"
#include "OsgSimulator.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

/**
\brief	Default constructor.

\author	dcofer
\date	4/15/2011
**/
OsgHinge::OsgHinge()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
OsgHinge::~OsgHinge()
{
}


void OsgHinge::DeleteHingeGraphics(osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgHingeLimit *lpUpperLimit, OsgHingeLimit *lpLowerLimit, OsgHingeLimit *lpPosFlap)
{
    if(osgJointMT.valid())
    {
        if(m_osgCylinderMT.valid()) osgJointMT->removeChild(m_osgCylinderMT.get());
		if(lpUpperLimit && lpUpperLimit->FlapTranslateMT()) osgJointMT->removeChild(lpUpperLimit->FlapTranslateMT());
		if(lpLowerLimit && lpLowerLimit->FlapTranslateMT()) osgJointMT->removeChild(lpLowerLimit->FlapTranslateMT());
		if(lpPosFlap && lpPosFlap->FlapTranslateMT()) osgJointMT->removeChild(lpPosFlap->FlapTranslateMT());
    }

    m_osgCylinder.release();
    m_osgCylinderGeode.release();
    m_osgCylinderMT.release();
    m_osgCylinderMat.release();
    m_osgCylinderSS.release();
}

/**
\brief	Creates the cylinder graphics.

\author	dcofer
\date	4/15/2011
**/
void OsgHinge::CreateCylinderGraphics(float fltHeight, float fltRadius)
{
	//Create the cylinder for the hinge
	m_osgCylinder = CreateConeGeometry(fltHeight, fltRadius, fltRadius, 30, true, true, true);
	m_osgCylinderGeode = new osg::Geode;
	m_osgCylinderGeode->addDrawable(m_osgCylinder.get());

	CStdFPoint vPos(0, 0, 0), vRot(osg::PI/2, 0, 0); 
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
	m_osgCylinderMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(1, 0.25, 1, 1));
	m_osgCylinderMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
	m_osgCylinderMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
	m_osgCylinderSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

	//apply the material
	m_osgCylinderSS->setAttribute(m_osgCylinderMat.get(), osg::StateAttribute::ON);
}

void OsgHinge::CreateHingeGraphics(float fltHeight, float fltRadius, float fltFlapWidth, float fltLimitPos, CStdColor vColor, 
                                   osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgHingeLimit *lpUpperLimit, 
                                   OsgHingeLimit *lpLowerLimit, OsgHingeLimit *lpPosFlap)
{
	CreateCylinderGraphics(fltHeight, fltRadius);

	if(lpUpperLimit) lpUpperLimit->SetupLimitGraphics(fltFlapWidth, fltHeight, fltLimitPos, vColor);
	if(lpLowerLimit) lpLowerLimit->SetupLimitGraphics(fltFlapWidth, fltHeight, fltLimitPos, vColor);
	if(lpPosFlap) lpPosFlap->SetupLimitGraphics(fltFlapWidth, fltHeight, fltLimitPos, vColor);

	osgJointMT->addChild(m_osgCylinderMT.get());
	osgJointMT->addChild(lpUpperLimit->FlapTranslateMT());
	osgJointMT->addChild(lpLowerLimit->FlapTranslateMT());
	osgJointMT->addChild(lpPosFlap->FlapTranslateMT());
}

		}		//Joints
	}			// Environment
}				//OsgAnimatSim
