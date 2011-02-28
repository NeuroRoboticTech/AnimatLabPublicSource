#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		/**
		\namespace	AnimatSim::Environment::Bodies

		\brief	Contains all of the different body parts that can be used in a structure. 
		**/
		namespace Bodies
		{

			/*! \brief 
				A box type of rigid body.
				 
				\remarks
				This is a box type of rigid body. You can specify the dimensions in 
				the x, y, and z directions for both the collision box and for the 
				graphic box that will acutally be displayed. This is useful because 
				you might want to graphic box to be bigger than the collision box.
				Typically you will want to remove collision detection between any
				parts that are connected by a joint. But by making the graphics box
				larger than the collision box it allows you to make it appear that 
				two rigid bodies overlap, without them actually coming into contact.

				\sa
				Body, Joint, CAlBox, CAlPlane, CAlCylinder, 
				CAlCone, CAlMuscle, CAlAttachment, CAlSphere     
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Box : public RigidBody
			{
			protected:
				///The dimension vector for the graphic box.
				float m_fltLength;
				float m_fltWidth;
				float m_fltHeight;

			public:
				//This is a test comment.
				Box();
				virtual ~Box();

				virtual float Length() {return m_fltLength;};
				virtual void Length(float fltVal, BOOL bUseScaling = TRUE);

				virtual float Width() {return m_fltWidth;};
				virtual void Width(float fltVal, BOOL bUseScaling = TRUE);

				virtual float Height() {return m_fltHeight;};
				virtual void Height(float fltVal, BOOL bUseScaling = TRUE);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
