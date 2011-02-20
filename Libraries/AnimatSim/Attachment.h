// Attachment.h: interface for the Attachment class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALMUSCLEATTACHMENT_H__EBC36518_3B2A_4634_9AB6_474F65149FCF__INCLUDED_)
#define AFX_ALMUSCLEATTACHMENT_H__EBC36518_3B2A_4634_9AB6_474F65149FCF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/*! \brief 
				Specifies a point on a rigid body where a muscle is to be attached.
			   
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

			class ANIMAT_PORT Attachment : public Sensor  
			{
			protected:

			public:
				Attachment();
				virtual ~Attachment();
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALMUSCLEATTACHMENT_H__EBC36518_3B2A_4634_9AB6_474F65149FCF__INCLUDED_)
