/**
\file	OdorType.h

\brief	Declares the odor type class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		class Odor;

		/**
		\brief	Odor type that can be emitted from a RigidBody and sensed by an OdorSensor. 

		\details OdorTypes can be defined for the entire simulation. A RigidBody can then emit
		one or more different OdorTypes. An OdorSensor can detect only a single OdorType, but you 
		could have multiple sensors that work together. The OdorType has a diffusion constant that 
		defines how fast the odors of that type will move from their source to a destination. 
		Each OdorType keeps a list of all Odor source objects in the environment.
		
		\author	dcofer
		\date	3/23/2011
		**/
		class ANIMAT_PORT OdorType : public AnimatBase 
		{
		protected:
				/// The diffusion constant that defines how fast odors of this type move through the environment.
				float m_fltDiffusionConstant;

				/// The array of odor sources of this type within the environment.
				CStdMap<std::string, Odor *> m_aryOdorSources;

		public:
			OdorType();
			virtual ~OdorType();

			virtual float DiffusionConstant();
			virtual void DiffusionConstant(float fltVal, bool bUseScaling = true);

			virtual Odor *FindOdorSource(std::string strOdorID, bool bThrowError = true);
			virtual void AddOdorSource(Odor *lpOdor);
			
			virtual float CalculateOdorValue(CStdFPoint &oSensorPos);
			
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
