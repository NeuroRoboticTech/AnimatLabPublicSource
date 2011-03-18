/**
\file	ArrayChart.h

\brief	Declares the array chart class.
**/

#pragma once

namespace AnimatSim
{
	namespace Charting
	{
		/**
		\brief	Array chart. 
		
		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT ArrayChart : public DataChart  
		{
		protected:
			/// Size of the 3-D array of data
			CStdFPoint m_vArraySize;

		public:
			ArrayChart();
			virtual ~ArrayChart();

			virtual string Type();
			virtual void CurrentRow(long iVal);

			virtual void Initialize();
			virtual void ReInitialize();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Charting
}				//AnimatSim
