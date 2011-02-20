// NervousSystem.h: interface for the Adapter class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_NERVOUS_SYSTEM_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_NERVOUS_SYSTEM_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

/*! \brief 
   xxxx.

   \remarks
   xxxx
		 
   \sa
	 xxx
	 
	 \ingroup AnimatSim
*/

namespace AnimatSim
{
	namespace Behavior
	{

		class ANIMAT_PORT NervousSystem : public AnimatBase 
		{
		protected:
			CStdPtrMap<string, NeuralModule> m_aryNeuralModules;
			CStdPtrArray<Adapter> m_aryAdapters;

			string m_strProjectPath;
			string m_strNeuralNetworkFile;

			NeuralModule *LoadNeuralModule(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			void AddNeuralModule(NeuralModule *lpModule);

			Adapter *LoadAdapter(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);

		public:
			NervousSystem();
			virtual ~NervousSystem();

			string ProjectPath();
			void ProjectPath(string strPath);

			string NeuralNetworkFile();
			void NeuralNetworkFile(string strFile);

			virtual NeuralModule *FindNeuralModule(string strModuleName, BOOL bThrowError = TRUE);
			virtual void AddNeuralModule(Simulator *lpSim, Structure *lpStructure, string strXml);
			virtual void RemoveNeuralModule(Simulator *lpSim, Structure *lpStructure, string strID);

			virtual void Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState = TRUE);
			virtual void ResetSimulation(Simulator *lpSim, Organism *lpOrganism);

			virtual void Initialize(Simulator *lpSim, Structure *lpStructure);
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);

			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

			virtual void Load(Simulator *lpSim, Structure *lpStructure, string strProjectPath, string strNeuralFile);
			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, string strProjectPath, string strNeuralFile) {};
			virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml) {};

		};

	}			//Behavior
}			//AnimatSim

#endif // !defined(AFX_NERVOUS_SYSTEM_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
