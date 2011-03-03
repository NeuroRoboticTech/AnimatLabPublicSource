// VsHud.cpp: implementation of the VsHud class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsHud::VsHud()
{
}

VsHud::~VsHud()
{

try
{
	m_aryHudItems.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsHud\r\n", "", -1, FALSE, TRUE);}
}

void VsHud::Reset()
{
	m_aryHudItems.RemoveAll();
}

void VsHud::Initialize()
{
	m_osgProjection = new osg::Projection;
    m_osgProjection->setMatrix(osg::Matrix::ortho2D(0, 800, 0, 600));

	VsHudItem *lpItem = NULL;
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
	VsHudItem *lpItem = NULL;
	int iCount = m_aryHudItems.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryHudItems[iIndex];
		lpItem->Update();
	}
}

void VsHud::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_aryHudItems.RemoveAll();

	if(oXml.FindChildElement("HudItems", false))
	{
		//*** Begin Loading HudItems. *****
		oXml.IntoChildElement("HudItems");

		int iCount = oXml.NumberOfChildren();
		VsHudItem *lpItem = NULL;
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			lpItem = LoadHudItem(oXml);
			m_aryHudItems.Add(lpItem);
		}

		oXml.OutOfElem();
		//*** End Loading HudItems. *****
	}
}

VsHudItem *VsHud::LoadHudItem(CStdXml &oXml)
{
	VsHudItem *lpItem=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpItem = dynamic_cast<VsHudItem *>(m_lpSim->CreateObject(strModuleName, "HudItem", strType));
	if(!lpItem)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "HudItem");

	lpItem->SetSystemPointers(m_lpSim, NULL, NULL, NULL);
	lpItem->Load(oXml);

	return lpItem;
}
catch(CStdErrorInfo oError)
{
	if(lpItem) delete lpItem;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpItem) delete lpItem;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


	}			// Visualization
}				//VortexAnimatSim
