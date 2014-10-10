/**
\file	Spring.h

\brief	Declares the spring class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/**
			\brief	Spring body part type. 
			
			\details A spring can be connected between two attachment points. The user can
			control the stiffness and damping, and natural length.

			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT Spring : public LineBase   
			{
			protected:
				///Keeps track of the initial state of the enabled flag.
				bool m_bInitEnabled;

				/// The natural length of the spring. 
				float m_fltNaturalLength;

				/// The unscaled natural length. This is used to calcuate the tension and displacement
				float m_fltNaturalLengthNotScaled;

				/// The stiffness of the spring
				float m_fltStiffness;

				/// The unscaled stiffness. This is used to calcuate the energy
				float m_fltStiffnessNotScaled;

				/// The damping of the spring
				float m_fltDamping;

				/// The damping of the spring
				float m_fltDampingNotScaled;

				/// The current displacement of the spring from its natural length
				float m_fltDisplacement;

				/// The current force being applied by the spring
				float m_fltTension;

				/// The current energy contained in the spring
				float m_fltEnergy;

                ///The velocity of the spring length change
                float m_fltVelocity;

                /// The rolling average velocity over the last 5 steps
                float m_fltAvgVelocity;

                /// circular array for calculating the rolling average of the velocity.
                CStdCircularArray<float> m_aryVelocityAvg;

                //Tension derived only from the stifness portion of the equation.
                float m_fltStiffnessTension;

                //Tension derived only from the damping portion of the equation.
                float m_fltDampingTension;

                /**
				\brief	Calculates the tension. 
				
				\author	dcofer
				\date	3/10/2011
				**/
				virtual void CalculateTension();

                /**
                 \brief Clears the velocity average circular queue.
                
                 \author    David Cofer
                 \date  2/2/2014
                 */
                virtual void ClearVelocityAverage();

			public:
				Spring();
				virtual ~Spring();
						
				static Spring *CastToDerived(AnimatBase *lpBase) {return static_cast<Spring*>(lpBase);}

				virtual bool InitEnabled();

				/**
				\brief	Gets the natural length of the spring. 
				
				\author	dcofer
				\date	3/4/2011
				
				\return	natural length. 
				**/
				virtual float NaturalLength();

				/**
				\brief	Sets the natural length of the spring. 
				
				\author	dcofer
				\date	3/4/2011
				
				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void NaturalLength(float fltVal, bool bUseScaling = true);
				
				/**
				\brief	Gets the stiffness of the spring. 
				
				\author	dcofer
				\date	3/4/2011
				
				\return	stiffness. 
				**/
				virtual float Stiffness();

				/**
				\brief	Sets the stiffness of the spring. 
				
				\author	dcofer
				\date	3/4/2011
				
				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void Stiffness(float fltVal, bool bUseScaling = true);

				/**
				\brief	Gets the damping of the spring. 
				
				\author	dcofer
				\date	3/4/2011
				
				\return	damping. 
				**/
				virtual float Damping();

				/**
				\brief	Sets the damping of the spring. 
				
				\author	dcofer
				\date	3/4/2011
				
				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void Damping(float fltVal, bool bUseScaling = true);

				virtual float Displacement();
				virtual float Tension();
				virtual float Energy();
				virtual float Velocity();

				virtual void ResetSimulation();
				virtual void CreateParts();
				virtual float *GetDataPointer(const std::string &strDataType);
				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void AddExternalNodeInput(int iTargetDataType, float fltInput);
				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
