/**
\file	ModuleThreadProcessor.h

\brief	Declares the thread processing class for a specific module. 
**/

#pragma once

namespace AnimatSim
{
	/**
	\class	ModuleThreadProcessor
		
	\brief	Handles processing of a single module within a thread of the simulation. 

	\details The simulation is split up among several threads. All neural modules are processed in one thread, physics in another, and 
    IO communications in another if needed. Each thread has a ThreadProcessor that controls the flow of execution within that thread. 
    Each ThreadProcessor has one or more ModuleThreadProcessors that it uses to do this. The ModuleThreadProcessor does the actual work.
		
	\author	dcofer
	\date	4/14/2014
	**/
	class ANIMAT_PORT ModuleThreadProcessor : public AnimatBase 
	{
	protected:

	public:
		ModuleThreadProcessor(std::string strModuleName);
		virtual ~ModuleThreadProcessor();

        virtual void StepSimulation();
        virtual void StepAdapters();
	};

}			//AnimatSim
