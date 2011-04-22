/**
\file	ReceptiveField.cpp

\brief	Implements the receptive field class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"
#include "ReceptiveField.h"

namespace AnimatSim
{
	namespace Environment
	{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
ReceptiveField::ReceptiveField()
{
	m_vVertex[0] = 0; m_vVertex[1] = 0; m_vVertex[2] = 0;
	m_fltCurrent = 0;
}

/**
\brief	Constructor.

\author	dcofer
\date	3/24/2011

\param	fltX	  	The x coordinate of the center of the receptive field. 
\param	fltY	  	The y coordinate of the center of the receptive field. 
\param	fltZ	  	The z coordinate of the center of the receptive field. 
\param	fltCurrent	The current value. 
**/
ReceptiveField::ReceptiveField(float fltX, float fltY, float fltZ, float fltCurrent)
{
	m_vVertex[0] = fltX; m_vVertex[1] = fltY; m_vVertex[2] = fltZ;
	m_fltCurrent = fltCurrent;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
ReceptiveField::~ReceptiveField()
{
}

/**
\brief	Method used to sort the ReceptiveField by vertex.

\details This method compares the vertex coordinates to find if this vertex is less than the one passed in.

\param [in,out]	lpItem	Item that is being compared. 

\return	true if this vertex is less than the one passed in, false if not.
**/
BOOL ReceptiveField::operator<(ReceptiveField *lpItem)
{
	if(this->m_vVertex[0] > lpItem->m_vVertex[0])
		return false;

	if(this->m_vVertex[1] > lpItem->m_vVertex[1])
		return false;

	if(this->m_vVertex[2] > lpItem->m_vVertex[2])
		return false;

	if(this->operator==(lpItem))
		return false;

	return true;
}

/**
\brief	Method used to sort the ReceptiveField by vertex.

\details This method compares the vertex coordinates to find if this vertex is more than the one passed in.

\param [in,out]	lpItem	Item that is being compared. 

\return	true if this vertex is more than the one passed in, false if not.
**/
BOOL ReceptiveField::operator>(ReceptiveField *lpItem)
{return !this->operator<(lpItem);}

/**
\brief	Checks whether the vertices of the receptive fields are the same.

\details Same is defined as within 1e-4 difference for each coordinate.

\author	dcofer
\date	3/24/2011

\param [in,out]	lpItem	Item that is being compared. 

\return	The result of the operation.
**/
BOOL ReceptiveField::operator==(ReceptiveField *lpItem)
{
	if( fabs(this->m_vVertex[0] - lpItem->m_vVertex[0]) < 1e-4 &&
		  fabs(this->m_vVertex[1] - lpItem->m_vVertex[1]) < 1e-4 &&
			fabs(this->m_vVertex[2] - lpItem->m_vVertex[2]) < 1e-4)
			return true;

	return false;
}

/**
\brief	Method used to sort the ReceptiveField by vertex.

\details This method compares the vertex coordinates to find if this vertex is less than the one passed in.

\author	dcofer
\date	3/24/2011

\param	fltX	The x coordinate to compare with. 
\param	fltY	The y coordinate to compare with. 
\param	fltZ	The z coordinate to compare with. 

\return	true if this vertex is less than the one passed in, else false.
**/
BOOL ReceptiveField::LessThanThan(float fltX, float fltY, float fltZ)
{
	//If the x values are not identical then decide if it is less than using the x value.
	if(fabs(this->m_vVertex[0] - fltX) > 1e-4)
	{
		if(this->m_vVertex[0] > fltX)
			return false;
		else
			return true;
	}

	//If the x values are identical and the y values are not identical then decide if it is less than using the y value.
	if(fabs(this->m_vVertex[1] - fltY) > 1e-4)
	{
		if(this->m_vVertex[1] > fltY)
			return false;
		else
			return true;
	}

	//And so on.
	if(fabs(this->m_vVertex[2] - fltZ) > 1e-4)
	{
		if(this->m_vVertex[2] > fltZ)
			return false;
		else
			return true;
	}

	//if we get to this point then it is only because the vertices are identical
	return false;
}

/**
\brief	Method used to sort the ReceptiveField by vertex.

\details This method compares the vertex coordinates to find if this vertex is greater than the one passed in.

\author	dcofer
\date	3/24/2011

\param	fltX	The x coordinate to compare with. 
\param	fltY	The y coordinate to compare with. 
\param	fltZ	The z coordinate to compare with. 

\return	true if this vertex is greater than the one passed in, else false.
**/
BOOL ReceptiveField::GreaterThanThan(float fltX, float fltY, float fltZ)
{return !this->LessThanThan(fltX, fltY, fltZ);}

/**
\brief	Tests if objects are considered equal.

\author	dcofer
\date	3/24/2011

\param	fltX	The x coordinate to compare with. 
\param	fltY	The y coordinate to compare with. 
\param	fltZ	The z coordinate to compare with. 

\return	true if this vertices are the same, else false.
**/
BOOL ReceptiveField::Equals(float fltX, float fltY, float fltZ)
{
	if( fabs(this->m_vVertex[0] - fltX) < 1e-4 &&
		  fabs(this->m_vVertex[1] - fltY) < 1e-4 &&
			fabs(this->m_vVertex[2] - fltZ) < 1e-4)
			return true;

	return false;
}

	}			//Environment
}				//AnimatSim