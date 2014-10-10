/**
\file	DataChart.h

\brief	Declares the data chart class.
**/

#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Charting

	\brief	Namespace for objects related to collecting the data for charts. 
	**/
	namespace Charting
	{
		/**
		\brief	Data chart base class.

		\details This is the base class for all charting activity. It is derived from an ActivatedItem, so it has a start/end time where
		it is activated and then deactivated. The user specifies a series of DataColumn objects for the chart. Each DataColumn is responsible
		for collecting data for a single variable from somewhere in the system. You can control how often the data is collected. The data is
		stored in memory buffers. There are several derived types of Data charts. The FileChart saves the stored data points into a file upon
		deactivation, while the MemoryChart stores the data in-memory in a continuing basis.
		
		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT DataChart : public ActivatedItem  
		{
		protected:
			/// Full pathname of the project file
			std::string m_strProjectPath;

			/// Filename of the configuration file
			std::string m_strConfigFilename;

			/// true to set the start and end time. If false then the chart collects continuously.
			bool m_bSetStartEndTime;

			/// Tells what the time slice step interval to use when collecting data. This is 
			short m_iCollectInterval;

			/// Tells what the time slice step interval to use when collecting data. This is 
			float m_fltCollectInterval;

			/// The number of time slices where we will collect data
			long m_lCollectTimeWindow;

			/// The time duration where we will collect data
			float m_fltCollectTimeWindow;

			/// The array of datacolumns columns. This is a sorted map that is used to get columns based on their ID.
			// The columns added to this map are copies of the pointer. They are <b>not</b> deleted.
			CStdMap<std::string, DataColumn *> m_aryColumnsMap;

			/// The primary array of data columns. This array deletes the columns when destructed.
			CStdPtrArray<DataColumn> m_aryDataColumns;

			/// Buffer for time data points.
			float *m_aryTimeBuffer;

			/// Buffer for data variable points.
			float *m_aryDataBuffer;

			/// Number of data columns
			long m_lColumnCount;

			/// Number of rows in the buffer
			long m_lRowCount;

			/// The currently selected column
			int m_lCurrentCol;

			/// The currently selected row
			int m_lCurrentRow;

			virtual long CalculateChartColumnCount();
			DataColumn *LoadDataColumn(CStdXml &oXml);
			virtual DataColumn *FindColumn(std::string strID, int &iIndex, bool bThrowError);

		public:
			DataChart();
			virtual ~DataChart();
			
			static DataChart *CastToDerived(AnimatBase *lpBase) {return static_cast<DataChart*>(lpBase);}

			virtual std::string Type();

			virtual void StartTime(float fltVal, bool bReInit = true);
			virtual void EndTime(float fltVal, bool bReInit = true);

			virtual bool SetStartEndTime();
			virtual void SetStartEndTime(bool bVal);

			virtual long BufferSize();
			virtual long UsedBufferSize();

			virtual long BufferByteSize();
			virtual long UsedBufferByteSize();

			virtual float *TimeBuffer();
			virtual float *DataBuffer();

			virtual int CollectInterval();
			virtual void CollectInterval(int iVal, bool bReInit = true);
			virtual void CollectInterval(float fltVal, bool bReInit = true);

			virtual long CollectTimeWindow();
			virtual void CollectTimeWindow(long lVal, bool bReInit = true);
			virtual void CollectTimeWindow(float fltVal, bool bReInit = true);

			virtual std::string ProjectPath();
			virtual void ProjectPath(std::string strVal);

			virtual long ColumnCount();

			virtual long CurrentRow();
			virtual void CurrentRow(long iVal);

			virtual bool Lock();
			virtual void Unlock();

			virtual void AddData(int iColumn, int iRow, float fltVal);

			virtual void Load(std::string strProjectPath, std::string strConfigFile);
			virtual void Load(CStdXml &oXml);

			virtual void AddColumn(DataColumn *lpColumn);
			virtual void AddColumn(std::string strXml, bool bDoNotInit);
			virtual void RemoveColumn(std::string strID, bool bThrowError = true);
			virtual DataColumn *FindColumn(std::string strID, bool bThrowError = true);

#pragma region DataAccesMethods
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);
#pragma endregion

			virtual bool operator<(ActivatedItem *lpItem);
			virtual void Initialize();
			virtual void ReInitialize();
			virtual void ResetSimulation();
			virtual void StepSimulation();
		};

	}			//Charting
}				//AnimatSim
