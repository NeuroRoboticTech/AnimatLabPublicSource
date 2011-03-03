// VsHudText.cpp: implementation of the VsHudText class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsSimulator.h"
#include "VsHudText.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsHudText::VsHudText()
{
	//Default color is white
	m_aryColor.Set(1, 1, 1, 1);
	m_ptPosition.Set(10,10, 0); //Default to the lower left corner
	m_strFont = "fonts/Arial.ttf";
	m_iCharSize = 30;
	m_strText = "";
	m_lpData = NULL;
}

VsHudText::VsHudText(float *aryColor, CStdFPoint &ptPosition, string strFont, int iCharSize, string strText, string strTargetID, string strDataType)
{
	m_aryColor.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	m_ptPosition = ptPosition;
	m_strFont = strFont;
	m_iCharSize = iCharSize;
	m_strText = strText;
	m_strTargetID = strTargetID;
	m_strDataType = strDataType;
}

VsHudText::~VsHudText()
{
}

void VsHudText::Initialize(osg::Projection *lpProjection)
{
	AnimatBase *lpBase = m_lpSim->FindByID(m_strTargetID);
	m_lpData = lpBase->GetDataPointer(m_strDataType);

	m_osgText = new osgText::Text;
	m_osgText->setDataVariance(osg::Object::DYNAMIC);
	m_osgText->setColor(osg::Vec4(m_aryColor[0], m_aryColor[1], m_aryColor[2], m_aryColor[3]));
	m_osgText->setPosition(osg::Vec3(m_ptPosition.x, m_ptPosition.y, m_ptPosition.z));
	m_osgText->setFont(m_strFont.c_str());
	m_osgText->setCharacterSize(m_iCharSize); 
	m_osgText->setText(""); 

	m_osgGeode = new osg::Geode;
    m_osgGeode->addDrawable(m_osgText.get());
    lpProjection->addChild(m_osgGeode.get());
}

void VsHudText::Update()
{
    char str[1024];

	if(m_osgText.valid() && m_lpData)
	{
		sprintf(str, m_strText.c_str(), *m_lpData);
		m_osgText->setText(str);
	}
}

void VsHudText::Load(CStdXml &oXml)
{
	VsHudItem::Load(oXml);

	oXml.IntoElem();

	m_aryColor.Load(oXml, "Color", FALSE);
	Std_LoadPoint(oXml, "Position", m_ptPosition, FALSE);
	m_strFont = oXml.GetChildString("Font", m_strFont);
	m_iCharSize = oXml.GetChildInt("CharSize", m_iCharSize);
	m_strText = oXml.GetChildString("Text", m_strText);
	m_strTargetID = oXml.GetChildString("TargetID");
	m_strDataType = oXml.GetChildString("DataType");

	oXml.OutOfElem();
}

	}			// Visualization
}				//VortexAnimatSim
