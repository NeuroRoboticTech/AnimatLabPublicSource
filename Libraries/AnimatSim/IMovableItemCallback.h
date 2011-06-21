#pragma once

namespace AnimatSim
{


	/**
	\brief	Movable Item callback to the GUI. 

	\details How the Movable Item callback works: I have this interface defined within AnimatSim. It contains NO references
	to any managed code, so it is still STL compliant and is not used if we are running stand-alone without a gui.
	If we are running a gui then when the simulation is created each object creates a managed ManagedAnimatTools.DataObjectInterface
	class that is used to send data down from the managed GUI to the unmanaged sim. If the part it is talking to is a MovableItem
	or a structure then it also creates a new unmanaged ManagedAnimatTools::MovableItemCallback class that implements the 
	IMovableItemCallback interface. It contains a gcroot pointer to its associated DataObjectInterface and callback methods
	for PositionChanged, RotationChanged, and SelectionChanged. When the user clicks on an object in the sim window it is picked
	and the Select method is called. That method checks to see if the pointer to the IMovableItemCallback is not null. If it is not
	then it calls the SelectionChanged method, which calls that method on the DataObjectInterface, which fires the SelectionChangedEvent
	of that MovableItem. However, there was a wrinkle in that any UI class has to be able to handle multi-threaded operations.
	Initially the treecontrol from DotNetMagic could not do that. I added code in the SelectNode to check if an Invoke was required.
	if it was then it did a begininvoke to re-fire the method using the UI thread.

	\author	dcofer
	\date	2/25/2011
	**/
	class ANIMAT_PORT IMovableItemCallback
	{
	protected:

	public:
		IMovableItemCallback(void);
		virtual ~IMovableItemCallback(void);

		/**
		\brief	Called to signal to the GUI that the position of the body part changed.
		
		\author	dcofer
		\date	3/25/2011
		**/
		virtual void PositionChanged() = 0;

		/**
		\brief	Called to signal to the GUI that the rotation of the body part changed.
		
		\author	dcofer
		\date	3/25/2011
		**/
		virtual void RotationChanged() = 0;

		/**
		\brief	Called to signal to the GUI that the selected body part changed.
		
		\author	dcofer
		\date	3/25/2011
		
		\param	bSelected	   	true if it was selected. 
		\param	bSelectMultiple	true if multiple items were selected. 
		**/
		virtual void SelectionChanged(BOOL bSelected, BOOL bSelectMultiple) = 0;

		/**
		\brief	Called to signal to the GUI that a part was clicked while AddBody mode was active.
		
		\author	dcofer
		\date	3/25/2011
		
		\param	fltPosX 	The x coordinate of the location clicked. 
		\param	fltPosY 	The y coordinate of the location clicked. 
		\param	fltPosZ 	The z coordinate of the location clicked. 
		\param	fltNormX	The x normal of the location clicked. 
		\param	fltNormY	The y normal of the location clicked. 
		\param	fltNormZ	The z normal of the location clicked. 
		**/
		virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ) = 0;

		/**
		\brief	Called to signal when the selected vertex has changed.
		
		\author	dcofer
		\date	6/21/2011
		
		\param	fltPosX	The position x coordinate.
		\param	fltPosY	The position y coordinate.
		\param	fltPosZ	The position z coordinate.
		**/
		virtual void SelectedVertexChanged(float fltPosX, float fltPosY, float fltPosZ) = 0;

	};
}