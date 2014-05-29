/**
\file	MemoryChart.h

\brief	Declares the memory chart class.
**/

#pragma once

namespace AnimatSim
{
	namespace Charting
	{
		/**
		\brief	Saves the data in memory.

		\details This chart is primarily used by the GUI It keeps the data buffer filled up. If the 
		buffer starts to run over it zeros out the data and starts back at the beginning. This allows the
		AnimatGUI::Tools::DataChart to poll the MemoryChart on a regular bases so it can suck the data
		up. When it does this it transfers the stored data from the MemoryChart to the GUI chart and erases it
		from the MemoryChart and starts collecting all over again. If for some reason the GUI chart does not
		retrieve the data before the buffer is about to fill up, then we start the collection over so we do 
		not get a buffer overrun, and that data would be lost to the GUI chart.
		
		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT MemoryChart : public DataChart  
		{
		protected:
			/// Critical section to lock access to the data buffer for writing.
			CStdCriticalSection *m_oRowCountLock;

		public:
			MemoryChart();
			virtual ~MemoryChart();

			static MemoryChart *CastToDerived(AnimatBase *lpBase) {return static_cast<MemoryChart*>(lpBase);}

			virtual std::string Type();

			virtual bool Lock();
			virtual void Unlock();

			virtual void Initialize();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Charting
}				//AnimatSim
