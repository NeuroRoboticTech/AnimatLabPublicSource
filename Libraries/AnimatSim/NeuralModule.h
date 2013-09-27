/**
\file	NeuralModule.h

\brief	Declares the neural module class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Behavior
	{
		/**
		\class	NeuralModule
		
		\brief	Neural module.

		\details An organism is a structure with a nervous system (NervousSystem). The 
		nervous system contains one or more neural modules. A neural module performs the processing
		for a given neural model library (firing rate, integrate and fire, etc.). Each neural module
		can operate with an independent time step. Elements in one module can interact with elements in 
		an another by using adapters to connect them. The module has a list of target adapters. At each 
		time step of the module it calls StepSimulation to add an external value to the target node in 
		another module.
		
		\author	dcofer
		\date	2/24/2011
		**/
		class ANIMAT_PORT NeuralModule : public AnimatBase 
		{
		protected:
			/// The pointer to the organism
			Organism *m_lpOrganism; 

			/// Zero-based integer index of the time step interval. This is the number of time slices between
			/// that this module must wait before stepping again.
			short m_iTimeStepInterval;  

			/// The DT time step for this neural module in seconds. 
			float m_fltTimeStep;

			/// Count variable that keeps track of how many slices have occured since the last StepSimulation
			// of this module. This is zeroed back out at the next step of this module.
			short m_iTimeStepCount;

			/// Full pathname of the string project file for this module.
			std::string m_strProjectPath;

			/// The string neural network file name
			std::string m_strNeuralNetworkFile;

			/// The pointer to the class factory for this module
			IStdClassFactory *m_lpClassFactory;

			/// An array of source adapters for this module.
			CStdArray<Adapter *> m_arySourceAdapters;

			/// An array of target adapters for this module.
			CStdArray<Adapter *> m_aryTargetAdapters;

			/// Number of target adapters
			short m_iTargetAdapterCount;

			virtual int FindAdapterListIndex(CStdArray<Adapter *> aryAdapters, std::string strID, bool bThrowError = true);

		public:
			NeuralModule();
			virtual ~NeuralModule();

			/**
			\brief	Gets the module name.
			
			\author	dcofer
			\date	3/18/2011
			
			\return	Module name.
			**/
			virtual std::string ModuleName() = 0;

			virtual short TimeStepInterval();
			virtual void TimeStepInterval(short iVal);

			virtual float TimeStep();
			virtual void TimeStep(float fltVal);

			virtual IStdClassFactory *ClassFactory();
			virtual void ClassFactory(IStdClassFactory *lpFactory);

			Simulator *GetSimulator();
			Organism *GetOrganism();

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

			/**
			\brief	Attaches a source adapter.

			\details This adds the specified adapter to this module. It can then process the adapters during a step of the simulation.

			\author	dcofer
			\date	3/18/2011

			\param [in,out]	lpAdapter	Pointer to an adapter. 
			**/
			virtual void AttachSourceAdapter(Adapter *lpAdapter);

			/**
			\brief	Removes a source adapter.

			\details This removes the specified adapter from this module.

			\author	dcofer
			\date	3/18/2011

			\param [in,out]	lpAdapter	Pointer to an adapter. 
			**/
			virtual void RemoveSourceAdapter(Adapter *lpAdapter);

			/**
			\brief	Attaches a target adapter.

			\details This adds the specified adapter to this module. It can then process the adapters during a step of the simulation.

			\author	dcofer
			\date	3/18/2011

			\param [in,out]	lpAdapter	Pointer to an adapter. 
			**/
			virtual void AttachTargetAdapter(Adapter *lpAdapter);

			/**
			\brief	Removes a target adapter.

			\details This removes the specified adapter from this module.

			\author	dcofer
			\date	3/18/2011

			\param [in,out]	lpAdapter	Pointer to an adapter. 
			**/
			virtual void RemoveTargetAdapter(Adapter *lpAdapter);

			virtual float *GetDataPointer(const std::string &strDataType);

			virtual void Initialize();
			virtual bool NeedToStep(bool bIncrement);
			virtual void ResetStepCounter();
			virtual void StepSimulation();

			/**
			\brief	Step adapters.
			
			\author	dcofer
			\date	2/25/2012
			**/
			virtual void StepAdapters();
		};

	}			//Behavior
}			//AnimatSim
