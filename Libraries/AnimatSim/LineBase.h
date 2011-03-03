// LineBase.h: interface for the LineBase class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
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

			public:
				LineBase();
				virtual ~LineBase();

				virtual float Length() {return m_fltLength;};
				virtual float PrevLength() {return m_fltPrevLength;};

				virtual BOOL AllowMouseManipulation();

				CStdArray<Attachment *> *AttachmentPoints() {return &m_aryAttachmentPoints;};
				virtual float CalculateLength();

				virtual void CreateParts();
				virtual void CreateJoints();
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
