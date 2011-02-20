// NeuronData.h: interface for the NeuronData class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace [*PROJECT_NAME*]
{
	namespace DataColumns
	{

		class [*TAG_NAME*]_PORT NeuronData : public DataColumn    
		{
		protected:
			string m_strOrganismID;
			CStdIPoint m_oPosition;
			string m_strDataType;

		public:
			NeuronData();
			virtual ~NeuronData();

			virtual string DataType() {return m_strDataType;}
			virtual void DataType(string strType) {m_strDataType = strType;}

			virtual void Initialize(Simulator *lpSim);
			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			//DataColumns
}				//[*PROJECT_NAME*]
