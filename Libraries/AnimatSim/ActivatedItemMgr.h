/**
\file	ActivatedItemMgr.h

\brief	Declares the activated item manager class. 
**/

#pragma once

namespace AnimatSim
{
	/**
	\class	ActivatedItemMgr
	
	\brief	Base manager class for ActivatedItem's.

	\details Activated items are types of objects that are activated and deactivated 
	at specific points in time. Examples of these types of objects are DataChart's and 
	ExternalStimulus. This base class is used to manage the loading, initialization,
	and activation/deactivation of those items. For each specific type of ActivateItem 
	you will need an derived manager class for those types. For example, to manage data charts
	you will need the DataChartMgr.<br><br>This class keeps a list of all activated items that
	are added to it and at each time step of the simulation it determines which of these items 
	need to be activated or deactivated. 
	
	\author	dcofer
	\date	3/1/2011
	**/
	class ANIMAT_PORT ActivatedItemMgr : public AnimatBase    
	{
	protected:
		/// The list of activated items. This is the list of unsorted pointers. Items put
		/// into this list will be destroyed when the list is destroyed, so be careful 
		/// what you place in or remove from this list.
		CStdArray<ActivatedItem *> m_aryItems;

		/// This is the sorted mpa of activated items. It is indexed based on the unique ID value.
		/// This allows us to easily find any ActivatedItem based on its ID. The pointers in this 
		/// list are a duplicate of the ones in m_aryItems. They are <b>NOT</b> deleted when the list
		/// is destroyed.
		CStdPtrMap<std::string, ActivatedItem> m_aryItemsMap;

	public:
		ActivatedItemMgr();
		virtual ~ActivatedItemMgr();

		virtual void Add(ActivatedItem *lpItem);
		virtual void Remove(std::string strID, bool bThrowError = true);
		virtual ActivatedItem *Find(std::string strID, int &iIndex, bool bThrowError);
		virtual ActivatedItem *Find(std::string strID, bool bThrowError = true);
		virtual int FindListPos(std::string strID, bool bThrowError = true);

		virtual void Reset();
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void ReInitialize();
		virtual void StepSimulation();
	};

}			//AnimatSim
