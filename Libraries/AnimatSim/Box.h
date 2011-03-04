/**
\file	Box.h

\brief	Declares the box class. 
**/

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
				///The length dimension of the box.
				float m_fltLength;

				///The width dimension of the box.
				float m_fltWidth;

				///The height dimension of the box.
				float m_fltHeight;

			public:
				Box();
				virtual ~Box();

				/**
				\brief	Gets the length of the box. 

				\author	dcofer
				\date	3/4/2011

				\return	Length of the box. 
				**/
				virtual float Length();

				/**
				\brief	Sets the length of the box. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new length value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void Length(float fltVal, BOOL bUseScaling = TRUE);

				/**
				\brief	Gets the width of the box. 

				\author	dcofer
				\date	3/4/2011

				\return	Width of the box. 
				**/
				virtual float Width();

				/**
				\brief	Sets the width of the box. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new width value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void Width(float fltVal, BOOL bUseScaling = TRUE);

				/**
				\brief	Gets the height of the box. 

				\author	dcofer
				\date	3/4/2011

				\return	Height of the box. 
				**/
				virtual float Height();

				/**
				\brief	Sets the Height of the box. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new height value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/				
				virtual void Height(float fltVal, BOOL bUseScaling = TRUE);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
