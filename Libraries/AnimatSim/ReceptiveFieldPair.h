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
		protected:
			/// Identifier for the field to use with this field pair.
			string m_strFieldID;
			/// GUID ID of the  target neuron where current will be injected.
			string m_strTargetNodeID;

			/// Pointer to the ReceptiveField associated with this pairing.
			ReceptiveField *m_lpField;
		public:
			ReceptiveFieldPair();
			virtual ~ReceptiveFieldPair();

			virtual void FieldID(string strID);
			virtual string FieldID();

			virtual void TargetNodeID(string strID);
			virtual string TargetNodeID();

			virtual ReceptiveField *Field();

			virtual void Initialize();
			virtual void StepSimulation();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
