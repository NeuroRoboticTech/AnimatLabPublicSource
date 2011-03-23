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
				///The unique Id for this OdorType. It is unique for each structure, 
				///but not across structures. So you could have two rigid bodies with the
				///same ID in two different organisms.
				string m_strID;  

				///The name for this body. 
				string m_strName;  

				/// The diffusion constant that defines how fast odors of this type move through the environment.
				float m_fltDiffusionConstant;

				/// The array of odor sources of this type within the environment.
				CStdMap<string, Odor *> m_aryOdorSources;

		public:
			OdorType();
			virtual ~OdorType();

			virtual float DiffusionConstant();
			virtual void DiffusionConstant(float fltVal, BOOL bUseScaling = TRUE);

			virtual Odor *FindOdorSource(string strOdorID, BOOL bThrowError = TRUE);
			virtual void AddOdorSource(Odor *lpOdor);
			
			virtual float CalculateOdorValue(CStdFPoint &oSensorPos);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
