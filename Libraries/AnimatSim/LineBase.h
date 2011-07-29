/**
\file	LineBase.h

\brief	Declares the line base class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	Base class for Line body part types.

			\details This is the base class for all body part types that create lines to display in the simulation.
			Examples of this type of object are muscles and springs. Attachment points are specified by the user and 
			the muscle is drawn between these points.
			
			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT LineBase : public RigidBody  
			{
			protected:
				///Current length of the line.
				float m_fltLength;

				///Length of the line in the previous timestep
				float m_fltPrevLength;

				///The ID's of the attachment points for this muscle. This is used during the load/initialization process.
				CStdArray<string> m_aryAttachmentPointIDs;

				///A pointer to the primary attachment part.
				CStdArray<Attachment *> m_aryAttachmentPoints;

				virtual void LoadAttachments(CStdXml &oXml);
				virtual void InitializeAttachments();
				virtual void AttachedPartMovedOrRotated(string strID);

			public:
				LineBase();
				virtual ~LineBase();

				virtual float Length();
				virtual float PrevLength();

				virtual BOOL AllowMouseManipulation();
				virtual void Position(CStdFPoint &oPoint, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
				virtual void AbsolutePosition(CStdFPoint &oPoint);
				virtual void OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm);

				CStdArray<Attachment *> *AttachmentPoints();
				virtual void AttachmentPoints(string srXml);

				virtual void Resize();
				virtual float CalculateLength();

				virtual float *GetDataPointer(string strDataType);
				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				
				virtual void CreateParts();
				virtual void CreateJoints();
				virtual void Load(CStdXml &oXml);

			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
