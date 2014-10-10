/**
\file	DataChartMgr.h

\brief	Declares the data chart manager class.
**/

#pragma once

namespace AnimatSim
{
	namespace Charting
	{
		/**
		\brief	Manager for data charts. 

		\details This class is derived from ActivatedItemMgr. It is responisble for maintaining a set of data charts.
		Each chart can have its own start/end times. 
		
		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT DataChartMgr : public ActivatedItemMgr  
		{
		protected:

			DataChart *LoadDataChart(CStdXml &oXml);

		public:
			DataChartMgr();
			virtual ~DataChartMgr();
			
			static DataChartMgr *CastToDerived(AnimatBase *lpBase) {return static_cast<DataChartMgr*>(lpBase);}

			virtual bool AddDataChart(std::string strXml);
			virtual bool RemoveDataChart(std::string strID);

			virtual void AddDataColumn(std::string strChartKey, DataColumn *lpColumn);
			virtual void RemoveDataColumn(std::string strChartKey, std::string strColumnName, bool bThrowError = true);
			virtual DataColumn *FindDataColumn(std::string strChartKey, std::string strColumnName, bool bThrowError = true);

			virtual void Load(CStdXml &oXml);
		};

	}			//Charting
}				//AnimatSim
