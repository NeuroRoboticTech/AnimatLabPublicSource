/**
\file	FileChart.h

\brief	Declares the file chart class.
**/

#pragma once

namespace AnimatSim
{
	namespace Charting
	{
		/**
		\brief	File chart. 
		
		\details This class is derived from DataChart, and its purpose is to save out the data into a 
		tab seperated value (tsv) file when the chart reaches its end time and is deactivated. It rights the
		data columns out using the uer specified Index for each column. This class is primarily used when running
		the simulation in stand-alone mode without any GUI. The GUI uses the MemoryChart instead.

		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT FileChart : public DataChart  
		{
		protected:
			/// Filename of the output file
			std::string m_strOutputFilename;

			/// The file stream
			std::ofstream oStream;

		public:
			FileChart();
			virtual ~FileChart();
			
			static FileChart *CastToDerived(AnimatBase *lpBase) {return static_cast<FileChart*>(lpBase);}

			virtual std::string Type();

			virtual std::string OutputFilename();
			virtual void OutputFilename(std::string strVal);

			virtual void ResetSimulation();
			virtual void Initialize();
			virtual void Deactivate();
			virtual void Load(CStdXml &oXml);
			virtual void SaveOutput();
		};

	}			//Charting
}				//AnimatSim
