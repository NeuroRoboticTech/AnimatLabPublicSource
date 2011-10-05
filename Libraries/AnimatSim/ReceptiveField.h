/**
\file	ReceptiveField.h

\brief	Declares the receptive field class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Receptive field that generates current based on the amount of contact force, and how close it is to the center of the field.

		\details 
		
		\author	dcofer
		\date	3/24/2011
		**/
		class ANIMAT_PORT ReceptiveField : public AnimatBase 
		{
		public:
			/// The vertex of the center of the receptive field in global coordinates.
			StdVector3 m_vVertex;

			/// The accumulated current for this receptive field.
			float m_fltCurrent;

			ReceptiveField();
			ReceptiveField(float fltX, float fltY, float fltZ, float fltStim);
			virtual ~ReceptiveField();

			void SetVertex(CStdFPoint vPoint);

			BOOL operator<(ReceptiveField *lpItem);
			BOOL operator>(ReceptiveField *lpItem);
			BOOL operator==(ReceptiveField *lpItem);
			BOOL LessThanThan(float fltX, float fltY, float fltZ);
			BOOL GreaterThanThan(float fltX, float fltY, float fltZ);
			BOOL Equals(float fltX, float fltY, float fltZ);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
