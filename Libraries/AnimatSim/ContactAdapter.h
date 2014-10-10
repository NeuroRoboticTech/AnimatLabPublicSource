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
			std::string m_strSourceBodyID;

			/// The name of the target NeuralModule.
			std::string m_strTargetModule;

			/// The array of ReceptiveFieldPair objects.
			CStdPtrArray<ReceptiveFieldPair> m_aryFieldPairs; 

			ReceptiveFieldPair *LoadFieldPair(CStdXml &oXml);

			virtual void AddFieldPair(std::string strXml, bool bDoNotInit);
			virtual void RemoveFieldPair(std::string strID, bool bThrowError = true);
			virtual int FindFieldPairListPos(std::string strID, bool bThrowError = true);

		public:
			ContactAdapter();
			virtual ~ContactAdapter();
			
			static ContactAdapter *CastToDerived(AnimatBase *lpBase) {return static_cast<ContactAdapter*>(lpBase);}

			virtual std::string SourceBodyID();
			virtual void SourceBodyID(std::string strID);

			virtual std::string SourceModule();

			virtual std::string TargetModule();
			virtual void TargetModule(std::string strModule);
			
#pragma region DataAccesMethods

			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

#pragma endregion

			virtual void Initialize();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim
