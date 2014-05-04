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
		/// Tells if this item is enabled or not. If it is not enabled then it is not run.
		bool m_bEnabled;

		 /// The pointer to a Simulation
		Simulator *m_lpSim;

		/// The pointer to this items parent Structure. If this is not relevant for this object then this is NULL
		AnimatSim::Environment::Structure *m_lpStructure;

		/// The pointer to this items parentNeuralModule. If this is not relevant for this object then this is NULL
		AnimatSim::Behavior::NeuralModule *m_lpModule; 

		/// The pointer to this items parent Node. If this is not relevant for this object then this is NULL
		Node *m_lpNode; 

		///The unique Id for this object. 
		std::string m_strID;  

		///The type for this object. Examples are Box, Plane, Neuron, etc.. 
		std::string m_strType;  

		///The name for this object. 
		std::string m_strName;  

		///Tells whether the object is selected or not
		bool m_bSelected;

	public:
		AnimatBase();
		virtual ~AnimatBase();

		virtual Simulator *GetSimulator();
		virtual Structure *GetStructure();
		virtual NeuralModule *GetNeuralModule();
		virtual Node *GetNode();

		virtual void Enabled(bool bVal);
		virtual bool Enabled();

		virtual std::string ID() ;
		virtual void ID(std::string strValue);

		virtual std::string Name();
		virtual void Name(std::string strValue);

		virtual std::string Type();
		virtual void Type(std::string strValue);

		virtual bool Selected();
		virtual void Selected(bool bValue, bool bSelectMultiple);

#pragma region DataAccesMethods

		/**
		\brief	Sets the system pointers.
		
		\details There are a number of system pointers that are needed for use in the objects. The
		primariy one being a pointer to the simulation object itself so that you can get global
		parameters like the scale units and so on. However, each object may need other types of pointers
		as well, for example neurons need to have a pointer to their parent structure/organism, and to
		the NeuralModule they reside within. So different types of objects will need different sets of
		system pointers. We call this method to set the pointers just after creation and before Load is
		called. We then call VerifySystemPointers here, during Load and during Initialize in order to
		ensure that the correct pointers have been set for each type of objects. These pointers can then
		be safely used throughout the rest of the system. 
		
		\author	dcofer
		\date	3/2/2011
		
		\param [in,out]	lpSim		The pointer to a simulation. 
		\param [in,out]	lpStructure	The pointer to the parent structure. 
		\param [in,out]	lpModule	The pointer to the parent module module. 
		\param [in,out]	lpNode		The pointer to the parent node. 
		\param	bVerify				true to call VerifySystemPointers. 
		**/
		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
		virtual void VerifySystemPointers();
		virtual float *GetDataPointer(const std::string &strDataType);
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		virtual bool HasProperty(const std::string &strName);
		virtual AnimatPropertyType PropertyType(const std::string &strName);
		virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
		virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

        virtual void RigidBodyAdded(std::string strID) {};
        virtual void RigidBodyRemoved(std::string strID) {};

#pragma endregion

#pragma region SimulationMethods

		virtual void Reset();
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void AfterResetSimulation();
		virtual void ReInitialize();
		virtual void Kill(bool bState = true);
		virtual void StepSimulation();

		virtual void SimStarting();
		virtual void SimPausing();
		virtual void SimStopping();

		virtual void TimeStepModified();

#pragma endregion

#pragma region SnapshotMethods

		virtual long CalculateSnapshotByteSize();
		virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
		virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

#pragma endregion

		virtual void VisualSelectionModeChanged(int iNewMode);
		virtual void Load(StdUtils::CStdXml &oXml);
	};

}				//AnimatSim
