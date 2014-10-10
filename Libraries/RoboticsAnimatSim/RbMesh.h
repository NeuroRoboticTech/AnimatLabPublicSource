
#pragma once


namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ROBOTICS_PORT RbMesh : public AnimatSim::Environment::Bodies::Mesh, public RbRigidBody
			{
			protected:

			public:
				RbMesh();
				virtual ~RbMesh();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
