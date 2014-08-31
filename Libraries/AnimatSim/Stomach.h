/**
\file	C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatSim\Stomach.h

\brief	Declares the stomach class. 
**/
#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/**
			\brief	The Stomach object is responsible for holding food energy.

			\details The Mouth takes food from a food source and places its energy content (calories)
			into the stomach. At each time step the energy content of the stomach is decremented by 
			a user specified amount. If the energy content reaches zero then the organism is killed.
			There can only be <b>ONE</b>
			
			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT Stomach : public RigidBody   
			{
			protected:
				/// The maximum energy level that the stomach can hold. Even if the mouth tries to put more
				/// energy into it it will not be allowed to exceed this level. 
				float m_fltMaxEnergyLevel;

				/// The starting energy level
				float m_fltInitEnergyLevel;

				/// The current energy level
				float m_fltEnergyLevel;

				/// The current consumption rate. This is calculated by adding m_fltAdapterConsumptionRate and m_fltBaseConsumptionRate
				float m_fltConsumptionRate;

				/// The adapter consumption rate. This is set by any adapters that are connected to the stomach. This allows
				/// neural systems to change the rate of consumption.
				float m_fltAdapterConsumptionRate;

				/// The base consumption rate. This is the standard, constant rate of consumption.
				float m_fltBaseConsumptionRate;

				/// The consumption for the current simulation step. This is the consumption rate times the time step size.
				float m_fltConsumptionForStep;

				/// If this is true then if the energy level reaches zero then the organism is killed.
				bool m_bKillOrganism;

				/// Set to true if the organism is killed.
				bool m_bKilled;

				/// Used to report in GetDataPointer if the organism was killed.
				float m_fltReportAlive;

			public:
				Stomach();
				virtual ~Stomach();
						
				static Stomach *CastToDerived(AnimatBase *lpBase) {return static_cast<Stomach*>(lpBase);}

				virtual float EnergyLevel();
				virtual void EnergyLevel(float fltVal);
				virtual void AddEnergy(float fltVal);

				virtual float ConsumptionRate();
				virtual void ConsumptionRate(float fltVal);

				virtual float BaseConsumptionRate();
				virtual void BaseConsumptionRate(float fltVal);

				virtual float MaxEnergyLevel();
				virtual void MaxEnergyLevel(float fltVal);

				virtual bool KillOrganism();
				virtual void KillOrganism(bool bVal);

				//Stomach parts are never static joints.
				virtual bool HasStaticJoint() {return false;};

				virtual void CreateParts();

				//Node Overrides
				virtual void AddExternalNodeInput(int iTargetDataType, float fltInput);
				virtual float *GetDataPointer(const std::string &strDataType);
				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void StepSimulation();
				virtual void ResetSimulation();
				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
