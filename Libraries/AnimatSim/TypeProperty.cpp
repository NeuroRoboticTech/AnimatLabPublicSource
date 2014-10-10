/**
\file	TypeProperty.cpp

\brief	Implements the TypeProperty class. 
**/

#include "StdAfx.h"
#include "TypeProperty.h"

namespace AnimatSim
{
/**
\brief	Default constructor. 

\author	dcofer
\date	4/29/2014
**/
TypeProperty::TypeProperty(std::string strName, AnimatPropertyType eType, AnimatPropertyDirection eDirection)
{
	m_strName = strName;
	m_eType = eType;
	m_eDirection = eDirection;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
TypeProperty::~TypeProperty()
{
}

std::string TypeProperty::TypeName()
{
	switch(m_eType)
	{
	case Invalid:
		return "Invalid";
	case Boolean:
		return "Boolean";
	case Integer:
		return "Integer";
	case Float:
		return "Float";
	case String:
		return "String";
	case Xml:
		return "Xml";
	}
		
	return "Invalid";
}

std::string TypeProperty::DirectionName()
{
	switch(m_eDirection)
	{
	case Get:
		return "Get";
	case Set:
		return "Set";
	case Both:
		return "Both";
	}
		
	return "Invalid";
}


}			//AnimatSim
