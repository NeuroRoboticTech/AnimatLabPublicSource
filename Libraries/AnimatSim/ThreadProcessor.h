/**
\file	ThreadProcessor.h

\brief	Declares the thread processing class. 
**/

#pragma once

namespace AnimatSim
{
	/**
	\class	ThreadProcessor
		
	\brief	Handles processing of a single thread of the simulation. 

	\details The simulation is split up among several threads. All neural modules are processed in one thread, physics in another, and 
    IO communications in another if needed. This class is in charge of coordinating all actions within a single thread. 
		
	\author	dcofer
	\date	4/14/2014
	**/
	class ANIMAT_PORT ThreadProcessor : public AnimatBase 
	{
	protected:
		/// An Array of module thread processors that will be run within this thread.
		CStdArray<ModuleThreadProcessor *> m_aryModules;

        virtual int FindModuleProcessorIndex(std::string strModuleName);

	public:
		ThreadProcessor();
		virtual ~ThreadProcessor();
						
		static ThreadProcessor *CastToDerived(AnimatBase *lpBase) {return static_cast<ThreadProcessor*>(lpBase);}

        virtual void CreateModuleProcessor(std::string strModuleName);
        virtual void RemoveModuleProcessor(std::string strModuleName);

        virtual void StartThread();
        virtual void EndThread();

        virtual void StepSimulation();
	};

}			//AnimatSim
