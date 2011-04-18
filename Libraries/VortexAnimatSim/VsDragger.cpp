#include "StdAfx.h"

#include "VsDragger.h"
#include "VsBody.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsDragger.h"
#include "VsTrackballDragger.h"
#include "VsTranslateAxisDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsDragger::VsDragger(VsBody *lpParent)
{
	if(!lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	m_lpVsBody = lpParent;

	//Create the gripperMT and default it to loc=0, rot=0
	m_osgGripperMT = new osg::MatrixTransform();
	osg::Matrix osgMT; osgMT.makeIdentity();
	m_osgGripperMT->setMatrix(osgMT);
	m_osgGripperMT->addChild(this);
	
	_autoTransform = new osg::AutoTransform;
	_autoTransform->setAutoScaleToScreen(true);
	addChild(_autoTransform.get());

	_sizeTransform = new osg::MatrixTransform;
	 //Screen coordinates because of AutoTransform parent
	_sizeTransform->setMatrix(osg::Matrix::scale(100, 100, 100));
	_autoTransform->addChild(_sizeTransform.get());

	_tbDragger = new VsTrackballDragger(); 
	_tbDragger->setName("TrackballDragger");
	_sizeTransform->addChild(_tbDragger.get());

	_transDragger = new VsTranslateAxisDragger();
	_transDragger->setName("TranslateAxisDragger");
	_sizeTransform->addChild(_transDragger.get());

	this->addDragger(_tbDragger.get());
	this->addDragger(_transDragger.get());
	this->getOrCreateStateSet()->setMode(GL_RESCALE_NORMAL, osg::StateAttribute::ON);
	this->setParentDragger(getParentDragger()); 
}

VsDragger::~VsDragger(void)
{
}


void VsDragger::setupDefaultGeometry()
{
	_tbDragger->setupDefaultGeometry();
	_transDragger->setupDefaultGeometry(); 

	SetupMatrix();
}

void VsDragger::AddToScene()
{
	SetupMatrix();
	if(m_lpVsBody && m_lpVsBody->RootGroup() && m_osgGripperMT.valid())
		if(!m_lpVsBody->RootGroup()->containsNode(m_osgGripperMT.get()))
			m_lpVsBody->RootGroup()->addChild(m_osgGripperMT.get());
}

void VsDragger::RemoveFromScene()
{
	if(m_lpVsBody && m_lpVsBody->RootGroup() && m_osgGripperMT.valid())
		if(m_lpVsBody->RootGroup()->containsNode(m_osgGripperMT.get()))
			m_lpVsBody->RootGroup()->removeChild(m_osgGripperMT.get());
}

void VsDragger::SetupMatrix()
{
	if(m_lpVsBody && m_lpVsBody->RootGroup() && m_osgGripperMT.valid())
	{
		//This gives the radius of the boundign sphere for the selected part.
		//We will multiply that by 2 and then scale the entire dragger by that amount.
		//We will also use that setting for the minimum scale of the autotransform.
		float fltMaxDim = m_lpVsBody->Physics_GetBoundingRadius();

		Simulator *lpSim = GetSimulator();
		if(fltMaxDim > (lpSim->InverseDistanceUnits()/2.0f))
			fltMaxDim = (lpSim->InverseDistanceUnits()/2.0f);

		//Use an equation to calculate the radius here. This seemed to work best. 
		//I tried several size values and found a good radius scale that fit it. Then
		//I did a regression to find this equation of the line for the radius.
		float fltRadius = fltMaxDim*0.501794454f + 0.639290375f;

		//We are dividing the number by 100 because we are using a _sizeTransform that scales it up by 100.
		_autoTransform->setMinimumScale( (fltRadius*0.01f) );

		//We use the final matrix here instead of local matrix because the joint can have an additional offset that
		//must be taken into account when setting up the dragger.
		m_osgGripperMT->setMatrix(m_lpVsBody->FinalMatrix());
		
		//Set the matrix for the dragger back to default.
		osg::Matrix mtNull = osg::Matrix::identity();
		mtNull *= osg::Matrix::scale(fltRadius,fltRadius,fltRadius);
		this->setMatrix(mtNull);

		//Lets setup the grip to zero out the rotation of the body so that it points 
		//to the default axis of the parent it is connected to. 
		osg::Matrix tempMT = m_lpVsBody->LocalMatrix(), invMT;
		tempMT.setTrans(osg::Vec3f(0, 0, 0));
		invMT.invert(tempMT);

		//We want to scale the translate axis to be 1.5 times as big as the rotate axis dragger.
		// Scale the translate dragger up a bit, otherwise the axes
		// will be in the trackball dragger's sphere and we won't be
		// able to pick them.
		float axesScale = 1.5;
		invMT = invMT * osg::Matrix::scale(axesScale,axesScale,axesScale);

		_transDragger->setMatrix(invMT);
	}
}

	}// end Visualization
}// end VortexAnimatSim