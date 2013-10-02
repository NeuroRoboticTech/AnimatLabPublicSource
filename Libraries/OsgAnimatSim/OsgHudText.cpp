/**
\file	OsgHudText.cpp

\brief	Implements the vortex heads-up display text class.
**/

#include "StdAfx.h"
#include "OsgHudText.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

/**
\brief	Default constructor.

\author	dcofer
\date	7/7/2011
**/
OsgHudText::OsgHudText()
{
	//Default color is white
	m_aryColor.Set(1, 1, 1, 1);
	m_ptPosition.Set(10,10, 0); //Default to the lower left corner
	m_strFont = "fonts/arial.ttf";
	m_iCharSize = 30;
	m_strText = "";
	m_lpDisplayData = NULL;
	m_lpUpdateData = NULL;
	m_fltUpdateInterval = 1;
}

/**
\brief	Constructor.

\author	dcofer
\date	7/7/2011

\param [in,out]	aryColor  	If non-null, the ary color.
\param [in,out]	ptPosition	The point position.
\param	strFont			  	The string font.
\param	iCharSize		  	Size of the character.
\param	strText			  	The string text.
\param	strTargetID		  	Identifier for the string target.
\param	strDataType		  	Type of the string data.
**/
OsgHudText::OsgHudText(float *aryColor, CStdFPoint &ptPosition, std::string strFont, int iCharSize, std::string strText, std::string strDisplayTargetID, std::string strDisplayDataType, std::string strUpdateTargetID, std::string strUpdateDataType, float fltUpdateInterval)
{
	m_aryColor.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	m_ptPosition = ptPosition;
	m_strFont = strFont;
	m_iCharSize = iCharSize;
	m_strText = strText;
	m_lpDisplayData = NULL;
	m_lpUpdateData = NULL;
	m_strDisplayTargetID = strDisplayTargetID;
	m_strDisplayDataType = strDisplayDataType;
	m_strUpdateTargetID = strUpdateTargetID;
	m_strUpdateDataType = strUpdateDataType;
	m_fltUpdateInterval = fltUpdateInterval;
}

/**
\brief	Destructor.

\author	dcofer
\date	7/7/2011
**/
OsgHudText::~OsgHudText()
{
}

void OsgHudText::Initialize(void *lpVoidProjection)
{
	HudText::Initialize(lpVoidProjection);

	m_fltPrevious = -1;
	osg::Projection *lpProjection = (osg::Projection *) (lpVoidProjection);
	if(!lpProjection)
		THROW_PARAM_ERROR(Osg_Err_lHudProjectionNotDefined, Osg_Err_strHudProjectionNotDefined, "ID", m_strID);

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

void OsgHudText::ResetSimulation()
{
	m_fltPrevious = -1;
}

void OsgHudText::Update()
{
    char str[1024];

	if(m_osgText.valid() && m_lpDisplayData && m_lpUpdateData && (fabs(*m_lpUpdateData-m_fltPrevious) > 0.1) )
	{
		sprintf(str, m_strText.c_str(), *m_lpDisplayData);
		m_osgText->setText(str);
		m_fltPrevious = *m_lpUpdateData;
	}
}

	}			// Visualization
}				//OsgAnimatSim
