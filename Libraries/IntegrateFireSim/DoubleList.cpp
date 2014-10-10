/**
\file	DoubleList.cpp

\brief	Implements the double list class.
**/

#include "stdafx.h"

namespace IntegrateFireSim
{
	namespace Utilities
	{
/**
\brief	Adds a head. 

\author	dcofer
\date	3/31/2011

\param	d	The. 

\return	index.
**/
int DoubleList::AddHead(double d)		// add to start of list, reset iterator
{
	listelem *temp=new listelem;
	if (temp==0)
		return 0;
	if (head==0)			// list empty, so addition will be first & last item
		tail=temp;			// keep track of tail of list
	temp->next=head;		// next pointer of new element points to end of list
	temp->data=d;
	cursor=head=temp;				// head of list pointer points to new item
	return 1;
}

// return 0 if ENOMEM

/**
\brief	Adds a tail. 

\author	dcofer
\date	3/31/2011

\param	d	The. 

\return	index.
**/
int DoubleList::AddTail(double d)		// add to end of list, reset iterator
{
	listelem* temp=new listelem;
	if (temp==0)
		return 0;

	temp->next=0;
	if (head==0)			// list empty, so addition will be first & last item
		cursor=head=tail=temp;			// keep track of head of list
	else {
		tail->next=temp;
		tail=temp;
		}
	tail->data=d;
	return 1;
}

/**
\brief	Deletes this object.

\author	dcofer
\date	3/31/2011

\return	.
**/
int DoubleList::Del(void)		// delete 1st item on list, reset iterator
{
	if (head==0)
		return 0;		// empty list
	
	listelem* temp=head;
	cursor=head=head->next;
	delete temp;
	return 1;
}

/**
\brief	Releases this object.

\author	dcofer
\date	3/31/2011
**/
void DoubleList::Release(void)
{
	while (head!=0)
		Del();
	cursor=0;
}

// Starts at head of list, returns pointer to data item, advances to next
// item.  When at end, returns 0 and resets to start

/**
\brief	Gets the iterate.

\author	dcofer
\date	3/31/2011

\return	null if it fails, else.
**/
double *DoubleList::Iterate(void)
{
	if (cursor==0) 
	{
		cursor=head;
		return 0;
	}
	double *d=&cursor->data;
	cursor=cursor->next;
	return d;
}

/**
\brief	Removes the head.

\author	dcofer
\date	3/31/2011

\return	.
**/
double DoubleList::RemoveHead()
{
	//ASSERT(IsEmpty()==false);
	double d=head->data;
	listelem* temp=head;
	cursor=head=head->next;
	delete temp;
	return d;
}	

	}			//Utilities
}				//IntegrateFireSim
