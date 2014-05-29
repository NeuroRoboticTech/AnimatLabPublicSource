/**
\file	Attachment.h

\brief	Declares the attachment class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/*! \brief 
				
			   
				\remarks
				This type of part specifies the position of a muscle attachment point.
				All CAlMuscle objects must specify the ID for two of these types of 
				object to determine where the ends of the muscle connect to the 
				parent and child rigid bodies. 

				\sa
				Body, Joint, CAlBox, CAlPlane, CAlCylinder, 
				CAlCone, CAlMuscle, CAlAttachment, CAlSphere                                
				 
				\ingroup AnimatSim
			*/

			/**
			\brief	Specifies a point on a rigid body where a muscle is to be attached.
			
			\details  This type of part specifies the position of a muscle attachment point.
				All Muscle objects must specify the ID for two of these types of 
				object to determine where the ends of the muscle connect to the 
				parent and child rigid bodies. 

			\author	dcofer
			\date	5/15/2011
			**/
			class ANIMAT_PORT Attachment : public Sensor  
			{
			protected:

			public:
				Attachment();
				virtual ~Attachment();
										
				static Attachment *CastToDerived(AnimatBase *lpBase) {return static_cast<Attachment*>(lpBase);}
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
