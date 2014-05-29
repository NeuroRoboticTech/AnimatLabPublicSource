/**
\file	RPRO.h

\brief	Declares the relative position, relative orientation joint class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			/**
			\brief	A RPRO (relative position, relative orientation) type of joint.
			
			\details This joint restricts movement in all 6 degrees of freedom. It is being added primarily
			to allow direct conversion of old animatlab projects to new ones. There were to many problems with
			trying to get old static (RPRO) joints to map to new static joints, so I am mapping them to RPRO joints.
			However, you should probably use the new static joints instead of RPRO.

			\author	dcofer
			\date	3/24/2011
			**/
			class ANIMAT_PORT RPRO : public Joint    
			{
			protected:

			public:
				RPRO();
				virtual ~RPRO();
						
				static RPRO *CastToDerived(AnimatBase *lpBase) {return static_cast<RPRO*>(lpBase);}

				float CylinderRadius();
				float CylinderHeight();
				float BallRadius();

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
