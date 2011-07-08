/**
\file	VsHud.cpp

\brief	Implements the vortex heads-up display class.
**/

#include "StdAfx.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{
/**
\brief	Default constructor.

\author	dcofer
\date	7/7/2011
**/
VsHud::VsHud()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	7/7/2011
**/
VsHud::~VsHud()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsHud\r\n", "", -1, FALSE, TRUE);}
}

void VsHud::Initialize()
{
	AnimatBase::Initialize();

	m_osgProjection = new osg::Projection;
    m_osgProjection->setMatrix(osg::Matrix::ortho2D(0, 800, 0, 600));

	HudItem *lpItem = NULL;
	int iCount = m_aryHudItems.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryHudItems[iIndex];
		lpItem->Initialize(m_osgProjection.get());
	}

	m_osgMT = new osg::MatrixTransform;
    m_osgMT->setReferenceFrame(osg::Transform::ABSOLUTE_RF);
    m_osgMT->getOrCreateStateSet()->setMode(GL_LIGHTING, osg::StateAttribute::OFF);
    m_osgMT->getOrCreateStateSet()->setAttributeAndModes(new osg::Program, osg::StateAttribute::ON);
    m_osgMT->addChild(m_osgProjection.get());

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	lpVsSim->OSGRoot()->addChild(m_osgMT.get());
}

void VsHud::Update()
{
	HudItem *lpItem = NULL;
	int iCount = m_aryHudItems.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryHudItems[iIndex];
		lpItem->Update();
	}
}


	}			// Visualization
}				//VortexAnimatSim
