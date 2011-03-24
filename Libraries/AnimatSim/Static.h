/**
\file	C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatSim\Static.h

\brief	Declares the static class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			/**
			\brief	A static type of joint that can not move.

			\details This type of joint is constrained so that it can not move at all
			relative to its parent. It can not rotate or translate. In essence
			when you use this joint you are making these two rigid bodies behave
			as if they were one part. When the parent part moves the same motion
			will be applied to the child part.
			
			\author	dcofer
			\date	3/24/2011
			**/
			class ANIMAT_PORT Static : public Joint  
			{
			public:
				Static();
				virtual ~Static();

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
