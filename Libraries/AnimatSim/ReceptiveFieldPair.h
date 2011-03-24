/**
\file	ReceptiveFieldPair.h

\brief	Declares the receptive field pair class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Receptive field pair. 
		
		\author	dcofer
		\date	3/24/2011
		**/
		class ANIMAT_PORT ReceptiveFieldPair : public AnimatBase 
		{
		public:
			/// The vertex of the center of the receptive field to find for this pair.
			StdVector3 m_vVertex;

			/// GUID ID of the  target neuron where current will be injected.
			string m_strTargetNodeID;

			/// Pointer to the ReceptiveField associated with this pairing.
			ReceptiveField *m_lpField;

			ReceptiveFieldPair();
			virtual ~ReceptiveFieldPair();

			void Initialize();
			void StepSimulation();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
