/**
\file	Mouth.h

\brief	Declares the mouth class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	The Mouth part type.

			\details When the mouth part is within a user specified proximity to a food source it can 
			be stimulated to eat from that food source. It then takes food energy from the source and 
			transfers it to the stomach of the organism. Only one mouth can exist for an organism.
			
			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT Mouth : public Sensor   
			{
			protected:
				/// The pointer to stomach for this organism
				Stomach *m_lpStomach;

				/// Identifier for the stomach
				std::string m_strStomachID;

				/// The eating rate. This is determined by neural input to this object. A neuron is connected to
				/// this item through an adapter. The input value that comes in from the adapter via AddExternalNodeInput
				/// controls the eating rate. 
				float m_fltEatingRate;

				/// Any food source that is further away than this minimum radius will not be available for eating.
				float m_fltMinFoodRadius;

				/// The current distance between the food and the mouth.
				float m_fltFoodDistance;

				virtual void SetStomachPointer(std::string strID);

			public:
				Mouth();
				virtual ~Mouth();

				static Mouth *CastToDerived(AnimatBase *lpBase) {return static_cast<Mouth*>(lpBase);}

				virtual float EatingRate();

				/**
				\brief	Gets the minium radius where food can be eaten. 

				\author	dcofer
				\date	3/4/2011

				\return	the radius. 
				**/
				virtual float MinFoodRadius();

				/**
				\brief	Sets the minimum radius where food can be eaten. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void MinFoodRadius(float fltVal, bool bUseScaling = true);

				virtual void StomachID(std::string strID);
				virtual std::string StomachID();

				//Node Overrides
				virtual void Initialize();
				virtual void AddExternalNodeInput(int iTargetDataType, float fltInput);
				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual float *GetDataPointer(const std::string &strDataType);
				virtual void StepSimulation();
				virtual void ResetSimulation();
				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
