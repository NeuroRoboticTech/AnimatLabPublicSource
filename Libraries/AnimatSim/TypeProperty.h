/**
\file	TypeProperty.h

\brief	Declares the type property class. 
**/

#pragma once

namespace AnimatSim
{

enum AnimatPropertyType
{
    Invalid,
    Boolean,
    Integer,
	Float,
	String,
	Xml
};

enum AnimatPropertyDirection
{
    Get,
    Set,
    Both
};

	/**
	\brief	Class that stores information about types for QueryProperty information. 
	
	\author	dcofer
	\date	3/16/2011
	**/
	class ANIMAT_PORT TypeProperty 
	{
	protected:

	public:
		TypeProperty(std::string strName, AnimatPropertyType eType, AnimatPropertyDirection eDirection);
		virtual ~TypeProperty();

		virtual std::string TypeName();
		virtual std::string DirectionName();

		std::string m_strName;
		AnimatPropertyType m_eType;
		AnimatPropertyDirection m_eDirection;
	};

}				//AnimatSim
