/**
\file	ContactAdapter.h

\brief	Declares the contact adapter class.
**/

#pragma once

namespace AnimatSim
{
	namespace Adapters
	{
		/**
		\brief	Contact adapter that processes ReceptiveField contacts.

		\details There is only a single ContactAdapter that processes all ReceptiveFieldPairs for a single RigidBody object.
		It has a list of all ReceptiveFields for that body and loops through them to determine the stimulus that needs to 
		be applied to the associated neurons. 
		
		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT ContactAdapter : public Adapter 
		{
		protected:
			/// GUID ID of the source RigidBody.
			string m_strSourceBodyID;

			/// The name of the target NeuralModule.
			string m_strTargetModule;

			/// The array of ReceptiveFieldPair objects.
			CStdPtrArray<ReceptiveFieldPair> m_aryFieldPairs; 

			ReceptiveFieldPair *LoadFieldPair(CStdXml &oXml);

			virtual void ContactAdapter::AddFieldPair(string strXml, BOOL bDoNotInit);
			virtual void ContactAdapter::RemoveFieldPair(string strID, BOOL bThrowError = TRUE);
			virtual int ContactAdapter::FindFieldPairListPos(string strID, BOOL bThrowError = TRUE);

		public:
			ContactAdapter();
			virtual ~ContactAdapter();

			virtual string SourceBodyID();
			virtual void SourceBodyID(string strID);

			virtual string SourceModule();

			virtual string TargetModule();
			virtual void TargetModule(string strModule);
			
#pragma region DataAccesMethods

			virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
			virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);

#pragma endregion

			virtual void Initialize();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim
