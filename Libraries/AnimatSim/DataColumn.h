/**
\file	DataColumn.h

\brief	Declares the data column class.
**/

#pragma once

namespace AnimatSim
{
	namespace Charting
	{
		/**
		\brief	Data column class.

		\details The data column class is responsible for collecting data from an object within the simulation
		and then adding that to the data chart. You typically will not need to derive a new class from this to specialize
		the data columnn. It is capable of getting any object in the system using its ID, and then you can specify the DataType
		in a call to GetDataPointer for that object to get a pointer to variable you need to chart.
		
		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT DataColumn : public AnimatBase  
		{
		protected:
			/// GUID ID of the target object that contains the variable we will be collecting.
			std::string m_strTargetID;

			/// The Data type of the variable we will be collecting. This is passed into the GetDataPointer method of the object.
			std::string m_strDataType;

			/// Pointer to the parent DataChart.
			DataChart *m_lpChart;

			/// Pointer to the target object that contains the data variable we will be collecting.
			AnimatBase *m_lpTarget;

			/// Pointer to the data variable we are collecting.
			float *m_lpDataValue;

			/// Determines whether how many other tabs are added after this data is written to the file.
			int m_iAppendSpaces;

			/// true it this chart has been initialized
			bool m_bInitialized;

			///This index is used to determine where in the array buffer that the data is stored. This is direclty related to the
			/// index in the array that is used in the GUI to retrieve the data, so they must match or the data plotted in the GUI
			/// will be for a different variable.
			int m_iColumnIndex;

			///This index is only used for 3D Array type charts. For normal line charts it is simply -1.
			///However, if it is not -1 then that means we want to specify the column and row where we should add the data for this column
			int m_iRowIndex; 

		public:
			DataColumn();
			virtual ~DataColumn();

			virtual int ColumnCount();

			virtual void Name(std::string strValue);

			virtual std::string DataType();
			virtual void DataType(std::string strType);

			virtual std::string TargetID();
			virtual void TargetID(std::string strID);

			virtual int AppendSpaces();
			virtual void AppendSpaces(int iSpaces);

			virtual bool IsInitialized();
			virtual void IsInitialized(bool bVal);

			virtual int ColumnIndex();
			virtual void ColumnIndex(int iIndex);

			virtual int RowIndex();
			virtual void RowIndex(int iIndex);

			virtual float *DataValue();
			
			/**
			\brief	Sets the system pointers.

			\author	dcofer
			\date	3/18/2011

			\param [in,out]	lpSim	   	The pointer to a simulation. 
			\param [in,out]	lpStructure	The pointer to a structure. 
			\param [in,out]	lpModule   	The pointer to a NeuralModule. 
			\param [in,out]	lpNode	   	The pointer to a node. 
			\param [in,out]	lpChart	   	The pointer to the parent chart. 
			\param	bVerify			   	true to verify. 
			**/
			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, DataChart *lpChart, bool bVerify);
			virtual void VerifySystemPointers();
			virtual void Initialize();
			virtual void ReInitialize();
			virtual void StepSimulation();
			virtual bool operator<(DataColumn *lpColumn);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void Load(CStdXml &oXml);
			virtual void SaveColumnNames(std::ofstream &oStream);
		};

		bool ANIMAT_PORT LessThanDataColumnCompare(DataColumn *lpColumn1, DataColumn *lpColumn2);

	}			//Charting
}				//AnimatSim
