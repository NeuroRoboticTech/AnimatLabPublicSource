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
			string m_strProjectPath;

			/// The string neural network file name
			string m_strNeuralNetworkFile;

			/// The pointer to the class factory for this module
			IStdClassFactory *m_lpClassFactory;

			/// An array of source adapters for this module.
			CStdArray<Adapter *> m_arySourceAdapters;

			/// An array of target adapters for this module.
			CStdArray<Adapter *> m_aryTargetAdapters;

			/// Number of target adapters
			short m_iTargetAdapterCount;

		public:
			NeuralModule();
			virtual ~NeuralModule();

			virtual string ModuleName() = 0;

			virtual string ProjectPath();
			virtual void ProjectPath(string strPath);

			virtual string NeuralNetworkFile();
			virtual void NeuralNetworkFile(string strFile);

			virtual short TimeStepInterval();
			virtual void TimeStepInterval(short iVal);

			virtual float TimeStep();
			virtual void TimeStep(float fltVal);

			virtual IStdClassFactory *ClassFactory();

			Simulator *GetSimulator();
			Organism *GetOrganism();

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode);
			virtual void VerifySystemPointers();

			virtual void AttachSourceAdapter(Adapter *lpAdapter);
			virtual void AttachTargetAdapter(Adapter *lpAdapter);

			virtual BOOL NeedToStep();
			virtual void StepSimulation();
		};

	}			//Behavior
}			//AnimatSim
