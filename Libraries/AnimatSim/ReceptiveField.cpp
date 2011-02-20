// ReceptiveField.cpp: implementation of the ReceptiveField class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"
#include "ReceptiveField.h"

namespace AnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a CollisionPair object..
   		
   \param lpParent This is a pointer to the parent of this rigid body. 
	          If this value is null then it is assumed that this is
						a root object and no joint is loaded to connect this
						part to the parent.

	 \return
	 No return value.

   \remarks
	 The constructor for a CollisionPair. 
*/

ReceptiveField::ReceptiveField()
{
	m_vVertex[0] = 0; m_vVertex[1] = 0; m_vVertex[2] = 0;
	m_fltCurrent = 0;
}

ReceptiveField::ReceptiveField(float fltX, float fltY, float fltZ, float fltCurrent)
{
	m_vVertex[0] = fltX; m_vVertex[1] = fltY; m_vVertex[2] = fltZ;
	m_fltCurrent = fltCurrent;
}

/*! \brief 
   Destroys the CollisionPair object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the CollisionPair object..	 
*/

ReceptiveField::~ReceptiveField()
{
}

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

BOOL ReceptiveField::operator>(ReceptiveField *lpItem)
{return !this->operator<(lpItem);}

BOOL ReceptiveField::operator==(ReceptiveField *lpItem)
{
	if( fabs(this->m_vVertex[0] - lpItem->m_vVertex[0]) < 1e-4 &&
		  fabs(this->m_vVertex[1] - lpItem->m_vVertex[1]) < 1e-4 &&
			fabs(this->m_vVertex[2] - lpItem->m_vVertex[2]) < 1e-4)
			return true;

	return false;
}

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

BOOL ReceptiveField::GreaterThanThan(float fltX, float fltY, float fltZ)
{return !this->LessThanThan(fltX, fltY, fltZ);}

BOOL ReceptiveField::Equals(float fltX, float fltY, float fltZ)
{
	if( fabs(this->m_vVertex[0] - fltX) < 1e-4 &&
		  fabs(this->m_vVertex[1] - fltY) < 1e-4 &&
			fabs(this->m_vVertex[2] - fltZ) < 1e-4)
			return true;

	return false;
}

void ReceptiveField::Trace(ostream &oOs)
{
	oOs << this->m_vVertex[0] << "," << this->m_vVertex[1] << "," << this->m_vVertex[2];
}
	}			//Environment
}				//AnimatSim