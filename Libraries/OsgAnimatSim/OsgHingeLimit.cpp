
#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgHingeLimit.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

OsgHingeLimit::OsgHingeLimit()
{
	m_vxHinge = NULL;
}

OsgHingeLimit::~OsgHingeLimit()
{
}

void OsgHingeLimit::HingeRef(Vx::VxHinge *vxHinge)
{
	m_vxHinge = vxHinge;
}

void OsgHingeLimit::Alpha(float fltA)
{
	m_vColor.a(fltA);

	if(m_osgFlapMat.valid() && m_osgFlapSS.valid())
	{
		m_osgFlapMat->setAlpha(osg::Material::FRONT_AND_BACK, fltA);

		if(fltA < 1)
			m_osgFlapSS->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
		else
			m_osgFlapSS->setRenderingHint(osg::StateSet::OPAQUE_BIN);
	}
}

void OsgHingeLimit::SetLimitPos()
{

	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpJoint);

	//Reset the position and rotation of the flap.
	if(m_osgFlapRotateMT.valid() && m_osgFlapTranslateMT.valid() && lpHinge)
	{

		float fltHeight = lpHinge->CylinderHeight();

		vPos.Set(0, 0, 0); vRot.Set(0, 0, -m_fltLimitPos); 
		m_osgFlapRotateMT->setMatrix(SetupMatrix(vPos, vRot));

		vPos.Set((fltHeight/2)*sin(-m_fltLimitPos), -(fltHeight/2)*cos(-m_fltLimitPos), 0); vRot.Set(0, 0, 0); 
		m_osgFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
	}

	//Set the limit on the physics hinge object.
	SetLimitValues();
}

void OsgHingeLimit::SetLimitValues()
{
	if(m_vxHinge)
	{
		if(m_bIsLowerLimit)
			m_vxHinge->setLowerLimit(m_vxHinge->kAngularCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
		else
			m_vxHinge->setUpperLimit(m_vxHinge->kAngularCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	}
}

void OsgHingeLimit::DeleteGraphics()
{
    m_osgFlapRotateMT.release();
    m_osgFlap.release();
    m_osgFlapMat.release();
    m_osgFlapSS.release();
}

void OsgHingeLimit::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpJoint);

	if(lpHinge)
	{
		//Create the min flap
		m_osgFlap = CreateBoxGeometry(lpHinge->FlapWidth(), (lpHinge->CylinderHeight()/2), 
									  lpHinge->CylinderHeight(), lpHinge->FlapWidth(), 
									  (lpHinge->CylinderHeight()/2), lpHinge->CylinderHeight());
		osg::ref_ptr<osg::Geode> osgFlap = new osg::Geode;
		osgFlap->addDrawable(m_osgFlap.get());

		CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

		//Rotate flap first then translate.
		vPos.Set(0, 0, 0); vRot.Set(0, 0, -m_fltLimitPos); 
		m_osgFlapRotateMT = new osg::MatrixTransform();
		m_osgFlapRotateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgFlapRotateMT->addChild(osgFlap.get());

		vPos.Set((lpHinge->CylinderHeight()/2)*sin(-m_fltLimitPos), -(lpHinge->CylinderHeight()/2)*cos(-m_fltLimitPos), 0); 
		vRot.Set(0, 0, 0); 
		m_osgFlapTranslateMT = new osg::MatrixTransform();
		m_osgFlapTranslateMT->setMatrix(SetupMatrix(vPos, vRot));
		m_osgFlapTranslateMT->addChild(m_osgFlapRotateMT.get());

		//create a material to use with the pos flap
		if(!m_osgFlapMat.valid())
			m_osgFlapMat = new osg::Material();		

		//create a stateset for this node
		m_osgFlapSS = m_osgFlapTranslateMT->getOrCreateStateSet();

		//set the diffuse property of this node to the color of this body	
		m_osgFlapMat->setAmbient(osg::Material::FRONT_AND_BACK, osg::Vec4(0.1, 0.1, 0.1, 1));
		m_osgFlapMat->setDiffuse(osg::Material::FRONT_AND_BACK, osg::Vec4(m_vColor.r(), m_vColor.g(), m_vColor.b(), m_vColor.a()));
		m_osgFlapMat->setSpecular(osg::Material::FRONT_AND_BACK, osg::Vec4(0.25, 0.25, 0.25, 1));
		m_osgFlapMat->setShininess(osg::Material::FRONT_AND_BACK, 64);
		m_osgFlapSS->setMode(GL_BLEND, osg::StateAttribute::OVERRIDE | osg::StateAttribute::ON); 

		//apply the material
		m_osgFlapSS->setAttribute(m_osgFlapMat.get(), osg::StateAttribute::ON);
	}
}

		}		//Bodies
	}			// Environment
}				//OsgAnimatSim
