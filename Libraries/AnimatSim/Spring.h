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
				BOOL m_bInitEnabled;

				///A pointer to the primary attachment part.
				Attachment *m_lpPrimaryAttachment;

				///A pointer to the secondary attachment part.
				Attachment *m_lpSecondaryAttachment;

				/// The natural length of the spring. 
				float m_fltNaturalLength;

				/// The stiffness of the spring
				float m_fltStiffness;

				/// The damping of the spring
				float m_fltDamping;

				/// The current displacement of the spring from its natural length
				float m_fltDisplacement;

				/// The current force being applied by the spring
				float m_fltTension;

				/// The current energy contained in the spring
				float m_fltEnergy;

			public:
				Spring();
				virtual ~Spring();

				Attachment *PrimaryAttachment() {return m_lpPrimaryAttachment;};
				void PrimaryAttachment(Attachment *lpVal) {m_lpPrimaryAttachment = lpVal;};

				Attachment *SecondaryAttachment() {return m_lpSecondaryAttachment;};
				void SecondaryAttachment(Attachment *lpVal) {m_lpSecondaryAttachment = lpVal;};

				virtual BOOL InitEnabled();

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
				virtual void NaturalLength(float fltVal, BOOL bUseScaling = TRUE);
				
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
				virtual void Stiffness(float fltVal, BOOL bUseScaling = TRUE);

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
				virtual void Damping(float fltVal, BOOL bUseScaling = TRUE);
				
				virtual float Displacement();
				virtual float Tension();
				virtual float Energy();

				virtual void CreateParts();
				virtual void AddExternalNodeInput(float fltInput);
				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
