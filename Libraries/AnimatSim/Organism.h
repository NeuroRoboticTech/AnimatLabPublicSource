// Organism.h: interface for the Organism class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALORGANISM_H__78FDAAA1_E9BE_4E8B_8732_D73DB94D8083__INCLUDED_)
#define AFX_ALORGANISM_H__78FDAAA1_E9BE_4E8B_8732_D73DB94D8083__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{

		/*! \brief 
			A dynamic organism that is controlled by a neural network.

			\remarks
			An organism is a type of Structure. The difference between 
			an organism and a structure is the same as that between a 
			crayfish and a rock. A crayfish is an object that thinks and 
			can move on its own. To do this it uses its brain to control
			the movements of its muscles. So unlike a structure, an organism
			contains a neural network that is attached to the motor neurons
			to control the movements of the limbs, and to sensory neurons to
			detect signals from the environment. This allows it to produce 
			independent behavior.


			\sa
			Structure, Organism, Body, Joint
			 
			\ingroup AnimatSim
		*/

		class ANIMAT_PORT Organism : public Structure
		{
		protected:
			NervousSystem *m_lpNervousSystem;
			BOOL m_bDead;

		public:
			Organism();
			virtual ~Organism();

			virtual BOOL IsDead() {return m_bDead;};
			virtual void Kill(Simulator *lpSim, BOOL bState = TRUE);

			virtual void Initialize(Simulator *lpSim);
			virtual void StepNeuralEngine(Simulator *lpSim);
			virtual void StepPhysicsEngine(Simulator *lpSim);

			virtual void ResetSimulation(Simulator *lpSim);

			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

#pragma region DataAccesMethods
			virtual float *GetDataPointer(string &strID, string &strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual AnimatSim::Behavior::NervousSystem *NervousSystem();
		};

	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALORGANISM_H__78FDAAA1_E9BE_4E8B_8732_D73DB94D8083__INCLUDED_)
