// DoubleList.cpp: implementation of the DoubleList class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

namespace IntegrateFireSim
{
	namespace Utilities
	{

//////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////

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
	

int DoubleList::Del(void)		// delete 1st item on list, reset iterator
{
	if (head==0)
		return 0;		// empty list
	
	listelem* temp=head;
	cursor=head=head->next;
	delete temp;
	return 1;
}

void DoubleList::Release(void)
{
	while (head!=0)
		Del();
	cursor=0;
}

// Starts at head of list, returns pointer to data item, advances to next
// item.  When at end, returns 0 and resets to start
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

double DoubleList::RemoveHead()
{
	//ASSERT(IsEmpty()==FALSE);
	double d=head->data;
	listelem* temp=head;
	cursor=head=head->next;
	delete temp;
	return d;
}	

	}			//Utilities
}				//IntegrateFireSim
