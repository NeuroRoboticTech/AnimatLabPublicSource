/**
\file	DoubleList.h

\brief	Declares the double list class.
**/

#pragma once

namespace IntegrateFireSim
{

	/**
	\namespace	IntegrateFireSim::Utilities

	\brief	Utility classes for the integrate and fire neuron model. 
	**/
	namespace Utilities
	{
		/**
		\brief	List of doubles. 
		
		\author	dcofer
		\date	3/31/2011
		**/
		class DoubleList  
		{
		public:

			/**
			\brief	Default constructor.
			
			\author	dcofer
			\date	3/31/2011
			**/
			DoubleList(void) {cursor=head=tail=0;}

			/**
			\brief	Finaliser.
			
			\author	dcofer
			\date	3/31/2011
			**/
			~DoubleList(void) {Release();}

			int AddHead(double d);	// add to head of list
			int AddTail(double d);	// add to tail of list
			int Del(void);
			void Release(void);
			double *Iterate(void);

			/**
			\brief	Gets the first in the list.
			
			\author	dcofer
			\date	3/31/2011
			
			\return	null if it fails, else.
			**/
			double *First(void) {if (head==0) return 0; else return &head->data;}

			/**
			\brief	Query if this object is empty.
			
			\author	dcofer
			\date	3/31/2011
			
			\return	true if empty, false if not.
			**/
			bool IsEmpty() {return head==0;}

			/**
			\brief	Gets the head object from this queue.
			
			\author	dcofer
			\date	3/31/2011
			
			\return	The head.
			**/
			double GetHead() {return head->data;} 	// list must not be empty

			double RemoveHead();	// list must not be empty

		private:
			struct listelem {
				double data;
				listelem* next;
				};
			/// The head
			listelem* head;		// pointer to head of list
			/// The tail
			listelem* tail;
			/// The cursor
			listelem* cursor;
		};

	}			//Utilities
}				//IntegrateFireSim
