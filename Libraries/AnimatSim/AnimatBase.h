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
		 /// The pointer to a Simulation
		Simulator *m_lpSim;

		/// The pointer to this items parent Structure. If this is not relevant for this object then this is NULL
		Structure *m_lpStructure;

		/// The pointer to this items parentNeuralModule. If this is not relevant for this object then this is NULL
		NeuralModule *m_lpModule; 

		/// The pointer to this items parent Node. If this is not relevant for this object then this is NULL
		Node *m_lpNode; 

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

		virtual Simulator *GetSimulator();
		virtual Structure *GetStructure();
		virtual NeuralModule *GetNeuralModule();
		virtual Node *GetNode();

		virtual string ID() ;
		virtual void ID(string strValue);

		virtual string Name();
		virtual void Name(string strValue);

		virtual string Type();
		virtual void Type(string strValue);

		virtual BOOL Selected();
		virtual void Selected(BOOL bValue, BOOL bSelectMultiple);

#pragma region DataAccesMethods

		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode);
		virtual float *GetDataPointer(string strDataType);
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
		virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

#pragma region SimulationMethods

		/**
		\fn	virtual void AnimatBase::Reset()
		
		\brief	Resets this object.

		\deatils Call this method to reset all data for this object back to
		its pre-loaded state.
		
		\author	dcofer
		\date	3/1/2011
		**/
		virtual void Reset() {};

		/**
		\fn	virtual void AnimatBase::Initialize(Simulator *lpSim)
		
		\brief	Initializes this object. 
		
		\author	dcofer
		\date	3/1/2011
		
		\param [in,out]	lpSim	The pointer to a simulation. 
		**/
		virtual void Initialize(Simulator *lpSim) {};

		/**
		\fn	virtual void AnimatBase::ResetSimulation(Simulator *lpSim)
		
		\brief	Resets the simulation back to time 0. 
		
		\details This method calls the ResetSimulation method on all subitems in order
		to reset the simulation back to the beginning.

		\author	dcofer
		\date	3/1/2011
		
		\param [in,out]	lpSim	The pointer to a simulation. 
		**/
		virtual void ResetSimulation(Simulator *lpSim) {};

		virtual void AfterResetSimulation(Simulator *lpSim, Structure *lpStructure) {};

		/**
		\fn	virtual void AnimatBase::ReInitialize(Simulator *lpSim)
		
		\brief	Re-initialize this object. 
		
		\author	dcofer
		\date	3/1/2011
		
		\param [in,out]	lpSim	The pointer to a simulation. 
		**/
		virtual void ReInitialize(Simulator *lpSim) {};

		/**
		\fn	virtual void AnimatBase::StepSimulation(Simulator *lpSim)
		
		\brief	Step the simulation for this object.

		\details This is called on an object each time it is stepped in the simulation.
		this is where its simulation code is processed. However, StepSimulation is not 
		necessarily called every single time that the simulation as a whole is stepped. 
		A good example of this is that neural modules can have different integration time
		steps. So a firing rate module may have a DT of 0.5 ms, while an integrate and fire
		model may have one of 0.1 ms. So the firing rate module would only get its StepSimulation
		method called every 5th time that the other module was called. This is all handed in 
		the StepSimulation method of the Simulator and NervousSystem.
		
		\author	dcofer
		\date	3/1/2011
		
		\param [in,out]	lpSim	The pointer to a simulation. 
		**/
		virtual void StepSimulation(Simulator *lpSim) {};

		/**
		\fn	virtual void AnimatBase::SimStarting()
		
		\brief	Called just before the simulation starts.

		\details This method is called on each AnimatBase object when the simulation starts.
		It allows it to perform any intialization prior to the beginning of the simulation that is needed.
		
		\author	dcofer
		\date	3/1/2011
		**/
		virtual void SimStarting() {};

		/**
		\fn	virtual void AnimatBase::SimPausing()
		
		\brief	Called just before the simulation pauses.

		\details This method is called on each AnimatBase object when the simulation pauses.
		It allows it to perform any intialization prior to the pause of the simulation that is needed.
		
		\author	dcofer
		\date	3/1/2011
		**/
		virtual void SimPausing() {};

		/**
		\fn	virtual void AnimatBase::SimStopping()
		
		\brief	Called just before the simulation stops.

		\details This method is called on each AnimatBase object when the simulation stops.
		It allows it to perform any intialization prior to the stop of the simulation that is needed.
		
		\author	dcofer
		\date	3/1/2011
		**/
		virtual void SimStopping() {};

#pragma endregion

#pragma region SnapshotMethods

		/**
		\fn	virtual long AnimatBase::CalculateSnapshotByteSize()
		
		\brief	Calculates the snapshot byte size.
		
		\details Sometimes the user may want to capture a snapshot of the simulation at a given point in
		time, and then be able to go back to that specific point. To do this we grab a snapshot of all
		the data in the system, including the neural variables. We essentially serialize the data into a
		binary format for later re-use. This method calculates the number of bytes that will be required
		to store the entire object. 
		
		\author	dcofer
		\date	2/24/2011
		
		\return	The calculated snapshot byte size. 
		**/
		virtual long CalculateSnapshotByteSize() {return 0;};

		/**
		\fn	virtual void AnimatBase::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
		
		\brief	Saves a key frame snapshot.
		
		\details Sometimes the user may want to capture a snapshot of the simulation at a given point in
		time, and then be able to go back to that specific point. To do this we grab a snapshot of all
		the data in the system, including the neural variables. We essentially serialize the data into a
		binary format for later re-use. This method goes through each module and saves its data into the
		byte array. 
		
		\author	dcofer
		\date	2/24/2011
		
		\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
		\param [in,out]	lIndex		Current zero-based index of the write position in the array. 
		**/
		virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex) {};

		/**
		\fn	virtual void AnimatBase::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
		
		\brief	Loads a key frame snapshot.
		
		\details Sometimes the user may want to capture a snapshot of the simulation at a given point in
		time, and then be able to go back to that specific point. To do this we grab a snapshot of all
		the data in the system, including the neural variables. We essentially serialize the data into a
		binary format for later re-use. This method goes through each module and loads its data from the
		byte array. 
		
		\author	dcofer
		\date	2/24/2011
		
		\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
		\param [in,out]	lIndex		Current zero-based index of the read position in the array. 
		**/
		virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex) {};

#pragma endregion

		/**
		\fn	virtual void AnimatBase::VisualSelectionModeChanged(int iNewMode)
		
		\brief	Visual selection mode changed. 
		
		\details This is called whenever the visual selection mode of the simulation is changed.
		This is when the user switches from selecting graphics, collision objects, joints, etc..

		\author	dcofer
		\date	3/1/2011
		
		\param	iNewMode	The new mode. 
		**/
		virtual void VisualSelectionModeChanged(int iNewMode) {};

		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
