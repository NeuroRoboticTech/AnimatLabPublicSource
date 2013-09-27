/**
\file	StdClassFactory.h

\brief	Declares the standard class factory class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Standard class factory. 

\details This is a standard interface used for all class factories. To make your library able able to be loaded 
you need to derive a class from this and then implement the GetStdClassFactory method within your new DLL.

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT IStdClassFactory 
{
public:
	IStdClassFactory();
	virtual ~IStdClassFactory();

	/**
	\brief	Creates an object of the specified class and object types.
	
	\author	dcofer
	\date	5/3/2011
	
	\param	strClassType 	Type of the class. 
	\param	strObjectType	Type of the object. 
	\param	bThrowError  	true to throw error if there is a problem. 
	
	\return	null if it fails and bThrowError is false, else a pointer to the created object.
	**/
	virtual CStdSerialize *CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError = true) = 0;

	static IStdClassFactory *LoadModule(std::string strModuleName);
};

typedef IStdClassFactory *(*GetClassFactory)(void); 

}				//StdUtils

