/**
\file	AnimatBase.h

\brief	Base class file for all Animat simulation objects.
**/

#pragma once

/**
\namespace	AnimatSim

\brief	Root namespace for the base simulation library for AnimatLab.

\detils This is the root namespace for the simulation library that contains
all of the base classes used in AnimatLab. The classes and methods in this 
library contain the core functionality used throughout the entire simulation system.
If you want to create new functionality for AnimatLab simulations you will be using
the base classes from this library.
**/
namespace AnimatSim
{
	/**
	\class	AnimatBase
	
	\brief	Animat base class. 
	
	\details This class contains the base variables and methods that are used
	by all of the other animat simulation objects. For example, it contains the
	unique ID string, type, name, etc. that is used for object creation and searching.
	It also has the base methods used to set data items and add/remove new items.

	\author	dcofer
	\date	2/21/2011
	**/

	class ANIMAT_PORT AnimatBase : public CStdSerialize 
	{
	protected:
		///The unique Id for this object. 
		string m_strID;  

		///The type for this object. Examples are Box, Plane, Neuron, etc.. 
		string m_strType;  

		///The name for this object. 
		string m_strName;  

		///Tells whether the object is selected or not
		BOOL m_bSelected;

	public:
		AnimatBase();
		virtual ~AnimatBase();

		virtual string ID() ;
		virtual void ID(string strValue);

		virtual string Name();
		virtual void Name(string strValue);

		virtual string Type();
		virtual void Type(string strValue);

		virtual BOOL Selected();
		virtual void Selected(BOOL bValue, BOOL bSelectMultiple);

		//virtual AnimatBase *FindByID(string strID, BOOL bThrowError = TRUE);
#pragma region DataAccesMethods

		virtual float *GetDataPointer(string strDataType);
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
		virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

		//This is called whenever the visual selection mode of the simulation is changed.
		//This is when the user switches from selecting graphics, collision objects, joints, etc..
		virtual void VisualSelectionModeChanged(int iNewMode) {};

		//These functions are called internally when the simulation is about to start up or pause.
		virtual void SimStarting() {};
		virtual void SimPausing() {};
		virtual void SimStopping() {};

		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
