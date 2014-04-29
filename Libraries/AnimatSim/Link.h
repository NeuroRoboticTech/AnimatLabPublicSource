/**
\file	Link.h

\brief	Declares the link base class. 
**/

#pragma once

namespace AnimatSim
{
	/**
	\brief	Base class for all link type objects. This is typically used for synapses. 
	
	\author	dcofer
	\date	3/16/2011
	**/
	class ANIMAT_PORT Link : public AnimatBase 
	{
	protected:
		/// The pointer to this link's organism
		Organism *m_lpOrganism;

		///Determines if this Link is enabled. This will only have any effect if this Link can be disabled.
		///The majority of Links, like rigid bodies, can not be disabled.
		bool m_bEnabled;

		///This is for reporting purposes.
		float m_fltEnabled;

		virtual void UpdateData();

	public:
		Link();
		virtual ~Link();

		virtual bool Enabled();
		virtual void Enabled(bool bValue);


#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual void VerifySystemPointers();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion
	};

}				//AnimatSim
