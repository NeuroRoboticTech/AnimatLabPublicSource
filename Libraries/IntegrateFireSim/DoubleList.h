
#pragma once

namespace IntegrateFireSim
{

	/**
	\namespace	IntegrateFireSim::Utilities

	\brief	Utility classes for the integrate and fire neuron model. 
	**/
	namespace Utilities
	{

		class DoubleList  
		{
		public:
			DoubleList(void) {cursor=head=tail=0;}
			~DoubleList(void) {Release();}
			int AddHead(double d);	// add to head of list
			int AddTail(double d);	// add to tail of list
			int Del(void);
			void Release(void);
			double *Iterate(void);
			double *First(void) {if (head==0) return 0; else return &head->data;}

			BOOL IsEmpty() {return head==0;}
			double GetHead() {return head->data;} 	// list must not be empty
			double RemoveHead();	// list must not be empty

		private:
			struct listelem {
				double data;
				listelem* next;
				};
			listelem* head;		// pointer to head of list
			listelem* tail;
			listelem* cursor;
		};

	}			//Utilities
}				//IntegrateFireSim
