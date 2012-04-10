/**
\file	Organism.h

\brief	Declares the organism class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		/**
		\brief	A dynamic organism that is controlled by a neural network.
		
		\details An organism is a type of Structure. The difference between 
			an organism and a structure is the same as that between a 
			crayfish and a rock. A crayfish is an object that thinks and 
			can move on its own. To do this it uses its brain to control
			the movements of its muscles. So unlike a structure, an organism
			contains a neural network that is attached to the motor neurons
			to control the movements of the limbs, and to sensory neurons to
			detect signals from the environment. This allows it to produce 
			independent behavior.

		\author	dcofer
		\date	2/25/2011
		**/
		class ANIMAT_PORT Organism : public Structure
		{
		protected:
			/// The pointer to the nervous system
			NervousSystem *m_lpNervousSystem;

			/// Tells if the organism is dead or not
			BOOL m_bDead;

		public:
			Organism();
			virtual ~Organism();

			virtual BOOL IsDead();

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

#pragma region DataAccesMethods
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

			virtual void Initialize();
			virtual void StepNeuralEngine();
			virtual void ResetSimulation();
			virtual void Kill(BOOL bState = TRUE);
			virtual void MinTimeStep(float &fltMin);

			virtual void Load(CStdXml &oXml);
			virtual AnimatSim::Behavior::NervousSystem *NervousSystem();
		};

	}			// Environment
}				//AnimatSim
