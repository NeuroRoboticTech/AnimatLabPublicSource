// Synapse.h: interface for the Synapse class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace [*PROJECT_NAME*]
{
	namespace Synapses
	{

		class [*TAG_NAME*]_PORT Synapse : public CStdSerialize   
		{
		protected:
			BOOL m_bEnabled;
			unsigned char m_iFromX;
			unsigned char m_iFromY;
			unsigned char m_iFromZ;
			Neuron *m_lpNeuron;

		public:
			Synapse();
			virtual ~Synapse();

			virtual unsigned char Type()
			{return REGULAR_SYNAPSE;};

			BOOL Enabled();
			void Enabled(BOOL bVal);

			unsigned char FromX();
			void FromX(unsigned char iVal);

			unsigned char FromY();
			void FromY(unsigned char iVal);

			unsigned char FromZ();
			void FromZ(unsigned char iVal);

			virtual void Initialize(Simulator *lpSim, Organism *lpOrganism, TestNeuralModule *lpModule);
			virtual float *GetDataPointer(short iCompoundIndex, short iDataType);
			virtual float CalculateCurrent(); 

			virtual void Load(Simulator *lpSim, Structure *lpStructure, Neuron *lpNeuron, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, Neuron *lpNeuron, CStdXml &oXml);
		};

	}			//Synapses
}				//[*PROJECT_NAME*]
